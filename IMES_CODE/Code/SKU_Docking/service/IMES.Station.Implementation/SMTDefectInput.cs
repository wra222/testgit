/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:UI for SMT Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI SMT Defect Input.docx –2012/05/21
 * UC:CI-MES12-SPEC-SA-UC SMT Defect Input.docx –2012/05/21            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-05-21  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
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
    public class _SMTDefectInput : MarshalByRefObject, ISMTDefectInput
    {
        private const Session.SessionType TheType = Session.SessionType.MB;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region ISMTDefectInput Members

        /// <summary>
        /// Check MBSno
        /// </summary>
        public void CheckMBSno(string input, string line, string editor, string station, string customer)
        {
            logger.Debug("(_SMTDefectInput)CheckMBSno begin, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("SMTDefectInput.xoml", "SMTDefectInput.rules", wfArguments);

                    currentSession.SetInstance(instance);
                    currentSession.AddValue(Session.SessionKeys.IsComplete, false);
                    currentSession.AddValue(Session.SessionKeys.DefectList, new List<string>());
                    currentSession.AddValue("DefectInfoList", new List<DefectCodeDescr>());

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
                logger.Debug("(_SMTDefectInput)CheckMBSno end, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            }
        }

        /// <summary>
        /// Input Defect
        /// </summary>
        public IList<DefectCodeDescr> InputDefect(string mb, string defect)
        {
            logger.Debug("(_SMTDefectInput)InputDefect start, MBSno:" + mb + ", Defect:" + defect);

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

                currentSession.AddValue(Session.SessionKeys.DCode, defect);

                currentSession.SwitchToWorkFlow();

                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }

                return currentSession.GetValue("DefectInfoList") as IList<DefectCodeDescr>;
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
                logger.Debug("(_SMTDefectInput)InputDefect end, MBSno:" + mb + ", Defect:" + defect);
            }
        }

        /// <summary>
        /// Clear Defect
        /// </summary>
        public void ClearDefect(string mb)
        {
            logger.Debug("(_SMTDefectInput)ClearDefect start, MBSno:" + mb);

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

                currentSession.AddValue(Session.SessionKeys.DefectList, new List<string>());
                currentSession.AddValue("DefectInfoList", new List<DefectCodeDescr>());

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
                logger.Debug("(_SMTDefectInput)ClearDefect end, MBSno:" + mb);
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(string mb)
        {
            logger.Debug("(_SMTDefectInput)Save start, MBSno:" + mb);

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

                currentSession.AddValue(Session.SessionKeys.IsComplete, true);
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
                logger.Debug("(_SMTDefectInput)Save end, MBSno:" + mb);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string mb)
        {
            logger.Debug("(_SMTDefectInput)Cancel start.");
            string sessionKey = mb;
            Session session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

            if (session != null)
            {
                SessionManager.GetInstance.RemoveSession(session);
            }
            logger.Debug("(_SMTDefectInput)Cancel end.");
        }
        #endregion
    }
}
