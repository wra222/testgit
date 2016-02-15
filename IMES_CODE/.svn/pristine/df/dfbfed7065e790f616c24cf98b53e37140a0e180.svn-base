using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_SAMBRepair
    {        
        DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime);
        DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime, string Family, IList<string> Model, string InputStation);
        DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime, string Family, IList<string> Model, IList<string> Station);
    }    
}
