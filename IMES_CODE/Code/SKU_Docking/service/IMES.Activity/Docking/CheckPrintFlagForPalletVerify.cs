// INVENTEC corporation (c)2009 all rights reserved. 
// Description:  根据输入的PalletNo,获取Pallet对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-02   Yuan XiaoWei                 create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using System.Collections.Generic;
using System;
using IMES.DataModel;


namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的PalletNo,获取Pallet对象，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Pallet为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据PalletNo，调用IPalletRepository的Find方法，获取Pallet对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PalletNo
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Pallet
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPalletRepository
    ///              Pallet
    /// </para> 
    /// </remarks>
	public partial class CheckPrintFlagForPalletVerify: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckPrintFlagForPalletVerify()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Get Pallet Object and put it into Session.SessionKeys.Pallet
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            string palletNo = currentProduct.PalletNo;
            string DeliveryNo = currentProduct.DeliveryNo;
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            string flag = DeliveryRepository.GetDeliveryInfoValue(DeliveryNo, "Flag");
            CurrentSession.AddValue(Session.SessionKeys.EEP, flag);

            return base.DoExecute(executionContext);
        }
	}
}
