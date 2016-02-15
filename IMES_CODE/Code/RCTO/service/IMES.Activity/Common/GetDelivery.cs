// INVENTEC corporation (c)2009 all rights reserved. 
// Description:  根据输入的DeliveryNo,获取Delivery对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-02   Yuan XiaoWei                 create
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
    ///      045
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据DeliveryNo，调用IDeliveryRepository的Find方法，获取Delivery对象，添加到Session中
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
    ///         Session.Delivery
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
	public partial class GetDelivery: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public GetDelivery()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 根据DeliveryNo，调用IDeliveryRepository的Find方法，获取Delivery对象，添加到Session中
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string deliveryNo = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            Delivery CurrentDelivery = DeliveryRepository.Find(deliveryNo);
            if (CurrentDelivery==null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(deliveryNo);
                throw new FisException("CHK107", errpara);
            }
            CurrentSession.AddValue(Session.SessionKeys.Delivery, CurrentDelivery);
            return base.DoExecute(executionContext);
        }
	}
}
