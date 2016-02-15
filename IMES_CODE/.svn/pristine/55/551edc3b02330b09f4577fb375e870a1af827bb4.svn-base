using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_MPInputEx
    {
        DataTable GetQueryResult(string Connection, DateTime ShipDate,
                IList<string> lstPdLine, string Family, IList<string> Model, string StationList, bool IsWithoutShift, string InputStation, bool grpModel, string ModelCategory);

        DataTable GetPoModel(string Connection, DateTime FromDate, DateTime ToDate, string Family, string Station);

        DataTable GetSelectDetail(string Connection, DateTime ShipDate,
                            IList<string> lstPdLine, string Family, IList<string> lstModel, string Line, string Station, bool IsWithoutShift);
        int[] GetDNShipQty(string Connection, DateTime ShipDate, string Model,string PrdType);
        //DataTable GetSelectDetail(string Connection, DateTime ShipDate,
        //                    IList<string> lstPdLine, string Family, IList<string> lstModel, string Line, string Station, bool IsWithoutShift);
        
    }
}
