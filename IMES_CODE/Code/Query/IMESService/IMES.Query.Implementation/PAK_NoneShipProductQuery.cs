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
using System.Data.Sql;
using System.Data.SqlClient;

namespace IMES.Query.Implementation
{
    public class PAK_NoneShipProductQuery : MarshalByRefObject, IPAK_NoneShipProductQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string model, string line, string station)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"IMES_NoneShipProductQuery";
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@Model", model),
                                                 SQLHelper.CreateSqlParameter("@Line", line),
                                                 SQLHelper.CreateSqlParameter("@Station", station)); 
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

        public DataTable GetPdLine(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select distinct AliasLine ");
                sb.AppendLine("from Line a ");
                sb.AppendLine("inner join LineEx b on a.Line = b.Line ");
                sb.AppendLine("where a.Stage in ('FA','PAK') ");
                sb.AppendLine("order by AliasLine ");
                SQLText = sb.ToString();
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

        public DataTable GetStation(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select Value from SysSetting where Name in('FAOnlineStation','PAKOnlineStation') ");
                SQLText = sb.ToString();
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
