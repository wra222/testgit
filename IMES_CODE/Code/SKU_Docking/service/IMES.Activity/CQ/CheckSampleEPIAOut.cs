using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Linq;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using System.Data.SqlClient;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// 针对急出货机器快速过EPIA ,需要check SN是否维护，还需要check download 完成距离现在时间不能超过维护时间
    /// </summary>
    public partial class CheckSampleEPIAOut : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckSampleEPIAOut()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
             Product newProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            // check SN Must in ConstValueType.Tyep='SampleEPIASNList'
            if (!partRepository.GetConstValueTypeList("SampleEPIASNList").Any(x => x.value == newProduct.CUSTSN))
            {
                FisException ex = new FisException("CQCHK1112", new List<string> { "SampleEPIASNList" });
                throw ex;
            }
            // check run in 时间到现在时间 必须小于维护。

            ProductLog downloadLog = prodRep.GetLatestLogByWcAndStatus(newProduct.ProId, this.CheckDefaultDLStation.Trim(), 1);
            int allowedTime = -1;
           IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("SampleEPIADLAllowTime");
           if (lstConstValueType != null && lstConstValueType.Count > 0)
           {
               allowedTime = Convert.ToInt32(lstConstValueType[0].value);
           }
            if (downloadLog == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.CheckDefaultDLStation.Trim());
                throw new FisException("CHK1008", errpara);
            }
            DateTime time66 = downloadLog.Cdt;
            if (allowedTime < (DateTime.Now - time66).TotalMinutes)
            {
                List<string> errpara = new List<string>();
                errpara.Add(allowedTime.ToString());
                throw new FisException("CQCHK1113", errpara); 
            }
            



            return base.DoExecute(executionContext);
        }
        /// <summary>
        /// DefaultCheckDLStation
        /// </summary>
        public static DependencyProperty CheckDefaultDLStationProperty = DependencyProperty.Register("CheckDefaultDLStation", typeof(string), typeof(CheckSampleEPIAOut), new PropertyMetadata("66"));

        /// <summary>
        ///  Check Default DL Station
        /// </summary>
        [DescriptionAttribute("DefaultCheckDLStation")]
        [CategoryAttribute("Setup ImageDown Station Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CheckDefaultDLStation
        {
            get
            {
                return ((string)(base.GetValue(CheckSampleEPIAOut.CheckDefaultDLStationProperty)));
            }
            set
            {
                base.SetValue(CheckSampleEPIAOut.CheckDefaultDLStationProperty, value);
            }
        }

	}
}
