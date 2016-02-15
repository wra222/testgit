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
    public partial class CheckASTForCombineAST_CheckCombinePart : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckASTForCombineAST_CheckCombinePart()
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
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            string needPartType = "ATSN9";
            string needPartNoSessionName = "NeedPartType" + needPartType;
            IPart needPart = CurrentSession.GetValue(needPartNoSessionName) as IPart;

            if (null != needPart)
            {
                IProduct currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
				
				IProductPart assetTag1 = new ProductPart();
                assetTag1.BomNodeType = needPart.BOMNodeType;
                assetTag1.Iecpn = string.Empty; //part.PN;
                assetTag1.CustomerPn = string.Empty; //part.CustPn;
                assetTag1.ProductID = currenProduct.ProId;
                assetTag1.PartID = needPart.PN;
                assetTag1.PartSn = needPart.PN;
                assetTag1.PartType = needPart.Descr;
                assetTag1.Station = Station;
                assetTag1.Editor = Editor;
                assetTag1.Cdt = DateTime.Now;
                assetTag1.Udt = DateTime.Now;
                currenProduct.AddPart(assetTag1);
                productRepository.Update(currenProduct, CurrentSession.UnitOfWork);            }

            return base.DoExecute(executionContext);
        }
    }
}
