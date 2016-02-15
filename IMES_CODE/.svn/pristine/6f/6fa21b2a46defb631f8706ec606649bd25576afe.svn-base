using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.PGIStatus
{
    public class SQL
    {

        public static void UpdatePGIDeliveryStatus(string connectionDB, int dbIndex, PGIStatus result, string PGIDnStatus)
        {
            string ID = result.ID+"%";

            string strSQL = @"update Delivery
                                set Status=@PGIDnStatus, Editor='SAPPGI', Udt=@Now
                                where DeliveryNo like @ID and Status in ('88', '98', '87')";


            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_HISTORY(1),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@ID", 16, ID),
                                                SQLHelper.CreateSqlParameter("@PGIDnStatus", 6, PGIDnStatus),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));

        }

        public static void UpdatePGIDeliveryStatusIncludeShipDate(string connectionDB, int dbIndex, PGIStatus result, string PGIDnStatus)
        {
            string ID = result.ID+"%";
            string PGI_Date = result.PGI_Date;

            string strSQL = @"update Delivery
                                set ShipDate = Convert(date, @PGI_Date), 
                                               Status= case when (Status in ('88', '98', '87')) 
                                                       then @PGIDnStatus
                                                       else Status
                                                       end, 
                                               Editor='SAPPGI', Udt=@Now
                                where DeliveryNo like @ID";


            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_HISTORY(1),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@ID", 16, ID),
                                                SQLHelper.CreateSqlParameter("@PGI_Date", 32, PGI_Date),
                                                SQLHelper.CreateSqlParameter("@PGIDnStatus", 6, PGIDnStatus),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));
        }
        
        public static void UpdatePGIStatus(string connectionDB, int dbIndex, PGIStatus result)
        {
            string SerialNumber = result.SerialNumber;
            string Plant = result.Plant;
            string EventType = result.EventType;
            string ID = result.ID;
            string DNType = result.Type;
            string PGI_Date = result.PGI_Date;

            string strSQL = @"if not exists(select * from PGIStatus where DN_Shipment=@ID and Plant=@Plant)
                                begin
                                    insert into PGIStatus(Plant, DN_Shipment, [Type], [Status], PGI_Date, Cdt, Udt)
                                    values (@Plant, @ID, @Type, @Status, @PGI_Date, @Now, @Now)  
                                end
                                else
                                begin
                                    update PGIStatus
                                    set [Type]=@Type, [Status]=@Status, PGI_Date=@PGI_Date, Udt=@Now
                                    where DN_Shipment=@ID and Plant=@Plant
                                end";


            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_HISTORY(1),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@Plant", 4, Plant),
                                                SQLHelper.CreateSqlParameter("@ID", 10, ID),
                                                SQLHelper.CreateSqlParameter("@Type", 1, DNType),
                                                SQLHelper.CreateSqlParameter("@Status", 6, EventType),
                                                SQLHelper.CreateSqlParameter("@PGI_Date", 25, PGI_Date),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));

        }

        public static void InsertPGILog(string connectionDB, int dbIndex, PGIStatus result, string State, string ErrorDescr)
        {
            string SerialNumber = result.SerialNumber;
            string Plant = result.Plant;
            string EventType = result.EventType;
            string ID = result.ID;
            string DNType = result.Type;
            string PGI_Date = result.PGI_Date;
            string Remark1 = (result.Remark1 == null ? "" : result.Remark1);

            string strSQL = @"insert into PGILog(SerialNumber, Plant, DN_Shipment, [Type], [Status], PGI_Date, Remark1, State, ErrorDescr, Cdt)
                              values (@SerialNumber, @Plant, @ID, @Type, @Status, @PGI_Date, @Remark1, @State, @ErrorDescr, @Now) ";


            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_HISTORY(1),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@SerialNumber", 25, SerialNumber),
                                                SQLHelper.CreateSqlParameter("@Plant", 4, Plant),
                                                SQLHelper.CreateSqlParameter("@ID", 10, ID),
                                                SQLHelper.CreateSqlParameter("@Type", 1, DNType),
                                                SQLHelper.CreateSqlParameter("@Status", 6, EventType),
                                                SQLHelper.CreateSqlParameter("@PGI_Date", 19, PGI_Date),
                                                SQLHelper.CreateSqlParameter("@Remark1", 25, Remark1),
                                                SQLHelper.CreateSqlParameter("@State", 16, State),
                                                SQLHelper.CreateSqlParameter("@ErrorDescr", 255, ErrorDescr),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));

        }           
        

        public static string GetPGIState(string connectionDB, int dbIndex, string SerialNumber)
        {

            string strSQL = @"if not exists(select SerialNumber from PGILog where SerialNumber=@SerialNumber)
                              begin
                                  set @ret = 'N'
                              end
                              else
                              begin
                                  select  @ret= (
                                     select top 1 (case when (State ='Success') then 'S' else 'F' end) as ret
                                     from PGILog where SerialNumber=@SerialNumber
                                     order by Cdt desc
                                   )
                              end";
           SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 8, "", ParameterDirection.InputOutput);

           //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_HISTORY(1),
           SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@SerialNumber", 25, SerialNumber),
                                                ParaRet);

            return ParaRet.Value.ToString().Trim(); 
        }  
        

    }
}