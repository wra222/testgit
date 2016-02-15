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
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// For PrDelete
    /// </summary>
    public partial class WritePrLog : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public WritePrLog()
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
            
            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (product == null)
            {
                throw new NullReferenceException("Product in session is null");
            }
            string model = (string)CurrentSession.GetValue(Session.SessionKeys.MONO);
            string prodid = (string)CurrentSession.GetValue(Session.SessionKeys.ProductIDOrCustSN);
            string partno = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);
            string prsn = (string)CurrentSession.GetValue("PRSN");

            var productLog = new ProductLog
            {
                ProductID = prodid,
                Model = model,
                Station = prsn,
                Status = StationStatus.Pass,
                Line = "PS",
                Editor = Editor,                
                Cdt = DateTime.Now
            };

            ProductLog[] logs = {productLog};
            productRepository.InsertProductLogs(logs);
            //product.AddLog(productLog);
            //productRepository.Update(product, CurrentSession.UnitOfWork);
            
            return base.DoExecute(executionContext);
        }
    }
}

