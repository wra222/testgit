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
using IMES.Infrastructure;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 使workflow暂停执行, 等待用户输入
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有需要等待用户输入的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         通过Session.SwitchToXXX()实现
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
    ///         无
    /// </para> 
    /// </remarks>
    public partial class WaitInput : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public WaitInput()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        protected internal override void PreExecute()
        {
            Session oldSession = CurrentSession;
            string workflowName = oldSession.WorkflowInstance.GetWorkflowDefinition().Name;
            string key = oldSession.Key;
            DateTime now = DateTime.Now;
            oldSession.SwitchToHost();     //is waiting  for session.SwitchToWorkFlow command

            //Vincent 2014-07-14 handle SwitchToWorkFlow後重新h從SessiomManager抓Session時，Session為null, 等待長時間Session Time out 已被清除
            Session session = CurrentSession;
            if (session != null)
            {
                session.Exception = null;
            }
            else
            {
                //當做RemoveSession from SessionManager 時遇到無session Case 調用外部程序handle rollback data
                HandleRollBack.Execute(workflowName, oldSession, key, this.Line, this.Editor, this.Customer);

                throw new Exception(string.Format("The workflow {0}::{1} operation of  key:{2}  Station:{3} Line:{4} Editor:{5} Cdt:{6} is invalid due to session time out or manual remove session. Please scan again!!",
                                                                      workflowName, this.Name, key, this.Station, this.Line, this.Editor, now.ToString("yyyyMMdd HH:mm:ss.fff")));
            }
        }
	}
}
