/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
*/

using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.Common; 

namespace IMES.Activity
{
    /// <summary>
    /// UpdateMaterialCpuStatus
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Online Generate AST
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.生成序号
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class UpdateMaterialCpuStatus : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateMaterialCpuStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// SnSessionKey Property
        /// </summary>
        public static DependencyProperty SnSessionKeyProperty = DependencyProperty.Register("SnSessionKey", typeof(string), typeof(UpdateMaterialCpuStatus));

        /// <summary>
        /// SnSessionKey Property
        /// </summary>
        [DescriptionAttribute("SnSessionKey")]
        [CategoryAttribute("SnSessionKey Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SnSessionKey
        {
            get
            {
                return ((string)(base.GetValue(UpdateMaterialCpuStatus.SnSessionKeyProperty)));
            }
            set
            {
                base.SetValue(UpdateMaterialCpuStatus.SnSessionKeyProperty, value);
            }
        }


        /// <summary>
        /// UpdateAction Property
        /// </summary>
        public static DependencyProperty UpdateActionProperty = DependencyProperty.Register("UpdateAction", typeof(string), typeof(UpdateMaterialCpuStatus));

        /// <summary>
        /// UpdateAction Property
        /// </summary>
        [DescriptionAttribute("UpdateAction")]
        [CategoryAttribute("UpdateAction Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string UpdateAction
        {
            get
            {
                return ((string)(base.GetValue(UpdateMaterialCpuStatus.UpdateActionProperty)));
            }
            set
            {
                base.SetValue(UpdateMaterialCpuStatus.UpdateActionProperty, value);
            }
        }


        /// <summary>
        /// UpdateStatus Property
        /// </summary>
        public static DependencyProperty UpdateStatusProperty = DependencyProperty.Register("UpdateStatus", typeof(string), typeof(UpdateMaterialCpuStatus));

        /// <summary>
        /// UpdateStatus Property
        /// </summary>
        [DescriptionAttribute("UpdateStatus")]
        [CategoryAttribute("UpdateStatus Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string UpdateStatus
        {
            get
            {
                return ((string)(base.GetValue(UpdateMaterialCpuStatus.UpdateStatusProperty)));
            }
            set
            {
                base.SetValue(UpdateMaterialCpuStatus.UpdateStatusProperty, value);
            }
        }


        /// <summary>
        /// DonotThrowExceptionWhenNoCpuSn Property
        /// </summary>
        public static DependencyProperty DonotThrowExceptionWhenNoCpuSnProperty = DependencyProperty.Register("DonotThrowExceptionWhenNoCpuSn", typeof(string), typeof(UpdateMaterialCpuStatus));

        /// <summary>
        /// DonotThrowExceptionWhenNoCpuSn Property
        /// </summary>
        [DescriptionAttribute("DonotThrowExceptionWhenNoCpuSn")]
        [CategoryAttribute("DonotThrowExceptionWhenNoCpuSn Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string DonotThrowExceptionWhenNoCpuSn
        {
            get
            {
                return ((string)(base.GetValue(UpdateMaterialCpuStatus.DonotThrowExceptionWhenNoCpuSnProperty)));
            }
            set
            {
                base.SetValue(UpdateMaterialCpuStatus.DonotThrowExceptionWhenNoCpuSnProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (null == currentProduct)
            {
                FisException fex = new FisException("SFC002", new string[] { this.Key });
                throw fex;
            }

            string sn = "";
            if (string.IsNullOrEmpty(SnSessionKey))
                sn = currentProduct.CVSN;
            else
                sn = CurrentSession.GetValue(SnSessionKey) as string;

            Session session = CurrentSession;
            if ("Y".Equals(DonotThrowExceptionWhenNoCpuSn))
                ActivityCommonImpl.Instance.Material.UpdateMaterialCpuStatus(sn, UpdateAction, UpdateStatus, false, false, ref session);
            else
                ActivityCommonImpl.Instance.Material.UpdateMaterialCpuStatus(sn, UpdateAction, UpdateStatus, true, true, ref session);

            return base.DoExecute(executionContext);
        }
    }
}
