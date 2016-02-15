using System;
using System.Text;
using IMES.DataModel;
using System.Data;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{

    public interface ISysSetting
    {
        /// <summary>
        /// GetSysSettingList
        /// </summary>
        /// <returns></returns>
        IList<SysSettingInfo> GetSysSettingListByCondition(SysSettingInfo sysSettingCondition);

        /// <summary>
        /// AddSysSettingInfo
        /// </summary>
        /// <param name="item"></param>
        void AddSysSettingInfo(SysSettingInfo item);

        /// <summary>
        /// UpdateSysSettingInfo
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateSysSettingInfo(SysSettingInfo setValue, SysSettingInfo condition);

        /// <summary>
        /// DeleteSysSettingInfo
        /// </summary>
        /// <param name="id"></param>
        void DeleteSysSettingInfo(int id);
    }
}
