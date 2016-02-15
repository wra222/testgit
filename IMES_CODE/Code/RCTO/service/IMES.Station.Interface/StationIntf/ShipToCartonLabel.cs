using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IShipToCartonLabel
    {
        IList<string> WFStart(string pdLine, string station, string editor, string customer);
        void WFCancel(string sessionKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByUI(string dn);

        IList<DeliveryPalletInfo> GetPalletInfoByDN(string dn);

        IList<ProductModel> GetProductByPallet(string plt);

        int GetQtyByPallet(string plt);

        int GetScanQtyByPallet(string plt);

        ArrayList InputProcess(string data, int type, string sessionKey, IList<PrintItem> printItemLst, string pdLine, string station, string editor, string customer);

        string GetMessageByPalletAndLocation(string deliveryNo, string pallet);

        ArrayList Reprint(string prodid, int key, string editor, string station, string customer, string reason, string pCode, IList<PrintItem> printItems);

        ArrayList ChangePrintLabel(string key);

        ArrayList ChangeRePrintLabel(string key);

        IList<string> GetEditAddr(string name);


    }   
}
