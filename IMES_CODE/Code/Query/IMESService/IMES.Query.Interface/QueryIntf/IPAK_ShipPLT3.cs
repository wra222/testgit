using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_ShipPLT3
    {
        DataTable GetShipPLT3Report(string Connection, DateTime inputDate);
    }
}
