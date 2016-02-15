using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
using System.Reflection;

namespace IMES.Query.Implementation
{
    public class SA_SAMBRepair : MarshalByRefObject, ISA_SAMBRepair
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.Debug(methodName + " GetMBRepairInfo begin");
            string strSQL =
                @"SELECT r.PCBNo,
                            r.PCBModelID,
                            r.Line,
                            RTRIM(r.Station)+' - '+s.Descr as Station,
                            r.Status,
                            r_info.Location ,
                            RTRIM(r_info.DefectCode)+ ' - '+ dc.Descr as Descr,                            
                            RTRIM(r_info.Cause)+' - '+di.Description as Cause,
                            RTRIM(r_info.Obligation) + ' - ' + di2.Description AS Obligation,
                            r_info.Remark,
                            r_info.Editor,                           
                            CONVERT(varchar(20), r.Cdt,120) AS Cdt,
                            CONVERT(varchar(20), r_info.Udt, 120) AS Udt
                    FROM PCBRepair r (NOLOCK) 
                    LEFT JOIN Station s (NOLOCK)  on r.Station=s.Station
                    LEFT JOIN PCBRepair_DefectInfo r_info (NOLOCK) on r.ID = r_info.PCARepairID
                    LEFT JOIN DefectCode dc (NOLOCK) on r_info.DefectCode = dc.Defect
                    LEFT JOIN DefectInfo di (NOLOCK) on r_info.Cause = di.Code AND di.Type='SACause'
                    LEFT JOIN DefectInfo di2 (NOLOCK) ON r_info.Obligation = di2.Code AND di2.Type = 'Obligation'
                WHERE r.Cdt BETWEEN @StartTime AND @EndTime                
                ORDER BY r.ID
                ";            

            //strSQL = string.Format(strSQL, StartTime.ToString("yyyy/MM/dd HH:MM:ss"), EndTime.ToString("yyyy/MM/dd HH:MM:ss"));
            logger.Debug(methodName + " GetMBRepairInfo strSQL=" + strSQL);

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                     strSQL, new SqlParameter("@StartTime", StartTime), new SqlParameter("@EndTime", EndTime));
            logger.Debug(methodName + " GetMBRepairInfo end");
            return dt;
        }

        public DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime, string Family, IList<string> Model, string InputStation)
        {
            IList<string> Station = new List<string>();
            Station.Add(InputStation);
            return GetMBRepairInfo(DBConnection, StartTime, EndTime, Family, Model, Station);
        }

        public DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime, string Family, IList<string> Model, IList<string> Station)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            try
            {
                //BaseLog.LoggingBegin(logger, methodName);
                logger.Debug(methodName + " GetMBRepairInfo begin");

                string SelectText = @"SELECT r.PCBNo,
                            r.PCBModelID,
                            r.Line,
                            RTRIM(r.Station)+' - '+s.Descr as Station,
                            r.Status,
                            r_info.Location ,
                            RTRIM(r_info.DefectCode)+ ' - '+ dc.Descr as Descr,                            
                            RTRIM(r_info.Cause)+' - '+di.Description as Cause,
                            RTRIM(r_info.Obligation) + ' - ' + di2.Description AS Obligation,
                            r_info.Remark,
                            r_info.Editor,                           
                            CONVERT(varchar(20), r.Cdt,120) AS Cdt,
                            CONVERT(varchar(20), r_info.Udt, 120) AS Udt,
                            Substring(r.PCBNo,1,2)  AS Model,
                             P.Family ";

                string FromText = @"FROM PCBRepair r (NOLOCK) 
                    LEFT JOIN Station s (NOLOCK)  on r.Station=s.Station
                    LEFT JOIN PCBRepair_DefectInfo r_info (NOLOCK) on r.ID = r_info.PCARepairID
                    LEFT JOIN DefectCode dc (NOLOCK) on r_info.DefectCode = dc.Defect
                    LEFT JOIN DefectInfo di (NOLOCK) on r_info.Cause = di.Code AND di.Type='SACause'
                    LEFT JOIN DefectInfo di2 (NOLOCK) ON r_info.Obligation = di2.Code AND di2.Type = 'Obligation'
                    LEFT JOIN ( SELECT DISTINCT 
						Replace(UPPER(InfoValue),'B SIDE','') AS Family ,PartNo
						FROM PartInfo(nolock) 
						WHERE InfoType = 'MDL' AND CHARINDEX('B SIDE', UPPER(InfoValue)) > 0 
                        ) P ON P.PartNo = r.PCBModelID  ";

                string WhereText = @" WHERE r.Cdt BETWEEN @StartTime AND @EndTime ";
                string OrderText = @" ORDER BY r.ID ";

                if (Station.Count != 0)
                {
                    WhereText += string.Format(" AND r.Station in ('{0}') ", string.Join("','", Station.ToArray()));
                }

                if (Family != "")
                {
                    WhereText += string.Format(" AND  P.Family = '{0}' ", Family);
                }

                if (Model.Count != 0)
                {
                    IList<string> MB_PC = new List<string>();
                    IList<string> MB_AIO = new List<string>();
                    //WhereText += string.Format(" and Substring(r.PCBNo,1,2) IN ('{0}') ", string.Join("','", Model.ToArray()));
                    for (int i = 0; i < Model.Count; i++)
                    {
                        if (Model[i].Length == 2)
                        {
                            MB_PC.Add(Model[i]);
                        }
                        else if (Model[i].Length == 3)
                        {
                            MB_AIO.Add(Model[i]);
                        }
                    }
                    WhereText += string.Format(" and ( Substring(r.PCBNo,1,2) IN ('{0}') ", string.Join("','", MB_PC.ToArray()));
                    WhereText += string.Format(" or Substring(r.PCBNo,1,3) IN ('{0}') )", string.Join("','", MB_AIO.ToArray()));
                }

                string strSQL = SelectText + FromText + WhereText + OrderText;
                //BaseLog.LoggingInfo(logger, strSQL);
                logger.Debug(methodName + " GetMBRepairInfo sql=" + strSQL);
                
                //strSQL = string.Format(strSQL, StartTime.ToString("yyyy/MM/dd HH:MM:ss"), EndTime.ToString("yyyy/MM/dd HH:MM:ss"));

                DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                         strSQL, new SqlParameter("@StartTime", StartTime), new SqlParameter("@EndTime", EndTime));
                return dt;
            }
            catch (Exception ex)
            {
                //BaseLog.LoggingError(logger, MethodBase.GetCurrentMethod(), ex);
                logger.Error(methodName + " GetMBRepairInfo err "+ ex.Message);
                throw;
            }
            finally
            {
                //BaseLog.LoggingEnd(logger, methodName);
                logger.Debug(methodName + " GetMBRepairInfo end");
            }

        }

    }
}

