using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_Query87Product
    {
        DataTable GetQueryResult(string DBConnection, string PalletType, string Line, DateTime ShipDate, string ProductType);
    }
}
