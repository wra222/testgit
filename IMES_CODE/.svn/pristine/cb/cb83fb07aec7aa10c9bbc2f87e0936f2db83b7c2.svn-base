using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface LotSetting
    {
        /// <summary>
        /// 添加一条Lot Setting记录.
        /// </summary>
        /// <param name="?"></param>
        void AddLotSetting(LotSettingInfo item);
        /// <summary>
        /// 删除选中的Lot Setting记录.
        /// </summary>
        /// <param name="item"></param>
        void RemoveLotSetting(LotSettingInfo item);
        /// <summary>
        /// 得到所有的Lot Setting记录.
        /// </summary>
        /// <returns></returns>
        IList<LotSettingInfo> GetAllLotSettingItems();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cond"></param>
        void UpdateLotSetting(LotSettingInfo item, LotSettingInfo cond);

         /// <summary>
        /// 
        /// </summary>
        /// <param name="stage"></param>
        IList<string> GetLine(string stage);

    }
}