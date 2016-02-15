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
    /// 檢查ProductLog 上站過站記錄，以及曾經過站Log.
    /// CheckLastProcessStation:設置最後出站站點(多筆使用'~'','分隔開來),預設為空值不檢查
    ///CheckPassProcessStation:設置過站站點(多筆使用'~'','分隔開來),預設為空值不檢查
    ///ExistsThrowError:假如有檢查到以上條件符合要報錯，預設:True, False:不符合設置條件以上報錯
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///     
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///        
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              Product
    /// </para> 
    /// </remarks>
    public partial class CheckProductLog : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public CheckProductLog()
		{
			InitializeComponent();
		}
        /// <summary>
        /// Write Product 最後獲站記錄
        /// </summary>
        public static DependencyProperty CheckLastProcessStationProperty = DependencyProperty.Register("CheckLastProcessStation", typeof(string), typeof(CheckProductLog));

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
                return ((string)(base.GetValue(CheckProductLog.CheckLastProcessStationProperty)));
            }
            set
            {
                base.SetValue(CheckProductLog.CheckLastProcessStationProperty, value);
            }
        }
        /// <summary>
        /// Write Product 过站记录
        /// </summary>
        public static DependencyProperty CheckPassProcessStationProperty = DependencyProperty.Register("CheckPassProcessStation", typeof(string), typeof(CheckProductLog));

        /// <summary>
        /// Product   历史过站记录
        /// </summary>
        [DescriptionAttribute("CheckPassProcessStation")]
        [CategoryAttribute("CheckPassProcessStation Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string CheckPassProcessStation
        {
            get
            {
                return ((string)(base.GetValue(CheckProductLog.CheckPassProcessStationProperty)));
            }
            set
            {
                base.SetValue(CheckProductLog.CheckPassProcessStationProperty, value);
            }
        }
        /// <summary>
        /// 抛错类型True：检查数据就报错。False：检查无数据就抛错.默认是True
        /// </summary>
        public static DependencyProperty ExistsThrowErrorProperty = DependencyProperty.Register("ExistsThrowError", typeof(bool), typeof(CheckProductLog),new PropertyMetadata(true));

        /// <summary>
        /// Product 抛错类型
        /// </summary>
        [DescriptionAttribute("ExistsThrowError")]
        [CategoryAttribute("ExistsThrowError Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool ExistsThrowError
        {
            get
            {
                return ((bool)(base.GetValue(CheckProductLog.ExistsThrowErrorProperty)));
            }
            set
            {
                base.SetValue(CheckProductLog.ExistsThrowErrorProperty, value);
            }
        }
        /// <summary>
        /// 设置例外，默认是False.  如果是Ture 根据ConstValueType ，抓取是否需要检查LOG
        /// </summary>
        public static DependencyProperty IsExceptProperty = DependencyProperty.Register("IsExcept", typeof(bool), typeof(CheckProductLog), new PropertyMetadata(false));

        /// <summary>
        /// Product 抛错类型
        /// </summary>
        [DescriptionAttribute("IsExcept")]
        [CategoryAttribute("IsExcept Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsExcept
        {
            get
            {
                return ((bool)(base.GetValue(CheckProductLog.IsExceptProperty)));
            }
            set
            {
                base.SetValue(CheckProductLog.IsExceptProperty, value);
            }
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
                if (IsExcept)//需要进行例外检查
                {
                    if (!NeedCheckLog(newProduct))//不需要检查所有LOG
                    {
                        return base.DoExecute(executionContext);
                    }
                }
                bool bFoundPassLog = false;
                bool bFoundLastLog = false;
                IList<ProductLog> productLogList = newProduct.ProductLogs.OrderBy(p=>p.Cdt).ToList();
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

                if (!string.IsNullOrEmpty(CheckPassProcessStation))
                {
                    if (count > 0)
                    {
                        var stationList = CheckPassProcessStation.Split(new char[] { ',', '~' });
                        bFoundPassLog = productLogList.Where(x => stationList.Contains(x.Station) && x.Status == StationStatus.Pass).Any();
                    }
                }
                if (string.IsNullOrEmpty(CheckLastProcessStation) && string.IsNullOrEmpty(CheckPassProcessStation))
                {
                    return base.DoExecute(executionContext);
                }

                //First Check LastLog
                if (!string.IsNullOrEmpty(CheckLastProcessStation))
                {
                    checkThrowError(bFoundLastLog, newProduct.ProId, CheckLastProcessStation);
                }

                //Second Check passlog
                if (!string.IsNullOrEmpty(CheckPassProcessStation))
                {
                    checkThrowError(bFoundPassLog, newProduct.ProId, CheckPassProcessStation);
                }             
                  
            }

            return base.DoExecute(executionContext);
        }

        private void checkThrowError(bool bFound, string productId, string checkStation)
        {
            if (ExistsThrowError)//设定true 检查到有值需要抛错
            {
                if (bFound)
                {
                    FisException ex = new FisException("CQCHK1076", new List<string> { productId, checkStation });
                    throw ex;
                }
            }
            else
            {
                if (!bFound)//无值需要提示未过此站
                {
                    FisException ex = new FisException("CQCHK1077", new List<string> { productId, checkStation });
                    throw ex;
                }
            }
        }
        private bool NeedCheckLog(IProduct p)
        {
            bool needchecklog = true;
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> lstConstValueType = partRepository.GetConstValueTypeList("NoCheckProductLogSN");
            if (null != lstConstValueType)
                foreach (ConstValueTypeInfo vi in lstConstValueType)
                {
                    if (null != p.CUSTSN && p.CUSTSN.Equals(vi.value))
                    {
                        needchecklog = false;
                        break;
                    }
                }
            if (needchecklog)
            {
                lstConstValueType = partRepository.GetConstValueTypeList("NoCheckProductLogModel");
                if (null != lstConstValueType)
                {
                    foreach (ConstValueTypeInfo vi in lstConstValueType)
                    {
                        if (null != p.Model && p.Model.Equals(vi.value))
                        {
                            needchecklog = false;
                            break;
                        }
                    }

                    if (needchecklog)
                    {
                        foreach (ConstValueTypeInfo vi in lstConstValueType)
                        {
                            if (null != p.Family && p.Family.Equals(vi.value))
                            {
                                needchecklog = false;
                                break;
                            }
                        }
                    }
                }
            }


            return needchecklog;
        }
    }
}
