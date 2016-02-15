// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Product绑定的PizzaID获取Pizza对象
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-15   Yuan XiaoWei                 create
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
    ///         1. 根据Product绑定的PizzaID获取Pizza对象
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
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Pizza
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPizzaRepository
    ///         IProduct
    ///         Pizza
    /// </para> 
    /// </remarks>
    public partial class GetProductPizza : BaseActivity
	{
		///<summary>
		///</summary>
        public GetProductPizza()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 根据Product绑定的PizzaID获取Pizza对象
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProduct CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
      
            Pizza CurrentPizza = CurrentProduct.PizzaObj;
            CurrentPizza.BindProduct = CurrentProduct;
            CurrentSession.AddValue(Session.SessionKeys.Pizza, CurrentPizza);
            return base.DoExecute(executionContext);
        }
	}
}
