// created by itc207024 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;

namespace IMES.Maintain.Interface.MaintainIntf
{
    public interface IModelProcess
    {
        /// <summary>
        /// 获得全部Rule Set列表
        /// </summary>
        /// <returns></returns>
        IList<RulesetInfoDataMaintain> GetProcessRuleSetList();

        /// <summary>
        /// 获得Model Info数据表中所有出现过的Name值
        /// </summary>
        /// <returns></returns>
        IList<string> GetModelInfoNameList();

        /// <summary>
        /// 根据Condtion获得Model Process规则列表
        /// </summary>
        /// <param name="conditionID">ProcessRuleSet表的ID</param>
        /// <returns>IList<ProcessRule></returns>
        IList<ProcessRule> GetRuleListByCondition(int conditionID);

        /// <summary>
        /// 优先级的交换
        /// </summary>
        /// <param name="highPriority">高优先级对象</param>
        /// <param name="lowPriority">低优先级对象</param>
        void ChangePriority(RulesetInfoDataMaintain highPriority, RulesetInfoDataMaintain lowPriority);

        /// <summary>
        /// 删除当前被选Rule Set及所有应用该Rule Set的所有Rule。
        /// </summary>
        /// <param name="ruleSetID">ProcessRuleSet表的ID</param>
        void DeleteProcessRuleSet(int ruleSetID);

        /// <summary>
        /// 添加一条ProcessRuleSet
        /// </summary>
        /// <param name="singleRuleSet">ProcessRuleset对象</param>
        string AddProcessRuleSet(RulesetInfoDataMaintain singleRuleSet);

        /// <summary>
        /// 修改一条ProcessRuleSet
        /// </summary>
        /// <param name="singleRuleSet">ProcessRuleset对象</param>
        void EditAddProcessRuleSet(RulesetInfoDataMaintain singleRuleSet);

        /// <summary>
        /// 添加一条ProcessRule
        /// </summary>
        /// <param name="rule">ProcessRule</param>
        string AddRule(ProcessRule rule);
        
        /// <summary>
        /// 修改一条ProcessRule
        /// </summary>
        /// <param name="rule">ProcessRule</param>
        void EditRule(ProcessRule rule);

        /// <summary>
        /// 删除当前被选Rule
        /// </summary>
        /// <param name="ruleID">ProcessRule的ID</param>
        void DeleteRule(int ruleID);

        /// <summary>
        /// 檢查並且刪除修改的ModelProcess
        /// </summary>
        /// <param name="ruleID">ProcessRule的ID</param>
        void CheckAdd_SaveModelProcess(int ruleID);

        #region "2011-4-25 New Interfaces"
            /// <summary>
            /// 根据Condtion和Process获得Model Process规则列表
            /// </summary>
            /// <param name="conditionID">ProcessRuleSet表的ID</param>
            /// <returns>IList<ProcessRule></returns>
            IList<ProcessRule> GetRuleListByConditionAndProcess(int conditionID, string processName);            
        #endregion

    }
}
