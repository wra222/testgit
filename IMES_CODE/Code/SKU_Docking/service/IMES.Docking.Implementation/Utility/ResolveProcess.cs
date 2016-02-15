﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository.Common;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.FisObject.Common.Process;
using IMES.DataModel;
using System.Text.RegularExpressions;

namespace IMES.Docking.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ResolveProcess
	{
        static readonly IProcessRepository processRep = RepositoryFactory.GetInstance().GetRepository<IProcessRepository>();
        static readonly IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="editor"></param>
        /// <param name="firstLine"></param>
        public static void CreateModelProcess(Model model, string editor, string firstLine)
        {        

            string modelName = model.ModelName;

            //IProcessRepository processRep = RepositoryFactory.GetInstance().GetRepository<IProcessRepository>();
            IList<ProcessRuleSet> ruleSetList= processRep.GetAllProcessRuleset();
            if (ruleSetList == null || ruleSetList.Count == 0)
            {
                throw new FisException("CHK112",new string[] { modelName });
            }

            IList<int> ruleSetIDList = processRep.GetAllRuleSetIDInProcessRule();
            if (ruleSetIDList == null || ruleSetIDList.Count == 0)
            {
                throw new FisException("CHK112", new string[] { modelName });
            }

            ruleSetList = ruleSetList.Where(x=>ruleSetIDList.Contains(x.ID)).ToList();
            if (ruleSetList == null || ruleSetList.Count == 0)
            {
                throw new FisException("CHK112", new string[] { modelName });
            }

            setProcessConditionValue(model, firstLine, ruleSetList);
            ruleSetList = ruleSetList.OrderByDescending(x => x.Priority).ToList();
            ProcessRule processRule = null;
            foreach (ProcessRuleSet item in ruleSetList)
            {
                var processRuleList = processRep.GetAllRuleByRuleSetID(item.ID).OrderByDescending(x=>x.Udt).ToList();
                processRule = compareProcessRule(processRuleList, item);
                if (processRule != null)
                {
                    break;
                }
            }

            if (processRule == null)
            {
                throw new FisException("CHK112", new string[] { modelName });
            }

            IList<ModelProcess> modelProcessList= processRep.GetModelProcessByModelLine(modelName, firstLine);
            if (modelProcessList == null || modelProcessList.Count == 0)
            {
                processRep.AddModelProcess(modelName, processRule.Process, firstLine, editor);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="editor"></param>
        /// <param name="firstLine"></param>
        public static void CreateModelProcess(string modelName, string editor, string firstLine)
        {
            //IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
            Model model = modelRep.Find(modelName);
            if (model == null)
            {
                throw new FisException("CHK038", new string[] { modelName });
            }
            CreateModelProcess(model, editor, firstLine);
        }

        static void setProcessConditionValue(Model model, string firstLine,  IList<ProcessRuleSet>  ruleSetList)
        {
            foreach (ProcessRuleSet item in ruleSetList)
            {
                IList<string> valueList = new List<string>();
                if (!string.IsNullOrEmpty(item.Condition1))
                {
                    valueList.Add(getConditionValue(model, firstLine, item.Condition1));
                }
                else
                {
                    item.ConditiionValueList = valueList;
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Condition2))
                {
                    valueList.Add(getConditionValue(model, firstLine, item.Condition2));
                }
                else
                {
                    item.ConditiionValueList = valueList;
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Condition3))
                {
                   valueList.Add( getConditionValue(model, firstLine, item.Condition3));
                }
                else
                {
                    item.ConditiionValueList = valueList;
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Condition4))
                {
                    valueList.Add( getConditionValue(model, firstLine, item.Condition4));
                }
                else
                {
                    item.ConditiionValueList = valueList;
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Condition5))
                {
                    valueList.Add(getConditionValue(model, firstLine, item.Condition5));
                }
                else
                {
                    item.ConditiionValueList = valueList;
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Condition6))
                {
                  valueList.Add( getConditionValue(model, firstLine, item.Condition6));
                }
                else
                {
                    item.ConditiionValueList = valueList;
                    continue;
                }

              
            }
        }

        static string getConditionValue(Model model, string firstLine, string name)
        {
            string ret = string.Empty;
            switch (name)
            {
                case "Model":
                    ret=model.ModelName;
                    break;
                case "Line":
                    ret = firstLine;
                    break;
                case "Family":
                    ret = model.FamilyName;
                    break;
                case "Customer":
                    ret = model.Family.Customer;
                    break;
                case "CustPN":
                    ret = model.CustPN;
                    break;
                case "Region":
                    ret = model.Region;
                    break;
                case "ShipType":
                    ret = model.ShipType;
                    break;
                case "OSCode":
                    ret = model.OSCode;
                    break;
                case "OSDesc":
                    ret = model.OSDesc;
                    break; 
                default:
                    ret =model.GetAttribute(name);
                    break;
            }
            return ret??string.Empty;
        }

        static ProcessRule compareProcessRule(IList<ProcessRule> processRuleList, ProcessRuleSet ruleSet)
        {
            ProcessRule ret =null;
            foreach (ProcessRule item in processRuleList)
            {
                if (matchRule(item, ruleSet))
                {
                    ret = item;
                    break;
                }
            }
            return ret;
        }

        static bool matchRule(ProcessRule item, ProcessRuleSet ruleSet)
        {
            IList<string> ruleValueList = ruleSet.ConditiionValueList;
            int count = ruleValueList.Count;
            bool bAllmatched = (count==0? false:true);
            
            for (int i=0; i<count;++i)
            {
                string ruleValue = ruleValueList[i];
                if (string.IsNullOrEmpty(ruleValue))
                {
                    bAllmatched = false;
                    break;
                }
                if (!checkValue(ruleValue, i, item))
                {
                    bAllmatched = false;
                    break;
                }               
            }

            return bAllmatched;
        }

        static bool checkValue(string ruleValue, int index, ProcessRule processRule)
        {
            string pattern = null;
            if (index == 0)
            {
                pattern = processRule.Value1;
            }
            else if (index == 1)
            {
                pattern = processRule.Value2;
            }
            else if (index == 2)
            {
                pattern = processRule.Value3;
            }
            else if (index == 3)
            {
                pattern = processRule.Value4;
            }
            else if (index == 4)
            {
                pattern = processRule.Value5;
            }
            else if (index == 5)
            {
                pattern = processRule.Value6;
            } 

            if (string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            return Regex.IsMatch(ruleValue, pattern, RegexOptions.Compiled);
        }


	}
}
