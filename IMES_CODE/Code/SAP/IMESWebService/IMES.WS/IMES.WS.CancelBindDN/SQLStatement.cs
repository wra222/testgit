using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.CancelBindDN
{
    public class SQL
    {
        public static void InsertCancelBindDNLog(string connectionDB, int dbIndex, 
                                                 string SerialNumber, string Plant, string DN, string Remark1, string Remark2,
                                                 EnumMsgCategory Category, EnumMsgState State, string ErrorDescr, string Editor)
        {
            Remark1 = (Remark1 == null ? "" : Remark1);
            Remark2 = (Remark2 == null ? "" : Remark2);

            string strSQL = @"insert into CancelBindDNLog(Action, SerialNumber, Plant, DN, Remark1, Remark2, State, ErrorDescr, Editor, Cdt)
                              values (@Category, @SerialNumber, @Plant, @DN, @Remark1, @Remark2, @State, @ErrorDescr, @Editor, @Now) ";


            //SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_HISTORY(1),
            SQLHelper.ExecuteNonQuery(SQLHelper.GetDBConnectionString(connectionDB, dbIndex),
                                                System.Data.CommandType.Text,
                                                strSQL,
                                                SQLHelper.CreateSqlParameter("@Category", 10, Category.ToString().Trim()),
                                                SQLHelper.CreateSqlParameter("@SerialNumber", 25, SerialNumber),
                                                SQLHelper.CreateSqlParameter("@Plant", 4, Plant),
                                                SQLHelper.CreateSqlParameter("@DN", 10, DN),
                                                SQLHelper.CreateSqlParameter("@Remark1", 25, Remark1),
                                                SQLHelper.CreateSqlParameter("@Remark2", 25, Remark2),
                                                SQLHelper.CreateSqlParameter("@State", 16, State.ToString().Trim()),
                                                SQLHelper.CreateSqlParameter("@ErrorDescr", 255, ErrorDescr),
                                                SQLHelper.CreateSqlParameter("@Editor", 20, Editor),
                                                SQLHelper.CreateSqlParameter("@Now", DateTime.Now));

        }


        public static string GetCancelDNState(string connectionDB, int dbIndex, string SerialNumber)
        {

            string strSQL = @"if not exists(select SerialNumber from CancelBindDNLog where SerialNumber=@SerialNumber and Action='Response')
                              begin
                                  set @ret = ''
                              end
                              else
                              begin
                                  select  @ret= (
                                     select top 1 (case when (State ='Success') then '' else ErrorDescr end) as ret
                                     from CancelBindDNLog where SerialNumber=@SerialNumber and Action='Response'
                                     order by Cdt desc
                                   )
                              end";
           SqlParameter ParaRet = SQLHelper.CreateSqlParameter("@ret", 255, "", ParameterDirection.InputOutput);

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