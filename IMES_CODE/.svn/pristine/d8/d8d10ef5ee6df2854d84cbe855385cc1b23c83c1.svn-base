
using System.Collections;
using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 将MB投入成制生产组装，记录良品MB信息和PrdId信息。
    /// 目的：建立MB SN与PrdId的一一对应关系，完成半制与成制的数据衔接
    /// </summary>
    public interface IBoardInput
    {
        /// <summary>
        /// 输入ProdId和相关信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="prodId">Product Id</param>
        /// <param name="editor">operator</param>
        IList<BomItemInfo> InputProdIdorCustsn(
            string pdLine,
            string input,
            string editor, string stationId, string customerId, out string realProdID, out string model);

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

        IList<PrintItem> Save(string prodId, IList<PrintItem> printItems, bool printflag, out string custsn);

        IList<PrintItem> PrintCustsnLabel(IList<PrintItem> printItems, string custsn);

        ArrayList Reprint(string prodid, string reason, string line, string editor, string station, string customer, string pCode, IList<PrintItem> printItems);

        /// <summary>
        /// SetDataCodeValue
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        string SetDataCodeValue(string model, string customer);

        ArrayList saveForPCAShippingLabel(
            string prodId,
            string MBno,
            string model,
            string dcode,
            IList<PrintItem> printItems);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
        int CountQty(string line);
    }
}
