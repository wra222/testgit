// INVENTEC corporation (c)2009 all rights reserved. 
// Description: CustomVersion绑定至MB
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-09   Yuan XiaoWei                 create
// Known issues:
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// CustomVersion绑定至MB
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于ICT Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.将CustomVersion绑定至MB对象;
    ///         2.保存MB对象;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.CustomVersion
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
    ///         更新PCB
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    ///         IMB
    /// </para> 
    /// </remarks>
    public partial class BindCustomVersion : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public BindCustomVersion()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 执行绑定CustomVersion到MB的逻辑操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            currentMB.CUSTVER = (string)CurrentSession.GetValue(Session.SessionKeys.CustomVersion);
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            currentMBRepository.Update(currentMB, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
