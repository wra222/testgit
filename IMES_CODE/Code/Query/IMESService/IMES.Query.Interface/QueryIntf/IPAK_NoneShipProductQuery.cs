using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_NoneShipProductQuery
    {
        DataTable GetQueryResult(string Connection, string model, string line, string station);
       
        DataTable GetPdLine(string Connection);

        DataTable GetStation(string Connection);
    }
}
