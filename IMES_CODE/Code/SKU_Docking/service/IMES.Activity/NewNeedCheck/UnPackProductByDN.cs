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
using System.Collections.Generic;
using IMES.FisObject.PAK.Pallet;
using System;

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
            Session session = CurrentSession;
            Product currentProduct = ((Product)session.GetValue(Session.SessionKeys.Product));
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            string unpackType = ((string)session.GetValue(Session.SessionKeys.CN));
            bool isPallet = ((bool)session.GetValue(Session.SessionKeys.Pallet));
            productRepository.BackUpProduct(currentProduct.ProId,this.Editor);

            ShipBoxDetInfo setValue = new ShipBoxDetInfo();
            ShipBoxDetInfo condition = new ShipBoxDetInfo();
            setValue.snoId = "";
            setValue.editor = this.Editor;
            condition.snoId = currentProduct.ProId;
            deliveryRepository.UpdateShipBoxDetInfo(setValue, condition);
            string palletNo = currentProduct.PalletNo;
            string deliveryNo = currentProduct.DeliveryNo;

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
            if (!currentProduct.IsBT)
            { currentProduct.UnitWeight = 0; }
            
           
            #region 清空Pallet weight
            if (!string.IsNullOrEmpty(palletNo))
            {
                //Don't  change PAKLocMAS 
                IPalletRepository currentPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                //IList<string> dnList = productRepository.GetDeliveryNoListByPalletNo(palletNo);
                Pallet pallet = currentPalletRepository.Find(palletNo);
                //if (dnList.Count < 2)
                //{
                //    PakLocMasInfo setVal = new PakLocMasInfo();
                //    PakLocMasInfo cond = new PakLocMasInfo();
                //    setVal.editor = Editor;
                //    setVal.pno = "";
                //    cond.pno = palletNo;
                //    cond.tp = "PakLoc";
                //    currentPalletRepository.UpdatePakLocMasInfoDefered(session.UnitOfWork, setVal, cond);
                //    //Clear Floor in Pallet
                //    pallet.Floor = "";
                //}

                //Clear  weight in Pallet 
                pallet.Weight = 0;
                pallet.Weight_L = 0;
                PalletLog palletLog = new PalletLog { PalletNo = pallet.PalletNo, Station = "RETURN", Line = "Weight:0", Cdt = DateTime.Now, Editor = this.Editor };
                pallet.AddLog(palletLog);
                currentPalletRepository.Update(pallet, session.UnitOfWork);
                //Clear weight in Pallet
               if (!string.IsNullOrEmpty(deliveryNo))
               {
                    session.AddValue(Session.SessionKeys.DeliveryNo, deliveryNo);
               }
            }
            else if (!string.IsNullOrEmpty(deliveryNo))
            {
                session.AddValue(Session.SessionKeys.DeliveryNo, deliveryNo);
            }
            #endregion

            productRepository.Update(currentProduct, session.UnitOfWork);

            productRepository.BackUpProductStatus(currentProduct.ProId, this.Editor);

            return base.DoExecute(executionContext);
        }
	}
}
