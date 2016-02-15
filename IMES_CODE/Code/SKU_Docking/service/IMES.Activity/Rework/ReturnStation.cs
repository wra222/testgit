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
   
    public partial class ReturnStation : BaseActivity
	{
		///<summary>
		///</summary>
        public ReturnStation()
		{
			InitializeComponent();
		}

    
        /// <summary>
        ///  Attr Name
        /// </summary>
        public static DependencyProperty AttrNameProperty = DependencyProperty.Register("AttrName", typeof(string), typeof(ReturnStation), new PropertyMetadata("FAIReturnStation"));

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
                return ((string)(base.GetValue(ReturnStation.AttrNameProperty)));
            }
            set
            {
                base.SetValue(ReturnStation.AttrNameProperty, value);
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
            string returnStation = product.GetAttributeValue(this.AttrName);

            if (!string.IsNullOrEmpty(returnStation))
            {
                 IMES.FisObject.FA.Product.ProductStatus preStatus = product.Status;
              
                IList<IMES.DataModel.TbProductStatus> preStationList = new List<TbProductStatus>();
                preStationList.Add(new TbProductStatus
                {
                    ProductID = preStatus.ProId,
                    Line = preStatus.Line,
                    ReworkCode = preStatus.ReworkCode,
                    Station = preStatus.StationId,
                    Status = (int)preStatus.Status,
                    Editor = preStatus.Editor,
                    TestFailCount = preStatus.TestFailCount,
                    Udt=DateTime.Now
                });
                IMES.FisObject.FA.Product.ProductStatus curStatus = new IMES.FisObject.FA.Product.ProductStatus()
                {
                    ProId = preStatus.ProId,
                    ReworkCode = preStatus.ReworkCode,
                    TestFailCount = preStatus.TestFailCount,
                    Editor = this.Editor,
                    Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                    StationId = returnStation,
                    Line = this.Line,
                    Cdt = DateTime.Now,
                    Udt = DateTime.Now
                };
                product.UpdateStatus(curStatus);

                var productLog = new ProductLog
                {
                    Model = product.Model,
                    Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                    Editor = this.Editor,
                    Line = this.Line,
                    Station = returnStation,
                    Cdt = DateTime.Now
                };

                product.AddLog(productLog);
                prodRep.Update(product, session.UnitOfWork);

                prodRep.UpdateProductPreStationDefered(CurrentSession.UnitOfWork, preStationList);            
                
            }
          
            
            return base.DoExecute(executionContext);
        }
	}
}
