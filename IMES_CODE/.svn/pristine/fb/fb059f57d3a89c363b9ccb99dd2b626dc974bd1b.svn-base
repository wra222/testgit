// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据Session中的DummyPalletCase,获取FDE Pallet Qty数量，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
// UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                                                 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-23   Chen Xu (itc208014)          create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.DataModel;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity.NewNeedCheck
{
    /// <summary>
    /// 根据Session中的DummyPalletCase,获取 FDE Pallet Qty对象，并放到Session中
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
    public partial class GetFDEPalletQty : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetFDEPalletQty()
		{
			InitializeComponent();
		}
        /// <summary>
        /// SVN 2569: change to palletQty
        /// Get FDE Pallet Qty and put it into Session.SessionKeys.PalletQty
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {   
            string DPC = (string)CurrentSession.GetValue(Session.SessionKeys.DummyPalletCase);
            string dummyPalletNo = (string) CurrentSession.GetValue(Session.SessionKeys.DummyPalletNo);
            int PalletQty = 0;
            IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery currentDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            IPalletRepository iPalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IList<DummyShipDetInfo> dummyshipdetLst = new List<DummyShipDetInfo>();
            IList<Delivery> DeliveryList = new List<Delivery>();
            FisException ex;
            List<string> erpara = new List<string>();

            switch (DPC)
            {
                case "NA":
                     
                    dummyshipdetLst =iPalletRepository.GetDummyShipDetListByPlt(dummyPalletNo);
                    if (dummyshipdetLst!=null && dummyshipdetLst.Count > 0)
                    {
                        PalletQty = dummyshipdetLst.Count;
                         IList<string> prodNoList = new List<string>();
                         foreach (DummyShipDetInfo iDSD in dummyshipdetLst)
                         {
                             prodNoList.Add(iDSD.snoId);
                             CurrentSession.AddValue(Session.SessionKeys.ProdNoList, PalletQty.ToString());
                         }
                   
                    }
                    else PalletQty = 0;
                    break;
                case "NAN":
                    //int scanQty = Int32.Parse((string) CurrentSession.GetValue(Session.SessionKeys.ScanQty));
                    //int deliveryQty = currentDelivery.Qty;
                    //PalletQty = scanQty + deliveryQty;
                    
                    //	For Case NA Non Dummy Pallet，Pallet Qty 为IMES_PAK..Delivery 中满足BOL 属性
                    //  = 方才刷入的Customer S/N 对应的Product 结合的Delivery 的BOL (IMES_PAK..DeliveryInfo.InfoValue，
                    //  Condition: InfoType = 'BOL')属性值的记录的Qty （IMES_PAK..Delivery.Qty）之和

                    //DeliveryList = ideliveryRepository.GetDeliveryListByInfoType("BOL");
                    string dn = (string) CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                    string DeliveryValue= ideliveryRepository.GetDeliveryInfoValue(dn, "BOL");

                    DeliveryList = ideliveryRepository.GetDeliveryListByInfoValue(DeliveryValue);
                    if (DeliveryList == null || DeliveryList.Count <= 0)
                    {
                        erpara.Add("BOL");
                        ex = new FisException("CHK020", erpara);
                        throw ex;
                    }
                    foreach (Delivery idn in DeliveryList)
                    {
                            PalletQty = PalletQty + idn.Qty;
                    }

                    break;
                case "BA":
                    dummyshipdetLst=iPalletRepository.GetDummyShipDetListByPlt(dummyPalletNo);
                    if (dummyshipdetLst!=null && dummyshipdetLst.Count > 0)
                    {
                        PalletQty = dummyshipdetLst.Count;
                        IList<string> prodNoList = new List<string>();
                        foreach (DummyShipDetInfo iDSD in dummyshipdetLst)
                        {
                            prodNoList.Add(iDSD.snoId);
                            CurrentSession.AddValue(Session.SessionKeys.ProdNoList, PalletQty.ToString());
                        }
                    }
                    else PalletQty = 0;
                    break;
                case "BAN":
                    string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                    string carrier = ideliveryRepository.GetDeliveryInfoValue(deliveryNo, "Carrier");
                    IList<string> DNLst = new List<string>();
                    DNLst = ideliveryRepository.GetDeliveryNoByShipDateAndValueAndType(currentDelivery.ShipDate,carrier,"Carrier");
                    if (DNLst == null)
                    {
                        PalletQty = 0;
                    }
                    else
                    {
                        foreach (string idn in DNLst)
                        {
                            IList<PalletInfo> palletInfoLst = new List<PalletInfo>();
                            palletInfoLst = iPalletRepository.GetPalletList(idn);
                            if (palletInfoLst == null || palletInfoLst.Count <= 0)
                            {
                                PalletQty = 0;
                            }
                            else 
                            {
                                PalletQty += palletInfoLst.Count;
                            }
                            
                        }
                    }
                    break;
                default:
                    break;
            }

            CurrentSession.AddValue(Session.SessionKeys.PalletQty, PalletQty.ToString());

            return base.DoExecute(executionContext);
        }
	}

}
