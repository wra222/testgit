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
	public partial class CheckShipForPalletVerify: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckShipForPalletVerify()
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
            string palletNo = currentProduct.PalletNo;
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IProductRepository ProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            List<string> erpara = new List<string>();
            FisException ex;

            string Consolidated = string.Empty;
            string curDn = currentProduct.DeliveryNo;
            Consolidated = DeliveryRepository.GetDeliveryInfoValue(curDn, "Consolidated");
            // SVN 2569: 如果Consolidated 属性不存在或者为空，则不用进行Delivery Download Check
            if (!string.IsNullOrEmpty(Consolidated))
            {
                string[] pattern = Consolidated.Split('/');
                string ConsolidateNo = string.Empty;
                int dnQty = 0;
                if (pattern.Length.ToString() != "2" || string.IsNullOrEmpty(pattern[0]) || string.IsNullOrEmpty(pattern[1]))
                {
                    erpara.Add(curDn);
                    ex = new FisException("PAK024", erpara);  //找不到该Delivery No 的Consolidated 属性
                    throw ex;
                }
                ConsolidateNo = pattern[0];
                dnQty = Int32.Parse(pattern[1]);

                //使用Consolidate No 检索IMES_PAK.DeliveryInfo 表，取得相关记录，统计这些记录共有多少个不同的LEFT(DeliveryNo，10)，该数据如果小于前面查询到的Delivery 的Consolidated 属性中定义的并板的Delivery 数量，则报告错误：“Delivery No 未完全Download!”
                int DistinctDNQty = 0;
                DistinctDNQty = DeliveryRepository.GetDistinctDeliveryNo(ConsolidateNo);
                if (DistinctDNQty < dnQty)
                {
                    int fullProdQty = DeliveryRepository.GetDnCountBySP(ConsolidateNo+"%");
                    if (DistinctDNQty + fullProdQty != dnQty)
                    {
                        erpara.Add(curDn);
                        ex = new FisException("PAK018", erpara);    //Delivery No 未完全Download!
                        throw ex;
                    }
                }

            }

            return base.DoExecute(executionContext);
        }
	}
}
