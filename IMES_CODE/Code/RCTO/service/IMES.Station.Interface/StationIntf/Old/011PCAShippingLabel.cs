// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [PCA Shipping Label] 实现如下功能：
    /// 批量刷入不良MB SN，实现MB移转进修护室
    /// </summary>
    public interface IPCAShippingLabel
    {
        /// <summary>
        /// 1.1	UC-PCA-PSL-01 PCA Shipping Label
        /// 确认良品MB信息，确认半制生产完成
        /// 1.	记录半制生产完成良品MB信息，如ECR、Version、Dcode、是否附CPU、VGA/B等。
        /// 2.	列印MB Shipping Label
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="dCode">DCode </param>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="editor">操作员工号</param>
        /// <returns>Print Items</returns>
        void InputMBSn(
            string pdLine,
            string dCode,
            string MB_SNo,
            string editor, string stationId, string customerId,string no1397);

        IList<PrintItem> Save(string MB_SNo, string dcode, IList<PrintItem> printItems,out string wcode);

        /// <summary>
        /// 1.2	UC-PCA-PSL-02 Reprint
        /// 标签损坏时，重印
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="dCode">DCode </param>
        /// <param name="editor">editor </param>
        /// <param name="stationId">stationId </param>
        /// <param name="customerId">customerId </param>
        /// <param name="printItems">printItems</param>
        /// <param name="MB_SNo">MB SNo</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Reprint(
            string pdLine,
            string dCode,
            string MB_SNo,
            string editor, string stationId, string customerId, IList<PrintItem> printItems, string reason, out string wcode);

        /// <summary>
        /// 输入[Check Item]信息，返回匹配的CheckItemId
        /// </summary>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="item">Scanned item info, may not be exact itemId</param>
        /// <returns>checkItemId</returns>
        IList<MatchedPartOrCheckItem> InputCheckItemData(
            string MB_SNo,
            string item);

       IList<string> Get1397No(string Mbsn);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);

        IList<BomItemInfo> GetCheckItemList(string sessionKey);
    }
}
