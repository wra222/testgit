
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
    public partial class CheckP2ForChangeAST : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckP2ForChangeAST()
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
            string prod2 = (string)CurrentSession.GetValue(Session.SessionKeys.Prod2);

           
            IList<ProductPart> list = new List<ProductPart>();
            ProductPart cond = new ProductPart();
            cond.PartID = info.PartNo;
            cond.BomNodeType = "AT";
            cond.ProductID = prod2;
            list = productRepository.GetProductPartList(cond);
            if (list != null && list.Count > 0)
            {
                string[] a = {prod2};
                cond.ProductID = "";
                productRepository.DeleteProductParts(a, cond);
            }
            

            return base.DoExecute(executionContext);
        }
    }
}

