// INVENTEC corporation (c)201all rights reserved. 
// Description:  检查 IECPN 在数据库中是否存在 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-05-06   Chen Xu (eB1-4)              create
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
using IMES.FisObject.FA.Product;
namespace IMES.Activity
{

    /// <summary>
    /// 检查 IECPN 在数据库中是否存在 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///          108 Small Parts Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         如果用户输入的[IECPN] 在数据库中不存在，则报告错误：“无法找到TSB PN!”
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
	///								CHK131: 无法找到TSB PN!
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.IECPN
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
    ///         IProductRepository
	///			IProduc
    /// </para> 
    /// </remarks>

    public partial class CheckIECPN : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckIECPN()
		{
			InitializeComponent();
		}
        
        /// <summary>
        /// 检查输入的IECPN在数据库中是否存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentIECPN = (string)CurrentSession.GetValue(Session.SessionKeys.IECPN);
            IProductRepository smallpartsRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            int ExitOrNot = smallpartsRepository.CheckIECPPN(currentIECPN);

            if (ExitOrNot > 0)
            {
                return base.DoExecute(executionContext);
            }

            else
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK131", errpara);
            }
        }
	}
}
