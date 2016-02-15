// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-03-30   Tong.Zhi-Yong                create
// Known issues:
using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         this.Key
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class UpdateFwdPltStatus : BaseActivity
	{
		///<summary>
		///</summary>
        public UpdateFwdPltStatus()
		{
			InitializeComponent();
		}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            var palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            var pltNo = CurrentSession.GetValue(Session.SessionKeys.PalletNo).ToString();

            palletRepository.UpdateFwdPltStatusDefered(CurrentSession.UnitOfWork, Key, pltNo, "Out", Editor);

            return base.DoExecute(executionContext);
        }
	}
}
