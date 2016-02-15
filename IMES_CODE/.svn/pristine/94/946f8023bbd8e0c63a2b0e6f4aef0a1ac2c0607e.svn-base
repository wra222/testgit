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
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Extend;
using System.Collections.Generic;


namespace IMES.Activity
{


    /// <summary>
    /// Check Cartion Location
    /// </summary>
  
    public partial class CheckBSamLoc : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckBSamLoc()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckBSamLoc), new PropertyMetadata(true));
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
                return ((bool)(base.GetValue(CheckBSamLoc.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckBSamLoc.IsStopWFProperty, value);
            }
        }

        /// <summary>
        ///  get Product session key
        /// </summary>
        public static DependencyProperty ProdSessionKeyProperty = DependencyProperty.Register("ProdSessionKey", typeof(string), typeof(CheckBSamLoc), new PropertyMetadata(Session.SessionKeys.Product));
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
                return ((string)(base.GetValue(CheckBSamLoc.ProdSessionKeyProperty)));
            }
            set
            {
                base.SetValue(CheckBSamLoc.ProdSessionKeyProperty, value);
            }
        }


        /// <summary>
        ///  NotAssignLocException
        /// </summary>
        public static DependencyProperty NotAssignLocExceptionProperty = DependencyProperty.Register("NotAssignLocException", typeof(bool), typeof(CheckBSamLoc), new PropertyMetadata(true));
        /// <summary>
        /// NotAssignLocException
        /// </summary>
        [DescriptionAttribute("NotAssignLocException")]
        [CategoryAttribute("NotAssignLocException")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotAssignLocException
        {
            get
            {
                return ((bool)(base.GetValue(CheckBSamLoc.NotAssignLocExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckBSamLoc.NotAssignLocExceptionProperty, value);
            }
        }

        /// <summary>
        ///  PAQC Out Status
        /// </summary>
        public static DependencyProperty PAQCNotAllowStatusProperty = DependencyProperty.Register("PAQCNotAllowStatus", typeof(string), typeof(CheckBSamLoc), new PropertyMetadata("8,A"));
        /// <summary>
        /// PAQC Out Status
        /// </summary>
        [DescriptionAttribute("PAQCNotAllowStatus")]
        [CategoryAttribute("PAQCNotAllowStatus")]
        [BrowsableAttribute(true)]        
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string PAQCNotAllowStatus
        {
            get
            {
                return ((string)(base.GetValue(CheckBSamLoc.PAQCNotAllowStatusProperty)));
            }
            set
            {
                base.SetValue(CheckBSamLoc.PAQCNotAllowStatusProperty, value);
            }
        }


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
                FisException e = new FisException("CHK975", new string[] {this.ProdSessionKey});
                e.stopWF = this.IsStopWF;
                throw e;
            }
            string loc=currentProduct.GetAttributeValue("CartonLocation");
            if (!string.IsNullOrEmpty(loc))
            {
                //int locId = int.Parse(loc);
                IBSamRepository bsamRepository = RepositoryFactory.GetInstance().GetRepository<IBSamRepository, BSamLocation>();
                //BSamLocation bsamlocation=  bsamRepository.Find(locId);
                BSamLocation bsamlocation = bsamRepository.Find(loc);
                if (bsamlocation != null)
                {
                    if (bsamlocation.HoldOutput == "Y")
                    {
                        FisException e = new FisException("CHK976", new string[] {loc});
                        e.stopWF = this.IsStopWF;
                        throw e;
                    }
                }
                else
                {
                    FisException e = new FisException("CHK977", new string[] { loc });
                    e.stopWF = this.IsStopWF;
                    throw e;
                }
            }
            else if (this.NotAssignLocException) 
            {
                FisException e = new FisException("CHK978", new string[] { currentProduct.ProId });
                e.stopWF = this.IsStopWF;                
                throw e;
            }

            string paQCStatus = currentProduct.GetAttributeValue("PAQC_QCStatus");
            if (!string.IsNullOrEmpty(paQCStatus) && !string.IsNullOrEmpty(PAQCNotAllowStatus))
            {
                if (PAQCNotAllowStatus.Contains(paQCStatus))
                {
                    FisException e = new FisException("CHK984", new string[] { currentProduct.CUSTSN, paQCStatus });
                    e.stopWF = this.IsStopWF;
                    throw e;
                }
            }
            return base.DoExecute(executionContext);
          
        }
	}
}
