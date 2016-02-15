﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using IMES.Infrastructure;
using IMES.Route;
using System.Workflow.Runtime;
using IMES.Infrastructure.WorkflowRuntime;

namespace IMES.Station.Implementation
{
    sealed class WorkflowUtility
    {
        internal class Constant
        {
            public const string Key = "Key";
            public const string Station = "Station";
            public const string CurrentFlowSession = "CurrentFlowSession";
            public const string Editor = "Editor";
            public const string PdLine = "PdLine";
            public const string Customer = "Customer";
            public const string SessionType = "SessionType";
        }
        public static Session InvokeWF(string sessionKey,
                                                       string station,
                                                       string pdLine,
                                                       string customer,
                                                       string editor,
                                                       Session.SessionType sessionType,
                                                       string wfName,
                                                       string wfRule,
                                                       Dictionary<string, object> sessionKeyValue)
        {
            Session currentSession = SessionManager.GetInstance.GetSession(sessionKey, sessionType);
            if (currentSession == null)
            {
                currentSession = new Session(sessionKey, sessionType, editor, station, pdLine, customer);
                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                wfArguments.Add(Constant.Key, sessionKey);
                wfArguments.Add(Constant.Station, station);
                wfArguments.Add(Constant.CurrentFlowSession, currentSession);
                wfArguments.Add(Constant.Editor, editor);
                wfArguments.Add(Constant.PdLine, pdLine);
                wfArguments.Add(Constant.Customer, customer);
                wfArguments.Add(Constant.SessionType, sessionType);
                RouteManagementUtils.GetWorkflow(station, wfName, wfRule, out wfName, out wfRule);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, wfRule, wfArguments);
                //Add Current Session key/value
                if (sessionKeyValue != null)
                {
                    foreach (KeyValuePair<string, object> item in sessionKeyValue)
                    {
                        currentSession.AddValue(item.Key, item.Value);
                    }
                }

                currentSession.SetInstance(instance);

                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                    throw new FisException("CHK020", new List<string> { sessionKey });
                }
                currentSession.WorkflowInstance.Start();
                currentSession.SetHostWaitOne();
            }
            else
            {
                throw new FisException("CHK020", new List<string> { sessionKey });
            }

            if (currentSession.Exception != null)
            {
                //Check Workflow Terminate Event will resume workflow event
                if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                {
                    currentSession.ResumeWorkFlow();
                }
                throw currentSession.Exception;
            }
            return currentSession;

        }

        public static void InvokeWF(string sessionKey,
                                                  string station,
                                                  string pdLine,
                                                  string customer,
                                                  string editor,
                                                  Session.SessionType sessionType,
                                                  string wfName,
                                                  string wfRule,
                                                  Dictionary<string, object> sessionKeyValue,
                                                   out Session currentSession)
        {
            currentSession = SessionManager.GetInstance.GetSession(sessionKey, sessionType);

            if (currentSession == null)
            {
                currentSession = new Session(sessionKey, sessionType, editor, station, pdLine, customer);
                Dictionary<string, object> wfArguments = new Dictionary<string, object>();
                wfArguments.Add(Constant.Key, sessionKey);
                wfArguments.Add(Constant.Station, station);
                wfArguments.Add(Constant.CurrentFlowSession, currentSession);
                wfArguments.Add(Constant.Editor, editor);
                wfArguments.Add(Constant.PdLine, pdLine);
                wfArguments.Add(Constant.Customer, customer);
                wfArguments.Add(Constant.SessionType, sessionType);
                RouteManagementUtils.GetWorkflow(station, wfName, wfRule, out wfName, out wfRule);
                WorkflowInstance instance = WorkflowRuntimeManager.getInstance.CreateXomlFisWorkflow(wfName, wfRule, wfArguments);
                //Add Current Session key/value
                if (sessionKeyValue != null)
                {
                    foreach (KeyValuePair<string, object> item in sessionKeyValue)
                    {
                        currentSession.AddValue(item.Key, item.Value);
                    }
                }

                currentSession.SetInstance(instance);

                if (!SessionManager.GetInstance.AddSession(currentSession))
                {
                    currentSession.WorkflowInstance.Terminate("Session:" + sessionKey + " Exists.");
                    throw new FisException("CHK020", new List<string> { sessionKey });
                }
                currentSession.WorkflowInstance.Start();
                currentSession.SetHostWaitOne();
            }
            else
            {
                throw new FisException("CHK020", new List<string> { sessionKey });
            }

            if (currentSession.Exception != null)
            {
                //Check Workflow Terminate Event will resume workflow event
                if (currentSession.GetValue(Session.SessionKeys.WFTerminated) != null)
                {
                    currentSession.ResumeWorkFlow();
                }
                throw currentSession.Exception;
            }

            // return currentSession;

        }

        public static Session SwitchToWF(Session session,
                                                       Dictionary<string, object> sessionKeyValue)
        {
            if (session == null)
            {
                throw new Exception("No Session !!");
            }

            if (sessionKeyValue != null)
            {
                foreach (KeyValuePair<string, object> item in sessionKeyValue)
                {
                    session.AddValue(item.Key, item.Value);
                }
            }
            session.Exception = null;
            session.SwitchToWorkFlow();
            if (session.Exception != null)
            {
                if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                {
                    session.ResumeWorkFlow();
                }
                throw session.Exception;
            }
            return session;
        }
        /// <summary>
        /// time out for SwitchToWF
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sessionKeyValue"></param>
        /// <param name="timeout">Second unit</param>
        /// <returns></returns>
        public static Session SwitchToWF(Session session,
                                                           Dictionary<string, object> sessionKeyValue,
                                                           int timeout)
        {
            if (session == null)
            {
                throw new Exception("No Session !!");
            }

            if (sessionKeyValue != null)
            {
                foreach (KeyValuePair<string, object> item in sessionKeyValue)
                {
                    session.AddValue(item.Key, item.Value);
                }
            }
            bool hasTimeOut = false;
            session.Exception = null;
            DateTime now = DateTime.Now;
            hasTimeOut =!session.SwitchToWorkFlow(timeout*1000);
            if (hasTimeOut)
            {
                throw new Exception(string.Format("[TimeOut] SessionKey:{0} Line:{1} Editor:{2} Station:{3} TimeOut:{4} Cdt:{5} waiting wf timeout!!!",
                                                                      session.Key, session.Line??"",session.Editor??"", session.Station, timeout.ToString(), now.ToString("yyyyMMdd HH:mm:ss.fff")));
            }
            else
            {
                if (session.Exception != null)
                {
                    if (session.GetValue(Session.SessionKeys.WFTerminated) != null)
                    {
                        session.ResumeWorkFlow();
                    }
                    throw session.Exception;
                }
            }

            return session;
        }

        public static Session SwitchToWF(string sessionKey,
                                                           Session.SessionType sessionType,
                                                           Dictionary<string, object> sessionKeyValue)
        {
            Session session = GetSession(sessionKey,
                                                         sessionType,
                                                         true);
            return SwitchToWF(session, sessionKeyValue);
        }

        public static Session SwitchToWF(string sessionKey,
                                                          Session.SessionType sessionType,
                                                          Dictionary<string, object> sessionKeyValue,
                                                          int timeout)
        {
            Session session = GetSession(sessionKey,
                                                         sessionType,
                                                         true);
            return SwitchToWF(session, sessionKeyValue,timeout);
        }

        public static Session GetSession(string sessionKey,
                                                         Session.SessionType sessionType,
                                                         bool noSessionThrowError)
        {
            Session session = SessionManager.GetInstance.GetSession(sessionKey, sessionType);
            if (noSessionThrowError && session == null)
            {
                throw new FisException("CHK021", new List<string> { sessionKey });
            }
            return session;
        }

        public static bool CancelSession(string sessionKey,
                                                         Session.SessionType sessionType)
        {
            Session session = GetSession(sessionKey, sessionType, false);
            if (session != null)
            {
                SessionManager.GetInstance.RemoveSession(session);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
