// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.StandardWeight;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// UpdateForCombinePalletWithoutCartonForFRU
    /// </summary>
    public partial class UpdateForCombinePalletWithoutCartonForFRU : BaseActivity
    {
        ///<summary>
        ///</summary>
        public UpdateForCombinePalletWithoutCartonForFRU()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.Product
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string dnsn = CurrentSession.GetValue(Session.SessionKeys.DeliveryNo) as string;
            string palletNo = CurrentSession.GetValue(Session.SessionKeys.PalletNo) as string;

            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            int condDeviceQty = 0;
            bool isAllStatus1 = true;
            IList<DeliveryPalletInfo> lstPallets = deliveryRep.GetDeliveryPalletListByDN(dnsn);
            foreach (DeliveryPalletInfo p in lstPallets)
            {
                if (palletNo.Equals(p.palletNo))
                {
                    condDeviceQty = p.deviceQty;
                }
                else
                {
                    if ("0".Equals(p.status))
                    {
                        isAllStatus1 = false;
                    }
                }
            }

            DeliveryPalletInfo dpiCond = new DeliveryPalletInfo(), dpiNew = new DeliveryPalletInfo();
            dpiCond.deliveryNo = dnsn;
            dpiCond.palletNo = palletNo;
            dpiCond.deviceQty = condDeviceQty;

            dpiNew.deliveryNo = dnsn;
            dpiNew.palletNo = palletNo;
            dpiNew.deviceQty = condDeviceQty;
            dpiNew.status = "1";
            dpiNew.editor = this.Editor;
            deliveryRep.UpdateDeliveryPalletInfoDefered(CurrentSession.UnitOfWork, dpiNew, dpiCond);

            if (isAllStatus1)
            {
            //    deliveryRep.UpdateDeliveryStatusDefered(CurrentSession.UnitOfWork, dnsn, "88");
            }

            return base.DoExecute(executionContext);
        }        
       
    }
}
