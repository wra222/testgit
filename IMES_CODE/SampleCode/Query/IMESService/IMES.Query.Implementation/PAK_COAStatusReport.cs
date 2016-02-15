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
    public class PAK_COAStatusReport : MarshalByRefObject, IPAK_COAStatusReport
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /*public DataTable GetQueryResult(string Connection, string StartSN, string EndSN,
                DateTime StartDate, DateTime EndDate, string Status)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @" SELECT a.IECPN,count(COASN) as Qty,Status 
                                    FROM COAStatus a INNER JOIN COAReceive b                                            
                                    ON  a.IECPN=b.CustPN                                     
                                    GROUP BY a.IECPN,Status 
                                    ORDER BY a.IECPN
                                   ";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@StartDate", StartDate),
                                                 SQLHelper.CreateSqlParameter("@EndDate", EndDate));
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
        }*/

        public DataTable GetQueryResultByCOASN(string Connection, string StartSN, string EndSN)                
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @" SELECT a.COASN,c.CUSTSN,d.Description as Status,a.Line
                                                FROM COAStatus a (NOLOCK) 
                                                LEFT JOIN Product_Part b  (NOLOCK)
                                                ON a.COASN=b.PartSn 
                                                LEFT JOIN Product c (NOLOCK)
                                                ON b.ProductID=c.ProductID
                                                LEFT JOIN ConstValue d (NOLOCK)
                                                ON ( rtrim(a.Status)=Value and len(rtrim(d.Value))=2) 
                                                     OR (rtrim(a.Status)=left(d.Value,2) and a.Line=right(rtrim(d.Value),1) and len(rtrim(d.Value))>2 )                                                      
                                                WHERE a.COASN BETWEEN @StartSN AND @EndSN   
                                                UNION ALL
                                                SELECT a.COASN,c.CUSTSN,d.Description as Status,a.Line
                                                FROM COAStatus a (NOLOCK) 
                                                INNER JOIN Pizza_Part b  (NOLOCK)
                                                ON a.COASN=b.PartSn 
                                                INNER JOIN Product c (NOLOCK)
                                                ON b.PizzaID=c.PizzaID 
                                                LEFT JOIN ConstValue d (NOLOCK) 
                                                ON ( rtrim(a.Status)=Value and len(rtrim(d.Value))=2) 
                                                     OR (rtrim(a.Status)=left(d.Value,2) and a.Line=right(rtrim(d.Value),1) and len(rtrim(d.Value))>2 )                                                      
                                                WHERE a.COASN BETWEEN @StartSN AND @EndSN                       
                                   ";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@StartSN", 32, StartSN, ParameterDirection.Input),
                                                 SQLHelper.CreateSqlParameter("@EndSN", 32, EndSN, ParameterDirection.Input));
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

        public DataTable GetQueryResultByCOADate(string Connection, DateTime FromDate, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"SELECT a.IECPN,COUNT(DISTINCT c.COASN) as Qty, RTRIM(b.Name)+' --> '+ b.Name as Status,a.Line,a.Status+'-->'+b.Description as COAStatus
                                                FROM COALog c (NOLOCK) INNER JOIN COAStatus a (NOLOCK) ON c.COASN=a.COASN 
                                                INNER JOIN ConstValue b (NOLOCK) 
                                                ON ( rtrim(a.Status)=Value and len(rtrim(b.Value))=2) 
                                                     OR (rtrim(a.Status)=left(b.Value,2) and a.Line=right(rtrim(b.Value),1) and len(rtrim(b.Value))>2 ) 
                                                 AND Type='COAStatus'
                                                WHERE a.Udt BETWEEN @FromDate AND @ToDate
                                                GROUP BY a.IECPN,b.Name,a.Line,a.Status,b.Description
                                                ORDER BY a.IECPN,b.Name,b.Description
                                    ";


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


        public DataTable GetQueryResultDetailByCOADate(string Connection, DateTime FromDate, DateTime ToDate,
                string IECPN, string Line, string Status)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {                
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 "rpt_COAStatusReport",
                                                 SQLHelper.CreateSqlParameter("@Type", 30, "COADateDetail"),
                                                 SQLHelper.CreateSqlParameter("@IECPN", 32, IECPN),
                                                 SQLHelper.CreateSqlParameter("@Status", 20, Status),
                                                 SQLHelper.CreateSqlParameter("@Line", 20, Line),                                                 
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

        public DataTable GetQueryResultByCOAStatus(string Connection, string Status)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string[] arr = Status.Split(' ');
                //  string line = "";
                if (arr.Length == 2)
                { Status = arr[0].Trim() + arr[1].Trim(); }

                string SQLText = @"SELECT a.IECPN,COUNT(DISTINCT a.COASN) as Qty,RTRIM(a.Line)+' --> '+ b.Name as Status,a.Line,a.Status+'-->'+b.Description as COAStatus
                                                FROM  COAStatus a (NOLOCK) 
	                                                INNER JOIN ConstValue b (NOLOCK) 
                                                ON	( rtrim(a.Status)=Value and len(rtrim(b.Value))=2) 
		                                                OR (rtrim(a.Status)=left(b.Value,2) and a.Line=right(rtrim(b.Value),1) and len(rtrim(b.Value))>2 ) 
		                                                AND b.Type='COAStatus'
                                                WHERE	a.Udt BETWEEN DATEADD(MONTH,-1,GETDATE()) AND GETDATE()                                                    
                                   ";
                if (Status != "")
                {
                    if (arr.Length == 1)
                    {
                        SQLText += " AND rtrim(a.Status)= '" + Status + "'";
                    }
                    else
                    {
                        SQLText += " AND rtrim(a.Status)+rtrim(a.Line) = '" + Status + "'";
                    }

                }

                SQLText += " GROUP BY a.IECPN,b.Name,a.Line,a.Status,b.Description";

                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@Status", 20, Status));

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

        public DataTable GetQueryByCOAAllStatus(string Connection)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"SELECT COUNT('x') as Qty,b.Description
                                 INTO #Temp
                                 FROM COAStatus a (NOLOCK) INNER JOIN ConstValue b  (NOLOCK) 
                                 ON ( rtrim(a.Status)=Value and len(rtrim(b.Value))=2) 
                                                     OR (rtrim(a.Status)=left(b.Value,2) 
                                                     and a.Line=right(rtrim(b.Value),1) and len(rtrim(b.Value))>2 ) 
                                 AND Type='COAStatus'
                                 WHERE a.Udt BETWEEN DATEADD(MONTH,-1,GETDATE()) AND GETDATE()
                                 GROUP BY b.Description

                                INSERT  INTO #Temp
                                SELECT COUNT('x') as Qty,b.Description                              
                                 FROM COAStatus a (NOLOCK) INNER JOIN ConstValue b  (NOLOCK) on rtrim(a.Status)=left(b.Value,2) and a.Line=right(rtrim(b.Value),1) and len(rtrim(b.Value))>2 and Type='COAStatus'
                                 WHERE a.Udt BETWEEN DATEADD(MONTH,-1,GETDATE()) AND GETDATE()
                                 GROUP BY b.Description
        
                                 DECLARE @PivotColumnHeaders NVARCHAR(MAX)
                                 SELECT @PivotColumnHeaders = 
                                  COALESCE(
                                    @PivotColumnHeaders + ',[' + Description + ']',
                                    '[' + Description+ ']'
                                  )
                                 FROM ConstValue (NOLOCK)
                                 WHERE Type='COAStatus'
                                
                                 DECLARE @PivotTableSQL NVARCHAR(MAX)
                                 SET @PivotTableSQL = N'
                                  SELECT *
                                  FROM (
                                    SELECT * FROM #Temp
                                  ) AS X
                                  PIVOT 
                                  (
                                    SUM(Qty)
                                    FOR Description IN ('+ 
                                        @PivotColumnHeaders +'
                                    )
                                  ) AS PVT
                                '
                                
                                EXEC(@PivotTableSQL) 
                                ";

               return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.Text,
                                                SQLText);
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

        public DataTable GetQueryByCOARemind(string Connection, string Type)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText ="";

                if (Type != "4") //超過5天結合未出貨
                {
                    SQLText += @" SELECT COASN,IECPN,Line,Cdt,DATEDIFF(DAY,Cdt,GETDATE()) as OverDue FROM COAStatus (NOLOCK) ";

                    if (Type == "1")//超過30天
                    {
                        SQLText += "WHERE Status in ('A0','P0','P1') and DATEDIFF(DAY,Cdt,GETDATE())>30 ";
                    }
                    else if (Type == "2")//還有5天
                    {
                        SQLText += "WHERE Status in ('A0','P0','P1') and 30-DATEDIFF(DAY,Cdt,GETDATE())<=5 "; //滯留在廠內天數:30天为过期日期,江坤和王芳确认
                    }
                    else if (Type == "3")//還有10天
                    {
                        SQLText += "WHERE Status in ('A0','P0','P1') and 30-DATEDIFF(DAY,Cdt,GETDATE())<=10 ";//滯留在廠內天數:30天为过期日期,江坤和王芳确认
                    }
                    return SQLHelper.ExecuteDataFill(Connection,
                                 System.Data.CommandType.Text,
                                 SQLText);  
                }
                else //超過5天結合未出貨
                {
                    SQLText += @"rpt_COAStatusReport_NoShipment";
                    return SQLHelper.ExecuteDataFill(Connection,
                                 System.Data.CommandType.StoredProcedure,
                                 SQLText);
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

        public DataTable GetQueryResultByCOADailyCheck(string Connection,string Type, DateTime FromDate, DateTime ToDate)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";

                if (Type == "TRANS")
                {
                    SQLText += @" SELECT BegSN,EndSN,Qty,PO,Cdt FROM COAReceive (NOLOCK)
                             WHERE Cdt BETWEEN @FromDate AND @ToDate";

                    return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToDate));
                }
                else
                {
                    SQLText += @" CREATE TABLE #Result(HPPNBegNo VARCHAR(20),TotalEndNo VARCHAR(20),Qty INT)

                                  SELECT  DISTINCT IECPN,Line
                                  INTO #Temp
                                  FROM COAStatus (NOLOCK)
                                  WHERE Status=@Status 
                                  AND Udt BETWEEN @FromDate AND @ToDate
                                  ORDER BY 1

                                  WHILE ((SELECT COUNT('x') FROM #Temp)>0)
                                  BEGIN
                                        DECLARE @IECPN NVARCHAR(20),@Line NVARcHAR(10)

                                        SELECT TOP 1 @IECPN=IECPN,@Line=Line
                                        FROM #Temp
                                		
                                        INSERT INTO #Result
	                                    SELECT IECPN ,COUNT(IECPN),'0'
                                        FROM COAStatus (NOLOCK)
	                                    WHERE Status=@Status 
                                        AND Udt BETWEEN @FromDate AND @ToDate
	                                    GROUP BY IECPN
                                	
	                                    INSERT INTO #Result
	                                    EXEC rpt_COAWIP 'COADateDetail',@IECPN,@Status,@Line,@FromDate,@ToDate
    
	                                    DELETE FROM #Temp WHERE IECPN=@IECPN
                                  END

                                  SELECT * FROM #Result ; ";
                   

                    return SQLHelper.ExecuteDataFill(Connection,
                                                System.Data.CommandType.Text,
                                                SQLText,
                                                SQLHelper.CreateSqlParameter("@Status", 20, "A2"),
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
