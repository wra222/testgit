/*
* INVENTEC corporation ©2011 all rights reserved. 
* Description:Combine Po in Carton
* UI:CI-MES12-SPEC-PAK-UI Combine Po in Carton.docx –2012/05/21 
* UC:CI-MES12-SPEC-PAK-UC Combine Po in Carton.docx –2012/05/21          
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-05-21   207003               Create   
* Known issues:
* TODO：
* 
*/
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.MO;
using carton = IMES.FisObject.PAK.CartonSSCC;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Carton NO
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         更新Product.CUSTSN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
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
    ///         Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IProduct
    ///         IProductRepository
    /// </para> 
    /// </remarks>
    public partial class CombineDNBefore : BaseActivity
	{
         
        /// <summary>
        /// 构造函数
        /// </summary>
        public CombineDNBefore()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        { 
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            string DN = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            string customerSn = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            string prod = (string)CurrentSession.GetValue(Session.SessionKeys.ProductIDOrCustSN);
            IProduct currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product); ;
             /* if (customerSn != "")
            {
                currentProduct = productRepository.GetProductByCustomSn(customerSn);
            }
            else if (prod != "")
            {
                currentProduct = productRepository.Find(prod);
            }
            if (null == currentProduct)
            {
                List<string> errpara = new List<string>();
                if (customerSn != "")
                {
                    errpara.Add(customerSn);
                }
                else if (prod != "")
                {
                    errpara.Add(prod);
                }
                throw new FisException("SFC002", errpara);
            }
          IMES.FisObject.FA.Product.ProductStatus newStatus = new IMES.FisObject.FA.Product.ProductStatus();
            newStatus.Status = "1";
            newStatus.StationId = this.Station;
            newStatus.Udt = DateTime.Now;
            string[] prodIds = new string[1];
            prodIds[0] = prod;
            productRepository.UpdateProductStatusesDefered(CurrentSession.UnitOfWork, newStatus, prodIds);

            ProductStatusInfo statusIfo = productRepository.GetProductStatusInfo(prod);
            var productLog = new ProductLog
            {
                ProductID = prod,
                Model = currentProduct.Model,
                Station = this.Station,
                Status = StationStatus.Pass,
                Line = statusIfo.pdLine,
                Editor = this.Editor,
                Cdt = DateTime.Now
            };
            ProductLog[] logs = { productLog };
            productRepository.InsertProductLogsDefered(CurrentSession.UnitOfWork, logs);*/
            if ("96" == this.Station)
            {

            }
            else
            {
                string ckk = "";
                bool retJapan = modelRep.CheckExistModelInfo("PN", currentProduct.Model, "#ABJ");
                if (retJapan == true)
                {
                    ckk = "1";
                }
                else
                {
                    ckk = "0";
                }
                bool have = productRepository.CheckExistProductInfo(prod, "CKK");
                if (have)
                {
                    IMES.FisObject.FA.Product.ProductInfo setValue = new IMES.FisObject.FA.Product.ProductInfo();
                    IMES.FisObject.FA.Product.ProductInfo condition = new IMES.FisObject.FA.Product.ProductInfo();
                    setValue.InfoValue = ckk;
                    setValue.Editor = this.Editor;
                    setValue.Udt = DateTime.Now;
                    condition.ProductID = prod;
                    condition.InfoType = "CKK";
                    productRepository.UpdateProductInfoDefered(CurrentSession.UnitOfWork, setValue, condition);
                }
                else
                {
                    IMES.FisObject.FA.Product.ProductInfo newValue = new IMES.FisObject.FA.Product.ProductInfo();
                    newValue.ProductID = prod;
                    newValue.InfoType = "CKK";
                    newValue.InfoValue = ckk;
                    newValue.Editor = this.Editor;
                    newValue.Udt = DateTime.Now;
                    newValue.Cdt = DateTime.Now;
                    productRepository.InsertProductInfoDefered(CurrentSession.UnitOfWork, newValue);
                }
            }
            CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);
            return base.DoExecute(executionContext);
        }
    }
}
