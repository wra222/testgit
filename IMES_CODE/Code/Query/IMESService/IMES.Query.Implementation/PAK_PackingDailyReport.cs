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

    public class PAK_PackingDailyReport : MarshalByRefObject,IPAK_PackingDailyReport
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetQueryResult(string Connection, DateTime inputDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
               
                string SQLText = @"sp_Query_PAK_PakingDailyReport_Ext";


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


        public DataTable GetQueryResult(string Connection, DateTime inputDate,string PAKStation,string FAStation )
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                //DataTable dt = null;
                //return dt;
//                string SQLText = @"sp_Query_PAK_PakingDailyReport '2012-12-12','81,3E,68,PK01,PK02,PK03,PK04,PK05,PKOK,8C,91,92,95,85,85Q,86,96,97,POP,9A,99',
//                                               '69,7P,9B,SP,80,9U,POF'";

//                return SQLHelper.ExecuteDataFill(Connection,
//                                                      System.Data.CommandType.Text,
//                                                      SQLText
//                                                      );
                string SQLText = @"sp_Query_PAK_PakingDailyReport";


                return SQLHelper.ExecuteDataFill(Connection,
                                                      System.Data.CommandType.StoredProcedure,
                                                      SQLText,
                                                      SQLHelper.CreateSqlParameter("@inputDate", inputDate),
                                                      SQLHelper.CreateSqlParameter("@FAStation", FAStation),
                                                      SQLHelper.CreateSqlParameter("@PAKStation", PAKStation)
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
