// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判定Customer SN 是否处于强制抽检状态
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-10-28   Kerwin                       create
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
    /// 判定Customer SN 是否处于强制抽检状态 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UI None EPIA to EPIA
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.使用Product ID 查询[ProductAttr] 表，
    ///           是否存在AttrName = ‘ForceEOQC’ and AttrValue=’Yes’ 的记录，
    ///           如果存在则表示该Customer SN 已经处于强制抽检状态
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     CHK155
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
    ///         ProductAttribute
    /// </para> 
    /// </remarks>
    public partial class CheckForceEOQC : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckForceEOQC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判定Customer SN 是否处于强制抽检状态
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得Product对象
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            IList<ProductAttribute> atts = currentProduct.ProductAttributes;
            var productAttrs = from attr in atts
                               where (attr.AttributeName == "ForceEOQC" && attr.AttributeValue == "Yes")
                               select attr;
            List<ProductAttribute> result = productAttrs.ToList();
            if (result != null && result.Count > 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentProduct.ProId);
                ex = new FisException("CHK155", erpara);
                throw ex;
            }                
            return base.DoExecute(executionContext);
        }

    }
}

