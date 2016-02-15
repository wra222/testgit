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
   
    public partial class SetReturnStation : BaseActivity
	{
		///<summary>
		///</summary>
        public SetReturnStation()
		{
			InitializeComponent();
		}

        /// <summary>
        ///  ReturnCurrentStation
        /// </summary>
        public static DependencyProperty IsReturnCurrentStationProperty = DependencyProperty.Register("IsReturnCurrentStation", typeof(bool), typeof(SetReturnStation), new PropertyMetadata(true));

        /// <summary>
        /// Return Current Station
        /// </summary>
        [DescriptionAttribute("ReturnCurrentStation")]
        [CategoryAttribute("Return Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsReturnCurrentStation
        {
            get
            {
                return ((bool)(base.GetValue(SetReturnStation.IsReturnCurrentStationProperty)));
            }
            set
            {
                base.SetValue(SetReturnStation.IsReturnCurrentStationProperty, value);
            }
        }

        /// <summary>
        ///  Store Attr Name
        /// </summary>
        public static DependencyProperty StoreAttrNameProperty = DependencyProperty.Register("StoreAttrName", typeof(string), typeof(SetReturnStation), new PropertyMetadata("FAIReturnStation"));

        /// <summary>
        /// Return Current Station
        /// </summary>
        [DescriptionAttribute("Store Attribute Name")]
        [CategoryAttribute("Return Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string StoreAttrName
        {
            get
            {
                return ((string)(base.GetValue(SetReturnStation.StoreAttrNameProperty)));
            }
            set
            {
                base.SetValue(SetReturnStation.StoreAttrNameProperty, value);
            }
        }

        /// <summary>
        ///  Return Station
        /// </summary>
        public static DependencyProperty ReturnStationProperty = DependencyProperty.Register("ReturnStation", typeof(string), typeof(SetReturnStation), new PropertyMetadata(""));

        /// <summary>
        /// Return Current Station
        /// </summary>
        [DescriptionAttribute("Return Station")]
        [CategoryAttribute("Return Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string ReturnStation
        {
            get
            {
                return ((string)(base.GetValue(SetReturnStation.ReturnStationProperty)));
            }
            set
            {
                base.SetValue(SetReturnStation.ReturnStationProperty, value);
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
            string returnStation = "";
            if (this.IsReturnCurrentStation)
            {
                returnStation = product.Status.StationId;
            }
            else
            {
                returnStation = this.ReturnStation;
            }

            if(!string.IsNullOrEmpty(this.StoreAttrName))
            {
                product.SetAttributeValue(this.StoreAttrName, returnStation, this.Editor, this.Station, ""); 
            }
            prodRep.Update(product, session.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
