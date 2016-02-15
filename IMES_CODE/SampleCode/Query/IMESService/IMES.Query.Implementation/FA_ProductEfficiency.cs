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
    public class FA_ProductEfficiency : MarshalByRefObject, IFA_ProductEfficiency
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                             string Family, IList<string> Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
    
            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("CREATE TABLE #period_time([startTime] datetime,[endtime] datetime ,lesson varchar(2),[D_N] varchar(2) ) ");
                sb.AppendLine("INSERT INTO #period_time([startTime],[endtime],[lesson],[D_N]) VALUES ");
                sb.AppendLine(string.Format("('{0} 08:00','{0} 13:00','1','d') ", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{0} 13:00','{0} 17:00','2','d')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{0} 17:00','{0} 20:30','3','d')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{0} 20:30','{0} 23:00','4','n')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{0} 23:00','{1} 03:00','5','n')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{1} 03:00','{1} 08:00','6','n')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine("");

                sb.AppendLine("SELECT a.* ");
                sb.AppendLine("INTO #base ");
                sb.AppendLine("FROM View_ProductEfficiency (nolock) a ");
                if (Family != "") {
                    sb.AppendLine(string.Format("INNER JOIN Model b ON a.Model = b.Model AND b.Family = '{0}'", Family));
                }
                sb.AppendLine(string.Format("WHERE a.Period = 'F' AND a.PeriodEnd_Date > '{0} 08:00' AND a.PeriodEnd_Date <= '{1} 08:00' ", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                if (Model.Count > 0)
                {
                    sb.AppendLine(string.Format("AND a.Model IN ('{0}')", string.Join("','", Model.ToArray())));
                }

                sb.AppendLine("SELECT SUBSTRING(c.Line,1,1) AS Line ,c.startTime,c.endtime,c.D_N, c.lesson ,d.Model,c.Station,Qty  "); 
                sb.AppendLine("INTO #rowdata ");
                sb.AppendLine("FROM  ");
                sb.AppendLine("(SELECT DISTINCT SUBSTRING(a.Line,1,1) AS Line , b.lesson,b.startTime,b.endtime ,b.D_N , c.Station  ");
                sb.AppendLine("FROM Line  a, #period_time b ,Station c  ");
                sb.AppendLine("WHERE a.Stage IN ('FA' ,'PAK') AND c.Station IN('40','65','85') ");
                sb.AppendLine(")  c ");
                sb.AppendLine("LEFT OUTER JOIN #base d (nolock)   ");
                sb.AppendLine("ON c.Line = SUBSTRING (d.Line,1,1)  ");
                sb.AppendLine("AND d.PeriodEnd_Date > c.startTime  ");
                sb.AppendLine("AND d.PeriodEnd_Date <= c.endtime ");
                sb.AppendLine("AND c.Station = d.Station ");
                sb.AppendLine("");

                sb.AppendLine("SELECt * into #Result FROM ( ");
                sb.AppendLine("SELECT Line,lesson, SUBSTRING(CONVERT(NVARCHAR(16),startTime,120),6,11)+ '~' + SUBSTRING(CONVERT(NVARCHAR(16),endtime,120),6,11) AS TimeRange,D_N, Station,ISNULL(SUM(Qty),0) AS Qty FROM #rowdata ");
                sb.AppendLine("GROUP BY Line,lesson,SUBSTRING(CONVERT(NVARCHAR(16),startTime,120),6,11) + '~' + SUBSTRING(CONVERT(NVARCHAR(16),endtime,120),6,11), D_N, Station ");
                sb.AppendLine(") ");
                sb.AppendLine("AS SourceTable ");
                sb.AppendLine("PIVOT ( ");
                sb.AppendLine("SUM(Qty)  ");
                sb.AppendLine("FOR Line IN ([A],[B],[C],[D],[E],[F],[G],[H],[J],[K],[L],[M],[R]) ");
                sb.AppendLine(" )");
                sb.AppendLine("AS PivotTable ");


                sb.AppendLine("SELECT lesson,TimeRange,D_N,RTRIM(a.Station) + ' - '+ b.Name AS Station, ");
                sb.AppendLine("[A],[B],[C],[D],[E],[F],[G],[H],[J],[K],[L],[M],[R] ");
                sb.AppendLine("FROM #Result a ");
                sb.AppendLine("LEFT JOIN Station b ON a.Station = b.Station ");
                sb.AppendLine("UNION ");
                sb.AppendLine("SELECT 'All',CASE D_N WHEN 'd' THEN '白班' WHEN 'n' THEN '夜班' ELSE D_N END ,D_N,RTRIM(a.Station) + ' - '+ b.Name AS Station, ");
                sb.AppendLine("sum([A]),sum([B]),sum([C]),sum([D]),sum([E]),sum([F]),sum([G]),sum([H]),sum([J]),sum([K]),sum([L]),sum([M]),sum([R]) ");
                sb.AppendLine("FROM #Result a ");
                sb.AppendLine("LEFT JOIN Station b ON a.Station = b.Station ");
                sb.AppendLine("group by D_N,RTRIM(a.Station) + ' - '+ b.Name ");
                sb.AppendLine("UNION ");
                sb.AppendLine("SELECT 'All','All','All',RTRIM(a.Station) + ' - '+ b.Name AS Station, ");
                sb.AppendLine("sum([A]),sum([B]),sum([C]),sum([D]),sum([E]),sum([F]),sum([G]),sum([H]),sum([J]),sum([K]),sum([L]),sum([M]),sum([R]) ");
                sb.AppendLine("FROM #Result a ");
                sb.AppendLine("LEFT JOIN Station b ON a.Station = b.Station ");
                sb.AppendLine("group by RTRIM(a.Station) + ' - '+ b.Name ");
                sb.AppendLine("ORDER BY D_N,Station,lesson ");



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

        public DataTable GetQueryDetail(string Connection, DateTime FromDate, DateTime ToDate,
                             string Family, IList<string> Model, IList<string> Station)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("CREATE TABLE #period_time([startTime] datetime,[endtime] datetime ,lesson varchar(2),[D_N] varchar(2) ) ");
                sb.AppendLine("INSERT INTO #period_time([startTime],[endtime],[lesson],[D_N]) VALUES ");
                sb.AppendLine(string.Format("('{0} 08:00','{0} 13:00','1','d') ", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{0} 13:00','{0} 17:00','2','d')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{0} 17:00','{0} 20:30','3','d')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{0} 20:30','{0} 23:00','4','n')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{0} 23:00','{1} 03:00','5','n')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine(string.Format(",('{1} 03:00','{1} 08:00','6','n')", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                sb.AppendLine("");

                sb.AppendLine("SELECT a.* ");
                sb.AppendLine("INTO #base ");
                sb.AppendLine("FROM View_ProductEfficiency (nolock) a ");
                if (Family != "")
                {
                    sb.AppendLine(string.Format("INNER JOIN Model b ON a.Model = b.Model AND b.Family = '{0}'", Family));
                }
                sb.AppendLine(string.Format("WHERE a.Period = 'F' AND a.PeriodEnd_Date > '{0} 08:00' AND a.PeriodEnd_Date <= '{1} 08:00' ", FromDate.ToString("yyyy-MM-dd"), ToDate.ToString("yyyy-MM-dd")));
                if (Model.Count > 0)
                {
                    sb.AppendLine(string.Format("AND a.Model IN ('{0}')", string.Join("','", Model.ToArray())));
                }

                sb.AppendLine("SELECT  c.Line ,c.startTime,c.endtime,c.D_N, c.lesson ,d.Model,c.Station,Qty  ");
                sb.AppendLine("INTO #rowdata ");
                sb.AppendLine("FROM  ");
                sb.AppendLine("(SELECT DISTINCT Line , b.lesson,b.startTime,b.endtime ,b.D_N , c.Station  ");
                sb.AppendLine("FROM Line  a, #period_time b ,Station c  ");
                sb.AppendLine(String.Format("WHERE a.Stage IN ('FA' ,'PAK') AND c.Station IN('{0}') ", string.Join("','",Station.ToArray())));
                sb.AppendLine(")  c ");
                sb.AppendLine("INNER JOIN #base d (nolock)   ");
                sb.AppendLine("ON c.Line = d.Line  ");
                sb.AppendLine("AND d.PeriodEnd_Date > c.startTime  ");
                sb.AppendLine("AND d.PeriodEnd_Date <= c.endtime ");
                sb.AppendLine("AND c.Station = d.Station ");
                sb.AppendLine("");

                sb.AppendLine("SELECt * FROM ( ");
                sb.AppendLine("SELECT Line,lesson,Station,Model, ISNULL(SUM(Qty),0) AS Qty   ");
                sb.AppendLine("FROM #rowdata  ");
                sb.AppendLine("GROUP BY Line,lesson,Station ,Model ");
                sb.AppendLine(")  ");
                sb.AppendLine("AS SourceTable ");
                sb.AppendLine("PIVOT ( ");
                sb.AppendLine("SUM(Qty) ");
                sb.AppendLine(string.Format("FOR Station IN ([{0}])  ", string.Join("],[", Station.ToArray())));
                 sb.AppendLine(") ");
                sb.AppendLine("AS PivotTable ");
                sb.AppendLine("ORDER BY lesson,Line,Model ");

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

        public DataTable GetQueryPeriodDetail(string Connection, DateTime PeriodFromDate, DateTime PeriodToDate,
                     string Family, IList<string> Model, IList<string> Station , IList<string> Line)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT Station, a.Model, Line, Qty ");
                sb.AppendLine("INTO #base ");
                sb.AppendLine("FROM View_ProductEfficiency (nolock) a  ");
                if (Family != "")
                {
                    sb.AppendLine(string.Format("INNER JOIN Model b ON a.Model = b.Model AND b.Family = '{0}'", Family));
                }
                sb.AppendLine(string.Format("WHERE a.Period = 'F' AND a.PeriodEnd_Date > '{0}' AND a.PeriodEnd_Date <= '{1}' ", PeriodFromDate.ToString("yyyy-MM-dd HH:mm:ss"), PeriodToDate.ToString("yyyy-MM-dd HH:mm:ss")));
                if (Model.Count > 0) {
                    sb.AppendLine(string.Format("AND a.Model IN ('{0}')", string.Join("','", Model.ToArray())));
                }
                if (Line.Count > 0)
                {
                    sb.AppendLine(string.Format("AND Line IN ('{0}')", string.Join("','", Line.ToArray())));
                }
                

                sb.AppendLine("SELECt * FROM (  ");
                sb.AppendLine("	SELECT * FROM #base ");
                sb.AppendLine(")   ");
                sb.AppendLine("AS SourceTable  ");
                sb.AppendLine("PIVOT (  ");
                sb.AppendLine("SUM(Qty)  ");
                sb.AppendLine(string.Format("FOR Station IN ([{0}])  ", string.Join("],[", Station.ToArray())));
                sb.AppendLine(")  ");
                sb.AppendLine("AS PivotTable  ");



                SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", PeriodFromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", PeriodToDate));
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

        public DataTable GetInputModel(string Connection, DateTime FromDate, DateTime ToDate, List<string> Family)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT DISTINCT(SUBSTRING(Model,1,5)) AS Model");
                sb.AppendLine("FROM View_ProductEfficiency (nolock)");
                sb.AppendLine("WHERE PeriodEnd_Date > @FromDate AND PeriodEnd_Date <= @ToDate");
                if (Family.Count > 0) { 
                    sb.AppendLine(string.Format("AND Family IN ('{0}')", string.Join("','", Family.ToArray())));
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
