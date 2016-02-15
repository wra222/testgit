/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for MB Combine CPU
 * UI:CI-MES12-SPEC-SA-UI MB Combine CPU.docx –2011/12/9 
 * UC:CI-MES12-SPEC-SA-UC MB Combine CPU.docx –2011/12/9            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-1-4   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// [MB Combine CPU]/[Product Combine CPU] 实现如下功能：
    /// 建立主板，Product和CPU Vender SN 的绑定关系
    /// </summary>
    public interface IMBCombineCPU
    {

        /// <summary>
        /// input mbsn ,SFC 
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="MB_SNo"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void MBCombineInputMBSN(
        string pdLine,
        string MB_SNo,
        string editor, string stationId, string customerId);

        /// <summary>
        /// 1.1	UC-PCA-MCC-01 MB Combine CPU
        /// 建立主板和CPU Vender 的绑定关系
        /// </summary>
        /// <param name="MB_SNo">MB SNo</param>
        /// <param name="CPUVendorSN">CPU Vendor SN</param>
        string  CombineCPU(
            string MB_SNo,
            string CPUVendorSN);

        /// <summary>
        /// input product ID, SFC
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="prodID"></param>
        /// <param name="editor"></param>
        /// <param name="stationId"></param>
        /// <param name="customerId"></param>
        void ProductCombineInputProdID(string pdLine,
            string prodID,
            string editor, string stationId, string customerId);

        /// <summary>
        /// 1.2	UC-PCA-MCC-02 Product Combine CPU
        /// 建立Product和CPU Vender 的绑定关系
        /// </summary>
        /// <param name="prodID">Product Id</param>
        /// <param name="CPUVendorSN">CPU Vendor SN</param>
        string  ProductCombineCPU(
        string prodID,
        string CPUVendorSN);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void CancelMB(string sessionKey);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sessionKey"></param>
        void CancelProduct(string sessionKey);
    }
}
