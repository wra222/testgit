using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using IMES.FisObject.Common.Part.PartStrategyConfig;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// 构造PartStrategy对象的工厂类
    /// </summary>
    public static class PartStrategyFactory
    {
        /// <summary>
        /// key: FisObjectType.Name, Value: Repository Object
        /// </summary>
        private static Dictionary<string, object> _strategies = new Dictionary<string, object>();

        private static object syncRoot = new object();

        private const string DEFAULT_ASSEMBLY_PATH = "IMES.FisObject.PartStrategy.CommonStrategy";
        private const string DEFAULT_FULE_TYPE_NAME = "IMES.FisObject.PartStrategy.CommonStrategy.CommonStrategy";

        
        ///<summary>
        /// 给据customer和parttype获取PartStrategy对象
        ///</summary>
        ///<param name="partType">partType</param>
        ///<param name="customer">customer</param>
        ///<returns></returns>
        public static IPartStrategy GetPartStrategy(string partType, string customer)
        {
            IPartStrategy strategy = null;
            IPartRepository rep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            PartType pt = rep.GetPartType(partType);

            IList<PartCheck> settingData = RepositoryFactory.GetInstance().GetRepository<IPartRepository>().GetPartCheck(partType, customer);
//            PartCheck setting = null;
//            if (settingData != null && settingData.Count > 0)
//            {
//                setting = settingData.First();
//            }

            lock (syncRoot)
            {
                if (!_strategies.ContainsKey(pt.PartTypeGroup))
                {

                    PartStrategySection cfg =
                        (PartStrategySection) ConfigurationManager.GetSection(
                                                  PartStrategySettingConstant.PartStrategyMappingsConfigurationSectionName);

                    PartStrategyMappingElement element = cfg.PartStrategyMappings[pt.PartTypeGroup];
                    string assPath;
                    string fullTypeName;
                    
                    if (element != null)
                    {
                        assPath = element.AssemblyName;
                        fullTypeName = element.FullTypeName;
                    }
                    else
                    {
                        assPath = DEFAULT_ASSEMBLY_PATH;
                        fullTypeName = DEFAULT_FULE_TYPE_NAME;
                    }
                    
                    Assembly ass = Assembly.Load(assPath);
                    Type tp = ass.GetType(fullTypeName, true);
//todo:                    strategy = Activator.CreateInstance(tp, new object[] {settingData}) as IPartStrategy;
                    strategy = Activator.CreateInstance(tp, new object[] { settingData }) as IPartStrategy;
                    if (strategy != null)
                    {
                        _strategies.Add(pt.PartTypeGroup, strategy);
                    }
                }
                strategy = (IPartStrategy)_strategies[pt.PartTypeGroup];
            }

            if (strategy != null)
            {
                strategy.SetPartCheckSetting(settingData);
            }
            return strategy;
        }
    }
}
