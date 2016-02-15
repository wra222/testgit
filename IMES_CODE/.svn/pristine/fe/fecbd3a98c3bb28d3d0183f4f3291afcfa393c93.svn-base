/*
 * INVENTEC corporation ©2012 all rights reserved. 
 * Description:Interface for RCTOWeight Page
 *             
 * UI:CI-MES12-SPEC-PAK-UI RCTO Weight
 * UC:CI-MES12-SPEC-PAK-UC RCTO Weight
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
 * 2012-09-08  itc202017             (Reference Ebook SourceCode) Create
 * Known issues:
 * TODO:
*/
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using IMES.Station.Interface.CommonIntf;
using IMES.DataModel;

namespace IMES.Station.Interface.StationIntf
{
    /// <summary>
    ///  UC 具体业务：  本站站号：85
    ///                1. Unit 称重；
    ///                2. 列印Unit Weight Label；
    ///                3. 上传数据至SAP
    /// </summary>
    public interface IRCTOWeight
    {

        #region RCTOWeight

        /// <summary>
        /// 获取Model的标准重量
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>标准重量</returns>
        decimal GetModelWeight(string model);

        /// <summary>
        /// 更新Model的标准重量
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="weight">UnitWeight</param>
        /// <param name="editor">Editor</param>
        void SetModelWeight(string model, decimal weight, string editor);
        #endregion
    }
}
