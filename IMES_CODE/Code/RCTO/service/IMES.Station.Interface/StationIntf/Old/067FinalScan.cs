using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入Pick ID以及Pallet No，实现Final Scan作业。
    /// 目的：Empty
    /// </summary>
    public interface IFinalScan
    {
        /// <summary>
        /// 输入pickID相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pickID">Pick ID</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <returns>Remain Pallet and Remain Qty</returns>
        IList InputPickID(
            string pdLine,
            string pickID,
            string editor, string stationId, string customer);

        /// <summary>
        /// 输入Pallet No相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="pltNo">Pallet No</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer"customer></param>
        IList InputPalletNo(
            string pdLine,
            string pickID,
            string pltNo,
            string editor, string stationId, string customer);

        IList<FwdPltInfo> GetFwdPltList(
            string pdLine,
            string pickID,
            string pltNo,
            string editor, string stationId, string customer);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
