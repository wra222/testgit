using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_UnShipSnList
    {
        DataTable[] GetSnList(string Connection, string ShipDate, string Model);
        DataTable GetDetail(string Connection, DateTime ShipDate, string Model);

    }
}
