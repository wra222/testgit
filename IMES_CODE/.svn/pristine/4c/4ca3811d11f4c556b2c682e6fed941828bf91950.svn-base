using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.Infrastructure.Utility.RuleSets
{
    /// <summary>
    /// RuleSet管理器的工厂类
    /// </summary>
    public class RuleSetManagerFactory
    {
        public static IRulesetManager CreateRuleSetManager(object args)
        {
            if (args == null || (args as string) == null)
                return new FileRuleSetManager();

            string remotingUrl = args as string;
            return new RemotingRuleSetManager(remotingUrl);
        }

        public static IRulesetManager CreateFileRuleSetManager()
        {
            return new FileRuleSetManager();
        }

        public static IRulesetManager CreateDBRuleSetManager()
        {
            return null;
        }
    }
}
