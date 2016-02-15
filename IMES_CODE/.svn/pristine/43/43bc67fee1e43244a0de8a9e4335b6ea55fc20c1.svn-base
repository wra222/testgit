/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:FA MB            
 * CI-MES12-SPEC-PAK-FA MB Return.docx –2012/1/10  
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-1-10   207003                Create  
 * Known issues:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

using IMES.Docking.Interface.DockingIntf;
using log4net;
using IMES.Infrastructure.WorkflowRuntime;
using System.Workflow.Runtime;
using IMES.Route;
namespace IMES.Docking.Implementation
{
    /// <summary>
    /// IMES service for FAMBReturn.
    /// </summary>
    public class _FAMBReturn : MarshalByRefObject, IFAMBReturn
    { 
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const Session.SessionType SessionType = Session.SessionType.Product;
        #region IFAMBReturn Members
        /// <summary>
        /// delete repair of MB,and save MB
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="mbsno">mbsno</param>
        public void OnMBSNSave(string line, string editor, string station, string customer, string mbsno)
        {
            string keyStr = "";
            try
            {
                
                string sessionKey = mbsno;
                keyStr = sessionKey;
                Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, SessionType);
                if (null == currentSession)
                {
                    currentSession = new Session(sessionKey, SessionType, editor, station, line, customer);
                }
                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                wfArguments.Add("Key", sessionKey);
                wfArguments.Add("Station", station);
                wfArguments.Add("CurrentFlowSession", currentSession);
                wfArguments.Add("Editor", editor);
                wfArguments.Add("PdLine", line);
                wfArguments.Add("Customer", customer);
                wfArguments.Add("SessionType", SessionType);

                string wfName, rlName;
                RouteManagementUtils.GetWorkflow(station, "FAMBReturn.xoml", "fambreturn.rules", out wfName, out rlName);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, rlName, wfArguments);
                currentSession.AddValue(Session.SessionKeys.MBSN, mbsno);
                currentSession.AddValue(Session.SessionKeys.IsComplete, false); 
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
                if (currentSession.Exception != null)
                {
                    if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        currentSession.ResumeWorkFlow();
                    }

                    throw currentSession.Exception;
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg, e);
                throw new Exception(e.mErrmsg);
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        /// <summary>
        /// 界面确认后接着做
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="continueDo"></param>
        public void MakeSureSave(string mbSno, bool continueDo)
        {
            logger.Debug("(_FAMBReturn)MakeSureSave start, [MBSno]: " + mbSno);
            FisException ex;
            List<string> erpara = new List<string>();
            string sessionKey = mbSno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session == null)
                {
                    erpara.Add(sessionKey);
                    ex = new FisException("CHK021", erpara);
                    throw ex;
                }
                else
                {
                    if (continueDo == true)
                    {
                        session.AddValue(Session.SessionKeys.IsComplete, true);
                    }
                    session.Exception = null;
                    session.SwitchToWorkFlow();

                    //check workflow exception
                    if (session.Exception != null)
                    {
                        if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                        {
                            session.ResumeWorkFlow();
                        }

                        throw session.Exception;
                    }
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
                throw e;
            }
            finally
            {
                Session sessionDelete = SessionManager.GetInstance.GetSession(mbSno, SessionType);
                if (sessionDelete != null)
                {
                    SessionManager.GetInstance.RemoveSession(sessionDelete);
                }
                logger.Debug("(_FAMBReturn)MakeSureSave end, MBSno:" + mbSno);
            }

        }
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sno"></param>
        public void Cancel(string sno)
        {
            logger.Debug("(_FAMBReturn)Cancel start, [MbSno]:" + sno);
            List<string> erpara = new List<string>();
            string sessionKey = sno;

            try
            {
                Session session = SessionManager.GetInstance.GetSession(sessionKey, SessionType);

                if (session != null)
                {
                    SessionManager.GetInstance.RemoveSession(session);
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
                throw e;
            }
            finally
            {
                logger.Debug("(_FAMBReturn)Cancel end, [MbSno]:" + sno);
            }
        }
        #endregion
    }
}
