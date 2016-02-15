using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ArchiveInterface
{
    public interface  IArchiveInterface
    {
        string GetDeleteSql(string tableName, string deleteCount,string selepTime);
        string CheckDaySetting(SqlConnection con);
        int GetDayDiffFromMinShipDay(SqlConnection con);
    }
}
