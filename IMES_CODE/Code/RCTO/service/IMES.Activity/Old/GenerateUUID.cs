// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 产生UUID, 只产生一个
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-09   Yuan XiaoWei                 create
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
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Threading;

namespace IMES.Activity
{
    /// <summary>
    /// 产生UUID, 只产生一个
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ICT Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.参考UC;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：CHK002
    ///         2.业务异常：CHK001
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
    ///         Session.UUID
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         无
    /// </para> 
    /// </remarks>
    public partial class GenerateUUID : BaseActivity
    {
        public GenerateUUID()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            if (currentMB == null)
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK002", errpara);
            }

            if (string.IsNullOrEmpty(currentMB.MAC) || currentMB.MAC.Length != 12 ||  currentMB.MAC.Substring(0, 1).CompareTo("7") > 0)
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK001", errpara);
            }

//            if (string.IsNullOrEmpty(currentMB.UUID))  （非正常操作）刷完ICT Input保存后，手动修改PCBStatus.Station=‘09’，重进ICT Input，保存后，PCB.MAC重新分配，但PCB.UUID却没有重新分配。谷晓川：经和徐建勇、高永勃、苑小伟商议，此问题不会在正常情况下出现，暂时不需修改 高永勃：经上线总结会议讨论，保存时，如果重新分配MAC 地址，那么UUID 也需要根据新的MAC 产生新的UUID
//            {
                IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                DateTime currentTime;
                DateTime currentUtcTime;
                lock (_syncRoot_GetSeq)
                {
                    currentTime = DateTime.Now;
                    currentUtcTime = DateTime.UtcNow;
                    Thread.Sleep(4);//UUID要求时间戳不能相同，一毫秒内不能产生两个UUID,但是SQL只能精确到3.333毫秒
                }
                string NewUUID = currentMBRepository.GenerateUUID(currentMB.MAC, currentTime, currentUtcTime);
                CurrentSession.AddValue(Session.SessionKeys.UUID, NewUUID);
//            }
//            else
//            {
//                CurrentSession.AddValue(Session.SessionKeys.UUID, currentMB.UUID);
//            }

            return base.DoExecute(executionContext);
        }

        private static object _syncRoot_GetSeq = new object();
    }
}
