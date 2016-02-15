using System;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// IEnergyLabel接口
    /// </summary>
    public interface IEnergyLabel
    {
        IList<string> GetEnergyLabel_Family();

        IList<string> GetFamily();

        IList<string> GetChinaLevel();

        IList<EnergyLabelInfo> GetEnergyLabelByCondition(EnergyLabelInfo condition);

        void addeditEnergyLabel(EnergyLabelInfo condition, string userName);

        void DeleteEnergyLabel(int id);
    }
}
