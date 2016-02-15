// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Mac绑定至MB
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-09   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
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

namespace IMES.Activity
{
    /// <summary>
    /// 将Mac绑定至MB
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于ICT Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.将Mac绑定至MB对象;
    ///         2.保存MB对象;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.MAC
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
    ///         更新PCB
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    ///         IMB
    /// </para> 
    /// </remarks>
    public partial class BindMAC : BaseActivity
	{

        /// <summary>
        /// 构造函数
        /// </summary>
		public BindMAC()
		{
			InitializeComponent();
		}

        /// <summary>
        /// MAC绑定至MB
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            currentMB.MAC = (string)CurrentSession.GetValue(Session.SessionKeys.MAC);
            
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            
            //ITC-1103-0079 c.	是否存在重复的MAC，需要报告错误：“Duplicate MAC Address!”
            IMB oldMB = (IMB)currentMBRepository.Find(currentMB.Sn);

            if ((oldMB!=null) && (!string.IsNullOrEmpty(oldMB.MAC)))
            {
                IList<IMB> oldmaclist = currentMBRepository.GetMBListByMAC(oldMB.MAC);
                if ((oldmaclist != null) && (oldmaclist.Count > 1))
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK032", errpara);
                }
            }

                IList<IMB> newmaclist = currentMBRepository.GetMBListByMAC(currentMB.MAC);
                if ((newmaclist != null) && (newmaclist.Count > 0))
                {
                    List<string> errpara = new List<string>();                    
                    throw new FisException("CHK032", errpara);
                }

            currentMBRepository.Update(currentMB, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
	}
}
