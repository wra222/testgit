using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入OQC检验完成的整机，记录良品/不良品信息（免检模式不走此站）。
    /// 目的：实现整机到Image D/L的移转
    /// </summary>
    public interface ReturnUsedKeys
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="CustSN">CustSN</param>
        /// <param name="editor">operator</param>
        ArrayList Check(            
            string CustSN,  
            string editor, string stationId, string customer);


        /// <summary>
        /// Save
        /// </summary>
        ArrayList Save(
            List<string> SNList,
            List<string> PNList,
            List<string> errList,
            string editor,
            string stationId, string customer);
    }
}
