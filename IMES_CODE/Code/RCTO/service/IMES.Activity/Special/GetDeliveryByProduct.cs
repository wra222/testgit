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

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GetDeliveryByProduct : BaseActivity
    {
        /// <summary>
        ///
        /// </summary>
        public GetDeliveryByProduct()
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

            //3C.
            if (CurrentProduct.DeliveryNo == "")
            {
                List<string> errpara = new List<string>();                
                throw new FisException("CHK823", errpara);
            }
            string deliveryNo = (string)CurrentProduct.DeliveryNo;
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery CurrentDelivery = DeliveryRepository.Find(deliveryNo);
            if (CurrentDelivery == null)
            {
                List<string> errpara = new List<string>();                
                throw new FisException("CHK823", errpara);
            }
            
            CurrentSession.AddValue(Session.SessionKeys.Delivery, CurrentDelivery);
            CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, CurrentDelivery.DeliveryNo);
            CurrentSession.AddValue("Data", CurrentDelivery.DeliveryNo);
            return base.DoExecute(executionContext);
        }
    }
}
