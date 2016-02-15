
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
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    ///
    /// </summary>
    public partial class GetASTByProduct : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public GetASTByProduct()
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

            //从Session里取得Product对象
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            //获取Product_Part所有AST信息( Product_Part.BomNodeType = ’AT’)
            IList<ProductPart> list = new List<ProductPart>();
            ProductPart cond = new ProductPart();
            cond.ProductID = currentProduct.ProId;
            cond.BomNodeType = "AT";
            list = productRepository.GetProductPartList(cond);

            IList<ASTInfo> infos = new List<ASTInfo>();

            foreach (ProductPart temp in list)
            {
                ASTInfo item = new ASTInfo();
                item.ASTType = temp.PartType;
                item.PartNo = temp.PartID;
                item.PartSn = temp.PartSn;

                infos.Add(item);
            }
            CurrentSession.AddValue(Session.SessionKeys.ProductPartList, infos);

            
            return base.DoExecute(executionContext);
        }
    }
}

