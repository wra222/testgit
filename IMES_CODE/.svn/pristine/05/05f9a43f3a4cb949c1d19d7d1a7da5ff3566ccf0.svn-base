// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 根据DeliveryNo号码，UnPack属于DeliveryNo的所有Product
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21                   create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// 用于UnPack属于DeliveryNo的所有Product
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
    ///         1.从Session中获取DeliveryNo，调用ProductRepository的Update方法
    ///           update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0
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
    ///         更新Product
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              
    /// </para> 
    /// </remarks>
    public partial class UnPackProductByDN : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackProductByDN()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 执行根据DeliveryNo修改所有属于该DeliveryNo的Product状态的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            string unpackType = ((string)CurrentSession.GetValue(Session.SessionKeys.CN));
            bool isPallet = ((bool)CurrentSession.GetValue(Session.SessionKeys.Pallet));
            productRepository.BackUpProduct(currentProduct.ProId,this.Editor);

            ShipBoxDetInfo setValue = new ShipBoxDetInfo();
            ShipBoxDetInfo condition = new ShipBoxDetInfo();
            setValue.snoId = "";
            condition.snoId = currentProduct.ProId;

            deliveryRepository.UpdateShipBoxDetInfo(setValue, condition);

            if (unpackType == "ALL")
            {
                currentProduct.PizzaID = string.Empty;
                currentProduct.CartonSN = string.Empty;
            }
            currentProduct.PalletNo = string.Empty;
            if (!isPallet)
            {
                currentProduct.DeliveryNo = string.Empty;
            }
            currentProduct.CartonWeight = 0;
            currentProduct.UnitWeight = 0;

            productRepository.Update(currentProduct, CurrentSession.UnitOfWork);

            productRepository.BackUpProductStatus(currentProduct.ProId, this.Editor);

            return base.DoExecute(executionContext);
        }
	}
}
