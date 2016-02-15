using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 完成kitting Box中Key parts label信息收集。
    /// 目的：比对kitting Box中Key parts label和系统所建资料是否一致完整；保证parts符合BOM设定，另外便于后续的追踪

    /// </summary>
    public interface ICombineKeyParts
    {
        /// <summary>
        /// 输入ProdId和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="subStation">Sub-station</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        IList<BomItemInfo> InputProdIdorCustsn(
            string pdLine,
            string input,
            string editor, string stationId, string customerId, out string realProdID, out string model);

        /// <summary>
        /// 输入PPID
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="ppid">PPID</param>
        /// <returns>主料的Part No</returns>
        IList<MatchedPartOrCheckItem> InputPPID(
            string prodId,
            string ppid);

        void Save(string prodId);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
   }
}
