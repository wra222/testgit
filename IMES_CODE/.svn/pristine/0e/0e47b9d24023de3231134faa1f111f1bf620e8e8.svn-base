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
    public class FA_ProductYield : MarshalByRefObject, IFA_ProductYield
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

     
        public DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
            List<string> Station, string Family, IList<string> lstPdLine, string Model, List<string> lstStation, string ModelCategory)
        {

                      string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string colStr = "SELECT ";
                string groupStr = " GROUP BY ";

//                string SQLText = @"SELECT c.Family,b.Model,a.Line,a.Station+d.Descr as Station,
//                                   COUNT(*) AS InputQty ,  
//                                   SUM( CASE WHEN a.Status=0 THEN 1 ELSE 0 END ) AS DefectQty                          
//                                   FROM ProductLog a (NOLOCK)
//                                   INNER JOIN Product b (NOLOCK) on a.ProductID=b.ProductID
//                                   INNER JOIN Model c (NOLOCK) on b.Model=c.Model
//                                   INNER JOIN Station d (NOLOCK) ON a.Station=d.Station 
//                                   WHERE 1=1 AND a.Cdt BETWEEN @FromDate AND @ToDate
//                                   ";
                string SQLText = @"
                               
                                    ba.Station+ba.Descr as Station,
                                   --COUNT(*) AS InputQty ,  
                                    SUM (CASE WHEN a.Line IS NULL THEN 0 ELSE 1 END ) AS InputQty ,
                                   SUM( CASE WHEN a.Status=0 THEN 1 ELSE 0 END ) AS DefectQty                          
                                   FROM #base ba
                                   LEFT JOIN  ProductLog a (NOLOCK) ON a.Station = ba.Station AND a.Line = ba.Line 	
			                                                                        AND a.Cdt BETWEEN @FromDate AND @ToDate 
                                   LEFT JOIN Product b (NOLOCK) on a.ProductID=b.ProductID
                                   LEFT JOIN Model c (NOLOCK) on b.Model=c.Model
                                   LEFT JOIN Station d (NOLOCK) ON a.Station=d.Station 
                                   WHERE 1=1 
                                   ";
                //if (Station != "")
                //{
                //    SQLText += "AND ba.Station = '" + Station + "'";
                //}

                
              
                if (Family != "")
                {
                    SQLText += "AND c.Family = '" + Family + "'";
                    colStr = colStr + " c.Family,";
                    groupStr = groupStr + " c.Family,";
                }
                else
                {
                   // colStr = colStr + " 'ALL' as Family,";
                    colStr = colStr + " c.Family as Family,";
                    groupStr = groupStr + " c.Family,";
                }


                if (Model != "")
                {
                    SQLText += "AND b.Model = '" + Model + "'";
                    colStr = colStr + "  b.Model,";
                    groupStr = groupStr + " b.Model,";
                }
                else
                {
                    colStr = colStr + " 'ALL' as Model,";
                   
                }


                if (lstPdLine.Count != 0)
                {
                    //SQLText += "AND a.Line = '" + Line + "'";
                    SQLText += string.Format("AND ba.Line IN ('{0}')", string.Join("','", lstPdLine.ToArray()));
                    colStr = colStr + "  ba.Line,";
                    groupStr = groupStr + " ba.Line,";
                }
                else
                {
                    colStr = colStr + " 'ALL' as Line,";
                }
                if (Station.Count > 0)
                {
                    SQLText += string.Format("AND ba.Station IN ('{0}')", string.Join("','", Station.ToArray()));
                    colStr = colStr + "  ba.Station,";
                   // groupStr = groupStr + " ba.Station,";
                }
                else
                {
                    colStr = colStr + " 'ALL' as Station,";
                }
                if (ModelCategory != "")
                {
                    SQLText += " AND dbo.CheckModelCategory(b.Model,'" + ModelCategory + "')='Y' ";
                }

                groupStr = groupStr + " ba.Station,ba.Descr";
                SQLText = colStr + SQLText + groupStr;

                //SQLText += "GROUP BY c.Family,b.Model,a.Station,a.Line,d.Descr";

                SQLText += "  ORDER BY ba.Station ";

                SQLText = String.Format("WITH #base AS (SELECT a.Station, b.Line,a.Descr FROM Station a,Line b WHERE Station IN ('{0}')) " , string.Join("','",lstStation.ToArray())).ToString() + SQLText;
                //SQLText += "  ORDER BY b.Model "; 
                //string SQL = string.Format(SQLText, FromDate.ToString("yyyy/MM/dd HH:mm:ss"), ToDate.ToString("yyyy/MM/dd HH:mm:ss"));


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

        public DataTable GetDetailQueryByInput(string Connection, string InputBegDate, string InputEndDate, string Family,
                                                    string Model, string Line, string Station, string Category)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"IMES_ProductYield_InputDetail";
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@InputBegDate", InputBegDate),
                                                 SQLHelper.CreateSqlParameter("@InputEndDate", InputEndDate),
                                                 SQLHelper.CreateSqlParameter("@Family", Family),
                                                 SQLHelper.CreateSqlParameter("@Model", Model),
                                                 SQLHelper.CreateSqlParameter("@Line", Line),
                                                 SQLHelper.CreateSqlParameter("@Station", Station),
                                                 SQLHelper.CreateSqlParameter("@Category", Category));
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

        public DataTable GetDetailQueryByDefect(string Connection, string InputBegDate, string InputEndDate, string Family,
                                                    string Model, string Line, string Station, string Category)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = @"IMES_ProductYield_DefectDetail";
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.StoredProcedure,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@InputBegDate", InputBegDate),
                                                 SQLHelper.CreateSqlParameter("@InputEndDate", InputEndDate),
                                                 SQLHelper.CreateSqlParameter("@Family", Family),
                                                 SQLHelper.CreateSqlParameter("@Model", Model),
                                                 SQLHelper.CreateSqlParameter("@Line", Line),
                                                 SQLHelper.CreateSqlParameter("@Station", Station),
                                                 SQLHelper.CreateSqlParameter("@Category", Category));
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
