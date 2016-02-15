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
    public class PAK_UnShipSnList : MarshalByRefObject, IPAK_UnShipSnList
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetDetail(string Connection, DateTime ShipDate, string Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            DataTable dt;
            try
            {

                dt = SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 "sp_Query_FA_UnShipDetail",
                                                SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                 SQLHelper.CreateSqlParameter("@Model", int.MaxValue, Model));

                return dt;

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

        public DataTable[] GetSnList(string Connection, string ShipDate, string Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            DataSet ds;
            DataTable[] dtArr = new DataTable[2];
            try
            {

                ds = SQLHelper.ExecuteDataFillForDataSet(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 "sp_Query_FA_WipByDN_GetCUSTSN",
                                                SQLHelper.CreateSqlParameter("@ShipDate", 64, ShipDate),
                                                 SQLHelper.CreateSqlParameter("@Model", int.MaxValue, Model));
                dtArr[0] = ds.Tables[0];
                dtArr[1] = ds.Tables[1];
                return dtArr;

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
