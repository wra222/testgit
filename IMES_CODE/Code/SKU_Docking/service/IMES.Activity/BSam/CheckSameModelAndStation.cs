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
using IMES.Infrastructure.Extend;
using System.Collections.Generic;

namespace IMES.Activity
{


    /// <summary>
    ///Check Same Model And Station
    /// </summary>
  
    public partial class CheckSameModelAndStation : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckSameModelAndStation()
		{
			InitializeComponent();
		}

        /// <summary>
        ///  get Product session key
        /// </summary>
        public static DependencyProperty ProdSessionKeyProperty = DependencyProperty.Register("ProdSessionKey", typeof(string), typeof(CheckSameModelAndStation), new PropertyMetadata(Session.SessionKeys.Product));
        /// <summary>
        /// get Product session key
        /// </summary>
        [DescriptionAttribute("ProdSessionKey")]
        [CategoryAttribute("ProdSessionKey")]
        [BrowsableAttribute(true)]        
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ProdSessionKey
        {
            get
            {
                return ((string)(base.GetValue(CheckSameModelAndStation.ProdSessionKeyProperty)));
            }
            set
            {
                base.SetValue(CheckSameModelAndStation.ProdSessionKeyProperty, value);
            }
        }

        /// <summary>
        /// Carton Location 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckSameModelAndStation), new PropertyMetadata(true));
        /// <summary>
        /// Carton Location 禁用時要停止workflow
        /// </summary>
        [DescriptionAttribute("IsStopWF")]
        [CategoryAttribute("IsStopWF")]
        [BrowsableAttribute(true)]       
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsStopWF
        {
            get
            {
                return ((bool)(base.GetValue(CheckSameModelAndStation.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckSameModelAndStation.IsStopWFProperty, value);
            }
        }


        ///// <summary>
        /////  PAQC Out Station
        ///// </summary>
        //public static DependencyProperty PAQCOutStationProperty = DependencyProperty.Register("PAQCOutStation", typeof(string), typeof(CheckSameModelAndStation), new PropertyMetadata(""));
        ///// <summary>
        ///// PAQC Out Station
        ///// </summary>
        //[DescriptionAttribute("PAQCOutStation")]
        //[CategoryAttribute("PAQCOutStation")]
        //[BrowsableAttribute(true)]        
        //[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        //public string PAQCOutStation
        //{
        //    get
        //    {
        //        return ((string)(base.GetValue(CheckSameModelAndStation.PAQCOutStationProperty)));
        //    }
        //    set
        //    {
        //        base.SetValue(CheckSameModelAndStation.PAQCOutStationProperty, value);
        //    }
        //}

        ///// <summary>
        /////  PAQC Out Status
        ///// </summary>
        //public static DependencyProperty PAQCOutStatusProperty = DependencyProperty.Register("PAQCOutStatus", typeof(string), typeof(CheckSameModelAndStation), new PropertyMetadata(""));
        ///// <summary>
        ///// PAQC Out Status
        ///// </summary>
        //[DescriptionAttribute("PAQCOutStatus")]
        //[CategoryAttribute("PAQCOutStatus")]
        //[BrowsableAttribute(true)]        
        //[DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        //public string PAQCOutStatus
        //{
        //    get
        //    {
        //        return ((string)(base.GetValue(CheckSameModelAndStation.PAQCOutStatusProperty)));
        //    }
        //    set
        //    {
        //        base.SetValue(CheckSameModelAndStation.PAQCOutStatusProperty, value);
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(this.ProdSessionKey);
            if (currentProduct == null)
            {
                FisException e = new FisException("CHK975", new string[] { this.ProdSessionKey });
                e.stopWF = this.IsStopWF;
                throw e;
            }

            IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
            if (prodList == null || prodList.Count == 0)
            {
                FisException e = new FisException("CHK975", new string[] { this.ProdSessionKey });
                e.stopWF = this.IsStopWF;
                throw e;
            }
            IProduct prod = prodList[0];
            if (currentProduct.Model != prod.Model)
            {
                FisException e = new FisException("CHK982", new string[] { currentProduct.Model, prod.Model,
                                                                                                                      currentProduct.Status.StationId, prod.Status.StationId,
                                                                                                                      currentProduct.Status.Status.ToString(), prod.Status.Status.ToString() });
                e.stopWF = this.IsStopWF;
                throw e;
            }

            CurrentSession.AddValue(ExtendSession.SessionKeys.IsSameStation, true);
            if (currentProduct.Status.StationId != prod.Status.StationId ||
                 currentProduct.Status.Status != prod.Status.Status)
            {
                CurrentSession.AddValue (ExtendSession.SessionKeys.IsSameStation, false);                   
             }
                        
            return base.DoExecute(executionContext);
          
        }
	}
}
