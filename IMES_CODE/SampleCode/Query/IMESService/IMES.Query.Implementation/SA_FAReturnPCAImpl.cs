using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
     

namespace IMES.Query.Implementation
{
    public class SA_FAReturnPCAImpl : MarshalByRefObject, ISA_FAReturnPCA
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public DataTable GetFAReturnPCAInfo(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> lstModel)
        {

            StringBuilder sb = new StringBuilder();
            /*
            sb.Append("SELECT tl.PCBNo, pt_info.InfoValue, tl_info.DefectCodeID  AS DefectCode, de.Descr, pr_info.Cause, pr_info.Site, pr_info.MajorPart, pr_info.Remark, pr_info.Location, pr_info.OldPartSno, tl.Cdt, pr_info.Udt ");
            sb.Append("FROM PCBTestLog tl ");
            sb.Append("LEFT JOIN PCBTestLog_DefectInfo tl_info on  tl_info.PCBTestLogID = tl.ID ");
            sb.Append("LEFT JOIN PCBRepair pr on pr.TestLogID = tl.ID ");
            sb.Append("LEFT JOIN PCBRepair_DefectInfo pr_info on pr_info.PCARepairID = pr.ID ");
            sb.Append("LEFT JOIN DefectCode de on de.Defect = tl_info.DefectCodeID ");
            sb.Append("LEFT JOIN PCB pc on pc.PCBNo = tl.PCBNo ");
            if (lstModel.Count == 0){
                sb.Append("LEFT JOIN PartInfo pt_info on pt_info.PartNo = pc.PCBModelID AND pt_info.InfoType = 'MB'");
            }
            else{
                sb.AppendFormat("RIGHT JOIN PartInfo pt_info on pt_info.PartNo = pc.PCBModelID AND pt_info.InfoType = 'MB' AND InfoValue in ('{0}') ", string.Join("','", lstModel.ToArray()));
            }
            sb.Append("LEFT JOIN Part pt on pt.PartNo = pt_info.PartNo And pt.PartType = 'MB' ");
            sb.Append("WHERE tl.Station IN ('33','33A')  AND tl.Cdt BETWEEN @StartTime AND @EndTime  ");
            sb.Append(" ORDER BY pt_info.InfoValue, tl.Cdt");
            */
            sb.Append("SELECT pr.PCBNo, pt_info.InfoValue, pr_info.DefectCode  AS DefectCode, de.Descr, ");
            sb.Append("pr_info.Cause, pr_info.Obligation, pr_info.Site, pr_info.MajorPart, pr_info.Remark, pr_info.Location, pr_info.OldPartSno, pr.Cdt, pr_info.Udt ");
            sb.Append("FROM PCBRepair pr ");
            sb.Append("LEFT JOIN PCBRepair_DefectInfo pr_info on pr_info.PCARepairID = pr.ID ");
            sb.Append("LEFT JOIN DefectCode de on de.Defect = pr_info.DefectCode ");
            sb.Append("LEFT JOIN PCB pc on pc.PCBNo = pr.PCBNo ");
            if (lstModel.Count == 0)
            {
                sb.Append("LEFT JOIN PartInfo pt_info on pt_info.PartNo = pc.PCBModelID AND pt_info.InfoType = 'MB'");
            }
            else
            {
                sb.AppendFormat("RIGHT JOIN PartInfo pt_info on pt_info.PartNo = pc.PCBModelID AND pt_info.InfoType = 'MB' AND InfoValue in ('{0}') ", string.Join("','", lstModel.ToArray()));
            }
            sb.Append("LEFT JOIN Part pt on pt.PartNo = pt_info.PartNo And pt.PartType = 'MB' ");
            sb.Append("WHERE (pr.Station = '33' or pr.Station = '33A')  AND pr.Cdt BETWEEN @StartTime AND @EndTime  ");
            sb.Append(" ORDER BY pt_info.InfoValue, pr.Cdt");
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                     sb.ToString(), new SqlParameter("@StartTime", StartTime), new SqlParameter("@EndTime", EndTime));
            return dt;
        }

        public string[] GetModel(String DBConnection)
        {

//            string strSQL = @"SELECT DISTINCT InfoValue FROM PartInfo WHERE PartNo in (
//                            	SELECT Distinct PartNo FROM PartInfo 
//	                            WHERE InfoType =  'MDL' 
//	                            AND  Replace(InfoValue,'B Side','') in (SELECT Family FROM Family)  
//	                            AND PartNo like '131%') 
//                            AND InfoType = 'MB' 
//                            ORDER BY InfoValue";
            string strSQL = "SELECT DISTINCT InfoValue FROM PartInfo WHERE InfoType = 'MB'";

            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,
                                                                                System.Data.CommandType.Text,
                                                                               strSQL);
            string[] result = new string[tb.Rows.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = tb.Rows[i]["InfoValue"].ToString();
            }
            return result;
        }

    }
}

