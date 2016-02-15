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
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;


namespace IMES.Station.Interface.StationIntf
{
    [Serializable]
    /// <summary>
    /// Row data define of CombineCOAandDN page
    /// </summary>
    public struct S_RowData_COAandDN
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
    [Serializable]
    /// <summary>
    /// Row data define of product for CombineCOAandDN page
    /// </summary>
    public struct S_RowData_Product
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
        /// isBT
        /// </summary>
        public string isBT; 
        /// <summary>
        /// isCDSI
        /// </summary>
        public string isCDSI;
        /// <summary>
        /// isFactoryPo
        /// </summary>
        public string isFactoryPo;
        /// <summary>
        /// isWin8
        /// </summary>
        public string isWin8;
        /// <summary>
        /// DN
        /// </summary>
        public string DN;
        /// <summary>
        /// isBSaM
        /// </summary>
        public string isBSaM;
        /// <summary>
        /// isJapan
        /// </summary>
        public string isJapan;
    };

    /// <summary>
    /// Combine COA and DN
    /// </summary>
    public interface ICombineCOAandDN
    {
        /// <summary>
        /// 获取Delivery表相关信息
        /// </summary>
        IList<S_RowData_COAandDN> GetDNList();
        /// <summary>
        /// 获取Product表相关信息
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="customerSN">customer SN</param>
        S_RowData_Product GetProduct(string line, string editor, string station, string customer, string customerSN);
        /// <summary>
        /// 取ModelBOM 中Model 直接下阶中有BomNodeType 为'P1' 的Part
        /// </summary>
        /// <param name="custSN">custSN</param>
        string CheckBOM(string custSN);
        /// <summary>
        /// Product_Part 中看是否有绑定COA – 存在BomNodeType = 'P1' ，IMES_GetData..Part.Descr LIKE 'COA%' 的Part
        /// </summary>
        /// <param name="custSN">custSN</param>
        string CheckPart(string custSN);
        /// <summary>
        /// 使用Code = @CustomerSN and Type =  'SN' 或者Code = @DeliveryNo and Type =  'DN'查询InternalCOA 表
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="deliveryNo">DeliveryNo</param>
        bool CheckInternalCOA(string custSN, string deliveryNo);
        /// <summary>
        /// check coa
        /// </summary>
        /// <param name="coaSN">coaSN</param> 
        /// <param name="partNO">partNO</param>
        int CheckCOA(string coaSN, string partNO);
        /// <summary>
        /// Product.DeliveryNo – Delivery No (from UI)
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="deliveryNo">DeliveryNo</param> 
        void UpdateProduct(string custSN, string deliveryNo);
        /// <summary>
        /// IMES_PAK..Delivery.Status = '87'
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param>
        /// <param name="custSN">custSN</param>
        /// <param name="coaSN">coaSN</param>
        string UpdateDeliveryStatus(string line, string editor, string station, string customer, string DN, string custSN, string coaSN);
        /// <summary>
        /// IMES_PAK..Delivery.Status = '87'
        /// </summary>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        /// <param name="DN">DN</param>
        /// <param name="custSN">custSN</param>
        /// <param name="coaSN">coaSN</param>
        /// <param name="printItems"></param>  
        ArrayList UpdateDeliveryStatusAndPrint(string line, string editor, string station, string customer, string DN, string custSN, string coaSN, IList<PrintItem> printItems);
        /// <summary>
        /// Insert Product_Part - Combine COA 
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="coaSN">coaSN</param>
        /// <param name="editor">editor</param>
        void BindPart(string custSN, string coaSN, string editor);
         /// <summary>
        /// Generate Pizza
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line">line</param>
        /// <param name="editor">editor</param>
        /// <param name="station">station</param>
        /// <param name="customer">customer</param>
        string GeneratePizza(string custSN, string line, string editor, string station, string customer);
        /// <summary>
        /// 印第一个pizza标签
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> PrintPizzaLabel(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems);
        /// <summary>
        /// 印第一个pizza标签
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
        IList<PrintItem> RePrint(string reason, string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems);
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
        IList<PrintItem> RePrintCOO(string reason, string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems);
         /// <summary>
        /// Product_Part 中看是否有绑定COA相等
        /// </summary>
        /// <param name="custSN">custSN</param>
        /// <param name="coaSN">coaSN</param> 
        int CheckPartCoa(string custSN, string coaSN);
        /// <summary>
        /// GetModel
        /// </summary>
        /// <param name="DN">DN</param>
        string GetModel(string DN);
        /// <summary>
        /// 获取Product表相关信息
        /// </summary>
        /// <param name="customerSN">customer SN</param>
        S_RowData_Product GetProductOnly(string customerSN);
        /// <summary>
        /// 获取DN表相关信息
        /// </summary>
        /// <param name="aDN">aDN</param>
        S_RowData_COAandDN GetADN(string aDN);
        /// <summary>
        /// 印第一个pizza标签
        /// </summary>
        /// <param name="custSN">CustomerSN</param>
        /// <param name="line"></param>
        /// <param name="editor"></param>
        /// <param name="station"></param>
        /// <param name="customer"></param>  
        /// <param name="printItems"></param>
        /// <returns>Print Items</returns>
        IList<PrintItem> PrintPizzaLabelFinal(string custSN, string line, string editor, string station, string customer, IList<PrintItem> printItems);
        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="custSN"></param>
        void Cancel(string custSN);
        /// <summary>
        /// 获取DN表相关信息
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="pono"></param> 
        IList<S_RowData_COAandDN> GetDNListQuick(string model, string pono);
        /// <summary>
        /// 无意义
        /// </summary>
        string Start();
    }
   
}
