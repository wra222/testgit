using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_ProductPlanInputQuery
    {
        DataTable GetQueryResult(string Connection, string PdLine, string ShipDate, string model);

        DataTable GetQueryResultByDayRange(string Connection, string PdLine, string dayFrom, string dayTo, string model);

        DataTable GetDetailQueryResult(string Connection, string PdLine, string ShipDate, string model,string station, string type);

        DataTable GetDetailQueryResultByDayRange(string Connection, string PdLine, string dayFrom, string dayTo, string model, string station, string type, string inputPdLine, string ShipDate);

        DataTable GetPdLine(string Connection, string Starg);
    }
}
