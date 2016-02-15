using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.DefectComponentDetail
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

        public static List<DCDetail> QueryDCDetailByBatchID(string connectionStr, string DBName, string BatchID)
        {
            string connectStr = string.Format(connectionStr, DBName);

            List<DCDetail> retList = new List<DCDetail>();

            string strSQL = @"select a.BatchID, a.Family, a.IECPn, a.PartType, a.Vendor, a.DefectCode, a.DefectDescr,
                                     a.PartSn, (b.Value+dbo.fn_AddZero(convert(varchar(10), a.ID), 'L', 9)) as PartSerialNo
                              from DefectComponent a
                              inner join SysSetting b on Name='CustomerCode'
                              where a.BatchID = @BatchID
                              order by a.Family, a.IECPn, a.Vendor, a.DefectCode";            
            
            DataTable dt = SQLHelper.ExecuteDataFill(connectStr,
            //DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.GetDBConnectionString(connectStr, 0),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@BatchID", 20, BatchID));
            foreach (DataRow dr in dt.Rows)
            {
                DCDetail item = new DCDetail();
                item.BatchID = dr["BatchID"].ToString().Trim();
                item.Family = dr["Family"].ToString().Trim();
                item.IECPn = dr["IECPn"].ToString().Trim();
                item.PartType = dr["PartType"].ToString().Trim();
                item.Vendor = dr["Vendor"].ToString().Trim();
                item.DefectCode = dr["DefectCode"].ToString().Trim();
                item.DefectDescr = dr["DefectDescr"].ToString().Trim();
                item.PartSn = dr["PartSn"].ToString().Trim();
                item.PartSerialNo = dr["PartSerialNo"].ToString().Trim();

                retList.Add(item);
            }

            return retList;
        }

        public static List<DCDetail> QueryDCDetailByCondition(string connectionStr, string DBName, string BatchID, string Family, string IECPn, string Vendor, string DefectCode)
        {
            string connectStr = string.Format(connectionStr, DBName);

            List<DCDetail> retList = new List<DCDetail>();

            string strSQL = @"select a.BatchID, a.Family, a.IECPn, a.PartType, a.Vendor, a.DefectCode, a.DefectDescr,
                                     a.PartSn, (b.Value+dbo.fn_AddZero(convert(varchar(10), a.ID), 'L', 9)) as PartSerialNo
                              from DefectComponent a
                              inner join SysSetting b on Name='CustomerCode'
                              where a.BatchID = @BatchID and
                                    a.Family = @Family and a.IECPn = @IECPn and
                                    a.Vendor = @Vendor and a.DefectCode = @DefectCode
                              order by a.Family, a.IECPn, a.Vendor, a.DefectCode";

            DataTable dt = SQLHelper.ExecuteDataFill(connectStr,
            //DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.GetDBConnectionString(connectStr, 0),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@BatchID", 20, BatchID),
                                                     SQLHelper.CreateSqlParameter("@Family", 50, Family),
                                                     SQLHelper.CreateSqlParameter("@IECPn", 32, IECPn),
                                                     SQLHelper.CreateSqlParameter("@Vendor", 32, Vendor),
                                                     SQLHelper.CreateSqlParameter("@DefectCode", 10, DefectCode));
            foreach (DataRow dr in dt.Rows)
            {
                DCDetail item = new DCDetail();
                item.BatchID = dr["BatchID"].ToString().Trim();
                item.Family = dr["Family"].ToString().Trim();
                item.IECPn = dr["IECPn"].ToString().Trim();
                item.PartType = dr["PartType"].ToString().Trim();
                item.Vendor = dr["Vendor"].ToString().Trim();
                item.DefectCode = dr["DefectCode"].ToString().Trim();
                item.DefectDescr = dr["DefectDescr"].ToString().Trim();
                item.PartSn = dr["PartSn"].ToString().Trim();
                item.PartSerialNo = dr["PartSerialNo"].ToString().Trim();

                retList.Add(item);
            }

            return retList;
        }

        public static void UpdateDCBstchStatusByBatchID(string connectionStr, string DBName, string BatchID, string Status, string Editor)
        {
            string connectStr = string.Format(connectionStr, DBName);

            string strSQL = @"if exists (select * from DefectComponentBatchStatus where BatchID = @BatchID and Status='00')
                              begin
                                  Update DefectComponentBatchStatus
                                  set Status = @Status, Editor = @Editor, Udt = @Now
                                  where BatchID = @BatchID
                              end

                              insert into DefectComponentBatchStatusLog(BatchID, Status, Editor, Cdt)
                              values(@BatchID, @Status, @Editor, @Now)";

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