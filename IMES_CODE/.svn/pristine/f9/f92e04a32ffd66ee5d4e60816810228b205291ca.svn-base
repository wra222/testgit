// INVENTEC corporation (c)2011 all rights reserved. 
// Description:
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
namespace IMES.Activity
{
    /// <summary>
    /// 保存各种类型的PrintLog
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///        无 
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
    public partial class ReuseCustomerSn : BaseActivity
    {

        /// <summary>
        /// constructor
        /// </summary>
        public ReuseCustomerSn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
			IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
			if (null!=product && !string.IsNullOrEmpty(product.CUSTSN)){
				CurrentSession.AddValue(Session.SessionKeys.PrintLogName, product.Customer + "SNO");
				CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, product.CUSTSN);
				CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, product.CUSTSN);
				CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, product.ProId);
			}
            return base.DoExecute(executionContext);
        }
    }
}