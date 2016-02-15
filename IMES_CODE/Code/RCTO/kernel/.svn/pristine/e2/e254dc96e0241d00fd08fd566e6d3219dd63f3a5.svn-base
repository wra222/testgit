using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Part.PartStrategyConfig
{
    /// <summary>
    /// PartStrategySection配置文件中自定义Section
    /// </summary>
    public class PartStrategySection : ConfigurationSection
    {
        [ConfigurationProperty(PartStrategySettingConstant.ConfigurationPropertyName, IsDefaultCollection = true)]
        public PartStrategyMappingCollection PartStrategyMappings
        {
            get { return (PartStrategyMappingCollection)base[PartStrategySettingConstant.ConfigurationPropertyName]; }
        }
    }
}
