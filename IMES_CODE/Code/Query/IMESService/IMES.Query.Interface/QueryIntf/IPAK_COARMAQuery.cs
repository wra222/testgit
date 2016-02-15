using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_COARMAQuery
    {
        DataTable GetCOARMAQueryResult(string Connection, String RMA, String COASN, Boolean NumType);
    }
}
