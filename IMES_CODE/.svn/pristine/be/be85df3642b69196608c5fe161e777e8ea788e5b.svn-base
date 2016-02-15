using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_PoWIPTracking
    {
        DataTable GetQueryResultBySP(string Connection, string ShipDate, string FAStation, string PAKStation, string Model,
                                                        string Line, string IsShiftLine, string grpType,
                                                        string DBName, out int[] CountDNQty);
        DataTable GetQueryResult(string Connection, DateTime ShipDate, string PoNo, string Model, string Process);
    //    int[] GetDNShipQty(string Connection, DateTime ShipDate, string PAKStation);

        DataSet GetDNShipQty(string Connection, DateTime ShipDate, string PoNo, string Model, string Process);
        DataTable GetSelectDetail2(string Connection, string ShipDate, string Model, string Line, string Station, string DN);

        DataTable GetSelectDetail(string Connection, DateTime ShipDate, string PoNo, string Model, string Process,string Station,string Line);
        DataTable GetQueryResult2(string Connection, string ShipDate, string Model, string Line, string StationList, string GroupType, string Family, string DnDate);
    }
}
