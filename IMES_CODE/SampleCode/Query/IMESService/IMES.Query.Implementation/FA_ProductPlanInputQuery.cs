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
    public class FA_ProductPlanInputQuery : MarshalByRefObject, IFA_ProductPlanInputQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string PdLine, string ShipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"IMES_ProductPlanInputQuery";
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@PdLine",PdLine),
                                                 SQLHelper.CreateSqlParameter("@ShipDate", ShipDate));
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

        public DataTable GetPdLine(string Connection, string Starg)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("select substring(Line,1,1) as PdLine ");
                sb.AppendLine("from Line where Stage=@Stage ");
                sb.AppendLine("group by substring(Line,1,1) ");
                sb.AppendLine("order by PdLine ");
                SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 new SqlParameter("@Stage", Starg));
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
