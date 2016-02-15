using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IOfflineLabelSetting
    {
        /// <summary>
        /// 获取所有OfflineLabelSetting数据
        /// </summary>
        /// <returns></returns>
        IList<OfflineLableSettingDef> getAllOfflineLabelSetting();

        /// <summary>
        /// 根据指定条件获取OfflineLabelSetting数据
        /// </summary>
        /// <returns></returns>
        IList<OfflineLableSettingDef> getOfflineLabelSetting(string FileName);

        /// <summary>
        /// 添加一条OfflineLabelSetting数据
        /// </summary>
        /// <param name="obj"></param>
        void addOfflineLabelSetting(OfflineLableSettingDef obj);

        /// <summary>
        /// 修改OfflineLabelSetting数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fileName"></param>
        void updateOfflineLabelSetting(OfflineLableSettingDef obj, String oldFileName);

        /// <summary>
        /// 根据指定条件，删除OfflineLabelSetting数据
        /// </summary>
        /// <param name="obj"></param>
        void deleteOfflineLabelSetting(OfflineLableSettingDef obj);



    }
}
