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
    public class SA_PCBStationQuery : MarshalByRefObject, ISA_PCBStationQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataTable GetPCBStationQuery(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> PdLine, IList<string> Family, IList<string> Model, IList<string> Station)
        {
            string selectSQL = "";
            string groupbySQL = "GROUP BY c.Descr";
            string orderbySQL = "ORDER BY Family";

            if (PdLine.Count > 0)
            {
                selectSQL += ", a.Line ";
                groupbySQL += ", a.Line ";
                orderbySQL += ", Line ";
            }
            else {
                selectSQL += ", 'All' AS Line";
            }


            if (Model.Count > 0 ){
                selectSQL += ", b.PCBModelID ";
                groupbySQL += ", b.PCBModelID ";
                orderbySQL += ", PCBModelID ";
            }
            else{
                selectSQL += ", 'All' AS PCBModelID ";               
            }

            if (Station.Count > 0) {
                selectSQL += ", a.Station+d.Descr AS Station";
                groupbySQL += ", a.Station+d.Descr";
                orderbySQL += ", Station ";            
            }
             else{
                selectSQL += ", 'All' AS Station ";               
            }


            StringBuilder sb = new StringBuilder();
            sb.AppendLine("WITH [TEMP] AS (");
            sb.AppendFormat("SELECT c.Descr AS Family {0}, COUNT(*) AS InputQty, SUM( CASE WHEN a.Status=0 THEN 1 ELSE 0 END ) AS DefectQty ", selectSQL);
            sb.AppendLine("FROM PCBLog a ");
            if (Model.Count > 0) {
                sb.AppendFormat("INNER JOIN PartInfo e ON e.InfoType = 'MB' AND InfoValue IN ('{0}') ", string.Join("','",Model.ToArray()));
            }
            else
            {
                sb.AppendLine("INNER JOIN PartInfo e ON e.InfoType = 'MB' ");
            }
            sb.AppendLine("INNER JOIN PCB b ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
            if (Family.Count > 0) {
                sb.AppendLine(string.Format("INNER JOIN Part c ON c.PartNo = a.PCBModel AND UPPER(c.Descr) IN ('{0}')  ", string.Join("','", Family.ToArray())));
            } else {                
                sb.AppendLine("INNER JOIN Part c ON c.PartNo = a.PCBModel ");
            }
            
            sb.AppendLine("INNER JOIN Station d ON a.Station=d.Station ");
            sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime ");
            if (PdLine.Count>0){
                sb.AppendFormat("AND a.Line in ('{0}') ",string.Join("','",PdLine.ToArray()));
            }
            
            if(Station.Count> 0){
                sb.AppendFormat("AND a.Station in ('{0}') ",string.Join("','", Station.ToArray()));

            }
            sb.AppendFormat("{0} ) ", groupbySQL);

            sb.AppendLine("SELECT *, CASE WHEN InputQty > 0 THEN CONVERT(varchar(10), CAST (DefectQty AS Float)/InputQty ) ELSE '0' END AS FPFRate, ");
		    sb.AppendLine("          CASE WHEN InputQty > 0 THEN CONVERT(varchar(10), 1 - (CAST (DefectQty AS Float)/InputQty) ) ELSE '1' END AS FPYRate ");
		    sb.AppendLine("FROM TEMP ");
            sb.AppendFormat("{0}", orderbySQL);
        

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,System.Data.CommandType.Text,
                                                    sb.ToString() ,new SqlParameter("@StartTime",StartTime),new SqlParameter("@EndTime",EndTime));

            return dt;
        }

        public DataTable GetModel(string DBConnection, IList<String> Family)
        {
            string strSQL = @"SELECT DISTINCT InfoValue FROM PartInfo WHERE PartNo in (
                            	SELECT Distinct PartNo FROM PartInfo 
	                            WHERE InfoType =  'MDL' 
	                            AND  Replace(UPPER(InfoValue),'B SIDE','') IN ('{0}')
	                            AND PartNo like '131%') 
                            AND InfoType = 'MB' 
                            ORDER BY InfoValue";            
            strSQL  = string.Format(strSQL,string.Join("','",Family.ToArray()));
            
            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            return dt;
        }

        public DataTable GetStation(string DBConnection, IList<String> Family)
        {
            string strSQL =
            @"SELECT * FROM dbo.Station WHERE StationType Like 'SA%'
                ";

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);
            return dt;
        }

        public DataTable GetSelectDetail(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> PdLine, string Family, IList<string> Model, IList<string> Station)
        {
            string selectSQL = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"SELECT a.PCBNo, r.PCBModelID, r.Line, RTRIM(r.Station) as Station, r_info.Location, RTRIM(r_info.DefectCode) as Defect,
            RTRIM(r_info.Cause) as Cause, RTRIM(r_info.Obligation) as Obligation, r_info.Remark, r_info.Editor,
            CONVERT(varchar(20), r.Cdt,120) AS Cdt, CONVERT(varchar(20), r_info.Udt, 120) AS Udt
            FROM PCBLog a (NOLOCK) ");

            if (Model.Count > 0)
            {
                sb.AppendFormat("INNER JOIN PartInfo e (NOLOCK) ON e.InfoType = 'MB' AND InfoValue IN ('{0}') ", string.Join("','", Model.ToArray()));
            }
            else
            {
                sb.AppendLine("INNER JOIN PartInfo e (NOLOCK) ON e.InfoType = 'MB' ");
            }

            sb.AppendLine("INNER JOIN PCB b (NOLOCK) ON a.PCBNo = b.PCBNo AND e.PartNo = b.PCBModelID ");
            if (Family.Length > 0)
            {
                sb.AppendLine(string.Format("INNER JOIN Part c (NOLOCK) ON c.PartNo = a.PCBModel AND UPPER(c.Descr) = '{0}' ", Family));
            }
            else
            {
                sb.AppendLine("INNER JOIN Part c (NOLOCK) ON c.PartNo = a.PCBModel ");
            }

            sb.AppendLine("INNER JOIN Station d ON a.Station=d.Station ");

            sb.AppendLine("Left Join PCBRepair r (NOLOCK) On r.LogID=a.ID ");
            sb.AppendLine("LEFT JOIN PCBRepair_DefectInfo r_info (NOLOCK) on r_info.ID=( select MAX(ID) from PCBRepair_DefectInfo where r.ID=PCARepairID ) ");

            sb.AppendLine("WHERE a.Cdt Between @StartTime AND @EndTime and a.Status='0' ");
            if (PdLine.Count > 0)
            {
                sb.AppendFormat("AND a.Line in ('{0}') ", string.Join("','", PdLine.ToArray()));
            }

            if (Station.Count > 0)
            {
                sb.AppendFormat("AND a.Station in ('{0}') ", string.Join("','", Station.ToArray()));

            }

            sb.AppendLine("order by c.Descr, a.PCBNo, a.Cdt ");

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text,
                                                    sb.ToString(), new SqlParameter("@StartTime", StartTime), new SqlParameter("@EndTime", EndTime));

            return dt;
        }

    }
}

