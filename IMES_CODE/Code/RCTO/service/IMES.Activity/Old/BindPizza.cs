// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 将PizzaID绑定到Product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-10   Yuan XiaoWei                 create
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
using IMES.FisObject.PAK;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity
{
    /// <summary>
    /// 将PizzaID绑定到Product
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于InitialPVS,FinalPVS
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.将PizzaID绑定至Product对象;
    ///         2.保存Product对象;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    ///         Session.PizzaID
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProductRepository
    ///         IProduct
    ///         PizzaID
    /// </para> 
    /// </remarks>
    public partial class BindPizza : BaseActivity
	{
		///<summary>
		///</summary>
        public BindPizza()
		{
			InitializeComponent();
		}

        /// <summary>
        /// PizzaID bind to Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Pizza CurrentPizza = (Pizza)CurrentSession.GetValue(Session.SessionKeys.Pizza);
            IProduct CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            CurrentProduct.PizzaID = CurrentPizza.PizzaID;
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ProductRepository.Update(CurrentProduct, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
