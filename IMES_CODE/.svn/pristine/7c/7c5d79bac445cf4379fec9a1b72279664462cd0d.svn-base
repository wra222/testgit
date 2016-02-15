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
    public class SA_PCBTestDefect : MarshalByRefObject, ISA_ICTTestDefect
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        #region IPCBTestDefect Members

        public DataTable GetDefectInfo(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> Family, IList<string> PdLine, IList<string> Model, IList<string> Station)
        {
            string strSQL =
               @"SELECT TOP 1000 a.ID,a.Line,a.Station,a.ActionName,a.PCBNo,c.Descr,a.Editor,a.Cdt	   
                  FROM PCBTestLog a
	              LEFT JOIN PCBTestLog_DefectInfo b on a.ID=b.PCBTestLogID
	              LEFT JOIN DefectCode c on c.Defect = b.DefectCodeID	
                  WHERE a.Status = 0  
	              AND a.PCBNo IN (
		                SELECT a.PCBNo FROM PCB a				
			            RIGHT JOIN ModelBOM b ON b.Component = a.PCBModelID AND b.Material_group = 'MB'
			            RIGHT JOIN Model c ON c.Model = b.Material  AND c.Family IN ('{0}')	
                        RIGHT JOIN dbo.PartInfo d ON InfoType='MB' AND d.PartNo = a.PCBModelID AND InfoValue in ('{1}') 
                   )
	               AND a.Cdt BETWEEN @StartTime AND @EndTime
                   AND a.Station IN ('{2}')
                   AND a.Line IN ('{3}');
                ";

            strSQL = string.Format(strSQL, string.Join("','", Family.ToArray()), string.Join("','", Model.ToArray()), string.Join("','", Station.ToArray()), string.Join("','", PdLine.ToArray()));

            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                                                System.Data.CommandType.Text,
                                                                               strSQL, new SqlParameter("@StartTime", StartTime), new SqlParameter("@EndTime", EndTime));

            return dt;
        }

        public DataTable GetModel(string DBConnection)
        {
            string strSQL =
            @"SELECT * FROM dbo.Station WHERE StationType = 'SATest'
                ";


            DataTable dt = SQLHelper.ExecuteDataFill(DBConnection,
                                                                                System.Data.CommandType.Text,
                                                                               strSQL);
            return dt;
        }

        public DataTable GetTestStation(string DBConnection, string Family)
        {
            string strSQL = @"SELECT DISTINCT InfoValue 
                              FROM PartInfo 
                              WHERE PartNo in (SELECT PartNo FROM Part WHERE PartType = 'MB')
                              AND InfoType = 'MB'";
            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,
                                                                                System.Data.CommandType.Text,
                                                                               strSQL);
            return tb;
        }

        #endregion
    }
}

