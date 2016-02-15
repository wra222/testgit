// INVENTEC corporation (c)2012 all rights reserved. 
// Description: UPDATE PAK_LocMas, Condition: PAK_LocMas.Pno = @PalletNo and Tp = 'PakLoc'
// UC: UC-MES12-SPEC-PAK-UC Pallet Verify  
// UI: UI-MES12-SPEC-PAK-UC Pallet Verify                      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-04-24   Chen Xu                      create
// Known issues:

using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// UPDATE WH_PLTMas set WC=@WC,Editor=@user,Udt=getdate() where PLT=@plt
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      UC-MES12-SPEC-PAK-UC Pallet Verify
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///       	Update PAK_LocMas
    ///         Condition：
    ///         PAK_LocMas.Pno = @PalletNo and Tp = 'PakLoc'
    ///         更新如下字段：
    /// 	        PAK_LocMas.Pno = ''
    ///	            Editor
    ///	            Udt
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Pallet
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
    ///              IPalletRepository
    ///              Pallet
    /// </para> 
    /// </remarks>
	public partial class UpdatePakLocMas: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdatePakLocMas()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Update PAK_LocMas
        ///         Condition：
        ///         PAK_LocMas.Pno = @PalletNo and Tp = 'PakLoc'
        ///         更新如下字段：
        /// 	        PAK_LocMas.Pno = ''
        ///	            Editor
        ///	            Udt
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);

            IPalletRepository iPallletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();

            PakLocMasInfo cond = new PakLocMasInfo();
            cond.pno = CurrentPalletNo;
            cond.tp = "PakLoc";

            PakLocMasInfo setVal = new PakLocMasInfo();
            setVal.pno = "";
            setVal.editor = Editor;
            setVal.udt = DateTime.Now;

            iPallletRepository.UpdatePakLocMasInfoDefered(CurrentSession.UnitOfWork, setVal, cond);
            
            return base.DoExecute(executionContext);
        }

        
	}
}
