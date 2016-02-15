// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [PCA Repair Input] 实现如下功能：
    /// 批量刷入不良MB SN，实现MB移转进修护室
    /// </summary>
    public interface IPCARepairInput
    {
        /// <summary>
        /// 1.1	UC-PCA-PRI-01 PCA Repair Input
        /// 批量刷入不良MB SN，实现MB移转进修护室
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="editor">operator</param>
        void PCARepairInput(
            string pdLine,
            string MB_SNo,
            string editor, string stationId, string customer);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
