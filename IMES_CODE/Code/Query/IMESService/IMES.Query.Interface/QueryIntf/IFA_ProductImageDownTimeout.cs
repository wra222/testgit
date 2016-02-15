using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_ProductImageDownTimeout
    {
        DataTable GetQueryResult(string DBConnection, string Line, DateTime FromDate, DateTime ToDate);
    }
}
