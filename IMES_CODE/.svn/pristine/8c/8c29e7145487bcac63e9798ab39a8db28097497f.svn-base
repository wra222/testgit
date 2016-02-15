// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Update PAK_BTLocMas CMBQty
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
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// Update PAK_BTLocMas CMBQty
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
    ///         1.Update PAK_BTLocMas set CmbQty = CmbQty + 1 where (SnoId = @loc) 
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
    ///         Session.WHLocationObj
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
    public partial class UpdateCMBQty : BaseActivity
    {

        /// <summary>
        /// constructor
        /// </summary>
        public UpdateCMBQty()
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
            //从Session里取得PakBtLocMasInfo对象
            PakBtLocMasInfo CurrentLocation = (PakBtLocMasInfo)CurrentSession.GetValue(Session.SessionKeys.WHLocationObj);
            //从Session里取得Product对象
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            var currentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            // The following Repository method can not "Update PAK_BTLocMas set CmbQty = CmbQty + 1 where (SnoId = @loc)"
            // After the process, it keep old value.
            currentRepository.UpdateForIncPakBtLocMasDefered(CurrentSession.UnitOfWork, CurrentLocation.snoId, Editor);

            return base.DoExecute(executionContext);
        }
    }
}
