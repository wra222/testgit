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
* ITC-1360-0328,Jessica Liu, 2012-2-11
* ITC-1360-0496, Jessica Liu, 2012-2-28
* ITC-1360-1363, Jessica Liu, 2012-3-10
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
    /// Check CDSI(shell机器)，如果是,则从CDSIAST表中根据ProdID取得资产标签，进行结合，并打印
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Combine AST
    ///      Online AST （由传入的参数DESCR区分）
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.判断是否是CDSI(shell机器)
    ///         2.如果是,则从CDSIAST表中根据ProdID取得资产标签，进行结合，并打印
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK203
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.Product
    ///         ProdidOrCustsn
    ///         DESCR
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
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>

    public partial class CombineAndPrintAST : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CombineAndPrintAST()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Check CDSI(shell机器)，如果是,则从CDSIAST表中根据ProdID取得资产标签，进行结合，并打印
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product getProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            var ProdidOrCustsn = (string)CurrentSession.GetValue("ProdidOrCustsn");
            string descr = (string)CurrentSession.GetValue("DESCR");


            bool isCDSI = getProduct.IsCDSI;

            CurrentSession.AddValue("IsCDSI", isCDSI);

            //2012-5-2
            CurrentSession.AddValue("ASTInfo", "");

            if (isCDSI == true)
            {
                string AST1 = "";
                string AST2 = "";
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                CdsiastInfo cdi = new CdsiastInfo();
                cdi.tp = "ASSET_TAG";
                cdi.snoId = getProduct.ProId;
                IList<CdsiastInfo> cdsiastInfoList = productRepository.GetCdsiastInfoList(cdi);
                if (cdsiastInfoList != null && cdsiastInfoList.Count > 0)
                {
                    AST1 = cdsiastInfoList[0].sno;
                }


                CdsiastInfo cdi2 = new CdsiastInfo();
                cdi2.tp = "ASSET_TAG2";
                cdi2.snoId = getProduct.ProId;
                IList<CdsiastInfo> cdsiastInfoList2 = productRepository.GetCdsiastInfoList(cdi2);
                if (cdsiastInfoList2 != null && cdsiastInfoList2.Count > 0)
                {
                    AST2 = cdsiastInfoList2[0].sno;
                }

                if ((AST1 == "") && (AST2 == ""))
                {
                    List<string> errpara = new List<string>();

                    errpara.Add(ProdidOrCustsn);

                    throw new FisException("CHK203", errpara);
                }
                else
                {
                    //不为空则存入，保存product和Asset SN的绑定关系
                    //保存product和Asset SN的绑定关系
                    //Insert Product_Part values(@prdid,@partpn,@astsn1’’,’AT’,@user,getdate(),getdate())
                    //Insert Product_Part values(@prdid,@partpn,@astsn2’’,’AT’,@user,getdate(),getdate())
                    //注：@partpn 为PartNo in (bom中BomNodeType=’AT’  Descr=’ATSN1’ 对应的Pn)
                    IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    IHierarchicalBOM bom = bomRep.GetHierarchicalBOMByModel(getProduct.Model);

                    IList<IBOMNode> ATList = bom.GetNodesByNodeType("AT");
                    IPart part =null;
                    if (ATList != null)
                    {
                        /* ITC-1360-1363, Jessica Liu, 2012-3-10
                        part = ATList.Where(at => at.Part.Descr == "ATSN3")
                                          .Select(at => at.Part)
                                          .FirstOrDefault<IPart>();
                        */
                        part = ATList.Where(at => at.Part.Descr == descr)
                                          .Select(at => at.Part)
                                          .FirstOrDefault<IPart>();

                    }

                    /* ITC-1360-0328,Jessica Liu, 2012-2-11
                    if(part ==null){
                        List<string> errpara = new List<string>();

                        errpara.Add(ProdidOrCustsn);

                        throw new FisException("CHK203", errpara);
                    }
                    */

                    if (part == null)
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add(ProdidOrCustsn);
                        throw new FisException("CHK205", erpara);
                    }
                    else
                    {
                        //ITC-1360-0496, Jessica Liu, 2012-2-28
                        if (!string.IsNullOrEmpty(AST1))
                        {
                            IProductPart assetTag1 = new ProductPart();
                            assetTag1.BomNodeType = "AT";
                            assetTag1.Iecpn = string.Empty; //part.PN;
                            assetTag1.CustomerPn = string.Empty; //part.CustPn;
                            assetTag1.ProductID = getProduct.ProId;
                            assetTag1.PartID = (part == null) ? "" : part.PN; //part.PN;
                            assetTag1.PartSn = AST1;
                            assetTag1.PartType = descr; //"AT";
                            assetTag1.Station = Station;
                            assetTag1.Editor = Editor;
                            assetTag1.Cdt = DateTime.Now;
                            assetTag1.Udt = DateTime.Now;
                            getProduct.AddPart(assetTag1);
                            productRepository.Update(getProduct, CurrentSession.UnitOfWork);
                        }

                        if (!string.IsNullOrEmpty(AST2))
                        {
                            IProductPart assetTag2 = new ProductPart();
                            assetTag2.BomNodeType = "AT";
                            assetTag2.Iecpn = string.Empty; //part.PN;
                            assetTag2.CustomerPn = string.Empty; //part.CustPn;

                            assetTag2.ProductID = getProduct.ProId;
                            assetTag2.PartID = (part == null) ? "" : part.PN; //part.PN;
                            assetTag2.PartSn = AST2;
                            assetTag2.PartType = descr; //"AT";
                            assetTag2.Station = Station;
                            assetTag2.Editor = Editor;
                            assetTag2.Cdt = DateTime.Now;
                            assetTag2.Udt = DateTime.Now;
                            getProduct.AddPart(assetTag2);
                            productRepository.Update(getProduct, CurrentSession.UnitOfWork);
                        }

                        CurrentSession.AddValue("AST1", AST1);
                        CurrentSession.AddValue("AST2", AST2);

                        //2012-5-2
                        string ASTinfo = "";
                        if (!string.IsNullOrEmpty(AST1) && !string.IsNullOrEmpty(AST2))
                        {
                            ASTinfo += AST1 + ", " + AST2;
                        }
                        else if (!string.IsNullOrEmpty(AST1) && string.IsNullOrEmpty(AST2))
                        {
                            ASTinfo = AST1;
                        }
                        else if (string.IsNullOrEmpty(AST1) && !string.IsNullOrEmpty(AST2))
                        {
                            ASTinfo = AST2;
                        }
                        CurrentSession.AddValue("ASTInfo", ASTinfo);
                    }
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
