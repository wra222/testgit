// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 更新Pizza状态，PizzaStatus表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
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
    /// 更新Pizza状态，PizzaStatus表
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PizzaKitting
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Pizza对象
    ///         2.调用Pizza对象的UpdateStatus方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PizzaID
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
    ///         更新PizzaStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPizzaRepository
    ///              PizzaID
    ///              PizzaStatus
    /// </para> 
    /// </remarks>
    public partial class UpdatePizzaStatus : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public UpdatePizzaStatus()
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
            IPizzaRepository PizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            var product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            //Pizza pizza = PizzaRepository.Find(product.PizzaID);
            Pizza pizza = product.PizzaObj;
            if (pizza != null && pizza.Status != null)
            {
                pizza.Status.StationID = Station.Substring(Station.Length-2, 2);
                PizzaRepository.Update(pizza, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }
    }


}
