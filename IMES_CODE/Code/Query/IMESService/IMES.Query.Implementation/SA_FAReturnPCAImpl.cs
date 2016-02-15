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
            sb.Append("SELECT pr.PCBNo,pi.InfoValue AS MBCT,e.CUSTSN AS UnitCUSTSN, pt_info.InfoValue, pr_info.DefectCode  AS DefectCode,de.EngDescr AS FailureDescription, de.Descr, ");
            sb.Append("pr_info.Cause,deinfo.EngDescr AS Causedescription, pr_info.Obligation, pr_info.Site, pr_info.MajorPart, pr_info.Remark, pr_info.Location, pr_info.OldPartSno, pr.Cdt, pr_info.Udt, ");
            //add by viclin
            sb.Append("pt.Descr as Family, SUBSTRING(e.Line,3,1) as [白/夜], ");
            sb.Append("e.Station  as Test_Station, SUBSTRING(e.Line,1,1) as FA_Line, ");
            sb.Append("e.DefectCode as FA_DefectCode,e.FAfailurestation , e.Descr as FA_Descr, e.Editor as FA_Editor, pcblog.Editor as SA_Editor ");
            //add by viclin

            sb.Append("FROM PCBRepair pr ");
            sb.Append("LEFT JOIN PCBRepair_DefectInfo pr_info on pr_info.PCARepairID = pr.ID ");
            sb.Append("LEFT JOIN DefectCode de on de.Defect = pr_info.DefectCode ");
            sb.Append("LEFT JOIN PCB pc on pc.PCBNo = pr.PCBNo ");
            sb.Append("LEFT JOIN PCBInfo pi on pi.PCBNo = pr.PCBNo and pi.InfoType='MBCT' ");
            sb.Append("LEFT JOIN dbo.DefectInfo deinfo");
            sb.Append(" ON pr_info.Cause=deinfo.Code ");

            if (lstModel.Count == 0)
            {
                sb.Append("LEFT JOIN PartInfo pt_info on pt_info.PartNo = pc.PCBModelID AND pt_info.InfoType = 'MB'");
            }
            else
            {
                sb.AppendFormat("RIGHT JOIN PartInfo pt_info on pt_info.PartNo = pc.PCBModelID AND pt_info.InfoType = 'MB' AND InfoValue in ('{0}') ", string.Join("','", lstModel.ToArray()));
            }
            sb.Append("LEFT JOIN Part pt on pt.PartNo = pt_info.PartNo And pt.BomNodeType = 'MB' ");
            //add by viclin
            sb.Append("left join PCBLog pcblog on pr.LogID = pcblog.ID  ");
            sb.Append("left join (select a2.ProductID,d2.CUSTSN,a2.Line,a2.Station,b2.Type,b2.DefectCode,b2.Cause,b2.Component,RTRIM(f2.Station)+'_'+RTRIM(f3.Name) AS FAfailurestation, ");
            sb.Append("b2.Obligation,b2.Location,b2.MajorPart,b2.Remark,b2.OldPartSno,c2.Descr,b2.Editor,CONVERT(CHAR(20),b2.Udt,120) AS Udt  ");
            sb.Append("from ProductRepair a2 ");
            sb.Append("left join ProductRepair_DefectInfo b2 ");
            sb.Append("on a2.ID=b2.ProductRepairID ");
            sb.Append("left join DefectCode c2 ");
            sb.Append(" on c2.Defect=b2.DefectCode ");
            sb.Append("LEFT JOIN dbo.Product d2 ");
            sb.Append(" ON a2.ProductID=d2.ProductID ");
            sb.Append("LEFT JOIN  dbo.ProductTestLog f2");
            sb.Append(" ON a2.TestLogID=f2.ID ");
            sb.Append("LEFT JOIN  dbo.Station f3");
            sb.Append(" ON f2.Station=f3.Station ");

            sb.Append(") e on  pr.PCBNo=e.OldPartSno and  CONVERT(CHAR(20),pr.Cdt,120)=e.Udt ");
            //add by viclin
            //sb.Append("WHERE (pr.Station = '33' or pr.Station = '33A')  AND pr.Cdt BETWEEN @StartTime AND @EndTime  ");
            sb.Append("WHERE (pr.Station = '33')  AND pr.Cdt BETWEEN @StartTime AND @EndTime   and (deinfo.Type='SACause' OR deinfo.Type IS NULL)");
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