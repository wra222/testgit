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

namespace IMES.Activity
{
    /// <summary>
    /// UpdateProductStatus And ProdcutStatusEx and Write ProdutLog
    /// </summary>
    public partial class UpdateMultiProductStatusAndLog : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public UpdateMultiProductStatusAndLog()
		{
			InitializeComponent();
		}
         
        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StationStatus), typeof(UpdateMultiProductStatusAndLog), new PropertyMetadata(StationStatus.Pass));

        /// <summary>
        /// Status of Product
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public StationStatus Status
        {
            get
            {
                return ((StationStatus)(base.GetValue(UpdateMultiProductStatusAndLog.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateMultiProductStatusAndLog.StatusProperty, value);
            }
        }

        /// <summary>
        ///  Update Station 
        /// </summary>
        public static DependencyProperty IsUpdateStationProperty = DependencyProperty.Register("IsUpdateStation", typeof(bool), typeof(UpdateMultiProductStatusAndLog), new PropertyMetadata(true));

        /// <summary>
        /// IsUpdateStation
        /// </summary>
        [DescriptionAttribute("IsUpdateStation")]
        [CategoryAttribute("IsUpdateStation Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsUpdateStation
        {
            get
            {
                return ((bool)(base.GetValue(UpdateMultiProductStatusAndLog.IsUpdateStationProperty)));
            }
            set
            {
                base.SetValue(UpdateMultiProductStatusAndLog.IsUpdateStationProperty, value);
            }
        }

        /// <summary>
        ///  Write Log file
        /// </summary>
        public static DependencyProperty IsWriteLogProperty = DependencyProperty.Register("IsWriteLog", typeof(bool), typeof(UpdateMultiProductStatusAndLog), new PropertyMetadata(true));

        /// <summary>
        /// IsWriteLog
        /// </summary>
        [DescriptionAttribute("IsWriteLog")]
        [CategoryAttribute("IsWriteLog Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsWriteLog
        {
            get
            {
                return ((bool)(base.GetValue(UpdateMultiProductStatusAndLog.IsWriteLogProperty)));
            }
            set
            {
                base.SetValue(UpdateMultiProductStatusAndLog.IsWriteLogProperty, value);
            }
        }



        /// <summary>
        /// Update Product Status
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> productIdList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.ProdNoList);

            if (productIdList == null)
            {
                if (this.IsUpdateStation)
                {
                    // update ProductStatusEx table
                    prodRep.UpdateProductPreStationDefered(CurrentSession.UnitOfWork, productIdList);

                    string AllowPass = "";
                    int testFailCount = 0;
                    if (CurrentSession.GetValue(ExtendSession.SessionKeys.AllowPass) != null)
                    {
                        AllowPass = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.AllowPass);
                    }

                    if (AllowPass == "N" && Status == StationStatus.Fail)
                    {
                        testFailCount = 999;
                    }

                    //update ProductStatus table
                    prodRep.UpdateProductStatusDefered(CurrentSession.UnitOfWork, 
                                                                                    productIdList, 
                                                                                    this.Line, 
                                                                                    this.Station, 
                                                                                    (int)this.Status, 
                                                                                    testFailCount, 
                                                                                    this.Editor);
                }
                if (this.IsWriteLog)
                {
                    //Write ProductLog
                    prodRep.WriteProductLogDefered(CurrentSession.UnitOfWork, 
                                                                            productIdList, 
                                                                            this.Line,
                                                                            this.Station, 
                                                                            (int)this.Status, 
                                                                            this.Editor);
                }
            }  
            return base.DoExecute(executionContext);
        }
	}
}
