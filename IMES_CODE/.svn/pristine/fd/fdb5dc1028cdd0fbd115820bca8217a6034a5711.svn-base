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
    public class PAK_Warehouse1 : MarshalByRefObject, IPAK_Warehouse1
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetWarehouseDashBoardData(string DBConnection,DateTime ShipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = string.Format(@"sp_Query_WarehouseDashBoard ");
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText,
                                                                    SQLHelper.CreateSqlParameter("@ShipDate", ShipDate)
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

        public DataTable GetWarehouseDashBoardData2(string DBConnection,DateTime ShipDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = string.Format("sp_Query_WarehouseDashBoard2");
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText,
                                                                  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate)
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

        //public DataTable GetWarehouseDashBoardData_Detail(string DBConnection, string ShipDate,string MAWB,string Status)
        public DataTable GetWarehouseDashBoardData_Detail(string DBConnection, string MAWB, string Status, string ShipDate1)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = string.Format("sp_Query_WarehouseDashBoard_Detail");
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText,
                                                                 
                                                                  SQLHelper.CreateSqlParameter("@MAWB", MAWB),
                                                                  SQLHelper.CreateSqlParameter("@Status", Status),
                                                                  SQLHelper.CreateSqlParameter("@ShipDate1", ShipDate1)
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
