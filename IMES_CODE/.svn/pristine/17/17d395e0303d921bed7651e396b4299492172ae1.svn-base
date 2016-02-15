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
using IMES.FisObject.PAK.Carton;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.BSam;

namespace IMES.Activity
{


    /// <summary>
    /// OutCartonLoc
    /// </summary>
  
    public partial class OutCartonLoc : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public OutCartonLoc()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 单条还是成批插入
        /// </summary>
        public static DependencyProperty IsSingleProperty = DependencyProperty.Register("IsSingle", typeof(bool), typeof(OutCartonLoc), new PropertyMetadata(false));

        /// <summary>
        /// 单条还是成批插入,Session.SessionKeys.ProdList
        /// </summary>
        [DescriptionAttribute("IsSingle")]
        [CategoryAttribute("IsSingle Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsSingle
        {
            get
            {
                return ((bool)(base.GetValue(OutCartonLoc.IsSingleProperty)));
            }
            set
            {
                base.SetValue(OutCartonLoc.IsSingleProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IBSamRepository bsamRep = RepositoryFactory.GetInstance().GetRepository<IBSamRepository, BSamLocation>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
            if (IsSingle)
            {
                Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (currentProduct == null)
                {
                    throw new FisException("CHK975", new string[] { Session.SessionKeys.Product }); 
                }

               // int locId = 0;
                string loc = currentProduct.GetAttributeValue("CartonLocation");
                if (!string.IsNullOrEmpty(loc)) //&&
                     //int.TryParse(loc, out locId))
                {
                    //bsamRep.MoveOutLocDefered(CurrentSession.UnitOfWork, locId, this.Editor);
                    bsamRep.MoveOutLocDefered(CurrentSession.UnitOfWork, loc, this.Editor);
                    currentProduct.SetAttributeValue("CartonLocation", "", this.Editor, this.Station, loc);
                    prodRep.Update(currentProduct, CurrentSession.UnitOfWork);
                }

            }
            else
            {
                IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
                if (prodList == null || prodList.Count == 0)
                {
                    throw new FisException("CHK975", new string[] { Session.SessionKeys.ProdList }); 
                }

                //IList<DeliveryCarton> bindDNList = (IList<DeliveryCarton>)CurrentSession.GetValue(ExtendSession.SessionKeys.BindDNList);

                //if (bindDNList == null || bindDNList.Count == 0)
                //{
                //    throw new FisException("CHK981", new string[] { prodList[0].Model }); ;
                //}

                //Carton carton = (Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
                //if (carton==null)
                //{

                //    throw new FisException("CHK978", new string[] { prodList[0].ProId });
                //}

               

                foreach (IProduct item in prodList)
                {
                    //int locId = 0;
                    string loc = item.GetAttributeValue("CartonLocation");
                    if (!string.IsNullOrEmpty(loc)) //&&
                         //int.TryParse(loc, out locId))
                    {
                        //bsamRep.MoveOutLocDefered(CurrentSession.UnitOfWork, locId, this.Editor);
                        bsamRep.MoveOutLocDefered(CurrentSession.UnitOfWork, loc, this.Editor);
                        item.SetAttributeValue("CartonLocation", "", this.Editor, this.Station, loc);
                        prodRep.Update(item, CurrentSession.UnitOfWork);
                    }
                }
            }
            //foreach (DeliveryCarton item in bindDNList)
            //{
            //    item.Status = DeliveryCartonState.Assign;
            //    carton.SetDeliveryInCarton(item);
            //}
            //carton.DNQty = bindDNList.Count;
            //carton.Model = prodList[0].Model;

            //cartonRep.Add(carton, CurrentSession.UnitOfWork);


            return base.DoExecute(executionContext);
          
        }
	}
}
