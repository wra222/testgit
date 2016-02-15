using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.NotifyStdWeight
{
    public class SQL
    {

        public static void UpdateStdWeight(string connectionDB, int dbIndex, NotifyStdWeight result)
        {
            string Model = (result.Model == null ? "":result.Model);
            string GrossWeight = (result.GrossWeight == null ? "":result.GrossWeight);

            string strSQL = @"if not exists(select * from PAK_SkuMasterWeight_FIS where Model=@Model)
                              begin
                                    insert into PAK_SkuMasterWeight_FIS(Model, Weight, Cdt)
                                    values (@Model, @GrossWeight, @Now)  
                              end
                              else
                              begin
                                     update PAK_SkuMasterWeight_FIS
                                     set Weight=@GrossWeight, Cdt=@Now
                                     where Model=@Model
                              end";

            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@Model", 20, Model),
                                                SQLHelper.CreateSqlParameter("@GrossWeight", 20, GrossWeight),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }

        public static string GetStdWeightState(string connectionDB, int dbIndex, string SerialNumber)
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