// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 保存VCode信息
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-24   Chen Xu (eB1-4)              create
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
    /// 保存VCode信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///        097 ITPCBTPLabel
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         若VCodeInfo已存在，更新 VCode, Editor, Cdt；
	///			若VCodeInfo不存在，插入 VCodeInfo（全部信息）
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.VCodeInfo （全部信息）
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
    ///          Save VCodeInfo,  Update  GetData..TPCB_Maintain 
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         ITPCBRepository
    ///         TPCB
    /// </para> 
    /// </remarks>
	
    public partial class SaveVCode:BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
		public SaveVCode()
		{
			InitializeComponent();
		}

        /// <summary>
        ///  保存VCode信息
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns>CurrentVCodeInfo</returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentTPCB = (string)CurrentSession.GetValue(Session.SessionKeys.TPCB);
            var currentTP = (string)CurrentSession.GetValue(Session.SessionKeys.TP);
            var currentVCode = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
            var currentEditor = (string)CurrentSession.GetValue(Session.SessionKeys.Editor);
            ITPCBRepository TPCBRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBRepository, TPCB>();
            VCodeInfo CurrentVCodeInfo = new VCodeInfo();
            CurrentVCodeInfo.tpcb = currentTPCB;
            CurrentVCodeInfo.tp = currentTP;
            CurrentVCodeInfo.vcode = currentVCode;
            CurrentVCodeInfo.Editor = currentEditor;
            TPCBRepository.SaveVCodeInfo(CurrentVCodeInfo);
            CurrentSession.AddValue(Session.SessionKeys.IsComplete, true);
            return base.DoExecute(executionContext);
        }
	}
}
