using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_PrdIdOnLine
    {
        DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
             IList<string> lstPdLine, string Station, string Pno, string Family, string ModelCategory);
    }
}
