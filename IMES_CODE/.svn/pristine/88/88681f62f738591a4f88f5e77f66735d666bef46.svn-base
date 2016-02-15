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

using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// Check Product Is CleanRoom
    /// </summary>
    public partial class CheckProductIsCleanRoom : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckProductIsCleanRoom()
		{
			InitializeComponent();
		}
         
        /// <summary>
        /// CheckedResultVariable
        /// </summary>
        public static DependencyProperty CheckedResultVariableProperty = DependencyProperty.Register("CheckedResultVariable", typeof(string), typeof(CheckProductIsCleanRoom), new PropertyMetadata(""));

        /// <summary>
        ///  Session Name Of CartonSN
        /// </summary>
        [DescriptionAttribute("CheckedResultVariable")]
        [CategoryAttribute("Session Name Of CheckedResult Variable")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CheckedResultVariable
        {
            get
            {
                return ((string)(base.GetValue(CheckProductIsCleanRoom.CheckedResultVariableProperty)));
            }
            set
            {
                base.SetValue(CheckProductIsCleanRoom.CheckedResultVariableProperty, value);
            }
        }



        /// <summary>
        /// Check Product Is CleanRoom
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (currentProduct == null)
            {
                var ex1 = new FisException("SFC002", new string[] { "" });
                throw ex1;
            }

            CurrentSession.AddValue(CheckedResultVariable, "N");

            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
          //  IList<ProductPart> pp = prodRep.GetProductPartByPartTypeLike(currentProduct.ProId, "Clean Room");
              IList<ProductPart> pp = prodRep.GetProductPartByPartTypeLike(currentProduct.ProId, "LCM");
            if (pp != null && pp.Count > 0)
            {
                IProduct cleanroomProduct = prodRep.GetProductByIdOrSn(pp[0].PartSn);
                if (cleanroomProduct != null)
                {
                    CurrentSession.AddValue(CheckedResultVariable, "Y");
                    CurrentSession.AddValue("Backup" + Session.SessionKeys.Product, currentProduct);
                    CurrentSession.AddValue(Session.SessionKeys.Product, cleanroomProduct);
                }
            }

            return base.DoExecute(executionContext);
        }
	}
}
