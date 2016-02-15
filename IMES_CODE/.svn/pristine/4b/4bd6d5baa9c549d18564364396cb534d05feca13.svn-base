using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_MOQuery
    {
        DataTable GetMOQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                             string Family, IList<string> Model, IList<string> MO, string ProductType);
        DataTable GetMO(string Connection, DateTime FromDate, DateTime ToDate,
                        string Family, IList<string> Model);
        DataTable GetMOModel(string Connection, DateTime FromDate, DateTime ToDate,
                        string Family);

    }
}
