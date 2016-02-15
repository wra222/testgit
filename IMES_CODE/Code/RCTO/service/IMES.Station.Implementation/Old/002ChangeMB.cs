using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.DataModel;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.PCA.MB;
using IMES.Station.Interface.CommonIntf;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBModel;
using IMES.Infrastructure.Repository.PCA;
using log4net;
using IMES.Route;
using IMES.Infrastructure.Extend;



namespace IMES.Station.Implementation
{
    public class ChangeMB : MarshalByRefObject,IChangeMB
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Session.SessionType theType = Session.SessionType.Common;

        public IList<PrintItem> ReplaceMB(string oldMB, 
                                                                 string reason, 
                                                                 string editor, 
                                                                 string stationId, 
                                                                 string customerId,
                                                                out IList<IMES.DataModel.MBInfo> startProdIdAndEndProdId, 
                                                                IList<PrintItem> printItems)
        {
            logger.Debug("(ChangeMB)ChangeMB start, oldMB:" + oldMB + " reason:" + reason + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();
            //IList<PrintItem> printList=null;
            try
            {

                //得到old MB的相关信息
                //MBRepository mbRepository = new MBRepository();
                //IMES.DataModel.MBInfo mbInfo = new IMES.DataModel.MBInfo();
                //mbInfo = mbRepository.GetMBInfo(oldMB);


                IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMES.FisObject.PCA.MB.IMB>();
                             
                Nullable<IMES.DataModel.MBInfo> NullAblembInfo=mbRepository.GetMBInfo(oldMB);                 
                               
                if (NullAblembInfo == null || NullAblembInfo.Value.id ==null)
                {
                    var ex1 = new FisException("SFC001", new string[] { oldMB });
                    throw ex1;
                }
                IMES.DataModel.MBInfo mbInfo = NullAblembInfo.Value;
                string sessionKey = mbInfo.SMTMOId;// oldMB;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, theType);
                if (Session == null)
                {
                   
                    Session = new Session(sessionKey, theType, editor, stationId, "", customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", mbInfo.line);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", theType);
                                

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "002MBLabelPrint.xoml", "002MBLabelPrint.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);



                    //  WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("002ChangeMB.xoml", null, wfArguments);

                    Session.AddValue(Session.SessionKeys.IsNextMonth, false);
                    Session.AddValue(Session.SessionKeys.Qty, 1);
                    Session.AddValue(Session.SessionKeys.DateCode, mbInfo.dateCode);
                    Session.AddValue(Session.SessionKeys.motherOrChild, "0");
                    Session.AddValue(Session.SessionKeys.MBMONO, mbInfo.SMTMOId);
                    Session.AddValue(Session.SessionKeys.ModelName, mbInfo._111LevelId);                    
                    Session.AddValue(ExtendSession.SessionKeys.IsReplaceMB , true);
                    Session.AddValue(Session.SessionKeys.OldMB, oldMB);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    Session.AddValue(ExtendSession.SessionKeys.ChangMBReason, reason);


                                        var mbModelRepository = (IMBModelRepository)RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                    MBModel model = (MBModel)mbModelRepository.Find(mbInfo._111LevelId);
                   
                   
                    Session.AddValue(Session.SessionKeys.MBCode, model.Mbcode);
                    Session.AddValue(Session.SessionKeys.MBType, model.Type);
                   
                    //2011/06/07 Jiali add
                    //Begin
                    //Session.AddValue(Session.SessionKeys.MBType, model.MultiQty);
                    Char[] OldMBtoChar = oldMB.ToCharArray(); 
                       

                    //last 1 line is exist before
                    Session.SetInstance(instance);

                    if (model.MultiQty != 1 && OldMBtoChar[5] == '0')
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK163", erpara);
                        throw ex;
                    }

                    //end

                 

                   

                    if (!SessionManager.GetInstance.AddSession(Session))
                    {
                        Session.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    Session.WorkflowInstance.Start();

                    Session.SetHostWaitOne();
                    
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (Session.Exception != null)
                {
                    if (Session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        Session.ResumeWorkFlow();
                    }
                    throw Session.Exception;
                }

                var MBNOList = (IList<string>)Session.GetValue(Session.SessionKeys.MBNOList);


                //IMES.DataModel.MBInfo info = new IMES.DataModel.MBInfo();
                // info.id = MBNOList[0];
                // info.line = mbInfo.MBStatus.Line;
                // info.SMTMOId = mbInfo.SMTMO;
                // info._111LevelId = mbInfo.Model;
                // info.custVersion = mbInfo.CUSTVER;
                // info.dateCode = mbInfo.DateCode;
                // info.ecr = mbInfo.ECR;
                // info.iecVersion = mbInfo.IECVER;
                // info.family = mbInfo.Family;
                 mbInfo.id = MBNOList[0];
                //startProdIdAndEndProdId = MBNOList;
                startProdIdAndEndProdId = new List<IMES.DataModel.MBInfo>();
                startProdIdAndEndProdId.Add(mbInfo);

                IList<PrintItem> returnList = this.getPrintList(Session); 
                return returnList;


            }
            catch (FisException e)
            {
                throw e;

            }
            catch (Exception e)
            {
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(ChangeMB)ChangeMB start, oldMB:" + oldMB + " reason:" + reason + " editor:" + editor + " stationId:" + stationId + " customerId:" + customerId);

            }
        }

        private IList<PrintItem> getPrintList(Session session)
        {
            try
            {
                object printObject = session.GetValue(Session.SessionKeys.PrintItems);
                session.RemoveValue(Session.SessionKeys.PrintItems);
                if (printObject == null)
                {
                    return null;
                }
                IList<PrintItem> printItems = (IList<PrintItem>)printObject;

                return printItems;
            }
            catch (FisException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
