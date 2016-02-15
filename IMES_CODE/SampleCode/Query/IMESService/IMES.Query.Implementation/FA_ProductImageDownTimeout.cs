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
    public class FA_ProductImageDownTimeout : MarshalByRefObject, IFA_ProductImageDownTimeout
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string DBConnection, string Line, DateTime FromDate, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
               try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(@"create table #Product_maxtime(ID INT, ProductID varchar(20))");

                sb.AppendLine(string.Format(" insert  #Product_maxtime(ID,ProductID) SELECT MAX(ID), ProductID from ProductLog (nolock)  where  left(Line,1) in (select value from dbo.fn_split('{0}',','))  group by ProductID", Line));
                sb.AppendLine (@"select b.ProductID,a.Model,a.Line,a.Station,a.Cdt  into #11 from ProductLog a,#Product_maxtime b where a.ProductID=b.ProductID and a.ID=b.ID and a.Station='6P' and a.Cdt between @FromDate and @ToDate ");
                sb.AppendLine(string.Format("select * from #11 where Cdt not between GETDATE() and DATEadd(HH,-4,GETDATE()) and left(Line,1) in(select value from dbo.fn_split('{0}',','))", Line));
                sb.AppendLine(@"drop table #11
                                drop table #Product_maxtime");

                string SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(DBConnection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate));
//                string SQLText = @"INSERT Product_maxtime (Cdt, ProductID)
//                                   SELECT MAX(Cdt), ProductID from ProductLog  where Cdt between @FromDate and @ToDate   group by ProductID
//                                   select b.ProductID,a.Model,a.Line,a.Station,b.Cdt  into #11 from ProductLog a,Product_maxtime b where a.ProductID=b.ProductID and a.Cdt=b.Cdt and a.Station='6P'
//                                   select * from #11 where Cdt between GETDATE() and DATEadd(HH,-3,GETDATE()) and left(Line,1) in(select value from dbo.fn_split('{0}',','))
//                                   drop table #11
//                                   truncate table Product_maxtime ";
//                return SQLHelper.ExecuteDataFill(DBConnection,
//                                                                  System.Data.CommandType.Text,
//                                                                  SQLText,
//                                                                  SQLHelper.CreateSqlParameter("@Line",30, Line),
//                                                                  SQLHelper.CreateSqlParameter("@FromDate", FromDate),
//                                                                  SQLHelper.CreateSqlParameter("@ToDate", ToDate)); 

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
