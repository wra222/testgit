using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.QTime;
using IMES.FisObject.Common.Line;
using System.Collections.Generic;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.TestLog;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Hold;
using IMES.Common;



namespace IMES.Activity
{
    /// <summary>
    /// 檢查DefectHoldRule及設定DefectHoldRule Action
    /// </summary>
    public partial class CheckAndSetDefectHoldRule : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckAndSetDefectHoldRule()
		{
			InitializeComponent();
		}

       

        /// <summary>
        /// 檢查DefectHoldRule 及設定DefectHoldRule Action
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IHoldRepository holdRep = RepositoryFactory.GetInstance().GetRepository<IHoldRepository>();
            ILineRepository lineRep = RepositoryFactory.GetInstance().GetRepository<ILineRepository, Line>();
            
            IProduct prod = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            //if (prod.Repairs.Count == 0)
            //{
            //    return base.DoExecute(executionContext);
            //}

            DefectHoldRuleInfo condition = new DefectHoldRuleInfo() {
                                                                                        CheckInStation=this.Station};

            IList<DefectHoldRuleInfo> holdRuleList= holdRep.GetDefectHoldRule(condition);
            if (holdRuleList == null || holdRuleList.Count == 0)
            {
                return base.DoExecute(executionContext);
            }

            IList<string> defectList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);

            if (defectList == null )
            {
                //return base.DoExecute(executionContext);
                defectList = new List<string>();
            }

            string line ="";
            string pdLine = "";
            if (string.IsNullOrEmpty(this.Line))
            {
                pdLine =prod.Status.Line.Substring(0,1);
                line = prod.Status.Line.Substring(0,1);
            }else{
                line = this.Line.Substring(0,1);
                pdLine = this.Line;
            }
            IList<DefectHoldRuleInfo>  defectCodeEqualDefectCountList=null;
            IList<DefectHoldRuleInfo>  defectCodeOverDefectCountList=null;
            IList<DefectHoldRuleInfo> equalDefectCountList=null;
            IList<DefectHoldRuleInfo> overDefectCountList=null;

            getHighPriorityDefectHoldRule(holdRuleList, 
                                                            line, 
                                                            new List<string>() { prod.Model, prod.Family },
                                                            out defectCodeEqualDefectCountList,
                                                            out defectCodeOverDefectCountList,
                                                            out equalDefectCountList,
                                                            out overDefectCountList);
            DefectHoldRuleInfo matchHoldRule = null;

            #region DefectCode 不為空白且EqualSameDefectCount>0
            if (defectCodeEqualDefectCountList.Count > 0 && matchHoldRule==null)
            {
                var defectCodeList = defectCodeEqualDefectCountList.Select(x => x.DefectCode).Distinct().OrderByDescending(x=>x);
                foreach (string defectCode in defectCodeList)
                {
                    var defectHoldList = defectCodeEqualDefectCountList.Where(x => x.DefectCode == defectCode).ToList();
                    foreach (DefectHoldRuleInfo rule in defectHoldList)
                    {
                        matchHoldRule = matchDefectCodeEqualRule(prod, rule, defectList);
                         if (matchHoldRule != null)
                        {
                               break;
                        }
                    }
                    if (matchHoldRule != null)
                    {
                        break;
                    }
                }                
            }
            #endregion

            #region DefectCode 不為空白且OverDefectCount>0
            if (defectCodeOverDefectCountList.Count > 0 && 
                 matchHoldRule==null)
            {               
                var defectCodeList = defectCodeOverDefectCountList.Select(x => x.DefectCode).Distinct().OrderByDescending(x=>x);
                foreach (string defectCode in defectCodeList)
                {
                    var defectHoldList = defectCodeOverDefectCountList.Where(x => x.DefectCode == defectCode).ToList();
                    matchHoldRule = matchOverRule(prod, defectHoldList, defectList);
                    if (matchHoldRule != null)
                    {
                        break;
                    }
                }                
            }
            #endregion

            #region DefectCode 空白且EqualSameDefectCount>0
            if (equalDefectCountList.Count > 0 && 
                defectList.Count>0 && 
                matchHoldRule==null)
            {
                foreach(string code in  defectList)
                {
                    foreach (DefectHoldRuleInfo item in equalDefectCountList )
                    {
                        matchHoldRule=matchEqualRule(prod,item,code);
                         if (matchHoldRule != null)
                        {
                            break;
                        }
                    }
                     if (matchHoldRule != null)
                    {
                        break;
                    }
                }               
            }
            #endregion

            #region DefectCode 空白且OverDefectCount>0
             if (overDefectCountList.Count > 0 && 
                 matchHoldRule==null)
            {     
               matchHoldRule= matchOverRule(prod,overDefectCountList,defectList);
            }
            #endregion

            if (matchHoldRule != null)
            {
                #region write Hold
                string preStation = prod.Status.StationId;

                ActivityCommonImpl.Instance.FutureHold.writeHoldStation(null,
                                        prod,
                                        pdLine,
                                        "DefectHold",
                                         matchHoldRule.HoldStation,
                                         matchHoldRule.HoldCode,
                                         "Descr:" + matchHoldRule.HoldDescr + "~PreStation:" + preStation + "~CurStation:" + matchHoldRule.CheckInStation + "~DefectHold ID:" + matchHoldRule.ID.ToString() ,
                                         this.Editor);

                //throw error message
                int sameDefectCount=matchHoldRule.EqualSameDefectCount??0;
                int overDefectCount=matchHoldRule.OverDefectCount??0;
                int defectCount=sameDefectCount==0?overDefectCount: sameDefectCount;
                string code=string.IsNullOrEmpty( matchHoldRule.DefectCode) ? string.Join(",", defectList.ToArray()):matchHoldRule.DefectCode;
                throw new FisException("CQCHK0036", new string[] { prod.ProId, 
                                                                                                prod.CUSTSN, 
                                                                                                matchHoldRule.ID.ToString(), 
                                                                                                matchHoldRule.HoldDescr,
                                                                                                code,
                                                                                                defectCount.ToString()});
                #endregion
            }      

          

            return base.DoExecute(executionContext);
        }

      
        private void getHighPriorityDefectHoldRule(IList<DefectHoldRuleInfo> holdRuleList, 
                                                                                                        string line,
                                                                                                        IList<string> familyModelList,
                                                                                                        out IList<DefectHoldRuleInfo>  defectCodeEqualDefectCountList,
                                                                                                        out IList<DefectHoldRuleInfo>  defectCodeoverDefectCountList,
                                                                                                        out IList<DefectHoldRuleInfo> equalDefectCountList,
                                                                                                        out IList<DefectHoldRuleInfo> overDefectCountList)
        {
            defectCodeEqualDefectCountList = new List<DefectHoldRuleInfo>();
            defectCodeoverDefectCountList = new List<DefectHoldRuleInfo>();
            equalDefectCountList = new List<DefectHoldRuleInfo>();
            overDefectCountList = new List<DefectHoldRuleInfo>();           
           
            IList<PriorityInfo> priorityList = new List<PriorityInfo>();

            foreach (DefectHoldRuleInfo item in holdRuleList)
            {
                if (!string.IsNullOrEmpty(item.Line) &&
                     item.Line == line &&
                    !string.IsNullOrEmpty(item.Family) &&
                   familyModelList.Contains(item.Family))
                {
                    priorityList.Add(new PriorityInfo() { ID = item.ID, DefectCode= item.DefectCode, Priority = 0, Sequence = judgeSequence(item) });
                    continue;
                }

                if (string.IsNullOrEmpty(item.Line) &&
                   !string.IsNullOrEmpty(item.Family) &&
                  familyModelList.Contains(item.Family))
                {
                    priorityList.Add(new PriorityInfo() { ID = item.ID, DefectCode = item.DefectCode, Priority = 1, Sequence = judgeSequence(item) });
                    continue;
                }

                if (!string.IsNullOrEmpty(item.Line) &&
                     item.Line == line &&
                  string.IsNullOrEmpty(item.Family))
                {
                    priorityList.Add(new PriorityInfo() { ID = item.ID, DefectCode = item.DefectCode, Priority = 2, Sequence = judgeSequence(item) });
                    continue;
                }

                if (string.IsNullOrEmpty(item.Line) &&
                   string.IsNullOrEmpty(item.Family))
                {
                    priorityList.Add(new PriorityInfo() { ID = item.ID, DefectCode = item.DefectCode, Priority = 3, Sequence = judgeSequence(item) });
                    continue;
                }
            }

            if (priorityList.Count > 0)
            {
                int firstPriority = priorityList.Min(x => x.Priority);
                var prioritySeq = priorityList.Where(x => x.Priority == firstPriority).OrderBy(y => y.DefectCode + y.Sequence).ToList();
                foreach (PriorityInfo item in prioritySeq)
                {

                    switch (item.Sequence)
                    {
                        case "00":
                            defectCodeEqualDefectCountList.Add(holdRuleList.Where(x => x.ID == item.ID).First());
                            break;
                        case "01":
                            defectCodeoverDefectCountList.Add(holdRuleList.Where(x => x.ID == item.ID).First());
                            break;
                        case "10":
                            equalDefectCountList.Add(holdRuleList.Where(x => x.ID == item.ID).First());
                            break;
                        case "11":
                            overDefectCountList.Add(holdRuleList.Where(x => x.ID == item.ID).First());
                            break;
                        default:
                            break;
                    }
                }
            }
        }



        private string judgeSequence(DefectHoldRuleInfo item)
        {
            if (!string.IsNullOrEmpty(item.DefectCode))
            {
                if (item.EqualSameDefectCount > 0)
                {
                    return "00";
                }
                else if (item.OverDefectCount > 0)
                {
                    return "01";
                }
            }
            else
            {
                if (item.EqualSameDefectCount > 0)
                {
                    return "10";
                }
                 else if (item.OverDefectCount > 0)
                {
                    return "11";
                }
            }

            return "20";
        }


        private DefectHoldRuleInfo matchDefectCodeEqualRule(IProduct prod, DefectHoldRuleInfo rule, IList<string> defectList)
        {
            DefectHoldRuleInfo matchHoldRule = null;

            int qty = getDefectCount(prod, 
                                                 rule.DefectCode, 
                                                 string.IsNullOrEmpty(rule.ExceptCause)? new List<string>() : rule.ExceptCause.Split(new char[] { '~' }).ToList());
            if (defectList.Contains(rule.DefectCode))
            {
                qty = qty + 1;
            }
            if (qty == (rule.EqualSameDefectCount ?? 0))
            {
                matchHoldRule= rule;
            }                 
            return matchHoldRule;
        }
      

        private DefectHoldRuleInfo matchEqualRule(IProduct prod, DefectHoldRuleInfo rule, string defectCode)
        {
            int qty = getDefectCount(prod, 
                                                defectCode,
                                                 string.IsNullOrEmpty(rule.ExceptCause) ? new List<string>() : rule.ExceptCause.Split(new char[] { '~' }).ToList()) + 1;
            if (qty == (rule.EqualSameDefectCount??0))
            {
                return rule;
            }
            return null;
        }


        private DefectHoldRuleInfo matchOverRule(IProduct prod, IList<DefectHoldRuleInfo> ruleList, IList<string> defectList)
        {
           
            DefectHoldRuleInfo matchRule=null;
            IList<PriorityInfo> remianingQtyList = new List<PriorityInfo>();
            foreach (DefectHoldRuleInfo item in ruleList)
            {
                int diffQty = getDefectCount(prod, 
                                                            item.DefectCode,
                                                            string.IsNullOrEmpty(item.ExceptCause) ? new List<string>() : item.ExceptCause.Split(new char[] { '~' }).ToList()) 
                                                            - (item.OverDefectCount ?? 0);
                if (string.IsNullOrEmpty(item.DefectCode))
                {
                    diffQty = diffQty + defectList.Count;
                }
                else if (defectList.Contains(item.DefectCode))
                {
                    diffQty = diffQty + 1;
                }

                if (diffQty>=0 )
                {
                    remianingQtyList.Add(new PriorityInfo() { ID = item.ID, Priority=diffQty});
                }
            }

            if (remianingQtyList.Count > 0)
            {
                int minQty=remianingQtyList.Min(x => x.Priority);
                var prioritySeq = remianingQtyList.Where(x => x.Priority == minQty).OrderByDescending(y => y.ID).ToList();
                matchRule = ruleList.Where(x => x.ID == prioritySeq[0].ID).First();          

            }
            return matchRule;
        }        

        private int getDefectCount(IProduct prod, string defectCode,IList<string> exceptCause)
        {
            int ret = 0;
            IList<Repair> repairList = prod.Repairs;
            foreach(Repair item in repairList)
            {
                var defectId= (from p in item.Defects
                                   where (string.IsNullOrEmpty(defectCode)  || p.DefectCodeID==defectCode) &&
                                              !exceptCause.Contains(p.Cause)
                                   select p.ID);
                if (defectId != null)
                {
                    ret = ret + defectId.Count();
                }
            }  
            return ret;
        }

        private class PriorityInfo
        {
            public int ID;
            public string DefectCode;
            public int Priority = -1;
            public string Sequence;
            
        }
	}


}
