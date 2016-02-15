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

namespace IMES.Activity
{
    /// <summary>
    /// 檢查上傳ProductLog
    /// </summary>
	public partial class CheckLastProductLog: BaseActivity
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public CheckLastProductLog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check Status 
        /// </summary>
        public static DependencyProperty CheckStatusProperty = DependencyProperty.Register("CheckStatus", typeof(IMES.FisObject.Common.Station.StationStatus), typeof(CheckLastProductLog), new PropertyMetadata(IMES.FisObject.Common.Station.StationStatus.Pass));

        /// <summary>
        ///  Check Status
        /// </summary>
        [DescriptionAttribute("Check Status")]
        [CategoryAttribute("Check Status")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public IMES.FisObject.Common.Station.StationStatus  CheckStatus
        {
            get
            {
                return ((IMES.FisObject.Common.Station.StationStatus)(base.GetValue(CheckLastProductLog.CheckStatusProperty)));
            }
            set
            {
                base.SetValue(CheckLastProductLog.CheckStatusProperty, value);
            }
        }


        /// <summary>
        /// Check Station 
        /// </summary>
        public static DependencyProperty CheckStationProperty = DependencyProperty.Register("CheckStation", typeof(string), typeof(CheckLastProductLog), new PropertyMetadata("LT"));

        /// <summary>
        ///  Check Station
        /// </summary>
        [DescriptionAttribute("CheckStation")]
        [CategoryAttribute("CheckStation")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string  CheckStation
        {
            get
            {
                return ((string)(base.GetValue(CheckLastProductLog.CheckStationProperty)));
            }
            set
            {
                base.SetValue(CheckLastProductLog.CheckStationProperty, value);
            }
        }


        /// <summary>
        /// Check Station log
        /// </summary>
        public static DependencyProperty CheckLastStationLogProperty = DependencyProperty.Register("CheckLastStationLog", typeof(bool), typeof(CheckLastProductLog), new PropertyMetadata(true));

        /// <summary>
        ///  Check Station
        /// </summary>
        [DescriptionAttribute("Check Station log")]
        [CategoryAttribute("Check Station log")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool CheckLastStationLog
        {
            get
            {
                return ((bool)(base.GetValue(CheckLastProductLog.CheckLastStationLogProperty)));
            }
            set
            {
                base.SetValue(CheckLastProductLog.CheckLastStationLogProperty, value);
            }
        }

        /// <summary>
        /// Check main logical 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
         protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string prod = currentProduct.ProId;
            IProductRepository prodRep =RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            
            ProductLog prodLog=null;
            if (this.CheckLastStationLog)
            {
                prodLog = prodRep.GetLatestLogByWc(prod, this.CheckStation);
            }
            else
            {
                prodLog = prodRep.GetLatestLog(prod);
            }

              if (prodLog == null || currentProduct.Status.Udt > prodLog.Cdt)
              {
                  throw new FisException("CQCHK0002", new string[] { });
              }

             if (prodLog.Station != this.CheckStation.Trim() || 
                 prodLog.Status != this.CheckStatus)
            {
                throw new FisException("CQCHK0003", new string[] { });
            }
                       
	        return base.DoExecute(executionContext);
        }
	}
}

