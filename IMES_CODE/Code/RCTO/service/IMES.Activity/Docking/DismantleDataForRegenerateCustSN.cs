/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Generate Customer SN For Docking Page
* UI:CI-MES12-SPEC-FA-UI Generate Customer SN For Docking.docx –2012/5/18 
* UC:CI-MES12-SPEC-FA-UC Generate Customer SN For Docking.docx –2012/5/18           
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
using IMES.FisObject.Common.PrintLog;

namespace IMES.Activity
{
    /// <summary>
    /// 检查Product是否有未修护的记录
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Generate Customer SN For Docking
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.生成序号
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK206
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
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
    public partial class DismantleDataForRegenerateCustSN : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DismantleDataForRegenerateCustSN()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查Product是否有未修护的记录
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
           //1 Insert UnpackProduct
            productRepository.BackUpProduct(this.Editor, new Product(currentProduct.ProId),null);

            //2Insert UnpackProduct_Part
            ProductInfo eqCondition = new ProductInfo();
            ProductInfo neqCondition= new ProductInfo();
            eqCondition.ProductID = currentProduct.ProId;
            neqCondition.InfoType = "ECR";
            productRepository.BackUpProductInfo(this.Editor,  eqCondition,  neqCondition);
            productRepository.DeleteProductInfo( eqCondition, neqCondition);

            ProductPart eqPartCondition = new ProductPart();
            ProductPart neqPartCondition= new ProductPart();
            eqPartCondition.ProductID = currentProduct.ProId;
            neqPartCondition.Station = "40";
            productRepository.BackUpProductPart(this.Editor, eqPartCondition, neqPartCondition);
            productRepository.DeleteProductPart(eqPartCondition, neqPartCondition);

            currentProduct.CartonWeight = 0;
            currentProduct.DeliveryNo = string.Empty;
            currentProduct.PalletNo = string.Empty;

            productRepository.Update(currentProduct,CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
    }
}
