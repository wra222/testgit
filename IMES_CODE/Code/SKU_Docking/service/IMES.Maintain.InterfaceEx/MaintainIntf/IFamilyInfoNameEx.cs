using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IFamilyInfoNameEx
    {
        /// <summary>
        /// 查询所有的FamilyInfoName
        /// </summary>
        /// <returns></returns>
        IList<FamilyInfoNameEx> GetFamilyInfoName();
        /// <summary>
        /// 添加FamilyInfoName
        /// 当添加的记录在其他存在的记录中重复时,抛出异常
        /// </summary>
        /// <param name="FamilyInfoItem"></param>
        /// <returns>返回被添加数据的ID</returns>
        void AddFamilyInfoName(FamilyInfoNameEx Item);
        /// <summary>
        /// 更新选中的FamilyInfoName
        /// </summary>
        /// <param name="FamilyInfoItem"></param>
        void UpdateSelectedFamilyInfoName(FamilyInfoNameEx Item, string nameKey);
        /// <summary>
        ///  删除FamilyInfoName
        /// 当添加的记录在其他存在的记录中重复时,抛出异常
        /// 当所要更新的记录在数据库中不存在的时,抛出异常
        /// </summary>
        /// <param name="FamilyInfoItem"></param>
        void DeleteSelectedFamilyInfoName(string nameKey);

    }
}
