using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;
using System;

namespace IMES.Station.Interface.StationIntf
{
    public interface ITouchGlassCheckTime
    {
        /// <summary>
        /// check Process
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="station"></param>
        void CheckMaSn(string sn, string station); //Check Material SN
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="snList"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="materialType"></param>
        /// <param name="printItems"></param>
        /// <returns></returns>
        ArrayList Save(string ct,  string line, string editor,
                               string station, string customer, string materialType, IList<PrintItem> printItems);
        void Cancel(string sn);
        ArrayList RePrint(
           string ct,
           string reason,
           IList<PrintItem> printItems,
           string editor,
           string stationId, string customer);
    }
}
