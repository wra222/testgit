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

        public DataTable GetQueryResult(string Connection, string PdLine, string ShipDate,string model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"IMES_ProductPlanInputQuery";
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@PdLine", PdLine),
                                                 SQLHelper.CreateSqlParameter("@ShipDate", Convert.ToDateTime(ShipDate)),
                                                 SQLHelper.CreateSqlParameter("@Model", model)); 
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

        public DataTable GetQueryResultByDayRange(string Connection, string PdLine, string dayFrom,string dayTo,string model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"IMES_ProductPlanInputQueryByInputDate";
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@PdLine", PdLine),
                                                 SQLHelper.CreateSqlParameter("@InputBegDate", Convert.ToDateTime(dayFrom)),
                                                 SQLHelper.CreateSqlParameter("@InputEndDate", Convert.ToDateTime(dayTo)),
                                                 SQLHelper.CreateSqlParameter("@Model", model));
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

        public DataTable GetDetailQueryResult(string Connection, string PdLine, string ShipDate, string model, string station, string type)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"IMES_ProductPlanInputQuery_Detail";//@PdLine, @ShipDate, @Model , @Station, @Type
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@PdLine", PdLine),
                                                 SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                 SQLHelper.CreateSqlParameter("@Model", model),
                                                 SQLHelper.CreateSqlParameter("@Station", station),
                                                 SQLHelper.CreateSqlParameter("@Type", type));
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

        public DataTable GetDetailQueryResultByDayRange(string Connection, string PdLine, string dayFrom, string dayTo, string model, string station, string type, string inputPdLine, string ShipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"IMES_ProductPlanInputQueryByInputDate_Detail ";//@PdLine, @InputLine, @InputBegDate,@InputEndDate, @Model , @Station, @Type
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@PdLine", PdLine),
                                                 SQLHelper.CreateSqlParameter("@InputBegDate", dayFrom),
                                                 SQLHelper.CreateSqlParameter("@InputEndDate", dayTo),
                                                 SQLHelper.CreateSqlParameter("@Model", model),
                                                 SQLHelper.CreateSqlParameter("@Station", station),
                                                 SQLHelper.CreateSqlParameter("@Type", type),
                                                 SQLHelper.CreateSqlParameter("@InputLine", inputPdLine),
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
