// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入Product的指定属性是否有值
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-21   Yuan XiaoWei                 create
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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入Product的指定属性是否有值
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于046等
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK028
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
    ///         
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
    public partial class CheckProductPropertyExist : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProductPropertyExist()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查输入Product的指定属性是否有值
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            bool IsPass = true;
            var currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            switch (PropertyToCheck)
            {
                case PropertyToCheckEnum.CartonSN:
                    if (string.IsNullOrEmpty(currentProduct.CartonSN))
                    {
                        IsPass = false;
                    }
                    break;
                case PropertyToCheckEnum.CartonWeight:
                    if (currentProduct.CartonWeight <= 0)
                    {
                        IsPass = false;
                    }
                    break;
                case PropertyToCheckEnum.MAC:
                    if (string.IsNullOrEmpty(currentProduct.MAC))
                    {
                        IsPass = false;
                    }
                    break;
                case PropertyToCheckEnum.PalletNo:
                    if (string.IsNullOrEmpty(currentProduct.PalletNo))
                    {
                        IsPass = false;
                    }
                    break;
                case PropertyToCheckEnum.UnitWeight:
                    if (currentProduct.UnitWeight <= 0)
                    {
                        IsPass = false;
                    }
                    break;
            }

            if (!IsPass)
            {
                var ex = new FisException("CHK074", new string[] { currentProduct.ProId, Enum.GetName(typeof(PropertyToCheckEnum),PropertyToCheck) });
                throw ex;
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  支持检查的属性
        /// </summary>
        public static DependencyProperty PropertyToCheckProperty = DependencyProperty.Register("PropertyToCheck", typeof(PropertyToCheckEnum), typeof(CheckProductPropertyExist));


        /// <summary>
        ///  支持检查的属性
        /// </summary>
        [DescriptionAttribute("PropertyToCheck")]
        [CategoryAttribute("PropertyToCheck Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public PropertyToCheckEnum PropertyToCheck
        {
            get
            {
                return ((PropertyToCheckEnum)(base.GetValue(CheckProductPropertyExist.PropertyToCheckProperty)));
            }
            set
            {
                base.SetValue(CheckProductPropertyExist.PropertyToCheckProperty, value);
            }
        }

        /// <summary>
        /// 支持检查的属性列表
        /// </summary>
        public enum PropertyToCheckEnum
        {
            /// <summary>
            /// UnitWeight
            /// </summary>
            UnitWeight = 1,

            /// <summary>
            /// CartonWeight
            /// </summary>
            CartonWeight = 2,

            /// <summary>
            /// CartonSN
            /// </summary>
            CartonSN = 4,

            /// <summary>
            /// PalletNo
            /// </summary>
            PalletNo = 8,

            /// <summary>
            /// MAC
            /// </summary>
            MAC = 16
        }
    }
}
