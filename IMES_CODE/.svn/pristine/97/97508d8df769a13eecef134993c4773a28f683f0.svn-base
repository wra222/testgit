// INVENTEC corporation (c)2012 all rights reserved. 
// Description: PCA OQC Output
// UI:CI-MES12-SPEC-SA-UI PCA OQC Output.docx 
// UC:CI-MES12-SPEC-SA-UC PCA OQC Output.docx  
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-16   ChenXu                       create
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    /// <summary>
    /// 检查MB上是否有未Pass的记录，检查MB是否存在未修复的记录
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于需要站 PCA OQC Output
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：1)  若MB上站未Pass，则报告错误：“该机器存在不良，不能刷此站”
    ///                     2)  若MB存在未修复的记录，则报告错误：“该机器在修复中”
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.MB 
    ///         
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
    ///        Product
    /// </para> 
    /// </remarks>
    public partial class CheckMBPassAndRepair : BaseActivity
	{
        /// <summary>
        /// CheckMBPass And Repair
        /// </summary>
        public CheckMBPassAndRepair()
		{
			InitializeComponent();
		}

        /// <summary>
        /// CheckMBPass And Repair
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            MBStatusEnum Status = currentMB.MBStatus.Status;

            //若MB上站未Pass，则报告错误：“该机器存在不良，不能刷此站”
            //select Status from PCBStatus nolock where PCBNo = MBSno,若Status为’0’，则报错。

            if (Status == MBStatusEnum.Fail)
            {
                FisException fe = new FisException("PAK082", new string[] { }); 
                throw fe;
            }
            else
            {
                CurrentSession.AddValue(ExtendSession.SessionKeys.DefectStation, this.Station);
            }

            //若MB存在未修复的记录，则报告错误：“该机器在修复中”
            //select * from PCBRepair nolock where  Status = '0' and PCBNo = MBSno

            string mbSNo = this.Key;
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            var repairs = currentMB.Repairs;
            var repair = (from r in repairs
                          where r.Status == 0
                          select r.ID.ToString()).ToArray();
            if (repair.Count() > 0)
            {
                FisException fe = new FisException("PAK083", new string[] { });
                throw fe;
            }
            return base.DoExecute(executionContext);
        }
	
	}
}
