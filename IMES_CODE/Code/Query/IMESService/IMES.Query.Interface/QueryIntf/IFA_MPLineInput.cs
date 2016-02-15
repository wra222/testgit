using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_MPLineInput
    {
        DataTable GetQueryResult(string Connection, DateTime FromDate, DateTime ToDate,
                IList<string> lstPdLine, string Family, IList<string> Model, string StationList, bool IsWithoutShift, string InputStation, bool grpModel, string ModelCategory);

        DataTable GetPoModel(string Connection, DateTime FromDate, DateTime ToDate, string Family, string Station);

        DataTable GetSelectDetail(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, IList<string> lstModel, string Line, string Station, bool IsWithoutShift, string InputStation);

        DataTable GetSelectDetail(string Connection, DateTime FromDate, DateTime ToDate,
                            IList<string> lstPdLine, string Family, IList<string> lstModel, string Line, string Station, bool IsWithoutShift, string InputStation, string StationList);
        
    }
}
