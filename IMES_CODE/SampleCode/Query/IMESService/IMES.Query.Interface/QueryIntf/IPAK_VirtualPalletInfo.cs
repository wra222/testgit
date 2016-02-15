using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_VirtualPalletInfo       
    {
        DataTable GetQueryResult(string Connection, DateTime fromDate, DateTime toDate);
    }
    
}
