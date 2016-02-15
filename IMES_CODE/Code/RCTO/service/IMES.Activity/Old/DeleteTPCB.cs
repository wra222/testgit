// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 删除TPCB信息
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-15   Chen Xu (eB1-4)              create
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
    ///  删除TPCB信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///          100 TPCBCollection
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          从Session获得Family，Pdline，PartNo，在数据库TPCB中查找并删除该信息
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
	///		    2.业务异常：   
    ///         
    ///         
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Family
	///			Session.Pdline
	///			Session.PartNo
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
    ///         Update TPCB
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         ITPCBInfoRepository
    ///         TPCB_Info
    /// </para> 
    /// </remarks>

    public partial class DeleteTPCB : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeleteTPCB()
		{
			InitializeComponent();
		}
        /// <summary>
        /// 删除TPCB信息
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var CurrentFamily = (string)CurrentSession.GetValue(Session.SessionKeys.FamilyName);
            var CurrentPdLine = (string)CurrentSession.GetValue(Session.SessionKeys.LineCode);
            var CurrentPartNo = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);

            ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();
            TPCBInfoRepository.DeleteTPCB(CurrentFamily, CurrentPdLine, CurrentPartNo);
            return base.DoExecute(executionContext);
        }
	}
}
