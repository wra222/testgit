using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// CreateProductandCombineLCM
    /// </summary>
    public interface ICreateProductandCombineLCM
    {
        IList<string> CreateProductID(string pdLine, string model, string mo, string editor, string station, string customer, string family, string moPrefix);

        IList<BomItemInfo> InputProdIdorCustsn(string pdLine,string input,string editor, string stationId, string customerId, out string realProdID, out string model);

        IMES.DataModel.MatchedPartOrCheckItem InputPPID(string prodId,string ppid);

        IMES.DataModel.MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);

        IList<PrintItem> Save(string prodId, bool flag, bool flag_39, IList<PrintItem> printItems, out string custsn);
        
        ArrayList Reprint(string prodid, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);

        void ClearPart(string sessionKey);

        void Cancel(string sessionKey);

        IList<MOInfo> GetMOListByFamily(string Family);
   }
}
