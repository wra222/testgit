// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-25   vincent          create
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
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// CheckBSamUnPackAllBySN
    /// </summary>
	public partial class CheckBSamUnPackAllBySN: BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckBSamUnPackAllBySN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Set SessionKey
        /// </summary>
        public static DependencyProperty SessionKeyProperty = DependencyProperty.Register("SessionKey", typeof(string), typeof(CheckBSamUnPackAllBySN), new PropertyMetadata(""));

        /// <summary>
        /// ConstValue Name
        /// </summary>
        [DescriptionAttribute("SessionKey")]
        [CategoryAttribute("InArguments Of CheckBSamUnPackAllBySN")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SessionKey
        {
            get
            {
                return ((string)(base.GetValue(CheckBSamUnPackAllBySN.SessionKeyProperty)));
            }
            set
            {
                base.SetValue(CheckBSamUnPackAllBySN.SessionKeyProperty, value);
            }
        }
        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckBSamUnPackAllBySN), new PropertyMetadata(true));
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
                return ((bool)(base.GetValue(CheckBSamUnPackAllBySN.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckBSamUnPackAllBySN.IsStopWFProperty, value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<string> allowStation = (IList<string>)CurrentSession.GetValue(this.SessionKey);

            if (currentProduct == null)
            {
                FisException e = new FisException("CHK975", new string[] { Session.SessionKeys.Product });
                e.stopWF = this.IsStopWF;
                throw e;
            }

            if (allowStation == null)
            {
                FisException e = new FisException("CHK990", new string[] { "ConstValueType Table", this.SessionKey, "" });
                e.stopWF = this.IsStopWF;
                throw e;
            }

            string station = currentProduct.Status.StationId;

            if (!allowStation.Contains(station))
            {
              //  FisException e = new FisException("CHK997", new string[] { currentProduct.CUSTSN, currentProduct.CartonSN });
                FisException e = new FisException("CHK997", new string[] { currentProduct.CUSTSN, station });
                e.stopWF = this.IsStopWF;
                throw e;
            }

            if (!string.IsNullOrEmpty(currentProduct.CartonSN) ||
                !string.IsNullOrEmpty(currentProduct.DeliveryNo) ||
                !string.IsNullOrEmpty(currentProduct.PalletNo))
            {
                FisException e = new FisException("CHK998", new string[] { currentProduct.CUSTSN });
                e.stopWF = this.IsStopWF;
                throw e;
            }
          
            return base.DoExecute(executionContext);

        }
	}
}
