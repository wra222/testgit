using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace IMES.WS.MOConfirmChange
{
    public class SQL
    {      
        public static string UpdateStatus(MoConfirmChangeResult CFCResult)
        {
            string Message = "";
            string TxnId = CFCResult.SerialNumber;
            string MoNumber = CFCResult.MoNumber;
            string Result = CFCResult.Result;
            SqlParameter outResult = SQLHelper.CreateSqlParameter("@ReturnMessage", 255, "", ParameterDirection.Output);
            SQLHelper.ExecuteNonQuery(SQLHelper.ConnectionString_CFG(),
                                    System.Data.CommandType.StoredProcedure,
                                    "SAP_MOConfirmResult",
                                    SQLHelper.CreateSqlParameter("@Function", 32, "MOConfirmChangeResult"),
                                    SQLHelper.CreateSqlParameter("@TxnId", 32, TxnId.Trim()),
                                    SQLHelper.CreateSqlParameter("@MoNumber", 20, MoNumber.Trim()),
                                    SQLHelper.CreateSqlParameter("@Result", 20, Result.Trim()),
                                    outResult);

            Message = outResult.Value.ToString();
            return Message;
        }
    }
}
