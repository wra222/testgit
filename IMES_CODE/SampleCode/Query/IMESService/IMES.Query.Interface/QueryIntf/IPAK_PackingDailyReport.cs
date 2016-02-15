using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_PackingDailyReport       
    {
        DataTable GetQueryResult(string Connection, DateTime inputDate,string PAKStation,string FAStation );
        DataTable GetQueryResult(string Connection, DateTime inputDate);

    }
    
}
