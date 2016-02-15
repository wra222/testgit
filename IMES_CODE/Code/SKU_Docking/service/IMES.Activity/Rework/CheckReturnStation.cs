using System;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Line;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.FisObject.Common.FisBOM;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using System.ComponentModel;
namespace IMES.Activity
{
 /// <summary>
 /// Set Return Station
 /// </summary>
   
    public partial class CheckReturnStation : BaseActivity
	{
		///<summary>
		///</summary>
        public CheckReturnStation()
		{
			InitializeComponent();
		}

        /// <summary>
        ///  ReturnCurrentStation
        /// </summary>
        public static DependencyProperty HasReturnStationThrowErrorProperty = DependencyProperty.Register("HasReturnStationThrowError", typeof(bool), typeof(CheckReturnStation), new PropertyMetadata(true));

        /// <summary>
        /// Return Current Station
        /// </summary>
        [DescriptionAttribute("Has ReturnStation ThrowError")]
        [CategoryAttribute("Return Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool HasReturnStationThrowError
        {
            get
            {
                return ((bool)(base.GetValue(CheckReturnStation.HasReturnStationThrowErrorProperty)));
            }
            set
            {
                base.SetValue(CheckReturnStation.HasReturnStationThrowErrorProperty, value);
            }
        }

        /// <summary>
        ///  Store Attr Name
        /// </summary>
        public static DependencyProperty AttrNameProperty = DependencyProperty.Register("AttrName", typeof(string), typeof(CheckReturnStation), new PropertyMetadata("FAIReturnStation"));

        /// <summary>
        /// Attribute Name
        /// </summary>
        [DescriptionAttribute("Attribute Name")]
        [CategoryAttribute("Return Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string AttrName
        {
            get
            {
                return ((string)(base.GetValue(CheckReturnStation.AttrNameProperty)));
            }
            set
            {
                base.SetValue(CheckReturnStation.AttrNameProperty, value);
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            Session session = CurrentSession;
            IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
            string value = product.GetAttributeValue(this.AttrName);

            if (!string.IsNullOrEmpty(value))
            {
                if (HasReturnStationThrowError)
                {
                    throw new FisException("CQCHK50010", new List<string>{this.AttrName, value, this.Station} );
                }
            }
            else if (!HasReturnStationThrowError)
            {
                throw new FisException("CQCHK50011", new List<string> { this.AttrName, this.Station });  
            }          
            return base.DoExecute(executionContext);
        }
	}
}
