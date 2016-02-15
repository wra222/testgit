/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2011/11/21 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2011/11/21            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* , Jessica Liu, 2012-4-16
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
    /// 绑定AST
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
    ///         AssetSN
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
    ///              FisBOM
    ///              NumControl
    ///              Part
    /// </para> 
    /// </remarks>
    public partial class BindAST : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BindAST()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 产生Asset SN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            var currentAST = (string)CurrentSession.GetValue("AssetSN");
            var ProdidOrCustsn = (string)CurrentSession.GetValue("ProdidOrCustsn");

            //Jessica Liu, 2012-4-16
            string descr = (string)CurrentSession.GetValue("DESCR");

            IPart part = null;

            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
            {
                IPart tempPart = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                //Jessica Liu, 2012-4-16
                //if ((tempPart.BOMNodeType == "AT") && (tempPart.Descr == "ATSN3"))
                if ((tempPart.BOMNodeType == "AT") && (tempPart.Descr == descr))
                {
                    part = tempPart;
                    break;
                }
            }

            if (part == null)
            {
                List<string> erpara = new List<string>();
                erpara.Add(ProdidOrCustsn);
                throw new FisException("CHK205", erpara);
            }
            else
            {
                IProductPart assetTag1 = new ProductPart();
                assetTag1.BomNodeType = "AT";
                assetTag1.Iecpn = string.Empty; //part.PN;
                assetTag1.CustomerPn = string.Empty; //part.CustPn;
                assetTag1.ProductID = currenProduct.ProId;
                assetTag1.PartID = part.PN;
                assetTag1.PartSn = currentAST;
                //Jessica Liu, 2012-4-16
                //assetTag1.PartType = "ATSN3"; //"AT";
                assetTag1.PartType = descr;
                assetTag1.Station = Station;
                assetTag1.Editor = Editor;
                assetTag1.Cdt = DateTime.Now;
                assetTag1.Udt = DateTime.Now;
                currenProduct.AddPart(assetTag1);
                productRepository.Update(currenProduct, CurrentSession.UnitOfWork);
            }

            return base.DoExecute(executionContext);
        }
    }
}
