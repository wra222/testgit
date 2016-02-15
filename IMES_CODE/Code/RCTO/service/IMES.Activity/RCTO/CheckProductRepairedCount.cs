// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Get QCStatus of product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-08-21    itc202017                     create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using System.ComponentModel;

namespace IMES.Activity
{
   /// <summary>
   /// Check Repair Count
   /// </summary>
    public partial class CheckProductRepairedCount : BaseActivity
    {
        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty OverRepairedCountProperty = DependencyProperty.Register("OverRepairedCount", typeof(int), typeof(CheckProductRepairedCount), new PropertyMetadata(0));

        /// <summary>
        /// Over RepairedCount
        /// </summary>
        [DescriptionAttribute("OverRepairedCount")]
        [CategoryAttribute("OverRepairedCount")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int OverRepairedCount
        {
            get
            {
                return ((int)(base.GetValue(CheckProductRepairedCount.OverRepairedCountProperty)));
            }
            set
            {
                base.SetValue(CheckProductRepairedCount.OverRepairedCountProperty, value);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckProductRepairedCount()
        {
            InitializeComponent();
        }


        /// <summary>
        ///Check Repair Count
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        /// 

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            
            if (prod.IsFirstRepair)
            {
                if (prod.Repairs != null && prod.Repairs.Count > this.OverRepairedCount)
                {
                    throw new FisException("CQCHK1071", new string[] { this.OverRepairedCount.ToString(), prod.Repairs.Count.ToString() });
                }
            }            
            return base.DoExecute(executionContext);
        }
    }
}
