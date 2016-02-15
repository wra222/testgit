using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_ProductStatement
    {
        DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, string Station, IList<string> Model, bool IsWithoutShift, string ModelCategory);
        DataTable GetQueryResultByModel(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, string Station, IList<string> Model, bool IsWithoutShift, string ModelCategory);
        DataTable GetInputModel(string Connection, DateTime FromDate, DateTime ToDate, string Family, string Station);


    }
}
