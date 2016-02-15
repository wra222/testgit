using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRCTOLabelPrint
    {
        ArrayList ProcessProd(string prodid, string existProd, string pdline, string station, string editor, string customer);

        void Cancel(string sessionKey);

        ArrayList ProcessMB(string key, string mbsn, IList<PrintItem> printItems);

        ArrayList Reprint(string prodid, string reason, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);
		
		/// <summary>
        /// RePrint
        /// </summary>
        ArrayList RePrint(string prodid, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);
    }
}
