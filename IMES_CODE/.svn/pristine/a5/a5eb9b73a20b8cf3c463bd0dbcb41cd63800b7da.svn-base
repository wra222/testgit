using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
using System.Reflection;
     

namespace IMES.Query.Implementation
{
   public class SA_OQCQuery: MarshalByRefObject,ISA_OQCQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetPCBOQCQuery(string DBConnection, DateTime StartTime, DateTime EndTime, string Status, IList<string> Cause,string Process)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT a.ActionName, a.PCBNo,c.PartNo, c.Descr as Family,a.Type, a.Line, a.FixtureID, a.Station, a.Status, a.JoinID, a.Editor, a.ErrorCode, a.Descr, a.Cdt, a.Remark ");
                sb.AppendLine("FROM PCBTestLog a ");
                sb.AppendLine("inner join PCB b on (a.PCBNo = b.PCBNo) ");
                sb.AppendLine("LEFT JOIN Part c on b.PCBModelID=c.PartNo ");
                sb.AppendLine("WHERE a.Cdt BETWEEN @FromDate AND @ToDate ");
                sb.AppendLine("and  a.Station = '31B'  ");
                if (!string.IsNullOrEmpty(Process))
                {
                    sb.AppendLine(string.Format("AND a.ActionName= '{0}' ", Process));

                }
                if (!string.IsNullOrEmpty(Status))
                {
                    sb.AppendLine(string.Format("AND a.ErrorCode= '{0}' ",Status ));

                }
                if (Cause.Count > 0 && Status=="NG")
                {
                    sb.AppendLine(string.Format("AND a.Descr in ('{0}') ", string.Join("','", Cause.ToArray())));
                }
               
                sb.AppendLine("ORDER BY a.Cdt ");
                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", StartTime),
                                                 SQLHelper.CreateSqlParameter("@ToDate", EndTime));
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
