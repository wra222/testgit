using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_COAStatusReport
    {
        DataTable GetQueryResultByCOASN(string Connection, string StartSN, string EndSN);

        DataTable GetQueryResultByCOADate(string Connection, DateTime FromDate, DateTime ToDate);

        DataTable GetQueryResultDetailByCOADate(string Connection, DateTime FromDate, DateTime ToDate,
                string IECPN, string Line, string Status);

        DataTable GetQueryResultByCOAStatus(string Connection, string Status);

        DataTable GetQueryByCOAAllStatus(string Connection);

        DataTable GetQueryByCOARemind(string Connection,string Type);

        DataTable GetQueryResultByCOADailyCheck(string Connection, string Type, DateTime FromDate, DateTime ToDate);
        
    }
}
