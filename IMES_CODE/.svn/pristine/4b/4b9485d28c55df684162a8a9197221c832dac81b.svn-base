/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: ChangeModelOnly
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 
 * Known issues:Any restrictions about this file 
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
using IMES.FisObject.Common .Process;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Part;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ChangeModelOnly: BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public ChangeModelOnly()
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
            Product currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;
            string ChangeToModel = CurrentSession.GetValue("ChangeToModel") as string;
            if (null != currentProduct && !string.IsNullOrEmpty(ChangeToModel))
            {
				IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
				currentProduct.Model = ChangeToModel;
				productRep.Update((IProduct)currentProduct, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }
    }
}
