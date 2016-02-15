// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  
// UI:CI-MES12-SPEC-SA-UI BGA Output.docx
// UC:CI-MES12-SPEC-SA-UC BGA Output.docx                       
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-1-10   Du Xuan (itc98066)          create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
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
    /// 添加MB的repair记录
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于以MB为操作主线的workflow
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         存入记录
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MB
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class AddRepairItem : BaseActivity
    {
               
        /// <summary>
        /// Constructor
        /// </summary>
        public AddRepairItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get MB Object and Put it into Session.SessionKeys.MB
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            IMB curMb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string reworkStation = (string)CurrentSession.GetValue(Session.SessionKeys.Remark);

            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            MBRptRepair newMb = new MBRptRepair();
            newMb.MBSn = curMb.Sn;
            newMb.Tp = "BGA";
            newMb.Status = "1";
            newMb.Mark = "0";
            newMb.Remark = reworkStation;
            newMb.Cdt = DateTime.Now;
            newMb.Udt = DateTime.Now;
            newMb.UserName = this.Editor;

            curMb.AddRptRepair(newMb);
            mbRepository.Update(curMb, CurrentSession.UnitOfWork);

            CurrentSession.AddValue(Session.SessionKeys.NewMB, newMb);

            return base.DoExecute(executionContext);
        }

    }
}
