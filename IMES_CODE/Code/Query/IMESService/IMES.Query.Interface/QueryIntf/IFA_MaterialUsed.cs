using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_MaterialUsed
    {
        DataTable GetQueryResult(string Connection, string qType, string fromDate, string toDate);

    }
}
