using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.DefectComponentUpdate
{
    public class SQL
    {
        public static List<SAPWeightDef> GetSAPWeightDef(string connectStr, string msgType)
        {
            List<SAPWeightDef> retList = new List<SAPWeightDef>();

            string strSQL = @"select ID, PlantCode, WeightUnit, VolumnUnit, ConnectionStr, 
                                                      DBName,  MsgType, Descr, Cdt, Udt 
                                            from SAPWeightDef
                                            where MsgType=@MsgType  
                                            order by ID";
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.GetDBConnectionString(connectStr, 0),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@MsgType", 16, msgType));
            foreach (DataRow dr in dt.Rows)
            {
                SAPWeightDef item = new SAPWeightDef();
                item.ID = (int)dr["ID"];
                item.PlantCode = dr["PlantCode"].ToString().Trim();
                item.WeightUnit = dr["WeightUnit"].ToString().Trim();
                item.VolumnUnit = dr["VolumnUnit"] == null ? string.Empty : dr["VolumnUnit"].ToString().Trim();
                item.ConnectionStr = dr["ConnectionStr"].ToString().Trim();
                item.DBName = dr["DBName"].ToString().Trim();
                item.MsgType = dr["MsgType"].ToString().Trim();
                item.Descr = dr["Descr"].ToString().Trim();
                item.Cdt = (DateTime)dr["Cdt"];
                item.Udt = (DateTime)dr["Udt"];

                retList.Add(item);
            }

            return retList;
        }

        public static string CheckDCBatchID(string connectionStr, string DBName, string BatchID)
        {
            string connectStr = string.Format(connectionStr, DBName);
            string strSQL = @"select @Status = Status from DefectComponentBatchStatus where BatchID = @BatchID";

            SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@Status", 4, "", ParameterDirection.InputOutput);

            SQLHelper.ExecuteDataFill(connectStr,
            //SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectStr, 0),
                                                 System.Data.CommandType.Text,
                                                 strSQL,
                                                 SQLHelper.CreateSqlParameter("@BatchID", 20, BatchID),
                                                 ParaRet);

            return ParaRet.Value.ToString().Trim();
        }

        public static string CheckDCPartSerialNo(string connectionStr, string DBName, string BatchID, string PartSn)
        {
            string connectStr = string.Format(connectionStr, DBName);
            string strSQL = @"if exists(select * from DefectComponent
                                        where BatchID = @BatchID and PartSn = @PartSn)
                              begin
                                set @Status = '1'
                              end
                              else
                              begin
                                set @Status = ''
                              end";

            SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@Status", 4, "", ParameterDirection.InputOutput);

            SQLHelper.ExecuteDataFill(connectStr,
                //SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectStr, 0),
                                                 System.Data.CommandType.Text,
                                                 strSQL,
                                                 SQLHelper.CreateSqlParameter("@BatchID", 20, BatchID),
                                                 SQLHelper.CreateSqlParameter("@PartSn", 16, PartSn),
                                                 ParaRet);

            return ParaRet.Value.ToString().Trim();
        }

        //public static void UpdateDCStatusByPartSerialNo(string connectionStr, string DBName, string BatchID, string PartSerialNo, string Status, string Editor)
        public static void UpdateDCStatusByPartSerialNo(string connectionStr, string DBName, string BatchID, string PartSn, string Status, string Editor)
        {
            string connectStr = string.Format(connectionStr, DBName);

            string strSQL = @"Update DefectComponent
                                    set Status = @Status, Editor= @Editor, Udt = @Now
                                where BatchID = @BatchID and PartSn =@PartSn

                                insert into DefectComponentLog(ActionName, ComponentID, RepairID, PartSn, 
                                                               Customer, Model, Family, DefectCode, DefectDescr, 
                                                               ReturnLine, Remark, BatchID, Comment, 
                                                               [Status], Editor, Cdt)
                                select 'UpdateStatusByIQS' as ActionName, ID, RepairID, PartSn,
                                       Customer, Model, Family, DefectCode, DefectDescr, 
                                       ReturnLine, 'WSDefectComponentUpdate' as Remark, BatchID, '' as Comment,
                                       @Status as [Status], @Editor as Editor, @Now as Cdt
                                from DefectComponent 
                                where BatchID = @BatchID and PartSn = @PartSn";

            //where BatchID = @BatchID and ID = convert(int, RIGHT(@PartSerialNo, 9))";

            SQLHelper.ExecuteDataFill(connectStr,
                //SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionStr, 0),
                                                            System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@BatchID", 20, BatchID),
                                                             //SQLHelper.CreateSqlParameter("@PartSerialNo", 16, @PartSerialNo),
                                                             SQLHelper.CreateSqlParameter("@PartSn", 16, @PartSn),
                                                             SQLHelper.CreateSqlParameter("@Status", 4, Status),
                                                             SQLHelper.CreateSqlParameter("@Editor", 32, Editor),
                                                             SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }

        public static void UpdateDCBatchStatusByBatchID(string connectionStr, string DBName, string BatchID, string Status, string Editor)
        {
            string connectStr = string.Format(connectionStr, DBName);

            string strSQL = @"if exists (select * from DefectComponentBatchStatus where BatchID=@BatchID and Status in ('00', '10'))
                               begin
                                    Update DefectComponentBatchStatus
                                    set Status = '11', Editor= @Editor, Udt = @Now
                                    where BatchID = @BatchID

                                    insert into DefectComponentBatchStatusLog(BatchID, Status, Editor, Cdt)
                                    values(@BatchID, '11', @Editor, @Now)
                               end
                               
                               if not exists (select * from DefectComponent where BatchID=@BatchID and Status not in ('20', '21', '22', '23'))
                               begin
                                    Update DefectComponentBatchStatus
                                    set Status = '30', Editor= @Editor, Udt = @Now
                                    where BatchID = @BatchID

                                    insert into DefectComponentBatchStatusLog(BatchID, Status, Editor, Cdt)
                                    values(@BatchID, '30', @Editor, @Now)
                               end";

            SQLHelper.ExecuteDataFill(connectStr,
                //SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionStr, 0),
                                                            System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@BatchID", 20, BatchID),
                                                             SQLHelper.CreateSqlParameter("@Status", 4, Status),
                                                             SQLHelper.CreateSqlParameter("@Editor", 32, Editor),
                                                             SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }
   
    }

    public class SAPWeightDef
    {
        public int ID;
        public string PlantCode;
        public string WeightUnit;
        public string VolumnUnit;
        public string ConnectionStr;
        public string DBName;
        public string MsgType;
        public string Descr;
        public DateTime Cdt;
        public DateTime Udt;
    }
}