using System;
using System.Collections.Generic;
using System.Linq;
using IMES.Query.Interface.QueryIntf;
using IMES.Query.DB;
using System.Text;
using IMES.Infrastructure;
using log4net;
using System.Reflection;
using System.Data;


namespace IMES.Query.Implementation
{
    class FA_rpt_sqeweeklyreport : MarshalByRefObject, IFA_rpt_sqeweeklyreport
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetSqeWeelkyQueryResult(string Connection, DateTime FromDate, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                //StringBuilder sb = new StringBuilder();

                //sb.AppendLine("SELECT OldPCBNo,NewPCBNo,Cdt FROM Change_PCB (nolock) WHERE Cdt BETWEEN @FromDate AND @ToDate");
                //         string SQLText = sb.ToString();
                //return SQLHelper.ExecuteDataFill(Connection,
                //                                 System.Data.CommandType.Text,
                //                                 SQLText,
                //                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                //                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate));
                string SQLText = @"rpt_sqeweeklyreport";


                return SQLHelper.ExecuteDataFill(Connection,
                                                      System.Data.CommandType.StoredProcedure,
                                                      SQLText,
                                                      SQLHelper.CreateSqlParameter("@begtime", FromDate),
                                                      SQLHelper.CreateSqlParameter("@endtime", ToDate)
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
    }
}
