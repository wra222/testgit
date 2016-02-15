/*
 * INVENTEC corporation ©2011 all rights reserved.          
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================

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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckProdForMVunpack : BaseActivity
    {
        /// <summary>
        ///
        /// </summary>
        public CheckProdForMVunpack()
        {
            InitializeComponent();
        }

        /// <summary>        
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            FisException ex;
            if (string.IsNullOrEmpty(CurrentProduct.ProId))
            {
                //无需打印
                List<string> erpara = new List<string>();
                //erpara.Add("CustSN：XXX不存在");
                erpara.Add(CurrentProduct.CUSTSN);
                ex = new FisException("CHK936", erpara);
                throw ex;

            }

            if (!string.IsNullOrEmpty(CurrentProduct.DeliveryNo))
            {
                //无需打印
                List<string> erpara = new List<string>();
                //erpara.Add("已经结合船务，请先解掉船务");
                ex = new FisException("CHK937", erpara);
                throw ex;

            }

            if (CurrentProduct.Status.Status == 0) {
                //无需打印
                List<string> erpara = new List<string>();
                //erpara.Add("CustSN：XXX存在不良，请先去修护");
                erpara.Add(CurrentProduct.CUSTSN);
                ex = new FisException("CHK938", erpara);
                throw ex;
            }
            //string[] tps = new string[1];
            IList<ProductQCStatus> QCStatusList = new List<ProductQCStatus>();
            QCStatusList = repProduct.GetQCStatusOrderByUdt(CurrentProduct.ProId, null);
            if (QCStatusList != null && QCStatusList.Count > 0)
            {
                ProductQCStatus qcStatus = QCStatusList[0];
                if (!(qcStatus.Status == "3" || qcStatus.Status == "2" || qcStatus.Status == "5" || qcStatus.Status == "6"))
                {
                    //errpara.Add("非抽检机器，不能Unpack");
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK939", erpara);
                    throw ex;
                }
            }


            return base.DoExecute(executionContext);
        }
    }
}
