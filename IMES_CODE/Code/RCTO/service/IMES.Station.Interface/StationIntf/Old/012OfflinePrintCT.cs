// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// 根据产线生产安排，选择CT Part，再根据维护的part基本信息打印出label
    /// 目的：便于后期parts的追踪
    /// </summary>
    public interface IOfflinePrintCT
    {
        /// <summary>
        /// 1.1	UC-FA-OPC-01 Offline Print CT
        /// 根据产线生产安排，选择CT Part，再根据维护的part基本信息打印出label
        /// 目的：便于后期parts的追踪
        /// </summary>
        /// <param name="PN">Part Number</param>
        /// <param name="DateCode">Date Code</param>
        /// <param name="Qty">要打印的数量</param>
        /// <param name="editor">operator</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> PrintCTForDell(
            string PN,
            string DateCode,
            int Qty,
            string editor, string stationId, string customer);

        /// <summary>
        /// 根据产线生产安排，选择CT Part，再根据维护的part基本信息打印出label
        /// 目的：便于后期parts的追踪
        /// </summary>
        /// <param name="AssemblyCode">AssemblyCode</param>
        /// <param name="DateCode">DateCode</param>
        /// <param name="VendoeDCode">VendoeDCode</param>
        /// <param name="qty">qty</param>
        /// <param name="editor">editor</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> PrintCTForTSB(
            string AssemblyCode,
            string DateCode,
            string VendorDCode,
            int qty,
            string editor, string stationId, string customer, IList<PrintItem> printItems, out IList<string> ctList);

        /// <summary>
        /// 根据产线生产安排，选择CT Part，再根据维护的part基本信息打印出label
        /// 目的：便于后期parts的追踪
        /// </summary>
        /// <param name="AssemblyCode">AssemblyCode</param>
        /// <param name="DateCode">DateCode</param>
        /// <param name="VendorDCode">VendorDCode</param>
        /// <param name="VendorCTLen">VendorCTLen</param>
        /// <param name="VendorSN">VendorSN</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> PrintCTForTSBCombine(
            string AssemblyCode,
            string DateCode,
            string VendorDCode,
            int VendorCTLen,
            string VendorSN,
            string editor, string stationId, string customer, IList<PrintItem> printItems, out IList<string> ctList, string pcode);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);
    }
}
