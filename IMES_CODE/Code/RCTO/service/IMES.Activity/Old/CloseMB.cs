// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 更新MB状态为Close
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-10   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 更新MB状态为Close
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         切分连板的Station
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.更新MB对象状态为Close;
    ///         2.保存MB对象;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
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
    ///         update PCBStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    ///         
    /// </para> 
    /// </remarks>
    public partial class CloseMB : BaseActivity
    {
        public CloseMB()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行修改MBStatus的Station，Status，Editor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            MBStatus newMBStatus = new MBStatus(currentMB.Sn, "CL", MBStatusEnum.CL, this.Editor, this.Line, DateTime.Now, DateTime.Now);
            currentMB.MBStatus = newMBStatus;
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            currentMBRepository.Update(currentMB, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
    }
}
