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
    /// CheckMaterialCpuStatus
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
    public partial class CheckMaterialCpuStatus : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMaterialCpuStatus()
        {
            InitializeComponent();
        }

        /// <summary>
        /// SnSessionKey Property
        /// </summary>
        public static DependencyProperty SnSessionKeyProperty = DependencyProperty.Register("SnSessionKey", typeof(string), typeof(CheckMaterialCpuStatus));

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
                return ((string)(base.GetValue(CheckMaterialCpuStatus.SnSessionKeyProperty)));
            }
            set
            {
                base.SetValue(CheckMaterialCpuStatus.SnSessionKeyProperty, value);
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
            if (ActivityCommonImpl.Instance.Material.NeedCheckMaterialCpuStatus(currentProduct, sn, ref session))
            {
                CurrentSession.AddValue("CheckMaterialCpuStatus", "Y");
            }
            else
            {
                CurrentSession.AddValue("CheckMaterialCpuStatus", "N");
            }

            return base.DoExecute(executionContext);
        }
    }
}
