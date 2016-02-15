using System;
using System.Collections;
using System.Collections.Generic;
using IMES.Docking.Interface.DockingIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// 本站实现的功能：检料
    /// </summary>
    public interface ICombinePack
    {
        /// <summary>
        /// 输入ProdId和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        IList<BomItemInfo> InputProdIdorCustsn(
            string input,　string editor, string stationId, string customerId, out string realProdID, out string model, out string CustomerSN, out bool IsPAQC);

        /// <summary>
        /// 输入MB sn/ Part Pno./ Part Sno/ ECR/ …
        /// </summary>
        /// <param name="prodId">Product Id</param>
        /// <param name="mbSn">MB SN</param>
	/// <param name="sub">替代料Part No</param>
	/// <returns>主料的Part No</returns>
        IMES.DataModel.MatchedPartOrCheckItem InputMBSn(
            string prodId,
            string mbSn);

        IMES.DataModel.MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);

        void Save(string prodId, out string custsn);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);



    }
}
