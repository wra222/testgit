/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description: GenerateSVBSno
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-01-31   Tong.Zhi-Yong     Create 
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
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO ;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 產生SVB Sno號相关逻辑
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      VGA label print
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.Insert MBInfo
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.SVBSnList
    ///         Session.SessionKeys.VGA
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         MBInfo
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class GenerateSVBSno : BaseActivity
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateSVBSno()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 產生SVB Sno號相关逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB vga = (IMB)CurrentSession.GetValue(Session.SessionKeys.VGA);
            IList<string> svbSnoList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.SVBSnList);
            MBInfo info = null;
            IMBRepository imr = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            if (svbSnoList != null)
            {
                foreach (string item in svbSnoList) 
                {
                    info = new MBInfo(0, vga.Sn, "SVB", item, Editor, DateTime.Now, DateTime.Now);
                    vga.AddMBInfo(info);
                }

                imr.Update(vga, CurrentSession.UnitOfWork);
            }

            return base.DoExecute(executionContext);
        }
	}
}
