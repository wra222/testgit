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

    public class PAK_ASTQuery : MarshalByRefObject,IPAK_ASTQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {

                string SQLText = @"select Code,
                                                RTRIM([Begin])+'~'+RTRIM([End]) as [Range],
                                                ISNULL(b.Value,'') as MaxNo
                                                from AssetRange a left join 
                                                (
                                                  select distinct NoName,Value from NumControl
                                                ) b
                                                on a.Code=b.NoName";

                return SQLHelper.ExecuteDataFill(Connection,
                                                      System.Data.CommandType.Text,
                                                      SQLText);

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
