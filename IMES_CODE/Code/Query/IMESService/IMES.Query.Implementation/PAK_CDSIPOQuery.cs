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
    public class PAK_CDSIPOQuery : MarshalByRefObject, IPAK_CDSIPOQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, string PO)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
   
                StringBuilder sb = new StringBuilder();

                string SQLText = @"sp_Query_PAK_CDSIPO";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@PO", 512, PO)
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
