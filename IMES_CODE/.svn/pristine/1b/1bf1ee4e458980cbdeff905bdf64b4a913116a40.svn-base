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
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Extend;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
	public partial class CheckUnPackStatusByDn:BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckUnPackStatusByDn()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Carton有多DN關係
        /// </summary>
        public static DependencyProperty MultiDnExceptionProperty = DependencyProperty.Register("MultiDnException", typeof(bool), typeof(CheckUnPackStatusByDn), new PropertyMetadata(true));

        /// <summary>
        /// Carton有多DN關係
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of GetDNRelationByDn")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool MultiDnException
        {
            get
            {
                return ((bool)(base.GetValue(CheckUnPackStatusByDn.MultiDnExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckUnPackStatusByDn.MultiDnExceptionProperty, value);
            }
        }

        /// <summary>
        /// 允許DeliveryStatus
        /// </summary>
        public static DependencyProperty AllowDNStatusProperty = DependencyProperty.Register("AllowDNStatus", typeof(string), typeof(CheckUnPackStatusByDn), new PropertyMetadata("00,87,88"));

        /// <summary>
        ///允許DeliveryStatus
        /// </summary>
        [DescriptionAttribute("AllowDNStatus")]
        [CategoryAttribute("InArguments Of CheckDnRelationByDn")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AllowDNStatus
        {
            get
            {
                return ((string)(base.GetValue(CheckUnPackStatusByDn.AllowDNStatusProperty)));
            }
            set
            {
                base.SetValue(CheckUnPackStatusByDn.AllowDNStatusProperty, value);
            }
        }

        ///// <summary>
        ///// 允許CartonStatus
        ///// </summary>
        //public static DependencyProperty AllowCartonStatusProperty = DependencyProperty.Register("AllowCartonStatus", typeof(string), typeof(CheckUnPackStatusByDn), new PropertyMetadata("Full,Partial"));

        ///// <summary>
        /////允許CartonStatus
        ///// </summary>
        //[DescriptionAttribute("AllowCartonStatus")]
        //[CategoryAttribute("InArguments Of CheckDnRelationByDn")]
        //[BrowsableAttribute(true)]
        //[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        //public string AllowCartonStatus
        //{
        //    get
        //    {
        //        return ((string)(base.GetValue(CheckUnPackStatusByDn.AllowCartonStatusProperty)));
        //    }
        //    set
        //    {
        //        base.SetValue(CheckUnPackStatusByDn.AllowCartonStatusProperty, value);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository dnRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            string dnNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            Delivery dn;
            if (CurrentSession.GetValue(Session.SessionKeys.Delivery) == null)
            {
                dn = dnRepository.Find(dnNo);
            }
            else
            {
                dn = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            }

             if (dn == null)
             {
                 FisException e = new FisException("CHK190", new string[] { dnNo });
                 throw e;
              }

             if (!AllowDNStatus.Contains(dn.Status))
             {
                 FisException e = new FisException("CHK992", new string[] { dn.DeliveryNo, dn.Status });               
                 throw e;
             }           


            IList<CartonProductInfo> cartonProdList=  cartonRep.GetCartonProductByExcludeDN(dnNo); 

            if (cartonProdList.Count>0 && MultiDnException)
            {
                var cartonSNList = (from p in cartonProdList
                                    select p.CartonSN.Trim() + "(BoxId:" + p.BoxId + ")").Distinct().ToList();
                var dnList = (from p in cartonProdList
                                    select p.DeliveryNo.Trim() ).Distinct().ToList();
                var palletList = (from p in cartonProdList
                              select p.PalletNo.Trim()).Distinct().ToList();
                string msg = string.Join(",", cartonSNList.ToArray()) + " DN:" + string.Join(",", dnList.ToArray()) + " Pallet No:" + string.Join(",", palletList.ToArray());
                FisException e = new FisException("CHK996", new string[] { dnNo, msg });
                throw e;
            }


            //if (!MultiDnException)
            //{
                //Remain product info 
                if (cartonProdList.Count > 0)
                {
                    var prodIds = (from p in cartonProdList
                                   select p.ProductID.Trim()).Distinct().ToList();
                    
                    var dnNoList = (from p in cartonProdList
                                              select p.DeliveryNo.Trim()).Distinct().ToList();
                     IList<Delivery> dnList =cartonRep.GetDeliveryByNoList(dnNoList);
                     foreach (Delivery item in dnList)
                     {
                         if (!AllowDNStatus.Contains(item.Status))
                         {
                             FisException e = new FisException("CHK992", new string[] { item.DeliveryNo, item.Status });
                             throw e;
                         }          
                     }
                     dnList.Add(dn);
                    
                    CurrentSession.AddValue(ExtendSession.SessionKeys.CartonProductInfoList,cartonProdList);
                     CurrentSession.AddValue(Session.SessionKeys.DeliveryList, dnList);
                     CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, prodIds);
                    //remain productId with another DN
                     CurrentSession.AddValue(Session.SessionKeys.ProdList, prodRep.GetProductListByIdList(prodIds));

                }
                else
                {
                    IList<Delivery> dnList = new List<Delivery>();
                    dnList.Add(dn);
                    CurrentSession.AddValue(ExtendSession.SessionKeys.CartonProductInfoList, new List<CartonProductInfo>());
                    CurrentSession.AddValue(Session.SessionKeys.DeliveryList, dnList);                   
                    CurrentSession.AddValue(Session.SessionKeys.NewScanedProductIDList, new List<string>());
                    CurrentSession.AddValue(Session.SessionKeys.ProdList, new List<IProduct>());
                }
           // }//
            //else
            //{
            //     IList<Delivery> dnList = new List<Delivery>();
            //    dnList.Add(dn);               
            //    CurrentSession.AddValue(Session.SessionKeys.DeliveryList, dnList);
            //}

             CurrentSession.AddValue(Session.SessionKeys.ModelName, dn.ModelName);

            return base.DoExecute(executionContext);

        }
	}
}
