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
using IMES.FisObject.PAK.Pizza;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProductForPalletDataVerify : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductForPalletDataVerify()
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
            int prodId = (int)CurrentSession.GetValue(Session.SessionKeys.DnIndex);

            IList<IProduct> ProdList = (IList<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList);

            //CurrentSession.AddValue("Data", ProdList[prodId]);
            
            CurrentSession.AddValue(Session.SessionKeys.Product, (Product)ProdList[prodId]);
            prodId++;
            CurrentSession.AddValue(Session.SessionKeys.DnIndex, prodId);
            return base.DoExecute(executionContext);
        }
    }
}

