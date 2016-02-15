using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.QueryInf;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.UnitOfWork;
using log4net;

namespace IMES.Station.Implementation
{
    class PCAQueryImpl : MarshalByRefObject, IPCAQuery
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IPCAQuery Members

        MBInfo IPCAQuery.GetMBInfo(string MBSN, out List<NextStation> NextStationList)
        {
            NextStationList = new List<NextStation>();
            MBInfo mb = null;
            string strSQL = @"select  a.PCBNo , a.CUSTSN, a.MAC, a.ECR, a.SMTMO, 
                                                  a.PCBModelID , b.Station, c.Descr, 
                                                  b.Line, b.Status, b.TestFailCount,  b.Udt, 
                                                  (case  when d.InfoValue is null then 'Normal' 
                                                       else d.InfoValue end) as ShipMode                                                    
                                                 from PCB a 
                                                 inner join PCBStatus b on ( a.PCBNo = b.PCBNo)
                                                 inner join Station c  on (b.Station = c.Station)
                                                 left join  PCBInfo d on  (a.PCBNo= d.PCBNo and d.InfoType=@ShipMode)
                                                 where a.PCBNo = @PCBNo or a.MAC = @PCBNo ";
            SqlParameter paraName = new SqlParameter("@PCBNo", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = MBSN;

            SqlParameter paraName1 = new SqlParameter("@ShipMode", SqlDbType.VarChar, 32);
            paraName1.Direction = ParameterDirection.Input;
            paraName1.Value = "ShipMode";

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL, 
                                                                                paraName,
                                                                                paraName1);
            if (tb.Rows.Count == 1)
            {
                mb = new MBInfo();
                mb.MBSN =tb.Rows[0]["PCBNo"].ToString().Trim();
                mb.MAC = tb.Rows[0]["MAC"].ToString().Trim();
                mb.CustomSN = tb.Rows[0]["CUSTSN"].ToString().Trim();
                mb.ECR = tb.Rows[0]["ECR"].ToString().Trim();
                mb.Line = tb.Rows[0]["Line"].ToString().Trim();
                mb.PartNo = tb.Rows[0]["PCBModelID"].ToString().Trim();
                mb.SMTMO = tb.Rows[0]["SMTMO"].ToString().Trim();
                mb.Station = tb.Rows[0]["Station"].ToString().Trim();
                mb.StationDescr = tb.Rows[0]["Descr"].ToString().Trim();
                mb.Status = (int)tb.Rows[0]["Status"];
                mb.TestFailCount =(int) tb.Rows[0]["TestFailCount"];
                mb.Udt = (DateTime)tb.Rows[0]["Udt"];


                string ShipMode = tb.Rows[0]["ShipMode"].ToString().Trim();

                strSQL = @"select b.Station,c.Descr
                                     from PartProcess a , Process_Station b, Station c
                                     where a.MBFamily = @PartNo and
                                                a.PilotRun= @IsFRU      and 
                                                a.Process = b.Process  and
                                                b.PreStation =@Station and
                                                b.Status=@Status  and 
                                                b.Station =c.Station";


               
                SqlParameter para1 = new SqlParameter("@PartNo", SqlDbType.VarChar, 32);
                para1.Direction = ParameterDirection.Input;
                para1.Value = mb.PartNo;
                SqlParameter para2 = new SqlParameter("@Station ", SqlDbType.VarChar, 32);
                para2.Direction = ParameterDirection.Input;
                para2.Value = mb.Station;
                SqlParameter para3 = new SqlParameter("@Status ", SqlDbType.Int);
                para3.Direction = ParameterDirection.Input;
                para3.Value = mb.Status;

                SqlParameter para4 = new SqlParameter("@IsFRU ", SqlDbType.VarChar,32);
                para4.Direction = ParameterDirection.Input;
                
                if (ShipMode == "FRU")
                {
                    para4.Value ="Y";
                }
                else
                {
                    para4.Value = "N";
                }
                DataTable tb1 = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                     System.Data.CommandType.Text,
                                                                                    strSQL,
                                                                                    para1,
                                                                                    para2,
                                                                                    para3,
                                                                                    para4);
                foreach (DataRow dr in tb1.Rows)
                {
                    NextStation nextStation= new NextStation();
                    nextStation.Station = dr["Station"].ToString().Trim();
                    nextStation.Description = dr["Descr"].ToString().Trim();
                    NextStationList.Add(nextStation);
                } 
            }           
            return mb;
           
        }


        DataTable IPCAQuery.GetMBHistory(string MBSN)
        {
            string strSQL = @"select  a.Station , c.Descr as StationName, a.Line, ISNULL(b.FixtureID,'') as FixtureID, 
                    (case a.Status when 1 then 'Pass' else 'False' end) as Status , 
                ISNULL(b.ErrorCode,'') as ErrorCode, a.Editor,a.Cdt 
                from PCBLog a 
                inner join Station c
                on a.Station = c.Station
                left  join PCBTestLog b 
                on a.PCBNo= b.PCBNo and a.Station = b.Station and a.Cdt =b.Cdt  
                where a.PCBNo =@PCBNo
                order by a.Cdt ";
            SqlParameter paraName = new SqlParameter("@PCBNo", SqlDbType.VarChar, 32);
            paraName.Direction = ParameterDirection.Input;
            paraName.Value = MBSN;
            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL,
                                                                                paraName);

            return tb;
        }


        DataTable IPCAQuery.GetStation()
        {
            string strSQL = @"select Station, Descr from Station where OperationObject=1 and StationType != 'SARepair' and Station NOT IN ('32') order by Station";

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL);

            return tb;
        }

        DataTable IPCAQuery.GetMultiPCBInfo(IList<string> PCBNoList)
        {
            string strSQL = @"select  a.PCBNo, case when b.Station in ('32','CL') then 'NO' else 'YES' end as AllowJump, 
                                      a.CUSTSN, a.MAC, a.ECR, a.PCBModelID, RTrim(b.Station), RTrim(b.Station) + ' - '+ c.Descr as StationDesc, 
                                      CASE b.Status WHEN 1 THEN 'PASS' ELSE 'Fail' END AS Status
                                            from PCB a, PCBStatus b , Station c
                                        where a.PCBNo= b.PCBNo and b.Station = c.Station and
                                                   a.PCBNo in ('{0}') ";
            string inSection= string.Join("','", PCBNoList.ToArray());
             strSQL = string.Format(strSQL, inSection);

            DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                                                                 System.Data.CommandType.Text,
                                                                                strSQL);
            
            return tb;
        }


        void IPCAQuery.UpdatePCBStatus(IList<string> PCBNoList,
                                                                   string station,
                                                                   int status,
                                                                   string editor)
        {
            try
            {

                SqlTransactionManager.Begin();
                DateTime now = DateTime.Now;
                string strSQL = @"update PCBStatus 
                                            set Station= @station ,
                                                   Status= @status,
                                                    TestFailCount=0,
                                                   Editor =@editor,
                                                   Udt=@now 
                                            where PCBNo in ('{0}') ";
                string inSection = string.Join("','", PCBNoList.ToArray());
                strSQL = string.Format(strSQL, inSection);

                logger.Error("UpdatePCBStatus SQL:" + strSQL);

                SqlParameter paraStation = new SqlParameter("@station", SqlDbType.VarChar, 32);
                paraStation.Direction = ParameterDirection.Input;
                paraStation.Value = station;
                SqlParameter paraStatus = new SqlParameter("@status", SqlDbType.Int);
                paraStatus.Direction = ParameterDirection.Input;
                paraStatus.Value = status;
                SqlParameter paraEditor = new SqlParameter("@editor", SqlDbType.VarChar, 32);
                paraEditor.Direction = ParameterDirection.Input;
                paraEditor.Value = editor;
                SqlParameter paraNow = new SqlParameter("@now", SqlDbType.DateTime);
                paraNow.Direction = ParameterDirection.Input;
                paraNow.Value = now;
                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                               CommandType.Text,
                                                               strSQL,
                                                               paraStation, paraStatus, paraEditor, paraNow);

                //Write PCBLog

                strSQL = @"insert PCBLog (PCBNo, PCBModel, Station, Status, Line, Editor, Cdt)
                                    select PCBNo, PCBModelID, @station,@status,@line,@editor,@now 
                                      from PCB 
                                      where PCBNo in ('{0}')";

                strSQL = string.Format(strSQL, inSection);

                SqlParameter paraLine = new SqlParameter("@line", SqlDbType.VarChar, 32);
                paraLine.Direction = ParameterDirection.Input;
                paraLine.Value = "NA";

                SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_GetData,
                                                               CommandType.Text,
                                                               strSQL,
                                                               paraStation,
                                                               paraStatus,
                                                               paraLine,
                                                               paraEditor,
                                                               paraNow);

                SqlTransactionManager.Commit();
            }
            catch (Exception e)
            {
                SqlTransactionManager.Rollback();
                throw e;
            }
        }

        #endregion

        #region

        private DataTable CreateLogTable()
        {
            DataTable newLog = new DataTable("PCBLog");

            // Add PCBLog column objects to the table.
            DataColumn ID = new DataColumn();
            ID.DataType = System.Type.GetType("System.Int32");
            ID.ColumnName = "ID";
            ID.AutoIncrement = true;
            newLog.Columns.Add(ID);    

            DataColumn pcbNo = new DataColumn();
            pcbNo.DataType = System.Type.GetType("System.String");
            pcbNo.ColumnName = "PCBNo";
            newLog.Columns.Add(pcbNo);

            DataColumn modelID = new DataColumn();
            modelID.DataType = System.Type.GetType("System.String");
            modelID.ColumnName = "PCBModel";
            newLog.Columns.Add(modelID);

            DataColumn station = new DataColumn();
            station.DataType = System.Type.GetType("System.String");
            station.ColumnName = "Station";
            newLog.Columns.Add(station);

            DataColumn status = new DataColumn();
            status.DataType = System.Type.GetType("System.Int32");
            status.ColumnName = "Status";
            newLog.Columns.Add(status);

            DataColumn line = new DataColumn();
            line.DataType = System.Type.GetType("System.String");
            line.ColumnName = "Line";
            newLog.Columns.Add(line);

            DataColumn editor = new DataColumn();
            editor.DataType = System.Type.GetType("System.String");
            editor.ColumnName = "Editor";
            newLog.Columns.Add(editor);

            DataColumn cdt = new DataColumn();
            cdt.DataType = System.Type.GetType("System.DateTime");
            cdt.ColumnName = "Cdt";
            newLog.Columns.Add(cdt);

            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = ID;
            newLog.PrimaryKey = keys;            

            // Return the new DataTable. 
            return newLog;

        }

        private void addLog(DataTable dt,
                                          string pcbNo,
                                        string model,
                                        string station,
                                        int status,
                                        string editor,
                                        DateTime now)
        {

            // Add some new rows to the collection. 
            DataRow row = dt.NewRow();
            row["PCBNo"] = pcbNo;
            row["PCBModel"] = model;
            row["Station"] = station;
            row["Line"] = "NA";
            row["Status"] = status;
            row["Editor"] = editor;
            row["Cdt"] = now;
            dt.Rows.Add(row);
            dt.AcceptChanges();
        }
        #endregion
    }
}
