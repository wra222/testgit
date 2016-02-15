// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 记录到PizzaLog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;

namespace IMES.Activity
{
    /// <summary>
    /// 用于记录到Pizza Log
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
    ///         1.从Session中获取Pizza，调用Pizza的AddLog方法
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
    ///         更新PizzaLog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPizzaRepository
    ///              Pizza
    ///              PizzaLog
    /// </para> 
    /// </remarks>
    public partial class WritePizzaLog : BaseActivity
    {
        private static readonly ILog logger = LogManager.GetLogger("fisLog");

        ///<summary>
        /// 构造函数
        ///</summary>
        public WritePizzaLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(WritePizzaLog));

        /// <summary>
        /// Status
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(WritePizzaLog.StatusProperty)));
            }
            set
            {
                base.SetValue(WritePizzaLog.StatusProperty, value);
            }
        }

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", typeof(bool), typeof(WritePizzaLog));

        /// <summary>
        /// 单条还是成批插入,Session.SessionKeys.PizzaNoList
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("IsSingle Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(WritePizzaLog.IsSingleProperty)));
            }
            set
            {
                base.SetValue(WritePizzaLog.IsSingleProperty, value);
            }
        }

        /// <summary>
        /// Wrint Pizza Log
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (Editor.Trim() == "")
                logger.Error("Editor from activity is empty!");
           
            string line = default(string);
            var PizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository,Pizza>();
            if (IsSingle)
            {
                Pizza pizza = CurrentSession.GetValue(Session.SessionKeys.Pizza) as Pizza;
                if (pizza == null)
                {
                    throw new NullReferenceException("Pizza in session is null");
                }
                line = string.IsNullOrEmpty(this.Line) ? pizza.Status.LineID : this.Line;
                var PizzaLog = new PizzaLog
                {
                    Model = pizza.Model,
                    Editor = Editor,
                    Line = line,
                    Station = Station,
                    Descr="",
                    PizzaID=pizza.PizzaID,
                    Cdt = DateTime.Now
                };

                pizza.AddPizzaLog(PizzaLog);
                PizzaRepository.Update(pizza, CurrentSession.UnitOfWork);
            }
            else
            {
                var pizzaNoList = CurrentSession.GetValue(Session.SessionKeys.PizzaNoList) as List<string>;
                if (pizzaNoList == null)
                {
                    throw new NullReferenceException("PizzaNoList in session is null");
                }
                foreach (var pizzaNo in pizzaNoList)
                {
                    Pizza pizzaObj=PizzaRepository.Find(pizzaNo);

                    line = string.IsNullOrEmpty(this.Line) ? pizzaObj.Status.LineID : this.Line;
                    var PizzaLog = new PizzaLog
                    {
                        Model = pizzaObj.Model,
                        Editor = Editor,
                        Line = line,
                        Station = Station,
                        Descr = "",
                        PizzaID = pizzaObj.PizzaID,
                        Cdt = DateTime.Now
                    };

                    pizzaObj.AddPizzaLog(PizzaLog);
                    PizzaRepository.Update(pizzaObj, CurrentSession.UnitOfWork);
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
