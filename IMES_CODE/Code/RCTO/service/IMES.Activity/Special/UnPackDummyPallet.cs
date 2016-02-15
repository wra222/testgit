// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据指定的DummyPalletNo解除绑定
// UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
// UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                                                 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-04-06   Chen Xu (itc208014)          create
// Known issues:

using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

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
    ///         1.从Session中获取DeliveryNo，调用ProductRepository的UnPackProductByDeliveryNoDefered方法
    ///           DELETE PAK_PackkingData WHERE InternalID = @DeliveryNo
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
    public partial class UnPackDummyPallet : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public UnPackDummyPallet()
		{
			InitializeComponent();
		}


        /// <summary>
        /// 根据指定的DummyPalletNo解除绑定
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string DummyPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.DummyPalletNo);
            IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();

            iPalletRepository.DeleteDummyDetInfoByPltDefered(CurrentSession.UnitOfWork, DummyPalletNo);
            return base.DoExecute(executionContext);
        }
	}
}
