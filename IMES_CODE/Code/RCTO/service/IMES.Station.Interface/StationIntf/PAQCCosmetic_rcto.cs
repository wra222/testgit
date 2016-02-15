using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPAQCCosmetic_rcto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="flag"></param>
        /// <param name="pdline"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList ProcessInput(string prodid, string flag, string pdline, string station, string editor, string customer);

        void Cancel(string sessionKey);

        ArrayList GetDefectInfo(string code);

        ArrayList Save(string key, IList<string> list);

    }
}
