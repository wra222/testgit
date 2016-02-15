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
    
    public class FA_PrdIdOnLine : MarshalByRefObject, IFA_PrdIdOnLine
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
             IList<string> lstPdLine, string Station, string Pno, string Family, string ModelCategory)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
//                string SQLText = @" SELECT a.ProductID,a.CUSTSN,a.Model,a.DeliveryNo,a.PalletNo,d.ShipDate,b.Line,
//                                    RTRIM(b.Station)+' - '+c.Descr as Station,b.Editor,b.Udt,e.Family
//                                    FROM Product a (NOLOCK) INNER JOIN ProductStatus b  (NOLOCK)
//                                    ON a.ProductID=b.ProductID
//                                    INNER JOIN Station c (NOLOCK)
//                                    ON b.Station=c.Station
//                                    LEFT JOIN Delivery d (NOLOCK) on a.DeliveryNo=d.DeliveryNo
//                                    LEFT  JOIN Model e (NOLOCK) ON a.Model=e.Model
//                                    WHERE 1=1 AND b.Udt BETWEEN @FromDate AND @ToDate                                    
//                                   ";

                string SQLText = @" SELECT a.ProductID,a.CUSTSN,a.Model,a.DeliveryNo,a.PalletNo,a.ShipDate,a.Line,
                                    RTRIM(a.Station)+' - '+a.Descr as Station,a.Editor,a.Udt,a.Family
                                    FROM view_wip_status a
                                    WHERE 1=1 AND a.Udt BETWEEN @FromDate AND @ToDate                                    
                                   ";
                if (lstPdLine.Count != 0)
                {
                    //SQLText += "AND b.Line = '" +PdLine + "'";
                    SQLText += string.Format("AND a.Line IN ('{0}')", string.Join("','", lstPdLine.ToArray()));
                }

                if (Station!="")
                {
                    SQLText += "AND a.Station = '" + Station + "'";
                }

                if (Pno != "")
                {
                    SQLText += "AND a.Model = '" + Pno + "'";
                }
                
                if (Family!="")
                {
                    SQLText += "AND a.Family = '" + Family + "'";
                }

                if (ModelCategory != "")
                {
                    SQLText += " AND dbo.CheckModelCategory(a.Model,'" + ModelCategory + "')='Y' ";
                }

                //string SQL = string.Format(SQLText, FromDate.ToString("yyyy/MM/dd HH:mm:ss"), ToDate.ToString("yyyy/MM/dd HH:mm:ss"));                                  

                SQLText += "  ORDER BY a.Udt ";
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate",FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate",ToDate));
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
