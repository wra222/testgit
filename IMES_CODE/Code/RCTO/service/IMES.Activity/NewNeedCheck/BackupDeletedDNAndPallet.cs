// INVENTEC corporation (c)2010 all rights reserved. 
// Description: PO Data模块的删除DN功能中的保存被删除的DN、Delivery_Pallet记录的子功能
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

namespace IMES.Activity
{
    /// <summary>
    /// PO Data模块的删除DN功能中的保存被删除的DN、Delivery_Pallet记录的子功能
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
    ///         被删的DN和Delivery_Pallet记录保存入DeletedDelivery、DeletedDeliveryPallet
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
    /// </para> 
    /// </remarks>
    public partial class BackupDeletedDNAndPallet : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BackupDeletedDNAndPallet()
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
            
            if (dn != null && dn != "")
            {
                currentDNRepository.BackupToDeliveryDefered(CurrentSession.UnitOfWork, dn);
                currentDNRepository.BackupToDeliveryPalletDefered(CurrentSession.UnitOfWork, dn);
            }
            else if (ship != null && ship != "")
            {
                currentDNRepository.BackupToDeliveryByShipmentNoDefered(CurrentSession.UnitOfWork, ship);
                currentDNRepository.BackupToDeliveryPalletByShipmentNoDefered(CurrentSession.UnitOfWork, ship);
            }

            return base.DoExecute(executionContext);
        }
    }
}
