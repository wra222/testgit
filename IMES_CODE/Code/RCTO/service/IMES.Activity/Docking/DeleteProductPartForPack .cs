/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/DeleteProductPartForPack
 * CI-MES12-SPEC-PAK-UC Combine Pack for Docking    
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2012-5-30                  (Reference Ebook SourceCode) Create

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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DeleteProductPartForPack : BaseActivity
    {
        /// <summary>
        ///
        /// </summary>
        public DeleteProductPartForPack()
        {
            InitializeComponent();
        }

        /// <summary>        
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            string[] prodList = new string[1];
            prodList[0] = product.ProId;
            ProductPart part = new ProductPart();
            part.Station = Station;
            productRep.DeleteProductPartsDefered(CurrentSession.UnitOfWork, prodList, part);
          
            return base.DoExecute(executionContext);
        }
    }
}
