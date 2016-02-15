// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入的[TPCB]/[T/P]/[VCode]在数据库中是否存在
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-23   Chen Xu (eB1-4)              create
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

namespace IMES.Activity
{
    /// <summary>
    ///  检查输入的[TPCB]/[T/P]/[VCode]在数据库中是否存在
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
    ///          打印前，从Session中获得[TPCB]/[T/P]/[VCode]，调用ITPCBRepository，检查在数据库中这组数据是否存在。
	///			 存在则打印，否则提示信息:请输入正确的数据，或者先保存数据再打印！
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///          1.系统异常：
    ///          2.业务异常：
	///								CHK095 : 请输入正确的数据，或者先保存数据再打印！！
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPCB
	///			Session.TP
	///			Session.VCode
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
    ///         ITPCBRepository
    ///         TPCB
    /// </para> 
    /// </remarks>

    public partial class CheckVCode : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckVCode()
		{
			InitializeComponent();
		}
        
        /// <summary>
        /// 检查输入的[TPCB]/[T/P]/[VCode]在数据库中是否存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var currentTPCB = (string)CurrentSession.GetValue(Session.SessionKeys.TPCB);
            var currentTP = (string)CurrentSession.GetValue(Session.SessionKeys.TP);
            var currentVCode = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
            ITPCBRepository TPCBRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBRepository, TPCB>();
            int ExitOrNot = TPCBRepository.CheckVCode(currentTPCB, currentTP, currentVCode);

            if (ExitOrNot > 0)
            {
                return base.DoExecute(executionContext);
            }

            else
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK095", errpara);
            }

        }
	}
}
