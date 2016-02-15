using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_PCBTestReport
    {
        DataTable GetPCBTestReportResult(string Connection, DateTime FromDate, DateTime ToDate, IList<string> lstFamily, IList<string> lstTestItem, IList<string> lstFixtureID);
        DataTable GetRepairDetailResult(string Connection, DateTime FromDate, DateTime ToDate, string PCBNo);
    }
}
