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
    public class PAK_ShipPLT3 : MarshalByRefObject, IPAK_ShipPLT3
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetShipPLT3Report(string Connection, DateTime inputDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = string.Format(@"sp_Query_Warehouse_Detail ");
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
