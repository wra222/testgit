using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_PakBufferQuery       
    {
        DataTable GetQueryResult(string Connection, string DN, string type,bool isUnWeight,DateTime fromDate,DateTime toDate);
    }
    
}
