/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: PCARepairImpl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-11-03   207006     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


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
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 保存产生的VirtualMoBOM
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于
    ///         102Virtual Mo
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         调用GenerateVirtualMoBOM sp
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.VirtualMOList
    ///         Session.ModelName
    ///        
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
    ///     
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///        
    /// </para> 
    /// </remarks>
    public partial class GenerateVirtualMoBOM : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GenerateVirtualMoBOM()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Generate Virtual MO BOM
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var moList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.VirtualMOList);
            string virtualMO = moList[0];
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            IMiscRepository currentMiscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            currentMiscRepository.GenerateVirtualMoBOM(model, 20, virtualMO);
            return base.DoExecute(executionContext);
        }
	}
}
