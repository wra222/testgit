/*
 * INVENTEC corporation ?2011 all rights reserved. 
 * Description:Interface for Const Value Maintain Page
 * UI:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1 
 * UC:CI-MES12-SPEC-PAK-DATA MAINTAIN(II).docx –2012/8/1              
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-8-6     Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
*/

using System;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IConstValueMaintain接口
    /// </summary>
    public interface IConstValueMaintain
    {
        /// <summary>
        /// 取得所有Type数据的list(按Type栏位排序)
        /// </summary>
        /// <returns></returns>
        IList<ConstValueInfo> GetConstValueTypeList();

        /// <summary>
        /// 根据Type取得对应的ConstValue数据的list(按Name栏位排序)
        /// </summary>
        /// <param name="type">过滤条件Type</param>
        /// <returns></returns>
        IList<ConstValueInfo> GetConstValueListByType(String type);

        /// <summary>
        /// 增加一条ConstValue的记录数据(update/insert)
        /// </summary>
        /// <param name="obj">增加ConstValueInfo结构</param>
        void AddConstValue(ConstValueInfo obj);

        /// <summary>
        /// 保存一条ConstValue的记录数据(update/insert)
        /// </summary>
        /// <param name="obj">更新ConstValueInfo结构</param>
        void SaveConstValue(ConstValueInfo obj);

        /// <summary>
        /// 删除一条ConstValue的记录数据
        /// </summary>
        /// <param name="obj">ConstValueInfo结构</param>
        void DeleteConstValue(ConstValueInfo obj);

        /// <summary>
        /// 删除ConstValue的记录数据By條件
        /// </summary>
        /// <param name="obj">ConstValueInfo结构</param>
        void DeleteConstValueByCondition(ConstValueInfo obj);

        void DeleteConstValue(IList<string> ids);

        /// <summary>
        /// 取得Type数据的Descrip(按Type栏位排序)
        /// </summary>
        /// <returns></returns>
        string GetConstValueDescriptionByType(string Type);
    }
}
