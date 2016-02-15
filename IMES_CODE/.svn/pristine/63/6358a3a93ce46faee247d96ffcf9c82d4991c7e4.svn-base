using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入Image D/L完成的整机，记录不良品信息。
    /// 目的：记录不良品信息
    /// </summary>
    public interface IIMAGEDownloadCheck
    {
        /// <summary>
        /// 输入ProductId和相关信息, 然后卡站
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        void InputProdId(
            string pdLine,
            string prodId,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 输入Defect Items然后保存
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="defectList">Defect IList</param>
        void InputDefectCodeList(
            string prodId,
            IList<string> defectList);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
