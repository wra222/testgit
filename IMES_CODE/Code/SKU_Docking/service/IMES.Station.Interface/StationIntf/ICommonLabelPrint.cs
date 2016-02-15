using System.Collections.Generic;
using System.Collections;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommonLabelPrint
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printItems"></param>
        /// <param name="pdline"></param>
        /// <param name="prodid"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList GetOfflineLabelSettingList(string editor, string customer);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdline"></param>
        /// <param name="station"></param>
        /// <param name="editor"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        ArrayList GetOfflineLabelSetting(string labelDescr, string editor, string customer);
        

    }
}
