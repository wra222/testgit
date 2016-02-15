// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo,InfoType,delete属于DeliveryNo的InfoType=@InfoType的所有ProductInfo
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21   Yuan XiaoWei                 create
// Known issues:
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    /// <summary>
    /// 用于根据DeliveryNo,InfoType,delete属于DeliveryNo的InfoType=@InfoType的所有ProductInfo
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取DeliveryNo,调用ProductRepository的UnPackProductInfoByDeliveryNoDefered方法
    ///           delete ProductInfo from ProductInfo as I inner join Product as P ON I.ProductID = P.ProductID
    ///           where I.InfoType=@InfoType and P.DeliveryNo=@DeliveryNo and (P.Model LIKE 'PC%' or P.Model LIKE 'QC%')
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
    ///         InfoType
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         delete ProductInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class UnPackProductInfoByDeliveryNo : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackProductInfoByDeliveryNo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// InfoType of ProductInfo to delete
        /// </summary>
        public static DependencyProperty InfoTypeProperty = DependencyProperty.Register("InfoType", typeof(string), typeof(UnPackProductInfoByDeliveryNo));

        /// <summary>
        /// InfoType of ProductInfo to delete
        /// </summary>
        [DescriptionAttribute("InfoType")]
        [CategoryAttribute("InArguments Of UnPackProductInfoByDeliveryNo")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string InfoType
        {
            get
            {
                return ((string)(base.GetValue(UnPackProductInfoByDeliveryNo.InfoTypeProperty)));
            }
            set
            {
                base.SetValue(UnPackProductInfoByDeliveryNo.InfoTypeProperty, value);
            }
        }

        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentDeliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);

            IProductRepository currentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();

            currentProductRepository.UnPackProductInfoByDeliveryNoDefered(CurrentSession.UnitOfWork, InfoType, CurrentDeliveryNo);
            return base.DoExecute(executionContext);
        }
	}
}
