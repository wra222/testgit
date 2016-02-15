using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.NotifyBsamModel
{
    public class SQL
    {

        public static void UpdateBsamModel(string connectionDB, int dbIndex, NotifyBsamModel result)
        {
            string IEC_A_Part = result.IEC_A_Part;
            string HP_A_Part = (result.HP_A_Part == null ? "":result.HP_A_Part);
            string HP_C_SKU = result.HP_C_SKU;

            string strSQL = @"if not exists(select * from BSamModel where A_Part_Model=@IEC_A_Part)
                              begin
                                    insert into BSamModel(A_Part_Model, C_Part_Model, HP_A_Part, HP_C_SKU, QtyPerCarton, Editor, Cdt, Udt)
                                    values (@IEC_A_Part, '', @HP_A_Part, @HP_C_SKU, 0, 'SAP', @Now, @Now)  
                              end";

            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@IEC_A_Part", 20, IEC_A_Part),
                                                SQLHelper.CreateSqlParameter("@HP_A_Part", 20, HP_A_Part),
                                                SQLHelper.CreateSqlParameter("@HP_C_SKU", 20, HP_C_SKU),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }

        public static string GetBsamModelState(string connectionDB, int dbIndex, string SerialNumber)
        {

            string strSQL = @"if not exists(select TxnId from TxnDataLog where TxnId=@SerialNumber and Category='Response')
                              begin
                                  set @ret = 'N'
                              end
                              else
                              begin
                                  select @ret= (case when (State='Success') then 'T' else 'F' end)
                                  from TxnDataLog where TxnId=@SerialNumber and Category='Response'
                              end";
           SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 8, "", ParameterDirection.InputOutput);

           SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@SerialNumber", 30, SerialNumber),
                                                ParaRet);

            return ParaRet.Value.ToString().Trim(); 
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
            string strSQL = @"if not Exists (select * from TxnDataLog where [Action]=@Action and [KeyValue1]=@KeyValue1 and [TxnId]=@TxnId and [Category]=@Category)
                              BEGIN  
                                    INSERT INTO TxnDataLog ([Category],[Action],[KeyValue1],[KeyValue2],[TxnId],
                                                            [ErrorCode],[ErrorDescr],[State],[Comment],[Cdt])
                                    VALUES (@Category,@Action,@KeyValue1,@KeyValue2,@TxnId,
                                            @ErrorCode,@ErrorDescr,@State,@Comment,GETDATE()) 
                              END";

            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionName, dbIndex),
                                                            System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@Category", 16, Category.ToString().Trim()),
                                                             SQLHelper.CreateSqlParameter("@Action", 32, Action),
                                                             SQLHelper.CreateSqlParameter("@KeyValue1", 32, KeyValue1),
                                                             SQLHelper.CreateSqlParameter("@KeyValue2", 32, KeyValue2),
                                                             SQLHelper.CreateSqlParameter("@TxnId", 40, TxnId),
                                                             SQLHelper.CreateSqlParameter("@ErrorCode", 32, ErrorCode),
                                                             SQLHelper.CreateSqlParameter("@ErrorDescr", 255, ErrorDescr),
                                                             SQLHelper.CreateSqlParameter("@State", 16, State.ToString().Trim()),
                                                             SQLHelper.CreateSqlParameter("@Comment", 255, Comment));

        }           
    }
}