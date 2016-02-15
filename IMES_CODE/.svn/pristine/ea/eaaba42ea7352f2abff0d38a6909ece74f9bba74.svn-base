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
using IMES.Infrastructure.Extend;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public enum UnPackByEnum
    {
        /// <summary>
        /// 
        /// </summary>
        Carton=0,
        /// <summary>
        /// 
        /// </summary>
        Delivery
    }

    /// <summary>
    /// 
    /// </summary>
	public partial class UnPackBackupProductByCarton: BaseActivity
	{
       /// <summary>
       /// 
       /// </summary>
		public UnPackBackupProductByCarton()
		{
			InitializeComponent();
		}

        /// <summary>
        /// UnPackDNByXXX        
        /// </summary>
        public static DependencyProperty UnPackByProperty = DependencyProperty.Register("UnPackBy", typeof(UnPackByEnum), typeof(UnPackBackupProductByCarton), new PropertyMetadata(UnPackByEnum.Carton));

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
                return ((UnPackByEnum)(base.GetValue(UnPackBackupProductByCarton.UnPackByProperty)));
            }
            set
            {
                base.SetValue(UnPackBackupProductByCarton.UnPackByProperty, value);
            }
        }


        /// <summary>
        /// ProductInfo.InfoType list
        /// </summary>
        public static DependencyProperty InfoTypeListProperty = DependencyProperty.Register("InfoTypeList", typeof(string), typeof(UnPackBackupProductByCarton), new PropertyMetadata("UCC,BoxId"));

        /// <summary>
        /// ProductInfo.InfoType
        /// </summary>
        [DescriptionAttribute("InfoTypeList")]
        [CategoryAttribute("InArguments Of UnPackBackupProductByCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string InfoTypeList
        {
            get
            {
                return ((string)(base.GetValue(UnPackBackupProductByCarton.InfoTypeListProperty)));
            }
            set
            {
                base.SetValue(UnPackBackupProductByCarton.InfoTypeListProperty, value);
            }
        }

        /// <summary>
        /// ProductInfo.InfoType list
        /// </summary>
        public static DependencyProperty IsBckupProductPartProperty = DependencyProperty.Register("IsBckupProductPart", typeof(bool), typeof(UnPackBackupProductByCarton), new PropertyMetadata(false));

        /// <summary>
        /// ProductInfo.InfoType
        /// </summary>
        [DescriptionAttribute("IsBckupProductPart")]
        [CategoryAttribute("InArguments Of UnPackBackupProductByCarton")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsBckupProductPart
        {
            get
            {
                return ((bool)(base.GetValue(UnPackBackupProductByCarton.IsBckupProductPartProperty)));
            }
            set
            {
                base.SetValue(UnPackBackupProductByCarton.IsBckupProductPartProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            Session session =CurrentSession;
            ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            IList<string> nameList = InfoTypeList.Split(new char[] { ',', ';' }).ToList();

            if (UnPackBy == UnPackByEnum.Carton)
            {
                Carton carton = (Carton)session.GetValue(Session.SessionKeys.Carton);
                IList<IProduct> prodList = (IList<IProduct>)session.GetValue(Session.SessionKeys.ProdList);
                string palletNo = null;
                IList<String> deliveryNoList = prodList.Where(y => !string.IsNullOrEmpty(y.DeliveryNo))
                                                                        .Select(x => x.DeliveryNo).Distinct().ToList();
                foreach (Product item in prodList)
                {
                    if (!string.IsNullOrEmpty(item.PalletNo))
                    {
                        palletNo = item.PalletNo;
                    }
                    prodRep.BackUpProductDefered(session.UnitOfWork, item.ProId, CurrentSession.Editor);

                    prodRep.BackUpProductStatusDefered(session.UnitOfWork, item.ProId, CurrentSession.Editor);

                    foreach (string name in nameList)
                    {
                        prodRep.BackUpProductInfoDefered(session.UnitOfWork, item.ProId, CurrentSession.Editor, name);
                    }

                    if (IsBckupProductPart)
                    {
                        prodRep.BackUpProductPartDefered(session.UnitOfWork, item.ProId, CurrentSession.Editor);
                    }
                }

                #region 清空Pallet weight
                if (!string.IsNullOrEmpty(palletNo))
                {
                    IPalletRepository currentPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                    //IList<string> dnList = prodRep.GetDeliveryNoListByPalletNo(palletNo);
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
                    PalletLog palletLog = new PalletLog { PalletNo = pallet.PalletNo, Station = "RE", Line = "Weight:0", Cdt = DateTime.Now, Editor = this.Editor };
                    pallet.AddLog(palletLog);
                    currentPalletRepository.Update(pallet, session.UnitOfWork);
                    //Clear weight in Pallet
                    if (deliveryNoList.Count > 0)
                    {
                        session.AddValue(Session.SessionKeys.DeliveryNoList, deliveryNoList);
                    }
                }
                else if (deliveryNoList.Count > 0)
                {
                    session.AddValue(Session.SessionKeys.DeliveryNoList, deliveryNoList);
                }
                #endregion

            }
            else
            {
                //Backup DN 相關的ProductID的資料
                string dnNo = (string)session.GetValue(Session.SessionKeys.DeliveryNo);

                prodRep.BackUpProductByDnDefered(session.UnitOfWork, dnNo, session.Editor);

                prodRep.BackUpProductStatusByDnDefered(session.UnitOfWork, dnNo, session.Editor);

                prodRep.BackUpProductInfoByDnDefered(session.UnitOfWork, dnNo, session.Editor, nameList);
                prodRep.BackUpProductPartByDnDefered(session.UnitOfWork, dnNo, session.Editor);


                //此DN與其他DN相關的ProductID (剩餘相關CartonSNProductID)
                IList<IProduct> prodList = (IList<IProduct>)session.GetValue(Session.SessionKeys.ProdList);

                foreach (Product item in prodList)
                {
                    prodRep.BackUpProductDefered(session.UnitOfWork, item.ProId, session.Editor);

                    prodRep.BackUpProductStatusDefered(session.UnitOfWork, item.ProId, session.Editor);

                    foreach (string name in nameList)
                    {
                        prodRep.BackUpProductInfoDefered(session.UnitOfWork, item.ProId, session.Editor, name);
                    }

                    if (IsBckupProductPart)
                    {
                        prodRep.BackUpProductPartDefered(session.UnitOfWork, item.ProId, session.Editor);
                    }
                }

                #region 清空 Reset Pallet weight
                IDeliveryRepository currentDeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
                IPalletRepository currentPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository>();
                IList<string> pnList = currentDeliveryRepository.GetPalletNoListByDeliveryNo(dnNo);
                foreach (string pn in pnList)
                {
                    //mantis1666: Unpack DN by DN，清除棧板庫位時若unpack 的 DN為棧板唯一的DN才能清庫位
                    //在Pallet 結合DN最後一筆時，才能清空Pallet Location 
                    Pallet pallet = currentPalletRepository.Find(pn);
                    IList<string> dnList = prodRep.GetDeliveryNoListByPalletNo(pn);
                    if (dnList.Count < 2)
                    {
                        PakLocMasInfo setVal = new PakLocMasInfo();
                        PakLocMasInfo cond = new PakLocMasInfo();
                        setVal.editor = Editor;
                        setVal.pno = "";
                        cond.pno = pn;
                        cond.tp = "PakLoc";
                        currentPalletRepository.UpdatePakLocMasInfoDefered(session.UnitOfWork, setVal, cond);
                        //Clear Floor in Pallet                    
                        pallet.Floor = "";
                        //Clear Floor in Pallet                    
                    }

                    //Clear  weight in Pallet 
                    pallet.Weight = 0;
                    pallet.Weight_L = 0;
                    PalletLog palletLog = new PalletLog { PalletNo = pallet.PalletNo, Station = "RETURN", Line = "Weight:0", Cdt = DateTime.Now, Editor = this.Editor };
                    pallet.AddLog(palletLog);
                    currentPalletRepository.Update(pallet, session.UnitOfWork);
                }
                #endregion
            }
            return base.DoExecute(executionContext);

        }

	}
}
