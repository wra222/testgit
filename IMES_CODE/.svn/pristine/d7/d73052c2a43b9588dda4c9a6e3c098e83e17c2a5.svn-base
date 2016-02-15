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

namespace IMES.Activity
{


    /// <summary>
    /// Generate Cartion SN
    /// </summary>
  
    public partial class AssignPalletWithCarton : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public AssignPalletWithCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 重新分配新的PalletNo
        /// </summary>
        public static DependencyProperty IsAssignPalletNoProperty = DependencyProperty.Register("IsAssignPalletNo", typeof(bool), typeof(AssignPalletWithCarton), new PropertyMetadata(false));
        /// <summary>
        ///  重新分配新的PalletNo
        /// </summary>
        [DescriptionAttribute("IsAssignPalletNo")]
        [CategoryAttribute("IsAssignPalletNo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsAssignPalletNo
        {
            get
            {
                return ((bool)(base.GetValue(AssignPalletWithCarton.IsAssignPalletNoProperty)));
            }
            set
            {
                base.SetValue(AssignPalletWithCarton.IsAssignPalletNoProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();

            IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
            if (prodList == null || prodList.Count == 0)
            {
                throw new FisException("CHK975", new string[] { "" });
            }

            IList<DeliveryCarton> bindDNList = (IList<DeliveryCarton>)CurrentSession.GetValue(ExtendSession.SessionKeys.BindDNList);

            if (bindDNList == null || bindDNList.Count == 0)
            {
                throw new FisException("CHK981", new string[] { prodList[0].Model });
            }

            Carton carton = (Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
            if (carton == null)
            {
                throw new FisException("CHK978", new string[] { prodList[0].ProId });
            }

            //check assigned PalletNo
            string assignPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.Pallet);
            if (string.IsNullOrEmpty(assignPalletNo) ||
                this.IsAssignPalletNo)
            {
                if (bindDNList.Count == 1)
                {
                    assignPalletNo = cartonRep.BindPalletWithDN(bindDNList[0].DeliveryNo);
                }
                else
                {
                    IList<string> dnList = new List<string>();
                    foreach (DeliveryCarton item in bindDNList)
                    {
                        dnList.Add(item.DeliveryNo);
                    }
                    assignPalletNo = cartonRep.BindPalletWithDN(dnList);
                }
            }
            //CurrentSession.AddValue("HasAssignPLTQrty", 0);           

            if (string.IsNullOrEmpty(assignPalletNo))
            {
                throw new FisException("PAK093", new string[] { bindDNList[0].DeliveryNo });
            }

            //CurrentSession.AddValue("HasAssignPLTQrty", 1);

                         
            Pallet assignPallet = palletRep.Find(assignPalletNo);
            

            if (assignPallet == null)
            {
                FisException fe = new FisException("PAK093", new string[] { });   //此船务栈板已满!
                throw fe;
            }

            //foreach (Product item in prodList)
            //{
            //    item.PalletNo = assignPallet.PalletNo;
            //    productRep.Update(item, CurrentSession.UnitOfWork);
            //}

            //foreach (DeliveryCarton item in bindDNList)
            //{
            //    item.Status = DeliveryCartonState.Reserve;
            //    carton.SetDeliveryInCarton(item);
            //}

            carton.Status = CartonStatusEnum.Reserve; 
            carton.PalletNo = assignPallet.PalletNo;            
            cartonRep.Update(carton, CurrentSession.UnitOfWork);

            //carton.PalletNo = assignPallet.PalletNo;
            //cartonRep.Update(carton, CurrentSession.UnitOfWork);

            CurrentSession.AddValue(Session.SessionKeys.Pallet, assignPallet);
            
            return base.DoExecute(executionContext);
          
        }
	}
}
