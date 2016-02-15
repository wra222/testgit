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
    public class PAK_BsamShipSnList : MarshalByRefObject, IPAK_BsamShipSnList
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetBsamShipSnListByDN(string Connection, string ShipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                
                return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "sp_Query_PAK_BsamPackingRpt",
                                                SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate));
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
        public DataTable GetBsamShipSnListByConsolidate(string Connection, string ShipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "sp_Query_PAK_BsamPackingRpt_ByConsolidate",
                                                SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate));
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
