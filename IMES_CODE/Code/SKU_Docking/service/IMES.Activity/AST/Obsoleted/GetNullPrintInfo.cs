/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/4/6 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/4/6            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-4-6   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1413-0012, Jessica Liu, 2012-6-15
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Linq; 

namespace IMES.Activity
{
    /// <summary>
    /// 获得空AST信息以便保存进printlog
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
    ///         Session.SessionKeys.Product
    ///         AST2
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.SessionKeys.PrintLogBegNo
    ///         Session.SessionKeys.PrintLogEndNo
    ///         Session.SessionKeys.PrintLogName
    ///         Session.SessionKeys.PrintLogDescr
    ///         HasTwoAST
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              FisBOM
    ///              NumControl
    ///              Part
    /// </para> 
    /// </remarks>
    public partial class GetNullPrintInfo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetNullPrintInfo()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 获取空AST以便写printlog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
    
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);
            /* ITC-1413-0012, Jessica Liu, 2012-6-15
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, "");               
            
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName,"AT");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN");    
            */
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "AT");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "");  

            return base.DoExecute(executionContext);
        }
    }
}
