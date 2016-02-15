/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:UI for PCA Repaire Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI PCA Repair Input.docx –2011/12/13 
 * UC:CI-MES12-SPEC-SA-UC PCA Repair Input.docx –2011/12/08            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-01-12  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for PCA Repair Input.
    /// </summary>
    public class _PCARepairInput : MarshalByRefObject, IPCARepairInput
    {
        private const Session.SessionType TheType = Session.SessionType.MB;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IPCARepairInput Members

        /// <summary>
        /// 输入MBSno，卡站
        /// </summary>
        public void InputMBSno(string input, string line, string editor, string station, string customer)
        {
            logger.Debug("(_PCARepairInput)InputMBSno end, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = input;

                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    currentSession = new Session(sessionKey, TheType, editor, station, line, customer);

                    Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                    wfArguments.Add("Key", sessionKey);
                    wfArguments.Add("Station", station);
                    wfArguments.Add("CurrentFlowSession", currentSession);
                    wfArguments.Add("Editor", editor);
                    wfArguments.Add("PdLine", line);
                    wfArguments.Add("Customer", customer);
                    wfArguments.Add("SessionType", TheType);
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("PCARepairInput.xoml", "PCARepairInput.rules", wfArguments);

                    currentSession.SetInstance(instance);

                    if (!SessionManager.GetInstance.AddSession(currentSession))
                    {
                        currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                        erpara.Add(sessionKey);
                        ex = new FisException("CHK192", erpara);
                        throw ex;
                    }

                    currentSession.WorkflowInstance.Start();
                    currentSession.SetHostWaitOne();
                }
                else
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK193", erpara);
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

                return;
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
                logger.Debug("(_PCARepairInput)InputMBSno end, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            }
        }

        /// <summary>
        /// 检查MB是否在修护区
        /// </summary>
        public bool IsMBInRepair(string mb)
        {
            logger.Debug("(_PCARepairInput)IsMBInRepair start, MBSno:" + mb);
            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = mb;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK194", erpara);
                    throw ex;
                }
                
                IMB objMB = (IMB)currentSession.GetValue(Session.SessionKeys.MB);
                IList<MBRptRepair> rptRepairList = objMB.MBRptRepairs;
                foreach (MBRptRepair ele in rptRepairList)
                {
                    if (ele.Status == "0") return true;
                }

                return false;
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
                logger.Debug("(_PCARepairInput)IsMBInRepair end, MBSno:" + mb);
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(string mb)
        {
            logger.Debug("(_PCARepairInput)Save start, MBSno:" + mb);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = mb;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, TheType);

                if (currentSession == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK194", erpara);
                    throw ex;
                }

                currentSession.SwitchToWorkFlow();
                
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return;
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
                logger.Debug("(_PCARepairInput)Save end, MBSno:" + mb);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string mb)
        {
            logger.Debug("(_PCARepairInput)Cancel start.");
            string sessionKey = mb;
            Session session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

            if (session != null)
            {
                SessionManager.GetInstance.RemoveSession(session);
            }
            logger.Debug("(_PCARepairInput)Cancel end.");
        }
        #endregion
    }
}
