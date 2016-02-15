using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.FisBomCompare
{
    public class SQL
    {
        public static string InsertBomCompareHeader(string connectionDB, int dbIndex, 
                                                 string iTxnId, string iPlant, string iModel, string Status, string Editor)
        {
            string TxnId = (iTxnId == null ? "" : iTxnId);
            string Plant = (iPlant == null ? "" : iPlant);
            string Model = (iModel == null ? "" : iModel);

            string strSQL = @"insert into BOM_Compare_Header(TxnId, Plant, Model, Status, Editor, Cdt, Udt)
                              values (@TxnId, @Plant, @Model, @Status, @Editor, @Now, @Now) 
                              SELECT @ret = @@IDENTITY";

            SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 255, "", ParameterDirection.InputOutput);

            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                                SQLHelper.CreateSqlParameter("@Plant", 8, Plant),
                                                SQLHelper.CreateSqlParameter("@Model", 16, Model),
                                                SQLHelper.CreateSqlParameter("@Status", 16, Status),
                                                SQLHelper.CreateSqlParameter("@Editor", 64, Editor),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now),
                                                ParaRet);
            return ParaRet.Value.ToString().Trim(); 
        }

        public static void UpdateBomCompareHeader(string connectionDB, int dbIndex,
                                                 string TxnId, string Status, string ErrorDescr, string Editor)
        {
            string strSQL = @"update BOM_Compare_Header
                              set Status=@Status, ErrorDescr=@ErrorDescr,
                                  Editor=@Editor, Udt=@Now
                              where TxnId = @TxnId ";


            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                                SQLHelper.CreateSqlParameter("@Status", 16, Status),
                                                SQLHelper.CreateSqlParameter("@Editor", 64, Editor),
                                                SQLHelper.CreateSqlParameter("@ErrorDescr", ErrorDescr),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }

        public static void UpdateBomCompareHeaderByID(string connectionDB, int dbIndex,
                                                 string ID, string Status, string ErrorDescr , string Editor)
        {
            string strSQL = @"update BOM_Compare_Header
                              set Status=@Status, ErrorDescr=@ErrorDescr,
                                  Editor=@Editor, Udt=@Now
                              where ID = CAST(@ID as int) ";


            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@ID", 32, ID),
                                                SQLHelper.CreateSqlParameter("@Status", 16, Status),
                                                SQLHelper.CreateSqlParameter("@Editor", 64, Editor),
                                                SQLHelper.CreateSqlParameter("@ErrorDescr", ErrorDescr),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }

        public static void InsertBomCompareItem(string connectionDB, int dbIndex,
                                                 string TxnId, string ItemNo, string AltGroup, string Component, string Qty, string Unit, string SAPErrorDescr, string Editor)
        {

            string strSQL = @"insert into BOM_Compare_Item(TxnId, ItemNo, AltGroup, Component, Qty, Unit, SAPErrorDescr, Editor, Cdt, Udt)
                              values (@TxnId, SUBSTRING('0000', 1, (4 - len(@ItemNo)))+CONVERT(varchar(4),@ItemNo), 
                                      @AltGroup, @Component, @Qty, @Unit, @SAPErrorDescr, @Editor, @Now, @Now) ";


            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                                SQLHelper.CreateSqlParameter("@ItemNo", 8, ItemNo),
                                                SQLHelper.CreateSqlParameter("@AltGroup", 4, AltGroup),
                                                SQLHelper.CreateSqlParameter("@Component", 32, Component),
                                                SQLHelper.CreateSqlParameter("@Qty", 16, Qty),
                                                SQLHelper.CreateSqlParameter("@Unit", 8, Unit),
                                                SQLHelper.CreateSqlParameter("@SAPErrorDescr", SAPErrorDescr),
                                                SQLHelper.CreateSqlParameter("@Editor", 64, Editor),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }
        public static void BulkInsertBomCompareItem(string connectionDB, int dbIndex, DataTable dt)
        {
            string DBConnectionStr = SQLHelper.GetDBConnectionString(connectionDB, dbIndex);
            using (SqlConnection conn = new SqlConnection(DBConnectionStr))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = 5000;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.BOM_Compare_Item";

                        bulkCopy.WriteToServer(dt);
                    }
                    transaction.Commit();
                }
                conn.Close();
            }
        }

        public static void BulkInsertSendData(string connectionDB, int dbIndex, DataTable dt)
        {
            string DBConnectionStr = SQLHelper.GetDBConnectionString(connectionDB, dbIndex);
            using (SqlConnection conn = new SqlConnection(DBConnectionStr))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        bulkCopy.BatchSize = 5000;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.SendData";

                        bulkCopy.WriteToServer(dt);
                    }
                    transaction.Commit();
                }
                conn.Close();
            }
        }

        public static void UpdateBomCompareItem(string connectionDB, int dbIndex,
                                                string TxnId, string ItemNo, string SAPErrorDescr, string Editor)
        {

            string strSQL = @"update BOM_Compare_Item
                                set SAPErrorDescr=@SAPErrorDescr, Editor=@Editor, Udt=@Now
                              where TxnId=@TxnId and ItemNo=@ItemNo";
            
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                                SQLHelper.CreateSqlParameter("@ItemNo", 8, ItemNo),
                                                SQLHelper.CreateSqlParameter("@SAPErrorDescr", SAPErrorDescr),
                                                SQLHelper.CreateSqlParameter("@Editor", 64, Editor),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }
/*
        public static List<FisBomList> GetFisBomData(string connectionDB, int dbIndex, string spName, string Model)
        {
            List<FisBomList> retList = new List<FisBomList>();

            string strSQL = @"exec @spName @Model";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                    SQLHelper.CreateSqlParameter("@spName", spName),
                                                    SQLHelper.CreateSqlParameter("@Model", 16, Model));
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                FisBomList item = new FisBomList();
                item.ItemNo = i;
                item.AltGroup = (dr["AltGroup"] == null ? "":dr["AltGroup"].ToString().Trim());
                item.Component = dr["Component"].ToString().Trim();
                item.Qty = (int)dr["Qty"];
                item.Unit = dr["Unit"].ToString().Trim();

                retList.Add(item);

                i++;
            }
            return retList;
        }
*/
        public static List<FisBomItem> GetFisBomData(string connectionDB, int dbIndex, string spName, string Model, string TxnId)
        {
            List<FisBomItem> retList = new List<FisBomItem>();

            string strSQL = @"exec @spName @Model";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                    SQLHelper.CreateSqlParameter("@spName", spName),
                                                    SQLHelper.CreateSqlParameter("@Model", 16, Model));
            int i = 1;
            int iQty = 0;
            foreach (DataRow dr in dt.Rows)
            {
                FisBomItem item = new FisBomItem();
                item.TxnId = TxnId;
                item.ItemNo = i.ToString().PadLeft(4, '0');
                item.AltGroup = (dr["AltGroup"] == null ? "" : dr["AltGroup"].ToString().Trim());
                item.Component = dr["Component"].ToString().Trim();
                iQty = (int)dr["Qty"];
                item.Qty = iQty.ToString() + ".000";
                item.Unit = dr["Unit"].ToString().Trim();
                item.SAPErrorDescr = "";
                item.Editor = "FIS";
                item.Cdt = DateTime.Now;
                item.Udt = DateTime.Now;

                retList.Add(item);

                i++;
            }
            return retList;
        }

        public static List<FisBomItem> GetFisBomItem(string connectionDB, int dbIndex, string TxnId)
        {
            List<FisBomItem> retList = new List<FisBomItem>();

            string strSQL = @"select TxnId, ItemNo, AltGroup, Component, Qty, Unit, 
                                     SAPErrorDescr, Editor, Cdt, Udt
                              from BOM_Compare_Item
                              where TxnId=@TxnId 
                              order by ItemNo";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                    SQLHelper.CreateSqlParameter("@TxnId", TxnId));
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                FisBomItem item = new FisBomItem();
                //item.ID = (int)dr["ID"];
                item.TxnId  = TxnId;
                item.ItemNo = dr["ItemNo"].ToString().Trim(); 
                item.AltGroup = (dr["AltGroup"] == null ? "" : dr["AltGroup"].ToString().Trim());
                item.Component = dr["Component"].ToString().Trim();
                item.Qty = dr["Qty"].ToString().Trim();
                item.Unit = dr["Unit"].ToString().Trim();
                item.SAPErrorDescr = dr["SAPErrorDescr"].ToString().Trim();
                item.Editor = dr["Editor"].ToString().Trim();
                item.Cdt = (DateTime)dr["Cdt"];
                item.Udt = (DateTime)dr["Udt"];

                retList.Add(item);

                i++;
            }
            return retList;
        }
        
        public static string GetBomHeaderStatus(string connectionDB, int dbIndex, string TxnId)
        {

           string strSQL = @"select  @ret= Status from BOM_Compare_Header where TxnId=@TxnId";

           SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 255, "", ParameterDirection.InputOutput);

           SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                                ParaRet);

            return ParaRet.Value.ToString().Trim(); 
        }           
    }

    public class FisBomList
    {
        public int ItemNo;
        public string AltGroup;
        public string Component;
        public int Qty;
        public string Unit;
    }

    public class FisBomHeader
    {
        public string TxnId;
        public string Plant;
        public string Model;
        public string Status;
        public string Editor;
        public DateTime Cdt;
        public DateTime Udt;
    }
    public class FisBomItem
    {
        private int id = 0;
        public int ID { get { return id; } }
        public string TxnId { get; set; }
        public string ItemNo { get; set; }
        public string AltGroup { get; set; }
        public string Component { get; set; }
        public string Qty { get; set; }
        public string Unit { get; set; }
        public string SAPErrorDescr { get; set; }
        public string Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
    }
    public class SendData
    {
        private int id = 0;
        public int ID { get { return id; } }
        public string Action { get; set; }
        public string KeyValue1 { get; set; }
        public string KeyValue2 { get; set; }
        public string TxnId { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescr { get; set; }
        public string State { get; set; }
        public int ResendCount { get; set; }
        public string Comment { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
    }
}