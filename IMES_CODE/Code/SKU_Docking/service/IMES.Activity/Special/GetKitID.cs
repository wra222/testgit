// INVENTEC corporation (c)2010all rights reserved. 
// Description:CI-MES12-SPEC-PAK-UC Packing Pizza.docx
//             获取Kit ID           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-12-01   zhu lei                      create
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
using IMES.FisObject.Common.Warranty;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure.UnitOfWork;
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
    ///      应用于PackingPizza
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         获取Kit ID
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///        Session.SessionKeys.WarrantyCode 
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         DCode
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IWarrantyRepository
    ///              Warranty
    /// </para> 
    /// </remarks>
    public partial class GetKitID : BaseActivity
    {
        private const long SS_INIT = 80000;
        private const string SS_TYPE = "FlowNumber";
        private const string SS_NAME = "KitIDFlowNumber";
        private const string SS_CUST = "HP";
        private static object SS_LOCK = new object();
        private static string GetFlowNumber()
        {
            long flow_number = 0;

            lock (SS_LOCK)
            {
                SqlTransactionManager.Begin();
                INumControlRepository rep = RepositoryFactory.GetInstance()
                    .GetRepository<INumControlRepository, NumControl>();
                NumControl next_avail = rep.GetMaxValue(SS_TYPE, SS_NAME);
                if (next_avail == null)
                {
                    next_avail = new NumControl();
                    next_avail.Customer = SS_CUST;
                    next_avail.NOType = SS_TYPE;
                    next_avail.NOName = SS_NAME;

                    flow_number = SS_INIT;
                    next_avail.Value = (flow_number + 1).ToString();

                    rep.InsertNumControl(next_avail);
                    SqlTransactionManager.Commit();
                }
                else
                {
                    flow_number = long.Parse(next_avail.Value);
                    next_avail.Value = (flow_number + 1).ToString();

                    IUnitOfWork uof = new UnitOfWork();
                    rep.Update(next_avail, uof);
                    uof.Commit();
                    SqlTransactionManager.Commit();
                }
            }

            return (flow_number % 100000).ToString("D5");
        }

        //private static object _syncRoot_GetSeq = new object();

        /// <summary>
        /// constructor
        /// </summary>
        public GetKitID()
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
            DateTime dt = DateTime.Now;
            IList<string> nameStrLst = null;
            string nameStr = "SITECODE";
            string year = string.Empty;
            string month = string.Empty;
            string siteCode = string.Empty;
            
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
            //siteCode的取得缺少
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            nameStrLst = partRepository.GetValueFromSysSettingByName(nameStr);
            if (nameStrLst.Count() > 0)
            {
                siteCode = nameStrLst[0].ToString();
            }
            string preSeqStr = "P" + year + siteCode + month;
            /*
            string likecont = preSeqStr + "{0}";
            NumControl numCtrl = null;
            INumControlRepository numCtrlRepository = RepositoryFactory.GetInstance().GetRepository<INumControlRepository, NumControl>();
            string type = "KitID";
            lock (_syncRoot_GetSeq)
            {
                bool insOrUpd = true;
                string maxMo = numCtrlRepository.GetMaxNumber(type, likecont);
                string seq = string.Empty;
                string maxSeqStr = string.Empty;
                string maxNumStr = string.Empty;
                //流水码的取得
                if (string.IsNullOrEmpty(maxMo))
                {
                    seq = "00001";
                    insOrUpd = true;
                }
                else
                {
                    seq = maxMo.Substring(maxMo.Length - 5, 5);
                    insOrUpd = false;
                    if (seq.ToUpper() == "99999")
                    {
                        List<string> errpara = new List<string>();
                        throw new FisException("CHK162", errpara);
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
                    int num = Convert.ToInt32(seq);
                    num += 1;
                    maxNumStr = num.ToString();
                    int len = maxNumStr.Length;
                    for (var i = 0; i < 5 - len; i++)
                    {
                        maxNumStr = "0" + maxNumStr;
                    }
                }
             }
             */
             string   maxSeqStr = preSeqStr + GetFlowNumber();
             CurrentSession.AddValue(Session.SessionKeys.PizzaID, maxSeqStr);

             CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "MP");
             CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, maxSeqStr);
             CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, maxSeqStr);
             CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");

            return base.DoExecute(executionContext);
        }
    }
}
