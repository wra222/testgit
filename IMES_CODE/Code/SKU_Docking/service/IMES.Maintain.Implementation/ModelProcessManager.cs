/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  ICT input interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-3-17  itc-207024          Create 
 * Known issues: ITC-1361-0073 ITC-1361-0075
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Process;
using log4net;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure;

namespace IMES.Maintain.Implementation
{
    class ModelProcessManager : MarshalByRefObject, IModelProcess
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
        public IProcessRepository processRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();

        #region IModelProcess 成员

        public IList<RulesetInfoDataMaintain> GetProcessRuleSetList()
        {
            IList<ProcessRuleSet> result =  processRepository.GetAllProcessRuleset();
            IList<RulesetInfoDataMaintain> rulesetInfoList = new List<RulesetInfoDataMaintain>();
            foreach (ProcessRuleSet ruleset in result)
            {
                RulesetInfoDataMaintain rulesetInfo = new RulesetInfoDataMaintain();
                rulesetInfo.Cdt = ruleset.Cdt;
                rulesetInfo.Condition1 = ruleset.Condition1;
                rulesetInfo.Condition2 = ruleset.Condition2;
                rulesetInfo.Condition3 = ruleset.Condition3;
                rulesetInfo.Condition4 = ruleset.Condition4;
                rulesetInfo.Condition5 = ruleset.Condition5;
                rulesetInfo.Condition6 = ruleset.Condition6;
                rulesetInfo.Editor = ruleset.Editor;
                rulesetInfo.Id = ruleset.ID;
                rulesetInfo.Priority = ruleset.Priority;
                rulesetInfo.Udt = ruleset.Udt;
                rulesetInfoList.Add(rulesetInfo);
            }
            return rulesetInfoList;
        }

        public IList<string> GetModelInfoNameList()
        {
            return modelRepository.GetAllModelInfoName();
        }

        public IList<ProcessRule> GetRuleListByCondition(int conditionID)
        {
            IList<ProcessRule> processRuleList = processRepository.GetAllRuleByRuleSetID(conditionID);
            return processRuleList;
        }

        public void ChangePriority(RulesetInfoDataMaintain highPriority, RulesetInfoDataMaintain lowPriority)
        {
            highPriority.Priority = highPriority.Priority + 1;
            lowPriority.Priority = lowPriority.Priority - 1;
            UnitOfWork uow = new UnitOfWork();
            ProcessRuleSet ruleset1 = new ProcessRuleSet();
            ruleset1.Cdt = highPriority.Cdt;
            ruleset1.Condition1 = highPriority.Condition1;
            ruleset1.Condition2 = highPriority.Condition2;
            ruleset1.Condition3 = highPriority.Condition3;
            ruleset1.Condition4 = highPriority.Condition4;
            ruleset1.Condition5 = highPriority.Condition5;
            ruleset1.Condition6 = highPriority.Condition6;
            ruleset1.Editor = highPriority.Editor;
            ruleset1.ID = highPriority.Id;
            ruleset1.Priority = highPriority.Priority;
            ruleset1.Udt = highPriority.Udt;
            ProcessRuleSet ruleset2 = new ProcessRuleSet();
            ruleset2.Cdt = lowPriority.Cdt;
            ruleset2.Condition1 = lowPriority.Condition1;
            ruleset2.Condition2 = lowPriority.Condition2;
            ruleset2.Condition3 = lowPriority.Condition3;
            ruleset2.Condition4 = lowPriority.Condition4;
            ruleset2.Condition5 = lowPriority.Condition5;
            ruleset2.Condition6 = lowPriority.Condition6;
            ruleset2.Editor = lowPriority.Editor;
            ruleset2.ID = lowPriority.Id;
            ruleset2.Priority = lowPriority.Priority;
            ruleset2.Udt = lowPriority.Udt;
            //若有应用Rule Set List表格中的当前选项或其上一个选项的Rule存在，则删除Model_Process数据表中的所有记录。
            if ((processRepository.GetAllRuleByRuleSetID(highPriority.Id) != null && processRepository.GetAllRuleByRuleSetID(highPriority.Id).Count > 0)
                || (processRepository.GetAllRuleByRuleSetID(lowPriority.Id) != null && processRepository.GetAllRuleByRuleSetID(lowPriority.Id).Count > 0))
            {
                processRepository.DeleteAllModelProcessDefered(uow);
            }
            ProcessRuleSet tempRuleSet1 = processRepository.GetRuleSetById(ruleset1.ID);
            ProcessRuleSet tempRuleSet2 = processRepository.GetRuleSetById(ruleset2.ID);
            ruleset1.Priority = tempRuleSet2.Priority;
            ruleset2.Priority = tempRuleSet1.Priority;
            processRepository.UpdateRuleSetPriorityDefered(uow, ruleset1);
            processRepository.UpdateRuleSetPriorityDefered(uow, ruleset2);
            uow.Commit();
        }

        public void DeleteProcessRuleSet(int ruleSetID)
        {
            UnitOfWork uow = new UnitOfWork();
            //若已有应用当前被选Rule Set的Rule存在，则删除Model_Process数据表中的所有记录。
            //mantis 1406
            //3、检查是否该Rule Set已经被使用，若被使用，则警示用户，放弃后续操作。
            IList<ProcessRule> list = new List<ProcessRule>();
            list = processRepository.GetAllRuleByRuleSetID(ruleSetID);
            if (list != null && list.Count > 0)
            {
                List<string> erpara = new List<string>();
                FisException ex = new FisException("DMT157", erpara);
                throw ex;
            }
            /*if (processRepository.GetAllRuleByRuleSetID(ruleSetID) != null 
                && processRepository.GetAllRuleByRuleSetID(ruleSetID).Count > 0)
            {
                processRepository.DeleteAllModelProcessDefered(uow);
            }*/
            //mantis 1406

            processRepository.DeleteProcessRulesetByIDDefered(uow,ruleSetID);
            uow.Commit();
        }

        public string AddProcessRuleSet(RulesetInfoDataMaintain singleRuleSet)
        {
            //若Rule Set CheckList框中所有被选项与Rule Set List中某个栏位组合完全对应相同，则警示用户，放弃后续操作
            ProcessRuleSet ruleset = new ProcessRuleSet();
            UnitOfWork uow = new UnitOfWork();
            ruleset.ID = singleRuleSet.Id;
            ruleset.Priority = singleRuleSet.Id;
            ruleset.Condition1 = singleRuleSet.Condition1;
            ruleset.Condition2 = singleRuleSet.Condition2;
            ruleset.Condition3 = singleRuleSet.Condition3;
            ruleset.Condition4 = singleRuleSet.Condition4;
            ruleset.Condition5 = singleRuleSet.Condition5;
            ruleset.Condition6 = singleRuleSet.Condition6;
            ruleset.Cdt = DateTime.Now;
            ruleset.Udt = DateTime.Now;
            ruleset.Editor = singleRuleSet.Editor;
            if (processRepository.IFRuleSetIsExists(ruleset))
            {
                List<string> erpara = new List<string>();
                FisException ex = new FisException("DMT031", erpara);
                throw ex;
            }
            processRepository.AddProcessRuleSet(ruleset);
            processRepository.DeleteAllModelProcessDefered(uow);
            uow.Commit();
            return ruleset.ID.ToString();

        }

        public void EditAddProcessRuleSet(RulesetInfoDataMaintain singleRuleSet)
        {
            //若Rule Set CheckList框中所有被选项与Rule Set List中某个栏位组合完全对应相同，则警示用户，放弃后续操作
            //若已有应用当前被选Rule Set的Rule存在，则删除这些Rule，同时删除Model_Process数据表中的所有记录。
            ProcessRuleSet ruleset = new ProcessRuleSet();
            ruleset.ID = singleRuleSet.Id;
            ruleset.Priority = singleRuleSet.Id;
            ruleset.Condition1 = singleRuleSet.Condition1;
            ruleset.Condition2 = singleRuleSet.Condition2;
            ruleset.Condition3 = singleRuleSet.Condition3;
            ruleset.Condition4 = singleRuleSet.Condition4;
            ruleset.Condition5 = singleRuleSet.Condition5;
            ruleset.Condition6 = singleRuleSet.Condition6;
            ruleset.Udt = DateTime.Now;
            ruleset.Editor = singleRuleSet.Editor;
            if(processRepository.IFRuleSetIsExists(ruleset))
            {
                List<string> erpara = new List<string>();
                FisException ex = new FisException("DMT031", erpara);
                throw ex;
            }
            UnitOfWork uow = new UnitOfWork();

            //mantis 1406
            //检查是否该Rule已经被使用，若被使用，则警示用户，放弃后续操作。
            IList<ProcessRule> list = new List<ProcessRule>();
            list = processRepository.GetAllRuleByRuleSetID(singleRuleSet.Id);
            if (list != null && list.Count > 0)
            {
                List<string> erpara = new List<string>();
                FisException ex = new FisException("DMT157", erpara);
                throw ex;
            }

            /*if (processRepository.GetAllRuleByRuleSetID(singleRuleSet.Id) != null
                && processRepository.GetAllRuleByRuleSetID(singleRuleSet.Id).Count > 0)
            {
                processRepository.DeleteRuleByRuleSetIDDefered(uow,singleRuleSet.Id);
                processRepository.DeleteAllModelProcessDefered(uow);
            }*/
            //mantis 1406

            processRepository.UpdateProcessRuleSetDefered(uow,ruleset);
            uow.Commit();
        }

        public string AddRule(ProcessRule rule)
        {
            //若所有非Disable的Column Value输入框中的内容全都与Rule List表格中某个数据行中的对应栏位数据相同，则警示用户，放弃后续操作   
            if (processRepository.IFRuleIsExists(rule))
            {
                List<string> erpara = new List<string>();
                FisException ex = new FisException("DMT032", erpara);
                throw ex;
            }
            UnitOfWork uow = new UnitOfWork();
            processRepository.DeleteModelProcessByProcessDefered(uow, rule.Process);
            ////若当前被选Rule Set是Model，则删除Model_Process数据表中对应用户所输Model条件的记录。否则，删除Model_Process数据表中的所有记录。
            //if (processRepository.IFConditionIsModel(rule.Rule_set_id))
            //{
            //    processRepository.DeleteModelProcessByModelDefered(uow, rule.Value1);
            //}
            //else
            //{
            //    processRepository.DeleteAllModelProcessDefered(uow);
            //}
            processRepository.AddProcessRuleDefered(uow,rule);
            uow.Commit();

            return rule.Id.ToString();
        }

        public void EditRule(ProcessRule rule)
        {
            //若所有非Disable的Column Value输入框中的内容全都与Rule List表格中某个数据行中的对应栏位数据相同，则警示用户，放弃后续操作  
            if (processRepository.IFRuleIsExists(rule))
            {
                List<string> erpara = new List<string>();
                FisException ex = new FisException("DMT032", erpara);
                throw ex;
            }
            UnitOfWork uow = new UnitOfWork();
            processRepository.DeleteModelProcessByProcessDefered(uow, rule.Process);
            ////若当前被选Rule Set是Model，则删除Model_Process数据表中对应修改前后两个Model的记录。否则，删除Model_Process数据表中的所有记录。
            //if (processRepository.IFConditionIsModel(rule.Rule_set_id))
            //{
            //    ProcessRule item = processRepository.GetProcessRuleById(rule.Id);
            //    if (item != null)
            //    {
            //        processRepository.DeleteModelProcessByModelDefered(uow, item.Value1);
            //    }
            //    processRepository.DeleteModelProcessByModelDefered(uow, rule.Value1);
            //}
            //else
            //{
            //    processRepository.DeleteAllModelProcessDefered(uow);
            //}           
            processRepository.UpdateProcessRuleDefered(uow,rule);
            uow.Commit();
        }

        public void DeleteRule(int ruleID)
        {
            UnitOfWork uow = new UnitOfWork();
            ProcessRule rule = processRepository.GetProcessRuleById(ruleID);
            //對ProcessRule, ProcessRuleSet 有任何異動時，需要清空使用中此Process的Model表Model_Process，
            //在block station activity 時需要再重新計算哪一些機型會使用到這Process 
            processRepository.DeleteModelProcessByProcessDefered(uow, rule.Process);
            processRepository.DeleteProcessRuleByIDDefered(uow, ruleID);
            uow.Commit();


            ////若当前被选Rule Set是Model，则删除Model_Process数据表中对应被删除Model的记录。否则，删除Model_Process数据表中的所有记录
            //if (processRepository.IFConditionIsModel(rule.Rule_set_id))
            //{
            //    processRepository.DeleteModelProcessByModelDefered(uow, rule.Value1);
            //}
            //else
            //{
            //    processRepository.DeleteAllModelProcessDefered(uow);
            //}
            //processRepository.DeleteProcessRuleByIDDefered(uow,ruleID);
            //uow.Commit();
        }

        /// <summary>
        /// 根据Condtion和Process获得Model Process规则列表
        /// </summary>
        /// <param name="conditionID">ProcessRuleSet表的ID</param>
        /// <returns>IList<ProcessRule></returns>
        public IList<ProcessRule> GetRuleListByConditionAndProcess(int conditionID, string processName)
        {
            IList<ProcessRule> processRuleList = processRepository.GetRuleListByRuleSetIdAndProcess(conditionID,processName);
            return processRuleList;
        }

        public void CheckAdd_SaveModelProcess(int ruleID)
        {
            UnitOfWork uow = new UnitOfWork();
            ProcessRule rule = processRepository.GetProcessRuleById(ruleID);
            processRepository.DeleteModelProcessByProcessDefered(uow, rule.Process);
        }


        #endregion
    }
}
