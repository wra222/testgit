using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using log4net;
using log4net.Config;

namespace UPH.DB
{
    public  class BaseLog
    {
        public static void LoggingBegin(ILog logger, string methodName)
        {

            logger.DebugFormat("BEGIN: {0}()", methodName);
            //logger.DebugFormat("BEGIN: {0}::{1}()", method.DeclaringType, method.Name);
        }
        public static void LoggingEnd(ILog logger, string methodName)
        {

            logger.DebugFormat("END:   {0}()", methodName);
            //logger.DebugFormat("END:   {0}::{1}()", method.DeclaringType, method.Name);
        }
        public static void LoggingError(ILog logger, string format, params object[] args)
        {

            logger.ErrorFormat(format, args);
            // logger.DebugFormat("ERROR: {0}::{1}()", method.DeclaringType, method.Name);
        }
        public static void LoggingError(ILog logger, MethodBase method, Exception e)
        {

            logger.Error(method, e);
        }

        public static void LoggingInfo(ILog logger, string format, params object[] args)
        {

            logger.DebugFormat(format, args);
        }
    }
}
