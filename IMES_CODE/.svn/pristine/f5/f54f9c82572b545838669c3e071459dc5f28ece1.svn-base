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
    public class FA_PoWIPTracking : MarshalByRefObject, IFA_PoWIPTracking
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetQueryResultBySP(string Connection, string ShipDate, string FAStation, string PAKStation, string Model,
                                                                 string Line, string IsShiftLine, string grpType, string DBName, out int[] CountDNQty)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            CountDNQty = GetDNShipQty(Connection,  DateTime.Parse(ShipDate),DBName,Model);
            BaseLog.LoggingBegin(logger, methodName);
            try
            {

                if (grpType == "Model")
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.StoredProcedure,
                                                "sp_Query_FA_WipByDN",
                                                SQLHelper.CreateSqlParameter("@Shipdate", 64, ShipDate),
                                                SQLHelper.CreateSqlParameter("@FAStation", 1024, FAStation),
                                                SQLHelper.CreateSqlParameter("@PAKStation", 1024, PAKStation),
                                                SQLHelper.CreateSqlParameter("@Model", 1024, Model),
                                                SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                                SQLHelper.CreateSqlParameter("@IsShiftLine", 32, IsShiftLine));
                }
                else
                {
                    return SQLHelper.ExecuteDataFill(Connection,
                                                    System.Data.CommandType.StoredProcedure,
                                                    "sp_Query_FA_WipByDN2",
                                                    SQLHelper.CreateSqlParameter("@Shipdate", 64, ShipDate),
                                                    SQLHelper.CreateSqlParameter("@FAStation", 1024, FAStation),
                                                    SQLHelper.CreateSqlParameter("@PAKStation", 1024, PAKStation),
                                                    SQLHelper.CreateSqlParameter("@Model", 1024, Model),
                                                    SQLHelper.CreateSqlParameter("@Line", 1024, Line),
                                                    SQLHelper.CreateSqlParameter("@IsShiftLine", 32, IsShiftLine));
                }
               
 //               @Shipdate varchar(64),@FAStation varchar(max),@PAKStation varchar(max),@Model varchar(MAX),
 //@Line varchar(255),@IsShiftLine varchar(16)
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
        private int[] GetDNShipQty(string Connection, DateTime ShipDate, string DBName, string Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string subSql = " and Model not  like 'PC%'";
            
              
                if (DBName == "HPIMES")
                {
                    subSql = " and Model like 'PC%'";
                    
                }
                if (!string.IsNullOrEmpty(Model))
                {
                    subSql += " and Model in (select value from dbo.fn_split (@Model,',')) ";
                }

                DataSet ds = new DataSet();

                string SQLText = @" declare @s varchar(256)
                                                select @s=Value from SysSetting where Name='PAKStation'
                                                select ISNULL(SUM(Qty),0) as Qty from Delivery (NOLOCK) where ShipDate=@ShipDate  {0} 
                                                 union ALL 
                                                select COUNT(*) as Qty from ProductStatus (NOLOCK) where 
                                                 ProductID in
                                                 (
	                                                 select ProductID from Product  (NOLOCK) where DeliveryNo in
	                                                 (
                                                        select DeliveryNo from Delivery (NOLOCK) where ShipDate=@ShipDate
                                                       
                                                      )
                                                       {0} 
                                                 )
                                                 and Station in(select value from dbo.fn_split (@s,','))";

                SQLText = string.Format(SQLText, subSql);
                DataTable dt= SQLHelper.ExecuteDataFill(Connection,
                                                  System.Data.CommandType.Text,
                                                  SQLText,
                                                  SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                   SQLHelper.CreateSqlParameter("@Model",1024,Model));

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
        public DataSet GetDNShipQty(string Connection, DateTime ShipDate, string PoNo, string Model, string Process)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                DataSet ds = new DataSet();
                string SQLText = @"SELECT SUM(Qty) as TotalQty FROM Delivery (NOLOCK) WHERE ShipDate=@ShipDate ";

                if (PoNo != "")
                {
                    SQLText += "AND PoNo = '" + PoNo + "'";
                }

                if (Model != "")
                {
                    SQLText += "AND Model = '" + Model + "'";
                }

                ds.Tables.Add(SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                 SQLHelper.CreateSqlParameter("@Process", 32, Process, ParameterDirection.Input)));



                SQLText = "";
                SQLText += @"SELECT COUNT('x') as ActualQty FROM Product b (NOLOCK) WHERE  b.DeliveryNo IN 
                        (SELECT DISTINCT DeliveryNo FROM Delivery (NOLOCK) WHERE ShipDate=@ShipDate  ";

                if (PoNo != "")
                {
                    SQLText += "AND PoNo = '" + PoNo + "'";
                }

                if (Model != "")
                {
                    SQLText += "AND Model = '" + Model + "'";
                }

                SQLText += ")";

                ds.Tables.Add(SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                 SQLHelper.CreateSqlParameter("@Process", 32, Process, ParameterDirection.Input)));
                return ds;
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

        public DataTable GetQueryResult(string Connection, DateTime ShipDate, string PoNo,string Model,string Process)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                /*string SQLText = @"SELECT b.Model,c.Line ,e.Descr,count('x') as Input
                                   INTO #Temp 
                                   FROM Delivery a 
                                   INNER JOIN Product b ON a.Model=b.Model 
                                   INNER JOIN ProductStatus c ON b.ProductID=c.ProductID                                   
                                   INNER JOIN Station e ON c.Station=e.Station
                                   WHERE 1=1 AND a.ShipDate=@ShipDate
                                   ";

                if (PoNo != "")
                {
                    SQLText += "AND a.PoNo = '" + PoNo + "'";
                }

                if (Model != "")
                {
                    SQLText += "AND b.Model = '" + Model + "'";
                }

                
                SQLText += "GROUP BY b.Model,c.Line,e.Descr";*/
                string SQLText = @"SELECT b.Model,c.Line ,e.Descr,SUM(f.Qty) as MOQty,SUM(f.Print_Qty) as MOPrintQty,COUNT('x') as input
                                   INTO #Temp 
                                   FROM Product b  (NOLOCK)
                                   INNER JOIN ProductStatus c  (NOLOCK) ON b.ProductID=c.ProductID
                                   INNER JOIN Station e  (NOLOCK) ON c.Station=e.Station
                                   INNER JOIN MO f  (NOLOCK) on  b.MO=f.MO 
                                   WHERE 1=1 AND b.DeliveryNo='' AND 
                                    b.Model IN 
                                    (SELECT DISTINCT Model FROM Delivery WHERE ShipDate=@ShipDate ";

                if (PoNo != "")
                {
                    SQLText += "AND PoNo = '" + PoNo + "'";
                }

                if (Model != "")
                {
                    SQLText += "AND Model = '" + Model + "'";
                }

                SQLText += " ) GROUP BY b.Model,c.Line,e.Descr";

                SQLText += @" DECLARE @PivotColumnHeaders NVARCHAR(MAX)
                           SELECT @PivotColumnHeaders = 
                              COALESCE(
                                @PivotColumnHeaders + ',[' + Descr + ']',
                                '[' + Descr+ ']'
                              )
                           FROM Station (NOLOCK)
                           --WHERE OperationObject!='1'
                           WHERE StationType  LIKE 'FA%'
                           ORDER BY Station

                           DECLARE @PivotTableSQL NVARCHAR(MAX)
                           SET @PivotTableSQL = N'
                              SELECT *
                              FROM (
                                SELECT * FROM #Temp
                              ) AS X
                              PIVOT 
                              (
                                SUM(input)
                                FOR Descr IN ('+ 
		                            @PivotColumnHeaders +'
                                )
                              ) AS PVT
                            '      
                            EXEC(@PivotTableSQL)                      
                            ";


                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                 SQLHelper.CreateSqlParameter("@Process", 32, Process, ParameterDirection.Input));
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

        public DataTable GetSelectDetail(string Connection, DateTime ShipDate, string PoNo, string Model, string Process,string Station,string Line)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"SELECT b.ProductID,b.CUSTSN,e.Descr,c.Line,c.Editor
                                FROM Product b (NOLOCK) 
                                INNER JOIN ProductStatus c (NOLOCK) ON b.ProductID=c.ProductID
                                INNER JOIN Station e (NOLOCK) ON c.Station=e.Station
                                INNER JOIN MO f (NOLOCK) ON  b.MO=f.MO 
                                WHERE 1=1 AND b.DeliveryNo='' 
                                AND c.Station=@Station AND 
                                c.Line =@Line AND
                                b.Model IN 
                                (SELECT DISTINCT Model FROM Delivery WHERE ShipDate=@ShipDate ";                                                                

                if (PoNo != "")
                {
                    SQLText += "AND PoNo = '" + PoNo + "'";
                }

                if (Model != "")
                {
                    SQLText += "AND Model = '" + Model + "'";
                }

                SQLText += " ) ";


                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@ShipDate", ShipDate),
                                                 SQLHelper.CreateSqlParameter("@Station",32,Station,ParameterDirection.Input),
                                                 SQLHelper.CreateSqlParameter("@Line", 32, Line, ParameterDirection.Input),
                                                 SQLHelper.CreateSqlParameter("@Process", 32, Process, ParameterDirection.Input));
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

        public DataTable GetSelectDetail2(string Connection, string ShipDate, string Model, string Line, string Station,string DN)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            BaseLog.LoggingBegin(logger, methodName);
            DateTime DShipDate = DateTime.Now;
            try
            {
                string SQLText = @"select ProductID,CUSTSN,Model,DeliveryNo,Line,Station,StationDescr from view_wip_station
                                                 where Station=@Station ";
                if (!string.IsNullOrEmpty(Line))
                {
                    string[] lineArr = Line.Split(',');
                    if (lineArr[0].Length == 1)
                    { SQLText += string.Format(" and  SUBSTRING(Line,1,1) in (Select value from  fn_split('{0}',',') ) ", Line); }
                    else
                    { SQLText += string.Format(" and Line in (Select value from  fn_split('{0}',',') ) ", Line); }

                }

                if (string.IsNullOrEmpty(DN)) //FA Staition 
                {
                    SQLText += string.Format(" and Model in (Select value from  fn_split('{0}',',') ) ", Model);

                }
                else
                {
                    SQLText += " and DeliveryNo=@DN"; 
                }

              DataTable  dt = SQLHelper.ExecuteDataFill(Connection,
                                                       System.Data.CommandType.Text,
                                                       SQLText,
                                                       SQLHelper.CreateSqlParameter("@ShipDate", DShipDate),
                                                       SQLHelper.CreateSqlParameter("@Station", 32, Station),
                                                        SQLHelper.CreateSqlParameter("@DN", 32, DN),
                                                        SQLHelper.CreateSqlParameter("@Line", 32, Line));
              return dt;
                
                //if (!string.IsNullOrEmpty(ShipDate))
                //{
                //    DShipDate = DateTime.Parse(ShipDate);
                //    SQLText += " and Model in(Select Model from Delivery where ShipDate=@ShipDate )";
                //}
               
                //    if (!string.IsNullOrEmpty(Model))
                //    {

                //        SQLText += string.Format(" and Model in (Select value from  fn_split('{0}',',') ) ", Model);
                //    }
                
               
                //if (!string.IsNullOrEmpty(DN))
                //{
                //    SQLText += " and DeliveryNo=@DN";
                //}
              
                //DataTable dt;
                //if (!string.IsNullOrEmpty(ShipDate))
                //{
                //    dt = SQLHelper.ExecuteDataFill(Connection,
                //                                         System.Data.CommandType.Text,
                //                                         SQLText,
                //                                         SQLHelper.CreateSqlParameter("@ShipDate", DShipDate),
                //                                         SQLHelper.CreateSqlParameter("@Station",32, Station),
                //                                          SQLHelper.CreateSqlParameter("@DN", 32, DN),
                //                                               SQLHelper.CreateSqlParameter("@Line", 32, Line));

                //}
                //else
                //{
                //    dt = SQLHelper.ExecuteDataFill(Connection,
                //                                        System.Data.CommandType.Text,
                //                                        SQLText,
                //                                       SQLHelper.CreateSqlParameter("@Station", 32, Station),
                //                                        SQLHelper.CreateSqlParameter("@DN", 32, DN),
                //                                        SQLHelper.CreateSqlParameter("@Line", 32, Line));


                //}
             //   return dt;
                
           //     and Model in (SELECT DISTINCT Model FROM Delivery WHERE ShipDate='2012-03-29')";
            
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
        public DataTable GetQueryResult2(string Connection, string ShipDate, string Model, string Line, string StationList,string GroupType,string Family,string DnDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);
            DateTime DShipDate=DateTime.Now;
            try
            {
                string JoinDNtxt = "";

                string colStr = "SELECT ";
                string groupStr = "GROUP BY ";
                string SQLText = @"   
                                                  INTO #Temp 
                                	              from view_wip_station        
                                	              where 1=1 ";
                //string SQL2 = "";
                bool IsShiftLine = false;//Line does not include shift e.q. A,B,C.....
                if (!string.IsNullOrEmpty(Line))
                {
                    string[] lineArr = Line.Split(',');
                    if (lineArr[0].Length == 1)
                    { IsShiftLine = true; }
                }
               
                if (!string.IsNullOrEmpty(ShipDate))
                {
                 
                    DShipDate = DateTime.Parse(ShipDate);
                    SQLText += " and Model in(Select Model from Delivery where ShipDate=@ShipDate )  ";
                }
                else
                {
                    if (!string.IsNullOrEmpty(Model))
                    {

                        SQLText += string.Format("and Model in (Select value from  fn_split('{0}',',') ) ", Model);
                    }
                }
                if (GroupType == "ALL")
                {
                    //groupStr = groupStr + " Model,Line,";
                    colStr = colStr + " 'ALL' as Model,'ALL' as Line,";
                }
                else if (GroupType == "Model")
                {
                    groupStr = groupStr + " Model,";
                    colStr = colStr + " Model, 'ALL' as Line,";
                }
                else if (GroupType == "Line")
                {
                    if (IsShiftLine)
                    {
                        groupStr = groupStr + " SUBSTRING(Line,1,1),";
                        colStr = colStr + " 'ALL' as Model, SUBSTRING(Line,1,1) as Line,";
                    }
                    else
                    { 
                        groupStr = groupStr + " Line,";
                        colStr = colStr + " 'ALL' as Model, Line,";
                    }
                    
                    //SUBSTRING(Line,1,1)
                }
                else //Group by Model,Line
                {
                    if (IsShiftLine)
                    {
                        groupStr = groupStr + "Model, SUBSTRING(Line,1,1),";
                        colStr = colStr + " Model, SUBSTRING(Line,1,1) as Line,";
                    }
                    else
                    {
                        groupStr = groupStr + " Model,Line,";
                        colStr = colStr + " Model,Line, ";
                    }

                   
                }
                if (!string.IsNullOrEmpty(Line))
                {
                    
                    if (IsShiftLine) 
                    {
                        SQLText += string.Format("and SUBSTRING(Line,1,1) in (Select value from  fn_split('{0}',',') ) ", Line);
                    }
                    else
                    {
                        SQLText += string.Format("and Line in (Select value from  fn_split('{0}',',') ) ", Line);
                    }
                  //  SQLText += string.Format("and Line in (Select value from  fn_split('{0}',',') ) ", Line);
                }
              
                if (Family != "")
                {
                    SQLText += "AND Family  = '" + Family + "'";
                  
                }
                if (!string.IsNullOrEmpty(DnDate) && (GroupType == "Model" || GroupType == "Model+Line"))
                {
                    JoinDNtxt = @" DECLARE @PivotTableSQL NVARCHAR(MAX)
                                           SET @PivotTableSQL = N'
                                              SELECT * INTO #Temp2 
                                              FROM (
                                                SELECT * FROM #Temp
                                              ) AS X
                                              PIVOT 
                                              (
                                                SUM(input)
                                                FOR Station IN ('+ 
		                                            @PivotColumnHeaders +'
                                                )
                                              ) AS PVT ; select a.DeliveryNo as DN,b.* from 
                                                            (select * from Delivery where ShipDate=''{0}'' ) a right join   
                                                          #Temp2 b on a.Model=b.Model 
                                                            order by b.Model
                                            '      
                                            EXEC(@PivotTableSQL)   ";
                    JoinDNtxt = string.Format(JoinDNtxt, DnDate);
                }
                else
                {
                    JoinDNtxt = @" DECLARE @PivotTableSQL NVARCHAR(MAX)
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
                                                    EXEC(@PivotTableSQL)        ";
                    colStr = colStr.Replace("SELECT",  "SELECT  'ALL' as DN,  ");
                
                }
                colStr = colStr + " rtrim(Station) as Station, COUNT(*) AS input";
                groupStr = groupStr + " Station";

                SQLText = colStr + SQLText + groupStr;
                SQLText += @" DECLARE @PivotColumnHeaders NVARCHAR(MAX)
                           SELECT @PivotColumnHeaders = 
                              COALESCE(
                                @PivotColumnHeaders + ',[' + Station + ']',
                                '[' + Station+ ']'
                              )
                           FROM Station (NOLOCK)
                        
                          inner join  
                           fn_split  ('{0}',',') a
                           on Station=a.value
                           order by a.ID  ";
                SQLText += JoinDNtxt;
                           //DECLARE @PivotTableSQL NVARCHAR(MAX)
                           //SET @PivotTableSQL = N'
                           //   SELECT *
                           //   FROM (
                           //     SELECT * FROM #Temp
                           //   ) AS X
                           //   PIVOT 
                           //   (
                           //     SUM(input)
                           //     FOR Station IN ('+ 
                           //         @PivotColumnHeaders +'
                           //     )
                           //   ) AS PVT
                           // '      
                           // EXEC(@PivotTableSQL)                      
                           // ";

                SQLText = string.Format(SQLText, StationList);
                DataTable dt;
                if (!string.IsNullOrEmpty(ShipDate))
                {
                  dt=  SQLHelper.ExecuteDataFill(Connection,
                                                       System.Data.CommandType.Text,
                                                       SQLText,
                                                       SQLHelper.CreateSqlParameter("@ShipDate", DShipDate));
                
                }
                else
                {
                    dt = SQLHelper.ExecuteDataFill(Connection,
                                                      System.Data.CommandType.Text,
                                                      SQLText);
                
                }
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
    }
}
