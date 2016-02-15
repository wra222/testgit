
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
using IMES.DataModel;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    ///
    /// </summary>
    public partial class UpdateProductPartForChangeAST : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public UpdateProductPartForChangeAST()
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
            ASTInfo info = (ASTInfo)CurrentSession.GetValue(Session.SessionKeys.ASTInfoList);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string prod1 = (string)CurrentSession.GetValue(Session.SessionKeys.Prod1);
            string prod2 = (string)CurrentSession.GetValue(Session.SessionKeys.Prod2);


            ProductPart setValue = new ProductPart();
            ProductPart cond = new ProductPart();
            
            setValue.ProductID = prod2;

            cond.ProductID = prod1;
            cond.PartID = info.PartNo;
            cond.BomNodeType = "AT";
            productRepository.UpdateProductPart(setValue, cond);
            
            return base.DoExecute(executionContext);
        }
    }
}

