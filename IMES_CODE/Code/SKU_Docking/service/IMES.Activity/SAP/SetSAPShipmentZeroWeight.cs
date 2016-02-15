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
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Activity
{
    /// <summary>
    /// 設置SAP Sipment Weight =0 
    /// </summary>
    public partial class SetSAPShipmentZeroWeight : BaseActivity
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public SetSAPShipmentZeroWeight()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 設置SAP Sipment Weight =0 
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            IList<string> dnList = null;

            string dn = (string)session.GetValue(Session.SessionKeys.DeliveryNo);
            if (string.IsNullOrEmpty(dn))
            {
                dnList = session.GetValue(Session.SessionKeys.DeliveryNoList) as IList<string>;
                if (dnList == null)
                {
                    dnList = new List<string>();
                }
            }
            else
            {
                dnList = new List<string>();
                dnList.Add(dn);
            }

            if (dnList.Count > 0)
            {

                IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                IPalletWeightRepository weightRep = RepositoryFactory.GetInstance().GetRepository<IPalletWeightRepository, PalletWeight>();

                #region SKU
                IList<ShipmentInfoDef> shipmentInfoList = deliveryRep.GetShipmentByDnList(SqlHelper.ConnectionString_PAK,dnList);
                var shipmentNoList= shipmentInfoList.Select(x=>x.Shipment.Substring(0,10)).Distinct().ToList();

               

                IList<FisToSapWeightDef> fisToSapWeightList = weightRep.ExistsFisToSapWeightByShipment(SqlHelper.ConnectionString_PAK,shipmentNoList);
                string shipmentNo=null;
                foreach (FisToSapWeightDef item in fisToSapWeightList)
                {
                   if (shipmentNo != item.Shipment)
                    {
                        shipmentNo = item.Shipment;
                        weightRep.InsertFisToSapWeightDefered(session.UnitOfWork, SqlHelper.ConnectionString_PAK,
                                                                                             new FisToSapWeightDef() { Shipment = item.Shipment, 
                                                                                                                                            ShipType = item.ShipType, 
                                                                                                                                            Weight = 0 });
 
                    }
                }
                #endregion


                #region Docking
                IList<ShipmentInfoDef>  dockingShipmentInfoList = deliveryRep.GetShipmentByDnList(SqlHelper.ConnectionString_Docking, dnList);
                var dockingShipmentNoList = dockingShipmentInfoList.Select(x => x.Shipment.Substring(0, 10)).Distinct().ToList();
                             

                IList<FisToSapWeightDef> dockingFisToSapWeightList = weightRep.ExistsFisToSapWeightByShipment(SqlHelper.ConnectionString_Docking, shipmentNoList);
               
                shipmentNo = null;
                foreach (FisToSapWeightDef item in dockingFisToSapWeightList)
                {
                    if (shipmentNo != item.Shipment)
                    {
                        shipmentNo = item.Shipment;
                        weightRep.InsertFisToSapWeightDefered(session.UnitOfWork, 
                                                                                            SqlHelper.ConnectionString_Docking,
                                                                                             new FisToSapWeightDef()
                                                                                             {
                                                                                                 Shipment = item.Shipment,
                                                                                                 ShipType = item.ShipType,
                                                                                                 Weight = 0
                                                                                             });

                    }
                }
                #endregion

            }
            return base.DoExecute(executionContext);
        }
	}
}
