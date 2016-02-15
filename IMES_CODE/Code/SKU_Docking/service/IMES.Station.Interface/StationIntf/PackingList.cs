using System.Collections.Generic;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    public interface IPackingList
    {
        ArrayList PackingListForOBCheck(string pdline, string station, string editor, string customer,
                                string data, string doc_type, string region, string carrier, string sessionKey);

        IList<string> PackingListForOBQuery(string pdline, string station, string editor, string customer,
                                        string region, string carrier, string begintime, string endtime);

        IList<string> WFStart(string pdLine, string station, string editor, string customer);

        void WFCancel(string sessionKey);


        ArrayList CheckSN(string sn, string flag, int count, string doctype, string pdline, string station, string editor, string customer);

        void insertPrintListTable(string list, int count);
    }

    public interface IPackingListForControl
    {
        IList<string> GetDocTypeList(string service);
        IList<string> GetRegionList();
        IList<string> GetCarrierList();
    }
}
