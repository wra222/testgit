// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的DeliveryNo
//              判断当前Product的DeliveryNo是否与之相同          
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-10   Yuan XiaoWei                 create
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    /// <summary>
    /// 根据Session中保存的DeliveryNo, 判断当前Product的DeliveryNo是否与之相同  
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于054
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.;
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK069
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    ///         Session.DeliveryNo
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class CheckSameDelivery : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckSameDelivery()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Session中保存的DeliveryNo, 
        /// 判断当前Product的DeliveryNo是否与之相同 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            string currentDeliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);

            if (currentProduct.DeliveryNo != currentDeliveryNo)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentProduct.ProId);
                erpara.Add(currentProduct.DeliveryNo);
                ex = new FisException("CHK069", erpara);
                if (IsStopWF == IsStopWFEnum.NotStop)
                {
                    ex.stopWF = false;
                }
                throw ex;
            }

            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  有FisException时，是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(IsStopWFEnum), typeof(CheckSameDelivery));

        /// <summary>
        ///  有FisException时，是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public IsStopWFEnum IsStopWF
        {
            get
            {
                return ((IsStopWFEnum)(base.GetValue(CheckSameDelivery.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckSameDelivery.IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// 有FisException时，是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public enum IsStopWFEnum
        {
            /// <summary>
            /// 停止WorkFlow
            /// </summary>
            Stop = 1,

            /// <summary>
            /// 不停止WorkFlow
            /// </summary>
            NotStop = 2
        }


    }
}
