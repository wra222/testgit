using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_BsamLocationQuery
    {
        DataTable GetResultForSummary(string Connection);
        DataTable GetResultForDetail(string Connection);
    }
}
