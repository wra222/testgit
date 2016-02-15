/*
 * INVENTEC corporation ©2013 all rights reserved. 
 * Description:IMES service interface for ESOPandAoiKbTest Page
 *             
 * UI:
 * UC: 
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
  * Known issues:
*/

using System.Collections.Generic;
using System.Data;
using IMES.DataModel;
using System.Collections;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    /// Change Key Parts
    /// </summary>
    public interface IESOPandAoiKbTest
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
        ArrayList GetProductInfo(string prodId, string line, string editor, string station, string customer, bool bQuery);
        
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="custsn">custsn</param>
        /// <param name="defectList">defectList</param>
        void Save(string custsn, IList<string> defectList);

        /// <summary>
        /// 取消工作流
        /// </summary>
        /// <param name="custsn">SessionKey(ProdId)</param>
        void Cancel(string custsn);

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
        void AOICallBack(string sn, string editor, string station,
                                        string line, string customer, string result, string errorCode, string errorDesc);

        /// <summary>
        /// 处理输入资产标签，返回结合的PartNo
        /// </summary>
        /// <param name="sesKey">SessionKey(ProdId)</param>
        /// <param name="ast">AST Label</param>
        string InputASTLabel(string sesKey, string ast);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="checkValue"></param>
        /// <returns></returns>
        MatchedPartOrCheckItem TryPartMatchCheck(string sessionKey, string checkValue);

    }
}
