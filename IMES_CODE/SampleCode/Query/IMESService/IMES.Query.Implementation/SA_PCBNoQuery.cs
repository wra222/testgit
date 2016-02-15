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
    public class SA_PCBNoQuery:MarshalByRefObject,ISA_PCBNoQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetPCBNoQueryResult(string Connection, DateTime FromDate, DateTime ToDate,String PCBNo)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT OldPCBNo,NewPCBNo,Cdt FROM Change_PCB (nolock)");
                if (PCBNo == "")
                {
                    sb.AppendLine("WHERE Cdt BETWEEN @FromDate AND @ToDate ");
                }
                else
                {
                    sb.AppendLine(string.Format("where OldPCBNo in(select value from dbo.fn_split('{0}',','))or NewPCBNo in(select value from dbo.fn_split('{0}',','))", PCBNo));
                }


                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate));
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
