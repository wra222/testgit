// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Check MAC 
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-09   207006                       ITC-1103-0050
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
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 检查MB是否有重复的MAC
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
    ///        MB
    /// </para> 
    /// </remarks>
    public partial class CheckMac : BaseActivity
	{
		public CheckMac()
		{
			InitializeComponent();
		}

        //ITC-1103-0050
        /// <summary>
        /// MB是否有重复的MAC
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
          
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            //是否存在重复的MAC，需要报告错误：“Duplicate MAC Address!”
            if (currentMBRepository.GetMBListByMAC (currentMB.MAC).Count  > 1)
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentMB.MAC);
                throw new FisException("CHK032", errpara);
            }
            return base.DoExecute(executionContext);
        }
	
	}
}
