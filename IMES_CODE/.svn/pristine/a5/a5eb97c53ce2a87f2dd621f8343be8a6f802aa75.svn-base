/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:IMES service interfaces for PoData (for docking) Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PoData to IMES for Docking.docx
 * UC:CI-MES12-SPEC-PAK-UC PoData to IMES for Docking.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-06-06  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Docking.Interface.DockingIntf
{
    /// <summary>
    /// Change Key Parts
    /// </summary>
    public interface IPoData
    {
        /// <summary>
        /// 上传PO Data
        /// </summary>
        /// <param name="dnLines">dnLines</param>
        /// <param name="pnLines">pnLines</param>
        /// <param name="editor">editor</param>
        /// <param name="sum">Total qty in these DN</param>
        IList<DNForUI> UploadData(IList<string> dnLines, IList<string> pnLines, string editor, out long sum);

        /// <summary>
        /// 获取符合输入条件的DN列表
        /// </summary>
        /// <param name="cond">Query-Condition</param>
        /// <param name="realCount">realCount</param>
        /// <param name="sum">Total qty in these DN</param>
        IList<DNForUI> QueryData(DNQueryCondition cond, out int realCount, out long sum);

        /// <summary>
        /// 获取DN属性列表
        /// </summary>
        /// <param name="dn">DeliveryNo</param>
        IList<DNInfoForUI> GetDNInfoList(string dn);

        /// <summary>
        /// 获取DN对应的Pallet列表
        /// </summary>
        /// <param name="dn">DeliveryNo</param>
        IList<DNPalletQty> GetDNPalletList(string dn);

        /// <summary>
        /// 获取Shipment对应的PalletCapacity列表
        /// </summary>
        /// <param name="ship">ShipmentNo</param>
        IList<PalletCapacityInfo> GetDNPalletCapacityList(string ship);

        /// <summary>
        /// 删除DN
        /// </summary>
        /// <param name="dn">DeliveryNo</param>
        int DeleteDN(string dn, string editor);

        /// <summary>
        /// 删除Shipment
        /// </summary>
        /// <param name="ship">ShipmentNo</param>
        int DeleteShipment(string ship, string editor);
        
        /// <summary>
        /// 获取打印模板
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="printItems">printItems</param>
        IList<PrintItem> GetPrintTemplate(string customer, IList<PrintItem> printItems);
    }
}
