// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 判断当前Product的Model的Region属性是否合法 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-02-20   Yuan XiaoWei                 create
// 2010-02-20   Yuan XiaoWei                 ITC-1155-0007
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
    /// 判断当前Product的Model的Region属性是否合法 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      
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
    ///                     CHK068
    ///                     CHK099
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
    public partial class CheckRegion : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckRegion()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据Session.Product和workflow中赋值给本Acitvity的Region值
        /// 判断Product对应的Model的Region是否和Workflow中赋值相同
        /// ShowErrWhenEqual=False的时候表示两者必须相等，不等报错
        /// ShowErrWhenEqual=True的时候表示两者必须不等，相等报错
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            int compareResult = Region.CompareTo(currentProduct.ModelObj.Region);
            if (ShowErrWhenEqual)
            {
                if (compareResult == 0)
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(Region);
                    ex = new FisException("CHK099", erpara);
                    if (IsStopWF == IsStopWFEnum.NotStop)
                    {
                        ex.stopWF = false;
                    }
                    throw ex;
                }

            }
            else if (compareResult != 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentProduct.ModelObj.Region);
                erpara.Add(Region);
                ex = new FisException("CHK068", erpara);
                if (IsStopWF == IsStopWFEnum.NotStop)
                {
                    ex.stopWF = false;
                }
                throw ex;
            }



            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// ShowErrWhenEqual
        /// False的时候表示两者必须相等，不等报错
        /// True的时候表示两者必须不等，相等报错
        /// </summary>
        public static DependencyProperty ShowErrWhenEqualProperty = DependencyProperty.Register("ShowErrWhenEqual", typeof(bool), typeof(CheckRegion));

        /// <summary>
        /// ShowErrWhenEqual:True Or False
        /// </summary>
        [DescriptionAttribute("ShowErrWhenEqual")]
        [CategoryAttribute("ShowErrWhenEqual Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
        public bool ShowErrWhenEqual
        {
            get
            {
                return ((bool)(base.GetValue(CheckRegion.ShowErrWhenEqualProperty)));
            }
            set
            {
                base.SetValue(CheckRegion.ShowErrWhenEqualProperty, value);
            }
        }

        /// <summary>
        /// Region
        /// </summary>
        public static DependencyProperty RegionProperty = DependencyProperty.Register("Region", typeof(string), typeof(CheckRegion));

        [DescriptionAttribute("Region")]
        [CategoryAttribute("Region Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Region
        {
            get
            {
                return ((string)(base.GetValue(CheckRegion.RegionProperty)));
            }
            set
            {
                base.SetValue(CheckRegion.RegionProperty, value);
            }
        }


        /// <summary>
        ///  根据设定的Region判断当前Model的Region不正确时，是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(IsStopWFEnum), typeof(CheckRegion));

        /// <summary>
        ///  根据设定的Region判断当前Model的Region不正确时，是否停止工作流，共有两种，Stop，NotStop
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public IsStopWFEnum IsStopWF
        {
            get
            {
                return ((IsStopWFEnum)(base.GetValue(CheckRegion.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckRegion.IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// 根据设定的Region判断当前Model的Region不正确时，是否停止工作流，共有两种，Stop，NotStop
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
