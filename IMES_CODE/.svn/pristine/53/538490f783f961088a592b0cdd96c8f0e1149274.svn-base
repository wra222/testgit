// INVENTEC corporation (c)2010all rights reserved. 
// Description:将当前Workflow新建的Pizza对象保存到Pizza表中，同时插入PizzaStatus
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

namespace IMES.Activity
{
    /// <summary>
    /// 将当前Workflow新建的Pizza对象保存到Pizza表中，同时插入PizzaStatus
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
    ///         1.将当前Workflow新建的Pizza对象保存到Pizza表中，同时插入PizzaStatus
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///        Session.SessionKeys.Pizza 
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
    ///         插入Pizza
    ///         插入PizzaStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPizzaRepository
    ///              Pizza
    ///              PizzaStatus
    /// </para> 
    /// </remarks>
    public partial class GeneratePizzaID : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public GeneratePizzaID()
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

            Pizza CurrentPizza = (Pizza)CurrentSession.GetValue(Session.SessionKeys.Pizza);

            PizzaStatus currentPizzaStatus = new PizzaStatus();
            currentPizzaStatus.Editor = this.Editor;
            currentPizzaStatus.LineID = this.Line;
            currentPizzaStatus.PizzaID = CurrentPizza.PizzaID;
            currentPizzaStatus.StationID = "MP";

            CurrentPizza.Status = currentPizzaStatus;
            IPizzaRepository currentPizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            currentPizzaRepository.Add(CurrentPizza, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
    }
}
