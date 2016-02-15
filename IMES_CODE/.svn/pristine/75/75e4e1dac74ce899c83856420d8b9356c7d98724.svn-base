using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArchiveInterface;
using System.Data;
using ArchiveDB;
using System.Data.SqlClient;
namespace DBSetting
{
    public class HPEDI : IArchiveInterface
    {
        private const int _minDay = 30;
        public string CheckDaySetting(SqlConnection con)
        {
            string errMsg = "";
            return errMsg;
        }
        public int GetDayDiffFromMinShipDay(SqlConnection con)
        {
            string cmd = @"  SELECT DATEDIFF(DAY,MIN(ACTUAL_SHIPDATE),GETDATE())
                                            From [PAK.PAKComn] ";
            int i = int.Parse(SQLTool.ExecuteScalar(con, cmd).ToString());
            return i;

        
        }
        public string GetDeleteSql(string tableName, string deleteCount, string selepTime)
        {
            string sql = "";
            switch (tableName)
            {
                case  "[DN_PrintList]":
                    sql = @"DELETE   [DN_PrintList] WHERE DN in    
                                 ( select PK_ID from ArchiveIDList where Item='DN')    ";
                    break;
                case "[PAKODMSESSION]":
                    sql = @" DELETE from   PAKODMSESSION 
                                      Where  SERIAL_NUM in 
                                        ( 
                                            select SERIAL_NUM from [PAK_PackkingData] WITH(NOLOCK)      
                                             where InternalID in    
                                          (select PK_ID from ArchiveIDList where Item='DN' )
                                        )   ";
                    break;
               
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                sql = sql.Replace("DELETE", " DELETE  Top({0}) ");
                sql = "While 1=1 Begin  WAITFOR DELAY '{1}'  " + sql + "     If @@rowcount <{0}  break END ";
                sql = string.Format(sql, deleteCount, selepTime);
            }
            return sql;
        }


    }
}

