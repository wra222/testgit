using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface ILightSta
    {
        /// <summary>
        /// 查询所有的light station记录.
        /// </summary>
        /// <returns></returns>
        IList<LightStDef> GetAllLightStation();
        /// <summary>
        /// 删除选中的一条lightstation记录.
        /// </summary>
        /// <param name="id"></param>
        void DelelteLightStationItem(int id);
        /// <summary>
        /// 添加一条lightstation记录.
        /// </summary>
        /// <param name="def"></param>
        int AddLightStationItem(LightStDef def);
        /// <summary>
        /// 更新一条lightstation记录
        /// </summary>
        /// <param name="def">要更新的数据</param>
        /// <param name="id"></param>
        void UpdateLightStationItem(LightStDef def,int id);
    }
}
