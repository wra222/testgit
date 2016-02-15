// INVENTEC corporation (c)2009 all rights reserved. 
// Description: MB SNo 是否已经切割过
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-10   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
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
    /// 将Mac绑定至MB
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于004 PrintComMB
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据输入的MB SNO判断其状态是否为CL
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK007
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    ///         IMB
    /// </para> 
    /// </remarks>
	public partial class CheckPCBStatusStation: BaseActivity
	{
        public CheckPCBStatusStation()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 如果MB状态为CL
        /// 抛出异常CHK007      
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            if (currentMB.MBStatus.Station == "CL") {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentMB.Sn);
                ex = new FisException("CHK007", erpara);
                throw ex;
            }
            
            return base.DoExecute(executionContext);
        }
	}
}
