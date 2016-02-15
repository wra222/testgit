/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: for set child mb sno for muti mb
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2009-12-22   207013     Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */


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
    ///  置本次循环里的child MB
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// 记录KP/CT的绑定关系
    /// 保存LCM与BTDL的绑定
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.LoopCount
    ///         Session.MBNOList
    ///         Session.MBList
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MBSN
    ///         Session.MB
    ///         Session.LoopCount
    /// </para> 
    ///<para> 
    /// 数据更新:
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class SetChildMBSN: BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public SetChildMBSN()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 置本次循环里的child MB
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var index = (int)CurrentSession.GetValue(Session.SessionKeys.LoopCount);
            var curmbsnlist =(List<string>)CurrentSession.GetValue(Session.SessionKeys.MBNOList);
            if (curmbsnlist == null || curmbsnlist.Count == 0 || !(index < curmbsnlist.Count) ||  curmbsnlist[index].Trim() == string.Empty)
            {
                var ex = new FisException("CHK012", new string[] {this.Key});
            }
            var MBObjectList = (List<IMB>)CurrentSession.GetValue(Session.SessionKeys.MBList);
            CurrentSession.AddValue(Session.SessionKeys.MBSN, curmbsnlist[index].Trim());
            CurrentSession.AddValue(Session.SessionKeys.MB, MBObjectList[index]);
            index++;
            CurrentSession.AddValue(Session.SessionKeys.LoopCount, index);
            return base.DoExecute(executionContext);
        }
	}
}
