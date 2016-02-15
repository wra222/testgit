using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_TestDefect
    {        
        DataTable GetDefectInfo(string DBConnection, DateTime StartTime, DateTime EndTime,
                                IList<string> Family, IList<string> PdLine, IList<String> Model, IList<string> Station);
        DataTable GetTestStation(string DBConnection, string Family);
        DataTable GetModel(string DBConnection, string Family);
    }    
}
