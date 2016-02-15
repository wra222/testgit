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
    public class PAK_Query87Product : MarshalByRefObject, IPAK_Query87Product
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string DBConnection, string PalletType, string Line, DateTime ShipDate,string ProductType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
               try
            {
                string SQLText = @"sp_Query_PAK_Query87Product ";
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                                  System.Data.CommandType.StoredProcedure,
                                                                  SQLText,
                                                                   SQLHelper.CreateSqlParameter("@palletType", 512,PalletType),
                                                                  SQLHelper.CreateSqlParameter("@shipDate", ShipDate),
                                                                  SQLHelper.CreateSqlParameter("@line", 128, Line)); 

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
