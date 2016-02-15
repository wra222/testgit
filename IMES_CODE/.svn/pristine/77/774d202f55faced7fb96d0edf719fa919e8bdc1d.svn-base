// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据输入的Pallet and WC,Write whpltlog
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
    /// 根据输入的Pallet and WC,Write whpltlog
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-PAK-UC WH Pallet Control
    ///      CI-MES12-SPEC-PAK-UC DT Control
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据Pallet and WC,Write whpltlog
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
	public partial class WriteWHPltLog: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public WriteWHPltLog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Write whpltlog
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (string.IsNullOrEmpty(PltFromSessionKey))
            {
                PltFromSessionKey = Session.SessionKeys.PalletNo;
            }
            string CurrentPalletNo = CurrentSession.GetValue(PltFromSessionKey) as string;
            
            WhPltLogInfo newLog =  new WhPltLogInfo();
            newLog.plt = CurrentPalletNo;
            newLog.editor = Editor;
            newLog.wc = Station;
            newLog.cdt = DateTime.Now;

            IPalletRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            CurrentRepository.InsertWhPltLogDefered(CurrentSession.UnitOfWork,newLog);
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// PltFromSessionKey Of WHPLTLog,can be PalletNo or DummyPalletNo
        /// </summary>
        public static DependencyProperty PltFromSessionKeyProperty = DependencyProperty.Register("PltFromSessionKey", typeof(string), typeof(WriteWHPltLog));

        /// <summary>
        /// PltFromSessionKey Of WHPLTLog,can be PalletNo or DummyPalletNo
        /// </summary>
        [Description("PltFromSessionKeyProperty")]
        [CategoryAttribute("InArguments of WriteWHPltLog")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string PltFromSessionKey
        {
            get
            {
                return ((string)(base.GetValue(PltFromSessionKeyProperty)));
            }
            set
            {
                base.SetValue(PltFromSessionKeyProperty, value);
            }
        }
	}
}
