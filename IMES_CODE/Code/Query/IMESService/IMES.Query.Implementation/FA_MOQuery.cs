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
    public class FA_MOQuery : MarshalByRefObject, IFA_MOQuery
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetMOQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                             string Family, IList<string> Model, IList<string> MO, string ProductType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("WITH TravelCard_Info as ( ");
                sb.AppendLine("SELECt b.Model,a.InfoValue,COUNT(a.ProductID) AS Travel_ShipQty FROM ProductInfo (nolock) a ");
                sb.AppendLine("LEFT JOIN Product b on a.ProductID= b.ProductID ");
                sb.AppendLine(string.Format("WHERE InfoType = 'ShipDate' AND (InfoValue between '{0}' AND '{1}' OR InfoValue between '{2}' AND '{3}')", FromDate.ToString("yyyy/MM/dd"), ToDate.ToString("yyyy/MM/dd"), FromDate.ToString("yyyy/M/d"), ToDate.ToString("yyyy/M/d")));
                sb.AppendLine("GROUP BY b.Model,a.InfoValue)  ");
                sb.AppendLine("    ,Delivery_Info AS( ");
                sb.AppendLine("SELECT Model,ShipDate,SUM(Qty) AS DN_ShipQty FROM Delivery WITH(NOLOCK) ");
                sb.AppendLine("WHERE ShipDate BETWEEN  @FromDate AND @ToDate ");
                sb.AppendLine("GROUP BY Model,ShipDate) ");

                sb.AppendLine("SELECT ROW_NUMBER()OVER(ORDER BY a.MO) AS ID, ");
                sb.AppendLine("a.MO, a.Model, CONVERT(varchar(10),a.CreateDate,111) AS [CreateDate], ");
                sb.AppendLine("CONVERT(varchar(10),a.StartDate,111) AS [StartDate], a.SAPStatus,a.Qty AS SAP_Qty, a.Print_Qty AS SAP_PrintQty, ");
                sb.AppendLine("a.CustomerSN_Qty AS SAP_CustSNQty, ISNULL(b.Travel_ShipQty,0) AS Travel_ShipQty, ISNULL(c.DN_ShipQty,0) AS DN_ShipQty ,a.Status ");
                sb.AppendLine("FROM MO a WITH(NOLOCK) ");
                sb.AppendLine("LEFT JOIN TravelCard_Info b WITH(NOLOCK) on a.Model = b.Model and CONVERT(varchar(10),a.StartDate,111) = b.InfoValue ");
                sb.AppendLine("LEFT JOIN Delivery_Info c WITH(NOLOCK) on a.Model = c.Model AND a.StartDate = c.ShipDate ");
                if (Family == "") {                    
                }
                else {
                    sb.AppendLine(string.Format("INNER JOIN Model d ON a.Model = d.Model AND d.Family = '{0}' ", Family));
                }
                sb.AppendLine("WHERE a.Status != 'C' AND StartDate BETWEEN @FromDate AND @ToDate ");
                if (Model.Count > 0) {
                    sb.AppendLine(string.Format("AND a.Model IN ('{0}') ", string.Join("','", Model.ToArray())));
                }

                if (MO.Count > 0) { 
                    sb.AppendLine(string.Format("AND a.MO IN ('{0}') ", string.Join("','", MO.ToArray())));
                }
                if (MO.Count > 0)
                {
                    sb.AppendLine(string.Format("AND a.MO IN ('{0}') ", string.Join("','", MO.ToArray())));
                }
                if (ProductType != "")
                {
                    sb.AppendLine(string.Format("AND dbo.CheckModelCategory (a.Model,'{0}')='{1}' ", ProductType,"Y"));
                }
                sb.AppendLine("ORDER BY MO ");

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
        public DataTable GetMO(string Connection, DateTime FromDate, DateTime ToDate,
                        string Family, IList<string> Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT DISTINCT a.MO FROM MO a WITH(NOLOCK)");
                if (Family == "") {
                    sb.AppendLine("INNER JOIN Model b ON a.Model = b.Model ");
                }
                else {
                    sb.AppendLine(string.Format("INNER JOIN Model b ON a.Model = b.Model AND b.Family = '{0}' ", Family)); 
                }
                sb.AppendLine("WHERE StartDate BETWEEN @FromDate AND @ToDate ");
                if (Model.Count > 0)
                {
                    sb.AppendLine(string.Format("AND a.Model IN ('{0}') ",string.Join("','",Model.ToArray())));
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
        public DataTable GetMOModel(string Connection, DateTime FromDate, DateTime ToDate,
                        string Family)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT DISTINCT a.Model FROM MO a WITH(NOLOCK) ");
                if (Family == "") {
                    sb.AppendLine("INNER JOIN Model b WITH(NOLOCK) ON a.Model = b.Model ");
                }
                else {
                    sb.AppendLine(string.Format("INNER JOIN Model b WITH(NOLOCK) ON a.Model = b.Model AND b.Family = '{0}' ", Family)); 
                }
	            
                sb.AppendLine("WHERE StartDate BETWEEN @FromDate AND @ToDate ");

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
