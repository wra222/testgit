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
    public class PAK_PLT3 : MarshalByRefObject, IPAK_PLT3
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetPLT3Report(string Connection, DateTime inputDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = string.Format(@"sp_Query_PLTReadinessReport ");
                return SQLHelper.ExecuteDataFill(Connection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText,
                                                                  SQLHelper.CreateSqlParameter("@inputDate", inputDate)
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
