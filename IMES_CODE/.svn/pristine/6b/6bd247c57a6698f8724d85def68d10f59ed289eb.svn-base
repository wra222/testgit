// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据输入的MaterialCT,获取Material对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2014-03-01  Vincent
// Known issues:
using System;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.ComponentModel;
using IMES.FisObject.Common.Material;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
namespace IMES.Activity
{
    /// <summary>
    /// UpdateDn87Status
    /// </summary>
    public partial class UpdateDn88Status : BaseActivity
    {
        ///<summary>
        ///UpdateDnStatus
        ///</summary>
        public UpdateDn88Status()
        {
            InitializeComponent();
        }

        /// <summary>
        /// UpdateDnStatus and Check DN Qty
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
             string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
             string cartonSN = (string)CurrentSession.GetValue(Session.SessionKeys.CartonSN);
             int qty =(int)(CurrentSession.GetValue(Session.SessionKeys.Qty)??0);             
             string category = (string)CurrentSession.GetValue(Session.SessionKeys.RCTO146Category);
            
             IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
             IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
             IMaterialRepository materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
             IMaterialBoxRepository materialBoxRep = RepositoryFactory.GetInstance().GetRepository<IMaterialBoxRepository>();
             IPalletRepository pltRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();

            IList<CombinedPalletCarton> palletCartonQty =null;
            string pltModeType = "";
             Delivery dn = dnRep.QueryAsLockDnForCombineCOAandDN_OnTrans(deliveryNo);
             if (dn == null)
             {
                 throw new FisException("CQCHK0019", new string[] { "Delivery", deliveryNo });
             }
            
             CurrentSession.AddValue(Session.SessionKeys.Delivery,dn);

            IList<DeliveryPalletInfo> dnPalletList=  dnRep.GetDeliveryPalletListByDNOnTrans(deliveryNo);
            if (dnPalletList ==null || dnPalletList.Count==0)
            {
                throw new FisException("CQCHK0019", new string[] { "Delivery_Pallet", deliveryNo });
            }


             if (category == "MBCT")
             {
                 pltModeType = "146MBCT";
                palletCartonQty= mbRep.GetCartonQtywithCombinedPallet(deliveryNo);
             }
             else if (category == "MaterialCT")
             {
                 pltModeType = "146PartCT";
                 palletCartonQty = materialRep.GetCartonQtywithCombinedPallet(deliveryNo);
             }
             else if (category == "NoMaterialCT")
             {
                 pltModeType = "146NoneCT";
                 palletCartonQty = materialBoxRep.GetCartonQtywithCombinedPallet(deliveryNo);
             }
             else
             {
                 //throw wrong CRTO146 Category
                 throw new FisException("CQCHK0006", new string[] { Session.SessionKeys.RCTO146Category });
             }

            bool fullPallet;
            bool fullCartonsInPallet;
            string palletNo = assignPalletNo(palletCartonQty, dnPalletList, out fullPallet, out fullCartonsInPallet);
            if (fullPallet)
            {
                if (!string.IsNullOrEmpty(palletNo))
                {
                    dnRep.UpdateDeliveryStatusDefered(CurrentSession.UnitOfWork, deliveryNo, "88");
                }
                else
                {
                    throw new FisException("CQCHK0037", new string[] { deliveryNo });
                }
            }
            else if (string.IsNullOrEmpty(palletNo))
            {
                throw new FisException("CQCHK0037", new string[] { deliveryNo });
            }

            CurrentSession.AddValue("FullCartonsInPallet", "N");
            if (fullCartonsInPallet)
                CurrentSession.AddValue("FullCartonsInPallet", "Y");
          
            Pallet plt=pltRep.Find(palletNo);
            if (plt == null)
            {
                 throw new FisException("CHK106    ", new string[] { palletNo });
            }

            plt.PalletModel = pltModeType;
            pltRep.Update(plt,CurrentSession.UnitOfWork);
             CurrentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
            return base.DoExecute(executionContext);
        }

        private string assignPalletNo(IList<CombinedPalletCarton> palletCartonQty, IList<DeliveryPalletInfo> dnPalletList, out bool fullPallet, out bool fullCartonsInPallet)
        {
            fullPallet=false;
            fullCartonsInPallet = false;
            bool assigned = false;
            string ret = null;
            IList<DeliveryPalletInfo> palletList= dnPalletList.OrderBy(x =>x.palletNo).ToList();
            int fullPalletQty = 0;
            foreach (DeliveryPalletInfo item in palletList)
            {
                var assignedPalletQty = (from p in palletCartonQty
                                         where p.PalletNo == item.palletNo
                                         select p).ToList();
                int needAssignedQty = 1;
                int totalAssignedQty = 0;
                if (assigned)
                {
                    needAssignedQty = 0;
                }

                if (assignedPalletQty != null && assignedPalletQty.Count > 0)
                {
                    totalAssignedQty = assignedPalletQty[0].Qty + needAssignedQty;
                }
                else
                {
                    totalAssignedQty = needAssignedQty;
                }

                int diffQty = (item.deliveryQty - totalAssignedQty);
                if (diffQty==0)  //last combined pallet
                {
                    fullPalletQty++;
                    if (!assigned)
                    {
                        ret = item.palletNo;
                        assigned = true;

                        fullCartonsInPallet = true;
                    }      
                }
                else if (diffQty<0)  //over combined -1 is full pallet
                {
                   fullPalletQty++;
                }
                else  //not full pallet case
                {
                    if (!assigned)
                    {
                        ret = item.palletNo;
                    }
                    break;
                }
            }

            fullPallet = (fullPalletQty == dnPalletList.Count);
            return ret;
        }
               
    }
    
}
