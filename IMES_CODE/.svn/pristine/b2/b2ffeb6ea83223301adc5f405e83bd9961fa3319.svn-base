// INVENTEC corporation (c)2010 all rights reserved. 
// Description:根据AcquirePizzaID获取的PizziaID生成一个Pizza对象放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-30   Yuan XiaoWei                 create
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
using IMES.FisObject.PAK;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 根据AcquirePizzaID获取的PizziaID生成一个Pizza对象放到Session中
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
    ///         1.根据AcquirePizzaID获取的PizziaID生成一个Pizza对象放到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.PizzaNoList
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
    ///              Pizza
    /// </para> 
    /// </remarks>
    public partial class CreateNewPizza : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateNewPizza()
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
            string pizzaIDList = (string)CurrentSession.GetValue(Session.SessionKeys.PizzaID);
            Pizza NewPizza = new Pizza();
            NewPizza.PizzaID = pizzaIDList;
            //IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            //NewPizza.BindProduct = currentProduct;
            PizzaStatus NewStatus = new PizzaStatus();
            NewStatus.Editor = Editor;
            NewStatus.LineID = Line;
            NewStatus.StationID = Station;
            NewStatus.PizzaID = NewPizza.PizzaID;
            NewPizza.Status = NewStatus;

            //currentProduct.PizzaObj = NewPizza;
            //CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);
            CurrentSession.AddValue(Session.SessionKeys.Pizza, NewPizza);
            return base.DoExecute(executionContext);
        }
    }
}
