using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_SNByFamily
    {
        DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                string Family, string Model, IList<string> lstPdLine, DateTime ShipDate, string ModelCategory);
    }
}
