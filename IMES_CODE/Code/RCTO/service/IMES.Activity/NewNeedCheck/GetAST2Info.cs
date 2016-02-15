/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/4/9 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/4/9            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-2-28   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-1619, Jessica Liu, 2012-4-9
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
    /// 获得AST2信息以便保存进printlog
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
    ///         ASTInfo
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
    ///         ASTInfo
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
    public partial class GetAST2Info : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetAST2Info()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 获取AST2以便写printlog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string Ast2 = CurrentSession.GetValue("AST2") as string;
    
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);
            /* ITC-1413-0012, Jessica Liu, 2012-6-15
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, Ast2);               
            
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName,"AT");
            //ITC-1360-1619, Jessica Liu, 2012-4-9
            //CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN3");          
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN");
            */
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "AT");         
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, Ast2);
        
            //2012-5-2
            string ASTinfo = (string)CurrentSession.GetValue("ASTInfo");
            if (ASTinfo != "")
            {
                ASTinfo += ", ";
            }
            ASTinfo += Ast2;
            CurrentSession.AddValue("ASTInfo", ASTinfo);

            return base.DoExecute(executionContext);
        }
    }
}
