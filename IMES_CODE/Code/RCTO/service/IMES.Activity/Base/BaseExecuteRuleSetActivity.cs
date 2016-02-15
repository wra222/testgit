using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using IMES.Infrastructure.Utility.RuleSets;
using IMES.Route;

namespace IMES.Activity
{
    /// <summary>
    /// 执行各种RuleSet的Activity的基类
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
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
    /// 
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
    [Designer(typeof(BaseRuleSetActivityDesigner), typeof(IDesigner))]
    public abstract partial class BaseExecuteRuleSetActivity : BaseActivity
    {
        ///<summary>
        ///</summary>
        public BaseExecuteRuleSetActivity()
        {
            InitializeComponent();
        }

        /// <summary>
        /// execute
        /// </summary>
        /// <param name="root"></param>
        /// <param name="executionContext"></param>
        /// <param name="fileName"></param>
        /// <param name="ruleSetName"></param>
        protected static void RuleSetExecute(System.Workflow.ComponentModel.Activity root, ActivityExecutionContext executionContext, string fileName, string ruleSetName)
        {
            RuleSet ruleSet = DeserializedRuleSetsManager.getInstance.getRuleSet(fileName, ruleSetName);
            ExecuteRuleSet(ruleSet, root, executionContext);
        }

        private static void ExecuteRuleSet(RuleSet ruleSet, System.Workflow.ComponentModel.Activity root, ActivityExecutionContext executionContext)
        {
            RuleValidation validation = new RuleValidation(root.GetType(), null);
            RuleExecution execution = new RuleExecution(validation, root, executionContext);
            ruleSet.Execute(execution);
        }
    }

    ///<summary>
    ///</summary>
    public sealed class BaseRuleSetActivityTheme : ActivityDesignerTheme
    {
        ///<summary>
        ///</summary>
        ///<param name="theme"></param>
        public BaseRuleSetActivityTheme(WorkflowTheme theme)
            : base(theme)
        {
            //BackgroundStyle = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            //BorderColor = Color.Green;

            this.BackColorStart = Color.White;
            this.BackColorEnd = Color.Pink;
        }
    }

    ///<summary>
    ///</summary>
    [ActivityDesignerTheme(typeof(BaseRuleSetActivityTheme))]
    public class BaseRuleSetActivityDesigner : ActivityDesigner
    {

        /// <summary>
        /// default execute
        /// </summary>
        protected override void DoDefaultAction()
        {
            try
            {
                IRemotingUrlService rus = this.GetService(
                    typeof(IRemotingUrlService)) as IRemotingUrlService;
                RuleDesigner.RemotingUrl = rus.RemotingUrl;
            }
            catch (System.Exception )
            {
            }

            RuleDesigner.DesignRule(this.Activity, false);
            base.DoDefaultAction();
        }
    }

}
