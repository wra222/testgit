/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service interface for PoData Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI PO Data.docx –2011/11/08 
 * UC:CI-MES12-SPEC-PAK-UC PO Data.docx –2011/11/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-09  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Change Key Parts
    /// </summary>
    public interface IPoData
    {
        /// <summary>
        /// 上传PO Data
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="dnLines">dnLines</param>
        /// <param name="pnLines">pnLines</param>
        /// <param name="editor">editor</param>
        IList<DNForUI> UploadData(string type, IList<string> dnLines, IList<string> pnLines, string editor);

        /// <summary>
        /// 获取符合输入条件的DN列表
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="cond">Query-Condition</param>
        /// <param name="realCount">realCount</param>
        IList<DNForUI> QueryData(string type, DNQueryCondition cond, out int realCount);

        /// <summary>
        /// 获取符合输入条件的DN列表(delete for OB user page)
        /// </summary>
        /// <param name="input">input-string</param>
        /// <param name="realCount">realCount</param>
        IList<VPakComnInfo> QueryOBData(string input, out int realCount);

        /// <summary>
        /// 获取DN属性列表
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="dn">DeliveryNo</param>
        IList<DNInfoForUI> GetDNInfoList(string type, string dn);

        /// <summary>
        /// 获取DN对应的Pallet列表
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="dn">DeliveryNo</param>
        IList<DNPalletQty> GetDNPalletList(string type, string dn);

        /// <summary>
        /// 获取Shipment对应的PalletCapacity列表
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="ship">ShipmentNo</param>
        IList<PalletCapacityInfo> GetDNPalletCapacityList(string type, string ship);

        /// <summary>
        /// 删除DN(for PL user)
        /// </summary>
        /// <param name="dn">DeliveryNo</param>
        int DeletePLDN(string dn, string editor);

        /// <summary>
        /// 删除DN(for OB user)
        /// </summary>
        /// <param name="dn">DeliveryNo</param>
        void DeleteOBDN(string dn, string editor);

        /// <summary>
        /// 删除DN By 10-bit DN List(for OB user)
        /// </summary>
        /// <param name="dnList">DeliveryNo List</param>
        /// <param name="fList">Failed DN List(Bad input)</param>
        void BatchDeleteOBDN(IList<string> dnList, string editor, out IList<string> fList);

        /// <summary>
        /// 删除Shipment
        /// </summary>
        /// <param name="ship">ShipmentNo</param>
        int DeleteShipment(string ship,string editor);
    }
}
