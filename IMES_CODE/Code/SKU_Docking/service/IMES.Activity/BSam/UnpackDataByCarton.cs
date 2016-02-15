// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Vincent          create
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PAK.DN;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public enum UnPackEnum
    {
        /// <summary>
        /// 解DN
        /// </summary>
        Delivery=0,
        /// <summary>
        /// 解 Pallet
        /// </summary>
        Pallet
    }
    /// <summary>
    /// 
    /// </summary>
	public partial class UnpackDataByCarton: BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public UnpackDataByCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// UnPackDNByXXX        
        /// </summary>
        public static DependencyProperty UnPackByProperty = DependencyProperty.Register("UnPackBy", typeof(UnPackByEnum), typeof(UnpackDataByCarton), new PropertyMetadata(UnPackByEnum.Carton));

        /// <summary>
        /// UnPackDNByXXX
        /// </summary>
        [DescriptionAttribute("UnPackBy")]
        [CategoryAttribute("InArguments Of UnPackBackupProductByCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public UnPackByEnum UnPackBy
        {
            get
            {
                return ((UnPackByEnum)(base.GetValue(UnpackDataByCarton.UnPackByProperty)));
            }
            set
            {
                base.SetValue(UnpackDataByCarton.UnPackByProperty, value);
            }
        }

        /// <summary>
        /// ProductInfo.InfoType list
        /// </summary>
        public static DependencyProperty InfoTypeListProperty = DependencyProperty.Register("InfoTypeList", typeof(string), typeof(UnpackDataByCarton), new PropertyMetadata("UCC,BoxId"));

        /// <summary>
        /// ProductInfo.InfoType
        /// </summary>
        [DescriptionAttribute("InfoTypeList")]
        [CategoryAttribute("InArguments Of UnpackDataByCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string InfoTypeList
        {
            get
            {
                return ((string)(base.GetValue(UnpackDataByCarton.InfoTypeListProperty)));
            }
            set
            {
                base.SetValue(UnpackDataByCarton.InfoTypeListProperty, value);
            }
        }


        /// <summary>
        /// ProductInfo.InfoType list
        /// </summary>
        public static DependencyProperty UnPackCategoryProperty = DependencyProperty.Register("UnPackCategory", typeof(UnPackEnum), typeof(UnpackDataByCarton), new PropertyMetadata(UnPackEnum.Delivery));

        /// <summary>
        /// ProductInfo.InfoType
        /// </summary>
        [DescriptionAttribute("UnPackCategory")]
        [CategoryAttribute("InArguments Of UnpackDataByCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public UnPackEnum UnPackCategory
        {
            get
            {
                return ((UnPackEnum)(base.GetValue(UnpackDataByCarton.UnPackCategoryProperty)));
            }
            set
            {
                base.SetValue(UnpackDataByCarton.UnPackCategoryProperty, value);
            }
        }


        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
             IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            Carton carton = (Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
            string dnNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            IList<IProduct> prodList = (IList<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList);
            IList<Delivery> dnList = (IList<Delivery>)CurrentSession.GetValue(Session.SessionKeys.DeliveryList);
            IList<CartonProductInfo> cartonProdInfoList = (IList<CartonProductInfo>)CurrentSession.GetValue(ExtendSession.SessionKeys.CartonProductInfoList);

            if (UnPackCategory == UnPackEnum.Pallet && UnPackBy == UnPackByEnum.Delivery)
            {
                throw new Exception("Not Support UnPackPalletByDN function!!"); 
            }

            IList<string> nameList = InfoTypeList.Split(new char[] { ',', ';' }).ToList();
            var typeList = (from p in nameList
                            where string.IsNullOrEmpty(p) == false
                            select p).ToList();

            var cartonSNList = (from p in prodList
                                select p.CartonSN).Distinct().ToList();

            #region unpack by Delivery
            if (UnPackBy == UnPackByEnum.Delivery)
            {
                #region
                //1.Upack Product
                prodRep.UnPackProductByDeliveryNoDefered(CurrentSession.UnitOfWork, dnNo);
                //2.Unpack ProductInfo
                  foreach (string InfoType in typeList)
                {
                    //Clear productInfo
                    prodRep.UnPackProductInfoByDeliveryNoDefered(CurrentSession.UnitOfWork, InfoType, dnNo);
                }
                //3. unpack EDi Packing Data                
                 palletRep.UnPackPackingDataByDeliveryNoDefered(CurrentSession.UnitOfWork, dnNo);
                //4.unpack EDI ODMSessionData
                 palletRep.UnPackPakOdmSessionByDeliveryNoDefered(CurrentSession.UnitOfWork, dnNo);
                //5.unpackShipBoxDet                 
                 dnRep.UpdateSnoidForShipBoxDetDefered(CurrentSession.UnitOfWork, "", dnNo);
                 //6.Update DeliveryStatus below doing this action             

                 //7.update carton status to UnPack by DN 
                 cartonRep.unBindCartonByDnDefered(CurrentSession.UnitOfWork, dnNo,CurrentSession.Editor);
                
                #endregion
            }
            #endregion

            #region unpack by Carton
            IList<string> custSNList = new List<string>();

            //1.Update Product
           
            foreach (Product item in prodList)
            {
                if (UnPackCategory == UnPackEnum.Delivery)
                {
                    //1.Clear Product 
                    item.DeliveryNo = "";
                    item.PalletNo = "";
                    item.CartonSN = "";
                    item.CartonWeight = 0;
                    item.UnitWeight = 0;
                    if (typeList.Count > 0)
                    {
                        //2.Clear productInfo
                        prodRep.RemoveProductInfosByTypeDefered(CurrentSession.UnitOfWork, item.ProId, typeList);
                    }
                    custSNList.Add(item.CartonSN);
                }
                else
                {
                    item.PalletNo = "";
                }                
               prodRep.Update(item, CurrentSession.UnitOfWork);
              
            }

            if (UnPackCategory == UnPackEnum.Delivery)
            {
                #region unpack DN
                //3.Clear EDI PackingDtaa
                if (cartonProdInfoList != null && cartonProdInfoList.Count > 0)
                {
                    var boxIdList = (from p in cartonProdInfoList
                                     select p.BoxId).Distinct().ToList();
                    foreach (string boxId in boxIdList)
                    {
                        cartonRep.RemoveEdiPackingDataDefered(CurrentSession.UnitOfWork, boxId);               
                    }  
                }
                else if (UnPackBy== UnPackByEnum.Carton)
                {
                    cartonRep.RemoveEdiPackingDataDefered(CurrentSession.UnitOfWork, carton.BoxId);
                }

                if (custSNList.Count > 0)
                {
                    //4.Clear EDI ODMSession 
                    cartonRep.RemoveEdiODMSessionDefered(CurrentSession.UnitOfWork, custSNList);
                }

                //5.ClearSnoIdInShipBoxDet
                foreach (string sn in cartonSNList)
                {
                    cartonRep.ClearSnoIdInShipBoxDetDefered(CurrentSession.UnitOfWork, 
                                                                                            //carton.CartonSN,
                                                                                            sn,
                                                                                            CurrentSession.Editor);
                    if (UnPackBy == UnPackByEnum.Carton)
                    {
                        //7.update carton status to UnPack
                        cartonRep.unBindCartonDefered(CurrentSession.UnitOfWork, sn, CurrentSession.Editor);
                    }
                }
                //6.Update DeliveryStatus
                foreach (Delivery item in dnList)
                {
                    if (item.Status != "00")
                    {
                        item.Status = "00";
                        DeliveryLog log = new DeliveryLog(0, item.DeliveryNo, "00", CurrentSession.Station, CurrentSession.Line, CurrentSession.Editor, DateTime.Now);
                        item.AddLog(log);
                        dnRep.Update(item, CurrentSession.UnitOfWork);
                    }
                }
                
                #endregion
            }
            else
            {
                #region unpack pallet
                //Update DeliveryStatus
                foreach (Delivery item in dnList)
                {
                    if (item.Status != "00")
                    {
                        int qty=prodRep.GetCombinedQtyByDN(item.DeliveryNo);
                        if (item.Qty > qty)
                        {
                            item.Status = "00";
                            DeliveryLog log = new DeliveryLog(0, item.DeliveryNo, "00", CurrentSession.Station, CurrentSession.Line, CurrentSession.Editor, DateTime.Now);
                            item.AddLog(log);
                            dnRep.Update(item, CurrentSession.UnitOfWork);

                        }
                        else if(item.Status != "87")
                        {
                            item.Status = "87";
                            DeliveryLog log = new DeliveryLog(0, item.DeliveryNo, "87", CurrentSession.Station, CurrentSession.Line, CurrentSession.Editor, DateTime.Now);
                            item.AddLog(log);
                            dnRep.Update(item, CurrentSession.UnitOfWork);
                        }
                       
                    }
                }

                //Update Carton.UnpackPalletNo & clear Carton.PalletNo
                carton.UnPackPalletNo = carton.PalletNo;
                carton.PalletNo = "";
                cartonRep.Update(carton, CurrentSession.UnitOfWork);
 
                #endregion
            }
            #endregion


            
            // update pallet location
            if (UnPackBy == UnPackByEnum.Delivery)
            {
                IList<string> pnList = dnRep.GetPalletNoListByDeliveryNo(dnNo);
               
                bool isMultiDn = false;
                if (cartonProdInfoList != null && cartonProdInfoList.Count > 0)
                {
                    isMultiDn = true;
                }

                // need check logical 
                foreach (string pn in pnList)
                {
                    //mantis1666: Unpack DN by DN，清除棧板庫位時若unpack 的 DN為棧板唯一的DN才能清庫位
                    //在Pallet 結合DN最後一筆時，才能清空Pallet Location 
                    IList<string> combineDnList = prodRep.GetDeliveryNoListByPalletNo(pn);
                    int palletQty=2;
                    if (isMultiDn)
                    {
                        palletQty = palletQty +(from p in cartonProdInfoList
                                                            where p.PalletNo == pn
                                                            select new { p.PalletNo, p.DeliveryNo }).Distinct().ToList().Count;
                    }
                    if (combineDnList.Count < palletQty)
                    {
                        PakLocMasInfo setVal = new PakLocMasInfo();
                        PakLocMasInfo cond = new PakLocMasInfo();
                        setVal.editor = Editor;
                        setVal.pno = "";
                        cond.pno = pn;
                        cond.tp = "PakLoc";
                        palletRep.UpdatePakLocMasInfoDefered(CurrentSession.UnitOfWork, setVal, cond);
                    }
                }
                
            }
            else
            {
                IList<string> cartonSNList1 = cartonRep.GetCartonSNListByPalletNo(carton.PalletNo, false);
                if (cartonSNList1.Count < 2)
                {
                    PakLocMasInfo setVal = new PakLocMasInfo();
                    PakLocMasInfo cond = new PakLocMasInfo();
                    setVal.editor = Editor;
                    setVal.pno = "";
                    cond.pno = carton.PalletNo;
                    cond.tp = "PakLoc";
                    palletRep.UpdatePakLocMasInfoDefered(CurrentSession.UnitOfWork, setVal, cond);
                }

               
            }





             return base.DoExecute(executionContext);

        }

	}
}
