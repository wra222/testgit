// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 保存 TPCB Det 信息
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-16   Chen Xu (eB1-4)              create
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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.TPCB;
using IMES.DataModel;


namespace IMES.Activity
{
	
    /// <summary>
    /// 保存 TPCB Det 信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         100 TPCBCollection
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          收集TPCB Code信息
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPCB
    ///			Session.Family,
	///			Session.Pdline.
	///			Session.Editor
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///        无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         Update TPCB Det
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         ITPCBInfoRepository
    ///         TPCB_Info
    /// </para> 
    /// </remarks>
    
    public partial class SaveTPCBDet: BaseActivity
	{
		public SaveTPCBDet()
		{
			InitializeComponent();
		}
	
        /// <summary>
        /// 保存TPCB Det信息
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
     
            ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();

            var currentCode= (string)CurrentSession.GetValue(Session.SessionKeys.TPCB);
            var currentFamily = (string)CurrentSession.GetValue(Session.SessionKeys.FamilyName);
            var currentPdLine = (string)CurrentSession.GetValue(Session.SessionKeys.LineCode);
            var currentEditor = (string)CurrentSession.GetValue(Session.SessionKeys.Editor);
            TPCBInfoRepository.SaveTPCBDet(currentCode, currentFamily, currentPdLine, currentEditor);
            return base.DoExecute(executionContext);
        }
    }
}