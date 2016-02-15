using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{

    public interface IFamilyMBCode
    {
        /// <summary>
        /// 添加一条ICombineKPSetting记录.
        /// </summary>
        /// <param name="?"></param>
        void AddCombineKPSetting(StationCheckInfo item);
        /// <summary>
        /// 删除选中的ICombineKPSetting记录.
        /// </summary>
        /// <param name="item"></param>
        void RemoveCombineKPSetting(StationCheckInfo item);
        /// <summary>
        /// 得到所有的ICombineKPSetting记录.
        /// </summary>
        /// <returns></returns>
        IList<StationCheckInfo> GetAllCombineKPSettingItems();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cond"></param>
        void UpdateCombineKPSetting(StationCheckInfo item, StationCheckInfo cond);
        IList<string> GetLine();
        IList<StationInfo> GetStation();
        IList<string> GetCheckType();
        IList<FamilyMbInfo> GetAllFamilyMbItems();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void RemoveFamilyMb(FamilyMbInfo item);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cond"></param>
        void UpdateFamilyMb(FamilyMbInfo item, FamilyMbInfo cond);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void AddFamilyMb(FamilyMbInfo item);
    }
}