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
    public class FA_EPIAModelList: MarshalByRefObject, IFA_EPIAModelList
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                             string Model)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @" SELECT a.Line,b.Model,d.Descr ,COUNT(*) AS input
                                    INTO #Temp 
	                                FROM ProductStatus a  WITH(NOLOCK) inner join Product b  WITH(NOLOCK) ON a.ProductID=b.ProductID
	                                INNER JOIN Model c ON b.Model=c.Model
	                                INNER JOIN Station d ON a.Station=d.Station                              
	                                WHERE 1=1 AND a.Cdt BETWEEN @FromDate AND @ToDate
                                   ";                
                if (Model != "")
                {
                    SQLText += "AND b.Model = '" + Model + "'";
                }

                SQLText += " GROUP BY a.Line,b.Model,d.Descr";

                SQLText += @" DECLARE @PivotColumnHeaders NVARCHAR(MAX)
                           SELECT @PivotColumnHeaders = 
                              COALESCE(
                                @PivotColumnHeaders + ',[' + Descr + ']',
                                '[' + Descr+ ']'
                              )
                           FROM Station
                           WHERE OperationObject=@Process
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
