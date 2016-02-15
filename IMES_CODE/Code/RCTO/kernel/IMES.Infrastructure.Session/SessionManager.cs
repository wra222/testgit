﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using log4net;

namespace IMES.Infrastructure
{
    /// <summary>
    /// Session管理器
    /// </summary>
    public sealed class SessionManager
    {
        public delegate void SessionsEvent(string methodName, object[] args);
        public delegate void SessionsException(string methodName, object[] args, Exception ex);
        public delegate void SessionsExternalTrigger(string methodName, Session sess, object[] args);
        public delegate void SessionWarnLog(string methodName, string text);

        #region . Singleton .
        private static readonly SessionManager _instance = new SessionManager();
        public static SessionManager GetInstance
        {
            get
            {
                return _instance;
            }                                                           
        }
        private SessionManager()
        {
            SessionManagerLogger.SetLogger(this);
            string _slidingTime = ConfigurationManager.AppSettings.Get("SessionTimeOut");
            if (!int.TryParse(_slidingTime, out CacheSlidingTime))
                CacheSlidingTime = 60;         
        }
        #endregion

        #region Cache manager
        private readonly SessionCacheRemovedAction sessionCacheRemovedAction = new SessionCacheRemovedAction();
        private readonly int CacheSlidingTime = 60;
        private readonly CacheManager _sessionCache = CacheFactory.GetCacheManager("SessionManager");
        //private readonly SlidingTime cacheSlidingTime = null;
        #endregion

        private readonly Dictionary<Guid, string> _wfIDKeyMap = new Dictionary<Guid, string>(); //a map of (wfInstanceId, sessionKey)
            
        //private const int DefaultSlidingTime = 60;
        private readonly object _synObjSessions = new object();

        public event SessionsEvent BeginGetSession;
        public event SessionsEvent EndGetSession;

        private readonly string MethodGetSession = "SessionManager::GetSession";
        private readonly string MethodGetSessionByType = "SessionManager::GetSessionByType";
        private readonly string MethodAddSession = "SessionManager::AddSession";
        private readonly string MethodRemoveSession = "SessionManager::RemoveSession";
        private readonly string MethodGetSessionByInstanceId = "SessionManager::GetSessionByInstanceId";
        private readonly string MethodTerminateWF = "SessionManager::TerminateWF";
       

        private readonly string MsgTerminateWFBegin = "[Begin]TerminateWF HostWaitingCount:{0} WFWaitingCount:{1}";
        private readonly string MsgTerminateWFEnd = "[End]TerminateWF HostWaitingCount:{0} WFWaitingCount:{1}";

        /// <summary>
        /// 通过sn获取session对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Session GetSession(string key, Session.SessionType type)
        {
            //MethodBase methodInfo = MethodBase.GetCurrentMethod();
            if (this.BeginGetSession != null)
                this.BeginGetSession(MethodGetSession, new object[] { key, type });

            Monitor.Enter(this._synObjSessions);
            try
            {
                Session session = null;
                string gKey = Session.GetGlobalSessionKey(key, type);

                if (this._sessionCache.Contains(gKey))
                {
                    session = (Session)this._sessionCache.GetData(gKey);
                    //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    //logger.InfoFormat("SessionManager.GetSession: Hash: {0}; Key: {1}; Type: {2}", session.GetHashCode().ToString(), session.Key, session.Type.ToString());
                }
                return session;
            }
            finally
            {
                Monitor.Exit(this._synObjSessions);
                if (this.EndGetSession != null)
                    this.EndGetSession(MethodGetSession, new object[] { key, type });
            }
        }

        public event SessionsEvent BeginGetSessionByType;
        public event SessionsEvent EndGetSessionByType;

        /// <summary>
        /// 根据类型获得session
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<Session> GetSessionByType(Session.SessionType type)
        {
            //MethodBase methodInfo = MethodBase.GetCurrentMethod();
            if (this.BeginGetSessionByType != null)
                this.BeginGetSessionByType(MethodGetSessionByType, new object[] { type });

            Monitor.Enter(this._synObjSessions);
            try
            {
                List<Session> commonList = new List<Session>();

                foreach (KeyValuePair<Guid, String> item in this._wfIDKeyMap)
                {
                    if (item.Value.Contains(Session.GetSessionTypeStr(type)))
                        commonList.Add((Session)this._sessionCache.GetData(item.Value));
                }
                return commonList;
            }
            finally
            {
                Monitor.Exit(this._synObjSessions);
                if (this.EndGetSessionByType != null)
                    this.EndGetSessionByType(MethodGetSessionByType, new object[] { type });
            }
        }

        public event SessionsEvent BeginAddSession;
        public event SessionsEvent EndAddSession;

        /// <summary>
        /// 添加Session对象和sn的对应
        /// </summary>
        /// <param name="session">session</param>
        public bool AddSession(Session session)
        {
            //MethodBase methodInfo = MethodBase.GetCurrentMethod();
            if (this.BeginAddSession != null)
                this.BeginAddSession(MethodAddSession, new object[] { session });

            Monitor.Enter(this._synObjSessions);
            try
            {
                string gKey = Session.GetGlobalSessionKey(session);

                //int slide = 0;

                //if (!int.TryParse(_slidingTime, out slide))
                //    slide = DefaultSlidingTime;

                if (!this._sessionCache.Contains(gKey))
                {
                    //this._sessionCache.Add(gKey, session, CacheItemPriority.Normal, new SessionCacheRemovedAction(), new SlidingTime(TimeSpan.FromMinutes(slide)));
                    this._sessionCache.Add(gKey, session, CacheItemPriority.Normal, sessionCacheRemovedAction, new SlidingTime(TimeSpan.FromMinutes(CacheSlidingTime)));
                    this._wfIDKeyMap.Add(session.Id, gKey);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                Monitor.Exit(this._synObjSessions);
                if (this.EndAddSession != null)
                    this.EndAddSession(MethodAddSession, new object[] { session });
            }
        }

        public event SessionsEvent BeginRemoveSession;
        public event SessionsEvent EndRemoveSession;
        public event SessionsException ErrorInRemoveSession;

        /// <summary>
        /// 删除指定的sn对应的session对象
        /// </summary>
        /// <param name="session">session</param>
        public void RemoveSession(Session session)
        {
            RemoveSession(session,true);
        }
        public void RemoveSession(Session session, bool isTermination)
        {
            //MethodBase methodInfo = MethodBase.GetCurrentMethod();
            if (this.BeginRemoveSession != null)
                this.BeginRemoveSession(MethodRemoveSession, new object[] { session, isTermination });

            #region trash
            //    'release part-uut binding cache
            //    BindingCacheManager.GetInstance.removeBindingByValue(session.Key)

            //    'release shipping 05 cache
            //    ICZUnitCacheManager.GetInstance.removeBindingByValue(session.Key)

            //    'release shipping pallete cache
            //    If session.Type = SessionType.SESSION_SHIPPING Then
            //        Dim pallet As Object = session.GetValue(SessionKey.Pallet)
            //        If Not pallet Is Nothing AndAlso TypeOf pallet Is Pallet Then
            //            Dim palletNo As String = CType(pallet, Pallet).PalletNo
            //            ShippingPalletCacheManager.GetInstance.removeBinding(palletNo)
            //        End If
            //    End If

            //    'release  pvs ckpo preorder
            //    If session.Type = SessionType.SESSION_TYPE_PO And isTermination Then
            //        If session.GetValue(SessionKey.CKPreOrdered) = True Then
            //            Dim ckpo As Object = session.GetValue(SessionKey.CKPO)
            //            If Not ckpo Is Nothing Then
            //                CType(ckpo, CKPO).countermand("")
            //            End If
            //        End If
            //    End If
            #endregion

            Monitor.Enter(this._synObjSessions);
            try
            {
                session.SetSessionCompleted();               
                string gKey = Session.GetGlobalSessionKey(session);
                this._sessionCache.Remove(gKey);
                this._wfIDKeyMap.Remove(session.Id);

                //Vincent 2014-11-17 first step release wf & host thread
                session.ResumeHost();
                session.ResumeWorkFlow();               

            }
            catch (Exception ex) 
            {
                if (this.ErrorInRemoveSession != null)
                    this.ErrorInRemoveSession(MethodRemoveSession, new object[] { session, isTermination }, ex);
            }
            finally
            {
                Monitor.Exit(this._synObjSessions);

                if (this.EndRemoveSession != null)
                    this.EndRemoveSession(MethodRemoveSession, new object[] { session, isTermination });
            }

            //Vincent add: check Terminate flag
            //if (isTermination)
            //{
            this.TerminateWF(session);
            //}
        }

        public event SessionsEvent BeginGetSessionByInstanceId;
        public event SessionsEvent EndGetSessionByInstanceId;

        /// <summary>
        /// 通过workflow的InstanceID获取对应的session对象
        /// </summary>
        /// <param name="instanceID">workflow的InstanceID</param>
        /// <returns>session对象</returns>
        public Session GetSessionByInstanceId(Guid instanceID)
        {
            //MethodBase methodInfo = MethodBase.GetCurrentMethod();
            if (this.BeginGetSessionByInstanceId != null)
                this.BeginGetSessionByInstanceId(MethodGetSessionByInstanceId, new object[] { instanceID });
            Monitor.Enter(this._synObjSessions);
            try
            {
                Session session = null;

                if (this._wfIDKeyMap.ContainsKey(instanceID))
                {
                    string key = this._wfIDKeyMap[instanceID];
                    if (this._sessionCache.Contains(key))
                        session = (Session)this._sessionCache.GetData(key);
                }
                return session;
            }
            finally
            {
                Monitor.Exit(this._synObjSessions);
                if (this.EndGetSessionByInstanceId != null)
                    this.EndGetSessionByInstanceId(MethodGetSessionByInstanceId, new object[] { instanceID });
            }
        }

        public string GetSessionKeyByInstanceId(Guid instanceID)
        {
            //MethodBase methodInfo = MethodBase.GetCurrentMethod();
            this.BeginGetSessionByInstanceId(MethodGetSessionByInstanceId, new object[] { instanceID });
            Monitor.Enter(this._synObjSessions);
            try
            {
                if (this._wfIDKeyMap.ContainsKey(instanceID))
                    return this._wfIDKeyMap[instanceID];
                return string.Empty;
            }
            finally
            {
                Monitor.Exit(this._synObjSessions);
                this.EndGetSessionByInstanceId(MethodGetSessionByInstanceId, new object[] { instanceID });
            }
        }

        public event SessionsEvent BeginTerminateWF;
        public event SessionsEvent EndTerminateWF;
        public event SessionsException ErrorInTerminateWF;
        public event SessionWarnLog TerminateWFWarnLog; 

        internal void TerminateWF(Session session)
        {
            //MethodBase methodInfo = MethodBase.GetCurrentMethod();
            if (this.BeginTerminateWF != null)
                this.BeginTerminateWF(MethodTerminateWF, new object[] { session });

            try
            { 
                //Vincent 2014-11-17 second step release remaing wf & host thread after one million second
                Thread.Sleep(50);
                bool hasCloseThread = false;
                if (session.GetHostWaitingCount > 0 ||
                   session.GetWFWaitingCount > 0 &&
                   this.TerminateWFWarnLog != null)
                {
                    this.TerminateWFWarnLog(MethodTerminateWF, string.Format(MsgTerminateWFBegin, session.GetHostWaitingCount.ToString(), session.GetWFWaitingCount));
                    hasCloseThread = true;
                }

                session.WFThreadClose();
                session.HostThreadClose();
                if (hasCloseThread)
                {
                    this.TerminateWFWarnLog(MethodTerminateWF, string.Format(MsgTerminateWFEnd, session.GetHostWaitingCount.ToString(), session.GetWFWaitingCount));
                }

                if (session.WorkflowInstance != null)
                {
                    string gKey = Session.GetGlobalSessionKey(session);
                    try
                    {
                        session.WorkflowInstance.Terminate("Session:" + gKey + " cleared.");
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                //'if the workflow has completed or terminated, WorkflowInstance.Terminate() would cause an exception, 
                //'this exception should not be thrown up the call stack.
                if (this.ErrorInTerminateWF != null)
                    this.ErrorInTerminateWF(MethodTerminateWF, new object[] { session }, ex);
            }
            finally
            {
                if (this.EndTerminateWF != null)
                    this.EndTerminateWF(MethodTerminateWF, new object[] { session });
            }
        }

        public event SessionsExternalTrigger RemovedForExpiredOrScavenged;
        private void OnRemovedForExpiredOrScavenged(string methodName,Session sess, object[] args)
        {
            if (this.RemovedForExpiredOrScavenged != null)
                this.RemovedForExpiredOrScavenged(methodName, sess, args);
        }

        [Serializable]
        public class SessionCacheRemovedAction : ICacheItemRefreshAction
        {
            private readonly string MethodCacheRefresh = "SessionCacheRemovedAction::Refresh";
            public void Refresh(string removedKey, object expiredValue, CacheItemRemovedReason removalReason)
            {
                if (removalReason == CacheItemRemovedReason.Expired || removalReason == CacheItemRemovedReason.Scavenged)
                {
                    //SessionManager.GetInstance.OnRemovedForExpiredOrScavenged(MethodBase.GetCurrentMethod().DeclaringType + "::" + MethodBase.GetCurrentMethod().Name, ((Session)expiredValue), new object[] { removalReason });
                    SessionManager.GetInstance.OnRemovedForExpiredOrScavenged(MethodCacheRefresh, ((Session)expiredValue), new object[] { removalReason });

                    SessionManager.GetInstance.RemoveSession((Session)expiredValue);
                }
            }
        }
    }
}