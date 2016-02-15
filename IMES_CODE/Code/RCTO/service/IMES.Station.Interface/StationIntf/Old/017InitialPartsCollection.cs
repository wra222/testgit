using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 收集相关parts信息，与ProdID进行绑定。为了提高生产效率，将part收集分为Initial和Final两站，Main Board是在单独的一站收集。此站收集的part数量由operator根据产线要求输入的。
    /// 目的：保证parts符合BOM设定，另外便于后续的追踪
    /// </summary>
    public interface IInitialPartsCollection
    {
        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="scanQty">Scan Qty</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        IList<BomItemInfo> InputProdId(
            string pdLine,
            string prodId,            
            string editor, string stationId, string customerId);

        /// <summary>
        /// 输入PPID
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="ppid">PPID</param>
        /// <returns>主料的Part No</returns>
        IList<MatchedPartOrCheckItem> InputPPID(
            string prodId,
            string ppid);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="prodId"></param>
        /// 
        void Save(string prodId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

    }

   

}
