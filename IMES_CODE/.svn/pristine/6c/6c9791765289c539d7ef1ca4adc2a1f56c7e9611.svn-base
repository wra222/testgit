/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: for set child mb sno for muti mb
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 2009-01-08   207013     Modify: ITC-1103-0074 、ITC-1103-0011
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.LCM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.BOM;

namespace IMES.Activity
{
    /// <summary>
    ///  保存LCM与TPDL的绑定
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// Insert LCMBind: LCMSno=LCM CT#, MESno=TPDL SN#, METype=’TPDL’
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPDLLCMCTNO
    ///         Session.TPDLSN
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
    /// LCMBind
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// ILCMRepository
    /// </para> 
    /// </remarks>
    public partial class SaveTPDL : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SaveTPDL()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存LCM与TPDL的绑定
        /// Insert LCMBind: LCMSno=LCM CT#, MESno=TPDL SN#, METype=’TPDL’
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string ctno = CurrentSession.GetValue(Session.SessionKeys.TPDLLCMCTNO).ToString();
            string tpdlsn = CurrentSession.GetValue(Session.SessionKeys.TPDLSN).ToString();
            ILCMRepository lcmRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository, LCM>();
            LCMME lcmmeobj = new LCMME();
            lcmmeobj.LCMSn = ctno;
            lcmmeobj.MESn = tpdlsn;
            lcmmeobj.METype = "TPDL";
            lcmmeobj.Editor = this.Editor;
            lcmRepository.InsertLCMBindDefered(CurrentSession.UnitOfWork, lcmmeobj);

            CurrentSession.AddValue(Session.SessionKeys.IsComplete, true);
            return base.DoExecute(executionContext);
        }
    }
}
