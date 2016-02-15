using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_PCBStationQuery
    {

        DataTable GetModel(string DBConnection, IList<String> Family);
        DataTable GetStation(string DBConnection, IList<String> Family);
        DataTable GetPCBStationQuery(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> PdLine, IList<string> Family, IList<string> Model, IList<string> Station);
        DataTable GetSelectDetail(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> PdLine, string Family, IList<string> Model, IList<string> Station);
    }    
}
