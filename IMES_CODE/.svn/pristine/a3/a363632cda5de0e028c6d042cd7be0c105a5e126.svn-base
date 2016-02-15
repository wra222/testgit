// INVENTEC corporation (c)2009 all rights reserved. 
// Description: CI-MES12-SPEC-FA-UC PCA Repair Test&Output.docx
//              Update MTA_Mark  
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
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// UpdateMTAMark
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
    public partial class UpdateMTAMark : BaseActivity
	{
        /// <summary>
        /// UpdateMTAMark
        /// </summary>
        public UpdateMTAMark()
		{
			InitializeComponent();
		}

        /// <summary>
        /// UpdateMTAMark
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            string mbSNo = this.Key;
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IList<int> repIdLst = (IList<int>)currentMBRepository.GetPcbRepairDefectInfoIdListWithDefect(mbSNo);
            foreach (var r in repIdLst)
            {
                MtaMarkInfo setValue = new MtaMarkInfo();
                MtaMarkInfo condition = new MtaMarkInfo();
                setValue.mark = "1";
                condition.rep_Id = r;
                currentMBRepository.UpdateMtaMarkInfoDefered(CurrentSession.UnitOfWork, setValue, condition);
            }
            
            return base.DoExecute(executionContext);
        }
	
	}
}
