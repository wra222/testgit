 //INVENTEC corporation (c)2011 all rights reserved. 
 //Description:NoneEPIAtoEPIA page
 //            
 //Update: 
 //Date        Name                  Reason 
 //==========  ===================== =====================================
 //2011-10-09  zhu lei               Create          
 //Known issues:
 //TODO:
 //编码完成，但数据库尚无相关可供测试数据。尚未调试，需整合阶段再行调试
using System;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using log4net;
using IMES.Route;


namespace IMES.Station.Implementation
{
    public class ForceEOQCImpl : MarshalByRefObject, IForceEOQC 
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType ProductSessionType = Session.SessionType.Product;

        #region IOQCInput members

        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>抽检结果: "EOQC", "OQC", or "SKIP"</returns>
        public bool InputProdId(string pdLine, string prodId, string editor, string stationId, string customer)
        {
            logger.Debug("(ForceEOQCImpl)InputProdId start, [pdLine]:" + pdLine
                + " [prodId]: " + prodId
                + " [editor]:" + editor
                + " [station]:" + stationId
                + " [customer]:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();            

            try
            {
                string sessionKey = prodId;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);
                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, ProductSessionType, editor, stationId, pdLine, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", ProductSessionType);

                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "NonEPIAToEPIA.xoml", string.Empty, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.CustSN, sessionKey);
                    currentSession.AddValue(Session.SessionKeys.Editor, editor);
                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();


                }
                else
                {
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
                return true;
            }
            catch (FisException e)
            {
                if (e.mErrcode == "SFC002")
                {
                    erpara.Add(prodId);
                    e = new FisException("CHK152", erpara);
                }
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            finally
            {
                logger.Debug("(ForceEOQCImpl)InputProdId end, [pdLine]:" + pdLine
                    + " [prodId]: " + prodId
                    + " [editor]:" + editor
                    + " [station]:" + stationId
                    + " [customer]:" + customer);
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

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, ProductSessionType);

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
