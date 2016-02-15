/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:IMES service interface for LabelLightGuide Page
 *             
 * UI:CI-MES12-SPEC-FA-UI Label Light Guide.docx –2011/10/26 
 * UC:CI-MES12-SPEC-FA-UC Label Light Guide.docx –2011/10/26            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2011-11-19  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
*/

using System.Collections.Generic;
using System.Data;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Change Key Parts
    /// </summary>
    public interface ILabelLightGuide
    {
        /// <summary>
        /// 根据ProductID获取product相关信息
        /// </summary>
        /// <param name="prodId">ProductID</param>
        /// <param name="line">Product Line</param>
        /// <param name="editor">Operator</param>
        /// <param name="station">Station</param>
        /// <param name="customer">Customer</param>
        /// <param name="bQuery">Only query data or not</param>
        IMES.DataModel.ProductInfo GetProductInfo(string prodId, string line, string editor, string station, string customer, bool bQuery);
        
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

        /// <summary>
        /// 根据model和Light Code获取LightGuide列表
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="code">code</param>
        IList<WipBuffer> getBomData(string model, string code);

        /// <summary>
        /// 根据hostname获取Comm设置列表
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <param name="editor">Operator</param>
        IList<COMSettingInfo> getCommSetting(string hostname, string editor);
    }
}
