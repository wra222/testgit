using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_ProductYield
    {
        DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                List<string> Station, string Family, IList<string> lstPdLine, string Model, List<string> lstStation, string ModelCategory);

        DataTable GetDetailQueryByInput(string Connection, string InputBegDate, string InputEndDate, string Family,
                                                    string Model, string Line, string Station, string Category);

        DataTable GetDetailQueryByDefect(string Connection, string InputBegDate, string InputEndDate, string Family,
                                                    string Model, string Line, string Station, string Category);
    }
}
