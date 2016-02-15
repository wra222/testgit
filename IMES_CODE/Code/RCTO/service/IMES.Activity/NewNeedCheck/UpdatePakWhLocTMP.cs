// INVENTEC corporation (c)2011 all rights reserved. 
// Description: UPDATE PAK_WHLoc_TMP SET PLT='',Tp='',BOL='' WHERE PLT=RTRIM(@PalletNo)
//              UPDATE PAK_WHLoc_TMP SET BOL='' WHERE BOL=RTRIM(@bol) and @bol!='' and PLT=''
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-20   ZhangKaiSheng                 create
// Known issues:

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// UPDATE PAK_WHLoc_TMP SET PLT='',Tp='',BOL='' WHERE PLT=RTRIM(@PalletNo)
    /// UUPDATE PAK_WHLoc_TMP SET BOL='' WHERE BOL=RTRIM(@bol) and @bol!='' and PLT=''
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC Final Scan
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.UPDATE PAK_WHLoc_TMP SET PLT='',Tp='',BOL='' WHERE PLT=RTRIM(@PalletNo)
    ///		    2.UUPDATE PAK_WHLoc_TMP SET BOL='' WHERE BOL=RTRIM(@bol) and @bol!='' and PLT=''
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
    ///		     IPalletRepository       
    ///              Pallet
    /// </para> 
    /// </remarks>
    public partial class UpdatePakWhLocTMP: BaseActivity
    {
        /// <summary>
        /// constructor
        /// </summary>
        public UpdatePakWhLocTMP()
        {
            InitializeComponent();
        }

        /// <summary>
        ///         1.UPDATE PAK_WHLoc_TMP SET PLT='',Tp='',BOL='' WHERE PLT=RTRIM(@PalletNo)
        ///		    2.UPDATE PAK_WHLoc_TMP SET BOL='' WHERE BOL=RTRIM(@bol) and @bol!='' and PLT=''
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);
            IPalletRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            //SELECT @bol=RTRIM(BOL) FROM PAK_WH_LocMas WHERE PLT1=RTRIM(@PalletNo) or PLT2=RTRIM(@PalletNo)
            IList<string>  bolstringLst = CurrentRepository.GetBolFromPakWhLocMasByPlt1AndPlt2(CurrentPalletNo);
            string bolstring = bolstringLst[0].TrimEnd();
            //UPDATE PAK_WHLoc_TMP SET PLT='',Tp='',BOL='' WHERE PLT=RTRIM(@PalletNo)
            CurrentRepository.UpdatePakWhLocTmpForClearPltAndTpAndBolDefered(CurrentSession.UnitOfWork, CurrentPalletNo);
            //UPDATE PAK_WHLoc_TMP SET BOL='' WHERE BOL=RTRIM(@bol) and @bol!='' and PLT=''
            CurrentRepository.UpdatePakWhLocTmpForClearBolDefered(CurrentSession.UnitOfWork,bolstring);
            
            return base.DoExecute(executionContext);
        }

    }
}
