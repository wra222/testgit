using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOfflineLcdCtPrint
    {
        bool CheckModel(string model, string customer);

        ArrayList Print(string model,string ct, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);

        void Cancel(string sessionKey);


    }
}
