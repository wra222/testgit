// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 2.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 2.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-26   Du Xuan (itc98066)          create
// ITC-1360-0707 shiplist查询结果抛出异常
// ITC-1360-1539 调整库位选择顺序
// ITC-1360-1634 Region='SNU'拼写错误
// ITC-1360-1714 针对“@Flag<>'N' and @ucc='' 这个分支”增加记录BoxId or UCC 步骤
// ITC-1360-1812 解除与其他栈板的结合接口修改
// Known issues:

using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;

namespace IMES.Activity
{
    /// <summary>
    /// Box Id / UCC 的分配方法
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Box Id / UCC 分配
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class AssignBoxUcc : BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public AssignBoxUcc()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Box Id / UCC 分配原则
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            List<string> errpara = new List<string>();
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            Delivery curDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            Pallet curPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);

            IList<string> ProductIDList = new List<string>();

            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            CurrentSession.AddValue("CreatePDF", "");
            IList<ProductBTInfo> btList = productRep.GetProductBT(curProduct.ProId);
            if (btList.Count > 0 && Station.Trim() == "92")
            {
                return base.DoExecute(executionContext);
            }

            string proBoxId = (string)curProduct.GetExtendedProperty("BoxId");
            string proUCC = (string)curProduct.GetExtendedProperty("UCC");

            //a.	取Delivery 的下列属性并保存到对应的变量中：
            string BoxId = "";
            string shipment = curDelivery.ShipmentNo;
            string model = curDelivery.ModelName;
            int qut = curDelivery.Qty;
            DateTime shipDate = curDelivery.ShipDate;
            string emea = (string)curDelivery.GetExtendedProperty("EmeaCarrier");
            string carrier = (string)curDelivery.GetExtendedProperty("Carrier");
            string regId = (string)curDelivery.GetExtendedProperty("RegId");
            string deport = (string)curDelivery.GetExtendedProperty("Deport");
            string flag = (string)curDelivery.GetExtendedProperty("Flag");
            string boxId = (string)curDelivery.GetExtendedProperty("BoxId");
            string boxReg = (string)curDelivery.GetExtendedProperty("BoxReg");
            string shipWay = (string)curDelivery.GetExtendedProperty("ShipWay");

            //b.	取Pallet 的UCC 属性(IMES_PAK..Pallet.UCC)，并保存到变量@ucc 中
            //IF EXISTS (SELECT *FROM ShipBoxDet NOLOCK WHERE DeliveryNo=@dn and PLT<>@plt and SnoId=@id)
            //BEGIN
	        //UPDATE ShipBoxDet SET SnoId='' ,Udt=GETDATE() WHERE DeliveryNo=@dn and SnoId=@id
            //END
            string ucc = curPallet.UCC;

            //c.如果Product 曾经结合过其它栈板，需要先解除与其他栈板的结合
            Boolean bindflag = deliveryRep.CheckExistShipBoxDetExceptPlt(curDelivery.DeliveryNo, curPallet.PalletNo, curProduct.ProId);
            if (bindflag)
            {
                deliveryRep.UpdateShipBoxDetForClearSnoidDefered(CurrentSession.UnitOfWork, curProduct.ProId, curDelivery.DeliveryNo);
            }

            IList<ShipBoxDetInfo> shipList;
            ShipBoxDetInfo shipInfo;
            //d.当@Flag='N' or @ucc<>'' 时(所谓自动单情况)，按照如下方法分配Box Id
            if ( (flag == "N")|| !string.IsNullOrEmpty(ucc))
            {
                //a)使用DeliveryNo=@dn and PLT=@plt and SnoId=@id 查询ShipBoxDet 表存在记录时，
                shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo, curProduct.ProId);
                
                if (shipList.Count != 0)
                {
                    //取查询到的记录的BoxId 字段值保存到变量@BoxId 中，并更新该记录的Udt 字段值为当前时间
                   
                    shipInfo = shipList[0];
                    BoxId = shipInfo.boxId;

                    ShipBoxDetInfo setvalue = new ShipBoxDetInfo();
                    ShipBoxDetInfo conf = new ShipBoxDetInfo();
                    conf.deliveryNo = curDelivery.DeliveryNo;
                    conf.plt = curPallet.PalletNo;
                    conf.snoId = curProduct.ProId;

                    setvalue.udt = DateTime.Now;
                    deliveryRep.UpdateShipBoxDetInfoDefered(CurrentSession.UnitOfWork, setvalue, conf);

                    //i.如果Product 已经结合过Box Id / UCC (两者只会结合一个)
                    if (!string.IsNullOrEmpty(proBoxId) || !string.IsNullOrEmpty(proUCC))
                    {
                        //则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；
                        if (!string.IsNullOrEmpty(proBoxId))
                        {
                            curProduct.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                        }
                        else
                        {
                            curProduct.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                        }
                    }
                    else
                    {
                        //如果尚未结合过，则结合@BoxId，记录到IMES_FA..ProductInfo （InfoValue = @BoxId， 
                        //InfoType 当@ucc<>''and LEN(@BoxId)=20 时，记录为'UCC'，否则记录为'BoxId'）
                        if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                        {
                            curProduct.SetExtendedProperty("UCC",BoxId,CurrentSession.Editor);
                        }
                        else
                        {
                            curProduct.SetExtendedProperty("BoxId",BoxId,CurrentSession.Editor);
                        }
                    }
                    
                    //ii.	更新Product结合的Pallet (IMES_FA..Product.PalletNo) 为@plt
                    ProductIDList.Add(curProduct.ProId);
                    productRep.BindPalletDefered(CurrentSession.UnitOfWork, curProduct.PalletNo, ProductIDList);
                }
                else
                {
                    //b)使用DeliveryNo=@dn and PLT=@plt and SnoId=@id 查询ShipBoxDet 表不存在记录时，
                    //使用DeliveryNo=@dn and PLT=@plt and SnoId='' 查询ShipBoxDet 表，
                    //取查询到的记录的BoxId 字段值保存到变量@BoxId 中

                    shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo, "");
                    if (shipList.Count == 0)
                    {
                        errpara.Add(this.Key);//无可用Box Id，请联系FIS!
                        throw new FisException("PAK102", errpara);
                    }

                    shipInfo = shipList[0];
                    BoxId = shipInfo.boxId;

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
                        deliveryRep.UpdateShipBoxDetForSetSnoIdDefered(CurrentSession.UnitOfWork,curProduct.ProId,curDelivery.DeliveryNo,curPallet.PalletNo, BoxId);
                        
                        //B.	如果Product 已经结合过Box Id / UCC (两者只会结合一个，Box Id / UCC 存放在IMES_FA..ProductInfo.Value，
                        //Condition: InfoType = 'BoxId' 或者 'UCC')，则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；
                        if (!string.IsNullOrEmpty(proBoxId) || !string.IsNullOrEmpty(proUCC))
                        {
                            if (!string.IsNullOrEmpty(proBoxId))
                            {
                                curProduct.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                            }
                            else
                            {
                                curProduct.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                            }
                            curProduct.Udt = DateTime.Now;
                        }
                        else
                        {
                            //如果尚未结合过，则结合@BoxId，记录到IMES_FA..ProductInfo 
                            //（InfoValue = @BoxId，InfoType 当@ucc<>'' and LEN(@BoxId)=20 时，记录为'UCC'，否则记录为'BoxId'）
                            if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                            {
                                curProduct.SetExtendedProperty("UCC",BoxId,CurrentSession.Editor);
                            }
                            else
                            {
                                curProduct.SetExtendedProperty("BoxId",BoxId,CurrentSession.Editor);
                            }
                        }
                        
                        //C.	更新Product结合的Pallet (IMES_FA..Product.PalletNo) 为@plt
                        //ProductIDList.Add(curProduct.ProId);
                        //productRep.BindPalletDefered(CurrentSession.UnitOfWork, curProduct.PalletNo, ProductIDList);
                        curProduct.Udt = DateTime.Now;
                        curProduct.PalletNo = curPallet.PalletNo;
                        productRep.Update(curProduct, CurrentSession.UnitOfWork);
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
                shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo, curProduct.ProId);
                if (shipList.Count > 0)
                {
                    shipInfo = shipList[0];
                    BoxId = shipInfo.boxId;

                    ShipBoxDetInfo setvalue= new ShipBoxDetInfo();
                    ShipBoxDetInfo conf = new ShipBoxDetInfo();
                    conf.deliveryNo = curDelivery.DeliveryNo;
                    conf.plt = curPallet.PalletNo;
                    conf.snoId = curProduct.ProId;
                    
                    setvalue.udt = DateTime.Now;
                    deliveryRep.UpdateShipBoxDetInfoDefered(CurrentSession.UnitOfWork,setvalue,conf);

                    if (!string.IsNullOrEmpty(proBoxId) || !string.IsNullOrEmpty(proUCC))
                    {
                        //则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；
                        if (!string.IsNullOrEmpty(proBoxId))
                        {
                            curProduct.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                        }
                        else
                        {
                            curProduct.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                        {
                            curProduct.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                        }
                        else
                        {
                            curProduct.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                        }
                    }
                    curProduct.Udt = DateTime.Now;
                    curProduct.PalletNo = curPallet.PalletNo;
                    productRep.Update(curProduct, CurrentSession.UnitOfWork);
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
                    shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo, "");
                    if (shipList.Count != 0)
                    {
                        shipInfo = shipList[0];
                        BoxId = shipInfo.boxId;
                        deliveryRep.UpdateShipBoxDetForSetSnoIdDefered(CurrentSession.UnitOfWork, curProduct.ProId, curDelivery.DeliveryNo, curPallet.PalletNo, BoxId);

                        if (!string.IsNullOrEmpty(proBoxId) || !string.IsNullOrEmpty(proUCC))
                        {
                            //则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；
                            if (!string.IsNullOrEmpty(proBoxId))
                            {
                                curProduct.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                            }
                            else
                            {
                                curProduct.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                            {
                                curProduct.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                            }
                            else
                            {
                                curProduct.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                            }
                        }
                        curProduct.Udt = DateTime.Now;
                        curProduct.PalletNo = curPallet.PalletNo;
                        productRep.Update(curProduct, CurrentSession.UnitOfWork);
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
                        shipList = deliveryRep.GetShipBoxDetList(curDelivery.DeliveryNo, curPallet.PalletNo, "");//??
                        if (shipList.Count == 0)
                        {
                            errpara.Add(this.Key);//无可用Box Id，请联系FIS!
                            throw new FisException("PAK102", errpara);
                        }

                        shipInfo = shipList[0];
                        BoxId = shipInfo.boxId;
                        if (string.IsNullOrEmpty(BoxId))
                        {
                            errpara.Add(this.Key);//无可用Box Id，请联系FIS!
                            throw new FisException("PAK102", errpara);
                        }
                        else
                        {
                            deliveryRep.UpdateShipBoxDetForSetSnoIdDefered(CurrentSession.UnitOfWork, curProduct.ProId, curDelivery.DeliveryNo, curPallet.PalletNo, BoxId);
                            // ProductIDList.Add(curProduct.ProId);
                            //productRep.BindPalletDefered(CurrentSession.UnitOfWork, curProduct.PalletNo, ProductIDList);

                            if (!string.IsNullOrEmpty(proBoxId) || !string.IsNullOrEmpty(proUCC))
                            {
                                //则需要更新该记录的InfoValue = @BoxId, Editor, Udt = Getdate()；
                                if (!string.IsNullOrEmpty(proBoxId))
                                {
                                    curProduct.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                                }
                                else
                                {
                                    curProduct.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ucc) && (BoxId.Length == 20))
                                {
                                    curProduct.SetExtendedProperty("UCC", BoxId, CurrentSession.Editor);
                                }
                                else
                                {
                                    curProduct.SetExtendedProperty("BoxId", BoxId, CurrentSession.Editor);
                                }
                            }
                            curProduct.Udt = DateTime.Now; 
                            curProduct.PalletNo = curPallet.PalletNo;
                            productRep.Update(curProduct, CurrentSession.UnitOfWork);
                        }
                    }
                }
            }

            //f.	当与分配的Delivery (@dn)结合的所有Product （IMES_FA..Product.DeliveryNo）数量与Delivery 定义数量（IMES_PAK..Delivery.Qty）相等，
            //并都已经结合了Pallet(IMES_FA..Product.PalletNo)时，更新Delivery 的状态为'88'
            //curDelivery.IsDNFull()
            int dvQty = productRep.GetCombinedQtyByDN(curDelivery.DeliveryNo);

            string oldDnFlag = (string)CurrentSession.GetValue("HasDN");
            if (oldDnFlag != "Y")
            {
                dvQty++;
            }

            if (dvQty == curDelivery.Qty)
            {
                curDelivery.Status = "88";
                curDelivery.Editor = CurrentSession.Editor;
                curDelivery.Udt = DateTime.Now;
                deliveryRep.Update(curDelivery, CurrentSession.UnitOfWork);
            }
            
            // g.下文描述的是针对不同的情况，如何获取需要提示用户的信息 
            //a)	取Delivery 的PalletQty (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'PalletQty')属性值，
            //并取其整数部分保存到@paletqty 变量中；如果该属性不存在，则令@paletqty = 60
            string paletqty = (string)curDelivery.GetExtendedProperty("PalletQty");
            
            if (string.IsNullOrEmpty(paletqty))
            {
                paletqty = "60";
            }
           
            //b)	使用如下方法，取得变量@pqty
            //SELECT @pqty = sum(DeliveryQty) FROM Delivery_Pallet NOLOCK WHERE PalletNo = @Plt
            int pqty = deliveryRep.GetSumDeliveryQtyOfACertainPallet(curPallet.PalletNo);

            //c)	使用如下方法，取得变量@pqty2
            //SELECT @pqty2=TierQty FROM PalletStandard WHERE FullQty=@paletqty
            int pqty2 = palletRep.GetTierQtyFromPalletQtyInfo(paletqty);

            //d)	如果@pqty>=@pqty2，则令@emeastr='海运，满一层请使用大的木头栈板'
            string emeastr = string.Empty;
            string pclor = string.Empty;

            if (pqty >= pqty2 )
            {
                emeastr = "海运，满一层请使用大的木头栈板";
            }
            //e)	如果(@Region='SNE' or @Region='SCE' ) and @shipway<>'T002'时
            //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='满一层请使用chep栈板'；@pclor=' H'；
            //否则令@emeastr=' '；@pclor=''
            if(( regId=="SNE" || regId=="SCE")&&(shipWay != "T002" ))
            {
                if (pqty >= pqty2 && pqty2 > 0)
                {
                    emeastr= "满一层请使用chep栈板";
                    pclor = " H";
                }
                else
                {
                    emeastr= "";
                    pclor = "";
                }
            }
            else if(( regId=="SNL" )&&(shipWay != "T002" ))
            {   //f)	不满足上一步的条件时，如果@Region='SNL' and @shipway<>'T002' 时
                //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='请使用绿色塑料栈板'；@pclor=' P'；
                //否则令@emeastr=' '；@pclor=''
                if (pqty >= pqty2 && pqty2 > 0 )
                {
                    emeastr= "请使用绿色塑料栈板";
                    pclor = " P";
                }
                else
                {
                    emeastr= "";
                    pclor = "";
                }

            }
            else if(( regId=="SNU" || regId == "SCU" ) && (shipWay != "T002" ))
            {    //g)	不满足前面的条件时，如果(@Region='SNU' or @Region='SCU' ) and @shipway<>'T002' 时
                 //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='请使用蓝色塑料栈板'；@pclor=' A'；
                 //否则令@emeastr=' '；@pclor=''
                if (pqty >= pqty2 && pqty2 > 0 )
                {
                    emeastr= "请使用蓝色塑料栈板";
                    pclor = " A";
                }
                else
                {
                    emeastr= "";
                    pclor = "";
                }
            }
            else if(( regId=="SNE" || regId == "SCE" ) && (shipWay == "T002" ))
            {
                //h)	不满足前面的条件时，如果(@Region='SNE' or @Region='SCE' ) and @shipway='T002' 时，
                //      则令@emeastr='EMEA海运,请使用E1栈板'；@pclor=' K'
                emeastr= "EMEA海运,请使用E1栈板";
                pclor = " K";
            }
            else if(( regId=="SAF" ) && (shipWay == "T001" ))
            {
                //i)	不满足前面的条件时，如果@shipway='T001' and @Region='SAF' 时，
                //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='請使用綠色塑料棧板'；@pclor=' P'；
                //否则令@emeastr=' '；@pclor=''
                if (pqty >= pqty2 && pqty2 > 0 )
                {
                    emeastr= "请使用绿色塑料栈板";
                    pclor = " P";
                }
                else
                {
                    emeastr= "";
                    pclor = "";
                }

            }
            else if ((regId == "SCN") && (shipWay == "T001"))
            {
                // j)	不满足前面的条件时，如果@shipway='T001' and @Region='SCN' 时，
                //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='满一层请使用大的木头栈板'；@pclor=' '；
                //否则令@emeastr=' '；@pclor=''
                if (pqty >= pqty2 && pqty2 > 0 )
                {
                    emeastr= "满一层请使用大的木头栈板";
                    pclor = " ";
                }
                else
                {
                    emeastr= "";
                    pclor = "";
                }

            }
            else
            {
                emeastr= "";
                pclor = "";
            }

            //h.下文描述的是分配库位
            //Model 的第10，11码是”29” 或者”39” 的产品是出货日本的产品；否则，是非出货日本的产品
            //PAK_LocMas.WC = ‘JP’ 的为日本专用库位
            //出货日本的Pallet优先分配出货日本专用库位，当出货日本专用库位用完时，出货日本的Pallet 方可使用普通库位
            string jpmodel =curProduct.Model.Substring(9,2);
            bool jpflag = false;
            if (jpmodel == "29" || jpmodel == "39")
            {
                jpflag = true;
            }

            //a)	当LEFT(@plt,2)<>'NA' and LEFT(@plt,2)<>'BA' 时，按照如下方法分配库位
            string floor = (string)CurrentSession.GetValue(Session.SessionKeys.Floor);
            string loc="";
            Boolean addflag = true;
            if (curPallet.PalletNo.Substring(0,2) !="NA" && curPallet.PalletNo.Substring(0,2) !="BA")
            {
                //i.	取与@plt 结合的Product 数量，如果数量为1，则按照如下方法分配库位
                IList<ProductModel> modelList = productRep.GetProductListByPalletNo(curPallet.PalletNo);
                IList<PakLocMasInfo> macList;
                string pdline;
                ProductModel newModel = new ProductModel();
                if (!string.IsNullOrEmpty(curProduct.PalletNo))
                {
                    foreach (ProductModel pmodel in modelList)
                    {
                        if (pmodel.ProductID == curProduct.ProId)
                        {
                            addflag = false;
                            break;
                        }
                    }
                    if (addflag)
                    {
                        newModel.ProductID = curProduct.ProId;
                        newModel.Model = curProduct.Model;
                        newModel.CustSN = curProduct.CUSTSN;
                        modelList.Add(newModel);
                    }
                }

                if (modelList.Count == 1 )
                {
                    // 1.	当使用Pno=@plt and Tp='PakLoc' and FL=@floor (@floor 来自页面用户的选择)查询PAK_LocMas表存在记录时，
                    //则取该记录的SnoId 字段值保存到变量@loc 中，并更新PAK_LocMas表中满足Pno=@plt and Tp='PakLoc' 条件的
                    //记录的PdLine 字段值为Product Id 的第一位字符，Udt 为当前时间
                    //bool chkmas = palletRep.CheckExistPakLocMas(curPallet.PalletNo, "PakLoc", floor);
                    
                    PakLocMasInfo normalconf = new PakLocMasInfo();
                    normalconf.pno = curPallet.PalletNo;
                    normalconf.tp = "PakLoc";
                    normalconf.fl = floor;

                    PakLocMasInfo neqconf = new PakLocMasInfo();
                    neqconf.wc = "JP";
                    if (jpflag)
                    {
                        PakLocMasInfo conf = new PakLocMasInfo();
                        conf.pno = curPallet.PalletNo;
                        conf.tp = "PakLoc";
                        conf.fl= floor;
                        conf.wc = "JP";
                        macList = palletRep.GetPakLocMasList(conf);
                        if (macList.Count == 0)
                        {
                            macList = palletRep.GetPakLocMasList(normalconf, neqconf);//(curPallet.PalletNo, "PakLoc", floor);
                        }
                    }
                    else
                    {
                        macList = palletRep.GetPakLocMasList(normalconf, neqconf);//(curPallet.PalletNo, "PakLoc", floor);
                    }
                    if (macList.Count>0)
                    {                        
                        loc = macList[0].snoId;
                        pdline = curProduct.ProId.Substring(0,1);
                        palletRep.UpdatePakLocMasForPdLineDefered(CurrentSession.UnitOfWork,pdline,curPallet.PalletNo,"PakLoc");
                    }
                    else
                    {
                        //2.当使用Pno=@plt and Tp='PakLoc' and FL=@floor (@floor 来自页面用户的选择)查询PAK_LocMas表不存在记录时，
                        //如果使用Pno='' and Tp='PakLoc' and FL=@floor (@floor 来自页面用户的选择)查询PAK_LocMas表存在记录时，
                        //按照SnoId 字段转为整型排序，取第一条记录的SnoId 字段值保存到变量@loc 中，
                        //并更新PAK_LocMas 表中满足SnoId=@loc and Tp='PakLoc' and Pno='' 条件的记录的Pno 字段值为@plt，
                        //PdLine 字段值为Product Id 的第一位字符，Udt 为当前时间
                        //分配库位时优先选择空的，再选择库位号小的
                       
                        normalconf.pno = "";
                        
                        if (jpflag)
                        {
                            PakLocMasInfo conf = new PakLocMasInfo();
                            conf.pno = "";
                            conf.tp = "PakLoc";
                            conf.fl = floor;
                            conf.wc = "JP";
                            macList = palletRep.GetPakLocMasList(conf);
                            if (macList.Count == 0)
                            {
                                macList = palletRep.GetPakLocMasList(normalconf, neqconf);//("", "PakLoc", floor);
                            }
                        }
                        else
                        {
                            macList = palletRep.GetPakLocMasList(normalconf, neqconf);//("", "PakLoc", floor);
                        }
                        if (macList.Count >0)//(palletRep.CheckExistPakLocMas("", "PakLoc", floor))
                        {                          
                            loc = macList[macList.Count-1].snoId;
                            pdline = curProduct.ProId.Substring(0,1);
                            palletRep.UpdatePakLocMasForPdLineAndPnoDefered(CurrentSession.UnitOfWork,pdline,curPallet.PalletNo,"","PakLoc",loc);
                        }
                        else
                        {
                            //3.当不满足前面条件时，令@loc = 'Others'
                            loc = "Others";
                        }
                    }
                }
                else //ii.	如果数量不为1，则按照如下方法分配库位
                {
                    //1.查询PAK_LocMas 表，取满足条件Tp='PakLoc' and Pno=@plt 记录的SnoId 字段值保存到变量@loc 中，
                    //如果ISNULL(@loc, '') = ''，则令@loc = 'Others'
                    PakLocMasInfo normalconf = new PakLocMasInfo();
                    normalconf.pno = curPallet.PalletNo;
                    normalconf.tp = "PakLoc";

                    PakLocMasInfo neqconf = new PakLocMasInfo();
                    neqconf.wc = "JP";

                    if (jpflag)
                    {
                        PakLocMasInfo conf = new PakLocMasInfo();
                        conf.pno = curPallet.PalletNo;
                        conf.tp = "PakLoc";
                        conf.wc = "JP";
                        macList = palletRep.GetPakLocMasList(conf);
                        if (macList.Count == 0)
                        {
                            macList = palletRep.GetPakLocMasList(normalconf, neqconf);//(curPallet.PalletNo, "PakLoc");
                        }
                    }
                    else
                    {
                        macList = palletRep.GetPakLocMasList(normalconf, neqconf);//(curPallet.PalletNo, "PakLoc");
                    }
                    if (macList.Count == 0)
                    {
                        loc = "";
                    }
                    else
                    {
                        loc = macList[0].snoId;
                    }
                    if (loc == "")
                    {
                        loc = "Others";
                    }
                }
                
            }
            else if (curPallet.PalletNo.Substring(0, 2) == "NA")
            {   //b)	当LEFT(@plt,2)='NA'时，令@loc = 'OT'
                loc = "OT";
            }
            else if (curPallet.PalletNo.Substring(0, 2) == "BA")
            {   //c)当LEFT(@plt,2)='BA'时，令@loc=RTRIM(@carrier)
                loc = carrier.TrimEnd();
            }

            //i.如果LEFT(@plt,2)<>'NA' and LEFT(@plt,2)<>'BA'
            if (curPallet.PalletNo.Substring(0, 2) != "NA" && curPallet.PalletNo.Substring(0, 2) != "BA")
            {
                //a)如果@pqty <= 6，则令@loc=''，并更新PAK_LocMas 表中满足条件Pno=@plt and Tp='PakLoc' 记录的Pno 字段值为''
                if (pqty <= 6)
                {
                    loc ="";
                    palletRep.UpdatePakLocMasForPnoDefered(CurrentSession.UnitOfWork, "", curPallet.PalletNo,"PakLoc");
                }
            }

            //j.	如果@emeastr<>''，则令@loc=RTRIM(@loc)+RTRIM(@pclor)
            if (!string.IsNullOrEmpty(emeastr))
            {
                loc = loc.TrimEnd() + pclor.TrimEnd();
            }

            //k.	准备HP EDI 数据，调用存储过程op_PackingData 实现
            //snoid char(9), --Product Id
            //@model char(12), --Product Model
            //@dn char(16), --Delivery
            //@plt char(12), --Pallet
            //@loc varchar(10),--Location
            //@pltqty int --Select Sum(DeliveryQty) from Delivery_Pallet where PalletNo = @plt
            //.CallOp_PackingData(string snoid, string model, string dn, string plt, string loc, int pltqty);
            //palletRep.CallOp_PackingData(curProduct.ProId, curProduct.Model, curDelivery.DeliveryNo, curPallet.PalletNo, loc, pqty);

            /*SqlParameter[] paramsArray = new SqlParameter[8];

            paramsArray[0] = new SqlParameter("@snoID", SqlDbType.Char);
            paramsArray[0].Value = curProduct.ProId;
            paramsArray[1] = new SqlParameter("@Model", SqlDbType.Char);
            paramsArray[1].Value = curProduct.Model;
            paramsArray[2] = new SqlParameter("@Delivery", SqlDbType.Char);
            paramsArray[2].Value = curDelivery.DeliveryNo;
            paramsArray[3] = new SqlParameter("@Pallet", SqlDbType.Char);
            paramsArray[3].Value = curPallet.PalletNo;
            paramsArray[4] = new SqlParameter("@Location", SqlDbType.VarChar);
            paramsArray[4].Value = loc;
            paramsArray[5] = new SqlParameter("@pltqty", SqlDbType.Int);
            paramsArray[5].Value = pqty;
            paramsArray[6] = new SqlParameter("@BoxId", SqlDbType.VarChar);
            paramsArray[6].Value = boxId;
            paramsArray[7] = new SqlParameter("@UCC", SqlDbType.VarChar);
            paramsArray[7].Value = ucc;

            SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString_PAK, CommandType.StoredProcedure, "op_PackingData", paramsArray);*/

            ////Lock The XXX: 2012.04.20 LiuDong
            //if (!string.IsNullOrEmpty(loc))
            //{
            //    Guid gUiD = Guid.Empty;
            //    var identity = new ConcurrentLocksInfo();
            //    identity.clientAddr = "N/A";
            //    identity.customer = CurrentSession.Customer;
            //    identity.descr = string.Format("ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            //    identity.editor = CurrentSession.Editor;
            //    identity.line = CurrentSession.Line;
            //    identity.station = CurrentSession.Station;
            //    identity.timeoutSpan4Hold = new TimeSpan(0, 0, 3).Ticks;
            //    identity.timeoutSpan4Wait = new TimeSpan(0, 0, 5).Ticks;
            //    gUiD = productRep.GrabLockByTransThread("Loc", loc, identity);
            //    CurrentSession.AddValue(Session.SessionKeys.lockToken_Loc, gUiD);
            //}
            ////Lock The XXX: 2012.04.20 LiuDong

            ////Lock The XXX: 2012.04.20 LiuDong
            //if (!string.IsNullOrEmpty(boxId))
            //{
            //    Guid gUiD = Guid.Empty;
            //    var identity = new ConcurrentLocksInfo();
            //    identity.clientAddr = "N/A";
            //    identity.customer = CurrentSession.Customer;
            //    identity.descr = string.Format("ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            //    identity.editor = CurrentSession.Editor;
            //    identity.line = CurrentSession.Line;
            //    identity.station = CurrentSession.Station;
            //    identity.timeoutSpan4Hold = new TimeSpan(0, 0, 3).Ticks;
            //    identity.timeoutSpan4Wait = new TimeSpan(0, 0, 5).Ticks;
            //    gUiD = productRep.GrabLockByTransThread("Box", boxId, identity);
            //    CurrentSession.AddValue(Session.SessionKeys.lockToken_Box, gUiD);
            //}
            ////Lock The XXX: 2012.04.20 LiuDong


            ////Lock The XXX: 2012.04.20 LiuDong
            //if (!string.IsNullOrEmpty(ucc))
            //{
            //    Guid gUiD = Guid.Empty;
            //    var identity = new ConcurrentLocksInfo();
            //    identity.clientAddr = "N/A";
            //    identity.customer = CurrentSession.Customer;
            //    identity.descr = string.Format("ThreadID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
            //    identity.editor = CurrentSession.Editor;
            //    identity.line = CurrentSession.Line;
            //    identity.station = CurrentSession.Station;
            //    identity.timeoutSpan4Hold = new TimeSpan(0, 0, 3).Ticks;
            //    identity.timeoutSpan4Wait = new TimeSpan(0, 0, 5).Ticks;
            //    gUiD = productRep.GrabLockByTransThread("Ucc", ucc, identity);
            //    CurrentSession.AddValue(Session.SessionKeys.lockToken_Ucc, gUiD);
            //}
            ////Lock The XXX: 2012.04.20 LiuDong


            palletRep.CallOp_PackingDataDefered(CurrentSession.UnitOfWork, curProduct.ProId, curProduct.Model, curDelivery.DeliveryNo,
                                            curPallet.PalletNo, loc, pqty, BoxId, ucc);

            //如果Product 结合的Delivery 的Flag IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Flag' 属性为'N'，
            //RegId IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'RegId'属性不等于'SCN' 时，需要生成Box Ship Label Pdf Document
            //if ((flag == "N") && (regId != "SCN"))
            //UC Update 2012/03/28 不再限制RegId <> ‘SCN’ 才需要产生PDF
            if (flag == "N") 
            {
                CurrentSession.AddValue("CreatePDF", "pdf");
            }
            else
            {
                CurrentSession.AddValue("CreatePDF", "");
            }

            return base.DoExecute(executionContext);
        }   
    }

          
}
