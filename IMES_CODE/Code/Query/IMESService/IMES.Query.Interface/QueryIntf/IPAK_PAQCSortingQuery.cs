using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_PAQCSortingQuery
    {
        DataTable GetPAQCSortingQueryResult(string Connection,String CUSTSN);
    }
}
