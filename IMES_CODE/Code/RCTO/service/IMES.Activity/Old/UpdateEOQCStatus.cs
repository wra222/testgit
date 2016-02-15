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
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;


namespace IMES.Activity
{
    public partial class UpdateEOQCStatus : BaseActivity
	{
		public UpdateEOQCStatus()
		{
			InitializeComponent();
		}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            try
            {
                IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                var CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
               
                IList<ProductInfo> infos = null;
                IProduct CurrentForceEOQC = CurrentProductRepository.FindOneProductWithProductIDOrCustSN(this.Key);
                infos = CurrentForceEOQC.ProductInfoes;
                ProductInfo prin = null;
                foreach (ProductInfo info in infos)
                {
                    if (info.InfoType == "ForceEOQC" && info.InfoValue == "0")
                    {
                        prin = info;
                        break;
                    }
                }

                if (prin != null)
                {
                    prin.InfoValue = "1";
                    //CurrentForceEOQC.UpdateStatus(prin); // Update 
                }
                else
                {
                    prin = new ProductInfo();
                    prin.InfoValue = "11";                    
                    //CurrentForceEOQC.AddQCStatus(prin); //Add
                }
                                                                               
                return base.DoExecute(executionContext);
            }
            catch (FisException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
	}
}
