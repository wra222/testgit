using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;
using System;

namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    /// <summary>
    /// Row data define of COAReturn page
    /// </summary>
    public struct S_DNfor146
    {
        public string DnNo;
        public int RemainQty;
        public int CartonQty;
        public int Qty;
        public string ShipDate;
        public string ShipWay;
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ICombineCartonAndDNfor146
    {
        ArrayList GetDeliveryAndVendorCodeList(string model,string materialType,int beginDay, int endDay);
        S_DNfor146 GetDnInfo(string dnNo,string materialType);
      //  ArrayList GetDeliveryInfo(string dnNo);
        void CheckCrSn(string sn, string line, string editor, string customer, string materialType, string station, string model); //Check Material SN
         void CheckMaSn(string sn,string station); //Check Material SN
         ArrayList Save(IList<string> snList, string model, string dnNo, string qty, string line, string editor,
                                string station, string customer, string materialType,IList<PrintItem> printItems);
        void Cancel(string sn);
        ArrayList RePrint(
           string ct,
           string reason,
           IList<PrintItem> printItems,
           string editor,
           string stationId, string customer);
	
    }
}
