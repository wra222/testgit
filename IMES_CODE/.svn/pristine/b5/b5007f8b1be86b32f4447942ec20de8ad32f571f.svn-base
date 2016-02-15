// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 清空product的PRSN属性 
//                   
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// For PrDelete
    /// </summary>
    public partial class UpdatePrSN : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public UpdatePrSN()
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
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            string prodid = (string)CurrentSession.GetValue(Session.SessionKeys.ProductIDOrCustSN);
            string partno = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);

            if(!String.IsNullOrEmpty(prodid) && !String.IsNullOrEmpty(partno))
                productRepository.DeleteProductPartByPartNo(prodid, partno);
            
            return base.DoExecute(executionContext);
        }
    }
}

