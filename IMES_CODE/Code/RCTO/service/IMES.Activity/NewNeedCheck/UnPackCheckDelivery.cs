/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckDeliveryForPackingList
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-20                  (Reference Ebook SourceCode) Create

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
    public partial class UnPackCheckDelivery : BaseActivity
    {
        /// <summary>
        ///
        /// </summary>
        public UnPackCheckDelivery()
        {
            InitializeComponent();
        }

        /// <summary>        
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            bool bFlag = false;
            string deliveryNo = product.DeliveryNo;

            List<string> erpara = new List<string>();
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();


            Delivery CurrentDelivery = DeliveryRepository.Find(deliveryNo);
            if (CurrentDelivery == null)
            {
                erpara.Add(deliveryNo);
                throw new FisException("CHK107", erpara);
            }

            int qty = CurrentDelivery.Qty;
            string ret_code = "0";
            int prd_qty = productRep.GetCombinedQtyByDN(deliveryNo);
            bool isPallet = ((bool)CurrentSession.GetValue(Session.SessionKeys.Pallet));
            if (!isPallet){
                prd_qty = prd_qty - 1;
            }
            bFlag = CurrentDelivery.IsDNFull(00);
            if (CurrentDelivery.Status == "98")
            {
                ret_code = "1";
            }
            else {
                ret_code = "0";
            }

            
             if (qty > prd_qty)
             {
                 CurrentDelivery.Status = "00";
             }
             else
             {
                 CurrentDelivery.Status = "87";
             }

             CurrentDelivery.Editor = this.Editor;
             CurrentDelivery.Udt = DateTime.Now;

             DeliveryRepository.Update(CurrentDelivery, CurrentSession.UnitOfWork);
            
             
            CurrentSession.AddValue(Session.SessionKeys.DCode, ret_code);
            return base.DoExecute(executionContext);
        }
    }
}
