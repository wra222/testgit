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
	public partial class CheckPalletForPalletVerifyReprint: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckPalletForPalletVerifyReprint()
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

            string palletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            List<string> erpara = new List<string>();
            FisException ex;

            int qty= DeliveryRepository.GetSumofDeliveryQtyFromDeliveryPallet( palletNo);
            int qty1 = ProductRepository.GetFactCartonQtyByPalletNo(palletNo);
            if (qty != qty1)
            {
                erpara.Add(palletNo);
                ex = new FisException("CHK878", erpara);
                throw ex;
            }
            IList<IProduct> prodList = ProductRepository.GetProductListByPalletNo2(palletNo);
            if (prodList.Count > 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.Product, prodList[0]);
            }
            else
            {
                erpara.Add(palletNo);
                ex = new FisException("CHK878", erpara);
                throw ex;
            }
            //CurrentSession.AddValue(Session.SessionKeys.PalletQty, qty);
            //CurrentSession.AddValue(Session.SessionKeys.PalletNo, palletNo);
            return base.DoExecute(executionContext);
        }
	}
}
