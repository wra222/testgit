using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_Warehouse1
    {
        DataTable GetWarehouseDashBoardData(string DBConnection,DateTime ShipDate);
        DataTable GetWarehouseDashBoardData2(string DBConnection,DateTime Shipdate);
        //DataTable GetWarehouseDashBoardData_Detail(string DBConnection, string ShipDate, string MAWB, string Status);
        DataTable GetWarehouseDashBoardData_Detail(string DBConnection, string MAWB, string Status, string ShipDate1);
    }
}
