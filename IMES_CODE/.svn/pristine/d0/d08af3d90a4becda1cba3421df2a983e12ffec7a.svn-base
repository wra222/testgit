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
    public class PAK_PAQCSortingQuery : MarshalByRefObject, IPAK_PAQCSortingQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetPAQCSortingQueryResult(string Connection,String CUSTSN)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(@"SELECT PAQCSortingID,CUSTSN,Status,Editor,Cdt
                                FROM PAQCSorting_Product (nolock)
                                WHERE PAQCSortingID in(
                                    SELECT PAQCSortingID 
                                    FROM PAQCSorting_Product (nolock)
                                    WHERE CUSTSN =@CUSTSN) order by PAQCSortingID");


                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@CUSTSN",10,CUSTSN));
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
