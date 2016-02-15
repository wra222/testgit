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
    public class PAK_IdleTime : MarshalByRefObject, IPAK_IdleTime
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetIdleTimeResult(string Connection, string SelectDay, int min)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            string m = Convert.ToString(min);
            try
            {
                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("select b.PdLine, b.Model,  count(*) as Qty ");
                //sb.AppendLine("from Delivery a ");
                //sb.AppendLine("cross apply dbo.fn_ProductIdleTime(a.Model) b ");
                //sb.AppendLine("where b.TimeLen > @Hours and a.ShipDate = @Day ");
                //sb.AppendLine("group by b.Model, b.PdLine ");
                //string SQLText = sb.ToString();
                //return SQLHelper.ExecuteDataFill(Connection,
                //                                 System.Data.CommandType.Text,
                //                                 SQLText,
                //                                 SQLHelper.CreateSqlParameter("@Hours", min),
                //                                 SQLHelper.CreateSqlParameter("@Day",SelectDay));
                return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "IMES_ProductIdleTime_Summary",
                                                SQLHelper.CreateSqlParameter("@ShipDate",64,SelectDay),
                                                SQLHelper.CreateSqlParameter("@Hour", 32, m)
                                                );
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
        public DataTable GetIdleTimeDetailResult(string Connection, string SelectDay, int min, string Station,string Line)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select distinct b.PdLine, b.Model, b.ProductID, b.CUSTSN, b.Station, b.[Time], b.TimeLen as [TimeLen<min>] ");
                sb.AppendLine("from Delivery a ");
                sb.AppendLine("cross apply dbo.fn_ProductIdleTime(a.Model) b ");
                sb.AppendLine("where b.TimeLen > @Hours and a.ShipDate = @Day ");
                sb.AppendLine("and b.Station=@Station and b.PdLine=@Line ");
                sb.AppendLine("order by b.Model desc,b.TimeLen ASC ");
                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@Hours", min),
                                                 SQLHelper.CreateSqlParameter("@Day", SelectDay),
                                                 SQLHelper.CreateSqlParameter("@Station", Station),
                                                 SQLHelper.CreateSqlParameter("@Line",Line));
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
