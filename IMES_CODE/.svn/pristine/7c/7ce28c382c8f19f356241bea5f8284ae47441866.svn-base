using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入整机PrdId，实现整机移转进OQC检验作业。
    /// 目的：实现整机从成制组装作业到OQC检验作业的交接
    /// </summary>
    public interface IOQCInput
    {
        /// <summary>
        /// 输入Product Id相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        /// <returns>抽检结果: "EOQC", "OQC", or "SKIP"</returns>
        string InputProdId(
            string pdLine,
            string prodId,
            string editor, string stationId, string customer);

        string InputProdId(
            string pdLine,
            string prodId,
            string editor, 
            string stationId, 
            string customer,
            out string Line);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);


    }
}
