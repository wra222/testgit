// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 用于Pallet，Unit，PizzaID，Carton称重的重量检查
// 从session中获取，StandardWeight，Tolerance，ActuralWeight进行比较
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-03   Yuan XiaoWei                 create
// 2010-06-12   Yuan XiaoWei                 modify ITC-1155-0104
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.Infrastructure;
namespace IMES.Activity
{
    /// <summary>
    /// 用于Pallet，Unit，PizzaID，Carton称重的重量检查
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于UnitWeight,PizzaWeight,CartonWeight,PalletWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从session中获取，StandardWeight，Tolerance，ActuralWeight进行比较
    ///         2.不同BU，重量误差标准不同：含有%表示维护的百分比，没有表示维护的误差多少克
    ///                Tosiba:维护误差的允许范围 正负多少千克
    ///                HP:以前是写死的2%
    ///           需要根据Tolerance的值进行比较
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK016
    ///                     CHK017
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.StandardWeight
    ///         Session.Tolerance
    ///         Session.ActuralWeight
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
    ///         
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class CheckWeight : BaseActivity
    {

        /// <summary>
        /// 
        /// </summary>
        public CheckWeight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 误差有两种情况，百分比和误差值
        /// 根据是否含有%来区别
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {


            String Tolerance = (String)CurrentSession.GetValue(Session.SessionKeys.Tolerance);
            if (CurrentSession.GetValue(Session.SessionKeys.StandardWeight) != null)
            {
                decimal StandardWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.StandardWeight);


                decimal ActuralWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);

                if (Tolerance.Contains("%"))
                {
                    decimal tolerancePercent;
                    if (decimal.TryParse(Tolerance.Replace("%", ""), out tolerancePercent))
                    {
                        if (tolerancePercent < 100 * System.Math.Abs((StandardWeight - ActuralWeight) / StandardWeight))
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(StandardWeight.ToString());
                            erpara.Add(Tolerance.ToString());
                            erpara.Add(ActuralWeight.ToString());
                            ex = new FisException("CHK016", erpara);
                            if (NotStopWF)
                            {
                                ex.stopWF = false;
                            }
                            throw ex;
                        }
                    }
                    else
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(Tolerance);
                        ex = new FisException("CHK017", erpara);
                        throw ex;
                    }

                }
                else
                {
                    decimal toleranceAbs;
                    if (decimal.TryParse(Tolerance, out toleranceAbs))
                    {
                        if (toleranceAbs < System.Math.Abs(StandardWeight - ActuralWeight))
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(StandardWeight.ToString());
                            erpara.Add(Tolerance.ToString());
                            erpara.Add(ActuralWeight.ToString());
                            ex = new FisException("CHK016", erpara);
                            if (NotStopWF)
                            {
                                ex.stopWF = false;
                            }
                            throw ex;
                        }
                    }
                    else
                    {
                        FisException ex;
                        List<string> erpara = new List<string>();
                        erpara.Add(Tolerance);
                        ex = new FisException("CHK017", erpara);
                        throw ex;
                    }
                }
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  有重量异常的时候，是否停止工作流，共有两种，True不停止，false 停止
        /// </summary>
        public static DependencyProperty NotStopWFProperty = DependencyProperty.Register("NotStopWF", typeof(bool), typeof(CheckWeight));

        /// <summary>
        /// NotStopWF:True Or False
        /// </summary>
        [DescriptionAttribute("NotStopWF")]
        [CategoryAttribute("NotStopWF Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool NotStopWF
        {
            get
            {
                return ((bool)(base.GetValue(CheckWeight.NotStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckWeight.NotStopWFProperty, value);
            }
        }
    }
}
