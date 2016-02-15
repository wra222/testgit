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
    public partial class CombineDNNormal : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public CombineDNNormal()
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

            string DN = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            string customerSn = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            string prod = (string)CurrentSession.GetValue(Session.SessionKeys.ProductIDOrCustSN);
            Delivery newDelivery = currentRepository.Find(DN);
            if (newDelivery == null)
            {
                throw new FisException("CHK190", new string[] { DN });//DN不存在
            }
            IProduct currentProduct = null;
            if (customerSn != "")
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
            if (productRepository.GetCombinedQtyByDN(DN) >= newDelivery.Qty)
            {
                IList<DNForUI> ret = new List<DNForUI>();
                IList<Delivery> getList = null;
                getList = currentRepository.GetDeliveryListByModel(newDelivery.ModelName, "00");
                foreach (Delivery tmp in getList)
                {
                    DN = tmp.DeliveryNo;
                    break;
                }
                newDelivery = currentRepository.Find(DN);
                if (productRepository.GetCombinedQtyByDN(DN) >= newDelivery.Qty)
                {
                    throw new FisException("CHK875", new string[] { });
                }
            }
            
            if (currentProduct.DeliveryNo != "" && currentProduct.DeliveryNo != DN)
            {
                Delivery oldDelivery = currentRepository.Find(currentProduct.DeliveryNo);
                if ("96" != this.Station)
                {
                    if (oldDelivery != null)
                    {
                        oldDelivery.Status = "00";
                        currentRepository.Update(oldDelivery, CurrentSession.UnitOfWork);
                    }
                }
            }
            IList<string> productList = new List<string>();
            productList.Add(currentProduct.ProId);
            productRepository.BindDNDefered(CurrentSession.UnitOfWork,DN, productList, newDelivery.Qty);
            int packedQty = productRepository.GetCombinedQtyByDN(DN);
            if (packedQty + 1 == newDelivery.Qty)
            {
               
                if ("96" == this.Station)
                {
                    newDelivery.Status = "87";
                }
                else
                {
                    newDelivery.Status = "87";
                }
                currentRepository.Update(newDelivery, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }

    }
}
