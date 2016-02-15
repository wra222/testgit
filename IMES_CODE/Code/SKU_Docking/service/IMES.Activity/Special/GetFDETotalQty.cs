// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据Session中的DummyPalletCase,获取FDE Total Qty数量，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
// UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                                                 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-22   Chen Xu (itc208014)          create
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
    /// 根据Session中的DummyPalletCase,获取 FDE Total Qty对象，并放到Session中
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
    public partial class GetFDETotalQty : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetFDETotalQty()
		{
			InitializeComponent();
		}
        /// <summary>
        /// SVN 2569: Add new
        /// Get FDE Total Qty and put it into Session.SessionKeys.Qty
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //FisException ex;
            //List<string> erpara = new List<string>();

            string DPC = (string)CurrentSession.GetValue(Session.SessionKeys.DummyPalletCase);
            string dummyPalletNo = (string) CurrentSession.GetValue(Session.SessionKeys.DummyPalletNo);
            IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IList<Delivery> DeliveryList = new List<Delivery>();
            int TotalQty = 0;

            switch (DPC)
            {
                case "NA":
                    TotalQty = 0;
                    break;
                case "NAN":
                    //DeliveryList= ideliveryRepository.GetDeliveryListByInfoType("BOL");
                    //if (DeliveryList == null || DeliveryList.Count <= 0)
                    //{
                    //    erpara.Add("BOL");
                    //    ex = new FisException("CHK020", erpara);
                    //    throw ex;
                    //}
                    //TotalQty = DeliveryList.Count;
                    
                    //ITC-1360-1743
                    //For Case NA Non Dummy Pallet，Total Qty 为IMES_PAK..Dummy_ShipDet 中
                    //满足BOL = 方才刷入的Customer S/N 对应的Product 结合的Delivery 的BOL(IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'BOL')属性值的记录总数

                    string dn = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                    string DeliveryValue = ideliveryRepository.GetDeliveryInfoValue(dn, "BOL");

                    DummyShipDetInfo dummyshipdet = new DummyShipDetInfo();
                    dummyshipdet.bol = DeliveryValue;
                   
                    IList<DummyShipDetInfo> dummyshipdetList = null;
                    dummyshipdetList = ideliveryRepository.GetDummyShipDetInfoList(dummyshipdet);

                    if (dummyshipdetList == null || dummyshipdetList.Count <= 0)
                    {
                        //erpara.Add("BOL");
                        //ex = new FisException("CHK020", erpara);
                        //throw ex;
                        TotalQty = 0;
                    }
                    TotalQty = dummyshipdetList.Count;

                    break;
                case "BA":
                    TotalQty = 0;
                    break;
                case "BAN":
                    //DeliveryList = ideliveryRepository.GetDeliveryListByInfoType("Carrier");
                    //if (DeliveryList == null || DeliveryList.Count <= 0)
                    //{
                    //    erpara.Add("BOL");
                    //    ex = new FisException("PAK047", erpara);
                    //    throw ex;
                    //}
                    //TotalQty = DeliveryList.Count;

                    //For Case BA Non Dummy Pallet，Total Qty 为Dummy_ShipDet 表中
                    //BOL = 方才刷入的Customer S/N 对应的Product 结合的Delivery (IMES_FA..Product.DeliveryNo)的ShipDate (IMES_PAK..Delivery.ShipDate)属性值 + Carrier 
                    //(IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Carrier')属性值相等的记录数量

                    // 其实就是dummyPalletNo (dummyPalletNo = shipDate + carrier;)

                    DummyShipDetInfo BANdummyshipdet = new DummyShipDetInfo();
                    BANdummyshipdet.bol = dummyPalletNo;

                    IList<DummyShipDetInfo> BANdummyshipdetList = null;
                    BANdummyshipdetList = ideliveryRepository.GetDummyShipDetInfoList(BANdummyshipdet);

                    if (BANdummyshipdetList == null || BANdummyshipdetList.Count <= 0)
                    {
                        //erpara.Add("BOL");
                        //ex = new FisException("CHK020", erpara);
                        //throw ex;
                        TotalQty = 0;
                    }
                    TotalQty = BANdummyshipdetList.Count;
                    

                    break;
                default:
                    break;
            }

            CurrentSession.AddValue(Session.SessionKeys.Qty, TotalQty.ToString());

            return base.DoExecute(executionContext);
        }
	}

}
