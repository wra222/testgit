// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据Session中的DummyPalletCase,获取GetFDESkuList数量，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
// UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                                                 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-23   Chen Xu (itc208014)          create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity.NewNeedCheck
{
    /// <summary>
    /// 根据Session中的DummyPalletCase,获取 GetFDESkuList对象，并放到Session中
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
    ///         Session.ScanQty
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.DummyPalletNo
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
    public partial class GetFDESkuList : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetFDESkuList()
		{
			InitializeComponent();
		}
        /// <summary>
        /// Get Dummy Pallet No and put it into Session.SessionKeys.DummyPalletNo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            FisException ex;
            List<string> erpara = new List<string>();

            string DPC = (string)CurrentSession.GetValue(Session.SessionKeys.DummyPalletCase);
            string dummyPalletNo = (string) CurrentSession.GetValue(Session.SessionKeys.DummyPalletNo);
           
            IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery currentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IProductRepository iproductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
            IList<string> prodNoList = new List<string>();
            IList<string> custSnList = new List<string>();
            IList<string> prodlst = null;
            IList<ProductModel> prodMoLst = null;
            IList<string> shipmentNoLst = null;
            IList<DeliveryPallet> dnpltlist = null;
            IList<string> palletNoLst = null;
            IList<DummyShipDetInfo> dummyshipdetLst = null;

            switch (DPC)
            {
                case "NA":
                    prodlst = new List<string>();
                    dummyshipdetLst = iPalletRepository.GetDummyShipDetListByPlt(dummyPalletNo);
                    if (dummyshipdetLst == null || dummyshipdetLst.Count <= 0)
                    {
                        erpara.Add(dummyPalletNo);
                        ex = new FisException("PAK053", erpara);    //找不到与Dummy Pallet No %1 匹配的Dummy_ShipDet！
                        throw ex;
                    }
                    foreach (DummyShipDetInfo iDSD in dummyshipdetLst)
                    {
                        prodlst.Add(iDSD.snoId);
                    }
                    prodMoLst = new List<ProductModel>();
                    prodMoLst = iproductRepository.GetproductlistOrderByCustSN(prodlst);
                    foreach (ProductModel prod in prodMoLst)
                    {
                        prodNoList.Add(prod.ProductID);
                        custSnList.Add(prod.CustSN);
                    }
                    break;
                case "NAN":
                    IList<Delivery> DeliveryLst = new List<Delivery>();
                    DeliveryLst = ideliveryRepository.GetDeliveryByValueAndType(dummyPalletNo, "BOL");
                    if (DeliveryLst == null || DeliveryLst.Count <= 0)
                    {
                        erpara.Add(dummyPalletNo);
                        erpara.Add("BOL");
                        ex = new FisException("PAK054", erpara);    //没有InfoValue为 %1 , InfoType为 %2 的Delivery！
                        throw ex;
                    }
                    shipmentNoLst = new List<string>();
                    foreach (Delivery idelivery in DeliveryLst)
                    {
                        if (String.IsNullOrEmpty(idelivery.ShipmentNo))
                        {
                            erpara.Add(idelivery.DeliveryNo);
                            ex = new FisException("PAK056", erpara);    //Delivery %1 的资料维护不全！
                            throw ex;
                        }
                        shipmentNoLst.Add(idelivery.ShipmentNo);
                    }
                    dnpltlist = new List<DeliveryPallet>();
                    dnpltlist = iPalletRepository.GetDeliveryPalletByShipmentNoList(shipmentNoLst);
                    if (dnpltlist == null || dnpltlist.Count <= 0)
                    {
                        FisException fe = new FisException("PAK055", new string[] { });  //没有满足条件的DeliveryPallet！
                        throw fe;
                    }
                    palletNoLst = new List<string>();
                    foreach (DeliveryPallet idnplt in dnpltlist)
                    {
                        //if (String.IsNullOrEmpty(idnplt.PalletID))
                        //{
                        //    erpara.Add(idnplt.DeliveryID);
                        //    ex = new FisException("PAK057", erpara);    //Delivery %1 的资料维护不全！
                        //    throw ex;
                        //}
                        palletNoLst.Add(idnplt.PalletID);
                    }
                    if (palletNoLst==null || palletNoLst.Count<=0)
                    {
                        FisException fe = new FisException("PAK055", new string[] { });  //没有满足条件的DeliveryPallet！
                        throw fe;
                    }

                    IList<ProductModel> prodLst = new List<ProductModel>();
                    prodLst = iproductRepository.GetProductListByPalletNoListOrderByCustSN(palletNoLst);
                    if (prodLst == null || prodLst.Count<=0)
                    {
                        erpara.Add(dummyPalletNo);
                        ex = new FisException("CHK079", erpara);    //找不到与此序号匹配的Product! 
                        throw ex;
                    }

                    //2012-3-16 : Revision: 9302: d)	从上一步得到的Product List 中，删除掉已经在Dummy_ShipDet 表中存在的Product，剩余的Products 即为Product List 内容
                    foreach (ProductModel prod in prodLst)
                    {
                        DummyShipDetInfo DSI = iPalletRepository.GetDummyShipDet(prod.ProductID);
                        if (DSI == null)
                        {
                            prodNoList.Add(prod.ProductID);
                            custSnList.Add(prod.CustSN);
                        }
                    }
                    break;
                case "BA":
                    prodlst = new List<string>();
                    dummyshipdetLst = iPalletRepository.GetDummyShipDetListByPlt(dummyPalletNo);
                    if (dummyshipdetLst == null || dummyshipdetLst.Count <= 0)
                    {
                        erpara.Add(dummyPalletNo);
                        ex = new FisException("PAK053", erpara);    //找不到与Dummy Pallet No %1 匹配的Dummy_ShipDet！
                        throw ex;
                    }
                    foreach (DummyShipDetInfo iDSD in dummyshipdetLst)
                    {
                        prodlst.Add(iDSD.snoId);
                    }
                    prodMoLst = new List<ProductModel>();
                    prodMoLst = iproductRepository.GetproductlistOrderByCustSN(prodlst);
                    foreach (ProductModel prod in prodMoLst)
                    {
                        prodNoList.Add(prod.ProductID);
                        custSnList.Add(prod.CustSN);
                    }
                    break;
                case "BAN":
                    string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                    DateTime shipDate = currentDelivery.ShipDate;
                    string carrier = ideliveryRepository.GetDeliveryInfoValue(deliveryNo, "Carrier");
                    if (shipDate == null || String.IsNullOrEmpty(carrier))
                    {
                        erpara.Add(deliveryNo);
                        ex = new FisException("PAK057", erpara);    //Delivery %1 的资料维护不全！
                        throw ex;
                    }
                    shipmentNoLst = new List<string>();
                    shipmentNoLst = ideliveryRepository.GetShipmentNoByShipDateAndValueAndType(shipDate, carrier, "Carrier");
                    if (shipmentNoLst == null || shipmentNoLst.Count <= 0)
                    {
                        erpara.Add(deliveryNo);
                        ex = new FisException("PAK057", erpara);    //Delivery %1 的资料维护不全！
                        throw ex;
                    }
                    dnpltlist = new List<DeliveryPallet>();
                    dnpltlist = iPalletRepository.GetDeliveryPalletByShipmentNoList(shipmentNoLst);
                    if (dnpltlist == null || dnpltlist.Count <= 0)
                    {
                        FisException fe = new FisException("PAK055", new string[] { });  //没有满足条件的DeliveryPallet！
                        throw fe;
                    }
                    palletNoLst = new List<string>();
                    foreach (DeliveryPallet idnplt in dnpltlist)
                    {
                        if (idnplt.PalletID.Substring(0, 2) == "BA")
                        {
                            palletNoLst.Add(idnplt.PalletID);
                        }
                    }
                    if (palletNoLst == null || palletNoLst.Count <= 0)
                    {
                        FisException fe = new FisException("PAK055", new string[] { });  //没有满足条件的DeliveryPallet！
                        throw fe;
                    }

                    prodLst = new List<ProductModel>();
                    prodLst = iproductRepository.GetProductListByPalletNoListOrderByCustSN(palletNoLst);
                    if (prodLst == null || prodLst.Count <= 0)
                    {
                        erpara.Add(dummyPalletNo);
                        ex = new FisException("CHK079", erpara);    //找不到与此序号匹配的Product! 
                        throw ex;
                    }

                    //2012-3-16 : Revision: 9302: e)	从上一步得到的Product List 中，删除掉已经在Dummy_ShipDet 表中存在的Product，剩余的Products 即为Product List 内容
                    foreach (ProductModel prod in prodLst)
                    {
                        DummyShipDetInfo DSI = iPalletRepository.GetDummyShipDet(prod.ProductID);
                        if (DSI == null)
                        {
                            prodNoList.Add(prod.ProductID);
                            custSnList.Add(prod.CustSN);
                        }
                    }
                    break;
                default:
                    break;
            }

            CurrentSession.AddValue(Session.SessionKeys.ProdNoList, prodNoList);
            CurrentSession.AddValue(Session.SessionKeys.CustomSnList, custSnList);

            return base.DoExecute(executionContext);
        }
	}

}
