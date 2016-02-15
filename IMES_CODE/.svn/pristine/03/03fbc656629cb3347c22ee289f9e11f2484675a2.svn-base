// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 根据输入的 TPCB,TP 获取VCode对象
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-03-22   Chen Xu (eB1-4)              create
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
    ///  根据TPCB和TP获取VCode对象
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
    ///         从Session中获得TPCB和TP，调用ITPCBRepository得到对应的VCode，放到Session中，可继续Update操作；
	///			若没有对应VCode，返回空，可继续Insert操作。
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
	///			2.业务异常：
	///								CHK097: 该TPCB  和TP 与多个Vcode绑定，请检查数据库！！
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPCB
	///			Session.TP
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.VCode
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
    
    
    public partial class GetVCode : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetVCode()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 根据TPCB,TP获取VCode对象并放到Session中
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns>VCode</returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currentTPCB = (string)CurrentSession.GetValue(Session.SessionKeys.TPCB);
            var currentTP = (string)CurrentSession.GetValue(Session.SessionKeys.TP);
            ITPCBRepository TPCBRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBRepository, TPCB>();
            IList<string> currentVCodeLst = TPCBRepository.GetVCodeInfo(currentTPCB, currentTP);

            if (currentVCodeLst.Count == 1)
            {
                //CurrentSession.AddValue(Session.SessionKeys.VCode, currentVCodeLst[0]);

                foreach (string curVCode in currentVCodeLst)
                {
                    CurrentSession.AddValue(Session.SessionKeys.VCode, curVCode);
                }
            }

            else if (currentVCodeLst.Count == 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.VCode, "");
            }

            else
            {
                List<string> errpara = new List<string>();
                errpara.Add(currentTPCB);
                errpara.Add(currentTP);
                throw new FisException("CHK097", errpara);
            }
            return base.DoExecute(executionContext);

        }
	}
}
