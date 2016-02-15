/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Combine AST Page
* UI:CI-MES12-SPEC-FA-UI Combine AST .docx –2012/7/17 
* UC:CI-MES12-SPEC-FA-UC Combine AST .docx –2012/7/17            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-7-17   Jessica Liu           (Reference Ebook SourceCode) Create
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
    public partial class CheckASTForCombineAST : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckASTForCombineAST()
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
            IProduct currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string currentAST = (string)CurrentSession.GetValue("AST");
            var ProdidOrCustsn = (string)CurrentSession.GetValue("ProdidOrCustsn");
            string trimAST = currentAST.Trim();
            

            FisException ex;
            List<string> erpara = new List<string>();

            if (trimAST.Length == 0)
            {
                erpara.Add(ProdidOrCustsn);
                throw new FisException("CHK917", erpara);   //请输入正确的AST!
            }

            IBOMRepository ibomRepository = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IList<string> PartNoLst = ibomRepository.GetPnListByModelAndBomNodeType(currenProduct.Model, "AT", "ATSN1");           
            string PartNo = "";
            if ((PartNoLst != null) && (PartNoLst.Count != 0))
            {
                PartNo = PartNoLst[0];
            }
            if (string.IsNullOrEmpty(PartNo) == true)
            {
                erpara.Add(currenProduct.ProId);
                ex = new FisException("CHK918", erpara);    //Product：%1 没有带资产标签ATSN1!
                throw ex;
            }
            /* 2012-9-4, Jessica Liu, UC需求变更
            string str2TG = PartNo;

            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            SupplierCodeInfo SCInfo = productRepository.GetSupplierCodeByVendorAndCodeLength(str2TG, currentAST);

            if (SCInfo == null || (string.IsNullOrEmpty(SCInfo.code) == true))
            {
                erpara.Add(str2TG);
                ex = new FisException("CHK919", erpara);    //%1 未维护格式，请与IE联系!
                throw ex;
            }
            string Format = SCInfo.code;

            string upperFormat = Format.ToUpper();
            string upperAST = trimAST.ToUpper();
            for (int i = 0; i < upperFormat.Length; i++)
            {
                if (upperFormat[i] != 'X')   
                {
                    if (upperFormat[i] != upperAST[i])
                    {
                        erpara.Add(trimAST);
                        ex = new FisException("CHK930", erpara);    //AST: 1% 格式错误!
                        throw ex;
                    }
                }
            }
            */
            string[] PNLst = new string[PartNoLst.Count];
            int i = 0;
            foreach (string tempPN in PartNoLst)
            {
                PNLst[i] = tempPN;
                i++;
            }
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            SupplierCodeInfo temp = productRepository.GetSupplierCodeByVendorsAndAstLike(PNLst, currentAST);
            string getVendor = "";
            if (temp != null)
            {
                getVendor = temp.vendor;
            }
            else
            {
                //检查输入的[AST]是否是符合条件的AST，若不符合条件，则报错：“AST：XXX格式错误；或者IE未维护AT1格式”
                List<string> errpara = new List<string>();
                errpara.Add(currentAST);
                throw new FisException("CHK954", errpara);
            }
            CurrentSession.AddValue("Vendor", getVendor);

            return base.DoExecute(executionContext);
        }
    }
}
