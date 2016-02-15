using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
//using IMES.Station.Interface.CommonIntf;
using System.Workflow.Runtime;
using IMES.DataModel;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.PrintLog;
using IMES.Route;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.Station.Implementation
{

    /// <summary>
    /// </summary>
    public partial class OfflineLcdCtPrint : MarshalByRefObject, IOfflineLcdCtPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IRCTOLabelReprint Members

        public bool CheckModel(string model, string customer)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                logger.Debug("(OfflineLcdCtPrint)CheckModel start, model:" + model + " , customer=" + customer);
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
                if (!modelRepository.CheckExistModel(customer, model))
                {
                    erpara.Add(model);
                    ex = new FisException("CHK928", erpara); // Model:%1不存在
                    throw ex;
                }
                return true;
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
                logger.Debug("(OfflineLcdCtPrint)CheckModel end, model:" + model + " , customer=" + customer);
            }
        }

        public ArrayList Print(string model, string ct, string editor, string station, string customer, string pCode, IList<PrintItem> printItems)
        {
            ArrayList retrunValue = new ArrayList();
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {
                logger.Debug("(OfflineLcdCtPrint)Print start, model:" + model + " ct:" + ct + " editor:" + editor + " customerId:" + customer);

                if (!string.IsNullOrEmpty(model))
                {
                    bool existCT = false;
                    if (ct.Length >= 5)
                    {
                        IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                        IHierarchicalBOM curBOM = bomRepository.GetHierarchicalBOMByModel(model);
                        IList<IBOMNode> bomNodeLst = curBOM.Nodes; //curBOM.FirstLevelNodes

                        foreach (IBOMNode n in bomNodeLst)
                        {
                            IList<PartInfo> part_infos = n.Part.Attributes;
                            if (part_infos != null && part_infos.Count > 0)
                            {
                                foreach (PartInfo part_info in part_infos)
                                {
                                    if (part_info.InfoType.Equals("VendorCode"))
                                    {
                                        if (part_info.InfoValue.Equals(ct.Substring(0, 5)))
                                        {
                                            existCT = true;
                                            break;
                                        }
                                    }
                                }
                                if (existCT)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (!existCT)
                    {
                        erpara.Add(model);
                        ex = new FisException("CHK1031", erpara); // CT有誤，請輸入正確的CT !
                        throw ex;
                    }
                }

                string sessionKey = ct;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (Session == null)
                {
                    Session = new Session(sessionKey, Session.SessionType.Product, editor, station, "", customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();

                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", "");
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", Session.SessionType.Product);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "OfflineLcdCtPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    // PartNo
                    Session.AddValue(Session.SessionKeys.ModelName, model);
                    Session.AddValue(Session.SessionKeys.PrintItems, printItems);

                    Session.AddValue(Session.SessionKeys.PrintLogName, "Offline LCD CT");
                    Session.AddValue(Session.SessionKeys.PrintLogBegNo, ct);
                    Session.AddValue(Session.SessionKeys.PrintLogEndNo, ct);
                    Session.AddValue(Session.SessionKeys.PrintLogDescr, ct);
                    Session.AddValue(Session.SessionKeys.Reason, "");

                    Session.SetInstance(instance);

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


                IList<PrintItem> returnList = (IList<PrintItem>)Session.GetValue(Session.SessionKeys.PrintItems);
                retrunValue.Add(returnList);
                return retrunValue;
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
                logger.Debug("(OfflineLcdCtPrint)Print end, model:" + model + " ct:" + ct + " editor:" + editor + " customerId:" + customer);

            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            try
            {
                logger.Debug("OfflineLcdCtPrint(Cancel) start, sessionKey:" + sessionKey);
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.Product);
                if (currentSession != null)
                {
                    SessionManager.GetInstance.RemoveSession(currentSession);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("OfflineLcdCtPrint(Cancel) end, sessionKey:" + sessionKey);
            }

        }




        #endregion
    }
}
