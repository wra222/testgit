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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    public partial class GetCustSN : BaseActivity
	{
		public GetCustSN()
		{
			InitializeComponent();
		}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.InfoFormat("GetProductActivity: Key: {0}", this.Key);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //var currentProduct = productRepository.Find(this.Key);
            //var currentProductCustSN = productRepository.FindOneProductWithProductIDOrCustSN(this.Key);
            var product = productRepository.GetProductByCustomSn(this.Key);

            if (product == null)
            {
                List<string> errpara = new List<string>();

                errpara.Add(this.Key);

                throw new FisException("CHK152", errpara);
            }
            //logger.InfoFormat("GetProductActivity: IProduct Hash: {0}; IProduct Key: {1}", currentProduct.GetHashCode().ToString(), currentProduct.Key);
            
            CurrentSession.AddValue(Session.SessionKeys.CustSN, this.Key);
            //add by sheng-ju for FRU 20110907
            CurrentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN,product.ProId);
            CurrentSession.AddValue(Session.SessionKeys.Product, product);

            //logger.InfoFormat("GetProductActivity: CurrentSession Hash: {0}; CurrentSession Key: {1}; CurrentSession Type: {2}", CurrentSession.GetHashCode().ToString(), CurrentSession.Key, CurrentSession.Type.ToString());
            
            return base.DoExecute(executionContext);
        }
	}
}
