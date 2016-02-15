using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data;
using log4net;
using System.Reflection;
using IMES.Query.DB;

namespace IMES.Query.Implementation
{
    public class FA_LocQuery : MarshalByRefObject,IFA_LocQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetLocRinDown(string Connection, string Station, string QueryType, string FromDate, string ToDate, string Line, string List,IList<string> model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {


                StringBuilder sb = new StringBuilder();
                sb.AppendLine("exec sp_Query_FA_LocQuery @Station,@QueryType,@BeginTime,@EndTime,@Line,@list,@Model ");
                DataTable dt = new DataTable();
                dt = SQLHelper.ExecuteDataFill(Connection,
                                             System.Data.CommandType.Text,
                                             sb.ToString(),
                                              SQLHelper.CreateSqlParameter("@Station", Station.Trim()),
                                              SQLHelper.CreateSqlParameter("@QueryType", QueryType.Trim()),
                                                 SQLHelper.CreateSqlParameter("@BeginTime", FromDate),
                                                    SQLHelper.CreateSqlParameter("@EndTime", ToDate),
                                                     SQLHelper.CreateSqlParameter("@Line", Line),
                                                     SQLHelper.CreateSqlParameter("@list", List),
                                                     SQLHelper.CreateSqlParameter("@Model", string.Join("','", model.ToArray()))
                                              );
               
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
    }
}
