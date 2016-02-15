
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    public interface IPrintBottomCaseLabel
    {

        /// <summary>
        /// 打印下殼標籤
        /// </summary>
        /// <param name="inputSn">custsn</param>
        /// <param name="pdLine">product line</param>
        /// <param name="stationId">product station</param>
        /// <param name="editor">editor</param>
        /// <param name="customerId">customer sn</param>
        /// <param name="printItems">Print Item列表</param>
        /// <returns>Print Item列表</returns>
        IList<PrintItem> Print(string custsn, string pdLine, string stationId, string editor, string customerId, string DenyStationList, IList<PrintItem> printItems, out string mac, out string model, out string sku, out string imei, out string imsi, out string iccid);
        
    }
}