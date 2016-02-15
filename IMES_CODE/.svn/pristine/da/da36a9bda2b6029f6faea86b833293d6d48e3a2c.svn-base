// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Close当前页面的Location
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-01   Kerwin                       create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pallet;

namespace IMES.Activity
{
    /// <summary>
    /// Close当前页面的Location
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Assign WH Location for BT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.UPDATE PAK_BTLocMas SET [Status] = 'Close' WHERE SnoId = @Location
    ///
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///                     
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.WHLocation
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
    ///         Pak_Btlocmas
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Product
    /// </para> 
    /// </remarks>
    public partial class CloseLocation : BaseActivity
    {

        /// <summary>
        /// constructor
        /// </summary>
        public CloseLocation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close当前页面的Location
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //从Session里取得Product对象
            string CurrentLocation = (string)CurrentSession.GetValue(Session.SessionKeys.WHLocation);

            var currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            //modify ITC-1360-1763 bug
            currentRepository.UpdateStatusForPakBtLocMasDefered(CurrentSession.UnitOfWork, "Close", CurrentLocation, this.Editor);
            return base.DoExecute(executionContext);
        }
    }
}
