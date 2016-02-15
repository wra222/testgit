using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 将流程卡和Kitting Box结合，放入，一起投入kitting生产线
    /// 目标：建立PrdId和Box ID的对应关系，以实现通过kitting系统分拣Key parts
    /// </summary>
    public interface IAssignWHLocation
    {
        /// <summary>
        /// 输入PdLine和ProdId并检查
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        ArrayList InputProdId(
            string floor,
            string pdLine,
            string customerSn, //prodId,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 输入9999 update table.
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="boxId"></param>
        void closeLocation(
            string floor, 
            string pdLine, 
            string customerSn, 
            string editor, 
            string stationId, 
            string customerId, 
            string location);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
