// INVENTEC corporation (c)2010 all rights reserved. 
// Description: Check TPCB Det 信息
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
    ///检查当前TPCB Det保存情况
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
    ///         检查当前TPCB Det是否已保存刷入的TPCB信息
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///								CHK115: 已经存在相同的TPCB相关信息！！
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPCB
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         ITPCBInfoRepository
    ///         TPCB_Info
    /// </para> 
    /// </remarks>
    
    public partial class CheckTPCBDet: BaseActivity
	{
        public CheckTPCBDet()
		{
			InitializeComponent();
		}
	

		 /// <summary>
        /// 检查当前TPCB Det保存情况
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();

            var currentCode= (string)CurrentSession.GetValue(Session.SessionKeys.TPCB);
            var currentEditor = (string)CurrentSession.GetValue(Session.SessionKeys.Editor);
           
            IList<TPCBDet> checkResult= TPCBInfoRepository.CheckTPCBDet(currentCode, currentEditor);

            if (checkResult.Count > 1)
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentCode);
                throw new FisException("CHK115", errpara);
            }


            return base.DoExecute(executionContext);
        }
    }
}