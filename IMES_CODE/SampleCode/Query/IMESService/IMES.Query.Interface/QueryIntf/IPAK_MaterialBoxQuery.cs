using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_MaterialBoxQuery
    {
        DataTable GetQueryResult(string Connection, DateTime StartDate, DateTime EndDate);
    }
}