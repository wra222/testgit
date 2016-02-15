using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using IMES.Query.DB;
using System.Data;

namespace IMES.WS.Common
{
    public class SQL
    {
        public static int ConCurrentLock(string name)
        {
            string strSQL = @"select Name, Value, Descr, Editor, Cdt, Udt
                                            from SystemSetting WITH (ROWLOCK,UPDLOCK)
                                            where Name=@name ";
            DateTime st = DateTime.Now;
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@name", 32, name.ToString().Trim()));


            return (DateTime.Now - st).Milliseconds;
        }

        public static string CheckSentData(string TxnId, string MoNumber)
        {
            string strSQL = @" IF EXISTS (SELECT * FROM SendData WHERE TxnId=@TxnId AND KeyValue1=@MoNumber)
                                    SET @CheckSendData='Y'
                               ELSE                                     
                                    SET @CheckSendData='N' ";
            SqlParameter outPara = SQLHelper.CreateSqlParameter("@CheckSendData", 4, "", ParameterDirection.Output);

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                      SQLHelper.CreateSqlParameter("@MoNumber", 32, MoNumber),
                                      outPara);

            return outPara.Value.ToString().Trim();
        }


        public static void InsertTxnDataLog( EnumMsgCategory  Category, 
                                                                  string Action, 
                                                                    string KeyValue1, 
                                                                    string KeyValue2, 
                                                                    string TxnId, 
                                                                    string ErrorCode, 
                                                                    string ErrorDescr, 
                                                                    EnumMsgState State, 
                                                                    string Comment)
        {
            string strSQL = @"INSERT INTO TxnDataLog ([Category],[Action],[KeyValue1],[KeyValue2],[TxnId],
                                                                                        [ErrorCode],[ErrorDescr],[State],[Comment],[Cdt])
                                        VALUES (@Category,@Action,@KeyValue1,@KeyValue2,@TxnId,
                                                        @ErrorCode,@ErrorDescr,@State,@Comment,GETDATE()) ";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                            System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@Category", 16, Category.ToString().Trim()),
                                                             SQLHelper.CreateSqlParameter("@Action", 32, Action),
                                                             SQLHelper.CreateSqlParameter("@KeyValue1", 32, KeyValue1),
                                                             SQLHelper.CreateSqlParameter("@KeyValue2", 32, KeyValue2),
                                                             SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                                             SQLHelper.CreateSqlParameter("@ErrorCode", 32, ErrorCode),
                                                             SQLHelper.CreateSqlParameter("@ErrorDescr", 255, ErrorDescr),
                                                             SQLHelper.CreateSqlParameter("@State", 16, State.ToString().Trim()),
                                                             SQLHelper.CreateSqlParameter("@Comment", 255, Comment));

        }

        public static void InsertTxnDataLog_DB(string connectionName, int dbIndex,
                                                                  EnumMsgCategory Category,
                                                                  string Action,
                                                                    string KeyValue1,
                                                                    string KeyValue2,
                                                                    string TxnId,
                                                                    string ErrorCode,
                                                                    string ErrorDescr,
                                                                    EnumMsgState State,
                                                                    string Comment)
        {
            string strSQL = @"INSERT INTO TxnDataLog ([Category],[Action],[KeyValue1],[KeyValue2],[TxnId],
                                                                                        [ErrorCode],[ErrorDescr],[State],[Comment],[Cdt])
                                        VALUES (@Category,@Action,@KeyValue1,@KeyValue2,@TxnId,
                                                        @ErrorCode,@ErrorDescr,@State,@Comment,GETDATE()) ";

            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionName, dbIndex),
                                                            System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@Category", 16, Category.ToString().Trim()),
                                                             SQLHelper.CreateSqlParameter("@Action", 32, Action),
                                                             SQLHelper.CreateSqlParameter("@KeyValue1", 32, KeyValue1),
                                                             SQLHelper.CreateSqlParameter("@KeyValue2", 32, KeyValue2),
                                                             SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                                             SQLHelper.CreateSqlParameter("@ErrorCode", 32, ErrorCode),
                                                             SQLHelper.CreateSqlParameter("@ErrorDescr", 255, ErrorDescr),
                                                             SQLHelper.CreateSqlParameter("@State", 16, State.ToString().Trim()),
                                                             SQLHelper.CreateSqlParameter("@Comment", 255, Comment));

        }

        public static void InsertSendData_DB(string connectionName, int dbIndex,
                                            string action,
                                            string key1,
                                            string key2,
                                            string txnId,
                                            string comment,
                                            EnumMsgState state,
                                            DateTime udt)
        {

            string strSQL = @"INSERT INTO SendData(Action, KeyValue1, KeyValue2, TxnId, ErrorCode, 
						                           ErrorDescr, State, ResendCount, Comment, Cdt,Udt)
                                            VALUES(@Action, @KeyValue1, @KeyValue2, @TxnId, '', 
		                                            '', @State, 0, @comment, @now,@now)";

            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionName, dbIndex),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@Action", 32, action),
                                      SQLHelper.CreateSqlParameter("@KeyValue1", 32, key1),
                                      SQLHelper.CreateSqlParameter("@KeyValue2", 32, key2),
                                      SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                      SQLHelper.CreateSqlParameter("@State", 32, state.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@comment", 255, comment.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@now", udt));
        }
       
        public static DataTable GetConfirmMoData(string TxnId, string MoNumber)
        {
            string strSQL = @" SELECT DeliveredQty, Model FROM ConfirmMO WHERE TxnId=@TxnId AND MO=@MoNumber";

            return SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId),
                                      SQLHelper.CreateSqlParameter("@MoNumber", 32, MoNumber));

        }


    }
}
