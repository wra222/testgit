// created by itc205033

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;
namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [MB Label Print] 实现如下两个功能：
    /// 1.	根据Mo，产生MB SNo，建立MB SNo与Mo的对应关系，并列印Barcode；
    /// 2.	打断某些MB SNo与Mo的归属关系。
    /// 这样做的目的有两个：
    /// 1.	实现通过Mo管控MB SNO
    /// 2.	实现生产订单追踪
    /// </summary>
    public interface IMBLabelPrint
    {
        /// <summary>
        /// 1.1	UC-PCA-MLP-01 Print
        /// 依据Mo
        /// 1.	产生MB SNo，并建立MB SNo 与Mo 的对应关系
        /// 2.	列印MB Label
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="isNextMonth">"ThisMonth"或"NextMonth"</param>
        /// <param name="mo">MO字串</param>
        /// <param name="qty">打印数量</param>
        /// <param name="dateCode">日期码</param>
        /// <param name="editor">operator</param>
        /// <param name="startProdIdAndEndProdId">返回起始和结束的ProdId</param>
        /// <returns>Print Items</returns>
        IList<PrintItem> Print(
            string pdLine,
            bool isNextMonth, 
            string mo,
            int qty,
            string dateCode,
            string editor, string stationId, string customerId,
            out IList<string> startProdIdAndEndProdId, string _111, IList<PrintItem> printItems);

        /// <summary>
        /// 1.2	UC-PCA-MLP-02 Find (暂不实现)
        /// 依据用户选择的[MB_CODE/PCB]/[111]/[Mo]/[This Month] or [Next Month]，Find & Display 产生MB SNo 的Start MB SNo。
        /// </summary>
        /// <param name="mo">MO字串</param>
        /// <param name="isNextMonth">"ThisMonth"或"NextMonth"</param>
        /// <param name="editor">operator</param>
        /// <returns>MB SNo</returns>
        string Find(
            string mo,
            string isNextMonth,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 1.3	UC-PCA-MLP-03 Dismantle
        /// 打断MB SNo与Mo的归属关系
        /// 异常情况：
        /// a.	如果没有输入[Start MB SNo]，则报告错误：“请输入Start MB/SB/VB NO !!”
        /// b.	如果没有输入[Dismantle Reason]，则报告错误：“必须输入Dismantle Reason”
        /// c.	如果用户输入的[End MB SNo]<[Start MB SNo]，则报告错误：“End MB SNo 必须大于 Start MB SNo"
        /// d.	如果用户输入的[End MB SNo]的Mo与[Start MB SNo] 的Mo 不同，则报告错误：“Start MB SNo 与 End MB SNo 的Mo 不同！”
        /// 异常情况：
        /// a.	如果在用户输入的[Start MB SNo]，[End MB SNo] 范围内在[SnoDetPCB] 表中没有记录存在，则报告错误：“MB NO is not exist !!”；注意需要明确的报告是MB NO，VB NO ，还是SB NO 不存在。
        /// b.	如果指定的MB SNo 已经投入生产，则报告错误“MB SNo: @mbsno 已经投入生产，不能进行Dismantle!!”
        /// </summary>
        /// <param name="startMBSNo">开始MB SNo</param>
        /// <param name="endMBSNo">结束MB SNo</param>
        /// <param name="reason">打断原因</param>
        /// <param name="editor">operator</param>
        /// <param name="stationId">stationId</param>
        /// <param name="customerId">customerId</param>
        void Dismantle(
            string startMBSNo,
            string endMBSNo,
            string reason,
            string editor, string stationId, string customerId);


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void Cancel(string sessionKey);


        void Reprint(
            string startMBSNo,
            string endMBSNo,
            string reason,
            string editor, string stationId, string customerId);

      //  ArrayList GetMBNoList(string beginNo, string endNo);
        IList<PrintItem> RePrintMbLabel(string beginNo, string endNo, string customerId, string reason, string editor,  string stationId, IList<PrintItem> printItems, out List<string> lstMBNo, out List<string> lstParttNo);
        bool CheckIsProduct(string beginNo, string endNo, string SA1StationName, out string ExistMB);
    }
}