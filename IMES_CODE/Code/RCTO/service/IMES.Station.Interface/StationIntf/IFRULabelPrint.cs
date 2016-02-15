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
    public interface IFRULabelPrint
    {
        
        /// <summary>
        /// InputModel
        /// </summary>
        /// <param name="model">string</param>
        /// <param name="editor">string</param>
        /// <param name="stationId">string</param>
        /// <param name="customer">string</param>
        /// <param name="items">PrintItem</param>
        /// <returns></returns>
        ArrayList InputModel(string model, string editor, string stationId, string customer, IList<PrintItem> items);

        ArrayList CheckModel(string model);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

    }
}
