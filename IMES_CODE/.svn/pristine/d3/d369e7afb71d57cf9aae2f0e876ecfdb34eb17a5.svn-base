using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Query.Interface.QueryIntf;
using System.Data.SqlClient;
using System.Data;
using log4net;
using IMES.Query.DB;
using IMES.Infrastructure;
using System.Reflection;
namespace IMES.Query.Implementation
{
    public class FA_TouchGlassTimeQuery : MarshalByRefObject, IFA_TouchGlassTimeQuery
    {
        public void InsertShipNo(string DBConntion, DataTable dt)
        {
            int i = SQLHelper.ExecuteNonQuery(DBConntion, CommandType.Text, "Truncate Table  ShipNo  ");
            SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(DBConntion, SqlBulkCopyOptions.UseInternalTransaction);
            sqlbulkcopy.DestinationTableName = "ShipNo";//数据库中的表名  
            sqlbulkcopy.WriteToServer(dt);
        }
        public DataTable GetCTMessage(string DBConntion, DateTime from, DateTime to, string Tp)
        {
            DataTable Result = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("exec TouchGlassTime @StartTime,@EndTime ,'" + Tp + "'");
            Result = SQLHelper.ExecuteDataFill(DBConntion, System.Data.CommandType.Text,
                                                 sb.ToString(), new SqlParameter("@StartTime", from), new SqlParameter("@EndTime", to));
            return Result;
        }
        public DataTable GetCTNo(string DBConntion, string CT)
        {
            DataTable Result = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select MaterialCT,Udt from Material(nolock) where MaterialCT= @CT ");
            Result = SQLHelper.ExecuteDataFill(DBConntion, System.Data.CommandType.Text,
                                                 sb.ToString(), SQLHelper.CreateSqlParameter("@CT", 32, CT));
            return Result;
        }
    }
}
