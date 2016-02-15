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
    public partial class CheckPalletDownForPalletVerify : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public CheckPalletDownForPalletVerify()
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
            ////临时注释掉FisException ex;

            string Consolidated = string.Empty;
            string curDn = currentProduct.DeliveryNo;
            Consolidated = DeliveryRepository.GetDeliveryInfoValue(curDn, "Consolidated");
            // SVN 2569: 如果Consolidated 属性不存在或者为空，则不用进行Delivery Download Check
            if (!string.IsNullOrEmpty(Consolidated))
            {
                int SumCartonQty = 0;
                int SumDnPallletQty = 0;
                IList<Delivery> dnList = DeliveryRepository.GetDeliveryListByInfoTypeAndValue("Consolidated", Consolidated);
                foreach (Delivery dn in dnList) {
                    string DeliveryNo = dn.DeliveryNo;
                    string cqty = DeliveryRepository.GetDeliveryInfoValue(DeliveryNo, "CQty");
                    Decimal qty = Convert.ToDecimal(cqty);
                    int cartonQty = (int)(dn.Qty / qty);
                    if (dn.Qty % qty != 0)
                        cartonQty++;
                    SumCartonQty += cartonQty;
                    int DnPallletQty = DeliveryRepository.GetSumDeliveryQtyOfACertainDN(DeliveryNo);
                    SumDnPallletQty += DnPallletQty; 
                }

                /*
                1、 获取Delivery的CQty属性（DeliveryInfo.InfoValue，Condition: InfoType= ‘CQty’）
                IDeliveryRepository::string GetDeliveryInfoValue(string dn, string infoType);

                2、  获取Delivery 对应的Delivery_Pallet 记录的SUM(Delivery_Pallet.DeliveryQty)
                IDeliveryRepository::int GetSumDeliveryQtyOfACertainDN(string deliveryNo);
                */
                //int DnPallletQty = DeliveryRepository.GetSumDeliveryQtyOfACertainDN(curDn);
                //临时注释掉，保出货
                //if (SumCartonQty != SumDnPallletQty)
                //{
                //    //从整机库get
                //    {
                //        erpara.Add(currentProduct.PalletNo);
                //        ex = new FisException("CHK903", erpara);    //PALLET  未完全Download!
                //        throw ex;
                //    }
                //}
            }

            return base.DoExecute(executionContext);
        }
	}
}
