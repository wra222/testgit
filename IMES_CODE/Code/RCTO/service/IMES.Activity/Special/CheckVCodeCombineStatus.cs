// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查VCode在数据库中绑定状态
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-25   Chen Xu (eB1-4)              create
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
    ///  检查VCode在数据库中绑定状态
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         097 ITPCBTPLabel
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         从Session中获得VCode, 调用ITPCBRepository，获得该VCode的绑定情况，
	///         若用户输入的[VCode]未绑定信息，可继续Save操作；
	///			若已经绑定了信息，但绑定的信息，就是当前Session里的信息，无操作，Session结束；
	///         若已经绑定了其他的[TPCB]/[Tp]，则报告错误“该VCode已经绑定了其他的[TPCB]/[Tp]！！
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
	///								CHK098：该VCode 被绑定多次，请检查数据库!!
    ///         2.业务异常：
	///								CHK096：该VCode已经绑定了其他的[TPCB]/[Tp]!!
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPCB
    ///         Session.TP
	///			Session.VCode
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///       无
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
    
    
    public partial class CheckVCodeCombineStatus:BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        
        public CheckVCodeCombineStatus()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 检查当前VCode绑定情况
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentTPCB = (string)CurrentSession.GetValue(Session.SessionKeys.TPCB);
            var currentTP = (string)CurrentSession.GetValue(Session.SessionKeys.TP);
            var currentVCode = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
      
            ITPCBRepository TPCBRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBRepository, TPCB>();
            IList<VCodeInfo> VCodeCombineInfoLst = TPCBRepository.GetVCodeCombineInfo(currentVCode);


            // 当前VCode未绑定,可执行Save操作
            if (VCodeCombineInfoLst == null || VCodeCombineInfoLst.Count == 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.IsComplete, false);
            }

            // 当前VCode绑定多次，数据库异常，请检查
            else if (VCodeCombineInfoLst.Count > 1)
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentVCode);
                throw new FisException("CHK098", errpara);
            }


            // 当前VCode绑定一次，检查绑定信息是否与当前输入信息相符
            else
            {
                foreach (VCodeInfo tpcbinfo in VCodeCombineInfoLst)
                {
                    // 与当前输入信息相符，结束并返回
                    if ((tpcbinfo.tpcb == currentTPCB) && (tpcbinfo.tp == currentTP))
                    {
                        CurrentSession.AddValue(Session.SessionKeys.IsComplete, true);

                    }
                    // 与当前输入信息不相符，该VCode绑定了其他的[TPCB]/[Tp]
                    else
                    {
                        List<string> errpara = new List<string>();
                        errpara.Add(currentVCode);
                        throw new FisException("CHK096", errpara);
                    }

                }
            }
            return base.DoExecute(executionContext);
        }

	}
}
