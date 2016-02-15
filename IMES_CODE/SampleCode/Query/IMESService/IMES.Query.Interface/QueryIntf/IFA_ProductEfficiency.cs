using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_ProductEfficiency
    {
        DataTable GetInputModel(string Connection, DateTime FromDate, DateTime ToDate, List<string> Family);

        DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                             string Family, IList<string> Model);

        DataTable GetQueryPeriodDetail(string Connection, DateTime FromDate, DateTime ToDate,
                     string Family, IList<string> Model, IList<string> Station, IList<string> Line);
        
        DataTable GetQueryDetail(string Connection, DateTime FromDate, DateTime ToDate,
                             string Family, IList<string> Model, IList<string> Station);

    }
}
