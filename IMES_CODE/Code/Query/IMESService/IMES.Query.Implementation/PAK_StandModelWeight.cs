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

    public class PAK_StandModelWeight : MarshalByRefObject, IPAK_StandModelWeight
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetModelQueryResult(string Connection, string Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine( string.Format(@"select a.Model,b.Family,a.UnitWeight as StandWeight
                                   from (SELECT *FROM ModelWeight a(nolock) where Model in(select value from dbo.fn_split('{0}',','))) a ", Model));

                sb.AppendLine("left join Model b on  a.Model=b.Model");
               // sb.AppendLine(string.Format("on a.Model=b.Model and a.Model in(select value from dbo.fn_split('{0}',','))", Model));


                string SQLText = sb.ToString();
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
