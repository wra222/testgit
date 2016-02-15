using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using log4net;

using IMES.Infrastructure.Utility;

[assembly:log4net.Config.XmlConfiguratorAttribute(Watch = true)]

namespace IMES.Infrastructure
{
    internal class SessionManagerLogger
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string MsgBegin = "BEGIN:";
        private static readonly string MsgEnd = "END:";
        private static readonly string MsgError = "ERROR:";
        private static readonly string MsgFormat = "{0} {1} {2}";
        private static readonly string MsgRemoveSession = "SessionCacheRemovedAction::{0} Session Time Out: [{1}] , {2}";

        static SessionManagerLogger(){}
      
        public static void SetLogger(SessionManager target)
        {
            target.BeginAddSession += new SessionManager.SessionsEvent(SessionManagerMethodsBeginning);
            target.BeginGetSession += new SessionManager.SessionsEvent(SessionManagerMethodsBeginning);
            target.BeginGetSessionByInstanceId += new SessionManager.SessionsEvent(SessionManagerMethodsBeginning);
            target.BeginGetSessionByType += new SessionManager.SessionsEvent(SessionManagerMethodsBeginning);
            target.BeginRemoveSession += new SessionManager.SessionsEvent(SessionManagerMethodsBeginning);
            target.BeginTerminateWF += new SessionManager.SessionsEvent(SessionManagerMethodsBeginning);

            target.EndAddSession += new SessionManager.SessionsEvent(SessionManagerMethodsEndding);
            target.EndGetSession += new SessionManager.SessionsEvent(SessionManagerMethodsEndding);
            target.EndGetSessionByInstanceId += new SessionManager.SessionsEvent(SessionManagerMethodsEndding);
            target.EndGetSessionByType += new SessionManager.SessionsEvent(SessionManagerMethodsEndding);
            target.EndRemoveSession += new SessionManager.SessionsEvent(SessionManagerMethodsEndding);
            target.EndTerminateWF += new SessionManager.SessionsEvent(SessionManagerMethodsEndding);

            target.ErrorInRemoveSession += new SessionManager.SessionsException(SessionManagerMethodsErroring);
            target.ErrorInRemoveSession += new SessionManager.SessionsException(SessionManagerMethodsErroring);
            target.ErrorInTerminateWF += new SessionManager.SessionsException(SessionManagerMethodsErroring);
            
            target.RemovedForExpiredOrScavenged += new SessionManager.SessionsExternalTrigger(SessionRemovedForExpiredOrScavenged);
            target.TerminateWFWarnLog += new SessionManager.SessionWarnLog(SessionManagerMethodsWarnLog);
        }

        private static void SessionManagerMethodsWarnLog(string methodName, string text)
        {
            if (logger.IsWarnEnabled)
            {
                //logger.Warn(methodInfo.DeclaringType + "::" + methodInfo.Name + " " + text);
                logger.WarnFormat(MsgFormat,methodName , string.Empty , text);
            }
        }

        private static void SessionManagerMethodsBeginning(string methodName, object[] args)
        {
            if (logger.IsDebugEnabled)
            {
                //logger.Debug(methodInfo.DeclaringType + "::" + methodInfo.Name + " BEGIN: " + ToStringTool.ToString(args));
                logger.DebugFormat(MsgFormat, methodName , MsgBegin , ToStringTool.ToString(args));
            }
        }

        private static void SessionManagerMethodsEndding(string methodName, object[] args)
        {
            if (logger.IsDebugEnabled)
            {
                //logger.Debug(methodInfo.DeclaringType + "::" + methodInfo.Name + "   END: " + ToStringTool.ToString(args));
                logger.DebugFormat(MsgFormat, methodName , MsgEnd , ToStringTool.ToString(args));
            }
        }

        private static void SessionManagerMethodsErroring(string methodName, object[] args, Exception ex)
        {
            //logger.Error(methodInfo.DeclaringType + "::" + methodInfo.Name + " ERROR: " + ToStringTool.ToString(args),ex);
            logger.Error(string.Format(MsgFormat, methodName , MsgError , ToStringTool.ToString(args)), ex);
        }

        private static void SessionRemovedForExpiredOrScavenged(string methodName, Session sess, object[] args)
        {
            logger.ErrorFormat(MsgRemoveSession, methodName, sess.ToString(), ToStringTool.ToString(args));
            //logger.Error("SessionCacheRemovedAction" + "::" + methodName + " Session Time Out: [" + sess.ToString() + "] , " + ToStringTool.ToString(args));
        }
    }
}
