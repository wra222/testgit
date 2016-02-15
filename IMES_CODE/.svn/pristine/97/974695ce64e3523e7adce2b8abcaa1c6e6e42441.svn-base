// INVENTEC corporation (c)2009 all rights reserved. 
// Description:   将CPU Vendor Sn 绑定到MB
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-12-10   Yuan XiaoWei                 create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
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
    /// 将CPU Vendor Sn 绑定到MB
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于Product Combine CPU站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.检查CPU是否已与其他MB绑定;
    ///         2.更新MB对象
    ///         3.保存MB对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             该Cpu已绑定至其它MB
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    ///         Session.CPUVendorSn
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
    ///         PCB
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMB
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class CombineMBCPU : BaseActivity
    {
        public CombineMBCPU()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string cvsn = (string)CurrentSession.GetValue(Session.SessionKeys.CPUVendorSn);

            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            string anotherPCBNo = currentMBRepository.IsUsedCvsn(cvsn);
            if (String.IsNullOrEmpty(anotherPCBNo))
            {
                currentMB.CVSN = cvsn;
                currentMBRepository.Update(currentMB, CurrentSession.UnitOfWork);
            }
            else
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add("CPU");
                erpara.Add(cvsn);
                erpara.Add(anotherPCBNo);
                ex = new FisException("CHK008", erpara);
                throw ex;
            }


            return base.DoExecute(executionContext);
        }
    }
}
