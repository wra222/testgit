/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Runtime;
using IMES.Station.Interface.StationIntf;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.FisObject.PCA.MB;
using log4net;
using IMES.Route;

namespace IMES.Station.Implementation
{
    public class _007CombineCPU : MarshalByRefObject, IMBCombineCPU
    {

        #region IMBCombineCPU Members
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// input mbsn ,SFC 
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="MB_SNo"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        public void MBCombineInputMBSN(
   string pdLine,
   string MB_SNo,
   string editor, string stationId, string customerId)
        {
            logger.Debug("(_007CombineCPU)MBCombineInputMBSN start, "
                            + " [pdLine]:" + pdLine
                           + " [MB_SNo]: " + MB_SNo
                           + " [editor]:" + editor
                           + " [station]:" + stationId
                           + " [customer]:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = MB_SNo;
            Session.SessionType TheType = Session.SessionType.MB;

            try
            {
                Session CurrentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (CurrentSession == null)
                {
                    CurrentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", CurrentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    string wfName, rlName;
                    RouteManagementUtils.GetWorkflow(stationId, "007MBCombineCPU.xoml", null, out wfName, out rlName);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);

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
                logger.Debug("(_007CombineCPU)MBCombineInputMBSN start, "
                                + " [pdLine]:" + pdLine
                               + " [MB_SNo]: " + MB_SNo
                               + " [editor]:" + editor
                               + " [station]:" + stationId
                               + " [customer]:" + customerId);
            }
        }

        /// <summary>
        /// 1.1	UC-PCA-MCC-01 MB Combine CPU
        /// 建立主板和CPU Vender 的绑定关系
        /// </summary>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="CPUVendorSN">CPU Vendor SN</param>
        public string MBCombineCPU(string MB_SNo, string CPUVendorSN)
        {
            logger.Debug("(_007CombineCPU)MBCombineCPU start, "
                           + " [MB_SNo]: " + MB_SNo
                           + " [CPUVendorSN]:" + CPUVendorSN);
            Session.SessionType TheType = Session.SessionType.MB;
            FisException ex;
            List<string> erpara = new List<string>();
            try
            {

                string sessionKey = MB_SNo;
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.CPUVendorSn, CPUVendorSN);
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
                return "ok";

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
                logger.Debug("(_007CombineCPU)MBCombineCPU End, "
                           + " [MB_SNo]: " + MB_SNo
                           + " [CPUVendorSN]:" + CPUVendorSN);
            }
        }

        /// <summary>
        /// input product ID, SFC
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="prodID"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        public void ProductCombineInputProdID(string pdLine,
     string prodID,
     string editor, string stationId, string customerId)
        {
            logger.Debug("(_007CombineCPU)ProductCombineInputProdID start, "
            + " [pdLine]:" + pdLine
           + " [prodID]: " + prodID
           + " [editor]:" + editor
           + " [station]:" + stationId
           + " [customer]:" + customerId);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodID;
            Session.SessionType TheType = Session.SessionType.Product;

            try
            {
                Session CurrentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (CurrentSession == null)
                {
                    CurrentSession = new Session(sessionKey, TheType, editor, stationId, pdLine, customerId);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    //一个MB_SNo对应一个workflow
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", stationId);
                    wfArguments.Add("CurrentFlowSession", CurrentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", pdLine);
                    wfArguments.Add("Customer", customerId);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("007ProductCombineCPU.xoml", null, wfArguments);

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
                logger.Debug("(_007CombineCPU)ProductCombineInputProdID End, "
        + " [pdLine]:" + pdLine
       + " [prodID]: " + prodID
       + " [editor]:" + editor
       + " [station]:" + stationId
       + " [customer]:" + customerId);
            }
        }

        /// <summary>
        /// 1.2	UC-PCA-MCC-02 Product Combine CPU
        /// 建立Product和CPU Vender 的绑定关系
        /// </summary>
        /// <param name="prodID">Product Id</param>
        /// <param name="CPUVendorSN">CPU Vendor SN</param>
        public string ProductCombineCPU(string prodID, string CPUVendorSN)
        {
            logger.Debug("(_007CombineCPU)ProductCombineCPU start, "
               + " [prodID]: " + prodID
               + " [CPUVendorSN]:" + CPUVendorSN);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = prodID;
            Session.SessionType TheType = Session.SessionType.Product;

            try
            {
                Session sessionInfo = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (sessionInfo == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    sessionInfo.AddValue(Session.SessionKeys.CPUVendorSn, CPUVendorSN);
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
                return "ok";
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
                logger.Debug("(_007CombineCPU)ProductCombineCPU End, "
               + " [prodID]: " + prodID
               + " [CPUVendorSN]:" + CPUVendorSN);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void CancelMB(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, Session.SessionType.MB);

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

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        public void CancelProduct(string sessionKey)
        {
            try
            {
                logger.Debug("Cancel start, sessionKey:" + sessionKey);

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
                logger.Debug("Cancel end, sessionKey:" + sessionKey);
            }
        }

        #endregion
    }
}
