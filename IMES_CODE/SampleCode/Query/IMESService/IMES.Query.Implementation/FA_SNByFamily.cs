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
    
    public class FA_SNByFamily : MarshalByRefObject, IFA_SNByFamily
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
             string Family, string Model, IList<string> lstPdLine, DateTime ShipDate, string ModelCategory)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"SELECT b.Family,a.Model,a.ProductID,a.CUSTSN,c.Udt,RTRIM(c.Station)+' - '+d.Descr as Station,c.Line
                                   FROM Product a (NOLOCK) INNER JOIN Model b (NOLOCK) ON a.Model=b.Model                                   
                                   INNER JOIN ProductStatus c (NOLOCK) ON a.ProductID=c.ProductID
                                   INNER JOIN Station d  (NOLOCK) ON c.Station=d.Station 
                                   WHERE 1=1 AND a.CUSTSN!='' AND c.Udt BETWEEN @FromDate AND @ToDate
                                   ";

                if (Family != "")
                {
                    SQLText += " AND b.Family = '" + Family + "'";
                }

                if (Model != "")
                {
                    SQLText += " AND a.Model = '" + Model + "'";
                }

                if (lstPdLine.Count != 0)
                {
                    SQLText += string.Format("AND c.Line IN ('{0}')", string.Join("','", lstPdLine.ToArray()));
                }

                DateTime tmp = new DateTime();
				
                if (ShipDate != tmp )
                {	//當有傳入ShipDate,搜尋已結合DN為傳入shipdate的機器
                    SQLText += "  AND a.DeliveryNo IN (SELECT DISTINCT DeliveryNo FROM Delivery (NOLOCK) WHERE ShipDate=@ShipDate ) ";

                }

                if (ModelCategory != "")
                {
                    SQLText += " AND dbo.CheckModelCategory(a.Model,'" + ModelCategory + "')='Y' ";
                }


                SQLText += "  ORDER BY c.Station ";
                //string SQL = string.Format(SQLText, FromDate.ToString("yyyy/MM/dd HH:mm:ss"), ToDate.ToString("yyyy/MM/dd HH:mm:ss"));


                if (ShipDate != tmp)
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                   System.Data.CommandType.Text,
                                                   SQLText,
                                                   SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                   SQLHelper.CreateSqlParameter("@ToDate", ToDate),
                                                   SQLHelper.CreateSqlParameter("@ShipDate", ShipDate));
                }
                else
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                   System.Data.CommandType.Text,
                                                   SQLText,
                                                   SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                   SQLHelper.CreateSqlParameter("@ToDate", ToDate));
               
               }
                
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
