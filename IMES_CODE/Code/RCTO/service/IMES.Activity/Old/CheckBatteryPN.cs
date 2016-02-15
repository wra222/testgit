// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 检查输入Battery PN的正确性
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-06   Yuan XiaoWei                 create
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
using IMES.FisObject.Common.PartSn;

namespace IMES.Activity
{
    /// <summary>
    /// 检查输入Battery PN的正确性
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于Combine KPCT
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.业务异常：CHK031
    ///                    
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PartSN
    ///         Session.BatteryPN
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         PartSn
    /// </para> 
    /// </remarks>
	public partial class CheckBatteryPN: BaseActivity
	{
		public CheckBatteryPN()
		{
			InitializeComponent();
		}

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string batteryPN = CurrentSession.GetValue(Session.SessionKeys.BatteryPN).ToString();
            PartSn currentPartSN = (PartSn)CurrentSession.GetValue(Session.SessionKeys.PartSN);

            //Battery pn  不等于 PartSN.IECPN
            if (string.Compare(currentPartSN.IecPn, batteryPN, false) != 0)
            {
                var ex = new FisException("CHK031", new string[] { batteryPN });
                ex.stopWF = false;
                throw ex;
            }

            return base.DoExecute(executionContext);
        }
	}
}
