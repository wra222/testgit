// 2010-01-07 Liu Dong(eB1-4)         Modify ITC-1103-0040
// 2010-01-07 Liu Dong(eB1-4)         Modify ITC-1103-0058
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using IMES.FisObject.Common.NumControl;
using IMES.Infrastructure;
using IMES.Infrastructure.Utility.Generates;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.Utility.RuleSets;
using IMES.Route;

namespace IMES.Activity
{
    /// <summary>
    /// 产生各种Number的Activity的基类
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseExecuteRuleSetActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    [Designer(typeof(BaseAcquireActivityDesigner), typeof(IDesigner))]
    public abstract partial class BaseAcquireActivity : BaseExecuteRuleSetActivity
    {
        /// <summary>
        /// Session Id
        /// </summary>
        public static DependencyProperty SessionIdProperty = DependencyProperty.Register("SessionId", typeof(Guid), typeof(BaseAcquireActivity));
        
        /// <summary>
        /// Session Id
        /// </summary>
        [DescriptionAttribute("SessionId")]
        [CategoryAttribute("Binding Property")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public Guid SessionId
        {
            get
            {
                return ((Guid)(base.GetValue(BaseAcquireActivity.SessionIdProperty)));
            }
            set
            {
                base.SetValue(BaseAcquireActivity.SessionIdProperty, value);
            }
        }

        ///<summary>
        ///</summary>
        public BaseAcquireActivity()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Init templates
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fileName"></param>
        /// <param name="ruleSetName"></param>
        /// <param name="snGenType"></param>
        protected void InitTemplates(System.Workflow.ComponentModel.ActivityExecutionContext executionContext, string fileName, string ruleSetName, string snGenType)
        {
            if (SNTemplateManager.GetInstance().GetDirtyBit(snGenType))
            {
                //System.Workflow.ComponentModel.Activity root = this.GetRootWorkflow(this);
                //if (root == null)
                //{
                //    root = this;
                //}
                RuleSetExecute(/*root*/ GetInheritedClassInst(), executionContext, fileName, ruleSetName);
                SNTemplateManager.GetInstance().SetDirtyBit(snGenType, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        protected void AddTemplatesOfAType(string type, IConverter[] val)
        {
            string key = GetKey(type);
            SNTemplateManager.GetInstance().Add(key, val);
        }

        /// <summary>
        /// Get Key
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetKey(string type)
        {
            return GetClass() + "." + type;
        }

        //protected void GetSession()
        //{
        //    this.session = this.GetSession(this.SessionId.ToString());
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract string GetClass();

        //protected string GetClassStr()
        //{
        //    return Enum.GetName(typeof(GeneratesConstants), GetClass());
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sess"></param>
        /// <param name="preSeqStr"></param>
        /// <param name="quantity"></param>
        /// <param name="seqRestart"></param>
        /// <param name="seqCvt"></param>
        /// <returns></returns>
        protected abstract SequencialNumberRanges getSequence(Session sess, string preSeqStr, int quantity, bool seqRestart, ISequenceConverter seqCvt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snrs"></param>
        /// <param name="seqCvt"></param>
        /// <param name="qty"></param>
        /// <param name="preSeqStr"></param>
        /// <returns></returns>
        protected List<string> GetAllNumbersInRange(SequencialNumberRanges snrs, ISequenceConverter seqCvt, int qty, string preSeqStr)
        {
            List<string> ret = new List<string>();
            ret.Add(snrs.Ranges[0].NumberBegin);
            for (int i = 0; i < qty - 2; i++)
            {
                ret.Add(string.Format(preSeqStr, seqCvt.NumberRule.IncreaseToNumber(this.CutOutSeq(preSeqStr, ret.Last<string>()), 1)));// 2010-01-07 Liu Dong(eB1-4)         Modify ITC-1103-0058
            }
            if (qty > 1)// 2010-01-07 Liu Dong(eB1-4)         Modify ITC-1103-0040
                ret.Add(snrs.Ranges[0].NumberEnd);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="noName"></param>
        /// <param name="maxNo"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        protected NumControl GetANumControl(string type, string noName, string maxNo, string customer)
        {
            NumControl nc = new NumControl(0, type, noName, maxNo, customer);
            return nc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preSeqStr"></param>
        /// <param name="wholeNumber"></param>
        /// <returns></returns>
        protected string CutOutSeq(string preSeqStr, string wholeNumber)
        {
            string ret = wholeNumber;
            string[] preSeqStrs = preSeqStr.Split(new string[] { "{0}" }, StringSplitOptions.RemoveEmptyEntries);
            if (preSeqStrs != null && preSeqStrs.Length > 0)
            {
                if (ret.StartsWith(preSeqStrs[0]))
                    ret = ret.Substring(preSeqStrs[0].Length);
                if (preSeqStrs.Length > 1 && ret.EndsWith(preSeqStrs[1]))
                    ret = ret.Substring(0, ret.Length - preSeqStrs[1].Length);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preSeqStr"></param>
        /// <param name="wholeNumber"></param>
        /// <param name="seqLength"></param>
        /// <returns></returns>
        protected string CutOutSeqOnlyPre(string preSeqStr, string wholeNumber, int seqLength)
        {
            string ret = wholeNumber;
            string[] preSeqStrs = preSeqStr.Split(new string[] { "{0}" }, StringSplitOptions.RemoveEmptyEntries);
            if (preSeqStrs != null && preSeqStrs.Length > 0)
            {
                if (ret.StartsWith(preSeqStrs[0]))
                    ret = ret.Substring(preSeqStrs[0].Length);
                ret = ret.Substring(0, seqLength);
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preSeqStr"></param>
        /// <returns></returns>
        protected string ExtendWildcardToEnd(string preSeqStr)
        {
            string ret = preSeqStr;
            string[] preSeqStrs = preSeqStr.Split(new string[] { "{0}" }, StringSplitOptions.RemoveEmptyEntries);
            if (preSeqStrs != null && preSeqStrs.Length > 1)
            {
                ret = ret.Remove(ret.Length - preSeqStrs[1].Length);
            }
            return ret;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class BaseAcquireActivityTheme : ActivityDesignerTheme
    {
        ///<summary>
        ///</summary>
        ///<param name="theme"></param>
        public BaseAcquireActivityTheme(WorkflowTheme theme)
            : base(theme)
        {
            this.BackColorStart = Color.White;
            this.BackColorEnd = Color.Pink;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ActivityDesignerTheme(typeof(BaseAcquireActivityTheme))]
    public class BaseAcquireActivityDesigner : ActivityDesigner
    {
        /// <summary>
        /// Default exceute
        /// </summary>
        protected override void DoDefaultAction()
        {
            try
            {
                IRemotingUrlService rus = this.GetService(
                    typeof(IRemotingUrlService)) as IRemotingUrlService;
                RuleDesigner.RemotingUrl = rus.RemotingUrl;
            }
            catch (System.Exception)
            {
            }
            RuleDesigner.DesignRule(this.Activity, true);
            base.DoDefaultAction();
        }
    }
}
