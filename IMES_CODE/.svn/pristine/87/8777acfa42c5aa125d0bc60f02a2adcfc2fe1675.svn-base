// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据Session中保存的FirstProductModel
//              判断当前Product的Model是否与之相同          
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
    /// 比较Model是否和第一个的Model相同
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于Combine PO in Carton,VirtualCarton
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
    ///                     CHK015
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    ///         Session.FirstProductModel  or Session.Delivery
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
    ///         Delivery
    /// </para> 
    /// </remarks>
    public partial class CheckSameModel : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckSameModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据比较的选项
        /// 判断当前Product的Model是否和第一个Product的Model相同
        /// 或与选择的Delivery的Model相同
        /// 第一个Product的Model保存在Session.SessionKeys.FirstProductModel
        /// Delivery的Model相同在Session.Delivery
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            if (CompareWith == CompareWithEnum.FirstProductModel)
            {
                string FirstProductModel = (string)CurrentSession.GetValue(Session.SessionKeys.FirstProductModel);

                if (CurrentProduct.Model != FirstProductModel)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(CurrentProduct.Model);
                    erpara.Add(FirstProductModel);
                    ex = new FisException("CHK015", erpara);
                    ex.stopWF = false;
                    throw ex;
                }
            }
            else
            {
                string DeliveryModel = ((Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery)).ModelName;

                if (CurrentProduct.Model != DeliveryModel)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(CurrentProduct.Model);
                    erpara.Add(DeliveryModel);
                    ex = new FisException("CHK056", erpara);
                    ex.stopWF = false;
                    throw ex;
                }
            }


            return base.DoExecute(executionContext);
        }


        /// <summary>
        /// 比较的选项，共有两种，FirstProductModel，DeliveryModel
        /// </summary>
        public static DependencyProperty CompareWithProperty = DependencyProperty.Register("CompareWith", typeof(CompareWithEnum), typeof(CheckSameModel));

        /// <summary>
        /// 比较的选项，共有两种，FirstProductModel，DeliveryModel
        /// </summary>
        [DescriptionAttribute("CompareWith")]
        [CategoryAttribute("CompareWith Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(CompareWithEnum.FirstProductModel)]
        public CompareWithEnum CompareWith
        {
            get
            {
                return ((CompareWithEnum)(base.GetValue(CheckSameModel.CompareWithProperty)));
            }
            set
            {
                base.SetValue(CheckSameModel.CompareWithProperty, value);
            }
        }


        /// <summary>
        /// 比较的选项，共有两种，FirstProductModel，DeliveryModel
        /// </summary>
        public enum CompareWithEnum
        {

            /// <summary>
            /// 与扫入的第一个Product的Model相比
            /// </summary>
            FirstProductModel = 1,

            /// <summary>
            /// 与选择的Delivery的Model相比
            /// </summary>
            DeliveryModel = 2
        }
    }
}
