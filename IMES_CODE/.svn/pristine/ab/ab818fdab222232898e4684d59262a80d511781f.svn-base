/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Combine AST Page
* UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2011/12/5 
* UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2011/12/5            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-12-2   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* UC 具体业务：检查是否是 Shell（CDSI） 机器(CI-MES12-SPEC-000-UC Common Rule.docx  2.10判断一个机器是否是CDSI机器（Shell）,加密机型)-UC变更，等待数据接口（in Activity：CombineAndPrintAST，SaveAST）
* UC 具体业务：删除AST标签-等待数据接口（in Activity：DeleteAST） 
* UC 具体业务： select @ast2=Sno from CDSIAST nolock where Tp ='ASSET_TAG2' and SnoId=@prdId；select @ast1=Sno from CDSIAST nolock where Tp ='ASSET_TAG' and SnoId=@prdId-等待数据接口（in Activity：CombineAndPrintAST）
* UC 具体业务：保存product和Asset SN的绑定关系-- Insert Product_Part values(@prdid,@partpn,@astsn,’’,’AT’,@user,getdate(),getdate())注：@partpn 为PartNo in (bom中BomNodeType=’AT’  Descr=’ATSN1’ 对应的Pn)-UC变更，等待数据接口（in Activity：SaveAST）
*/


using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 删除AST的处理
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
    ///         1.删除AST标签
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
    ///              Product
    /// </para> 
    /// </remarks>

    public partial class DeleteAST : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeleteAST()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 删除AST的处理
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {          
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);

            /* 2012-9-4, Jessica Liu, UC需求变更
            var currentPart = (IProductPart)CurrentSession.GetValue("Part");

            currenProduct.RemovePart(currentPart.Value, currentPart.PartID);
            */
            //delete Product_Part where ProductID = '*****' and BomNodeType = 'AT' and PartType = 'ATSN1'
            string[] a = { currenProduct.ProId };
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            var otCond = new IMES.FisObject.Common.Part.ProductPart();
            otCond.BomNodeType = "AT";
            otCond.PartType = "ATSN1";
            productRepository.DeleteProductParts(a, otCond);

            return base.DoExecute(executionContext);
        }
    }
}
