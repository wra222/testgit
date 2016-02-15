using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.NotifyECOModel
{
    public class SQL
    {
        public static List<SAPMAWBDef> GetSAPMawbDefByPlantCode(string connectionDB, int dbIndex, string dataType, string PlantCode)
        {
            List<SAPMAWBDef> retList = new List<SAPMAWBDef>();

            string strSQL = @"select ID, PlantCode, ConnectionStr, DBName,  MsgType, Descr, Cdt, Udt, VolumnUnit 
                                            from SAPWeightDef
                                            where MsgType=@MsgType and PlantCode=@PlantCode
                                            order by ID";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@MsgType", 16, dataType),
                                                     SQLHelper.CreateSqlParameter("@PlantCode", 8, PlantCode));
            foreach (DataRow dr in dt.Rows)
            {
                SAPMAWBDef item = new SAPMAWBDef();
                item.ID = (int)dr["ID"];
                item.PlantCode = dr["PlantCode"].ToString().Trim();
                item.ConnectionStr = dr["ConnectionStr"].ToString().Trim();
                item.DBName = dr["DBName"].ToString().Trim();
                item.MsgType = dr["MsgType"].ToString().Trim();
                item.Descr = dr["Descr"].ToString().Trim();
                item.Cdt = (DateTime)dr["Cdt"];
                item.Udt = (DateTime)dr["Udt"];
                item.VolumnUnit = dr["VolumnUnit"].ToString().Trim();

                retList.Add(item);
            }

            return retList;
        }

        public static string CheckECOModelExists(string connectionStr, string DBName, NotifyECOModel result, string PlantCode)
        {
            string ECRNo = (result.ECRNo == null ? "" : result.ECRNo);
            string Model = (result.Model == null ? "" : result.Model);

            string connectStr = string.Format(connectionStr, DBName);

            string strSQL = @"if exists(select top 1 ECRNo from ECOModel where ECRNo=@ECRNo and Model=@Model and Plant=@Plant)
                              begin
                                  set @ret = 'Y'
                              end
                              else
                              begin
                                  select @ret= 'N'
                              end";
            SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 8, "", ParameterDirection.InputOutput);

            SQLHelper.ExecuteDataFill(connectStr,
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@ECRNo", 20, ECRNo),
                                                SQLHelper.CreateSqlParameter("@Model", 20, Model),
                                                SQLHelper.CreateSqlParameter("@Plant", 20, PlantCode),
                                                ParaRet);

            return ParaRet.Value.ToString().Trim();
        }

        public static string CheckECOModelInWIP(string connectionStr, string DBName, NotifyECOModel result)
        {
            string Model = (result.Model == null ? "" : result.Model);

            string connectStr = string.Format(connectionStr, DBName);

//            string strSQL = @"if exists(select top 1 a.ProductID
//                                        from Product a
//                                        inner join ProductStatus b on a.ProductID = b.ProductID and 
//                                                                      b.Station not in (select Value from ConstValueType where Type='NoneWIPStationForECOModel')
//                                        where a.Model = @Model)
//                              begin
//                                    if not exists (select * from ConstValueType where Type='TravelCardHoldModel' and Value like '%'+@Model+'%')
//                                    begin
//                                            insert into ConstValueType(Type, Value, Description, Editor, Cdt, Udt)
//                                            values ('TravelCardHoldModel', '^'+@Model+'$', 'ECOModel', 'SAP', @Now, @Now)
//
//                                            set @ret = 'Hold'
//                                    end
//                                    else
//                                    begin
//                                        set @ret = 'IsHold'
//                                    end
//                              end
//                              else
//                              begin
//                                    set @ret = 'NoneWIP'
//                              end";

            string strSQL = @"if not exists (select * from ConstValueType where Type='TravelCardHoldModel' and Value like '%'+@Model+'%')
                              begin
                                    insert into ConstValueType(Type, Value, Description, Editor, Cdt, Udt)
                                    values ('TravelCardHoldModel', '^'+@Model+'$', 'ECOModel', 'SAP', @Now, @Now)

                                    set @ret = 'Hold'
                              end
                              else
                                   begin
                                   set @ret = 'IsHold'
                              end";

            SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 8, "", ParameterDirection.InputOutput);

            SQLHelper.ExecuteDataFill(connectStr,
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@Model", 20, Model),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now),
                                                ParaRet);

            return ParaRet.Value.ToString().Trim();
        }

        public static void InsertECOModel(string connectionStr, string DBName, NotifyECOModel result, string PlantCode, string ECOStatus)
        {
            string ECRNo = (result.ECRNo == null ? "" : result.ECRNo);
            string ECONo = (result.ECONo == null ? "" : result.ECONo);
            string Model = (result.Model == null ? "" : result.Model);
            string ValidateFromDate = (result.ValidateFromDate == null ? "" : result.ValidateFromDate);

            string connectStr = string.Format(connectionStr, DBName);

            string strSQL = @"if not exists(select top 1 ECRNo from ECOModel where ECRNo=@ECRNo and Model=@Model and Plant=@Plant)
                              begin
                                    insert into ECOModel(Plant, ECRNo, ECONo, Model, ValidateFromDate, PreStatus, Status, Editor, Cdt, Udt)
                                                 values (@Plant, @ECRNo, @ECONo, @Model, @ValidateFromDate,'', @Status, 'SAP', @Now, @Now)  
                              end";

            SQLHelper.ExecuteDataFill(connectStr,
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@ECRNo", 32, ECRNo),
                                                SQLHelper.CreateSqlParameter("@ECONo", 32, ECONo),
                                                SQLHelper.CreateSqlParameter("@Model", 32, Model),
                                                SQLHelper.CreateSqlParameter("@Plant", 20, PlantCode),
                                                SQLHelper.CreateSqlParameter("@ValidateFromDate", 32, ValidateFromDate),
                                                SQLHelper.CreateSqlParameter("@Status", 20, ECOStatus),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
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
            string strSQL = @"if not Exists (select top 1* from TxnDataLog where [Action]=@Action and [KeyValue1]=@KeyValue1 and [KeyValue2]=@KeyValue2 and [TxnId]=@TxnId and [Category]=@Category)
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
    public class SAPMAWBDef
    {
        public int ID;
        public string PlantCode;
        public string ConnectionStr;
        public string DBName;
        public string MsgType;
        public string Descr;
        public DateTime Cdt;
        public DateTime Udt;
        public string VolumnUnit;
    }
}