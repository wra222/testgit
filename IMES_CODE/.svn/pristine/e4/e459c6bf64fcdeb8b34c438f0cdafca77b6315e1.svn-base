// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 更新PartSN,将VendorSN绑定上
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-06   Yuan XiaoWei                 create
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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 更新PartSN的VendorSN,Editor,Udt
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于Combine KPCT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：
    ///                     
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.VendorSN
    ///         Session.PartSN
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPartSnRepository
    /// </para> 
    /// </remarks>
	public partial class BindKPCT: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public BindKPCT()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 更新PartSN的VendorSN,Editor,Udt
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartSnRepository currentPartSNRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
            PartSn currentPartSN = (PartSn)CurrentSession.GetValue(Session.SessionKeys.PartSN);
            string currentVendorSN = CurrentSession.GetValue(Session.SessionKeys.VendorSN).ToString();
            currentPartSN.VendorSn = currentVendorSN;
            currentPartSN.Editor = Editor;
            currentPartSNRepository.Update(currentPartSN, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
	}
}
