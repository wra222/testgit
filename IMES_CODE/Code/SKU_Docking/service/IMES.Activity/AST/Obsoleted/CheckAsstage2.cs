/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Online Generate AST Page
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* Known issues:
* TODO：
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
    /// Check Asstage-2
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
    public partial class CheckAsstage2 : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckAsstage2()
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

            string isAsstage2 = "N";
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            IList<IBOMNode> bomNodes = bom.GetNodesByNodeType("PP");
            foreach (IBOMNode bomNode in bomNodes)
            {
                if ("Asstage-2".Equals(bomNode.Part.Type))
                {
                    isAsstage2 = "Y";

                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.CUSTSN);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.CUSTSN);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "Asstage-2");
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, bomNode.Part.Descr);

                    break;
                }
            }

            CurrentSession.AddValue("IsAsstage2", isAsstage2);

            return base.DoExecute(executionContext);
        }
    }
}
