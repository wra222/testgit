using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.Part.PartStrategyConfig
{
    /// <summary>
    /// 配置文件中自定义的ConfigurationElementCollection
    /// </summary>
    public class PartStrategyMappingCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new PartStrategyMappingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PartStrategyMappingElement)element).PartTypeName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return PartStrategySettingConstant.ConfigurationElementName; }
        }

        public PartStrategyMappingElement this[int index]
        {
            get { return (PartStrategyMappingElement)this.BaseGet(index); }
            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new PartStrategyMappingElement this[string partTypeName]
        {
            get { return (PartStrategyMappingElement)this.BaseGet(partTypeName); }
        }

        public bool ContainsKey(string keyName)
        {
            bool result = false;
            object[] keys = this.BaseGetAllKeys();
            foreach (object key in keys)
            {
                if ((string)key == keyName)
                {
                    result = true;
                    break;

                }
            }
            return result;
        }
    }
}
