
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
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{

    /// <summary>
    /// 
    /// </summary>
    public partial class GetProductListByCarton : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public GetProductListByCarton()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string cartonNo = (string)CurrentSession.GetValue(Session.SessionKeys.Carton);

            IList<IProduct> prodList = new List<IProduct>();
            prodList = productRepository.GetProductListByCartonNo(cartonNo);


            CurrentSession.AddValue(Session.SessionKeys.ProdList, prodList);

            return base.DoExecute(executionContext);
        }
    }
}