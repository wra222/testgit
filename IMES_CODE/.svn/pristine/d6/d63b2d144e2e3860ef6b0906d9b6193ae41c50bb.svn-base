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
    public class FA_ProductStatement : MarshalByRefObject, IFA_ProductStatement
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, string Station, IList<string> Model, bool IsWithoutShift, string ModelCategory)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("WITH base AS( ");
                sb.AppendLine("SELECT DISTINCT a.ProductID, b.CUSTSN ");
                sb.AppendLine("FROM ProductLog a  (NOLOCK) ");
                sb.AppendLine("INNER JOIN Product b (NOLOCK) ON b.ProductID = a.ProductID ");
                if (Family != ""){
                    sb.AppendFormat("INNER JOIN Model c (NOLOCK) ON c.Model = b.Model AND c.Family = '{0}' ", Family);
                }else{
                    sb.AppendLine("INNER JOIN Model c (NOLOCK) ON c.Model = b.Model ");
                }
                
                sb.AppendLine("WHERE  a.Cdt BETWEEN @FromDate AND @ToDate ");
                if (Station != "") {
                    sb.AppendFormat("AND a.Station = '{0}' ", Station);                
                }
		        if (lstPdLine.Count > 0)
                {
                    if (IsWithoutShift)
                    {
                        sb.AppendFormat("AND SUBSTRING(a.Line,1,1) in ('{0}') ", string.Join("','", lstPdLine.ToArray()));
                    }
                    else
                    {
                        sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", lstPdLine.ToArray()));
                    }
                }
                if(Model.Count > 0){
                    sb.AppendFormat("AND b.Model in ('{0}') ", string.Join("','", Model.ToArray()));
                }
                if (ModelCategory != "")
                {
                    sb.AppendFormat(" AND dbo.CheckModelCategory(b.Model,'" + ModelCategory + "')='Y' ");
                }

                sb.AppendLine(") ");
                sb.AppendLine("SELECT ROW_NUMBER()OVER(ORDER BY a.ProductID) AS [No], a.ProductID, m.Family, a.CUSTSN, mi.Value as 'Desc', d.Model , b.Station + '' + c.Name AS Station, ");
                sb.AppendLine("CASE b.Status WHEN '1' THEN 'PASS' WHEN '0' THEN 'FAIL' ELSe CONVERT(varchar(5),b.Status) END AS [Status], b.Line, b.Editor, b.Udt ");
                sb.AppendLine("FROM base a (NOLOCK) ");
                sb.AppendLine("LEFT JOIN ProductStatus b (NOLOCK) ON a.ProductID = b.ProductID ");
                sb.AppendLine("LEFT JOIN Station (NOLOCK) c ON b.Station = c.Station ");
                sb.AppendLine("LEFT JOIN Product (NOLOCK) d ON a.ProductID = d.ProductID ");
                sb.AppendLine("LEFT JOIN Model (NOLOCK) m ON d.Model = m.Model ");
                sb.AppendLine("LEFT JOIN ModelInfo (NOLOCK) mi ON d.Model = mi.Model and mi.Name='MN2' ");

                SQLText = sb.ToString();
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

        public DataTable GetQueryResultByModel(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, string Station, IList<string> Model, bool IsWithoutShift, string ModelCategory)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();
               
                sb.AppendLine("SELECT DISTINCT a.* ");
                sb.AppendLine("INTO #base");
                sb.AppendLine("FROM ProductLog a  (NOLOCK) ");
                sb.AppendLine("INNER JOIN Product b (NOLOCK) ON b.ProductID = a.ProductID ");
                if (Family != "")
                {
                    sb.AppendFormat("INNER JOIN Model c (NOLOCK) ON c.Model = b.Model AND c.Family = '{0}' ", Family);
                }
                else
                {
                    sb.AppendLine("INNER JOIN Model c (NOLOCK) ON c.Model = b.Model ");
                }

                sb.AppendLine("WHERE  a.Cdt BETWEEN @FromDate AND @ToDate ");
                if (Station != "")
                {
                    sb.AppendFormat("AND a.Station = '{0}' ", Station);
                }
                if (lstPdLine.Count > 0)
                {
                    if (IsWithoutShift)
                    {
                        sb.AppendFormat("AND SUBSTRING(a.Line,1,1) in ('{0}') ", string.Join("','", lstPdLine.ToArray()));
                    }
                    else
                    {
                        sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", lstPdLine.ToArray()));
                    }
                }
                if (Model.Count > 0)
                {
                    sb.AppendFormat("AND b.Model in ('{0}') ", string.Join("','", Model.ToArray()));
                }
                if (ModelCategory != "")
                {
                    sb.AppendFormat(" AND dbo.CheckModelCategory(b.Model,'" + ModelCategory + "')='Y' ");
                }

                sb.Append("SELECT DISTINCT d.Model,a.ProductID , ");
                if (IsWithoutShift) {
                    sb.AppendLine(" SUBSTRING(b.Line,1,1) AS Line, ");
                }
                else {
                    sb.AppendLine(" b.Line AS Line, ");
                }
                sb.AppendLine("CASE ISNULL(b.Station,'0') WHEN '0' THEN 0 ELSE 1 END AS F0input, ");
                sb.AppendLine("CASE ISNULL(c.Station,'0') WHEN '0' THEN 0 ELSE 1 END AS Boardinput ");
                sb.AppendLine("INTO #ProID ");
                sb.AppendLine("FROM #base a ");
                sb.AppendLine("LEFT JOIN ProductLog b ON a.ProductID = b.ProductID AND b.Station in ('F0','B1') ");
                sb.AppendLine("LEFT JOIN ProductLog c ON a.ProductID = c.ProductID AND c.Station = '40' ");
                sb.AppendLine("LEFT JOIN Product d ON d.ProductID = a.ProductID ");
               
                sb.AppendLine("SELECT a.Model, ");
                sb.AppendLine("SUM(CASE WHEN a.Station IN ('6A') THEN 1 ELSE 0 END ) AS EPIAInput, ");
                sb.AppendLine("SUM(CASE WHEN a.Station IN ('6A') AND a.Status = '0' THEN 1 ELSE 0 END ) AS EPIAFail, ");
                sb.AppendLine("SUM(CASE WHEN a.Station = '71' THEN 1 ELSE 0 END) AS PIAInput, ");
                sb.AppendLine("SUM(CASE WHEN a.Station = '79A' AND Status = '0' THEN 1 ELSE 0 END) AS PIAFail, ");
                //sb.AppendLine("'--' AS PIAInput,--SUM(CASE WHEN b.Tp = 'PIA1' THEN 1 ELSE 0 END) AS PIATest, ");
                //sb.AppendLine("'--' AS PIAFail,--SUM(CASE WHEN b.Tp = 'PIA1' AND Status = '7' THEN 1 ELSE 0 END) AS PIAPass, ");
                sb.AppendLine("SUM(CASE WHEN a.Station IN ('PO') THEN 1 ELSE 0 END ) AS PAQCInput, ");
                sb.AppendLine("SUM(CASE WHEN a.Station IN ('PO') AND a.Status = '0' THEN 1 ELSE 0 END ) AS PAQCFail ");
                sb.AppendLine("INTO #qc ");
                sb.AppendLine("FROM #base a (nolock) ");                
                sb.AppendLine("GROUP By a.Model	");

                sb.AppendLine("SELECT  a.Model,  a.Line , SUM(a.F0input) AS [F0InputSum], SUM(a.Boardinput) AS [BoardInputSum] , ");
                sb.AppendLine("e.EPIAInput,e.EPIAFail,'' AS EPIAFRate,e.PIAInput,e.PIAFail,'' AS PIAFRate, e.PAQCInput,e.PAQCFail ,'' AS PAQCFRate ");
                sb.AppendLine("FROM #ProID a (NOLOCK) ");
                sb.AppendLine("LEFT JOIN #qc (NOLOCK) e ON a.Model = e.Model ");
                sb.AppendLine("GROUP BY  a.Model,a.Line,e.EPIAInput,e.EPIAFail,e.PIAInput,e.PIAFail,e.PAQCInput,e.PAQCFail ");
                sb.AppendLine("ORDER BY Model, a.Line ");



                SQLText = sb.ToString();
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

        public DataTable GetInputModel(string Connection, DateTime FromDate, DateTime ToDate, string Family, string Station)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT DISTINCT b.Model FROM ProductLog a ");
                sb.AppendLine("LEFT JOIN Product b ON a.ProductID = b.ProductID ");
                if (Family != "")
                {
                    sb.AppendLine(string.Format("INNER JOIN Model c ON b.Model = c.Model AND c.Family = '{0}' ", Family));
                }
                sb.AppendFormat("WHERE a.Cdt BETWEEN @FromDate AND @ToDate ", Station);
                if (Station != "") {
                    sb.AppendLine(string.Format("AND a.Station = '{0}'", Station));
                }
                string SQLText = string.Format(sb.ToString(), Family);
                    
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
