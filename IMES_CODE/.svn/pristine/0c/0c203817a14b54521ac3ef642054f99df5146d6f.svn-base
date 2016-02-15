using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISKULabelPrint
    {
        ArrayList GetPCBID(string ProductID);

        ArrayList Print(string prodid,string PCBID, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);

        void Cancel(string sessionKey);

		/// <summary>
        /// RePrint
        /// </summary>
        ArrayList RePrint(string prodid, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);
    }
}
