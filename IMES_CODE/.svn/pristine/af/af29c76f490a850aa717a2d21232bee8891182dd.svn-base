/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Implementation for KB CT Check Page
 * UI:CI-MES12-SPEC-FA-UI KB CT Check.docx –2012/6/12 
 * UC:CI-MES12-SPEC-FA-UC KB CT Check.docx –2012/6/12            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-6-12   Jessica Liu           (Reference Ebook SourceCode) Create
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
using IMES.FisObject.Common.FisBOM; 
using IMES.FisObject.Common.Part;   
//using System.Linq;
using IMES.Infrastructure.Extend;

namespace IMES.Station.Implementation
{

    public class KBCTCheck : MarshalByRefObject, IKBCTCheck
    {
        private const Session.SessionType TheType = Session.SessionType.Product;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IKBCTCheck Members


        /// <summary>
        /// ProdId相关判断处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void CheckProdId(string prodid, string pdLine, string editor, string stationId, string customerId)
        {
            CheckProdId(prodid, pdLine, editor, stationId, customerId, false);
        }

        public void CheckProdIdOffline(string prodid, string pdLine, string editor, string stationId, string customerId)
        {
            CheckProdId(prodid, pdLine, editor, stationId, customerId, true);
        }

        private void CheckProdId(string prodid, string pdLine, string editor, string stationId, string customerId, bool isOffline)
		{
            logger.Debug("(KBCTCheck)CheckProdId start, inputProdid:" + prodid + " pdline:" + pdLine + " editor:" + editor + " customerId:" + customerId);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                if (Session == null)
                {
                    Session = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);
                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", Session);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);

                    string wfName, rlName;

                    if (!isOffline)
                        RouteManagementUtils.GetWorkflow(stationId, "KBCTCheck.xoml", null, out wfName, out rlName);
                    else
                        RouteManagementUtils.GetWorkflow(stationId, "KBCTCheckOffline.xoml", "KBCTCheckOffline.rules", out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

                    Session.AddValue(Session.SessionKeys.ProductIDOrCustSN, prodid);

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
    
                return;
            }
            catch (FisException e)
            {
				logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
				logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(KBCTCheck)CheckProdId end, inputProdid:" + prodid + " pdline:" + pdLine + " editor:" + editor + " customerId:" + customerId);
            }
            
        }


        /// <summary>
        /// Check及保存处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="kbct">KB CT</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void CheckAndSave(string prodid, string kbct, string pdLine, string editor, string stationId, string customerId)
        {
            CheckAndSave(prodid, kbct, pdLine, editor, stationId, customerId, false);
        }

        public string CheckAndSaveOffline(string prodid, string kbct, string pdLine, string editor, string stationId, string customerId)
        {
            return CheckAndSave(prodid, kbct, pdLine, editor, stationId, customerId, true);
        }

        public string CheckAndSave(string prodid, string kbct, string pdLine, string editor, string stationId, string customerId, bool isOffline)
        {
            logger.Debug("(KBCTCheck)checkAndSave start, inputProdid:" + prodid + " kbct:" + kbct+ " pdline:" + pdLine + " editor:" + editor + " customerId:" + customerId);

            string notMatch = "";
			FisException ex;
			List<string> erpara = new List<string>();
			
            try
            {
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue("KBCT", kbct);
                    
                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();		
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

                if (isOffline)
                    notMatch = sessionInfo.GetValue("NotMatchKBCT") as string;

                return notMatch;
            }
            catch (FisException e)
            {
				logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
				logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(KBCTCheck)checkAndSave end, inputProdid:" + prodid + " kbct:" + kbct + " pdline:" + pdLine + " editor:" + editor + " customerId:" + customerId);
            }
        }


        /// <summary>
        /// Check及保存处理
        /// </summary>
        /// <param name="prodid">Product ID</param>
        /// <param name="kbct">KB CT</param>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        public void SaveDefect(string prodid, string defect)
        {
            logger.Debug("(KBCTCheck)SaveDefect start, inputProdid:" + prodid + " defect:" + defect);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = prodid;
                Session Session = SessionManager.GetInstance.GetSession(sessionKey, TheType);
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(ExtendSession.SessionKeys.DefectStation, sessionInfo.GetValue("Station") as string);

                    IList<string> defectList = new List<string>();
                    defectList.Add(defect);
                    sessionInfo.AddValue(Session.SessionKeys.DefectList, defectList);

                    sessionInfo.Exception = null;
                    sessionInfo.SwitchToWorkFlow();
                }

                if (sessionInfo.Exception != null)
                {
                    if (sessionInfo.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        sessionInfo.ResumeWorkFlow();
                    }

                    throw sessionInfo.Exception;
                }

                return;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(KBCTCheck)SaveDefect end, inputProdid:" + prodid + " defect:" + defect);
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
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

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
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

         #endregion
    }
}
