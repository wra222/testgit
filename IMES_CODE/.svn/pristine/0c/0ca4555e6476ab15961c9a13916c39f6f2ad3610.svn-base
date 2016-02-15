// INVENTEC corporation (c)2011 all rights reserved. 
// Description:记录DeliveryLog
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-17   Yuan XiaoWei                 create
// 2012-02-29   Yuan XiaoWei                 ITC-1360-0835
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
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的DeliveryNo,获取Delivery对象，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Unpack
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据DeliveryNo，记录DeliveryLog
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.DeliveryNo
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
    ///              Delivery
    /// </para> 
    /// </remarks>
	public partial class WriteDeliveryLog: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public WriteDeliveryLog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 根据DeliveryNo记录DeliveryLog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext){
        
            Delivery thisDelivery = (Delivery)CurrentSession.GetValue(Session.SessionKeys.Delivery);
            string deliveryNo = thisDelivery.DeliveryNo;
            IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();

            DeliveryLog thisLog = new DeliveryLog(0, deliveryNo, thisDelivery.Status, Station, Line, Editor, DateTime.Now);

            thisDelivery.AddLog(thisLog);
            currentRepository.Update(thisDelivery, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
