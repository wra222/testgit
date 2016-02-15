// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 保存CartonWeight的称重结果
//              更新Product的CartonWeight
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-21   Yuan XiaoWei                 create
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.StandardWeight;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 保存CartonWeight的称重结果，更新Product的CartonWeight
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于CartonWeight，PizzaWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从session中获取ActuralWeight
    ///         2.从session中获取Product
    ///         3.将ActuralWeight赋予Product的CartonWeight属性
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ActuralWeight
    ///         Session.Product
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
    ///         Product的CartonWeight属性
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///            IProduct
    ///            IProductRepository
    /// </para> 
    /// </remarks>
    public partial class UpdatePizzaWeight : BaseActivity
	{
        public UpdatePizzaWeight()
		{
			InitializeComponent();
		}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProduct CurrentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            CurrentProduct.CartonWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.CartonWeight);
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ProductRepository.Update(CurrentProduct, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
