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
    public interface PIAOutput
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
            string editor, string stationId, string customer, out string qcStatus);

        /// <summary>
        /// 记录良品/不良品信息（免检模式不走此站）。
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="defectList">Defect IList</param>
        void InputDefectCodeList(
            string prodId,
            IList<string> defectList, bool isInputDefet, string qcStatus);

        /// <summary>
        /// Cancel
        /// </summary>
        void Cancel(string prodId);

        /// <summary>
        /// 记录良品/不良品信息（免检模式不走此站）。
        /// </summary>
        /// <param name="RetStation">RetStation</param>
        /// <param name="CustSn">CustSn</param>
        List<string> MVunpack(
            string RetStation,
            string CustSn,
            string editor, string stationId, string customer);
    }
}
