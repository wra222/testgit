// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 查询TPCB_Maintain数据库中当前所有绑定情况
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
    public partial class QueryVCode : BaseActivity
	{
        /// <summary>
        /// 查询TPCB_Maintain数据库中当前所有绑定情况
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
        ///         查询TPCB_Maintain数据库中当前所有绑定情况
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
        ///         Session.VCodeInfoLst
        /// </para> 
        ///<para> 
        /// 数据更新:
        ///         无
        /// </para> 
        ///<para> 
        /// 相关FisObject:
        ///         ITPCBRepository
        ///         TPCB
        /// </para> 
        /// </remarks>
        
        public QueryVCode()
		{
			InitializeComponent();
		}
        
        /// <summary>
        /// 查询TPCB_Maintain数据库中当前所有绑定情况
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            ITPCBRepository TPCBRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBRepository, TPCB>();
            IList<VCodeInfo> currentVCodeInfoLst = TPCBRepository.QueryAll();
            CurrentSession.AddValue(Session.SessionKeys.VCodeInfoLst,currentVCodeInfoLst);
            return base.DoExecute(executionContext);
        }
	}



}
