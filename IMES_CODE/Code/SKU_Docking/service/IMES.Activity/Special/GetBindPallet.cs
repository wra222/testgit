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
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class GetBindPallet : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetBindPallet()
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
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            Delivery currentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            IList<DeliveryPalletInfo> palletList = new List<DeliveryPalletInfo>();
            IDeliveryRepository DeliveryRepository =
                    RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            palletList = DeliveryRepository.GetDeliveryPalletListOrderByUnitQty(currentDelivery.DeliveryNo);
            foreach (DeliveryPalletInfo temp in palletList)
            {
                if (temp.palletNo == currentProduct.PalletNo)
                {
                    CurrentSession.AddValue(Session.SessionKeys.Pallet, temp.palletNo);
                    return base.DoExecute(executionContext);
                }
            }

            CurrentSession.AddValue(Session.SessionKeys.Pallet, "");
            return base.DoExecute(executionContext);
        }

    }
}

