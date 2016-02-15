/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for ITCND Check QC Hold Setting Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/5/10 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/5/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-10   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 
*/

using System;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IITCNDCheckQCHoldSetting接口
    /// </summary>
    public interface IITCNDCheckQCHoldSetting
    {
        /// <summary>
        /// 取得所有ITCNDCheckQCHold记录(按Code栏位排序)
        /// </summary>
        /// <returns>ITCNDCheckQCHold记录列表</returns>
        IList<ITCNDCheckQCHoldDef> GetITCNDCheckQCHoldList();

        /// <summary>
        /// 取得15天前ITCNDCheckQCHold记录(按Code栏位排序)
        /// </summary>
        /// <returns>ITCNDCheckQCHold记录列表(15天)</returns>
        IList<ITCNDCheckQCHoldDef> GetITCNDCheckQCHoldListByDay();

        /// <summary>
        /// 保存一条ITCNDCheckQCHold的记录数据(Add), 若记录中不存在与传入Code相同名称的Code，则提示业务异常
        /// </summary>
        /// <param name="obj">ITCNDCheckQCHoldDef结构</param>
        void AddITCNDCheckQCHold(ITCNDCheckQCHoldDef obj);


        /// <summary>
        /// 删除一条ITCNDCheckQCHold的记录数据
        /// </summary>
        /// <param name="obj">ITCNDCheckQCHoldDef结构</param>
        void DeleteITCNDCheckQCHold(ITCNDCheckQCHoldDef obj);


        /// <summary>
        /// 更新一条ITCNDCheckQCHold的记录数据(update), 若Code与存在记录的Code的名称相同，则提示业务异常
        /// </summary>
        /// <param name="obj">更新ITCNDCheckQCHoldDef结构</param>
        /// <param name="oldCode">修改前Code</param>
        void UpdateITCNDCheckQCHold(ITCNDCheckQCHoldDef obj, string oldCode);

        /// <summary>
        /// 更新一条ITCNDCheckQCHold的记录数据(update)
        /// </summary>
        /// <param name="obj">更新ITCNDCheckQCHoldDef结构</param>
        void UpdateITCNDCheckQCHold(ITCNDCheckQCHoldDef obj);

        /// <summary>
        /// 判斷該ITCNDCheckQCHold記錄是否存在，存在回傳true，不存在回傳false
        /// </summary>
        /// <param name="obj">ITCNDCheckQCHoldDef结构</param>
        bool CheckExistITCNDCheckQCHold(ITCNDCheckQCHoldDef obj);

    }
}
