// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 保存PalletWeight的称重结果，更新Pallet的Weight
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-03   Yuan XiaoWei                 create
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
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{


    /// <summary>
    /// 保存PalletWeight的称重结果，更新Pallet的Weight
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PalletWeight
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从session中获取ActuralWeight
    ///         2.从session中获取Pallet
    ///         3.将ActuralWeight赋予Pallet的Weight属性
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ActuralWeight
    ///         Session.Pallet
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
    ///         Pallet的Weight属性
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///            Pallet
    ///            IPalletRepository
    /// </para> 
    /// </remarks>
    public partial class UpdatePalletWeight : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public UpdatePalletWeight()
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
            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);
            CurrentPallet.Weight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
            IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            PalletRepository.Update(CurrentPallet, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
