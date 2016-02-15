/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:BindCoaToProduct
* CI-MES12-SPECFA-UC Combine COA and DN .docx           
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-2-16   207003          (Reference Ebook SourceCode) Create
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
using IMES.FisObject.PAK.COA;
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
    ///     Combine COA and DN
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
    public partial class BindCoaToProduct : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BindCoaToProduct()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 产生BindCoaToProduct
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            ICOAStatusRepository coaRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart part = null;
            string custSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            var product = productRepository.GetProductByCustomSn(custSN);
            if (null == product)
            {
                List<string> errpara = new List<string>();

                errpara.Add(custSN);
                throw new FisException("SFC002", errpara);
            }

            //var product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string model = product.Model;
            // 取ModelBOM 中Model 直接下阶中有BomNodeType 为'P1' 的Part
            IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IHierarchicalBOM sessionBOM = null;
            sessionBOM = ibomRepository.GetHierarchicalBOMByModel(product.Model);
            IList<IBOMNode> bomNodeLst = new List<IBOMNode>();
            bomNodeLst = sessionBOM.FirstLevelNodes;
            if (bomNodeLst != null && bomNodeLst.Count > 0)
            {
                foreach (IBOMNode ibomnode in bomNodeLst)
                {
                    IPart currentPart = ibomnode.Part;
                    if (currentPart.BOMNodeType == "P1" && currentPart.Descr.IndexOf("COA") == 0)
                    {
                        part = currentPart;
                        break;
                    }
                }
            }
            
            string coaSN = (string)CurrentSession.GetValue(Session.SessionKeys.COASN);
           
            if (coaSN != "" && part == null)
            {
                List<string> err = new List<string>();
                err.Add(coaSN);
                err.Add(custSN);
                throw new FisException("CHK187", err);
            }
            else if (coaSN != "" && part != null)
            {

                ICOAStatusRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                IList<COAStatus> coaList = currentRepository.GetCOAStatusRange(coaSN, coaSN);
                if (null == coaList || coaList.Count == 0)
                {
                    List<string> err = new List<string>();
                    err.Add(coaSN);
                    err.Add(custSN);
                    throw new FisException("CHK187", err);
                }
                foreach (COAStatus tmp in coaList)
                {
                    if (part.PN != tmp.IECPN)
                    {
                        List<string> err = new List<string>();
                        err.Add(coaSN);
                        err.Add(custSN);
                        throw new FisException("CHK187", err);
                    }
                }
                IList<ProductPart> parts = new List<ProductPart>();
                parts = productRepository.GetProductPartsByPartSn(coaSN);
                foreach (ProductPart iparts in parts)
                {
                    if (iparts.PartSn == coaSN && iparts.ProductID != product.ProId)
                    {
                        List<string> err = new List<string>();
                        err.Add(coaSN);
                        err.Add(custSN);
                        throw new FisException("CHK187", err);
                    }
                }
               
                // IMES_FA..Product_Part 表中与当前Product 绑定的Parts 
                string coa = "";
                IList<IProductPart> productParts = new List<IProductPart>();
                productParts = ((IProduct)product).ProductParts;
                foreach (ProductPart iprodpart in productParts)
                {
                    IPart curPart = ipartRepository.GetPartByPartNo(iprodpart.PartID);
                    if (curPart.BOMNodeType == "P1" && curPart.Descr.IndexOf("COA") == 0)
                    {
                        if (coa == "" && null != iprodpart.PartSn)
                        {
                            coa = iprodpart.PartSn;
                        }
                    }
                }

                if (coa != "")
                {
                    IList<string> productList = new List<string>();
                    productList.Add(((IProduct)product).ProId);
                    productRepository.BindDN("", productList);
                    List<string> errpara = new List<string>();
                    errpara.Add(coa);
                    throw new FisException("CHK857", errpara);
                }
                else
                {

                    COAStatus reCOA = coaRepository.Find(coaSN);
                    IProductPart bindPart = new ProductPart();
                    bindPart.ProductID = product.ProId;
                    bindPart.PartID = part.PN;
                    bindPart.PartSn = coaSN;
                    bindPart.Cdt = DateTime.Now;
                    bindPart.BomNodeType = "P1";
                    bindPart.PartType = part.Type;
                    bindPart.CustomerPn = part.CustPn;
                    if (this.Editor == null)
                    {
                        Editor = "";
                    }
                    if (this.Station == null)
                    {
                        Station = "";
                    }
                    bindPart.Station = Station;
                    bindPart.Editor = Editor;
                    bindPart.CheckItemType = "";
                    bindPart.Iecpn = "";
                    product.AddPart(bindPart);
                    reCOA.Editor = Editor;
                    reCOA.Status = "A1";
                    coaRepository.UpdateCOAStatusDefered(CurrentSession.UnitOfWork, reCOA);
                    COALog newItem = new COALog();
                    newItem.COASN = coaSN;
                    newItem.LineID = custSN;
                    newItem.Editor = Editor;
                    newItem.StationID = "A1";
                    newItem.Tp = "";
                    coaRepository.InsertCOALogDefered(CurrentSession.UnitOfWork, newItem);
                    productRepository.Update(product, CurrentSession.UnitOfWork);
                }
            }
            

            return base.DoExecute(executionContext);
        }
    }
}
