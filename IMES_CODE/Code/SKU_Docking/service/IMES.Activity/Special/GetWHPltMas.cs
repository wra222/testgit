// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据输入的Pallet,获取WhPltMasInfo对象，并放到Session中
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-30   Yuan XiaoWei                 create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的Pallet,获取WHPLTMas对象，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC WH Pallet Control
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Pallet，调用IPalletRepository的GetWHPltMas方法，获取WhPltMasInfo对象，添加到Session中
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PalletNo
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.Pallet
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
	public partial class GetWHPltMas: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetWHPltMas()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Get WHPltMas Object and put it into Session.SessionKeys.WHPltMas
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Pallet CurrentPallet = (Pallet)CurrentSession.GetValue(Session.SessionKeys.Pallet);
            IPalletRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            WhPltMasInfo resultWhPltMas =  CurrentRepository.GetWHPltMas(CurrentPallet.PalletNo);

            CurrentSession.AddValue(Session.SessionKeys.WHPltMas, resultWhPltMas);
            return base.DoExecute(executionContext);
        }
	}
}
