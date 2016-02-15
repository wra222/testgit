/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Dismantle WM
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 
 * Known issues:Any restrictions about this file 
 */


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
using IMES.FisObject.Common .Process;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Part;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DismantleXYZ: BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public DismantleXYZ()
        {
            InitializeComponent();
        }
		
		/// <summary>
        ///  Dismantle InfoTypes
        /// </summary>
        public static DependencyProperty DismantleInfoTypesProperty = DependencyProperty.Register("DismantleInfoTypes", typeof(string), typeof(DismantleXYZ), new PropertyMetadata(""));
        /// <summary>
        /// Dismantle InfoTypes
        /// </summary>
        [DescriptionAttribute("DismantleInfoTypes")]
        [CategoryAttribute("DismantleInfoTypes")]
        [BrowsableAttribute(true)]        
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string DismantleInfoTypes
        {
            get
            {
                return ((string)(base.GetValue(DismantleXYZ.DismantleInfoTypesProperty)));
            }
            set
            {
                base.SetValue(DismantleXYZ.DismantleInfoTypesProperty, value);
            }
        }

        /// <summary>
        /// 根据Product，删除 ProductInfo表中的WM数据
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> erpara = new List<string>();
            List<string> lstValuetype = new List<string>();
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            string[] xyz = DismantleInfoTypes.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
			//lstValuetype.Add("WM");        
            //删除ProductInfo表 RemoveProductInfosByTypeDefered
            //productRepository.RemoveProductInfosByTypeDefered(CurrentSession.UnitOfWork,currentProduct.ProId,lstValuetype);                
			productRepository.RemoveProductInfosByTypeDefered(CurrentSession.UnitOfWork, currentProduct.ProId, xyz.ToArray() );

            return base.DoExecute(executionContext);
        }
    }
}
