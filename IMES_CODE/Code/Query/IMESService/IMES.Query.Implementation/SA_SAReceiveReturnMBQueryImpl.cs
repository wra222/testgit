using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using IMES.Query.DB;
using IMES.Query.Interface.QueryIntf;
using log4net;
     

namespace IMES.Query.Implementation
{
    public class SA_SAReceiveReturnMBQuery : MarshalByRefObject, ISA_SAReceiveReturnMBQuery
    {
        #region SA_SAReceiveReturnMBQuery Members

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            logger.Debug(methodName + " GetMBRepairInfo begin");
//            string strSQL =
//                @"SELECT r.PCBNo,
//                            r.PCBModelID,
//                            r.Line,
//                            RTRIM(r.Station)+' - '+s.Descr as Station,
//                            r.Status,
//                            r_info.Location ,
//                            RTRIM(r_info.DefectCode)+ ' - '+ dc.Descr as Descr,                            
//                            RTRIM(r_info.Cause)+' - '+di.Description as Cause,
//                            RTRIM(r_info.Obligation) + ' - ' + di2.Description AS Obligation,
//                            r_info.Remark,
//                            r_info.Editor,                           
//                            CONVERT(varchar(20), r.Cdt,120) AS Cdt,
//                            CONVERT(varchar(20), r_info.Udt, 120) AS Udt
//                    FROM PCBRepair r (NOLOCK) 
//                    LEFT JOIN Station s (NOLOCK)  on r.Station=s.Station
//                    LEFT JOIN PCBRepair_DefectInfo r_info (NOLOCK) on r.ID = r_info.PCARepairID
//                    LEFT JOIN DefectCode dc (NOLOCK) on r_info.DefectCode = dc.Defect
//                    LEFT JOIN DefectInfo di (NOLOCK) on r_info.Cause = di.Code AND di.Type='SACause'
//                    LEFT JOIN DefectInfo di2 (NOLOCK) ON r_info.Obligation = di2.Code AND di2.Type = 'Obligation'
//                WHERE r.Cdt BETWEEN @StartTime AND @EndTime                
//                ORDER BY r.ID
//                ";

            logger.Debug(methodName + " GetMBRepairInfo strSQL=" + " sp_Query_SA_ReturnMB");

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.StoredProcedure,
                                                     "sp_Query_SA_ReturnMB",
                                                     new SqlParameter("@BegTime", StartTime),
                                                     new SqlParameter("@EndTime", EndTime));

            //DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
            //                                        strSQL, new SqlParameter("@StartTime", StartTime), new SqlParameter("@EndTime", EndTime));
            logger.Debug(methodName + " GetMBRepairInfo end");
            return dt;
        }

        public DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> MBCode)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                logger.Debug(methodName + " GetMBRepairInfo begin");
//                string SelectText = @"SELECT r.PCBNo,
//                            r.PCBModelID,
//                            r.Line,
//                            RTRIM(r.Station)+' - '+s.Descr as Station,
//                            r.Status,
//                            r_info.Location ,
//                            RTRIM(r_info.DefectCode)+ ' - '+ dc.Descr as Descr,                            
//                            RTRIM(r_info.Cause)+' - '+di.Description as Cause,
//                            RTRIM(r_info.Obligation) + ' - ' + di2.Description AS Obligation,
//                            r_info.Remark,
//                            r_info.Editor,                           
//                            CONVERT(varchar(20), r.Cdt,120) AS Cdt,
//                            CONVERT(varchar(20), r_info.Udt, 120) AS Udt,
//                            Substring(r.PCBNo,1,2)  AS Model,
//                             P.Family ";

//                string FromText = @"FROM PCBRepair r (NOLOCK) 
//                    LEFT JOIN Station s (NOLOCK)  on r.Station=s.Station
//                    LEFT JOIN PCBRepair_DefectInfo r_info (NOLOCK) on r.ID = r_info.PCARepairID
//                    LEFT JOIN DefectCode dc (NOLOCK) on r_info.DefectCode = dc.Defect
//                    LEFT JOIN DefectInfo di (NOLOCK) on r_info.Cause = di.Code AND di.Type='SACause'
//                    LEFT JOIN DefectInfo di2 (NOLOCK) ON r_info.Obligation = di2.Code AND di2.Type = 'Obligation'
//                    LEFT JOIN ( SELECT DISTINCT 
//						Replace(UPPER(InfoValue),'B SIDE','') AS Family ,PartNo
//						FROM PartInfo(nolock) 
//						WHERE InfoType = 'MDL' AND CHARINDEX('B SIDE', UPPER(InfoValue)) > 0 
//                        ) P ON P.PartNo = r.PCBModelID  ";

//                string WhereText = @" WHERE r.Cdt BETWEEN @StartTime AND @EndTime ";
//                string OrderText = @" ORDER BY r.ID ";
                string MBCodeList = "";
                if (MBCode.Count != 0)
                {
                    //WhereText += string.Format(" and Substring(r.PCBNo,1,2) IN ('{0}') ", string.Join("','", MBCode.ToArray()));
                    MBCodeList = string.Join(",", MBCode.ToArray());
                }
                
                //string strSQL = SelectText + FromText + WhereText + OrderText;
                logger.Debug(methodName + " GetMBRepairInfo sql=" + " sp_Query_SA_ReturnMB");
                DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.StoredProcedure,
                                                         "sp_Query_SA_ReturnMB",
                                                         new SqlParameter("@BegTime", StartTime),
                                                         new SqlParameter("@EndTime", EndTime),
                                                         new SqlParameter("@MBCode", MBCodeList));
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(methodName + " GetMBRepairInfo err " + ex.Message);
                throw;
            }
            finally
            {
                logger.Debug(methodName + " GetMBRepairInfo end");
            }
        }

        public DataTable GetMBCode(string DBConnection)
        {
            string strSQL = @"SELECT DISTINCT InfoValue as MBCode
                                FROM PartInfo 
                                WHERE PartNo in (
                            	                SELECT Distinct PartNo 
                                                FROM PartInfo 
	                                            WHERE InfoType =  'MDL' 
	                                            AND PartNo like '131%') 
                                AND InfoType = 'MB' 
                                ORDER BY InfoValue";

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            return dt;
        }
        
       #endregion   
    }      
}

