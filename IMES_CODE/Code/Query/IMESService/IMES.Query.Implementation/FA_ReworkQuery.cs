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
    class FA_ReworkQuery : MarshalByRefObject, IFA_ReworkQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"rpt_ReworkQuery";


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
