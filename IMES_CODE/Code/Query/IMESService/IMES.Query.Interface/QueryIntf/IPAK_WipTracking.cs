using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_WipTracking
    {
        
        DataTable WipTrackingByDN_PAK(string Connection, string ShipDate, string FAStation, string PAKStation, string Model,
                                                                string Line, string IsShiftLine, string grpType, string DBName, out int[] CountDNQty);
        DataTable GetDetailForWipTracking(string Connection, string Model, string Line, string Station, string DN);
        int[] GetDNShipQty_Docking(string Connection, DateTime ShipDate, string Model);
        int[] GetDNShipQty(string Connection, DateTime ShipDate, string Model, string PrdType);
        int[] GetDNShipQty_TEST(string Connection, DateTime ShipDate, string Model, string PrdType);
        int[] GetDNShipQty_TEST(string Connection, string ShipDate, string Model, string PrdType);
        
        DataTable WipTrackingByDN_PAK_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type,string PrdType,string DBName,out int[] CountDNQty);
        DataTable WipTrackingByDN_PAK_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string PrdType, string DBName);
            
        DataTable WipTrackingByDN_FA_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string DBName, out int[] CountDNQty);
        DataTable WipTrackingByDN_FA_Ex(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string DBName);
        DataTable WipTrackingByDN_GetNullTable(string Connection, string ShipDate, string Model, string Line, string Process, string Type, string DBName);

    }
    
}
