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
    public class FA_MPInput : MarshalByRefObject, IFA_MPInput
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, IList<string> Model, string StationList, bool IsWithoutShift, string InputStation, bool grpModel, string ModelCategory)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
    
            try
            {
                string colStr = "SELECT ";
                string groupStr = "GROUP BY ";

//                string SQLText = @" SELECT a.Line,c.Family,b.Model,d.Descr,COUNT(*) AS input
//                                    INTO #Temp 
//	                                FROM ProductStatus a (NOLOCK) inner join Product b (NOLOCK) ON a.ProductID=b.ProductID
//	                                INNER JOIN Model c (NOLOCK) ON b.Model=c.Model
//	                                INNER JOIN Station d (NOLOCK) ON a.Station=d.Station
//	                                WHERE 1=1 AND a.Udt BETWEEN @FromDate AND @ToDate
//                                   ";
//                string SQLText = @"   
//                                                    INTO #Temp 
//                	                                FROM ProductStatus a (NOLOCK) inner join Product b (NOLOCK) ON a.ProductID=b.ProductID
//                	                                INNER JOIN Model c (NOLOCK) ON b.Model=c.Model
//                	                                INNER JOIN Station d (NOLOCK) ON a.Station=d.Station
//                	                                WHERE 1=1 AND a.Udt BETWEEN @FromDate AND @ToDate
//                                                   ";
                string SQLText = @"   INTO #Temp 
                                	  from view_wip_station a 
                                                         
                                    ";
                if (InputStation == "")
                {
                    SQLText += "INNER JOIN (SELECT DISTINCT ProductID FROM  ProductLog WITH(NOLOCK) WHERE Cdt BETWEEN @FromDate AND @ToDate ) AS PL ";
                }
                else {
                    SQLText += string.Format("INNER JOIN (SELECT DISTINCT ProductID FROM  ProductLog WITH(NOLOCK) WHERE Cdt BETWEEN @FromDate AND @ToDate AND Station = '{0}' ) AS PL ", InputStation);
                }
                SQLText += "ON a.ProductID = PL.ProductID ";

                if (lstPdLine.Count != 0)
                {
                    //SQLText += "AND a.Line IN  '" + lstLine + "'";                    
                    if (IsWithoutShift)
                    {
                        SQLText += string.Format("AND SUBSTRING(a.Line,1,1) IN ('{0}')", string.Join("','", lstPdLine.ToArray()));
                        colStr = colStr + " SUBSTRING(a.Line,1,1) AS Line,";
                        groupStr = groupStr + " SUBSTRING(a.Line,1,1) ,";
                    }
                    else {
                        SQLText += string.Format("AND a.Line IN ('{0}')", string.Join("','", lstPdLine.ToArray()));
                        colStr = colStr + " a.Line,";     
                        groupStr = groupStr + " a.Line,";

                    }
                }
                else
                {
                    colStr = colStr + " 'ALL' as Line,";

                }

                if (Family != "")
                {
                    SQLText += "AND a.Family  = '" + Family + "'";
                    colStr = colStr + " a.Family,";
                    groupStr = groupStr + " a.Family,";
                }
                else
                {
                    colStr = colStr + " 'ALL' as Family,";
                }

                if (Model.Count > 0)
                {
                    SQLText += "AND a.Model  IN ('" + string.Join("','", Model.ToArray()) + "')";
                }
                if (grpModel)
                {
                    colStr = colStr + " 'All' AS Model, ";
                }
                else {
                    colStr = colStr + " a.Model,";
                    groupStr = groupStr + " a.Model,";
                }

                if (ModelCategory != "")
                {
                    SQLText += " AND dbo.CheckModelCategory(a.Model,'" + ModelCategory + "')='Y' ";
                }

                colStr = colStr + " rtrim(a.Station) as Station, ISNULL(COUNT(*),0) AS input";
                groupStr = groupStr + " a.Station";

                SQLText = colStr + SQLText + groupStr;
                //SQLText += " GROUP BY a.Line,c.Family ,b.Model ,d.Descr";

                SQLText += @" 
                            DECLARE @PivotColumnHeaders NVARCHAR(MAX)
                            SET @PivotColumnHeaders = ''

                           SELECT @PivotColumnHeaders = 
                              COALESCE(
                                @PivotColumnHeaders + ',[' + RTRIM(value) + ']',
                                '[' + RTRIM(value)+ ']'
                              )
                           FROM fn_split  ('{0}',',')
                           ORDER BY ID

                           IF LEN(@PivotColumnHeaders) > 1
                           BEGIN
                                SELECT @PivotColumnHeaders   = SUBSTRING(@PivotColumnHeaders,2,LEN(@PivotColumnHeaders)-1)
                           END

                           DECLARE @PivotTableSQL NVARCHAR(MAX)
                           SET @PivotTableSQL = N'
                              SELECT *
                              FROM (
                                SELECT * FROM #Temp
                              ) AS X
                              PIVOT 
                              (
                                SUM(input)
                                FOR Station IN ('+ 
		                            @PivotColumnHeaders +'
                                )
                              ) AS PVT
                            '      
                            EXEC(@PivotTableSQL)                      
                            ";
                SQLText = string.Format(SQLText, StationList);

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

        public DataTable GetPoModel(string Connection, DateTime FromDate, DateTime ToDate, string Family, string Station) 
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("SELECT DISTINCT b.Model FROM ProductLog a WITH(NOLOCK) ");
                sb.AppendLine("LEFT JOIN Product b WITH(NOLOCK) ON a.ProductID = b.ProductID ");
                if (Family != "")
                {
                    sb.AppendLine(string.Format("INNER JOIN Model c WITH(NOLOCK) ON b.Model = c.Model AND c.Family = '{0}' ", Family));
                }
                sb.AppendFormat("WHERE a.Cdt BETWEEN @FromDate AND @ToDate ", Station);
                if (Station != "")
                {
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

        public DataTable GetSelectDetail(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, IList<string> lstModel, string Line, string Station, bool IsWithoutShift, string InputStation) { 
            return GetSelectDetail( Connection,  FromDate,  ToDate,
                            lstPdLine,  Family, lstModel,  Line,  Station,  IsWithoutShift,  InputStation, "");
        }

        public DataTable GetSelectDetail(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, IList<string> lstModel, string Line, string Station, bool IsWithoutShift, string InputStation, string StationList )
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT ROW_NUMBER() OVER (ORDER BY a.ProductID DESC)  AS ID, a.ProductID, a.CUSTSN, a.PCBID,a.Family, a.Model, ");
                sb.AppendLine("a.PalletNo,CONVERT(nvarchar(11), a.ShipDate,111) AS ShipDate, a.Station, a.[Status], a.Descr, a.Line, ");
                sb.AppendLine("CONVERT(varchar(20), a.Cdt, 120) AS Cdt, CONVERT(varchar(20), a.Udt, 120) AS Udt, a.Editor ");
                sb.AppendLine("FROM view_wip_status a ");

                //sb.AppendLine("INNER JOIN ProductLog PL ON a.ProductID = PL.ProductID AND PL.Cdt BETWEEN @FromDate AND @ToDate ");
                //if (InputStation != "") {
                //    sb.AppendLine(string.Format("AND PL.Station = '{0}'", InputStation));
                //}


                if (InputStation == "")
                {
                    sb.AppendLine("INNER JOIN (SELECT DISTINCT ProductID FROM  ProductLog WITH(NOLOCK) WHERE Cdt BETWEEN @FromDate AND @ToDate ) AS PL ");
                }
                else
                {
                    sb.AppendLine(string.Format("INNER JOIN (SELECT DISTINCT ProductID FROM  ProductLog WITH(NOLOCK) WHERE Cdt BETWEEN @FromDate AND @ToDate AND Station = '{0}' ) AS PL ", InputStation));
                }
                sb.AppendLine("ON a.ProductID = PL.ProductID ");

                if (Station == "") {
                    sb.AppendLine(string.Format("WHERE 1=1 ", Station));
                } else {
                    sb.AppendLine(string.Format("WHERE a.Station IN ('{0}') ", Station));
                }
                
                if (!(Family == "" || Family.ToUpper() == "ALL"))
                {
                    sb.Append(string.Format("AND a.Family IN ('{0}') ", Family));
                }

                //if (!(Model == "" || Model.ToUpper() == "ALL"))
                //{
                //    sb.Append(string.Format("AND a.Model IN ('{0}') ", Model));
                //}
                if (lstModel.Count == 0 || lstModel[0].ToUpper() == "ALL")
                {
                    //Nothing To Do!!
                }                    
                else
                {
                    sb.Append(string.Format("AND a.Model IN ('{0}') ", string.Join("','", lstModel.ToArray())));
                }

                if (!(Line == "" || Line.ToUpper() == "ALL"))
                {
                    if (IsWithoutShift)
                    {
                        sb.Append(string.Format("AND SUBSTRING(a.Line,1,1) IN  ('{0}') ", Line));
                    }
                    else
                    {
                        sb.Append(string.Format("AND a.Line IN  ('{0}') ", Line));
                    }
                }

                if (lstPdLine.Count != 0)
                {                 
                    if (IsWithoutShift)
                    {
                        sb.Append(string.Format("AND SUBSTRING(a.Line,1,1) IN ('{0}')", string.Join("','", lstPdLine.ToArray())));
                    }
                    else
                    {
                        sb.Append(string.Format("AND a.Line IN ('{0}')", string.Join("','", lstPdLine.ToArray())));
                    }
                }

                if (StationList != "") {
                    sb.Append(string.Format("AND a.Station in ('{0}')", StationList.Replace(",", "','")));
                }

                sb.AppendLine(" ORDER BY a.ProductID DESC");



                string SQLText = sb.ToString();


                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate),
                                                 SQLHelper.CreateSqlParameter("@Station", 32, Station, ParameterDirection.Input),
                                                 SQLHelper.CreateSqlParameter("@Line", 32, Line, ParameterDirection.Input),
                                                 SQLHelper.CreateSqlParameter("@Family", 32, Family, ParameterDirection.Input));
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
