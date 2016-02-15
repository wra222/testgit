using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_COAStockQty
    {
        DataTable GetQueryResult(string Connection, DateTime EndDate);
        DataTable GetQueryResultExt(string Connection, string status,string line,bool isSummary);

    }
}
