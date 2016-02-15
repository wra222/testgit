using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入OQC检验完成的整机，记录良品/不良品信息（免检模式不走此站）。
    /// 目的：实现整机到Image D/L的移转
    /// </summary>
    public interface ChangeToEPIAPIA
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>prestation</returns>
        List<string> InputProdId(
            string pdLine,
            string prodId,
            string editor, string stationId, string customer,bool isChangeToEpia, out string qcStatus);

       
    }
}
