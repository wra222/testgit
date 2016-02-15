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

using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// Reget Product From Backup
    /// </summary>
    public partial class RegetProductFromBackup : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public RegetProductFromBackup()
		{
			InitializeComponent();
		}
        

        /// <summary>
        /// Reget Product From Backup
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = CurrentSession.GetValue("Backup" + Session.SessionKeys.Product) as IProduct;
            if (currentProduct != null)
            {
                CurrentSession.AddValue(Session.SessionKeys.Product, currentProduct);
            }

            return base.DoExecute(executionContext);
        }
	}
}
