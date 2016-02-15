// INVENTEC corporation (c)2011 all rights reserved. 
// Description: Assign WH Location
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-19   210003                       create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.Infrastructure;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 当天已经完成称重的Pallet 数量
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Pallet Weight.docx
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///       Pallet 数量
    ///    ISNULL（IMES_PAK..Pallet.Weight, ''） ！＝ '' 表示该Pallet 已经完成称重
    ///    IMES_PAK..Pallet.Udt>= CONVERT(char(10),GETDATE(),111) 表示当天完成
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.UCC
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.PalletNo
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         Pallet
    /// </para> 
    /// </remarks>
    public partial class GetPalletNoByUCC : BaseActivity
	{
        /// <summary>
        /// 用UCC ID查询对应的Pallet No
        /// </summary>
		public GetPalletNoByUCC()
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
            //从Session里取得UCC
            string ucc = (string)CurrentSession.GetValue(Session.SessionKeys.UCC);

            IPalletRepository palletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            string pallet_no = palletRepository.GetPalletNoByUcc(ucc);
            if (string.IsNullOrEmpty(pallet_no))
            {
                FisException ex;
                ex = new FisException("PAK023", new string[] { });
                throw ex;
            }
            CurrentSession.AddValue(Session.SessionKeys.PalletNo, pallet_no);
            return base.DoExecute(executionContext);
        }

	}
}
