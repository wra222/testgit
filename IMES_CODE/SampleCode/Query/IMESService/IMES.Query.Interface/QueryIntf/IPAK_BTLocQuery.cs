using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_BTLocQuery       
    {
        DataTable GetQueryResult(string Connection, string line,string input,string inputType);
        DataTable GetDetailResult(string Connection, string LocID);
    }
 
}
