﻿using System;
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
    public class FA_PIADefectList : MarshalByRefObject, IFA_PIADefectList
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataTable GetQueryResult(string Connection, DateTime FromRepairDate, DateTime ToRepairDate,
              string Station, string Defect, string Cause, List<string> PdLine)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            BaseLog.LoggingBegin(logger, methodName);

            try
            {
                string SQLText = "";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT distinct a.ProductID, f.Model, a.Line,b.OldPart,b.OldPartSno,b.NewPart,b.NewPartSno, ");
                sb.AppendLine("RTRIM(g.Station)+' - '+e.Descr as Station,b.Editor, ");
                sb.AppendLine("c.Defect + '' + c.Descr as Defect,d.Code + '' + d.Description as Cause, b.Remark, a.Cdt ");
                sb.AppendLine("FROM ProductRepair a (NOLOCK) ");
                sb.AppendLine("LEFT JOIN ProductRepair_DefectInfo b (NOLOCK) ON a.ID=b.ProductRepairID ");
                sb.AppendLine("LEFT JOIN DefectCode c (NOLOCK) ON b.DefectCode=c.Defect ");
                sb.AppendLine("LEFT JOIN DefectInfo d (NOLOCK) ON b.Cause=d.Code AND d.Type='FACause' ");                
                sb.AppendLine("LEFT JOIN Product f (NOLOCK) ON a.ProductID = f.ProductID ");
                sb.AppendLine("INNER JOIN dbo.ProductTestLog g (NOLOCK) ON g.ID = a.LogID ");
                sb.AppendLine("LEFT JOIN Station e (NOLOCK) ON a.Station=g.Station ");
                sb.AppendLine("WHERE a.Cdt BETWEEN @FromDate AND @ToDate ");
                if (PdLine.Count > 0)
                {
                    sb.AppendFormat("AND a.Line IN ('{0}') ", string.Join("','", PdLine.ToArray()));
                }
                if (Station != "")
                {
                    sb.AppendFormat("AND g.Station = '{0}' ", Station);
                }

                if (Defect != "")
                {
                    sb.AppendFormat("AND c.Defect = '{0}' ", Defect);
                }

                if (Cause != "")
                {
                    sb.AppendFormat("AND d.Code = '{0}' ", Cause);
                }
                sb.AppendLine(" ORDER BY a.Cdt");

                SQLText = sb.ToString();
                return SQLHelper.ExecuteDataFill(Connection,
                                                 System.Data.CommandType.Text,
                                                 SQLText,
                                                 SQLHelper.CreateSqlParameter("@FromDate", FromRepairDate),
                                                 SQLHelper.CreateSqlParameter("@ToDate", ToRepairDate));                                            
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

        public DataTable GetQueryResult(string Connection, DateTime FromRepairDate, DateTime ToRepairDate,
              List<string> Station, List<string> Defect, List<string> Cause, List<string> PdLine, string ModelCategory) 
        {
                  string methodName = MethodBase.GetCurrentMethod().Name;

                  BaseLog.LoggingBegin(logger, methodName);

                  try
                  {
                      string SQLText = "";
                      StringBuilder sb = new StringBuilder();
                      //sb.AppendLine("SELECT a.ProductID, f.Model, a.Line, RTRIM(g.Station)+' - '+e.Descr as Station,");
                      //sb.AppendLine("b.OldPart,b.OldPartSno,b.NewPart,b.NewPartSno, b.Editor, ");
                      //sb.AppendLine("c.Defect + '' + c.Descr as Defect,d.Code + '' + d.Description as Cause, b.Remark, a.Cdt ");
                      //sb.AppendLine("FROM ProductRepair a (NOLOCK) ");
                      //sb.AppendLine("LEFT JOIN ProductRepair_DefectInfo b (NOLOCK) ON a.ID=b.ProductRepairID ");
                      //sb.AppendLine("LEFT JOIN DefectCode c (NOLOCK) ON b.DefectCode=c.Defect ");
                      //sb.AppendLine("LEFT JOIN DefectInfo d (NOLOCK) ON b.Cause=d.Code AND d.Type='FACause' ");
                      //sb.AppendLine("LEFT JOIN Product f (NOLOCK) ON a.ProductID = f.ProductID ");
                      //sb.AppendLine("INNER JOIN dbo.ProductTestLog g (NOLOCK) ON g.ID = a.LogID ");
                      //sb.AppendLine("LEFT JOIN Station e (NOLOCK) ON a.Station=g.Station ");
                      //sb.AppendLine("WHERE a.Cdt BETWEEN @FromDate AND @ToDate ");
                      //if (PdLine.Count > 0)
                      //{
                      //    sb.AppendLine(string.Format("AND a.Line IN ('{0}') ", string.Join("','", PdLine.ToArray())));
                      //}
                      //if (Station.Count > 0)
                      //{
                      //    sb.AppendLine(string.Format("AND g.Station IN ('{0}') ", string.Join("','", Station.ToArray())));
                      //}

                      //if (Defect.Count > 0)
                      //{
                      //    sb.AppendLine(string.Format("AND c.Defect IN ('{0}') ", string.Join("','", Defect.ToArray())));
                      //}

                      //if (Cause.Count > 0 )
                      //{
                      //    sb.AppendLine(string.Format("AND d.Code IN ('{0}') ", string.Join("','", Cause.ToArray())));
                      //}
                      //sb.AppendLine(" ORDER BY a.Cdt");

                      sb.AppendLine("SELECT distinct a.ProductID, mo.Family, pro.Model,	SUBSTRING(CONVERT(varchar(8),DATEADD(HH,-8,c.Udt),112 ),5,2) AS [Month], ");
			           sb.AppendLine("DATEPART(wk, DATEADD(HH,-8,c.Udt)) AS [Week], substring(b.Line,1,1) AS Line, substring(b.Line,3,1) AS Shift, ");
			           sb.AppendLine("a.Station, isnull(c.DefectCode,h.DefectCodeID) AS Defect, c.Cause, c.MajorPart, c.Remark, c.OldPart, c.OldPartSno, c.NewPart, c.NewPartSno, c.Editor, c.Udt ");
                       sb.AppendLine("FROM ProductTestLog a WITH(NOLOCK) ");
                       sb.AppendLine("inner JOIN ProductTestLog_DefectInfo h WITH(NOLOCK) ON a.ID=h.ProductTestLogID");
                       sb.AppendLine("LEFT JOIN ProductRepair b WITH(NOLOCK) ON a.ID = b.TestLogID ");  
                       if (PdLine.Count > 0){
                           sb.Append(string.Format("AND b.Line IN ('{0}') ", string.Join("','",PdLine.ToArray())));
                       }

                       sb.AppendLine("Inner JOIN ProductRepair_DefectInfo c WITH(NOLOCK) ON c.ProductRepairID = b.ID ");
                       if (Defect.Count > 0){
                           sb.Append(string.Format("AND c.DefectCode IN ('{0}') ", string.Join("','", Defect.ToArray())));
                       }
                       if(Cause.Count > 0){
                           sb.Append(string.Format("AND c.Cause IN ('{0}') ", string.Join("','", Cause.ToArray())));
                       }
                      
                       sb.AppendLine("LEFT JOIN DefectCode deco (NOLOCK) ON c.DefectCode=deco.Defect ");
                       sb.AppendLine("LEFT JOIN DefectInfo deinf (NOLOCK) ON c.Cause=deinf.Code AND deinf.Type='FACause' ");
                       sb.AppendLine("inner JOIN Product pro (NOLOCK) ON a.ProductID = pro.ProductID ");
                       sb.AppendLine("LEFT JOIN Station sta (NOLOCK) ON a.Station = sta.Station ");
                       sb.AppendLine("inner JOIN Model mo (NOLOCK) ON pro.Model = mo.Model ");
                       sb.AppendLine("WHERE a.Status = 0 AND a.Cdt BETWEEN @FromDate AND @ToDate ");
                       if (Station.Count > 0)
                       {
                           sb.AppendLine(string.Format("AND a.Station IN ('{0}')", string.Join("','", Station.ToArray())));
                       }

                       if (ModelCategory != "")
                       {
                           sb.AppendLine(" AND dbo.CheckModelCategory(pro.Model,'" + ModelCategory + "')='Y' ");
                       }

                       sb.AppendLine("ORDER BY a.ProductID DESC ");

                      SQLText = sb.ToString();
                      return SQLHelper.ExecuteDataFill(Connection,
                                                       System.Data.CommandType.Text,
                                                       SQLText,
                                                       SQLHelper.CreateSqlParameter("@FromDate", FromRepairDate),
                                                       SQLHelper.CreateSqlParameter("@ToDate", ToRepairDate));
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
