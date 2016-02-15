/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: check whether dn combine product
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2011-05-05   201032     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{

    public partial class CheckDNCombineProduct : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckDNCombineProduct()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 检查dn是否与机器结合了
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            CurrentSession.AddValue(Session.SessionKeys.DNCombineProduct, false);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string inputNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            IList<IProduct> lstProduct = productRepository.GetProductListByDeliveryNo(inputNo);
            if (lstProduct != null && lstProduct.Count != 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.DNCombineProduct, true);
                List<string> productIDList = new List<string>();
                List<string> cartonSNList = new List<string>();
                List<string> productCustSNList = new List<string>();
                foreach (var product in lstProduct)
                {
                    productIDList.Add(product.ProId);
                    cartonSNList.Add(product.CartonSN);
                    //collect customer sn
                    productCustSNList.Add(product.CUSTSN);

                    
                }
                CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, productIDList);
                CurrentSession.AddValue(Session.SessionKeys.CartonSNList, cartonSNList);
                CurrentSession.AddValue(Session.SessionKeys.NewScanedProductCustSNList, productCustSNList);
               
            }
           
            
            return base.DoExecute(executionContext);
        }
    }
}
