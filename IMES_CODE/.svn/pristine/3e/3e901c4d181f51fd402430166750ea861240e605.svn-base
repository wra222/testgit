using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using log4net;
using System.Reflection;
using System.Transactions;
using IMES.WS.Common;
using System.Web.Configuration;
using IMES.Query.DB;

namespace IMES.WS.MoQuery
{
    public class Execute
    {
       
        private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        

        //1.檢查必要的input parameter
        public static void ValidateParameter(MoQuery moquery)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                BaseLog.LoggingInfo(logger, "Header: \r\n{0}", ObjectTool.ObjectTostring(moquery));
                if (string.IsNullOrEmpty(moquery.MoNumber))
                {
                    throw new Exception("The MoNumber of moquery is null or no data");
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
            
        }


     

        //4.Build Response message structure
        public static MoQueryResponse BuildResponseMsg(string monumber)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            MoQueryResponse response;
            try
            {
                BaseLog.LoggingInfo(logger, "Header: \r\n{0}", ObjectTool.ObjectTostring(monumber));
                response = SQL.GetMoInfo(monumber);

                return response;
            }
            catch (Exception e)
            {
                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw e;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }

      

        }
    }
}