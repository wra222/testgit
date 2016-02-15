using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface PCAMBContact
    {
        /// <summary>
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="NewMB">New MB</param>
        /// <param name="OldMB">Old MB</param>
        /// <returns>prestation</returns>
        string CheckMBandSave(
            string pdLine,
            string NewMB,
            string OldMB,
            string editor, string stationId, string customer);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);
    }
}
