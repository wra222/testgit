using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IWeightSetting
    {
        void RemoveWeightSettingItem(COMSettingDef def);

        int AddOrUpdateWeightSettingItem(COMSettingDef def);

        IList<COMSettingDef> GetAllWeightSettingItems();
    }
}
