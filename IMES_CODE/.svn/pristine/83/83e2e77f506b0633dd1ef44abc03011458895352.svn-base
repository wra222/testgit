using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IRepairInfoMaintain
    {
        /// <summary>
        /// 根据用户所选type查询符合条件的记录
        /// </summary>
        /// <param name="type">用户所选的type</param>
        /// <returns></returns>
        IList<RepairInfoMaintainDef> GetRepairInfoByCondition(string type);
        /// <summary>
        /// 根据用户的操作添加或更新一条记录
        /// </summary>
        /// <param name="def"></param>
        int AddOrUpdateRepairInfoMaintain(RepairInfoMaintainDef def);
        /// <summary>
        /// 删除用户选中的记录
        /// </summary>
        /// <param name="def">用户所要删除的记录</param>
        void RemoveRepairInfoMaintainItem(RepairInfoMaintainDef def); 
    }
}
