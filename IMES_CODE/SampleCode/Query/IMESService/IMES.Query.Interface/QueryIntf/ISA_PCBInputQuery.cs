using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_PCBInputQuery
    {

        DataTable GetModel(string DBConnection, IList<String> Family);
        DataTable GetStation(string DBConnection, IList<String> Family);
        DataTable GetPCBInputQuery(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> PdLine, string Family, IList<string> Model, string InputStation, string StationList);
        DataTable GetSelectDetail(string Connection, DateTime FromDate, DateTime ToDate, string Family, string Model, string Line, string Station, string InputStation);
        

    }    
}
