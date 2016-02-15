using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.Service.Common;
using System.Configuration;

namespace IMES.SAP.Implementation
{
    public class SQL
    {

        public static List<SAPWeightDef> GetSAPWeightDef(enumMsgType msgType)
        {
            List<SAPWeightDef> retList = new List<SAPWeightDef>();

            string dataType = (msgType == enumMsgType.Pallet || msgType == enumMsgType.Standard ? 
                                                                msgType.ToString() : 
                                                                enumMsgType.Delivery.ToString());

            string strSQL = @"select ID, PlantCode, WeightUnit, VolumnUnit, ConnectionStr, 
                                                      DBName,  MsgType, Descr, Cdt, Udt 
                                            from SAPWeightDef
                                            where MsgType=@MsgType  
                                            order by ID";

            DataTable dt = SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                     System.Data.CommandType.Text,
                                                     strSQL,
                                                     SQLHelper.CreateSqlParameter("@MsgType", 16, dataType));
            foreach (DataRow dr in dt.Rows)
            {
                SAPWeightDef item =new SAPWeightDef();
                item.ID = (int)dr["ID"];
                item.PlantCode = dr["PlantCode"].ToString().Trim();
                item.WeightUnit = dr["WeightUnit"].ToString().Trim();
                item.VolumnUnit = dr["VolumnUnit"] == null ? string.Empty : dr["VolumnUnit"].ToString().Trim();
                item.ConnectionStr =  dr["ConnectionStr"].ToString().Trim();
                item.DBName =  dr["DBName"].ToString().Trim();
                item.MsgType  =dr["MsgType"].ToString().Trim();
                item.Descr = dr["Descr"].ToString().Trim();
                item.Cdt = (DateTime)dr["Cdt"];
                 item.Udt = (DateTime)dr["Udt"];

                 retList.Add(item);               
            }

            return retList;
        }

        public static void UpdateSAPWeightStatus(string connectionStr, string DBName, List<string> idList, string state)
        {
            List<SAPDeliveryWeight> retList = new List<SAPDeliveryWeight>();

            string strSQL = @"update FISTOSAP_WEIGHT
                                            set SendStatus =@Status, Udt=getdate()
                                            where ID in ({0})";

            strSQL =string.Format(strSQL, string.Join(",",idList.ToArray()));

            string connectStr = string.Format(connectionStr, DBName);
            SQLHelper.ExecuteNonQuery(connectStr,
                                                              System.Data.CommandType.Text,
                                                              strSQL,
                                                              SQLHelper.CreateSqlParameter("@Status", 16, state));
           

        }

        public static List<SAPDeliveryWeight> GetSAPDeliveryWeight(string connectionStr, string DBName)
        {
            List<SAPDeliveryWeight> retList = new List<SAPDeliveryWeight>();

            string connectStr= string.Format(connectionStr, DBName);

            string strSQL = @"select ID, [DN/Shipment], [Status], KG, Cdt, SendStatus
                                            from dbo.FISTOSAP_WEIGHT
                                            where SendStatus !=@Status or SendStatus is NULL";

//            string strSQL = @"select a.ID, a.[DN/Shipment], a.[Status], 
//                                case when(e.CustomerID = 'ASUS' and c.ShipWay ='T001')
//                                then ROUND(a.KG, 0)
//                                else a.KG
//                                end as KG, a.Cdt, a.SendStatus
//                              from dbo.FISTOSAP_WEIGHT a
//                              left join Delivery b on a.[DN/Shipment] = b.ShipmentNo
//                              left join DeliveryEx c on b.DeliveryNo = c.DeliveryNo
//                              left join Model d on b.Model = d.Model
//                              left join Family e on d.Family = e.Family
//                              where a.SendStatus !=@Status or a.SendStatus is NULL"; 
            

            DataTable dt = SQLHelper.ExecuteDataFill(connectStr,
                                                                             System.Data.CommandType.Text,
                                                                             strSQL,
                                                                             SQLHelper.CreateSqlParameter("@Status", 16, "OK"));
            foreach (DataRow dr in dt.Rows)
            {
                SAPDeliveryWeight item = new SAPDeliveryWeight();
                item.ID = (int)dr["ID"];
                item.Shippment = dr["DN/Shipment"].ToString().Trim();
                item.Type = dr["Status"].ToString().Trim();
                //item.Weight = ((float)dr["KG"]).ToString().Trim();
                item.Weight = dr["KG"].ToString().Trim();
                item.Cdt = (DateTime)dr["Cdt"];
                item.Status = dr["SendStatus"].ToString().Trim();

                retList.Add(item);
            }

            return retList;
        
        }


        public static List<SAPPalletWeight> GetSAPPalletWeight(string connectionStr, string DBName)
        {
            List<SAPPalletWeight> retList = new List<SAPPalletWeight>();

            string connectStr = string.Format(connectionStr, DBName);
            string ReduceDayNum = ConfigurationManager.AppSettings["ReduceDayNum"];

            /*
            string strSQL = @"select a.PalletNo, a.Weight, a.Length ,a.[Width], a.[Height], 
                                                       a.Station, b.AttrValue as SendStatus,
                                                       f.ID, f.Type
                                                from Pallet a
                                                left join PalletAttr b on (a.PalletNo = b.PalletNo and b.AttrName ='SendStatus') 
                                                left join (select distinct d.PalletNo , isnull(e.InfoValue, c.DeliveryNo) as ID , isnull(e.InfoValue,'D') as Type
		                                                          from Delivery c
		                                                          inner join Delivery_Pallet d on (c.DeliveryNo = d.DeliveryNo)      
		                                                          left join DeliveryInfo e on (c.DeliveryNo = e.DeliveryNo and e.InfoType ='Consolidated')) f
		                                                         on a.PalletNo = f.PalletNo
                                                where a.Station in ('99','OT','DT') and
                                                      (b.AttrValue is null   or
                                                       b.AttrValue !='OK')";
            */

//            string strSQL = @"select top 2000 a.PalletNo, a.[Weight], 
//                                     isnull(c.[Len], 0.0) as [Length], isnull(c.Wide, 0.0) as Width,  isnull(c.High, 0.0) as Height, 
//                                     a.Station, b.AttrValue as SendStatus,
//                                     f.ID, f.Type
//                              from Pallet a
//                              left join PalletAttr b on (a.PalletNo = b.PalletNo and b.AttrName ='SendStatus') 
//                              left join PLTStandard c on a.PalletNo = c.PLTNO
//                              outer apply dbo.fn_Get_DN_Shipment(a.PalletNo) f
//                              where a.Station in ('99','OT','DT','F99', 'S99') and
//                                    (b.AttrValue is null  or b.AttrValue !='OK') and a.[Weight] != 0 and 
//                                    f.ID is NOT NULL and 
//                                    f.ShipDate > dateadd(DD, convert(int, @ReduceDayNum) , GETDATE())";

            string strSQL = @"select top 2000 a.PalletNo, 
                                     case when(d4.CustomerID = 'ASUS') 
                                     then a.[Weight_L]
                                     else a.[Weight]
                                     end as [Weight],  
                                     isnull(c.[Len], 0.0) as [Length], isnull(c.Wide, 0.0) as Width,  isnull(c.High, 0.0) as Height, 
                                     a.Station, b.AttrValue as SendStatus,
                                     f.ID, f.Type				
                                from Pallet a
                                inner join Delivery_Pallet d1 on d1.PalletNo = a.PalletNo
                                inner join Delivery d2 on d2.DeliveryNo = d1.DeliveryNo
                                inner join Model d3 on d3.Model = d2.Model
                                inner join Family d4 on d4.Family = d3.Family
                                left join PalletAttr b on (a.PalletNo = b.PalletNo and b.AttrName ='SendStatus') 
                                left join PLTStandard c on a.PalletNo = c.PLTNO
                                outer apply dbo.fn_Get_DN_Shipment(a.PalletNo) f
                                where a.Station in ('99','OT','DT','F99', 'S99') and
                                    (b.AttrValue is null  or b.AttrValue !='OK') and a.[Weight] != 0 and 
                                    f.ID is NOT NULL and 
                                    f.ShipDate > dateadd(DD, convert(int, @ReduceDayNum) , GETDATE())";

            DataTable dt = SQLHelper.ExecuteDataFill(connectStr,
                                                                             System.Data.CommandType.Text,
                                                                             strSQL,
                                                                             SQLHelper.CreateSqlParameter("@Status", 16, "OK"),
                                                                             SQLHelper.CreateSqlParameter("@ReduceDayNum", 5, ReduceDayNum));
            foreach (DataRow dr in dt.Rows)
            {    
                SAPPalletWeight item = new SAPPalletWeight();

                item.ID = dr["ID"].ToString().Split(new char[] {'/'})[0].Trim().Substring(0,10);
                item.Type = dr["Type"].ToString().Trim()=="D"? "D":"S";
                item.PalletNo = dr["PalletNo"].ToString().StartsWith("NA") ? dr["PalletNo"].ToString().Substring(2).Trim() : dr["PalletNo"].ToString().Trim();
                item.Weight = dr["Weight"].ToString().Trim();
                item.Length = dr["Length"] == null || Convert.ToDecimal(dr["Length"]) == 0 ? "" : dr["Length"].ToString().Trim();
                item.Width = dr["Width"] == null || Convert.ToDecimal(dr["Width"]) == 0 ? "" : dr["Width"].ToString().Trim();
                item.Height = dr["Height"] == null || Convert.ToDecimal(dr["Height"]) == 0 ? "" : dr["Height"].ToString().Trim();
                item.Station = dr["Station"].ToString().Trim();
                item.Status = dr["Station"] == null ? "" : dr["Station"].ToString().Trim();
                
                retList.Add(item);
            }

            return retList;

        }

        public static List<SAPMasterWeight> GetSAPMasterWeight(string connectionStr, string DBName)
        {
            List<SAPMasterWeight> retList = new List<SAPMasterWeight>();

            string connectStr = string.Format(connectionStr, DBName);
            string ReduceDayNum = ConfigurationManager.AppSettings["ReduceDayNum"];

            string strSQL = @"select Model, isnull(UnitWeight, 0.00) as GrossWeight, 0.00 as NetWeight, SendStatus
                              from ModelWeight
                              where Remark = 'New' and isnull(SendStatus, '') !=@Status and UnitWeight !=0.00 and
                                    Udt > convert(varchar(10), GETDATE(), 111)";

            DataTable dt = SQLHelper.ExecuteDataFill(connectStr,
                                                                             System.Data.CommandType.Text,
                                                                             strSQL,
                                                                             SQLHelper.CreateSqlParameter("@Status", 16, "OK"));
            foreach (DataRow dr in dt.Rows)
            {
                SAPMasterWeight item = new SAPMasterWeight();

                item.ID = dr["Model"].ToString();
                item.GrossWeight = dr["GrossWeight"].ToString().Trim();
                item.NetWeight = dr["NetWeight"].ToString().Trim();
                item.Status = dr["SendStatus"] == null ? "" : dr["SendStatus"].ToString().Trim();

                retList.Add(item);
            }

            return retList;

        }

        public static void UpdateSAPPalletStatus(string connectionStr, string DBName, 
                                                                           string  palletNo, string Id, 
                                                                           string idType, string state)
        {
            string strSQL = @"IF exists (select PalletNo from Pallet where PalletNo =@PalletNo)
                              Begin 
                                delete from PalletAttr
                                            where  AttrName in ('SendStatus', 'ID','IDType') and
                                                        PalletNo =@PalletNo
                                            insert PalletAttr(AttrName, PalletNo, AttrValue, Editor, Cdt, Udt)
                                            values('SendStatus',@PalletNo,@Status,@Editor,@now, @now)
                                            insert PalletAttr(AttrName, PalletNo, AttrValue, Editor, Cdt, Udt)
                                            values('ID',@PalletNo,@ID,@Editor,@now, @now )
                                            insert PalletAttr(AttrName, PalletNo, AttrValue, Editor, Cdt, Udt)
                                            values('IDType',@PalletNo,@Type,@Editor,@now, @now)
                              End
                              Else
                              Begin
                                delete from PalletAttr
                                            where  AttrName in ('SendStatus', 'ID','IDType') and
                                                        PalletNo ='NA'+@PalletNo
                                            insert PalletAttr(AttrName, PalletNo, AttrValue, Editor, Cdt, Udt)
                                            values('SendStatus','NA'+@PalletNo,@Status,@Editor,@now, @now)
                                            insert PalletAttr(AttrName, PalletNo, AttrValue, Editor, Cdt, Udt)
                                            values('ID','NA'+@PalletNo,@ID,@Editor,@now, @now )
                                            insert PalletAttr(AttrName, PalletNo, AttrValue, Editor, Cdt, Udt)
                                            values('IDType','NA'+@PalletNo,@Type,@Editor,@now, @now)
                              End";

           

            string connectStr = string.Format(connectionStr, DBName);
            SQLHelper.ExecuteNonQuery(connectStr,
                                                              System.Data.CommandType.Text,
                                                              strSQL,
                                                              SQLHelper.CreateSqlParameter("@PalletNo", 32, palletNo),
                                                              SQLHelper.CreateSqlParameter("@Status", 16, state),
                                                              SQLHelper.CreateSqlParameter("@ID", 32, Id),
                                                               SQLHelper.CreateSqlParameter("@Type", 16, idType),
                                                                SQLHelper.CreateSqlParameter("@Editor", 16, "FIS"),
                                                                 SQLHelper.CreateSqlParameter("@now", DateTime.Now));


        }

        //public static void UpdateSAPMasterWeightStatus(string connectionStr, string DBName, List<string> idList, string state)
        public static void UpdateSAPMasterWeightStatus(string connectionStr, string DBName, string Model, string state)
        {
            List<SAPMasterWeight> retList = new List<SAPMasterWeight>();

            string strSQL = @"update ModelWeight
                                            set SendStatus =@Status, Udt=getdate()
                                            where Model =@Model";

            //strSQL = string.Format(strSQL, string.Join(",", idList.ToArray()));

            string connectStr = string.Format(connectionStr, DBName);
            SQLHelper.ExecuteNonQuery(connectStr,
                                                              System.Data.CommandType.Text,
                                                              strSQL,
                                                              SQLHelper.CreateSqlParameter("@Model", 16, Model),
                                                              SQLHelper.CreateSqlParameter("@Status", 16, state));


        }

        public static void InsertSendData(string action,
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

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
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

        public static void InsertSendData(string action,
                                              string key1,
                                              string key2,
                                              string txnId,
                                              string errorCode,
                                              string errorDescr,
                                              string comment,
                                              EnumMsgState state,
                                              DateTime udt)
        {

            string strSQL = @"INSERT INTO SendData(Action, KeyValue1, KeyValue2, TxnId, ErrorCode, 
						                           ErrorDescr, State, ResendCount, Comment, Cdt,Udt)
                                            VALUES(@Action, @KeyValue1, @KeyValue2, @TxnId, @ErrorCode, 
		                                            @ErrorDescr, @State, 0,@comment, @now,@now)";

            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                      System.Data.CommandType.Text,
                                      strSQL,
                                      SQLHelper.CreateSqlParameter("@Action", 32, action),
                                      SQLHelper.CreateSqlParameter("@KeyValue1", 32, key1),
                                      SQLHelper.CreateSqlParameter("@KeyValue2", 32, key2),
                                      SQLHelper.CreateSqlParameter("@TxnId", 32, txnId),
                                      SQLHelper.CreateSqlParameter("@State", 32, state.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@comment", 255, comment.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@now", udt),
                                      SQLHelper.CreateSqlParameter("@ErrorCode", 32, errorCode.ToString().Trim()),
                                      SQLHelper.CreateSqlParameter("@ErrorDescr", 255, errorDescr.ToString().Trim()));                                      
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

        public static void InsertSendData_DB(string connectionDB, int dbIndex,
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

            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
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

        public static void InsertTxnDataLog_DB(string connectionDB, int dbIndex,
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

            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
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

        public static void InsertCancelBindDNLog(string connectionDB, int dbIndex,
                                                 string SerialNumber, string Plant, string DN, string Remark1, string Remark2,
                                                 string Category, string State, string ErrorDescr, string Editor)
        {
            Remark1 = (Remark1 == null ? "" : Remark1);
            Remark2 = (Remark2 == null ? "" : Remark2);

            string strSQL = @"insert into CancelBindDNLog(Action, SerialNumber, Plant, DN, Remark1, Remark2, State, ErrorDescr, Editor, Cdt)
                              values (@Category, @SerialNumber, @Plant, @DN, @Remark1, @Remark2, @State, @ErrorDescr, @Editor, @Now) ";


            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_HISTORY(1),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@Category", 10, Category),
                                                SQLHelper.CreateSqlParameter("@SerialNumber", 25, SerialNumber),
                                                SQLHelper.CreateSqlParameter("@Plant", 4, Plant),
                                                SQLHelper.CreateSqlParameter("@DN", 10, DN),
                                                SQLHelper.CreateSqlParameter("@Remark1", 25, Remark1),
                                                SQLHelper.CreateSqlParameter("@Remark2", 25, Remark2),
                                                SQLHelper.CreateSqlParameter("@State", 16, State),
                                                SQLHelper.CreateSqlParameter("@ErrorDescr", 255, ErrorDescr),
                                                SQLHelper.CreateSqlParameter("@Editor", 20, Editor),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));

        }
        public static void InsertMasterWeightLog(string connectionDB, int dbIndex,
                                                 string SerialNumber, string Plant, string Model,
                                                 string GrossWeight, string NetWeight, string Unit, string Remark1,
                                                 string Category, string State, string ErrorDescr, string Editor)
        {
            Remark1 = (Remark1 == null ? "" : Remark1);

            string strSQL = @"insert into MasterWeightLog(Action, SerialNumber, Plant, Model, GrossWeight, NetWeight, Unit, Remark1, 
                                                          State, ErrorDescr, Editor, Cdt)
                              values (@Category, @SerialNumber, @Plant, @Model, @GrossWeight, @NetWeight, @Unit, @Remark1, 
                                      @State, @ErrorDescr, @Editor, @Now) ";

            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@Category", 10, Category),
                                                SQLHelper.CreateSqlParameter("@SerialNumber", 35, SerialNumber),
                                                SQLHelper.CreateSqlParameter("@Plant", 4, Plant),
                                                SQLHelper.CreateSqlParameter("@Model", 18, Model),
                                                SQLHelper.CreateSqlParameter("@GrossWeight", 16, GrossWeight),
                                                SQLHelper.CreateSqlParameter("@NetWeight", 16, NetWeight),
                                                SQLHelper.CreateSqlParameter("@Unit", 2, Unit),
                                                SQLHelper.CreateSqlParameter("@Remark1", 25, Remark1),
                                                SQLHelper.CreateSqlParameter("@State", 16, State),
                                                SQLHelper.CreateSqlParameter("@ErrorDescr", 255, ErrorDescr),
                                                SQLHelper.CreateSqlParameter("@Editor", 20, Editor),
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

    public class SAPPalletWeight
    {
        public string ID;
        public string Type;
        public string PalletNo;
        public string Weight;
        public string Length;
        public string Width;
        public string Height;
        public string Station;
        public string Status;

    }

    public class SAPDeliveryWeight
    {
        public int ID;
        public string Shippment;
        public string Type;
        public string Weight;
        public string Status;
        public DateTime Cdt;        
    }

    public class SAPMasterWeight
    {
        public string ID;
        public string GrossWeight;
        public string NetWeight;
        public string Status;

    }
}