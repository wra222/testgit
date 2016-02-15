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
    public class FA_ProductRepairQuery : MarshalByRefObject, IFA_ProductRepairQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetProductRepairQueryResult(string Connection, DateTime FromDate, DateTime ToDate, String SN)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                if (SN == "")
                {
                    sb.AppendLine(@"SELECT a.OldPartSno,a.NewPartSno,c.CUSTSN,a.MajorPart,d.Family,a.Cdt,a.Udt FROM
                                    dbo.ProductRepair_DefectInfo a,
                                    ProductRepair b,Product c (nolock),dbo.Model d
                                    WHERE a.Udt BETWEEN @FromDate AND @ToDate and a.OldPartSno<>''
                                   and a.ProductRepairID=b.ID and b.ProductID=c.ProductID AND c.Model=d.Model ");
                }
                else
                {
                    sb.AppendLine(string.Format(@"SELECT OldPartSno,NewPartSno,c.CUSTSN,a.MajorPart,d.Family,a.Cdt,a.Udt FROM dbo.ProductRepair_DefectInfo a,ProductRepair b,Product c,Model d 
                                                where a.OldPartSno<>'' and a.ProductRepairID=b.ID and b.ProductID=c.ProductID and c.Model=d.Model
                                                and c.ProductID in(select value from dbo.fn_split('{0}',','))
                                                union
                                                SELECT OldPartSno,NewPartSno,c.CUSTSN,a.MajorPart,d.Family,a.Cdt,a.Udt FROM dbo.ProductRepair_DefectInfo a,ProductRepair b,Product c,Model d 
                                                where a.OldPartSno<>'' and a.ProductRepairID=b.ID and b.ProductID=c.ProductID and c.Model=d.Model
                                                and c.CUSTSN in(select value from dbo.fn_split('{0}',','))", SN));
                }

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