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
    ///Check Same Model
    /// </summary>
  
    public partial class CheckSameModelforCleanRoom : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckSameModelforCleanRoom()
		{
			InitializeComponent();
		}

        

        /// <summary>
        /// Carton Location 禁用時要停止workflow
        /// </summary>
        public static DependencyProperty IsStopWFProperty = DependencyProperty.Register("IsStopWF", typeof(bool), typeof(CheckSameModelforCleanRoom), new PropertyMetadata(true));
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
                return ((bool)(base.GetValue(CheckSameModelforCleanRoom.IsStopWFProperty)));
            }
            set
            {
                base.SetValue(CheckSameModelforCleanRoom.IsStopWFProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
            if (prodList == null || prodList.Count == 0)
            {
                FisException e = new FisException("CHK975", new string[] {""});
                e.stopWF = this.IsStopWF;
                throw e;
            }
            IProduct prod = prodList[0];
            if (currentProduct.Model != prod.Model)
            {
                FisException e = new FisException("CHK1015", new string[] { currentProduct.Model });
                e.stopWF = this.IsStopWF;
                throw e;
            }
            return base.DoExecute(executionContext);
          
        }
	}
}
