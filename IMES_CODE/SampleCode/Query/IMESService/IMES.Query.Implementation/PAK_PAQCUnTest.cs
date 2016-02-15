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

    public class PAK_PAQCUnTest : MarshalByRefObject, IPAK_PAQCUnTest
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime shipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"sp_Query_PAQCUnTest";

                return SQLHelper.ExecuteDataFill(Connection,
                                                       System.Data.CommandType.StoredProcedure,
                                                       SQLText,
                                                       SQLHelper.CreateSqlParameter("@shipDate", shipDate));

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
