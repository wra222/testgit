// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Benson          create
//2013-03-13    Vincent           release
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Carton;
using IMES.DataModel;

namespace IMES.Activity
{


    /// <summary>
    /// Assign Box and Ucc With Carton
    /// </summary>
  
    public partial class AssignBoxUccWithCarton : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public AssignBoxUccWithCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> errpara = new List<string>();

            IList<IProduct> prodList = CurrentSession.GetValue(Session.SessionKeys.ProdList) as IList<IProduct>;
            IList<DeliveryCarton> bindDNList = (IList<DeliveryCarton>)CurrentSession.GetValue(ExtendSession.SessionKeys.BindDNList);
            Pallet curPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);          

            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();

            Carton carton = (Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
           
            Delivery curDelivery = deliveryRep.Find(bindDNList[0].DeliveryNo);

            CurrentSession.AddValue(Session.SessionKeys.Delivery, curDelivery);
            
           

            //a.	取Delivery 的下列属性并保存到对应的变量中：
            string BoxId = "";            
            string flag = (string)curDelivery.GetExtendedProperty("Flag");
           
            //b.	取Pallet 的UCC 属性(IMES_PAK..Pallet.UCC)，并保存到变量@ucc 中
            //IF EXISTS (SELECT *FROM ShipBoxDet NOLOCK WHERE DeliveryNo=@dn and PLT<>@plt and SnoId=@id)
            //BEGIN
            //UPDATE ShipBoxDet SET SnoId='' ,Udt=GETDATE() WHERE DeliveryNo=@dn and SnoId=@id
            //END
            string ucc = curPallet.UCC;

            //c.如果Product 曾经结合过其它栈板，需要先解除与其他栈板的结合
            foreach (DeliveryCarton item in bindDNList)
            {
                Boolean bindflag = deliveryRep.CheckExistShipBoxDetExceptPlt(item.DeliveryNo, curPallet.PalletNo, carton.CartonSN);
                if (bindflag)
                {
                    deliveryRep.UpdateShipBoxDetForClearSnoidDefered(CurrentSession.UnitOfWork, carton.CartonSN, item.DeliveryNo);
                }
            }

            IList<ShipBoxDetInfo> shipList;
            IList<string> boxIdList;
            ShipBoxDetInfo shipInfo;

            //d.当@Flag='N' or @ucc<>'' 时(所谓自动单情况)，按照如下方法分配Box Id
            if ((flag == "N") || !string.IsNullOrEmpty(ucc))
            {
                //a)使用DeliveryNo=@dn and PLT=@plt and SnoId=@id 查询ShipBoxDet 表存在记录时，
                shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo,carton.CartonSN);

                if (shipList.Count != 0)
                {
                    //取查询到的记录的BoxId 字段值保存到变量@BoxId 中，并更新该记录的Udt 字段值为当前时间

                    shipInfo = shipList[0];
                    BoxId = shipInfo.boxId;

                    ShipBoxDetInfo setvalue = new ShipBoxDetInfo();
                    ShipBoxDetInfo conf = new ShipBoxDetInfo();
                    conf.deliveryNo = curDelivery.DeliveryNo;
                    conf.plt = curPallet.PalletNo;
                    conf.snoId = carton.CartonSN;

                    setvalue.udt = DateTime.Now;
                    deliveryRep.UpdateShipBoxDetInfoDefered(CurrentSession.UnitOfWork, setvalue, conf);

                    //如果尚未结合过，则结合@BoxId，记录到IMES_FA..ProductInfo （InfoValue = @BoxId， 
                    //InfoType 当@ucc<>''and LEN(@BoxId)=20 时，记录为'UCC'，否则记录为'BoxId'）
                    if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                    {
                        foreach (Product item in prodList)
                        {
                            item.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                            productRep.Update(item, CurrentSession.UnitOfWork);
                        }
                    }
                    else
                    {
                        foreach (Product item in prodList)
                        {
                            item.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                            productRep.Update(item, CurrentSession.UnitOfWork);
                        }
                    }                   

                    ////ii.	更新Product结合的Pallet (IMES_FA..Product.PalletNo) 为@plt
                    //ProductIDList.Add(curProduct.ProId);
                    //productRep.BindPalletDefered(CurrentSession.UnitOfWork, curProduct.PalletNo, ProductIDList);
                    carton.BoxId = BoxId;
                }
                else
                {
                    //b)使用DeliveryNo=@dn and PLT=@plt and SnoId=@id 查询ShipBoxDet 表不存在记录时，
                    //使用DeliveryNo=@dn and PLT=@plt and SnoId='' 查询ShipBoxDet 表，
                    //取查询到的记录的BoxId 字段值保存到变量@BoxId 中
                   
                    boxIdList = deliveryRep.GetAndUpdateShipBoxDet(curDelivery.DeliveryNo, curPallet.PalletNo, carton.CartonSN);
                    if (boxIdList.Count == 0)
                    {
                        errpara.Add(this.Key);//无可用Box Id，请联系FIS!
                        throw new FisException("PAK102", errpara);
                    }

                    
                    BoxId = boxIdList[0];

                    //i.	如果ISNULL(@BoxId, '') = ''，则报告错误：“无可用Box Id，请联系FIS!”
                    if (string.IsNullOrEmpty(BoxId))
                    {
                        errpara.Add(this.Key);//无可用Box Id，请联系FIS!
                        throw new FisException("PAK102", errpara);

                    }
                    else
                    {
                        //ii.	如果ISNULL(@BoxId, '') <> ''， 
                        //A.	Update ShipBoxDet 记录结合Product 信息
                        //Condition: DeliveryNo=@dn and PLT=@plt and BoxId=@BoxId SET：SnoId=@id,Udt=GETDATE()

                        //deliveryRep.UpdateShipBoxDetForSetSnoIdDefered(CurrentSession.UnitOfWork,curProduct.ProId,curDelivery.DeliveryNo,curPallet.PalletNo, BoxId);

                        //B.	如果Product 已经结合过Box Id / UCC (两者只会结合一个，Box Id / UCC 存放在IMES_FA..ProductInfo.Value，
                        //Condition: InfoType = 'BoxId' 或者 'UCC')，则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；
                        deliveryRep.UpdateAssignBoxIdEditorDefered(CurrentSession.UnitOfWork, curDelivery.DeliveryNo, curPallet.PalletNo, carton.CartonSN);
                        if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                        {
                            foreach (Product item in prodList)
                            {
                                item.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                                productRep.Update(item, CurrentSession.UnitOfWork);
                            }
                        }
                        else
                        {
                            foreach (Product item in prodList)
                            {
                                item.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                                productRep.Update(item, CurrentSession.UnitOfWork);
                            }
                        }                   

                        //C.	更新Product结合的Pallet (IMES_FA..Product.PalletNo) 为@plt
                        //ProductIDList.Add(curProduct.ProId);
                        //productRep.BindPalletDefered(CurrentSession.UnitOfWork, curProduct.PalletNo, ProductIDList);
                        carton.BoxId = BoxId;
                    }
                }
            }
            else  //e	当@Flag<>'N' and @ucc='' 时(所谓手动单情况)，按照如下方法分配Box Id
            {
                //a)	使用DeliveryNo=@dn and PLT=@plt and SnoId=@id 查询ShipBoxDet 表存在记录时，取查询到的记录的BoxId 字段值保存到变量@BoxId 中，
                //并更新该记录的Udt 字段值为当前时间
                //i.	如果Product 已经结合过Box Id / UCC (两者只会结合一个，Box Id / UCC 存放在IMES_FA..ProductInfo.Value，Condition: InfoType = 'BoxId' 或者 'UCC')，
                //则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；如果尚未结合过，则结合@BoxId，记录到IMES_FA..ProductInfo （InfoValue = @BoxId，InfoType 当@ucc<>'' and LEN(@BoxId)=20 时，记录为'UCC'，否则记录为'BoxId'）
                //ii.	更新Product结合的Pallet (IMES_FA..Product.PalletNo) 为@plt
                shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo, carton.CartonSN);
                if (shipList.Count > 0)
                {
                    shipInfo = shipList[0];
                    BoxId = shipInfo.boxId;

                    ShipBoxDetInfo setvalue = new ShipBoxDetInfo();
                    ShipBoxDetInfo conf = new ShipBoxDetInfo();
                    conf.deliveryNo = curDelivery.DeliveryNo;
                    conf.plt = curPallet.PalletNo;
                    conf.snoId = carton.CartonSN;

                    setvalue.udt = DateTime.Now;
                    deliveryRep.UpdateShipBoxDetInfoDefered(CurrentSession.UnitOfWork, setvalue, conf);

                    if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                    {
                        foreach (Product item in prodList)
                        {
                            item.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                            productRep.Update(item, CurrentSession.UnitOfWork);
                        }
                    }
                    else
                    {
                        foreach (Product item in prodList)
                        {
                            item.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                            productRep.Update(item, CurrentSession.UnitOfWork);
                        }
                    }
                    carton.BoxId = BoxId;
                }
                else
                {   //Mantis #0000954   
                    // a)	如果使用DeliveryNo=@dn and PLT=@plt and SnoId='' 查询ShipBoxDet 表存在记录时，按照下列方法，分配Box Id
                    //i.	使用DeliveryNo=@dn and PLT=@plt and SnoId='' 查询ShipBoxDet 表，取记录的BoxId 字段值保存到@BoxId 变量中
                    //ii.	Update ShipBoxDet 记录结合Product 信息 Condition: DeliveryNo=@dn and PLT=@plt and BoxId=@BoxId SET：SnoId=@id,Udt=GETDATE()
                    //iii.	如果Product 已经结合过Box Id / UCC (两者只会结合一个，Box Id / UCC 存放在IMES_FA..ProductInfo.Value，Condition: InfoType = 'BoxId' 或者 'UCC')，
                    //      则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；如果尚未结合过，则结合@BoxId，记录到IMES_FA..ProductInfo 
                    //      （InfoValue = @BoxId，InfoType 当@ucc<>'' and LEN(@BoxId)=20 时，记录为'UCC'，否则记录为'BoxId'）
                    //iv.	更新Product结合的Pallet (IMES_FA..Product.PalletNo) 为@plt
                    //shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo, "");
                    boxIdList = deliveryRep.GetAndUpdateShipBoxDet(curDelivery.DeliveryNo, curPallet.PalletNo, carton.CartonSN);
                    //if (shipList.Count != 0)
                    if (boxIdList.Count != 0)
                    {
                        //shipInfo = shipList[0];
                        //BoxId = shipInfo.boxId;
                        //deliveryRep.UpdateShipBoxDetForSetSnoIdDefered(CurrentSession.UnitOfWork, curProduct.ProId, curDelivery.DeliveryNo, curPallet.PalletNo, BoxId);
                        deliveryRep.UpdateAssignBoxIdEditorDefered(CurrentSession.UnitOfWork, curDelivery.DeliveryNo, curPallet.PalletNo, carton.CartonSN);
                        BoxId = boxIdList[0];
                        if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                        {
                            foreach (Product item in prodList)
                            {
                                item.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                                productRep.Update(item, CurrentSession.UnitOfWork);
                            }
                        }
                        else
                        {
                            foreach (Product item in prodList)
                            {
                                item.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                                productRep.Update(item, CurrentSession.UnitOfWork);
                            }
                        }
                        carton.BoxId = BoxId;
                    }
                    else
                    {
                        //b)	如果使用DeliveryNo=@dn and PLT=@plt and SnoId='' 查询ShipBoxDet 表不存在记录时，按照下列方法，分配Box Id
                        //i.	使用DeliveryNo=@dn and SnoId='' 查询ShipBoxDet 表，按照PLT, BoxId 升序排序，取TOP 1 记录的PLT / BoxId 字段值保存到@plt / @BoxId 变量中
                        //ii.	如果ISNULL(@BoxId, '') = ''，则报告错误：“无可用Box Id，请联系FIS!”
                        //iii.	Update ShipBoxDet 记录结合Product 信息   Condition: DeliveryNo=@dn and PLT=@plt and BoxId=@BoxId SET：SnoId=@id,Udt=GETDATE()
                        //iv.   如果Product 已经结合过Box Id / UCC (两者只会结合一个，Box Id / UCC 存放在IMES_FA..ProductInfo.Value，Condition: InfoType = 'BoxId' 或者 'UCC')，
                        //      则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；如果尚未结合过，则结合@BoxId，记录到IMES_FA..ProductInfo 
                        //      （InfoValue = @BoxId，InfoType 当@ucc<>'' and LEN(@BoxId)=20 时，记录为'UCC'，否则记录为'BoxId'）
                        //v.	更新Product结合的Pallet (IMES_FA..Product.PalletNo) 为@plt
                        //shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo, "");//??
                        boxIdList = deliveryRep.GetAndUpdateShipBoxDet(curDelivery.DeliveryNo, curPallet.PalletNo, carton.CartonSN);
                        //if (shipList.Count == 0)
                        if (boxIdList.Count == 0)
                        {
                            errpara.Add(this.Key);//无可用Box Id，请联系FIS!
                            throw new FisException("PAK102", errpara);
                        }

                        //shipInfo = shipList[0];
                        //BoxId = shipInfo.boxId;
                        BoxId = boxIdList[0];
                        if (string.IsNullOrEmpty(BoxId))
                        {
                            errpara.Add(this.Key);//无可用Box Id，请联系FIS!
                            throw new FisException("PAK102", errpara);
                        }
                        else
                        {
                            //deliveryRep.UpdateShipBoxDetForSetSnoIdDefered(CurrentSession.UnitOfWork, curProduct.ProId, curDelivery.DeliveryNo, curPallet.PalletNo, BoxId);
                            // ProductIDList.Add(curProduct.ProId);
                            //productRep.BindPalletDefered(CurrentSession.UnitOfWork, curProduct.PalletNo, ProductIDList);
                            deliveryRep.UpdateAssignBoxIdEditorDefered(CurrentSession.UnitOfWork, curDelivery.DeliveryNo, curPallet.PalletNo, carton.CartonSN);

                            if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                            {
                                foreach (Product item in prodList)
                                {
                                    item.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                                    productRep.Update(item, CurrentSession.UnitOfWork);
                                }
                            }
                            else
                            {
                                foreach (Product item in prodList)
                                {
                                    item.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                                    productRep.Update(item, CurrentSession.UnitOfWork);
                                }
                            }
                            carton.BoxId = BoxId;
                        }
                    }
                }
            }

            //f.	当与分配的Delivery (@dn)结合的所有Product （IMES_FA..Product.DeliveryNo）数量与Delivery 定义数量（IMES_PAK..Delivery.Qty）相等，
            //并都已经结合了Pallet(IMES_FA..Product.PalletNo)时，更新Delivery 的状态为'88'
            //curDelivery.IsDNFull()           

            //int combinePLTQty = (CurrentSession.GetValue("HasAssignPLTQrty") == null ? 0 : (int)CurrentSession.GetValue("HasAssignPLTQrty"));
            foreach (IProduct item in prodList)
            {
                item.PalletNo = curPallet.PalletNo;
                productRep.Update(item, CurrentSession.UnitOfWork);                
            }

            IList<Delivery> dnList = new List<Delivery>();
            foreach (DeliveryCarton item in bindDNList)
            {
                //int dvQty = productRep.GetCombinedPalletQtyByDN(item.DeliveryNo) + item.AssignQty;
                int dvQty = productRep.GetCombinedQtyByDN(item.DeliveryNo) + item.AssignQty;
                
                int dnQty = 0;
                string dnStatus = "";
                Delivery dn;
                if (curDelivery.DeliveryNo == item.DeliveryNo)
                {
                    dn = curDelivery;
                    dnQty = curDelivery.Qty;
                    dnStatus = curDelivery.Status;
                }
                else
                {
                    dn = deliveryRep.Find(item.DeliveryNo);
                    dnQty = dn.Qty;
                    dnStatus = dn.Status;
                }

                dnList.Add(dn);
                if (dvQty == dnQty && dnStatus!="87")
                {
                    dn.Status = "87";
                    dn.Editor = CurrentSession.Editor;
                    dn.Udt = DateTime.Now;
                    DeliveryLog log = new DeliveryLog(0, item.DeliveryNo, "87", CurrentSession.Station, CurrentSession.Line, CurrentSession.Editor, DateTime.Now);
                    dn.AddLog(log);
                    deliveryRep.Update(dn, CurrentSession.UnitOfWork);
                }
            }
            
            foreach (DeliveryCarton item in bindDNList)
            {
                //item.Status = DeliveryCartonState.Assign;
                carton.SetDeliveryInCarton(item);
            }

            carton.Status = carton.FullQty > carton.Qty ? CartonStatusEnum.Partial : CartonStatusEnum.Full;
            cartonRep.Update(carton, CurrentSession.UnitOfWork);

            CurrentSession.AddValue(Session.SessionKeys.DeliveryList, dnList);

            return base.DoExecute(executionContext);
          
        }
	}
}
