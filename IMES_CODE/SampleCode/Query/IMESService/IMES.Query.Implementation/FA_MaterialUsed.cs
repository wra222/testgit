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
    public class FA_MaterialUsed : MarshalByRefObject, IFA_MaterialUsed
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string qType, string fromDate, string toDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select Site, dbo.GetQueryDateType(@qType, Period_Date) 'Month', Platforms, c.Descr 'Commodity Type', HP_PN, ");
                sb.AppendLine("IEC_PN, Vendor, Consume_Qty from MaterialConsumption m ");
                sb.AppendLine("left join Commodity c on m.Commodity_Type=c.Code ");
                sb.AppendLine("where Period=@qType and Period_Date between dbo.GetPeriodDate(@qType, @fromDate) and dbo.GetPeriodDate(@qType, @toDate) ");
                sb.AppendLine("order by Site, [Month], Platforms, [Commodity Type] ");

                /*
                if ("M".Equals(qType))
                {
                    sb.AppendLine("select Site, dbo.GetQueryDateType(@qType, Period_Date) 'Month', Platforms, c.Descr 'Commodity Type', HP_PN, ");
                    sb.AppendLine("IEC_PN, Vendor, Consume_Qty from MaterialConsumption m ");
                    sb.AppendLine("left join Commodity c on m.Commodity_Type=c.Code ");
                    sb.AppendLine("where Period=@qType and Period_Date = dbo.GetQueryDate(@qType, @periodYear, @parm2) ");
                }
                else if ("Q".Equals(qType))
                {
                    sb.AppendLine("select Site, dbo.GetQueryDateType(@qType, Period_Date) 'Quarter', Platforms, c.Descr 'Commodity Type', HP_PN, ");
                    sb.AppendLine("IEC_PN, Vendor, Consume_Qty from MaterialConsumption m ");
                    sb.AppendLine("left join Commodity c on m.Commodity_Type=c.Code ");
                    sb.AppendLine("where Period=@qType and Period_Date = dbo.GetQueryDate(@qType,@periodYear, @parm2) ");
                }
                */
                
                SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 new SqlParameter("@qType", qType),
                                                 //new SqlParameter("@qDate", qDate));
                                                 new SqlParameter("@fromDate", fromDate),
                                                 new SqlParameter("@toDate", toDate));
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
