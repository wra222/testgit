/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for CombineDNPalletforBT Page            
 * CI-MES12-SPEC-PAK Combine DN & Pallet for BT.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    /// <summary>
    /// Row data define of CombineDNPalletforBT page
    /// </summary>
    public struct S_RowData_DN
    {
        /// <summary>
        /// DeliveryNO
        /// </summary>
        public string DeliveryNO;
        /// <summary>
        /// Model
        /// </summary>
        public string Model;
        /// <summary>
        /// CustomerPN
        /// </summary>
        public string CustomerPN;
        /// <summary>
        /// PoNo
        /// </summary>
        public string PoNo;
        /// <summary>
        /// Date
        /// </summary>
        public string Date;
        /// <summary>
        /// Qty
        /// </summary>
        public string Qty;
        /// <summary>
        /// PackedQty
        /// </summary>
        public string PackedQty;
    };
    
    /// <summary>
    /// Combine DN & Pallet for BT
    /// </summary>
    public interface ICombineDNPalletforBT
    {
        /// <summary>
        /// 获取Delivery表相关信息
        /// </summary>
        IList<S_RowData_DN> GetDNList();
        /// <summary>
        /// 获取Pallet表相关信息GetDeliveryPalletListByDN
        /// </summary>
        /// <param name="DN">DN</param>
        IList<SelectInfoDef> GetPalletList(string DN);
        /// <summary>
        /// 获取product表相关信息 GetProductListByDeliveryNoAndPalletNo
        /// </summary>
        /// <param name="PalletNo">PalletNo</param>
        /// <param name="DN">DN</param>
        IList<ProductModel> GetProductList(string PalletNo, string DN);
        /// <summary>
        /// CheckProductAndDN
        /// </summary>
        /// <param name="custSN">customer SN</param>
        /// <param name="DN">DN</param>
        string CheckProductAndDN(string custSN, string DN);
       
        /// <summary>
        /// AssignAll
        /// </summary>
        /// <param name="custSN">customer SN</param>
        /// <param name="line">line</param>
        /// <param name="code">code</param>
        /// <param name="floor">floor</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param>
        string AssignAll(string custSN, string line, string code, string floor,
                                                    string editor, string station, string customer, string DN); 
        /// <summary>
        /// GetTemplateName
        /// </summary>
        /// <param name="DN">DN</param>
        string GetTemplateName(string DN);
         /// <summary>
        /// check product
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="custSN">customer SN</param>
        int CheckProduct(string line, string editor, string station, string customer, string custSN);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameList"></param>
        /// <returns></returns>
        ArrayList GetSysSettingList(IList<string> nameList);

        /// <summary>
        /// AssignAllNew
        /// </summary>
        /// <param name="custSN">customer SN</param>
        /// <param name="line">line</param>
        /// <param name="code">code</param>
        /// <param name="floor">floor</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param>
        ArrayList AssignAllNew(string custSN, string line, string code, string floor,
                                                    string editor, string station, string customer, string DN); 



    }
}
