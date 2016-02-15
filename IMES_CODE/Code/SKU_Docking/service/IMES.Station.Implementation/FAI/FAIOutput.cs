/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* Known issues:
* TODO：
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Runtime;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Route;
using IMES.Station.Interface.StationIntf;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.TestLog;
//using System.Linq;
using IMES.Infrastructure.Extend;

namespace IMES.Station.Implementation
{

    public class FAIOutput : MarshalByRefObject, IFAIOutput
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        /// <summary>
        /// ProdId相关判断处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public ArrayList CheckProdId(string custsn, string pdLine, string editor, string stationId, string customerId)
		{
            FisException ex;
            List<string> erpara = new List<string>();

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5}, )", methodName, pdLine, editor, stationId, customerId, custsn);
            try
            {
                string sessionKey = custsn;
                Session CurrentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (CurrentSession == null)
                {
                    CurrentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", CurrentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    RouteManagementUtils.GetWorkflow(stationId, "FAIOutput.xoml", "FAIOutput.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    CurrentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, custsn);

                    CurrentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(CurrentSession))
                    {
                        CurrentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    CurrentSession.WorkflowInstance.Start();
                    CurrentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK020", erpara);
                    throw ex;
                }

                //check workflow exception
                if (CurrentSession.Exception != null)
                {
                    if (CurrentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        CurrentSession.ResumeWorkFlow();
                    }
                    throw CurrentSession.Exception;
                }

                IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

                string stationDesc = "";
                IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                IStation station = itemRepository.Find(product.Status.StationId);
                if (null == station)
                {
                    //throw new FisException("Can Not Found Station!");
                    stationDesc = product.Status.StationId;
                }
                else
                {
                    stationDesc = product.Status.StationId + " - " + (station.Name == null ? station.Name : station.Descr);
                }

                string FAIOutputReturnStation = product.GetAttributeValue("FAIOutputReturnStation");
                CurrentSession.AddValue("FAIOutputReturnStation", FAIOutputReturnStation);

                ArrayList lst = new ArrayList();
                lst.Add(product.CUSTSN);
                lst.Add(product.ProId);
                lst.Add(product.Model);
                lst.Add(stationDesc);

                return lst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                if (e.mErrcode.ToString().Trim() == "CQCHK50011")
                {
                    // 非FAI檢驗機器，請確認！
                    ex = new FisException("CQCHK50016", erpara);
                    throw ex;
                }
                else
                {
                    throw e;
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5}, )", methodName, pdLine, editor, stationId, customerId, custsn);
            }
            
        }


        /// <summary>
        /// Save
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="kbct">KB CT</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public ArrayList Save(string custsn, IList<string> defectList, string pdLine, string editor, string stationId, string customerId)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList lstReturn = new ArrayList();

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5},  )", methodName, pdLine, editor, stationId, customerId, custsn);
            try
            {
                string sessionKey = custsn;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }

                sessionInfo.AddValue(ExtendSession.SessionKeys.TestLogActionName, "FAIOut");
                if (null != defectList && defectList.Count > 0)
                {
                    sessionInfo.AddValue(Session.SessionKeys.DefectList, defectList);
                    sessionInfo.AddValue(ExtendSession.SessionKeys.TestLogErrorCode, defectList[0]);
                }

                

                sessionInfo.Exception = null;
                sessionInfo.SwitchToWorkFlow();

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

                string FAIOutputReturnStationId = "";
                string FAIOutputReturnStationName = "";
                if (null != defectList && defectList.Count > 0)
                {
                    FAIOutputReturnStationId = sessionInfo.GetValue("FAIOutputReturnStation") as string;

                    IStationRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository>();
                    IStation station = itemRepository.Find(FAIOutputReturnStationId);
                    if (null != station)
                    {
                        FAIOutputReturnStationName = (station.Name == null ? station.Name : station.Descr);
                    }
                }

                lstReturn.Add(FAIOutputReturnStationId);
                lstReturn.Add(FAIOutputReturnStationName);

                return lstReturn;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5},  )", methodName, pdLine, editor, stationId, customerId, custsn);
            }

        }


        /// <summary>
        /// CheckDefect
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="kbct">KB CT</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public ArrayList CheckDefect(string custsn, string defect, string pdLine, string editor, string stationId, string customerId)
        {
            FisException ex;
            List<string> erpara = new List<string>();
            ArrayList lstReturn = new ArrayList();

            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5}, defect={6} )", methodName, pdLine, editor, stationId, customerId, custsn, defect);
            try
            {
                string sessionKey = custsn;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }

                IProduct product = (IProduct)sessionInfo.GetValue(Session.SessionKeys.Product);
                if (null != product.TestLog)
                {
                    foreach (TestLog log in product.TestLog)
                    {
                        if (null != log.Defects)
                        {
                            foreach (TestLogDefect logD in log.Defects)
                            {
                                if (defect == logD.DefectCode)
                                {
                                    // Defect已经存在!
                                    throw new FisException("CQCHK50014", new string[] {  });
                                }
                            }
                        }
                    }
                }

                IDefectRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
                string strDefect = itemRepository.GetDefect("PRD", defect);
                if (string.IsNullOrEmpty(strDefect))
                {
                    // 请输入合法的Defect!!
                    throw new FisException("CHK908", new string[] { });
                }

                lstReturn.Add(strDefect);
                lstReturn.Add(itemRepository.TransToDesc(defect));

                return lstReturn;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}( pdline={1}, editor={2}, stationId={3}, customerId={4}, custsn={5}, defect={6} )", methodName, pdLine, editor, stationId, customerId, custsn, defect);
            }

        }
		
		
		/// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void Cancel(string sessionKey)
        {
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            logger.DebugFormat("BEGIN: {0}( sessionKey={1} )", methodName, sessionKey);
            try
            {
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

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
                throw;
            }
            finally
            {
                logger.DebugFormat("END: {0}( sessionKey={1} )", methodName, sessionKey);
            }
        }

    }
}
