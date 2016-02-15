// INVENTEC corporation (c)2012all rights reserved. 
// Description:Combine COA and DN.docx
//             获取Pizza ID           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-02-03   207003                     create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PAK;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;
namespace IMES.Activity
{
    /// <summary>
    /// 获取DCode
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于Combine coa and dn
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         获取Pizza ID
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///        Session.SessionKeys.Lines 
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         PizzaID
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              INumControlRepository
    /// </para> 
    /// </remarks>
    public partial class GetPizzaIDForCOA : BaseActivity
    {
        private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// constructor
        /// </summary>
        public GetPizzaIDForCOA()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取DCode
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string line = this.Line;
            string lineAs = "";
            if (line != null && line.Length > 0)
            {
                lineAs = line.Substring(0, 1);
            }
            else
            {
                lineAs = " ";
            }
            DateTime dt = DateTime.Now;
            string year = string.Empty;
            string month = string.Empty;
            year = dt.Year.ToString().Substring(dt.Year.ToString().Length - 1, 1);
            switch (dt.Month.ToString())
            {
                case "10":
                    month = "A";
                    break;
                case "11":
                    month = "B";
                    break;
                case "12":
                    month = "C";
                    break;
                default:
                    month = dt.Month.ToString().Substring(dt.Month.ToString().Length - 1, 1);
                    break;
            }          
            string preSeqStr =  year + lineAs + month;
            string likecont = preSeqStr + "{0}";
            NumControl numCtrl = null;
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();

            string type = "PizzaID";
            lock (_syncRoot_GetSeq)
            {
                bool insOrUpd = true;
                string maxMo = numCtrlRepository.GetMaxNumber(type, likecont);
                string seq = string.Empty;
                string maxSeqStr = string.Empty;
                string maxNumStr = string.Empty;
                bool ChongQingIs = false;
                //流水码的取得
                if (string.IsNullOrEmpty(maxMo))
                {
                    seq = "200000";
                    insOrUpd = true;
                    
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    ArrayList retList = new ArrayList();
                    IList<string> valueList = new List<string>();
                    valueList = partRepository.GetValueFromSysSettingByName("Site");
                    if (valueList.Count == 0)
                    {
                    }
                    else
                    {
                        if (valueList[0] == "ICC")
                        {
                            ChongQingIs = true;
                        }
                    }
                    if (ChongQingIs == true)
                    {
                        seq = "500000";
                    }
                }
                else
                {
                    seq = maxMo.Substring(maxMo.Length - 6, 6);
                    insOrUpd = false;
                    if (seq.ToUpper() == "499999")
                    {
                        /*List<string> errpara = new List<string>();
                        throw new FisException("CHK162", errpara);*/
                    }
                    IList<NumControl> numCtrlLst = numCtrlRepository.GetNumControlByNoTypeAndValue(type, maxMo);
                    int numCtrlId = numCtrlLst[0].ID;
                    numCtrl = numCtrlRepository.Find(numCtrlId);
                }
                if (insOrUpd)
                {
                    maxNumStr = seq;
                }
                else
                {
                    if (seq.ToUpper() == "499999")
                    {
                        maxNumStr = "200000";
                    }
                    else if (seq.ToUpper() == "999999")
                    {
                        maxNumStr = "500000";
                    }
                    else
                    {
                        int num = Convert.ToInt32(seq);
                        num += 1;
                        maxNumStr = num.ToString();
                        int len = maxNumStr.Length;
                        for (var i = 0; i < 6 - len; i++)
                        {
                            maxNumStr = "0" + maxNumStr;
                        }
                    }
                }
                maxSeqStr = preSeqStr + maxNumStr;
                CurrentSession.AddValue(Session.SessionKeys.PizzaID, maxSeqStr);

                if (insOrUpd)
                {
                    //numCtrlRepository.Add(new NumControl(0, type, string.Empty, maxSeqStr, this.Customer), CurrentSession.UnitOfWork);
                    numCtrlRepository.InsertNumControl(new NumControl(0, type, string.Empty, maxSeqStr, this.Customer));
                }
                else
                {
                    numCtrl.Value = maxSeqStr;
                    numCtrl.NOType = "PizzaID";
                    numCtrl.NOName = "";
                    numCtrl.Customer = this.Customer;
                    numCtrlRepository.SaveMaxNumber(numCtrl, false, preSeqStr + "{0}");
                    //numCtrlRepository.Update(numCtrl, CurrentSession.UnitOfWork);
                }
            }


            return base.DoExecute(executionContext);
        }
    }
}
