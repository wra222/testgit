// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 更新Delivery的ShipDate
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-01   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{


    /// <summary>
    /// 更新Delivery的ShipDate
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UPDATE_SHIPDATE
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Delivery对象
    ///         2.更新ShipDate
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Delivery
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
    ///         更新Deliveryt的ShipDate
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IDeliveryRepository
    ///              Delivery
    /// </para> 
    /// </remarks>
    public partial class UpdateDeliveryShipDate : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateDeliveryShipDate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行修改Delivery的ShipDate，Editor，Udt的操作
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            Delivery CurrentDelivery = (Delivery) CurrentSession.GetValue(Session.SessionKeys.Delivery);

            DateTime shipDate = (DateTime) CurrentSession.GetValue(Session.SessionKeys.ShipDate);

            CurrentDelivery.ShipDate = shipDate;

            CurrentDelivery.Editor = this.Editor;
            CurrentDelivery.Udt = DateTime.Now;

            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            DeliveryRepository.Update(CurrentDelivery, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
    }
}
