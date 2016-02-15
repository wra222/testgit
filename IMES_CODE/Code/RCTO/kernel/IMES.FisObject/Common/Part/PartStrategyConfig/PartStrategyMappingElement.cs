using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Part.PartStrategyConfig
{
    /// <summary>
    /// 配置文件中自定义的Element类
    /// </summary>
    public class PartStrategyMappingElement : ConfigurationElement
    {
        [ConfigurationProperty(PartStrategySettingConstant.PartTypeAttributeName,
            IsKey = true, IsRequired = true)]
        public string PartTypeName
        {
            get
            {
                return (string)this[PartStrategySettingConstant.PartTypeAttributeName];
            }
            set
            {
                this[PartStrategySettingConstant.PartTypeAttributeName] = value;
            }
        }

        [ConfigurationProperty(PartStrategySettingConstant.AssemblyNameAttributeName,
            IsRequired = true)]
        public string AssemblyName
        {
            get
            {
                return (string)this[PartStrategySettingConstant.AssemblyNameAttributeName];
            }
            set
            {
                this[PartStrategySettingConstant.AssemblyNameAttributeName] = value;
            }
        }

        [ConfigurationProperty(PartStrategySettingConstant.FullTypeNameAttributeName,
            IsRequired = true)]
        public string FullTypeName
        {
            get
            {
                return (string)this[PartStrategySettingConstant.FullTypeNameAttributeName];
            }
            set
            {
                this[PartStrategySettingConstant.FullTypeNameAttributeName] = value;
            }
        }
    }
}
