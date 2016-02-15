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
    public class FA_KeyPartDefectReport : MarshalByRefObject, IFA_KeyPartDefectReport
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetKeyPartDefectReport(string Connection, DateTime FromDate, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(@"SELECT ID, Family, MajorPart, Cdt, Line, CTNo, '' as DateCode,
                                Voder, Remark, RRmark, DeLoc, Editor, '' as 不良原因, '' as 责任归属, '' as 处理办法,
                                '' as 工程师签名,'' as  [厂商复判结果(SQE)],'' as  [处理办法(SQE)], '' as [处理时间(SQE)], '' as [工程师签名(SQE)],
                                PartNo  FROM KeyParts_Defect_RPT WITH(NOLOCK)");
                sb.AppendLine("WHERE Cdt BETWEEN @FromDate AND @ToDate ");


                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
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
