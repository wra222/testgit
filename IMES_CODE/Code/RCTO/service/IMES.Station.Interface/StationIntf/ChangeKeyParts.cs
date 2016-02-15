/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service interface for ChangeKeyParts Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Change Key Parts.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Change Key Parts.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-11  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System.Collections.Generic;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Change Key Parts
    /// </summary>
    public interface IChangeKeyParts
    {
        /// <summary>
        /// 获取Product的信息和Part信息
        /// </summary>
        /// <param name="input">ProductId/CustSN</param>
        /// <param name="kpType">Key Part Type</param>
        /// <param name="line">Product Line</param>
        /// <param name="editor">Operator</param>
        /// <param name="station">Station</param>
        /// <param name="customer">Customer</param>
        IList<BomItemInfo> GetTableData(string input, string kpType, string line, string editor, string station, string customer, out ProductInfo info, out string retWC);

        /// <summary>
        /// 刷入替换物料后的处理
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
        /// <param name="checkValue">checkValue</param>
        MatchedPartOrCheckItem TryPartMatchCheck(string sesKey, string checkValue);

        /// <summary>
        /// Clear CT already input
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
        void ClearCT(string sesKey);
        
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
        void Save(string sesKey);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
        void Cancel(string sesKey);
    }
}
