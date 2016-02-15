/*
 * INVENTEC corporation (c)2011 all rights reserved. 
 * Description: Pallet Verify FDE Only
 * UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
 * UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx 
 * Update: 
 * Date         Name                Reason 
 * ==========   =================   =====================================
 * 2011-11-22   Chen Xu (eB1-4)     create
 * Known issues:
 */

using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Pallet Verify FDEOnly
    /// station : 99
    /// 本站实现的功能：
    ///     检查Pallet 上的所有SKU；
    ///     列印Ship to Pallet Label；
    ///     列印Pallet SN Label；Delivery Label
    /// </summary> 
    /// 
    public interface IPalletVerifyFDEOnly
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
        /// <returns>PalletInfo</returns>
        ArrayList InputFirstCustSn(string firstSn, string line, string editor, string station, string customer, out int index);

        /// <summary>
        /// 每次刷SN都调用该方法, SFC
        /// 将custSN放到Session.CustSN中(string)
        /// </summary>
        /// <param name="firstSn">firstSn</param>
        /// <param name="custSn">custSn</param>
        /// <returns>index</returns>
        int InputCustSn(string firstSn, string custSn);

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
        /// <param name="ScanProductNoList">ScanProductNoList</param>
        /// <param name="DummyPalletNo">DummyPalletNo</param>
        /// <param name="printParams">printParams</param>
        /// <returns></returns>
        IList<PrintItem> Save(string firstSn, IList<PrintItem> printItems, IList<string> ScanProductNoList, out string DummyPalletNo, out ArrayList printParams);

        #endregion


        #region methods do not interact with the running workflow


        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="uutSn"></param>
        void Cancel(string uutSn);

        #endregion

        #region rePrint


        /// <summary>
        /// 重印
        /// </summary>
        /// <param name="dummyPalletNo">dummyPalletNo</param>
        /// <param name="reason">reason</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="printItems">printItems</param>
        IList<PrintItem> rePrint(string dummyPalletNo, string reason, string line, string editor, string station, string customer, IList<PrintItem> printItems);

        #endregion

    }
}
