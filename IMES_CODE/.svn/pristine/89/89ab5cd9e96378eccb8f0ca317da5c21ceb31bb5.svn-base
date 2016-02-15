/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Service for KP Defect Input Page
 *             
 * UI:CI-MES12-SPEC-SA-UI KeyParts Defect Input.docx
 * UC:CI-MES12-SPEC-SA-UC KeyParts Defect Input.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-20  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.Runtime;
using IMES.DataModel;
using IMES.FisObject.Common.Repair;
using IMES.Infrastructure;
using IMES.Infrastructure.WorkflowRuntime;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;
namespace IMES.Station.Implementation
{
    /// <summary>
    /// IMES service for KP defect input.
    /// </summary>
    public class _KPDefectInput : MarshalByRefObject, IKPDefectInput
    {
        private const Session.SessionType TheType = Session.SessionType.Common;
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IKPDefectInput Members

        /// <summary>
        /// Check CTNO
        /// </summary>
        public void CheckCTNO(string input, string line, string editor, string station, string customer)
        {
            logger.Debug("(_KPDefectInput)CheckCTNo begin, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
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
                    WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow("KPDefectInput.xoml", "KPDefectInput.rules", wfArguments);

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
                logger.Debug("(_KPDefectInput)CheckCTNo end, input:" + input + " pdLine:" + line + " editor:" + editor + " stationId:" + station + " customerId:" + customer);
            }
        }

        /// <summary>
        /// Input Defect
        /// </summary>
        public IList<DefectCodeDescr> InputDefect(string ctno, string defect)
        {
            logger.Debug("(_KPDefectInput)InputDefect start, CTNO:" + ctno + ", Defect:" + defect);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = ctno;
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
                logger.Debug("(_KPDefectInput)InputDefect end, CTNO:" + ctno + ", Defect:" + defect);
            }
        }

        /// <summary>
        /// Clear Defect
        /// </summary>
        public void ClearDefect(string ctno)
        {
            logger.Debug("(_KPDefectInput)ClearDefect start, CTNO:" + ctno);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = ctno;
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
                logger.Debug("(_KPDefectInput)ClearDefect end, CTNO:" + ctno);
            }
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(string ctno)
        {
            logger.Debug("(_KPDefectInput)Save start, CTNO:" + ctno);

            FisException ex;
            List<string> erpara = new List<string>();

            try
            {
                string sessionKey = ctno;
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
                logger.Debug("(_KPDefectInput)Save end, CTNO:" + ctno);
            }
        }

        /// <summary>
        /// 取消工作流
        /// </summary>
        public void Cancel(string ctno)
        {
            logger.Debug("(_KPDefectInput)Cancel start.");
            string sessionKey = ctno;
            Session session = SessionManager.GetInstance.GetSession(sessionKey, TheType);

            if (session != null)
            {
                SessionManager.GetInstance.RemoveSession(session);
            }
            logger.Debug("(_KPDefectInput)Cancel end.");
        }
        #endregion
    }
}
