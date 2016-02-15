/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service interface for AFTMVS Page
 *             
 * UI:CI-MES12-SPEC-FA-UI AFT MVS.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC AFT MVS.docx –2011/10/25            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-10-28  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System.Collections.Generic;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Product QC信息获取与保存
    /// </summary>
    public interface IAFTMVS
    {
        /// <summary>
        /// 获取Product的Model和BOM信息
        /// </summary>
        /// <param name="input">ProductId/CustSN</param>
        /// <param name="line">Product Line</param>
        /// <param name="editor">Operator</param>
        /// <param name="station">Station</param>
        /// <param name="customer">Customer</param>
        /// <param name="model">(out)Model</param>
        IList<BomItemInfo> InputProductIDorCustSN(string input, string line, string editor, string station, string customer, out string prodID, out string model);

        /// <summary>
        /// 处理输入资产标签，返回结合的PartNo
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
        /// <param name="ast">AST Label</param>
        string InputASTLabel(string sesKey, string ast);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
        void Cancel(string sesKey);

        /// <summary>
        /// 获取当前Product的QC方法信息
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
         
        /*
        * Answer to: ITC-1360-0975
        * Description: Multi-language message in QC method.
        */
        string GetQCMethod(string sesKey, out string wc);

        /// <summary>
        /// 获取指定PdLine的QC统计信息
        /// </summary>
        /// <param name="pdLine">Pd Line</param>
        /// <param name="piaCnt">PIA Count</param>
        /// <param name="epiaCnt">EPIA Count</param>
        /// <param name="passCnt">Pass Count</param>
        void GetQCStatics(string pdLine, out int piaCnt, out int epiaCnt, out int passCnt);

        /// <summary>
        /// 获取ESOP文件名列表
        /// </summary>
        /// <param name="model">Model</param>
        IList<string> GetESOPList(string model);      
    }
}
