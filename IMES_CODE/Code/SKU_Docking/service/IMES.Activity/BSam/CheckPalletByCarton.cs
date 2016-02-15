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
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Carton;
using IMES.Infrastructure.Extend;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 檢查是否需要分配棧板
    /// </summary>
	public partial class CheckPalletByCarton: BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckPalletByCarton()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 找不到Carton 報錯
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(bool), typeof(CheckPalletByCarton), new PropertyMetadata(false));

        /// <summary>
        /// 找不到Carton Model 報錯
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of CheckBSamModel")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotExistException
        {
            get
            {
                return ((bool)(base.GetValue(CheckPalletByCarton.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckPalletByCarton.NotExistExceptionProperty, value);
            }
        }


        /// <summary>
        /// 找不到Carton 報錯
        /// </summary>
        public static DependencyProperty ExistExceptionProperty = DependencyProperty.Register("ExistException", typeof(bool), typeof(CheckPalletByCarton), new PropertyMetadata(true));

        /// <summary>
        /// 找到Carton Model 報錯
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of CheckBSamModel")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool ExistException
        {
            get
            {
                return ((bool)(base.GetValue(CheckPalletByCarton.ExistExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckPalletByCarton.ExistExceptionProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            bool IsNeedAssignPallet = false;
            bool IsNeedAssignDevice = false;
            CurrentSession.AddValue(ExtendSession.SessionKeys.IsNeedAssignPallet, false);
            CurrentSession.AddValue(ExtendSession.SessionKeys.IsNeedAssignDevice, false);
            Carton carton = cartonRep.Find(currentProduct.CartonSN);
            if (carton == null && NotExistException)
            {
                throw new FisException("CHK993", new string[] { currentProduct.CartonSN });
            }

            if (carton != null)
            {
                switch(carton.Status)
                {
                    case CartonStatusEnum.Partial:
                    case CartonStatusEnum.Full:
                       
                        if (!carton.PalletNo.Equals(currentProduct.PalletNo))
                        {
                            FisException e = new FisException("CHK995", new string[] { currentProduct.CartonSN, carton.PalletNo, currentProduct.PalletNo });
                            throw e;
                        }
                        else if (carton.Qty != carton.AssignQty())
                        {
                             CurrentSession.AddValue(ExtendSession.SessionKeys.IsNeedAssignDevice, true);
                            IsNeedAssignDevice = true;

                        }
                        else if (string.IsNullOrEmpty(carton.PalletNo) && 
                                    string.IsNullOrEmpty(currentProduct.PalletNo) && 
                                   !string.IsNullOrEmpty(carton.UnPackPalletNo))
                        {
                            CurrentSession.AddValue(ExtendSession.SessionKeys.IsNeedAssignPallet, true);
                            IsNeedAssignPallet = true;
                        }                        
                        else
                        {
                            FisException e = new FisException("CHK994", new string[] { currentProduct.CartonSN, carton.PalletNo });
                            throw e;
                        }
                        break;
                    default:
                        break;
                }
                
                if (IsNeedAssignPallet ||  IsNeedAssignDevice)
                {
                    IList<IProduct> prodList = prodRep.GetProductListByCartonNo(currentProduct.CartonSN);
                    foreach (Product item in prodList)
                    {
                        if (!carton.PalletNo.Equals(item.PalletNo))
                        {
                            FisException e = new FisException("CHK995", new string[] { carton.CartonSN, carton.PalletNo, item.PalletNo });
                        }
                    }
                    CurrentSession.AddValue(Session.SessionKeys.Carton, carton);
                    CurrentSession.AddValue(Session.SessionKeys.ProdList, prodList);
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, prodRep.GetProductIDListByCarton(currentProduct.CartonSN));
                    
                    IList<Delivery> dnList = new List<Delivery>();
                    IList<DeliveryCarton> dcList = carton.DeliveryCartons;
                    IList<AvailableDelivery> availableDNList = new List<AvailableDelivery>();
                   
                    foreach (DeliveryCarton item in dcList)
                    {

                        Delivery dn =deliveryRep.Find(item.DeliveryNo);
                        if (dn!=null)
                        {
                           dnList.Add(dn);
                           AvailableDelivery availableDN = new AvailableDelivery();
                           availableDN.DeliveryNo=item.DeliveryNo;
                           availableDN.Model =item.Model;
                           availableDN.CartonQty=item.Qty;
                            availableDN.DNCartonQty = dn.DeliveryEx.CartonQty;
                            availableDN.PackCategory =dn.DeliveryEx.PalletType;
                            availableDN.Qty = dn.Qty;
                            availableDN.QtyPerCarton = carton.FullQty;
                            availableDN.RemainQty = dn.Qty - prodRep.GetCombinedQtyByDN(item.DeliveryNo);
                            availableDN.ShipDate = dn.ShipDate;
                            availableDN.ShipmentNo = dn.DeliveryEx.ShipmentNo;
                            availableDN.TotalCartonQty = 0;
                           availableDNList.Add(availableDN);
                        }
                    }
                    CurrentSession.AddValue(Session.SessionKeys.DeliveryList,dnList);
                    CurrentSession.AddValue(ExtendSession.SessionKeys.BindDNList, carton.DeliveryCartons);
                    CurrentSession.AddValue(ExtendSession.SessionKeys.AvailableDNList, availableDNList);
                    CurrentSession.AddValue(ExtendSession.SessionKeys.BindDeiviceQty, carton.Qty);
                    CurrentSession.AddValue(ExtendSession.SessionKeys.FullCartonQty, carton.FullQty);
                }

            }

           
            return base.DoExecute(executionContext);

        }
	}
}
