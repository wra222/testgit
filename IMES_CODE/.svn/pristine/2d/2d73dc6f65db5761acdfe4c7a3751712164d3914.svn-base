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
using IMES.Common; 

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
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;

            IProduct currenProduct = utl.IsNull<IProduct>(session, Session.SessionKeys.Product);
            IList<IPart> needGenAstPartList = utl.IsNull<IList<IPart>>(session, Session.SessionKeys.NeedGenAstPartList);
            //var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            var currentAST = (string)session.GetValue("AssetSN");
            var currentAST3 = (string)session.GetValue("AssetSN3");
            //var ProdidOrCustsn = (string)CurrentSession.GetValue("ProdidOrCustsn");
            var ProdidOrCustsn = currenProduct.ProId + "/" + currenProduct.CUSTSN;

            //Jessica Liu, 2012-4-16
            //string descr = (string)session.GetValue("DESCR");

            IPart part = null;

            //IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(currenProduct.Model);
            //for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
            //{
            //    IPart tempPart = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
            //    //Jessica Liu, 2012-4-16
            //    //if ((tempPart.BOMNodeType == "AT") && (tempPart.Descr == "ATSN3"))
            //    if ((tempPart.BOMNodeType == "AT") && (tempPart.Descr == descr))
            //    {
            //        part = tempPart;
            //        break;
            //    }
            //}
            if (needGenAstPartList.Count > 0)
            {
                part = needGenAstPartList.FirstOrDefault();
            }

            if (part == null)
            {
                throw new FisException("CHK205", new List<string>{ProdidOrCustsn} );
            }
            else
            {
                string descr = part.Descr;
                IProductPart assetTag1 = new ProductPart();
                assetTag1.BomNodeType = part.BOMNodeType; //"AT";
                assetTag1.Iecpn = string.Empty; //part.PN;
                assetTag1.CustomerPn = part.GetAttribute("AV")??string.Empty; //part.CustPn;
                assetTag1.ProductID = currenProduct.ProId;
                assetTag1.PartID = part.PN;
                assetTag1.PartSn = currentAST;
                //Jessica Liu, 2012-4-16
                //assetTag1.PartType = "ATSN3"; //"AT";
                assetTag1.PartType = descr;
                assetTag1.Station = Station;
                assetTag1.Editor = Editor;
                assetTag1.CheckItemType = "GenASTSN";
                assetTag1.Cdt = DateTime.Now;
                assetTag1.Udt = DateTime.Now;
                currenProduct.AddPart(assetTag1);

                if (!string.IsNullOrEmpty(currentAST3))
                {
                    IProductPart assetTag3 = new ProductPart();
                    assetTag3.BomNodeType = part.BOMNodeType; //"AT";
                    assetTag3.Iecpn = string.Empty; //part.PN;
                    assetTag3.CustomerPn = part.GetAttribute("AV") ?? string.Empty; //part.CustPn;
                    assetTag3.ProductID = currenProduct.ProId;
                    assetTag3.PartID = part.PN;
                    assetTag3.PartSn = currentAST3;
                    
                    assetTag3.PartType = descr;
                    assetTag3.Station = this.Station;   //"AT3";//Station;
                    assetTag3.Editor = Editor;
                    assetTag3.Cdt = DateTime.Now;
                    assetTag3.Udt = DateTime.Now;
                    assetTag3.CheckItemType = "GenAST3SN";
                    currenProduct.AddPart(assetTag3);
                }
                productRepository.Update(currenProduct, session.UnitOfWork);
            }

            return base.DoExecute(executionContext);
        }
    }
}
