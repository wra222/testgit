/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Combine AST Page
* UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2012/7/17 
* UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2012/7/17            
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
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 检查AST是否满足要求
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Combine AST
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
    ///         AST
    ///         ProdidOrCustsn
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IBOMRepository
    ///             
    /// </para> 
    /// </remarks>
    public partial class CheckASTForCombineAST_CheckNeedPart : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckASTForCombineAST_CheckNeedPart()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 检查AST是否满足要求
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

            string needPartType = "ATSN9";
            string needPartNoSessionName = "NeedPartType" + needPartType;
            IPart needPart = null;

            IProduct currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            if (currenProduct.Model.IndexOf("PC") == 0)
            {
                IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
                IList<IBOMNode> PLBomNodeList = bom.GetFirstLevelNodesByNodeType("PL");
                foreach (IBOMNode bomNode in PLBomNodeList)
                {
                    IPart part = bomNode.Part;
                    if (needPartType.Equals(part.Descr))
                    {
                        needPart = part;
                        break;
                    }
                }
            }

            CurrentSession.AddValue(needPartNoSessionName, needPart);

            return base.DoExecute(executionContext);
        }
    }
}
