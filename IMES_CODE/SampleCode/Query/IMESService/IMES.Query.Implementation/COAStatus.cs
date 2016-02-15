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
    public class COAStatus : MarshalByRefObject, ICOAStatus
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public System.Data.DataTable GetCOAStatus(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"SELECT Value,Value + ' - ' + Name as Name FROM ConstValue WHERE Type='COAStatus'";
                return SQLHelper.ExecuteDataFill(Connection,
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

        public System.Data.DataTable GetCOADate(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @" SELECT REPLACE(CONVERT(NVARCHAR(10),DATEADD(d,rows-1,DATEADD(MONTH,-1,GETDATE())),111),'/','-') Date
	                                FROM (SELECT  id,ROW_NUMBER()OVER(ORDER BY id) rows   
		                                    FROM sysobjects ) Tmp
	                                WHERE Tmp.rows <= datediff(d,DATEADD(MONTH,-1,GETDATE()), GETDATE()) + 1 
                                    ";
                return SQLHelper.ExecuteDataFill(Connection,
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
    }
}