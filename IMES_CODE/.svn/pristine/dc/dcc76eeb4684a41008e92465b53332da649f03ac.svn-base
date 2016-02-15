using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using IMES.Query.DB;
using System.Data;
using System.Data.SqlClient;
using IMES.WS.Common;

namespace IMES.WS.MoQuery
{
    public class SQL
    {      

      
        public static MoQueryResponse GetMoInfo(string monumber)
        {
            string strSQL = "select [Status],IsHold from MOStatus where MO=@MO";
            DataTable dt=     SQLHelper.ExecuteDataFill(SQLHelper.ConnectionString_CFG(),
                                                           System.Data.CommandType.Text,
                                                            strSQL, SQLHelper.CreateSqlParameter("@MO", 20, monumber.Trim()));
            MoQueryResponse moresponse = new MoQueryResponse();
            if (dt.Rows.Count == 0)
            {
                moresponse.HoldResult="";
                moresponse.Result = "NotFound";
                moresponse.MoNumber = monumber;

            }
            else
            {
                moresponse.HoldResult =dt.Rows[0]["IsHold"].ToString().Trim();
                moresponse.Result = dt.Rows[0]["Status"].ToString().Trim();
                moresponse.MoNumber = monumber;
            }
            return moresponse;
        }

      
            
        
    }
}