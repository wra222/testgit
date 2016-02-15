// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 更新PizzaStatus
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 用于更新Pizza的状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Pizza为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Pizza，调用Pizza的UpdateStatus方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pizza
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
    ///         更新PizzaStatus  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPizzaRepository
    ///              Pizza
    ///              PizzaStatus
    /// </para> 
    /// </remarks>
    public partial class UpdatePizzaStatusExt : BaseActivity
    {
        

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdatePizzaStatusExt));

     

        /// <summary>
        /// constructor
        /// </summary>
        public UpdatePizzaStatusExt()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update Pizza Status
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
        
            string line = "";
            var PizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository,Pizza>();
            Pizza pizza = CurrentSession.GetValue(Session.SessionKeys.Pizza) as Pizza;
            if (pizza == null)
             {
                    throw new NullReferenceException("Pizza in session is null");
              }
            line = string.IsNullOrEmpty(this.Line) ? pizza.Status.LineID : this.Line;
            var pizzaStatus = new PizzaStatus();
            pizzaStatus.StationID=this.Station;
            pizzaStatus.LineID = line;
            pizzaStatus.Editor = Editor;
            pizzaStatus.PizzaID = pizza.PizzaID;
            pizzaStatus.Cdt = DateTime.Now;
            pizzaStatus.Udt = DateTime.Now;
            pizza.UpdatePizzaStatus(pizzaStatus);
            PizzaRepository.Update(pizza, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }

       
    }
}
