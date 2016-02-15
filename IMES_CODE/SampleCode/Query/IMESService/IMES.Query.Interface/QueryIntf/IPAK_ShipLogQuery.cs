using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_ShipLogQuery
    {
        DataTable GetQueryResult(string Connection,string model,DateTime fromDate,DateTime toDate,string prdType);
    }
    
}
