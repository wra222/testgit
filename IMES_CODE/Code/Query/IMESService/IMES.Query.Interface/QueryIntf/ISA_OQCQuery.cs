using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_OQCQuery
    {
        DataTable GetPCBOQCQuery(string DBConnection, DateTime StartTime, DateTime EndTime,  string Status, IList<string> Cause,string Process);

    }
}
