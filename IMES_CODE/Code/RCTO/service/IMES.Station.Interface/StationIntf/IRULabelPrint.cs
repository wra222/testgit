using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRULabelPrint
    {
        ArrayList CheckPalletNo(string PalletNo);

        ArrayList Print(string PalletNo, string RUNoQty, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);

        void Cancel(string sessionKey);

		/// <summary>
        /// RePrint
        /// </summary>

        ArrayList CheckRePrintPalletNo(string PalletNo);

        ArrayList RePrint(string palletNo, string line, string editor, string station, string customer, IList<PrintItem> printItems);
    }
}
