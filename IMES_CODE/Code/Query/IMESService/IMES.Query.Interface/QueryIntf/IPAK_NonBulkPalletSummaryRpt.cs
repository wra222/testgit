using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface IPAK_NonBulkPalletSummaryRpt 
    {
        DataTable GetQueryResultForDetatilSub(string Connection, DateTime shipDate, string paramType, string paramValue, string PAKStation, string model);
        DataTable GetDetail(string Connection, string type, string selectTxt);

        DataTable GetQueryResultForSummary(string Connection, DateTime fromShipDate, DateTime toShipDate,string prdType, string dbName);
        DataTable GetDetailForSummary(string Connection, DateTime shipDate, string PAKStation, string palletNo);
        DataTable GetQueryResultForDetatilMain(string Connection, DateTime shipDate, DateTime toDate, string PAKStation, string prdType);
        DataTable GetQueryResultForDetatilMain(string Connection, DateTime shipDate, DateTime toDate, string PAKStation);

    }
    
}
