using System.Collections.Generic;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    public interface ScanningList
    {
        ArrayList ScanningListForCheck(string pdline, string station, string editor, string customer,
                                string data, string doc_type);

        void Cancel(string sessionKey);
        void Print(string sessionKey, List<string> printlist);
        ArrayList ScanningCopyFile();

    }
      
    public interface ScanningListForControl
    {
        IList<string> GetDocTypeList();
    }
}
