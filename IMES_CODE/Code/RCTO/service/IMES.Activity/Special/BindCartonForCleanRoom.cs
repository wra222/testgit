/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Bind Carton SN to Product for Clear Room

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
using IMES.Infrastructure.Extend;
namespace IMES.Activity
{  
    /// <summary>
    ///
    /// </summary>
   
    public partial class BindCartonForCleanRoom : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BindCartonForCleanRoom()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
           var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
           string cartonSN =(String)CurrentSession.GetValue(Session.SessionKeys.Carton);
           currentProduct.CartonSN = cartonSN;
           productRepository.Update(currentProduct, CurrentSession.UnitOfWork);
           IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
           IList<string> prodIdList = CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList) as IList<string>;
           if (prodList == null)
           {
               prodList = new List<IProduct>();
               prodIdList = new List<string>();
               CurrentSession.AddValue(Session.SessionKeys.ProdList, prodList);
               CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, prodIdList);
           }
           prodList.Add(currentProduct);
           prodIdList.Add(currentProduct.ProId);
           return base.DoExecute(executionContext);
        }
    }
}
