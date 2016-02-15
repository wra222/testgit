/*

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using System.Collections;
using log4net;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Station.Interface.CommonIntf;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.FisBOM;
using IMES.Route;
using IMES.Infrastructure.Extend;


namespace IMES.Station.Implementation
{
    public class OfflineCommonPartsLabelPrint : MarshalByRefObject, IOfflineCommonPartsLabelPrint
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        public ArrayList Print(string model, IList<PrintItem> printItems, string line, string editor, string station, string customer)
        {
            logger.Debug("(OfflineCommonPartsLabelPrint)InputCustomerSN start, model:" + model
                          + "editor:" + editor + "station:" + station + "customer:" + customer);

            try
            {
                string SPS = CheckModelInfo(model);
                if(string.IsNullOrEmpty(SPS))
               { throw new FisException("PAK085", new string[] { model, "MB" }); }
              
                ArrayList retList = new ArrayList();
                string sessionKey = model;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
              
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, station, line, customer);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(station, "OfflineCommonPartsLabelPrint.xoml", "", out wfName, out rlName);
                    //RouteManagementUtils.GetWorkflow(station, "104KPPrint.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.PrintItems, printItems);
                    currentSession.AddValue(Session.SessionKeys.PrintLogName, printItems[0].LabelType);
                    currentSession.AddValue(Session.SessionKeys.PrintLogBegNo,model);
                    currentSession.AddValue(Session.SessionKeys.PrintLogEndNo,model);
                    currentSession.AddValue(Session.SessionKeys.PrintLogDescr,"");
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }


                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
                //===============================================================================
                retList.Add(SPS);
                IList<PrintItem> pItem = (IList<PrintItem>)currentSession.GetValue(Session.SessionKeys.PrintItems);
                retList.Add(pItem);
                return retList;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);

                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }

        }


        private string CheckModelInfo(string model)
        {
            IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IList<IMES.FisObject.Common.Model.ModelInfo> lst = modelRepository.GetModelInfoByModelAndName(model, "MB");
             if (lst.Count > 0)
            { 
              return lst[0].Value.Trim();
            }
             else
             {return "";}
        }
        
    }
}
