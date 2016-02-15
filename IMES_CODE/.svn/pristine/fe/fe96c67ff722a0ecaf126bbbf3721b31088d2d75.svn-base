using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入整机Customer SN，实现PAQC Input作业。
    /// 目的：Empty
    /// </summary>
    public interface IPAQCInput
    {
        /// <summary>
        /// 输入Customer SN相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="custSN">Customer SN</param>
        /// <param name="editor">operator</param>
        /// <returns>抽检结果: "EOQC", "OQC", or "SKIP"</returns>
        IList InputCustSN(
            string pdLine,
            string custSN,
            string editor, string stationId, string customer, IList<PrintItem> lstPrtItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        /// <summary>
        /// Reprint Label
        /// </summary>
        /// <param name="custSN"></param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>
        /// <param name="printItems"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        IList<PrintItem> ReprintLabel(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems, string reason);
    }
}
