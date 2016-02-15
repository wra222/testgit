/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: FamilyInfo Maintain
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012/05/18   kaisheng           (Reference Ebook SourceCode) Create
 * * issue:
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
   public interface IFamilyInfo
    {
        /// <summary>
        /// 查询所有的FamilyInfo
        /// </summary>
        /// <returns></returns>
        IList<FamilyInfoDef > GetAllFamilyInfo();
        /// <summary>
        /// 添加FamilyInfo
        /// 当添加的记录在其他存在的记录中重复时,抛出异常
        /// </summary>
        /// <param name="FamilyInfoItem"></param>
        /// <returns>返回被添加数据的ID</returns>
        int AddSelectedFamilyInfo(FamilyInfoDef  FamilyInfoItem);
        /// <summary>
        /// 删除选中的FamilyInfo
        /// </summary>
        /// <param name="FamilyInfoItem"></param>
        void DeleteSelectedFamilyInfo(FamilyInfoDef  FamilyInfoItem);
        /// <summary>
        /// 更新FamilyInfo
        /// 当添加的记录在其他存在的记录中重复时,抛出异常
        /// 当所要更新的记录在数据库中不存在的时,抛出异常
        /// </summary>
        /// <param name="FamilyInfoItem"></param>
        void UpdateSelectedFamilyInfo(FamilyInfoDef  FamilyInfoItem);

        /// <summary>
        /// Get ConstValueType by Type List
        /// </summary>
        /// <param name="Type"></param>
        IList<ConstValueTypeInfo> GetFamilyInfoNameList(string Type);
    }
}
