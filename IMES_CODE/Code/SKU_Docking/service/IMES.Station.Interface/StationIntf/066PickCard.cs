using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入整机Truck ID，实现Pick Card Print作业。
    /// 目的：Empty
    /// </summary>
    public interface IPickCard
    {
        /// <summary>
        /// Init function
        /// </summary>
        /// <param name="param"></param>
        /// <returns>Item 0: date string</returns>
        IList Init(IList param);

        /// <summary>
        /// 输入truckID相关信息并处理
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="truckID">Truck ID</param>
        /// <param name="dateStr">date str</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customer">customer</param>
        /// <param name="items">items</param>
        /// <returns>List of PrintItem</returns>
        IList InputTruckID(
            string pdLine,
            string truckID,
            string dateStr,
            string editor, string stationId, string customer, IList<PrintItem> items);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

    }
}
