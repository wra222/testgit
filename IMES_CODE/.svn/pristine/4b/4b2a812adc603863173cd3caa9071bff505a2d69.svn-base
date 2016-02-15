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

namespace IMES.Activity.CQ
{
    /// <summary>
    /// Tablet check last log 6K
    /// </summary>
    public partial class CheckTabletIamgeLog : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckTabletIamgeLog()
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
            bool bFoundLastLog = false;
            if (newProduct != null)
            {

                if (NeedCheckLog(newProduct))//需要检查LOG
                {
                    IList<ProductLog> productLogList = newProduct.ProductLogs.OrderBy(p => p.Cdt).ToList();
                    int count = productLogList.Count;
                    if (!string.IsNullOrEmpty(CheckLastProcessStation))
                    {
                        if (count > 0)
                        {
                            var stationList = CheckLastProcessStation.Split(new char[] { ',', '~' });
                            ProductLog lastProductLog = productLogList[count - 1];
                            if (stationList.Contains(lastProductLog.Station) && lastProductLog.Status == StationStatus.Pass)
                            {
                                bFoundLastLog = true;
                            }
                        }
                    }
                    if (!bFoundLastLog && NoExistsThrowError)
                    {
                        FisException ex = new FisException("CQCHK1077", new List<string> { newProduct.ProId, CheckLastProcessStation });
                        throw ex;
                    }
                }
                else
                {
                    return base.DoExecute(executionContext);
                }
            }
           
            return base.DoExecute(executionContext);
        }
        /// <summary>
        /// Write Product 最後獲站記錄
        /// </summary>
        public static DependencyProperty CheckLastProcessStationProperty = DependencyProperty.Register("CheckLastProcessStation", typeof(string), typeof(CheckTabletIamgeLog), new PropertyMetadata("6K"));

        /// <summary>
        /// Product 最後獲站
        /// </summary>
        [DescriptionAttribute("CheckLastProcessStation")]
        [CategoryAttribute("CheckLastProcessStation Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CheckLastProcessStation
        {
            get
            {
                return ((string)(base.GetValue(CheckTabletIamgeLog.CheckLastProcessStationProperty)));
            }
            set
            {
                base.SetValue(CheckTabletIamgeLog.CheckLastProcessStationProperty, value);
            }
        }

        /// <summary>
        /// 抛错类型True：检查数据就报错。False：检查无数据就抛错.默认是True
        /// </summary>
        public static DependencyProperty ExistsThrowErrorProperty = DependencyProperty.Register("ExistsThrowError", typeof(bool), typeof(CheckTabletIamgeLog), new PropertyMetadata(true));

        /// <summary>
        /// Product 抛错类型
        /// </summary>
        [DescriptionAttribute("ExistsThrowError")]
        [CategoryAttribute("ExistsThrowError Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NoExistsThrowError
        {
            get
            {
                return ((bool)(base.GetValue(CheckTabletIamgeLog.ExistsThrowErrorProperty)));
            }
            set
            {
                base.SetValue(CheckTabletIamgeLog.ExistsThrowErrorProperty, value);
            }
        }
        
        private bool NeedCheckLog(IProduct p)
        {
            bool needchecklog = false;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueInfo> lstConstValueType = partRepository.GetConstValueListByType("TabletImageCheck");
            if (null != lstConstValueType)
                foreach (ConstValueInfo vi in lstConstValueType)
                {
                    if (p.Family.Equals(vi.name)&&this.Station.Equals(vi.value))
                    {
                        needchecklog = true;
                        break;
                    }
                }
            return needchecklog;
        }
	}
}
