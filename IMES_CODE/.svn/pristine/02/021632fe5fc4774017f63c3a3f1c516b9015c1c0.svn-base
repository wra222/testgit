using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;

namespace IMES.Infrastructure.Utility.RuleSets
{
    /// <summary>
    /// RuleSet的管理接口
    /// </summary>
    public interface IRulesetManager
    {
        /// <summary>
        /// 获取一个指定的RuleSet
        /// </summary>
        /// <param name="site">流程所在Site</param>
        /// <param name="station">流程所在Station</param>
        /// <param name="name">RuleSetName</param>
        /// <returns>RuleSet</returns>
        /// <remarks></remarks>
        RuleSet getRuleSet(string site, string station, string name);
        RuleDefinitions getRuleDef(string site, string station, string name);
        RuleSet getRuleSetDesign(string site, string station, string name);

        /// <summary>
        /// 保存一个RuleSet
        /// </summary>
        /// <param name="site">流程所在Site</param>
        /// <param name="station">流程所在Station</param>
        /// <param name="name">RuleSet Name</param>
        /// <param name="ruleSet">RuleSet内容</param>
        /// <remarks></remarks>
        void saveRuleSet(string site, string station, string name, RuleSet ruleSet);
        void saveRuleDef(string site, string station, string name, RuleDefinitions ruleDef);
        void saveRuleSetDesign(string site, string station, string name, RuleSet ruleSet);
    }
}
