/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: UpdateMBForDCode
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-17   zhulei            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

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
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.Repository.PCA;
using IMES.FisObject.PCA.MBChangeLog;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// UpdateMBForDCode
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-FA-UC PCA Shipping Label Print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.MBNOList中每个MBNO
    ///             1.创建MB对象
    ///             2.保存MB对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ModelName
    ///         Session.SMTMONO
    ///         Session.DateCode
    ///         Session.MBNOList
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
    ///         insert PCB
    ///         insert PCBStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class UpdateMBForDCode : BaseActivity
    {
        /// <summary>
        /// UpdateMBForDCode
        /// </summary>
        public UpdateMBForDCode()
        {
            InitializeComponent();
        }

        /// <summary>
        /// UpdateMBForDCode
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var CurrentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            //保存DCode
            string dCode = CurrentSession.GetValue(Session.SessionKeys.DCode) as string;
            CurrentMB.DateCode = dCode;

            mbRepository.Update(CurrentMB, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
    }
}
