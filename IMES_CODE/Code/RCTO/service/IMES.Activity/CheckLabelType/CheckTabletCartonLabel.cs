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
using IMES.DataModel;
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// Check TabletCartonLabel Print Master Carton Label
    /// </summary>
    public partial class CheckTabletCartonLabel : BaseActivity
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public CheckTabletCartonLabel()
		{
			InitializeComponent();
		}

        /// <summary>
        ///Check  LabelType
        /// </summary>
        public static DependencyProperty LabelTypeProperty = DependencyProperty.Register("LabelType", typeof(string), typeof(CheckTabletCartonLabel), new PropertyMetadata("Tablet Carton Label"));

        /// <summary>
        /// Check LabelType
        /// </summary>
        [DescriptionAttribute("LabelType")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string LabelType
        {
            get
            {
                return ((string)(base.GetValue(CheckTabletCartonLabel.LabelTypeProperty)));
            }
            set
            {
                base.SetValue(CheckTabletCartonLabel.LabelTypeProperty, value);
            }
        }

        /// <summary>
        ///DeliveryInfo CQty
        /// </summary>
        public static DependencyProperty QtyPerCartonProperty = DependencyProperty.Register("QtyPerCarton", typeof(int), typeof(CheckTabletCartonLabel), new PropertyMetadata(5));

        /// <summary>
        /// DeliveryInfo CQty
        /// </summary>
        [DescriptionAttribute("QtyPerCarton Delivery CQty")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int QtyPerCarton
        {
            get
            {
                return ((int)(base.GetValue(CheckTabletCartonLabel.QtyPerCartonProperty)));
            }
            set
            {
                base.SetValue(CheckTabletCartonLabel.QtyPerCartonProperty, value);
            }
        }

        /// <summary>
        /// CheckTabletCartonLabel
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var PrintItemList = (IList<PrintItem>)CurrentSession.GetValue(Session.SessionKeys.PrintItems);

            //0.No Print Item List do nothing
            if (PrintItemList == null || PrintItemList.Count == 0)
            {
                return base.DoExecute(executionContext);
            }

            var resultPrintItemList = (from item in PrintItemList
                                       where item.LabelType == LabelType
                                       select item).ToList();

            if (resultPrintItemList.Count > 0)
            {
                Product Prod = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
                if ( string.IsNullOrEmpty(Prod.DeliveryNo) || 
                     !CheckDeliveryCQty(Prod.DeliveryNo))
                {

                        resultPrintItemList = (from item in PrintItemList
                                                            where item.LabelType != LabelType
                                                            select item).ToList();
                      CurrentSession.AddValue(Session.SessionKeys.PrintItems, resultPrintItemList);
                    
                }
            }

            return base.DoExecute(executionContext);
        }

        private bool CheckDeliveryCQty(string deliveryNo)
        {
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
            Delivery dn = dnRep.Find(deliveryNo);
            if (dn != null && (dn.DeliveryEx.QtyPerCarton== this.QtyPerCarton))
            {
                return true;
            }
            return false;
        }
	}
}
