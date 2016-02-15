// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 判断当前Product的Status的station属性状态 
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
    public partial class CheckProductStation : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckProductStation()
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

            if (String.IsNullOrEmpty(currentProduct.Status.StationId))
            {
                FisException ex;
                List<string> erpara = new List<string>();
                ex = new FisException("CHK809", erpara);
                throw ex;
            }
            if(currentProduct.Status.StationId == "99")
            {
                FisException ex;
                List<string> erpara = new List<string>();                
                ex = new FisException("CHK810", erpara);
                throw ex;
            }
            else
            {
            }

            return base.DoExecute(executionContext);
        }
    }
}

