// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-13   itc207013                    create
// Known issues:
using System;
using System.Collections.Generic;
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
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.PAK.Pizza;
using IMES.DataModel;
using IMES.FisObject.FA.Product;


namespace IMES.Activity 
{
    /// <summary>
    /// InsertPakSkuMasterWeightFis
    /// </summary>
    public partial class InsertPakSkuMasterWeightFis : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InsertPakSkuMasterWeightFis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// InsertPakSkuMasterWeightFis
        /// input parameters:model dn.
        /// via product object, can be changed if needed
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            //???
            Product currentProduct = (Product) CurrentSession.GetValue(Session.SessionKeys.Product);
            string productModel = currentProduct.Model;
            string productDn = currentProduct.DeliveryNo;
            IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            decimal weightget = repPizza.GetAverageWeightFromPakSkuMasterWeightFis(productModel);

            if (weightget == 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(productDn);
                erpara.Add(productModel);
                ex = new FisException("PAK041", erpara);
                throw ex;
            }
            var item = new PakSkuMasterWeightFisInfo { model = productModel, weight = weightget, cdt = DateTime.Now };
            repPizza.InsertPakSkuMasterWeightFis(item);
            return base.DoExecute(executionContext);
        }
    }
}
