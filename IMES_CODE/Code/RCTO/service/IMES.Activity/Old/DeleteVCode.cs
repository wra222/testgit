// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 删除数据库中当前VCode相关信息
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-24   Chen Xu (eB1-4)              create
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.TPCB;
using IMES.DataModel;


namespace IMES.Activity
{
    /// <summary>
    /// 删除数据库中当前VCode相关信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         097 ITPCBTPLabel
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         从Session获得VCode, 删除数据库GetData..TPCB_Maintain中当前VCode的相关信息
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.VCode
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
    ///         Update TPCB_Maintain
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         ITPCBRepository
    ///         TPCB
    /// </para> 
    /// </remarks>
    public partial class DeleteVCode: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeleteVCode()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Delete数据库中当前VCode相关信息
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentVCode = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
            ITPCBRepository TPCBRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBRepository, TPCB>();
            TPCBRepository.DeleteVcodeInfo(currentVCode);
            CurrentSession.AddValue(Session.SessionKeys.IsComplete, true);
            return base.DoExecute(executionContext);
        }

	}
}
