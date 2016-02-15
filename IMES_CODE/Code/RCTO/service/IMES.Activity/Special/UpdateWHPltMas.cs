// INVENTEC corporation (c)2011 all rights reserved. 
// Description: UPDATE WH_PLTMas set WC=@WC,Editor=@user,Udt=getdate() where PLT=@plt
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-30   Yuan XiaoWei                 create
// Known issues:

using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

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
    ///      CI-MES12-SPEC-PAK-UC WH Pallet Control
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.UPDATE WH_PLTMas set WC=@WC,Editor=@user,Udt=getdate() where PLT=@plt
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
	public partial class UpdateWHPltMas: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateWHPltMas()
		{
			InitializeComponent();
		}

        /// <summary>
        /// UPDATE WH_PLTMas set WC=@WC,Editor=@user,Udt=getdate() where PLT=@plt
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string CurrentPalletNo = (string)CurrentSession.GetValue(Session.SessionKeys.PalletNo);

            WhPltMasInfo newMasInfo = new WhPltMasInfo();
            newMasInfo.plt = CurrentPalletNo;
            newMasInfo.editor = Editor;
            newMasInfo.wc = WC;
            newMasInfo.udt = DateTime.Now;

            IPalletRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            CurrentRepository.UpdateWhPltMasDefered(CurrentSession.UnitOfWork,newMasInfo,CurrentPalletNo);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// WC Of WHPLTMas,can be IN or RW
        /// </summary>
        public static DependencyProperty WCProperty = DependencyProperty.Register("WC", typeof(string), typeof(UpdateWHPltMas));

        /// <summary>
        /// WC Of WHPLTMas,can be IN or RW
        /// </summary>
        [Description("WCProperty")]
        [CategoryAttribute("InArguments of UpdateWHPltMas")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string WC
        {
            get
            {
                return ((string)(base.GetValue(WCProperty)));
            }
            set
            {
                base.SetValue(WCProperty, value);
            }
        }
	}
}
