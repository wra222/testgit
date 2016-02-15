/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Pallet Verify
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx 
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * Known issues:
 */

using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;
using System.Data;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// Pallet Verify
    /// station : 9A
    /// 本站实现的功能：
    ///     检查Pallet 上的所有PRODUCT；
    ///     列印Ship to Pallet Label；
    ///     内销要额外列印一张Pallet CPMO Label
    /// </summary> 
    /// 
    public interface IPalletVerifyDataForDocking
    {
        #region methods interact with the running workflow

        /// <summary>
        /// 刷第一个SN时，调用该方法启动工作流，根据输入CustSN获取PalletInfo,成功后调用InputCustSn
        /// 将custSN放到Session.CustSN中(string)
        /// 返回ArrayList
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station(Station="86")</param>
        /// <param name="customer">customer</param>
        /// <param name="index">index</param>
        /// <param name="labeltypeBranch">labeltypeBranch</param>
        /// <returns>PalletInfo</returns>
        ArrayList InputFirstCustSn(string firstSn, string line, string editor, string station, string customer, out int index, out string labeltypeBranch);

        /// <summary>
        /// 每次刷SN都调用该方法, SFC
        /// 将custSN放到Session.CustSN中(string)
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="custSn">custSn</param>
        /// <returns>ArrayList</returns>
        ArrayList InputCustSn(string firstSn, string custSn);

        /// <summary>
        /// Save Data，返回打印重量标签的PrintItem，结束工作流
        /// Update IMES_FA..ProductStatus – 更新Pallet 上所有SKU 的状态
        /// Station – Pallet Verify 站号
        /// Status – '1'
        /// Editor – Editor (from UI)
        /// Udt – Current Time
        /// Insert IMES_FA..ProductLog – 记录Pallet 上所有SKU 的Log
        /// 列印Ship to Pallet Label；内销要额外列印一张Pallet CPMO Label
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="printItems">printItems</param> 
        /// <param name="printparams1">printparams1</param>
        /// <param name="printparams2">printparams2</param>
        /// <returns></returns>
        ArrayList Save(string firstSn, IList<PrintItem> printItems);

        #endregion


        #region methods do not interact with the running workflow


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn"></param>
        void Cancel(string uutSn);

        #endregion

        #region call Op_TemplateCheckLaNew

        /// <summary>
        /// 调存储过程： op_TemplateCheck_LANEW
        /// </summary>
        /// <param name="dn">dn</param>
        /// <param name="docType">docType</param>
        /// <returns>DataTable</returns>
        DataTable call_Op_TemplateCheckLaNew(string dn, string docType);

        #endregion

        #region getDeliveryPalletList

        /// <summary>
        /// 与Pallet绑定的所有Delivery
        /// </summary>
        /// <param name="palletno">palletno</param>
        /// <returns>list</returns>
        IList<string> getDeliveryPalletList(string palletno);

        #endregion

        #region rePrint


        /// <summary>
        /// 重印
        /// </summary>
        /// <param name="palletNo">palletNo</param>
        /// <param name="reason">reason</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>

        ArrayList rePrint(string palletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems, out ArrayList printparams1, out string labeltypeBranch, out IList<string> PDFPLLst, out IList<string> modelList);

        #endregion

    }
}
