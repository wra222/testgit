using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.Service.Common;

namespace IMES.Service.MO
{
    public class CompleteMO
    {
        public string MOId;
        public string IsParentMO;
        public int Qty=0;
    }

    public class MoConfirmChange
    {
        public string MoNumber;
        public string SerialNumber;
        public string Result;
    }   


    public class SQL
    {
        public static bool CheckConfirmMORunning(string name)
        {
            string strSQL = @"select Name, Value, Descr, Editor, Cdt, Udt
                                            from SystemSetting WITH (ROWLOCK,UPDLOCK)
                                            where Name=@name ";
                        
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@name", 32, name.ToString().Trim()));
            if (dt.Rows.Count == 0)
            {
                return true;
            }

            foreach (DataRow dr in dt.Rows)
            {
                if (dr[1].ToString() == "F")
                {
                    return false;
                }
                break;
            }

            return true;
        }

        public static void UpdateConfirmMORunning(string name,string value)
        {

            string strSQL = @" update SystemSetting
                                           set Value =@value,
                                                 Udt =getdate() 
                                          where Name=@name";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                                             System.Data.CommandType.Text,
                                                             strSQL,
                                                             SQLHelper.CreateSqlParameter("@name", 32, name.ToString().Trim()),
                                                            SQLHelper.CreateSqlParameter("@value", 32, value.ToString().Trim()));
           
        }

       

        public static List<CompleteMO> GetCompleteProductMO(EnumProductMOState status, string isRework)
        {
            List<CompleteMO> moList = new List<CompleteMO>();
            string strSQL = @"SELECT MO, IsParentMO, count(MO) AS Qty 
                                              FROM ProductMO
                                              WHERE Status=@Status AND IsRework=@Rework
                                              GROUP BY IsParentMO, MO
                                              ORDER BY IsParentMO, MO ";             

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@Status", 20, status.ToString().Trim()),
                                                     SQLHelper.CreateSqlParameter("@Rework", 1, isRework));
            foreach( DataRow dr in dt.Rows)
            {

                moList.Add(new CompleteMO
                                            {
                                                MOId = dr["MO"].ToString().Trim(),
                                                IsParentMO = dr["IsParentMO"].ToString().Trim(),
                                                Qty = (int)dr["Qty"]
                                            });  
            }

            return moList;
        }

        public static int CaculateConfirmMO(string moId, 
                                            string txnId , 
                                            string isParentMO,
                                            out List<string> productIdList, 
                                            out string errorDescr)
        {            
            productIdList = new List<string>();
            errorDescr="";
            SqlParameter retPara = SQLHelper.CreateSqlParameter("@Ret", 0, ParameterDirection.ReturnValue);
            SqlParameter errorPara = SQLHelper.CreateSqlParameter("@ReturnMessage", 255, "", ParameterDirection.Output);

            //return >=0:success
            //          < 0:Fail  need output ErrorDescr 
            // if success  case: 
            //     1. insert ConfirmMO and ConfirmMaterial table and 
            //     2.  return @Ret>=0 
            //     3. return ProductID datatable
            //if Fail case:
            //     1.  return @Ret<0
            //      2. set @ErrorDescr = error message
 
            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.StoredProcedure,
                                                     "SAP_MOConfirm",
                                                     SQLHelper.CreateSqlParameter("@MoNumber", 20, moId),
                                                     SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                                     SQLHelper.CreateSqlParameter("@IsParentMO", 1, isParentMO),
                                                     errorPara,
                                                     retPara);
            int retValue= (int)retPara.Value;

            errorDescr = ((string)(errorPara.Value)).Trim();

            if (retValue >= 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    productIdList.Add(dr[0].ToString().Trim());
                }
            }         

            return retValue;
        }


        public static DataTable GetConfirmMO(string moId,string txnId)
        {
            string strSQL = @"SELECT TOP 1 ID, MO, TxnId, MOType, Unit, Model, DeliveredQty, ConfirmDate, Cdt 
                              FROM ConfirmMO 
                              WHERE MO =@MO AND TxnId=@TxnId
                              ORDER BY Cdt DESC";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@MO", 20, moId),
                                                     SQLHelper.CreateSqlParameter("@TxnId", 32, txnId));
            return dt;
        }

        public static DataTable GetConfirmMOMaterial(int confirmMOId)
        {

            string strSQL = @"SELECT ID, ConfirmMOId, PartNo, MOItem, Reservation, 
                                     ResvItem, WithdrawQty, Unit, AltGroup, ParentMaterial, 
                                     Cdt
                              FROM ConfirmMaterial
                              WHERE  ConfirmMOId= @ConfirmMOId
                              ORDER  BY ParentMaterial, AltGroup";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@ConfirmMOId", confirmMOId));
            return dt;
        }


        public static void InsertSendDataAndLog(EnumMsgCategory Category,
                                               string action,
                                               string key1,
                                               string key2,
                                               string txnId,
                                               EnumMsgState state,
                                               DateTime udt)
        {

            string strSQL = @"INSERT INTO SendData(Action, KeyValue1, KeyValue2, TxnId, ErrorCode, 
						                           ErrorDescr, State, ResendCount, Comment, Cdt,Udt)
                                            VALUES(@Action, @KeyValue1, @KeyValue2, @TxnId, '', 
		                                            '', @State, 0, '', @now,@now)
                                
                              INSERT INTO TxnDataLog(Category, Action, KeyValue1, KeyValue2, TxnId, 
					                                 ErrorCode, ErrorDescr, State, Comment, Cdt)
                                              VALUES(@Category, @Action, @KeyValue1, @KeyValue2, @TxnId, 
                                                     '','', @State, '', @now)";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@Action", 32, action),
                                      SQLHelper.CreateSqlParameter("@KeyValue1", 32, key1),
                                      SQLHelper.CreateSqlParameter("@KeyValue2", 32, key2),
                                      SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                      SQLHelper.CreateSqlParameter("@State", 32, state.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@now", udt),
                                      SQLHelper.CreateSqlParameter("@Category", 32, Category.ToString().Trim()));            
        }

        public static void InsertSendDataAndLog(EnumMsgCategory Category,
                                              string action,
                                              string key1,
                                              string key2,
                                              string txnId,
                                              string errorCode,
                                              string errorDescr,
                                              EnumMsgState state,
                                              DateTime udt)
        {

            string strSQL = @"INSERT INTO SendData(Action, KeyValue1, KeyValue2, TxnId, ErrorCode, 
						                           ErrorDescr, State, ResendCount, Comment, Cdt,Udt)
                                            VALUES(@Action, @KeyValue1, @KeyValue2, @TxnId, @ErrorCode, 
		                                            @ErrorDescr, @State, 0, '', @now,@now)
                                
                              INSERT INTO TxnDataLog(Category, Action, KeyValue1, KeyValue2, TxnId, 
					                                 ErrorCode, ErrorDescr, State, Comment, Cdt)
                                              VALUES(@Category, @Action, @KeyValue1, @KeyValue2, @TxnId, 
                                                      @ErrorCode,@ErrorDescr, @State, '', @now)";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@Action", 32, action),
                                      SQLHelper.CreateSqlParameter("@KeyValue1", 32, key1),
                                      SQLHelper.CreateSqlParameter("@KeyValue2", 32, key2),
                                      SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                      SQLHelper.CreateSqlParameter("@State", 32, state.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@now", udt),
                                      SQLHelper.CreateSqlParameter("@ErrorCode", 32, errorCode.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@ErrorDescr", 255, errorDescr.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@Category", 32, Category.ToString().Trim()));
        }

        public static void UpdateSendDataAndInsertLog(EnumMsgCategory Category,
                                                      string action,
                                                      string key1,
                                                      string key2,
                                                      string txnId,
                                                      string errorCode,
                                                      string errorDescr,
                                                      EnumMsgState state,
                                                      DateTime udt)
        {

            string strSQL = @"UPDATE SendData
                              SET State =@State,
                                  Udt =@now,
                                  ErrorCode =@ErrorCode,
                                  ErrorDescr=@ErrorDescr
                              WHERE Action =@Action AND TxnId =@TxnId  
                                        
                            INSERT INTO TxnDataLog(Category, Action, KeyValue1, KeyValue2, TxnId, 
					                               ErrorCode, ErrorDescr, State, Comment, Cdt)
                                            VALUES(@Category, @Action, @KeyValue1, @KeyValue2, @TxnId, 
                                                    @ErrorCode,@ErrorDescr, @State, '', @now)";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@Action", 32, action),
                                      SQLHelper.CreateSqlParameter("@KeyValue1", 32, key1),
                                      SQLHelper.CreateSqlParameter("@KeyValue2", 32, key2),
                                      SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                      SQLHelper.CreateSqlParameter("@State", 32, state.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@now", udt),
                                      SQLHelper.CreateSqlParameter("@Category", 32, Category.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@ErrorCode", 32, errorCode),
                                      SQLHelper.CreateSqlParameter("@ErrorDescr", 255, errorDescr));
        }


        public static void UpdateProductMOStatusAndInsertLog(string function,
                                                             string action,
                                                             List<string> productIdList,
                                                             EnumProductMOState state,
                                                             string txnId,
                                                             string station,
                                                             DateTime udt)
        {
            string strSQL = @" UPDATE ProductMO
                               SET Status=@State,
                                   LastTxnId =@LastTxnId,
                                   Udt =@now
                               WHERE ProductID in ({0})
                                                 
                               INSERT ProductMOLog
                                (ProductID, MO, Function, Action, IsParentMO,Station, PreStatus, Status, IsRework, IsHold, 
                                      TxnId, Comment, Editor, Cdt)
                               SELECT ProductID, MO, @Function, @Action, IsParentMO,@Station, Status, @State, IsRework, IsHold, 
		                              @LastTxnId, @now, @now
                               FROM  ProductMO
                               WHERE ProductID in ({0}) ";

            string inSQLStr="'" + string.Join("','", productIdList.ToArray()) + "'";
            strSQL = string.Format(strSQL,inSQLStr);

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@Function", 32, function),
                                      SQLHelper.CreateSqlParameter("@Action", 32, action),                                                             
                                      SQLHelper.CreateSqlParameter("@LastTxnId", 32, txnId),
                                      SQLHelper.CreateSqlParameter("@Station", 32, station),
                                      SQLHelper.CreateSqlParameter("@State", 32, state.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@now", udt));
        }

        public static void UpdateProductMOStatusAndInsertLogByComplete(string function,
                                                                       string action,
                                                                       List<string> productIdList,
                                                                       EnumProductMOState state,
                                                                       string txnId,
                                                                       string station,
                                                                       DateTime udt,
                                                                       string editor,
                                                                       EnumProductMOState CheckCompletedState,
                                                                       string moId)
        {
            string strSQL = @"UPDATE ProductMO
                             SET Status=@State,
                                 LastTxnId =@LastTxnId,
                                 Udt =@now
                             WHERE ProductID in ({0}) AND MO=@moId AND Status=@CompleteState
                             
                            INSERT ProductMOLog(ProductID, MO, [Function], [Action], IsParentMO, 
                                                 Station, PreStatus, Status, IsRework, IsHold, 
                                                 TxnId, Comment, Editor, Cdt)
                            SELECT ProductID, MO, @Function, @Action, IsParentMO, 
                                    @Station, @CompleteState, @State, IsRework, IsHold, 
                                    @LastTxnId,'',@editor , @now
                            FROM  ProductMO
                            WHERE ProductID in ({0}) AND MO=@moId AND Status=@State";

            string inSQLStr = "'" + string.Join("','", productIdList.ToArray()) + "'";
            strSQL = string.Format(strSQL, inSQLStr);

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@Function", 32, function),
                                      SQLHelper.CreateSqlParameter("@Action", 32, action),
                                      SQLHelper.CreateSqlParameter("@LastTxnId", 32, txnId),
                                      SQLHelper.CreateSqlParameter("@Station", 32, station),
                                      SQLHelper.CreateSqlParameter("@State", 32, state.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@editor",32, editor),
                                      SQLHelper.CreateSqlParameter("@now",udt),
                                      SQLHelper.CreateSqlParameter("@moId",32, moId),
                                      SQLHelper.CreateSqlParameter("@CompleteState",32, CheckCompletedState.ToString().Trim()));
                                      
        }


        public static void UpdateProductMOReworkStatusAndInsertLog(string function,
                                                                   string action,
                                                                   List<string> productIdList,
                                                                   string txnId,
                                                                   string station,
                                                                   DateTime udt,
                                                                   string editor,
                                                                   EnumProductMOState CheckReworkState,
                                                                   string moId)
        {
            string strSQL = @"UPDATE ProductMO
                              SET IsRework='Y',
                                  LastTxnId =@LastTxnId,
                                  Udt =@now
                              WHERE ProductID in ({0}) AND MO=@moId AND Status!=@ReworkState
                                 
                              INSERT ProductMOLog(ProductID, MO, [Function], [Action], IsParentMO, 
                                                 Station, PreStatus, Status, IsRework, IsHold, 
                                                 TxnId, Comment, Editor, Cdt)
                              SELECT ProductID, MO, @Function, @Action, IsParentMO, 
                                      @Station, Status,  Status, 'Y', IsHold, 
                                      @LastTxnId,'',@editor , @now
                              FROM  ProductMO
                              WHERE ProductID in ({0}) AND MO=@moId AND Status!=@ReworkState ";

            string inSQLStr = "'" + string.Join("','", productIdList.ToArray()) + "'";
            strSQL = string.Format(strSQL, inSQLStr);

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@Function", 32, function),
                                      SQLHelper.CreateSqlParameter("@Action", 32, action),
                                      SQLHelper.CreateSqlParameter("@LastTxnId", 32, txnId),
                                      SQLHelper.CreateSqlParameter("@Station", 32, station),
                                      SQLHelper.CreateSqlParameter("@editor", 32, editor),
                                      SQLHelper.CreateSqlParameter("@now", udt),
                                      SQLHelper.CreateSqlParameter("@moId", 32, moId),
                                      SQLHelper.CreateSqlParameter("@ReworkState", 32, CheckReworkState.ToString().Trim()));
        }

        public static int CaculateConfirmChangeMO(string moId,
                                           string txnId,
                                           string isParentMO,
                                           out List<string> productIdList,
                                           out string errorDescr)
        {
            productIdList = new List<string>();
            errorDescr = "";
            SqlParameter retPara = SQLHelper.CreateSqlParameter("@Ret", 0, ParameterDirection.ReturnValue);
            SqlParameter errorPara = SQLHelper.CreateSqlParameter("@ReturnMessage", 255, "", ParameterDirection.Output);

            //return >=0:success
            //          < 0:Fail  need output ErrorDescr 
            // if success  case: 
            //     1. insert ConfirmMO and ConfirmMaterial table and 
            //     2.  return @Ret>=0 
            //     3. return ProductID datatable
            //if Fail case:
            //     1.  return @Ret<0
            //      2. set @ErrorDescr = error message

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.StoredProcedure,
                                                     "SAP_MOConfirmChange",
                                                     SQLHelper.CreateSqlParameter("@MoNumber", 20, moId),
                                                     SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                                     SQLHelper.CreateSqlParameter("@IsParentMO", 1, isParentMO),
                                                     errorPara,
                                                     retPara);
            int retValue = (int)retPara.Value;

            errorDescr = ((string)(errorPara.Value)).Trim();

            if (retValue >= 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    productIdList.Add(dr[0].ToString().Trim());
                }
            }

            return retValue;
        }

        public static DataTable GetConfirmReworkMO(string moId, string txnId)
        {
            string strSQL = @"SELECT TOP 1 ID, MO, TxnId, Model,Plant,ProductVer,ConfirmDate, Cdt
                              FROM ConfirmReworkMO 
                              WHERE MO =@MO AND TxnId=@TxnId
                              ORDER BY Cdt DESC";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@MO", 20, moId),
                                                     SQLHelper.CreateSqlParameter("@TxnId", 32, txnId));
            return dt;
        }

        public static DataTable GetConfirmReworkMaterial(int ConfirmReworkMOId)
        {

            string strSQL = @"SELECT ID, ConfirmReworkMOId, PartNo, ABS(WithdrawQty) as WithdrawQty, Unit, Mvt,Cdt
                              FROM ConfirmReworkMaterial
                              WHERE  ConfirmReworkMOId= @ConfirmReworkMOId";                              

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@ConfirmReworkMOId", ConfirmReworkMOId));
            return dt;
        }

        public static string UpdateStatus(MoConfirmChange CFResult)
        {
            string Message = "";
            string TxnId = CFResult.SerialNumber;
            string MoNumber = CFResult.MoNumber;
            string Result = CFResult.Result;
            SqlParameter outResult = SQLHelper.CreateSqlParameter("@ReturnMessage", 255, "", ParameterDirection.Output);
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                    System.Data.CommandType.StoredProcedure,
                                    "SAP_MOConfirmResult",
                                    SQLHelper.CreateSqlParameter("@Function", 32, "MOConfirmChange"),
                                    SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId.Trim()),
                                    SQLHelper.CreateSqlParameter("@MoNumber", 20, MoNumber.Trim()),
                                    SQLHelper.CreateSqlParameter("@Result", 20, Result.Trim()),
                                    outResult);

            Message = outResult.Value.ToString();
            return Message;

        }

        public static DataTable GetMOStatus(string moId)
        {

            string strSQL = @"select a.MO, a.Plant, a.CreateDate, a.StartDate, a.Qty,
                                                       a.SAPStatus, a.Print_Qty, a.Transfer_Qty, b.[Status],
                                                       b.HoldCode, b.IsHold     
                                                from MO a, MOStatus b 
                                                where a.MO = b.MO and 
                                                          a.MO=@MO";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@MO", 32, moId));
            return dt;
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

        public static void InsertTxnDataLog(EnumMsgCategory Category,
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
    }
}