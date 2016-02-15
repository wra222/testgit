using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 完成kitting Box中剩余Key parts label信息收集。此站收集的part数量由operator根据产线要求输入的。
    /// 目的：比对kitting Box中Key parts label和系统所建资料是否一致完整；保证parts符合BOM设定，另外便于后续的追踪
    /// </summary>
    public interface IFinalPartsCollection
    {
        /// <summary>
        /// 输入Product Id和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
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
        IList<MatchedPartOrCheckItem> InputPPID(
            string prodId,
            string ppid);

        /// <summary>
        /// save product
        /// </summary>
        void Save(string prodId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    
    }

}
