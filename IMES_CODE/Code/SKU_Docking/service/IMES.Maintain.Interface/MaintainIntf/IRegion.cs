// created by itc207024 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IRegion
    {
        /// <summary>
        /// 获得Region列表
        /// </summary>
        /// <returns></returns>
        IList<RegionInfo> GetRegionList();

        /// <summary>
        /// 添加新纪录
        /// </summary>
        /// <returns></returns>
        void AddRegion(RegionInfo region);

        /// <summary>
        /// 更新纪录
        /// </summary>
        /// <returns></returns>
        void UpdateRegion(RegionInfo region, string regionName);

        /// <summary>
        /// 删除纪录
        /// </summary>
        /// <returns></returns>
        void DeleteRegion(string region);

        /// <summary>
        /// 判断Region是否已经存在
        /// </summary>
        /// <returns></returns>
        bool IFRegionIsExists(string region);

        /// <summary>
        /// 判断Region是否已经被使用
        /// </summary>
        /// <returns></returns>
        bool IFRegionInUSE(string region);

        #region new Region

        IList<RegionInfo> GetRegionList(RegionInfo condition);

        void InsertRegion(RegionInfo item);

        void UpdateRegion(RegionInfo condition, RegionInfo item);

        void DeleteRegion(RegionInfo condition);

        #endregion
 
    }
}
