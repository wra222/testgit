// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Benson          create
//2013-03-13    Vincent           release
// Known issues:
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
using System.Collections.Generic;
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.BSam;
using IMES.DataModel;


namespace IMES.Activity
{


    /// <summary>
    /// Write Carton QC Log
    /// </summary>
  
    public partial class SetDeliveryExInfo : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public SetDeliveryExInfo()
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
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
           
            IList<Delivery> deliveryList = CurrentSession.GetValue("DeliveryList") as IList<Delivery>;
            IList<DeliveryPalletInfo> dpList = CurrentSession.GetValue("DeliveryPalletList") as IList<DeliveryPalletInfo>;

            IList<ModelCartonQty> modelQtyList = new List<ModelCartonQty>();
            //Check deliveryList is null or dpList is null throw error


            foreach (Delivery ele in deliveryList)
            {

                var deliveryPallet = (from p in dpList
                                      where p.deliveryNo == ele.DeliveryNo
                                      select p).ToList();

                if (deliveryPallet == null || deliveryPallet.Count == 0)
                {
                    // throw error code
                }
                else
                {
                    ele.DeliveryEx = SetDeliveryEx(ele.DeliveryNo, ele.Qty, ele.DeliveryInfoes, deliveryPallet[0]);
                    modelQtyList.Add(new ModelCartonQty { Model = ele.ModelName, Qty = ele.DeliveryEx.QtyPerCarton });
                }
            }

            // Update BSamModel.QtyPerCarton
            var modelCartonList = (from p in modelQtyList
                                                  group p by new { Model = p.Model, Qty = p.Qty } into t
                                                  select new ModelCartonQty { Model = t.Key.Model, Qty = t.Key.Qty }).ToList();

            IBSamRepository bsamRepository = RepositoryFactory.GetInstance().GetRepository<IBSamRepository,BSamLocation>();
           

            foreach (ModelCartonQty item in modelCartonList)
            {
                bsamRepository.UpdateBSamModelQtyPerCartonDefered(CurrentSession.UnitOfWork, item.Model, item.Qty, this.Editor);
            }

            return base.DoExecute(executionContext);
          
        }

        /// <summary>
        ///  Get DeliveryInfo to set DeliveryEx
        /// </summary>   

        private DeliveryEx SetDeliveryEx(string deliveryNo, int qty, IList<DeliveryInfo> infoList, DeliveryPalletInfo deliveryPallet)
        {
            DeliveryEx item = new DeliveryEx();
            item.DeliveryNo = deliveryNo;
            int DNCount =0;
            int cnQty = 0;

            string cqty = infoList.Where(x => x.InfoType == "CQty").Select(y => y.InfoValue).FirstOrDefault();
            if (!string.IsNullOrEmpty(cqty))
            {
                item.QtyPerCarton = (int)double.Parse(cqty);
            }
            
            foreach (DeliveryInfo info in infoList)
            {
                 //if  (string.IsNullOrEmpty(info.InfoValue))
                 //{
                 //    continue;
                 //}
                switch (info.InfoType)
                {
                    case "Consolidated":                       
                        string[] s = info.InfoValue.Split(new char[] { '/' });
                        item.ShipmentNo = s[0].Trim();
                        item.ConsolidateQty = int.Parse(s[1].Trim()); 
                        break;  
                    case "PackingType":
                        item.ShipType = info.InfoValue;
                         break;
                    //case "CQty":
                    case "CnQty":
                         cnQty = (int)double.Parse(info.InfoValue);
                         if (cnQty > 0)
                         {
                             item.QtyPerCarton = cnQty;
                         }
                        break;                               
                    case "RegId":
                        item.MessageCode = info.InfoValue;
                        break;                   
                    case "ShipToParty":
                        item.ShipToParty = info.InfoValue.Substring(info.InfoValue.IndexOf("-") + 1).Trim();
                        break;
                    case "GroupId":
                        item.GroupId = info.InfoValue;
                        break;
                    case "Priority":
                        item.Priority = info.InfoValue;
                        break;
                    case "Flag":
                        item.OrderType= info.InfoValue;
                        break;
                    case "BOL":
                        item.BOL= info.InfoValue;
                        break;
                    case "EmeaCarrier":
                        item.HAWB = info.InfoValue;
                        break;
                    case "Carrier":
                        item.Carrier = info.InfoValue;
                        break;
                    case "ShipWay":
                        item.ShipWay = info.InfoValue;
                        break;
                    case "Invoice":
                        item.PackID= info.InfoValue;
                        break;
                     case "InvoiceNum":
                         DNCount=int.Parse( info.InfoValue);
                        break;
                    case "Invoice_NO":
                        DNCount = int.Parse(info.InfoValue);
                        break;
                    case "PalletQty":
                        string[] data = info.InfoValue.Split('.');
                        item.StdPltFullQty = data[0].Trim();
                        if (data.Length>1)
                        {
                            item.StdPltStackType = data[1].Trim();
                         }
                        break;
    
                }
            }

            if (string.IsNullOrEmpty(item.ShipmentNo))
            {
                item.ShipmentNo = deliveryNo.Substring(0, 10);
                item.ConsolidateQty = DNCount;
            }

            if (item.QtyPerCarton > 0)
            {
                item.CartonQty = qty % item.QtyPerCarton == 0 ? (qty / item.QtyPerCarton) : (qty / item.QtyPerCarton) + 1;
            }

            item.PalletType = deliveryPallet.palletNo.Substring(0, 2) == "NA" ? "C" : "P";

            item.Editor = this.Editor;

            return item;
        }

        private class ModelCartonQty
        {
            public string Model { get; set; }
            public int Qty { get; set; }
        }
	}
}
