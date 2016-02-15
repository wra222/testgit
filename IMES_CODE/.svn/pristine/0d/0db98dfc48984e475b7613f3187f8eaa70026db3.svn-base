using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 刷入需要进行Rework 的笔记本的Product Id 及其它相关数据，进入Rework 流程
    /// </summary>
    public interface IReworkInput
    {
        /// <summary>
        /// 输入ProductId并检查
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="reworkCode">Rework Code</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        void InputProdId(
            string pdLine,
            string reworkCode,
            string prodId,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 输入检查条目并检查
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="checkItem">Check Item</param>
        void InputCheckItem(
            string prodId,
            string checkItem);
    }
}
