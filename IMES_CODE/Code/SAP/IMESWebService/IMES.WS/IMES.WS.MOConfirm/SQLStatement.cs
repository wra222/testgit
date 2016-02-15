using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace IMES.WS.MOConfirm
{
    public class SQL
    {        
        public static string UpdateStatus(MoConfirmResult CFResult)
        {
            string Message = "";
            string TxnId = CFResult.SerialNumber;
            string MoNumber = CFResult.MoNumber;
            string Result = CFResult.Result;
            SqlParameter outResult = SQLHelper.CreateSqlParameter("@ReturnMessage", 255, "", ParameterDirection.Output);
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                    System.Data.CommandType.StoredProcedure,
                                    "SAP_MOConfirmResult",
                                    SQLHelper.CreateSqlParameter("@Function", 32, "MOConfirmResult"),
                                    SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId.Trim()),
                                    SQLHelper.CreateSqlParameter("@MoNumber", 20, MoNumber.Trim()),
                                    SQLHelper.CreateSqlParameter("@Result", 20, Result.Trim()),
                                    outResult);

            Message = outResult.Value.ToString();
            return Message;

        }

        public static void UpdateSAPMOStatus(string moId,
                                                                 string status,
                                                                  int qty)
        {


            string strSQL = @"UPDATE MO 
                                          SET SAPStatus=@SAPStatus,SAPQty=@SAPQty,Udt=getdate()
                                          WHERE MO=@MO";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                              System.Data.CommandType.Text,
                                                              strSQL,
                                                              SQLHelper.CreateSqlParameter("@MO", 20, moId),
                                                              SQLHelper.CreateSqlParameter("@SAPStatus", 20, status),
                                                              SQLHelper.CreateSqlParameter("@SAPQty", qty));

        }
        //Need modify .....
        public static void UpdateMOStatus(string moId,
                                                                 string function,
                                                                 string action,
                                                                 string status,
                                                                 string comment,
                                                                string txnId,
                                                                  string editor)
        {


            string strSQL = @"insert MOStatusLog(MO, [Function], Action, Station, PreStatus, Status, IsHold, HoldCode, TxnId, Comment, Editor, Cdt)
                                            select MO,@Function,@Action,'', Status,@Status, IsHold, HoldCode,@TxnId, @Comment, @Editor,  @Now
                                             from MOStatus
                                            where  MO =@MO;
                                            update MOStatus
                                            set Status=@Status,
                                                Comment =@Comment,
                                                LastTxnId=@TxnId, 
                                                Editor =@Editor,    
                                                Udt=@Now
                                            where  MO =@MO; ";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                              System.Data.CommandType.Text,
                                                              strSQL,
                                                              SQLHelper.CreateSqlParameter("@MO", 20, moId),
                                                              SQLHelper.CreateSqlParameter("@Function", 32, function),
                                                              SQLHelper.CreateSqlParameter("@Action", 32, action),
                                                              SQLHelper.CreateSqlParameter("@Status", 32, status),
                                                              SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                                              SQLHelper.CreateSqlParameter("@Comment", 255, comment),
                                                              SQLHelper.CreateSqlParameter("@Editor", 32,editor),
                                                              SQLHelper.CreateSqlParameter("@Now", DateTime.Now));

        }

        public static DataTable  QueryMOStatus(string moId)
        {
            string strSQL = @"select a.MO, a.Plant, a.Model, a.Qty, 
                                               a.SAPQty , a.SAPStatus, a.Print_Qty,
                                               a.Transfer_Qty, b.Status 
                                        from MO a, MOStatus b 
                                        where a.MO= b.MO
                                          and a.MO =@MO";
           return SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                                                   System.Data.CommandType.Text,
                                                                                   strSQL,
                                                                                    SQLHelper.CreateSqlParameter("@MO",32, moId));
            
        }
    }
}