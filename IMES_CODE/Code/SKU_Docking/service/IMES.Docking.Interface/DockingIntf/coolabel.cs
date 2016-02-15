/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Web method for CombineCOAandDN Page            
 * CI-MES12-SPEC-PAK Combine COA and DN.docx
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-12-20  itc207003              Create
 * Known issues:
*/

using System;
using System.Collections.Generic;
using IMES.DataModel;
using System.Collections;


namespace IMES.Docking.Interface.DockingIntf
{
    [Serializable]
    /// <summary>
    /// Row data define of CombineCOAandDN page
    /// </summary>
    public struct S_CooLabel
    {
        /// <summary>
        /// Model
        /// </summary>
        public string Model;
        /// <summary>
        /// ProductID
        /// </summary>
        public string ProductID;
        /// <summary>
        /// CustomerSN
        /// </summary>
        public string CustomerSN;
        /// <summary>
        /// PalletNo
        /// </summary>
        public string PalletNo;
        /// <summary>
        /// CartonSN
        /// </summary>
        public string CartonSN;
        /// <summary>
        /// Mo 
        /// </summary>
        public string Mo;
        /// <summary>
        /// Total
        /// </summary>
        public string Total;
        /// <summary>
        /// Pass
        /// </summary>
        public string Pass;
        /// <summary>
        /// IsJapan
        /// </summary>
        public string IsJapan;
        /// <summary>
        /// IsCombineDN
        /// </summary>
        public string IsCombineDN;
    };
    
    /// <summary>
    /// Coo Lable
    /// </summary>
    public interface ICooLabel
    {
         /// <summary>
        /// 得DN
        /// </summary>
        /// <param name="mode">mode</param>
        /// <param name="status">status</param>
        /// <returns></returns>
        IList<DNForUI> GetDeliveryListByModel(string mode, string status);
       
        /// <summary>
        /// 判断是否是日本
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="model">model</param>
        /// <param name="valuePrefix">valuePrefix</param>
        /// <returns></returns>
        bool ISJapan(string name, string model, string valuePrefix);
       
        /// <summary>
        /// 得product
        /// </summary>
        /// <param name="customerSn">customerSn</customerSn>
        /// <param name="station">station</param> 
        /// <returns></returns>
        S_CooLabel GetProductBySN(string customerSn, string station);
        
        /// <summary>
        /// 获取Product表相关信息
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSN">customer SN</param>
        /// <param name="prod">prod</param> 
        S_CooLabel GetProduct(string editor, string station, string customer, string customerSN, string prod);
        
        /// <summary>
        /// 得product
        /// </summary>
        /// <param name="prod">name</param>
        /// <param name="station">station</param>  
        /// <returns></returns>
        S_CooLabel GetProductByProd(string prod, string station);
        
        /// <summary>
        /// 得QTY
        /// </summary>
        /// <param name="deliveryNo">deliveryNo</param>
        /// <returns></returns>
        int GetQTY(string deliveryNo);
        
        /// <summary>
        /// 更新ProductStatus
        /// </summary>
        /// <param name="station">station</param>
        /// <param name="status">status</param>
        /// <param name="editor">editor</param>
        /// <param name="prod">prod</param>
        /// <returns></returns>
        void UpdateProStatus(string station, string status, string editor, string prod);

         /// <summary>
        /// 更新ProductStatus
        /// </summary>
        /// <param name="station">station</param>
        /// <param name="editor">editor</param> 
        /// <param name="prod">prod</param>
        /// <returns></returns>
        void InsertProLog(string station, string editor, string prod);
       
 
        /// <summary>
        /// 更新RecordCKK
        /// </summary>
        /// <param name="editor">editor</param> 
        /// <param name="prod">prod</param>
        /// <param name="CKK">CKK</param> 
        /// <returns></returns>
        void RecordCKK(string editor, string prod, string CKK);

        /// <summary>
        /// 更新RecordCKK
        /// </summary>
        /// <param name="customerSn">customerSn</param>
        /// <returns></returns>
        string DoPAQC(string customerSn);

       /// <summary>
        /// 获取CombineDN表相关信息
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSn">customerSn</param>
        /// <param name="prod">prod</param> 
        /// <param name="DN">DN</param> 
        /// <param name="IsChk">IsChk</param> 
        string CombineDN(string editor, string station, string customer, string customerSn, string prod, string DN, string IsChk);
		string CombineDN(string editor, string station, string customer, string customerSn, string prod, string DN, string IsChk, string IsBTChk);
         /// <summary>
        /// 印标签
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> PrintCOOLabel(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems);
        /// <summary>
        /// 印RePrint标签
        /// </summary>
        /// <param name="reason">reason</param>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> RePrintCOO(string reason, string custSN, string line, string editor, string station, string customer, string pcode, IList<PrintItem> printItems);
        /// <summary>
        /// 判断是否更新drop
        /// </summary>
        /// <param name="DN">DN</param>
        /// <returns></returns>
        bool ISDNChange(string DN); 
         /// <summary>
        /// 获取Product表相关信息
        /// </summary>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSN">customer SN</param>
        /// <param name="prod">prod</param> 
        S_CooLabel GetProductOnly(string editor, string station, string customer, string customerSN, string prod);
    }
}
