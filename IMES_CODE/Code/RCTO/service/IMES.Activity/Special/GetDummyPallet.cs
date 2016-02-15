// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据Session中的DummyPalletCase,获取Dummy Pallet No对象，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI Pallet Verify.docx 
// UC:CI-MES12-SPEC-PAK-UC Pallet Verify.docx                                                 
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-22   Chen Xu (itc208014)          create
// Known issues:
using System;
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
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity.NewNeedCheck
{
    /// <summary>
    /// 根据Session中的DummyPalletCase,获取Dummy Pallet No对象，并放到Session中
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
    ///         1.根据DeliveryNo，调用IDeliveryRepository的GetDeliveryInfoValue方法，获取dummyPalletNo对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DummyPalletCase, Session.DeliveryNo
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
    ///              IPalletRepository ,IDeliveryRepository
    ///              Pallet,DeliveryNo
    /// </para> 
    /// </remarks>
    public partial class GetDummyPallet : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetDummyPallet()
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
            DummyShipDetInfo dummyshipdet = new DummyShipDetInfo();
            dummyshipdet = (DummyShipDetInfo)CurrentSession.GetValue(Session.SessionKeys.DummyShipDet);
            string deliveryNo = (string) CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            string dummyPalletNo = string.Empty;
            IDeliveryRepository ideliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            
            switch (DPC)
            {
                case "NA":
                    if (dummyshipdet == null || string.IsNullOrEmpty(dummyshipdet.plt))
                    {
                        FisException fe = new FisException("PAK106", new string[] { });  //请维护DummyShipDet表数据！
                        throw fe;

                    }
                    dummyPalletNo = dummyshipdet.plt;
                    break;
                case "NAN":
                    dummyPalletNo = ideliveryRepository.GetDeliveryInfoValue(deliveryNo, "BOL");
                    if (string.IsNullOrEmpty(dummyPalletNo))
                    {
                        erpara.Add("BOL");
                        ex = new FisException("PAK047", erpara); ////没有InfoType为 %1 的DeliveryInfo！
                        throw ex;
                    }
                    break;
                case "BA":
                    if (dummyshipdet == null || string.IsNullOrEmpty(dummyshipdet.plt))
                    {
                        FisException fe = new FisException("PAK106", new string[] { });  //请维护DummyShipDet表数据！
                        throw fe;

                    }
                    dummyPalletNo = dummyshipdet.plt;
                    break;
                case "BAN":
                    Delivery currentDelivery = (Delivery) CurrentSession.GetValue(Session.SessionKeys.Delivery);
                    //ITC-1360-1368:Revision: 8761 : 增加说明Delivery.ShipDate 在Pallet Verify (FDE Only) 的业务中使用的时候，需要转换为yyyy/mm/dd 格式的字符串使用
                    //string shipDate = currentDelivery.ShipDate.ToString();
                    string shipDate = currentDelivery.ShipDate.ToString("yyyy/MM/dd");
                    if (string.IsNullOrEmpty(shipDate))
                    {
                        FisException fe = new FisException("PAK056", new string[] { });  //Delivery %1 的资料维护不全！
                        throw fe;
                    }
                    string carrier = ideliveryRepository.GetDeliveryInfoValue(deliveryNo, "Carrier");
                    if (string.IsNullOrEmpty(carrier))
                    {
                        erpara.Add("Carrier");
                        ex = new FisException("PAK047", erpara);    //没有InfoType为 %1 的DeliveryInfo！
                        throw ex;
                    }                   
                    dummyPalletNo = shipDate + carrier;
                    break;
                default:
                    break;
            }

            //ITC-1360-1276: Dummy_ShipDet.PLT才是Dummy Pallet No,而ShipDate (IMES_PAK..Delivery.ShipDate)属性值和 Carrier (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Carrier')属性值仅是UI 显示的Pallet No 
            // Check the Dummy Pallet No Naming Rule: Prefix:90
            //if (dummyPalletNo.Substring(0, 2) != "90")
            //{
            //    erpara.Add(dummyPalletNo);
            //    ex = new FisException("PAK048", erpara);
            //    throw ex;
            //}

            CurrentSession.AddValue(Session.SessionKeys.DummyPalletNo, dummyPalletNo);

            return base.DoExecute(executionContext);
        }
	}

}
