// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 判断当前Product的Status的WC属性是否合法 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-10   Lucy Liu                 create
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
    /// 判断当前Product的Status的WC属性是否合法  
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      FG Shipping Label Reprint;FG Shipping Label(TRO) Reprint
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.; 判断Product对应的Status的WC是否>=Workflow中赋值,如果否则抛异常
    ///         2.;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK002
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
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckProductStatus : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProductStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Session.Product和workflow中赋值给本Acitvity的WC值
        /// 判断Product对应的Status的WC是否>=Workflow中赋值,如果否则抛异常
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得Product对象
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            if (WC.CompareTo(currentProduct.Status.StationId) > 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                //erpara.Add(currentProduct.Status.StationId);
                //erpara.Add(WC);
                erpara.Add(currentProduct.ProId);
                ex = new FisException("PAK002", erpara);
                throw ex;
            }

            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// WC
        /// </summary>
        public static DependencyProperty WCProperty = DependencyProperty.Register("WC", typeof(string), typeof(CheckProductStatus));

        [DescriptionAttribute("WC")]
        [CategoryAttribute("WC Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string WC
        {
            get
            {
                return ((string)(base.GetValue(CheckProductStatus.WCProperty)));
            }
            set
            {
                base.SetValue(CheckProductStatus.WCProperty, value);
            }
        }


    }
}

