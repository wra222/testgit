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
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UnPackUnpdateDnStatus : BaseActivity
    {
        /// <summary>
        ///
        /// </summary>
        public UnPackUnpdateDnStatus()
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
            string deliveryNo = product.DeliveryNo;

            List<string> erpara = new List<string>();
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            string[] prodList = new string[1];
            prodList[0] = product.ProId;
            ProductPart tmp = new ProductPart();
            tmp.Station = "8C";

            productRep.BackUpProductPart(prodList, tmp, this.Editor);
            productRep.DeleteProductParts(prodList,tmp);

            Delivery CurrentDelivery = DeliveryRepository.Find(deliveryNo);
            if (CurrentDelivery == null)
            {
               // erpara.Add(deliveryNo);
               // throw new FisException("CHK107", erpara);
                return base.DoExecute(executionContext);
            }

            if (CurrentDelivery.Status == "88" || CurrentDelivery.Status == "87" || CurrentDelivery.Status == "00")
            {  
                CurrentDelivery.Status = "00";
                
                CurrentDelivery.Editor = this.Editor;
                CurrentDelivery.Udt = DateTime.Now;

                DeliveryRepository.Update(CurrentDelivery, CurrentSession.UnitOfWork);

            }
        
          
            return base.DoExecute(executionContext);
        }
    }
}
