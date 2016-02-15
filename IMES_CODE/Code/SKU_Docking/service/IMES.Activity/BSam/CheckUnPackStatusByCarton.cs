// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Vincent          create
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
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.DN;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
	public partial class CheckUnPackStatusByCarton: BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckUnPackStatusByCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckUnPackStatusByCarton), new PropertyMetadata(true));
        /// <summary>
        ///  禁用時要停止workflow
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(CheckUnPackStatusByCarton.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckUnPackStatusByCarton.IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// 允許DeliveryStatus
        /// </summary>
        public static DependencyProperty AllowDNStatusProperty = DependencyProperty.Register("AllowDNStatus", typeof(string), typeof(CheckUnPackStatusByCarton), new PropertyMetadata("00,87,88"));

        /// <summary>
        ///允許DeliveryStatus
        /// </summary>
        [DescriptionAttribute("AllowDNStatus")]
        [CategoryAttribute("InArguments Of CheckUnPackStatusByCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AllowDNStatus
        {
            get
            {
                return ((string)(base.GetValue(CheckUnPackStatusByCarton.AllowDNStatusProperty)));
            }
            set
            {
                base.SetValue(CheckUnPackStatusByCarton.AllowDNStatusProperty, value);
            }
        }

        /// <summary>
        /// 允許CartonStatus
        /// </summary>
        public static DependencyProperty AllowCartonStatusProperty = DependencyProperty.Register("AllowCartonStatus", typeof(string), typeof(CheckUnPackStatusByCarton), new PropertyMetadata("Full,Partial"));

        /// <summary>
        ///允許CartonStatus
        /// </summary>
        [DescriptionAttribute("AllowCartonStatus")]
        [CategoryAttribute("InArguments Of CheckUnPackStatusByCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AllowCartonStatus
        {
            get
            {
                return ((string)(base.GetValue(CheckUnPackStatusByCarton.AllowCartonStatusProperty)));
            }
            set
            {
                base.SetValue(CheckUnPackStatusByCarton.AllowCartonStatusProperty, value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IDeliveryRepository dnRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Carton carton =(Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
            IList<Delivery> dnList = new List<Delivery>();


            if (!AllowCartonStatus.Contains(carton.Status.ToString()))
            {
                FisException e = new FisException("CHK991", new string[] { carton.CartonSN, carton.Status.ToString() });
                e.stopWF = this.IsStopWF;
                throw e;
            }

            foreach (DeliveryCarton item in carton.DeliveryCartons)
            {
                Delivery dn= dnRepository.Find(item.DeliveryNo);
                if (!AllowDNStatus.Contains(dn.Status))
                {
                    FisException e = new FisException("CHK992", new string[] { item.DeliveryNo, dn.Status });
                    e.stopWF = this.IsStopWF;
                    throw e;
                }
                dnList.Add(dn);
            }

            CurrentSession.AddValue(Session.SessionKeys.DeliveryList, dnList);
            return base.DoExecute(executionContext);

        }
	}

   
}
