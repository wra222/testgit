using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using IMES.Infrastructure;
using log4net;
using System.Reflection;
using System.Data;

namespace IMES.Query.Implementation
{
    public class Station : MarshalByRefObject,IStation        
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetStation(string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";

                SQLText = "  SELECT RTRIM(Station) as Station, RTRIM(Descr) as Descr,RTRIM(Name),RTRIM(StationType) FROM Station  (NOLOCK) ";

                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 SQLText);
            }
            catch (Exception e)
            {

                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }

        public DataTable GetStation(string process, string DBConnection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                if (process.Length==1)
                    SQLText = "SELECT Station, RTRIM(Descr) as Descr FROM Station  (NOLOCK) WHERE StationType  LIKE 'FA%'  ORDER BY 1";
                else
                    SQLText = "SELECT Station, Station+' '+RTRIM(Descr) as Descr FROM Station (NOLOCK) WHERE StationType  LIKE 'FA%' ORDER BY 1";
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@process", 32, process, ParameterDirection.Input));
            }
            catch (Exception e)
            {

                BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), e);
                throw;
            }
            finally
            {
                BaseLog.LoggingEnd(logger, methodName);
            }
        }
    }
}
