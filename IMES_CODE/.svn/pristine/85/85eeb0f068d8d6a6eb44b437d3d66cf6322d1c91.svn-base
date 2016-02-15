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
    public class SA_PCBInfo : MarshalByRefObject, ISA_PCBInfo
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IPCAQuery Members

        public MBInfo GetMBInfo(string DBConnection, string MBSN, out List<NextStation> NextStationList)
        {
            NextStationList = new List<NextStation>();
            MBInfo mb = null;            

            string strSQL = @"SELECT  a.PCBNo , a.CUSTSN, a.MAC, a.ECR, a.SMTMO, 
                              a.PCBModelID , b.Station, c.Descr, 
                              b.Line, b.Status, b.TestFailCount,  CONVERT(nvarchar(20) , b.Udt, 120) AS Udt,  
                              (case  when d.InfoValue is null then 'Normal' 
                                   else d.InfoValue end) as ShipMode                                                    
                             FROM PCB a (NOLOCK)
                             INNER JOIN PCBStatus b (NOLOCK) on ( a.PCBNo = b.PCBNo)
                             INNER JOIN Station c  (NOLOCK) on (b.Station = c.Station)
                             LEFT JOIN  PCBInfo d (NOLOCK) on (a.PCBNo= d.PCBNo and d.InfoType=@ShipMode)
                             WHERE a.PCBNo = @PCBNo or a.MAC = @PCBNo ";
            SqlParameter paraName = new SqlParameter("@PCBNo", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = MBSN;

            SqlParameter paraName1 = new SqlParameter("@ShipMode", SqlDbType.VarChar, 32);
            paraName1.Direction = ParameterDirection.Input;
            paraName1.Value = "ShipMode";

            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     paraName,
                                                     paraName1);
            if (tb.Rows.Count == 1)
            {
                mb = new MBInfo();
                mb.MBSN = tb.Rows[0]["PCBNo"].ToString().Trim();
                mb.MAC = tb.Rows[0]["MAC"].ToString().Trim();
                mb.CustomSN = tb.Rows[0]["CUSTSN"].ToString().Trim();
                mb.ECR = tb.Rows[0]["ECR"].ToString().Trim();
                mb.Line = tb.Rows[0]["Line"].ToString().Trim();
                mb.PartNo = tb.Rows[0]["PCBModelID"].ToString().Trim();
                mb.SMTMO = tb.Rows[0]["SMTMO"].ToString().Trim();
                mb.Station = tb.Rows[0]["Station"].ToString().Trim();
                mb.StationDescr = tb.Rows[0]["Descr"].ToString().Trim();
                mb.Status = (int)tb.Rows[0]["Status"];
                mb.TestFailCount = (int)tb.Rows[0]["TestFailCount"];
                mb.Udt = DateTime.Parse(tb.Rows[0]["Udt"].ToString());


                string ShipMode = tb.Rows[0]["ShipMode"].ToString().Trim();

                strSQL = @"SELECT b.Station,c.Descr
                           FROM PartProcess a (NOLOCK), Process_Station b (NOLOCK) , Station c (NOLOCK)
                           WHERE (a.MBFamily IN
                                    (SELECT Descr FROM Part WHERE PartNo= @PartNo) 
                                OR a.MBFamily = @PartNo) 
                           AND a.Process = b.Process  
                           AND b.PreStation =@Station 
                           AND b.Status=@Status AND b.Station =c.Station";


                SqlParameter para1 = new SqlParameter("@PartNo", SqlDbType.VarChar, 32);
                para1.Direction = ParameterDirection.Input;
                para1.Value = mb.PartNo;
                SqlParameter para2 = new SqlParameter("@Station ", SqlDbType.VarChar, 32);
                para2.Direction = ParameterDirection.Input;
                para2.Value = mb.Station;
                SqlParameter para3 = new SqlParameter("@Status ", SqlDbType.Int);
                para3.Direction = ParameterDirection.Input;
                para3.Value = mb.Status;

                SqlParameter para4 = new SqlParameter("@IsFRU ", SqlDbType.VarChar, 32);
                para4.Direction = ParameterDirection.Input;

                if (ShipMode == "FRU")
                {
                    para4.Value = "Y";
                }
                else
                {
                    para4.Value = "N";
                }
                DataTable tb1 = SQLHelper.ExecuteDataFill(DBConnection,
                                                          System.Data.CommandType.Text,
                                                          strSQL,
                                                          para1,
                                                          para2,
                                                          para3,
                                                          para4);
                foreach (DataRow dr in tb1.Rows)
                {
                    NextStation nextStation = new NextStation();
                    nextStation.Station = dr["Station"].ToString().Trim();
                    nextStation.Description = dr["Descr"].ToString().Trim();
                    NextStationList.Add(nextStation);
                }
            }
            return mb;

        }

        public DataTable GetMBHistory(string DBConnection, string MBSN)
        {
            string strSQL = @"SELECT  a.Station , c.Descr as StationName, a.Line, ISNULL(b.FixtureID,'') as FixtureID, 
                                 (case a.Status when 1 then 'Pass' else 'Fail' end) as Status , 
                                 ISNULL(b.ErrorCode,'') as ErrorCode, a.Editor,CONVERT(varchar(20) , a.Cdt, 120) AS Cdt 
                              FROM PCBLog a (NOLOCK)
                              INNER JOIN Station c (NOLOCK) on a.Station = c.Station
                              LEFT  JOIN PCBTestLog b (NOLOCK) on a.PCBNo= b.PCBNo AND a.Station = b.Station AND a.Cdt =b.Cdt  
                              WHERE a.PCBNo =@PCBNo
                              ORDER BY a.Cdt ";
            SqlParameter paraName = new SqlParameter("@PCBNo", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = MBSN;
            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     paraName);
            return tb;
        }

        public DataTable GetStation(String MBSN ,string DBConnection)
        {
            string strSQL = @"SELECT f.Station,f.Descr
                              FROM PCB a
	                            LEFT JOIN Part b ON b.PartNo = a.PCBModelID AND Flag = 1
	                            LEFT JOIN PartProcess c on c.MBFamily = b.Descr	
	                            LEFT JOIN PCBStatus d on d.PCBNo = a.PCBNo
	                            LEFT JOIN Process_Station e ON e.Process = c.Process AND e.PreStation = d.Station AND e.Status = d.Status
	                            LEFT JOIN Station f ON e.Station = f.Station	
                            WHERE a.PCBNo = '{0}'";
            strSQL = string.Format(strSQL, MBSN);

            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL);

            return tb;
        }


        public DataTable GetChangePCBLog(string DBConnection, String MBSN)
        {
            
            StringBuilder sb =new StringBuilder();           
            sb.AppendLine("DECLARE @OldPCBNo char(10) ");
            sb.AppendLine("DECLARE @NewPCBNo char(10) ");
            sb.AppendLine("SELECT Top 1 * INTO #temp ");
            sb.AppendLine("FROM dbo.Change_PCB ");
            sb.AppendFormat("WHERE NewPCBNo = '{0}' OR OldPCBNo = '{0}'", MBSN);
            sb.AppendLine("SELECT Top 1 @OldPCBNo = OldPCBNo , @NewPCBNo = NewPCBNo FROM #temp ");

            sb.AppendLine("WHILE EXISTS(SELECT TOP 1 * FROM dbo.Change_PCB WHERE OldPCBNo = @NewPCBNo )");
            sb.AppendLine("BEGIN ");
            sb.AppendLine("INSERT INTO #temp ");
            sb.AppendLine("SELECT TOP 1 * FROM dbo.Change_PCB ");
            sb.AppendLine("WHERE OldPCBNo = @NewPCBNo ");
            sb.AppendLine("SELECT TOP 1 @NewPCBNo = NewPCBNo ");
            sb.AppendLine("FROM dbo.Change_PCB ");
            sb.AppendLine("WHERE OldPCBNo = @NewPCBNo ");
            sb.AppendLine("END ");
            sb.AppendLine("WHILE EXISTS(SELECT TOP 1 * FROM dbo.Change_PCB WHERE NewPCBNo = @OldPCBNo )");
            sb.AppendLine("BEGIN ");
            sb.AppendLine("INSERT INTO #temp ");
            sb.AppendLine("SELECT TOP 1 * FROM dbo.Change_PCB ");
            sb.AppendLine("WHERE NewPCBNo = @OldPCBNo ");
            sb.AppendLine("SELECT TOP 1 @OldPCBNo = OldPCBNo ");
            sb.AppendLine("FROM dbo.Change_PCB ");
            sb.AppendLine("WHERE NewPCBNo = @OldPCBNo ");
            sb.AppendLine("END ");
            sb.AppendLine("SELECT NewPCBNo,OldPCBNo,Reason,Editor,Cdt FROM #temp ORDER BY Cdt desc ");

            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,
                                                                                 System.Data.CommandType.Text,
                                                                                sb.ToString());
            return tb;
        }

        public DataTable GetRepairInfo(string DBConnection, String MBSN)
        {
            string strSQL = @"SELECT r.Line,RTRIM(r.Station)+' - '+s.Descr as StationName, Status,
	                            r_info.Location , RTRIM(r_info.DefectCode)+ ' - '+ dc.Descr as Descr,    
                                RTRIM(r_info.Cause)+' - '+di.Description as Cause,
                                RTRIM(r_info.Obligation) + ' - ' + di2.Description AS Obligation,
                                r_info.Remark, r.Editor, CONVERT(varchar(20), r.Cdt, 120) AS Cdt
                             FROM PCB a (NOLOCK) 
                             LEFT JOIN PCBRepair r (NOLOCK) ON a.PCBNo=r.PCBNo
                             LEFT JOIN Station s on r.Station=s.Station
                             INNER JOIN PCBRepair_DefectInfo r_info (NOLOCK) on r.ID = r_info.PCARepairID
                             LEFT JOIN DefectCode dc (NOLOCK) on r_info.DefectCode = dc.Defect
                             LEFT JOIN DefectInfo di (NOLOCK) on r_info.Cause = di.Code AND di.Type='SACause'
                             LEFT JOIN DefectInfo di2 (NOLOCK) ON r_info.Obligation = di2.Code AND di2.Type = 'Obligation'
                             WHERE a.PCBNo=@PCBNo OR a.MAC=@PCBNo ORDER BY a.Cdt";
            SqlParameter paraName = new SqlParameter("@PCBNo", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = MBSN;
            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     paraName);
            return tb;
        }


        public DataTable GetTestLog(string DBConnection, String MBSN) {

            string sql = "SELECT * FROM SysSetting WHERE Name = 'PCATestStation'";
            DataTable dtstation = SQLHelper.ExecuteDataFill(DBConnection, System.Data.CommandType.Text, sql);

            List<string> station = new List<string>();
            if (dtstation.Rows.Count > 0) {
                station.AddRange(dtstation.Rows[0]["Value"].ToString().Split(','));
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM PCBTestLog a ");
            sb.AppendLine("LEFT JOIN PCBTestLog_DefectInfo b ON b.PCBTestLogID = a.ID ");
            sb.AppendLine("LEFT JOIN DefectCode c ON c.Defect = b.DefectCodeID ");
            sb.AppendLine(string.Format( "WHERE a.PCBNo = '{0}' ",MBSN));
            sb.AppendLine(string.Format("AND Station IN ('{0}') ", string.Join("','", station.ToArray())));
            sb.AppendLine("ORDER BY a.ID ");

            DataTable tb = SQLHelper.ExecuteDataFill(DBConnection,System.Data.CommandType.Text,sb.ToString());
            return tb;
        }
       #endregion   
    }      
}

