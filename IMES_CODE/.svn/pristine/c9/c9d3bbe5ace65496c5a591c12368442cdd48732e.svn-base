// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块的删除DN功能中的删除Delivery、Pallet部分子功能
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-09   itc202017                     Create
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using System.Collections.Generic;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块的删除DN功能中的删除Delivery、Pallet部分子功能
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Delete DN for PL user
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         按照DN删除Delivery、DeliveryInfo、Delivery_Pallet、Pallet相关数据
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.DeliveryNo
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    ///              IPalletRepository
    /// </para> 
    /// </remarks>
    public partial class DeleteDN : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeleteDN()
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
            string dn = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            string ship = (string)CurrentSession.GetValue("ShipmentNo");

            IDeliveryRepository currentDNRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPalletRepository currentPltRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            if (dn != null && dn != "")
            {
                CurrentSession.AddValue("ShipmentNo", currentDNRepository.Find(dn).ShipmentNo);
                IList<string> pnList = currentDNRepository.GetPalletNoListByDnAndWithSoloDn(dn);
                currentDNRepository.DeleteDeliveryAttrLogByDnDefered(CurrentSession.UnitOfWork, dn);
                currentDNRepository.DeleteDeliveryAttrsByDnDefered(CurrentSession.UnitOfWork, dn);
                currentDNRepository.DeleteDeliveryInfoByDnDefered(CurrentSession.UnitOfWork, dn);
                currentDNRepository.DeleteDeliveryPalletByDnDefered(CurrentSession.UnitOfWork, dn);
                currentDNRepository.DeleteDeliveryByDnDefered(CurrentSession.UnitOfWork, dn);
                //currentDNRepository.DeleteDnDefered(CurrentSession.UnitOfWork, dn);   //Includes above three calls.
                currentDNRepository.DeletePalletAttrLogDefered(CurrentSession.UnitOfWork, pnList);
                currentPltRepository.DeletePalletAttrsDefered(CurrentSession.UnitOfWork, pnList);
                currentPltRepository.DeletePalletsDefered(CurrentSession.UnitOfWork, pnList);
            }
            else if (ship != null && ship != "")
            {
                IList<string> pnList = currentDNRepository.GetPalletNoListByShipmentAndWithSoloShipment(ship);
                currentDNRepository.DeleteDeliveryAttrLogByShipmentNoDefered(CurrentSession.UnitOfWork, ship);
                currentDNRepository.DeleteDeliveryAttrsByShipmentNoDefered(CurrentSession.UnitOfWork, ship);
                currentDNRepository.DeleteDeliveryInfoByShipmentNoDefered(CurrentSession.UnitOfWork, ship);
                currentDNRepository.DeleteDeliveryPalletByShipmentNoDefered(CurrentSession.UnitOfWork, ship);
                currentDNRepository.DeleteDeliveryByShipmentNoDefered(CurrentSession.UnitOfWork, ship);
                currentDNRepository.DeletePalletAttrLogDefered(CurrentSession.UnitOfWork, pnList);
                currentPltRepository.DeletePalletAttrsDefered(CurrentSession.UnitOfWork, pnList);
                currentPltRepository.DeletePalletsDefered(CurrentSession.UnitOfWork, pnList);
            }
            
            return base.DoExecute(executionContext);
        }
	}
}
