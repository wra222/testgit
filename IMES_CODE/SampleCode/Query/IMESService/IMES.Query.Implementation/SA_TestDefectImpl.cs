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
    public class SA_PCBTestDefect : MarshalByRefObject, ISA_TestDefect
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IPCBTestDefect Members

        public DataTable GetDefectInfo(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> Family, IList<string> PdLine, IList<string> Model, IList<string> Station)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT a.ID,a.Line,a.Station,a.ActionName,a.PCBNo,c.Descr,a.Editor,a.Cdt ");
            sb.AppendLine("FROM PCBTestLog a ");
            sb.AppendLine("LEFT JOIN PCBTestLog_DefectInfo b on a.ID=b.PCBTestLogID ");
            sb.AppendLine("LEFT JOIN DefectCode c on c.Defect = b.DefectCodeID ");
            sb.AppendLine("WHERE a.Status = 0 ");
            if (Family.Count >0) {
                sb.AppendLine("AND SUBSTRING(a.PCBNo,1,2) IN ( ");
                sb.AppendLine("SELECT DISTINCT InfoValue AS Model FROM PartInfo WHERE PartNo in ( ");
                sb.AppendLine(string.Format("SELECT Distinct PartNo FROM PartInfo WHERE InfoType =  'MDL' AND  Replace(UPPER(InfoValue),'B SIDE','') = '{0}') ", string.Join("','", Family.ToArray())));
                sb.AppendLine("AND InfoType = 'MB' ");
                sb.AppendLine(")");
            }

            if (Model.Count > 0){
                sb.AppendLine("AND a.PCBNo IN ( ");
                sb.AppendLine(" SELECT a.PCBNo FROM PCB a ");
                sb.AppendLine(string.Format("    INNER JOIN PartInfo b ON b.InfoType = 'MB' ANd b.InfoValue IN ('{0}') ",Model.ToArray()));
				sb.AppendLine("    AND b.PartNo Like '131%' AND a.PCBModelID = b.PartNo) ");
            }
            sb.AppendLine("AND a.Cdt BETWEEN @StartTime AND @EndTime ");
            if (Station.Count > 0) { 
                sb.AppendLine(string.Format("AND a.Station IN ('{0}') ",string.Join("','",Station.ToArray())));
            }
            if (PdLine.Count > 0)
            {
                sb.AppendLine(string.Format("AND a.Line IN ('{0}'); ", string.Join(",",PdLine.ToArray())));
            }

            
            //strSQL = string.Format(strSQL, string.Join("','", Family.ToArray()), string.Join("','", Model.ToArray()), string.Join("','", Station.ToArray()), string.Join("','", PdLine.ToArray()));

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                                 System.Data.CommandType.Text,
                                                             sb.ToString(), new SqlParameter("@StartTime", StartTime), new SqlParameter("@EndTime", EndTime));

            return dt;
        }

        public DataTable GetModel(string DBConnection,string Family)
        {
            string strSQL = @"SELECT DISTINCT InfoValue AS Model FROM PartInfo WHERE PartNo in (
                            	SELECT Distinct PartNo FROM PartInfo 
	                            WHERE InfoType =  'MDL' 
	                            AND  Replace(UPPER(InfoValue),'B SIDE','') = '{0}'
                                ) 
                            AND InfoType = 'MB' 
                            ORDER BY InfoValue";
            strSQL = string.Format(strSQL, Family);

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                                                System.Data.CommandType.Text,
                                                                               strSQL);
            return dt;
        }

        public DataTable GetTestStation(string DBConnection, string Family)
        {
            string strSQL =
            @"SELECT * FROM dbo.Station WHERE StationType = 'SATest'
                ";

            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,
                                                                                System.Data.CommandType.Text,
                                                                               strSQL);
            return tb;
        }

        #endregion
    }
}

