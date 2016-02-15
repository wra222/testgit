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
    /// 1.检查平板的电池信息
    /// 2.检查ProductInfo 里MAC与Product.MAC 比对 信息
    /// </summary>
    public partial class CheckBatteryAndProductInfo : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckBatteryAndProductInfo()
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
            if (newProduct != null)
            {
                if (NeedCheckBatteryUploadTime(newProduct))//需要检查电池上传时间
                {
                     ProductLog passlast66log=   prodRep.GetLatestLogByWcAndStatus(newProduct.ProId, "66", 1);
                     IList<IMES.FisObject.FA.Product.ProductInfo> productInfoList = newProduct.ProductInfoes.Where(x => x.InfoType == CheckBatteryInfoType && !string.IsNullOrEmpty(x.InfoValue)).ToList();
                     if (productInfoList == null || productInfoList.Count == 0)
                     {
                         throw new FisException("CHK1072", new string[] { newProduct.CUSTSN, CheckBatteryInfoType });
                     }
                     if (passlast66log == null)
                     {
                         throw new FisException("CHK1073", new string[] { newProduct.CUSTSN });
                     }
                     //上传电池电量的Udt 必须小于66 的Cdt
                     if (passlast66log.Cdt > productInfoList[0].Udt)
                     {
                         throw new FisException("CHK1074", new string[] { newProduct.CUSTSN });
                     }

                }
                if (NeedCheckMAC_ADDR1(newProduct))//需要检查MAC_ADDR1
                {
                    IList<IMES.FisObject.FA.Product.ProductInfo> productInfoList = newProduct.ProductInfoes.Where(x => x.InfoType == CheckMACaddr1InfoType && !string.IsNullOrEmpty(x.InfoValue)).ToList();
                    if (productInfoList == null || productInfoList.Count == 0)
                    {
                        throw new FisException("CHK1072", new string[] { newProduct.CUSTSN,CheckMACaddr1InfoType });
                    }
                    if (newProduct.MAC.Replace(":", "").Trim() != productInfoList[0].InfoValue.Replace(":", "").Trim())
                    {
                        throw new FisException("CHK1077", new string[] { newProduct.CUSTSN.Trim(), CheckMACaddr1InfoType });
                    }
                }
            }
            return base.DoExecute(executionContext);
        }
        private bool NeedCheckBatteryUploadTime(IProduct p)
        {
            bool needchecklog = false;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("CheckTabletBatteryTime");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo vi in lstConstValueType)
                {
                    if (p.Family.Equals(vi.value) )
                    {
                        needchecklog = true;
                        break;
                    }
                }
            return needchecklog;
        }
        private bool NeedCheckMAC_ADDR1(IProduct p)
        {
            bool needchecklog = false;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("CheckMAC_ADDR1");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo vi in lstConstValueType)
                {
                    if (p.Family.Equals(vi.value))
                    {
                        needchecklog = true;
                        break;
                    }
                }
            return needchecklog;
        }
        #region 参数
        /// <summary>
        /// 电池电量Infotype
        /// </summary>
        public static DependencyProperty CheckBatteryInfoTypeProperty = DependencyProperty.Register("CheckBatteryInfoType", typeof(string), typeof(CheckBatteryAndProductInfo),new PropertyMetadata("BATW"));
        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("CheckBatteryInfoType")]
        [CategoryAttribute("CheckBatteryInfoType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
      
        public string CheckBatteryInfoType
        {
            get
            {
                return ((string)(base.GetValue(CheckBatteryAndProductInfo.CheckBatteryInfoTypeProperty)));
            }
            set
            {
                base.SetValue(CheckBatteryAndProductInfo.CheckBatteryInfoTypeProperty, value);
            }
        }

        /// <summary>
        /// ProductLog
        /// </summary>
        public static DependencyProperty CheckBatteryLogProperty = DependencyProperty.Register("CheckBatteryLog", typeof(string), typeof(CheckBatteryAndProductInfo), new PropertyMetadata("66"));

        /// <summary>
        /// ProductLog.Cdt 与电池电量的Udt比对
        /// </summary>
        [DescriptionAttribute("CheckBatteryLog")]
        [CategoryAttribute("CheckBatteryLog Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CheckBatteryLog
        {
            get
            {
                return ((string)(base.GetValue(CheckBatteryAndProductInfo.CheckBatteryLogProperty)));
            }
            set
            {
                base.SetValue(CheckBatteryAndProductInfo.CheckBatteryLogProperty, value);
            }
        }
        /// <summary>
        /// MAC_ADDR1
        /// </summary>
        public static DependencyProperty CheckMACaddr1InfoTypeProperty = DependencyProperty.Register("CheckMACaddr1InfoType", typeof(string), typeof(CheckBatteryAndProductInfo), new PropertyMetadata("MAC_ADDR1"));
       /// <summary>
       /// 
       /// </summary>
        [DescriptionAttribute("CheckMACaddr1InfoType")]
        [CategoryAttribute("CheckMACaddr1InfoType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        
        public string CheckMACaddr1InfoType
        {
            get
            {
                return ((string)(base.GetValue(CheckBatteryAndProductInfo.CheckMACaddr1InfoTypeProperty)));
            }
            set
            {
                base.SetValue(CheckBatteryAndProductInfo.CheckMACaddr1InfoTypeProperty, value);
            }
        }


        #endregion
    }
}
