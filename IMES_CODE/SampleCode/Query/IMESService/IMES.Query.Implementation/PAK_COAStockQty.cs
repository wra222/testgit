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
    public class PAK_COAStockQty : MarshalByRefObject, IPAK_COAStockQty
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetQueryResultExt(string Connection, string status, string line, bool isSummary)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            string SQLText = "";
            try
            {
                if (isSummary)
                {
                    if (line == "")
                    {
                        SQLText = @"  SELECT IECPN,COUNT(COASN) as Qty 
                                                   FROM COAStatus (NOLOCK)
                                                 Where   Status=@Status 
                                                   GROUP BY IECPN 
                                                   ORDER BY IECPN ";
                    }
                    else
                    {
                        SQLText = @"  SELECT IECPN,COUNT(COASN) as Qty 
                                                   FROM COAStatus (NOLOCK)
                                                 Where Line =@Line and Status=@Status 
                                                   GROUP BY IECPN 
                                                   ORDER BY IECPN ";
                    }
                    return SQLHelper.ExecuteDataFill(Connection,
                                              System.Data.CommandType.Text,
                                              SQLText,
                                              SQLHelper.CreateSqlParameter("@Status", 64, status),
                                              SQLHelper.CreateSqlParameter("@Line", 64, line)
                                              );

                }
                else
                {
                     SQLText = @"rpt_COAStatusReport_ALL";

                    return SQLHelper.ExecuteDataFill(Connection,
                                                     System.Data.CommandType.StoredProcedure,
                                                     SQLText,
                                                     SQLHelper.CreateSqlParameter("@Status", 64, status),
                                                     SQLHelper.CreateSqlParameter("@Line", 64, line)
                                                     );
                
                }


               
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



        public DataTable GetQueryResult(string Connection, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"SELECT IECPN,COUNT(COASN) as Qty 
                                   FROM COAStatus (NOLOCK)
                                   WHERE Cdt> '2009/01/01 00:00:00' AND Cdt < @ToDate 
                                   GROUP BY IECPN 
                                   ORDER BY IECPN
                                   ";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,                                                 
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate));
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
