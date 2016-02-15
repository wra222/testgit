/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: ChangeProductBTOnly
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 
 * Known issues:Any restrictions about this file 
 */


using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.FisObject.Common.Process;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Part;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ChangeProductBTOnly : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public ChangeProductBTOnly()
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
            string isBT = CurrentSession.GetValue("isBT") as string;
            if ("Y".Equals(isBT))
            {
                Product currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as Product;
                ProductBTInfo bt = new ProductBTInfo();
                bt.productID = currentProduct.ProId;
                bt.bt = "BT" + DateTime.Now.ToString("yyyyMMdd");
                bt.editor = Editor;
                bt.cdt = DateTime.Now;
                bt.udt = DateTime.Now;

                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                productRep.InsertProductBTDefered(CurrentSession.UnitOfWork, bt);
            }
            return base.DoExecute(executionContext);
        }
    }
}
