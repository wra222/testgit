// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入VendorSN的正确性
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
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
namespace IMES.Activity
{
    /// <summary>
    /// 检查输入VendorSN的正确性
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         仅应用于Combine KPCT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK028
    ///                     CHK029
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.VendorSN
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
	public partial class CheckVendorSN: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckVendorSN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 检查VendorSN是否已经和CTSN绑定
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartSnRepository currentPartSNRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
            string currentVendorSN = CurrentSession.GetValue(Session.SessionKeys.VendorSN).ToString();

            //Vendor SN是不是已經與其他PartSn綁定
            if (currentPartSNRepository.FindPartSnByVendorSn(currentVendorSN) != null)
            {
                var ex = new FisException("CHK030", new string[] { currentVendorSN });
                ex.stopWF = false;
                throw ex;
            }
            CurrentSession.AddValue(Session.SessionKeys.IsComplete, true);
            return base.DoExecute(executionContext);
        }
	}
}
