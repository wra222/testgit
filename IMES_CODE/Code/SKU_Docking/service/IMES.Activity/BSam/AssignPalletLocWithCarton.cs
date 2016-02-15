// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-20       Vincent          create
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
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure.Extend;
using IMES.DataModel;
using IMES.FisObject.PAK.StandardWeight;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
	public partial class AssignPalletLocWithCarton: BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public AssignPalletLocWithCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        ///  OverPalletLayer 值
        /// </summary>
        public static DependencyProperty OverPalletLayerProperty = DependencyProperty.Register("OverPalletLayer",
                                                                                                                                        typeof(int),
                                                                                                                                        typeof(AssignPalletLocWithCarton),
                                                                                                                                        new PropertyMetadata(3));

        /// <summary>
        /// OverPalletLayer 值
        /// </summary>
        [DescriptionAttribute("OverPalletLayer")]
        [CategoryAttribute("InArugment Of AssignPalletLocWithCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int OverPalletLayer
        {
            get
            {
                return ((int)(base.GetValue(AssignPalletLocWithCarton.OverPalletLayerProperty)));
            }
            set
            {
                base.SetValue(AssignPalletLocWithCarton.OverPalletLayerProperty, value);
            }
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
            Delivery curDelivery =(Delivery) CurrentSession.GetValue(Session.SessionKeys.Delivery);

            // g.下文描述的是针对不同的情况，如何获取需要提示用户的信息 
            //a)	取Delivery 的PalletQty (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'PalletQty')属性值，
            //并取其整数部分保存到@paletqty 变量中；如果该属性不存在，则令@paletqty = 60
            string paletqty = (string)curDelivery.GetExtendedProperty("PalletQty");
            string regId = (string)curDelivery.GetExtendedProperty("RegId");
            if (regId != null && regId.Length == 3)
            { 
                    regId = regId.Substring(1, 2); 
            }
            else
            {
                    regId = ""; 
            }
            string shipWay = (string)curDelivery.GetExtendedProperty("ShipWay");
            string carrier = (string)curDelivery.GetExtendedProperty("Carrier");
            string flag = (string)curDelivery.GetExtendedProperty("Flag");

            string model = prodList[0].Model;
            string pdline = prodList[0].ProId.Substring(0, 1);
            string ucc = curPallet.UCC;

            //if (string.IsNullOrEmpty(paletqty))
            //{
            //    paletqty = "60";
            //}
            string palletLayer = (string)curDelivery.GetExtendedProperty("PalletLayer");
            string locWC = "";

            #region CQ 棧板類型
            string emeastr = string.Empty;
            string pclor = string.Empty;
            IList<PalletType> lstPalletType = IMES.Infrastructure.Utility.Common.CommonUti.GetPalletType(curPallet.PalletNo, curDelivery.DeliveryNo);
           
            if (lstPalletType.Count == 0)
            {
                pclor = "";
                emeastr = "Other";
            }
            else
            {
                pclor = lstPalletType[0].Code.Trim();
                emeastr = lstPalletType[0].Type.Trim();
            }

            int pqty = deliveryRep.GetSumDeliveryQtyOfACertainPallet(curPallet.PalletNo);

            #endregion

            #region for CQ 不使用 PalletyQty欄位來判斷棧板類型,所以disable Code
            ////b)	使用如下方法，取得变量@pqty
            ////SELECT @pqty = sum(DeliveryQty) FROM Delivery_Pallet NOLOCK WHERE PalletNo = @Plt
            //int pqty = deliveryRep.GetSumDeliveryQtyOfACertainPallet(curPallet.PalletNo);

            ////c)	使用如下方法，取得变量@pqty2
            ////SELECT @pqty2=TierQty FROM PalletStandard WHERE FullQty=@paletqty
            //int pqty2 = palletRep.GetTierQtyFromPalletQtyInfo(paletqty);

            ////d)	如果@pqty>=@pqty2，则令@emeastr='海运，满一层请使用大的木头栈板'
            //string emeastr = string.Empty;
            //string pclor = string.Empty;

            //if (pqty > pqty2)
            //{
            //    emeastr = "海运，满一层请使用大的木头栈板";
            //}
            ////e)	如果(@Region='SNE' or @Region='SCE' ) and @shipway<>'T002'时
            ////i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='满一层请使用chep栈板'；@pclor=' H'；
            ////否则令@emeastr=' '；@pclor=''
            //if ((regId == "NE" || regId == "CE") && (shipWay != "T002"))
            //{
            //    if (pqty > pqty2 && pqty2 > 0)
            //    {
            //        emeastr = "满一层请使用chep栈板";
            //        pclor = " H";
            //    }
            //    else
            //    {
            //        emeastr = "";
            //        pclor = "";
            //    }
            //}
            //else if ((regId == "NL") && (shipWay != "T002"))
            //{   //f)	不满足上一步的条件时，如果@Region='SNL' and @shipway<>'T002' 时
            //    //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='请使用绿色塑料栈板'；@pclor=' P'；
            //    //否则令@emeastr=' '；@pclor=''
            //    if (pqty > pqty2 && pqty2 > 0)
            //    {
            //        emeastr = "请使用蓝色塑料栈板";
            //        pclor = " A";
            //    }
            //    else
            //    {
            //        emeastr = "";
            //        pclor = "";
            //    }

            //}
            //else if ((regId == "NU" || regId == "CU") && (shipWay != "T002"))
            //{    //g)	不满足前面的条件时，如果(@Region='SNU' or @Region='SCU' ) and @shipway<>'T002' 时
            //    //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='请使用蓝色塑料栈板'；@pclor=' A'；
            //    //否则令@emeastr=' '；@pclor=''
            //    if (pqty > pqty2 && pqty2 > 0)
            //    {
            //        emeastr = "请使用蓝色塑料栈板";
            //        pclor = " A";
            //    }
            //    else
            //    {
            //        emeastr = "";
            //        pclor = "";
            //    }
            //}
            //else if ((regId == "NE" || regId == "CE") && (shipWay == "T002"))
            //{
            //    //h)	不满足前面的条件时，如果(@Region='SNE' or @Region='SCE' ) and @shipway='T002' 时，
            //    //      则令@emeastr='EMEA海运,请使用E1栈板'；@pclor=' K'
            //    emeastr = "EMEA海运,请使用E1栈板";
            //    pclor = " K";
            //}
            //else if ((regId == "AF") && (shipWay == "T001"))
            //{
            //    //i)	不满足前面的条件时，如果@shipway='T001' and @Region='SAF' 时，
            //    //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='請使用綠色塑料棧板'；@pclor=' P'；
            //    //否则令@emeastr=' '；@pclor=''
            //    if (pqty > pqty2 && pqty2 > 0)
            //    {
            //        emeastr = "请使用绿色塑料栈板";
            //        pclor = " P";
            //    }
            //    else
            //    {
            //        emeastr = "";
            //        pclor = "";
            //    }

            //}
            ////else if ((   regId == "CN") && (shipWay == "T001"))
            //else if ((ActivityCommonImpl.Instance.CheckDomesticDN(regId)) && (shipWay == "T001"))
            //{
               
            //    // j)	不满足前面的条件时，如果@shipway='T001' and @Region='SCN' 时，
            //    //i.	如果@pqty>=@pqty2 and @pqty2>0，则令@emeastr='满一层请使用大的木头栈板'；@pclor=' '；
            //    //否则令@emeastr=' '；@pclor=''
            //    if (pqty > pqty2 && pqty2 > 0)
            //    {
            //        emeastr = "满一层请使用大的木头栈板";
            //        pclor = " ";
            //    }
            //    else
            //    {
            //        emeastr = "";
            //        pclor = "";
            //    }

            //}
            //else
            //{
            //    emeastr = "";
            //    pclor = "";
            //}

            #endregion
            //h.下文描述的是分配库位
            //Model 的第10，11码是”29” 或者”39” 的产品是出货日本的产品；否则，是非出货日本的产品
            //PAK_LocMas.WC = ‘JP’ 的为日本专用库位
            //出货日本的Pallet优先分配出货日本专用库位，当出货日本专用库位用完时，出货日本的Pallet 方可使用普通库位
            if (pqty <= 6)
            { locWC = "Virtual"; }
            
            string jpmodel = model.Substring(9, 2);
            //bool jpflag = false;
            if (jpmodel == "29" || jpmodel == "39")
            {
                locWC = "JP";
                //jpflag = true;
            }

            //a)	当LEFT(@plt,2)<>'NA' and LEFT(@plt,2)<>'BA' 时，按照如下方法分配库位
            string floor = (string)CurrentSession.GetValue(Session.SessionKeys.Floor);
            string loc = "";
            //Boolean addflag = true;
            if (curPallet.PalletNo.Substring(0, 2) != "NA" && curPallet.PalletNo.Substring(0, 2) != "BA")
            {
                int palletQty = deliveryRep.GetSumDeliveryQtyOfACertainPallet(curPallet.PalletNo);
                //Vincent add Check NBO包裝方式               
                int palletLayerQty = 0;
                if (!string.IsNullOrEmpty(palletLayer) &&
                     int.TryParse(palletLayer, out palletLayerQty) &&
                    palletLayerQty > this.OverPalletLayer)
                {
                    locWC = "NBO";
                }
                //i.	取与@plt 结合的Product 数量，如果数量为1，则按照如下方法分配库位
                IList<ProductModel> modelList = productRep.GetProductListByPalletNo(curPallet.PalletNo);
                IList<PakLocMasInfo> macList;
                 // For Mantis 0000874: 6台及6台以下的栈板库位分配
              
               // For Mantis 0000874: 6台及6台以下的栈板库位分配



                //string pdline;
                //ProductModel newModel = new ProductModel();
                //if (!string.IsNullOrEmpty(curPallet.PalletNo))
                //{
                //    foreach (ProductModel pmodel in modelList)
                //    {
                //        if (pmodel.ProductID == curProduct.ProId)
                //        {
                //            addflag = false;
                //            break;
                //        }
                //    }
                //    if (addflag)
                //    {
                //        newModel.ProductID = curProduct.ProId;
                //        newModel.Model = curProduct.Model;
                //        newModel.CustSN = curProduct.CUSTSN;
                //        modelList.Add(newModel);
                //    }
                //}

                if (modelList.Count == 0)
                {
                    // 1.	当使用Pno=@plt and Tp='PakLoc' and FL=@floor (@floor 来自页面用户的选择)查询PAK_LocMas表存在记录时，
                    //则取该记录的SnoId 字段值保存到变量@loc 中，并更新PAK_LocMas表中满足Pno=@plt and Tp='PakLoc' 条件的
                    //记录的PdLine 字段值为Product Id 的第一位字符，Udt 为当前时间
                    //bool chkmas = palletRep.CheckExistPakLocMas(curPallet.PalletNo, "PakLoc", floor);

                    PakLocMasInfo normalconf = new PakLocMasInfo();
                    normalconf.pno = curPallet.PalletNo;
                    normalconf.tp = "PakLoc";
                    normalconf.fl = floor;
                    normalconf.wc = locWC;
                    macList = palletRep.GetPakLocMasList(normalconf);
                    if (macList.Count == 0 && !string.IsNullOrEmpty(locWC))
                    {
                        normalconf.wc = "";
                        macList = palletRep.GetPakLocMasList(normalconf);
                    }

                    if (macList.Count > 0)
                    {
                        loc = macList[0].snoId;
                        
                        palletRep.UpdatePakLocMasByIdDefered(CurrentSession.UnitOfWork,
                                                                                       macList[0].id,
                                                                                       curPallet.PalletNo,
                                                                                       pdline,
                                                                                       this.Editor);
                       
                    }
                    else
                    {
                        normalconf.pno = "";
                        normalconf.tp = "PakLoc";
                        normalconf.fl = floor;
                        normalconf.wc = locWC;

                        macList = palletRep.GetPakLocMasList(normalconf);
                        if (macList.Count == 0 && !string.IsNullOrEmpty(locWC))
                        {
                            normalconf.wc = "";
                            macList = palletRep.GetPakLocMasList(normalconf);
                        }

                        if (macList.Count > 0)//(palletRep.CheckExistPakLocMas("", "PakLoc", floor))
                        {
                            loc = macList[macList.Count - 1].snoId;                           
                            palletRep.UpdatePakLocMasByIdDefered(CurrentSession.UnitOfWork,
                                                                                     macList[macList.Count - 1].id,
                                                                                     curPallet.PalletNo,
                                                                                     pdline,
                                                                                     this.Editor);
                            //palletRep.UpdatePakLocMasForPdLineAndPnoDefered(CurrentSession.UnitOfWork,pdline,curPallet.PalletNo,"","PakLoc",loc);                            
                        }
                        else
                        {
                            //3.当不满足前面条件时，令@loc = 'Others'
                            loc = "Others";
                        }

                    }

                    #region  disable Code Vincent 不使用負面表列方式找庫位
                    //PakLocMasInfo neqconf = new PakLocMasInfo();
                    //neqconf.wc = "JP";
                    //if (jpflag)
                    //{
                    //    PakLocMasInfo conf = new PakLocMasInfo();
                    //    conf.pno = curPallet.PalletNo;
                    //    conf.tp = "PakLoc";
                    //    conf.fl = floor;
                    //    conf.wc = "JP";
                    //    macList = palletRep.GetPakLocMasList(conf);
                    //    if (macList.Count == 0)
                    //    {
                    //        macList = palletRep.GetPakLocMasList(normalconf, neqconf);//(curPallet.PalletNo, "PakLoc", floor);
                    //    }
                    //}
                    //else
                    //{
                    //    macList = palletRep.GetPakLocMasList(normalconf, neqconf);//(curPallet.PalletNo, "PakLoc", floor);
                    //}
                    //if (macList.Count > 0)
                    //{
                    //    loc = macList[0].snoId;
                    //    //pdline = curProduct.ProId.Substring(0, 1);
                    //    palletRep.UpdatePakLocMasForPdLineDefered(CurrentSession.UnitOfWork, pdline, curPallet.PalletNo, "PakLoc");
                    //}
                    //else
                    //{
                    //    //2.当使用Pno=@plt and Tp='PakLoc' and FL=@floor (@floor 来自页面用户的选择)查询PAK_LocMas表不存在记录时，
                    //    //如果使用Pno='' and Tp='PakLoc' and FL=@floor (@floor 来自页面用户的选择)查询PAK_LocMas表存在记录时，
                    //    //按照SnoId 字段转为整型排序，取第一条记录的SnoId 字段值保存到变量@loc 中，
                    //    //并更新PAK_LocMas 表中满足SnoId=@loc and Tp='PakLoc' and Pno='' 条件的记录的Pno 字段值为@plt，
                    //    //PdLine 字段值为Product Id 的第一位字符，Udt 为当前时间
                    //    //分配库位时优先选择空的，再选择库位号小的

                    //    normalconf.pno = "";

                    //    if (jpflag)
                    //    {
                    //        PakLocMasInfo conf = new PakLocMasInfo();
                    //        conf.pno = "";
                    //        conf.tp = "PakLoc";
                    //        conf.fl = floor;
                    //        conf.wc = "JP";
                    //        macList = palletRep.GetPakLocMasList(conf);
                    //        if (macList.Count == 0)
                    //        {
                    //            macList = palletRep.GetPakLocMasList(normalconf, neqconf);//("", "PakLoc", floor);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        macList = palletRep.GetPakLocMasList(normalconf, neqconf);//("", "PakLoc", floor);
                    //    }
                    //    if (macList.Count > 0)//(palletRep.CheckExistPakLocMas("", "PakLoc", floor))
                    //    {
                    //        loc = macList[macList.Count - 1].snoId;
                    //        //pdline = curProduct.ProId.Substring(0, 1);
                    //        palletRep.UpdatePakLocMasForPdLineAndPnoDefered(CurrentSession.UnitOfWork, pdline, curPallet.PalletNo, "", "PakLoc", loc);
                    //    }
                    //    else
                    //    {
                    //        //3.当不满足前面条件时，令@loc = 'Others'
                    //        loc = "Others";
                    //    }
                    //}
                    #endregion
                }
                else //ii.	如果数量不为1，则按照如下方法分配库位
                {
                    //1.查询PAK_LocMas 表，取满足条件Tp='PakLoc' and Pno=@plt 记录的SnoId 字段值保存到变量@loc 中，
                    //如果ISNULL(@loc, '') = ''，则令@loc = 'Others'
                   
                    PakLocMasInfo normalconf = new PakLocMasInfo();
                    normalconf.pno = curPallet.PalletNo;
                    normalconf.tp = "PakLoc";
                    normalconf.wc = locWC;
                    macList = palletRep.GetPakLocMasList(normalconf);
                    if (macList.Count == 0 && !string.IsNullOrEmpty(locWC))
                    {
                        normalconf.wc = "";
                        macList = palletRep.GetPakLocMasList(normalconf);
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


                    #region  disable Code Vincent 不使用負面表列方式找庫位
                    //PakLocMasInfo neqconf = new PakLocMasInfo();
                    //neqconf.wc = "JP";

                    //if (jpflag)
                    //{
                    //    PakLocMasInfo conf = new PakLocMasInfo();
                    //    conf.pno = curPallet.PalletNo;
                    //    conf.tp = "PakLoc";
                    //    conf.wc = "JP";
                    //    macList = palletRep.GetPakLocMasList(conf);
                    //    if (macList.Count == 0)
                    //    {
                    //        macList = palletRep.GetPakLocMasList(normalconf, neqconf);//(curPallet.PalletNo, "PakLoc");
                    //    }
                    //}
                    //else
                    //{
                    //    macList = palletRep.GetPakLocMasList(normalconf, neqconf);//(curPallet.PalletNo, "PakLoc");
                    //}
                    //if (macList.Count == 0)
                    //{
                    //    loc = "";
                    //}
                    //else
                    //{
                    //    loc = macList[0].snoId;
                    //}
                    //if (loc == "")
                    //{
                    //    loc = "Others";
                    //}
                    #endregion
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
            /*
            if (curPallet.PalletNo.Substring(0, 2) != "NA" && curPallet.PalletNo.Substring(0, 2) != "BA")
            {
                //a)如果@pqty <= 6，则令@loc=''，并更新PAK_LocMas 表中满足条件Pno=@plt and Tp='PakLoc' 记录的Pno 字段值为''
                if (pqty <= 6)
                {
                    loc = "";
                    palletRep.UpdatePakLocMasForPnoDefered(CurrentSession.UnitOfWork, "", curPallet.PalletNo, "PakLoc");
                }
            }*/

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

            foreach (Product item in prodList)
            {
                palletRep.CallOp_PackingDataDefered(CurrentSession.UnitOfWork, item.ProId, item.Model, item.DeliveryNo,
                                            item.PalletNo, loc, pqty, carton.BoxId, ucc);
            }  
            //如果Product 结合的Delivery 的Flag IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Flag' 属性为'N'，
            //RegId IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'RegId'属性不等于'SCN' 时，需要生成Box Ship Label Pdf Document
            //if ((flag == "N") && (regId != "SCN"))
            //UC Update 2012/03/28 不再限制RegId <> ‘SCN’ 才需要产生PDF
            if (flag == "N")
            {
              CurrentSession.AddValue(Session.SessionKeys.DeliveryNo, curDelivery.DeliveryNo);
                /*   //Save PDF File Name
              string PdfFileName=curDelivery.DeliveryNo+ "-"+carton.CartonSN+"-[BoxShipLabel].pdf";
              carton.SetExtendedProperty("PdfFileName", PdfFileName,this.Editor);
             // var pdffilename = dn + "-" + key + "-[BoxShipLabel].pdf" */
               CurrentSession.AddValue("CreatePDF", "pdf");
            }
            else
            {
                CurrentSession.AddValue("CreatePDF", "");
            }

            //Change Delivery Status -> 88
            IList<Delivery> dnList = (IList<Delivery>)CurrentSession.GetValue(Session.SessionKeys.DeliveryList);
            if (dnList == null || dnList.Count == 0)
            {
                dnList = new List<Delivery>();
                foreach (DeliveryCarton item in bindDNList)
                {                   
                    if (curDelivery.DeliveryNo == item.DeliveryNo)
                    {                        
                    dnList.Add(curDelivery);
                    }
                    else
                    {
                        dnList.Add(deliveryRep.Find(item.DeliveryNo));
                    }                   
                }
            }
            
            foreach (Delivery item in dnList)
            {
                int dvQty = productRep.GetCombinedPalletQtyByDN(item.DeliveryNo);
                var dn = (from p in bindDNList
                          where p.DeliveryNo == item.DeliveryNo
                          select p).ToList();
                if (dn.Count>0)
                {
                        dvQty= dvQty+ dn[0].AssignQty;
                }

                 int dnQty = item.Qty;
                string dnStatus = item.Status;                

                if (dvQty == dnQty && dnStatus != "88")
                {
                    item.Status = "88";
                    item.Editor = CurrentSession.Editor;
                    item.Udt = DateTime.Now;
                    DeliveryLog log = new DeliveryLog(0, item.DeliveryNo, "88", CurrentSession.Station, CurrentSession.Line, CurrentSession.Editor, DateTime.Now);
                    item.AddLog(log);
                    deliveryRep.Update(item, CurrentSession.UnitOfWork);
                }
            }

            CurrentSession.AddValue(ExtendSession.SessionKeys.PalletLoc, loc);
            CurrentSession.AddValue(Session.SessionKeys.Product,prodList[0]);
            CurrentSession.AddValue(Session.SessionKeys.IsBT, 0);

            return base.DoExecute(executionContext);

        }
	}
}
