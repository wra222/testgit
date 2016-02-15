// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 更新ProductStatus
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PAK.Pallet;

/*
 * Must Change Later: 
 * 1. returnStation will must be modified later, and at the same time, 
 *    the same parameter in the service must be changed too.
 * 2. the following sentence must be enable:
 *    // palletRepository.UpdateStatusForPakBtLocMas(current_status, current_Location);
 *    in this file. for now this interface has not been usable.
 */

namespace IMES.Activity
{
    /// <summary>
    /// 用于更新Product的状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Product为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取Product，调用Product的UpdateStatus方法
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         session.Sessionkey.returnStation --> must modified later
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
    ///         更新
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPalletRepository
    /// </para> 
    /// </remarks>
    public partial class UpdateBtLocStatus : BaseActivity
    {
        /// <summary>
        /// UpdateBtLocStatus
        /// </summary>
        public UpdateBtLocStatus()
        {
            InitializeComponent();
        }
        /// <summary>
        /// simple activity, we just get the Location information from the session, 
        /// and pass through it and a static state "Close" into the palletRepository.
        /// UpdateStatusForPakBtLocMas, and then call parent interface. :)
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var current_status = default(string);
            var current_Location = default(string);
            current_status = "Close";
            current_Location = CurrentSession.GetValue(Session.SessionKeys.ReturnStation).ToString();
            IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            palletRepository.UpdateStatusForPakBtLocMas(current_status, current_Location, this.Editor); //(string status, string snoId);

            return base.DoExecute(executionContext);
        }
    }
}
