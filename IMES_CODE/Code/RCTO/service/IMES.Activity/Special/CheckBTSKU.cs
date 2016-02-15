// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 判断当前SKU是否是BT
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-01   Kerwin                       create
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
    /// 判断当前SKU是否是BT
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Assign WH Location for BT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.IF EXISTS (SELECT SnoId FROM FA.dbo.FA_SnoBTDet (nolock) WHERE SnoId=@sno1) 
    ///           then BT
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     PAK006
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
    public partial class CheckBTSKU : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBTSKU()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 判断当前SKU是否是BT,如果不是走BT流程的Customer S/N，则需要报告错误：非BT流程机器
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //从Session里取得Product对象
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (!CurrentProduct.IsBT)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(CurrentProduct.ProId);
                ex = new FisException("PAK008", erpara);
                throw ex;
            }
            return base.DoExecute(executionContext);
        }

    }
}

