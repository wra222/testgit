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
    public class FA_MPInputEx : MarshalByRefObject, IFA_MPInputEx
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime ShipDate,
                            IList<string> lstPdLine, string Family, IList<string> Model, string StationList, bool IsWithoutShift, string InputStation, bool grpModel, string ModelCategory)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
    
            try
            {
                string colStr = "SELECT ";
                string groupStr = "GROUP BY ";
                string SQLText = @"   INTO #Temp1
                                	  from view_FA_wip_station a 
                                              where   1=1   ";
                if (InputStation != "")
                {

                    SQLText += string.Format(" AND Station = '{0}' ) ", InputStation);
                }
                // SQLText += "ON a.ProductID = PL.ProductID ";
                string sqlDN = "select count(*) as DnQ,Model into #dn from view_FA_wip_station where ShipDate=@ShipDate group by Model";
                if (lstPdLine.Count != 0)
                {
                    //SQLText += "AND a.Line IN  '" + lstLine + "'";                    
                
                        SQLText += string.Format("AND SUBSTRING(a.Line,1,1) IN ('{0}')", string.Join("','", lstPdLine.ToArray()));
                        colStr = colStr + " SUBSTRING(a.Line,1,1) AS Line,";
                        groupStr = groupStr + " SUBSTRING(a.Line,1,1) ,";
                       sqlDN="select count(*) as DnQ,Model into #dn from view_FA_wip_station where ShipDate=@ShipDate And " +
                                     string.Format(" SUBSTRING(Line,1,1) IN ('{0}')", string.Join("','", lstPdLine.ToArray())) + " group by Model";
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
                else
                {
                    SQLText += @"  and a.Model in
                                     (select distinct Model from Delivery where ShipDate=@ShipDate) ";
                }

                if (grpModel)
                {
                    colStr = colStr + " 'All' AS Model, ";
                }
                else
                {
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

                SQLText += @"    Select SUM(Qty) as Qty,Model into #Temp2 from Delivery 
                                     where ShipDate=@ShipDate group by Model  
                                   select a.*,b.Qty into #Temp from #Temp1 a left join #Temp2 b 
                                  on a.Model=b.Model ";

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
                           END " + sqlDN +
                         
                         @"  DECLARE @PivotTableSQL NVARCHAR(MAX)
                           SET @PivotTableSQL = N'
                              SELECT * into #Piva
                              FROM (
                                SELECT * FROM #Temp
                              ) AS X
                              PIVOT 
                              (
                                SUM(input)
                                FOR Station IN ('+ 
		                            @PivotColumnHeaders +'
                                )
                              ) AS PVT  ";
                SQLText+= @"; Update #Piva set [CombineDN]=f.DnQ from #Piva r
						                inner join #dn f
					                on r.Model=f.Model 
                                            Update #Piva set [CombineDN]=ISNULL([CombineDN],0)
                                            select * from #Piva order by Model,Family,Line; ' EXEC(@PivotTableSQL)   ;";
                SQLText = string.Format(SQLText, StationList);
                DataTable dt = SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@ShipDate", ShipDate));
                return dt;
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

        //public DataTable GetSelectDetail(string Connection, DateTime ShipDate,
        //                    IList<string> lstPdLine, string Family, IList<string> lstModel, string Line, string Station, bool IsWithoutShift) { 
        //    return GetSelectDetail( Connection,  ShipDate,
        //                    lstPdLine,  Family, lstModel,  Line,  Station,  IsWithoutShift);
        //}

        public DataTable GetSelectDetail(string Connection, DateTime ShipDate,
                            IList<string> lstPdLine, string Family, IList<string> lstModel, string Line, string Station, bool IsWithoutShift)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT ROW_NUMBER() OVER (ORDER BY a.ProductID DESC)  AS ID, a.ProductID, a.CUSTSN, a.PCBID,a.Family, a.Model, ");
                sb.AppendLine("a.PalletNo,CONVERT(nvarchar(11), a.ShipDate,111) AS ShipDate, a.Station, a.[Status], a.Descr,a.Line as StartLine,a.StationLine as Line, ");
                sb.AppendLine("CONVERT(varchar(20), a.Cdt, 120) AS Cdt, CONVERT(varchar(20), a.Udt, 120) AS Udt, a.Editor ");
                sb.AppendLine("FROM view_FA_wip_station a where 1=1  ");
                //sb.AppendLine(" and ShipDate=@ShipDate ");

                //if (InputStation != "")
                //{
                //    sb.AppendLine("INNER JOIN (SELECT DISTINCT ProductID FROM  ProductLog WITH(NOLOCK) WHERE Cdt BETWEEN @FromDate AND @ToDate ) AS PL ");
                //}
                if (Station != "")
                {
                   // sb.AppendLine(string.Format("  AND Station = '{0}'  ", Station));
                    //in (select value from     fn_split('36',',') )
                    if (Station == "CombineDN")
                    {  sb.AppendLine(" AND ShipDate=@ShipDate");}
                    else {  sb.AppendLine(string.Format(" AND Station  in (select value from     fn_split('{0}',',') )  ", Station));}
                   
                }
                

                //if (Station == "") {
                //    sb.AppendLine(string.Format("and 1=1 ", Station));
                //} else {
                //    sb.AppendLine(string.Format("WHERE a.Station IN ('{0}') ", Station));
                //}
                
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

                    sb.Append("AND Model in (select Model from Delivery where ShipDate=@ShipDate) ");
                }                    
                else
                {
                    sb.Append(string.Format("AND a.Model IN ('{0}') ", string.Join("','", lstModel.ToArray())));
                }
                //if (!(Line == "" || Line.ToUpper() == "ALL"))
                //{
                //    sb.Append(string.Format("AND SUBSTRING(a.Line,1,1) IN ('{0}')", string.Join("','", Line.ToArray())));
                //}
                //if (!(Line == "" || Line.ToUpper() == "ALL"))
                //{
                //    if (IsWithoutShift)
                //    {
                //        sb.Append(string.Format("AND SUBSTRING(a.Line,1,1) IN  ('{0}') ", Line));
                //    }
                //    else
                //    {
                //        sb.Append(string.Format("AND a.Line IN  ('{0}') ", Line));
                //    }
                //}
                if (lstPdLine.Count != 0)
                {
                    sb.Append(string.Format("AND SUBSTRING(a.Line,1,1) IN ('{0}')", string.Join("','", lstPdLine.ToArray())));
                }
                //if (lstPdLine.Count != 0)
                //{                 
                //    if (IsWithoutShift)
                //    {
                //        sb.Append(string.Format("AND SUBSTRING(a.Line,1,1) IN ('{0}')", string.Join("','", lstPdLine.ToArray())));
                //    }
                //    else
                //    {
                //        sb.Append(string.Format("AND a.Line IN ('{0}')", string.Join("','", lstPdLine.ToArray())));
                //    }
                //}




                //if (StationList != "") {
                //    sb.Append(string.Format("AND a.Station in ('{0}')", StationList.Replace(",", "','")));
                //}

                sb.AppendLine(" ORDER BY a.ProductID DESC");



                string SQLText = sb.ToString();


                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
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


        public int[] GetDNShipQty(string Connection, DateTime ShipDate, string Model, string PrdType)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string subSql = "and dbo.CheckModelCategory(Model,'{0}')='Y'";
                subSql = string.Format(subSql, PrdType);
                string stationList = PrdType == "PC" ? "PAKStation" : "PAKStation_RCTO";
                if (!string.IsNullOrEmpty(Model))
                {
                    subSql += " and Model in (select value from dbo.fn_split (@Model,',')) ";
                }
                DataSet ds = new DataSet();

                string SQLText = @" declare @s varchar(256)
                                                select @s=Value from SysSetting  WITH(NOLOCK) where Name='{1}'
                                                select ISNULL(SUM(Qty),0) as Qty from Delivery (NOLOCK) 
                                            where ShipDate=@ShipDate  {0} 
                                                 union ALL 
                                              
                                              select COUNT(*) as Qty from view_wip_station_PAK3
                                               where ShipDate=@ShipDate   {0}";

                SQLText = string.Format(SQLText, subSql, stationList);
                DataTable dt;
                if (!string.IsNullOrEmpty(Model))
                {
                    dt = SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                   SQLHelper.CreateSqlParameter("@Model", Model));
                }
                else
                {
                    dt = SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate));
                }


                int[] inArr = new int[2];
                inArr[0] = int.Parse(dt.Rows[0][0].ToString());
                inArr[1] = int.Parse(dt.Rows[1][0].ToString());
                return inArr;

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
