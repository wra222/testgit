// INVENTEC corporation (c)2011 all rights reserved. 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-21   210003                       create
// Known issues:

using System.Workflow.ComponentModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 判读是否扫描2D Barcode。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         
    ///         CI-MES12-SPEC-PAK-UC Pallet Weight.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// 
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
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
    ///         Session.IsScan2DBarCode
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class IsScan2DBarCode : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public IsScan2DBarCode()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 2D Barcode 检查
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string PalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            bool isScan =  palletRepository.IsScan2DBarCode(PalletNo);
            CurrentSession.AddValue(Session.SessionKeys.Consolidated, isScan);
            return base.DoExecute(executionContext);
        }
	}
}
