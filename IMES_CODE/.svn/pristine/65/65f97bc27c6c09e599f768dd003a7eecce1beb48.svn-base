// INVENTEC corporation (c)2009 all rights reserved. 
// Description: CI-MES12-SPEC-FA-UC PCA Repair Test&Output.docx
//              Check MB Pass    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-10   zhu lei                      create
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

namespace IMES.Activity
{
    /// <summary>
    /// MBSNo是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于需要站
    /// </para>
    /// <para>
    /// 实现逻辑：
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
    public partial class CheckMBPass : BaseActivity
	{
        /// <summary>
        /// CheckMBPass
        /// </summary>
        public CheckMBPass()
		{
			InitializeComponent();
		}

        /// <summary>
        /// CheckMBPass
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string mbSNo = this.Key;
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            var repairs = currentMB.Repairs;
            var repair = (from r in repairs
                          where r.Status == 0
                          select r.ID.ToString()).ToArray();
            if (repair.Count() > 0)
            {
                List<string> errpara = new List<string>();
                errpara.Add(mbSNo);
                throw new FisException("CHK212", errpara);
            }
            
            return base.DoExecute(executionContext);
        }
	
	}
}
