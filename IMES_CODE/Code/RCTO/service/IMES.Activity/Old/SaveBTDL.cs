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
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
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
    /// 记录KP/CT的绑定关系
    /// 保存LCM与BTDL的绑定
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PartSN
    ///         Session.VendorSN
    ///         Session.BTDLLCMCTNO
    ///         Session.BTDLSN
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
    public partial class SaveBTDL : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SaveBTDL()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Save BTDL	
        /// A.记录KP/CT的绑定关系
        /// Update PartSN.VendorSN
        /// PartSnRepository.update
        /// B.保存LCM与BTDL的绑定
        /// Insert LCMBind: LCMSno=LCM CT#, MESno=BTDL SN#, METype=’BTDL’ 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        { 
            IPartSnRepository currentPartSNRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
            PartSn currentPartSN = (PartSn)CurrentSession.GetValue(Session.SessionKeys.PartSN);
            string currentVendorSN = CurrentSession.GetValue(Session.SessionKeys.VendorSN).ToString();
            currentPartSN.VendorSn = currentVendorSN;
            currentPartSN.Editor = Editor;
            currentPartSNRepository.Update(currentPartSN, CurrentSession.UnitOfWork);

            string ctno = CurrentSession.GetValue(Session.SessionKeys.BTDLLCMCTNO).ToString();
            string btdlsn = CurrentSession.GetValue(Session.SessionKeys.BTDLSN).ToString();

            ILCMRepository lcmRepository = RepositoryFactory.GetInstance().GetRepository<ILCMRepository, LCM>();
            LCMME lcmmeobj=new LCMME();
            lcmmeobj.LCMSn =ctno;
            lcmmeobj.MESn = btdlsn;
            lcmmeobj.METype ="BTDL";
            lcmmeobj.Editor = this.Editor;
            lcmRepository.InsertLCMBindDefered(CurrentSession.UnitOfWork, lcmmeobj);
            CurrentSession.AddValue(Session.SessionKeys.IsComplete, true);
            return base.DoExecute(executionContext);
        }
    }
}
