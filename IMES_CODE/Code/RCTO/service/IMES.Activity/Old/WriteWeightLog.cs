// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 记录到WeightLog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
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
using IMES.FisObject.PAK.WeightLog;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 用于Unit，PizzaID，Carton称重的Log记录，记录到WeightLog表
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于UnitWeight,PizzaWeight,CartonWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.如果是UnitWeight，PizzaWeight，从Session中获取Product，ActuralWeight作为WeightLog的的SN，Weight
    ///         2.如果是CartonWeight，从Session中获取CartonSN，CartonWeight作为WeightLog的的SN，Weight
    ///         3.构建WeightLog对象，保存到WeightLog表
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
    ///         Session.ActuralWeight
    ///         或
    ///         Session.CartonSN
    ///         Session.CartonWeight
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
    ///         WeightLog  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IWeightLogRepository
    ///              WeightLog
    /// </para> 
    /// </remarks>
    public partial class WriteWeightLog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WriteWeightLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据称重类型插入WeightLog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            WeightLog NewWeightLog = new WeightLog();


            if (WeightType == WeightTypeTypeEnum.CartonWeight)
            {
                NewWeightLog.Weight = (decimal)CurrentSession.GetValue(Session.SessionKeys.CartonWeight);
                NewWeightLog.SN = (string)CurrentSession.GetValue(Session.SessionKeys.Carton);
            }
            else
            {
                NewWeightLog.Weight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
                NewWeightLog.SN = Key;
            }

            NewWeightLog.Editor = Editor;
            NewWeightLog.Line = Line;
            NewWeightLog.Station = Station;
            //NewWeightLog.Cdt = DateTime.Now;//由Repostory负责写Cdt

            IWeightLogRepository WeightLogRepository = RepositoryFactory.GetInstance().GetRepository<IWeightLogRepository, WeightLog>();
            WeightLogRepository.Add(NewWeightLog, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 称重的类型，共有四种 UnitWeight = 1,PizzaWeight = 2,CartonWeight = 4,PalletWeight = 8
        /// </summary>
        public static DependencyProperty WeightTypeProperty = DependencyProperty.Register("WeightType", typeof(WeightTypeTypeEnum), typeof(WriteWeightLog));

        /// <summary>
        /// 称重的类型，共有四种 UnitWeight = 1,PizzaWeight = 2,CartonWeight = 4,PalletWeight = 8
        /// </summary>
        [DescriptionAttribute("WeightType")]
        [CategoryAttribute("WeightType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public WeightTypeTypeEnum WeightType
        {
            get
            {
                return ((WeightTypeTypeEnum)(base.GetValue(WriteWeightLog.WeightTypeProperty)));
            }
            set
            {
                base.SetValue(WriteWeightLog.WeightTypeProperty, value);
            }
        }
    }

    /// <summary>
    /// 称重的类型，共有四种 UnitWeight = 1,PizzaWeight = 2,CartonWeight = 4,PalletWeight = 8
    /// </summary>
    public enum WeightTypeTypeEnum
    {
        /// <summary>
        /// 单体称重
        /// </summary>
        UnitWeight = 1,

        /// <summary>
        /// Pizza称重
        /// </summary>
        PizzaWeight = 2,

        /// <summary>
        /// Carton称重
        /// </summary>
        CartonWeight = 4,

        /// <summary>
        /// Pallet称重
        /// </summary>
        PalletWeight = 8
    }
}
