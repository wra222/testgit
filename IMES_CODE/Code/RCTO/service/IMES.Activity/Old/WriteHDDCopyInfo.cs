// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 记录硬盘拷贝信息到HDDCopyInfo表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-01-04   Yuan XiaoWei                 create
// 2010-01-04   Yuan XiaoWei                 Modify ITC-1122-0086
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
using IMES.FisObject.Common.HDDCopyInfo;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.PartSn;

namespace IMES.Activity
{
    /// <summary>
    /// 记录硬盘拷贝信息到HDDCopyInfo表
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于HDD Test
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.从Session中获取MachineNo，OriginalHDD,ConnectNo作为HDDCopyInfo的的MachineNo，OriginalHDD,ConnectNo
    ///         2.构建HDDCopyInfo对象，保存到HDDCopyInfo表
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MachineNo
    ///         Session.OriginalHDD
    ///         Session.ConnectNo
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
    ///         HDDCopyInfo  
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IHDDCopyInfoRepository
    ///              HDDCopyInfo
    /// </para> 
    /// </remarks>
    public partial class WriteHDDCopyInfo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WriteHDDCopyInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存HDDCopyInfo
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override System.Workflow.ComponentModel.ActivityExecutionStatus DoExecute(System.Workflow.ComponentModel.ActivityExecutionContext executionContext)
        {
            string currentMachineNo = (string)CurrentSession.GetValue(Session.SessionKeys.MachineNo);
            string currentOriginalHDD = (string)CurrentSession.GetValue(Session.SessionKeys.OriginalHDD);
            string currentConnectNo = (string)CurrentSession.GetValue(Session.SessionKeys.ConnectNo);
            string currentHDDNo = ((PartSn)CurrentSession.GetValue(Session.SessionKeys.PartSN)).IecSn;
            IHDDCopyInfoRepository currentHDDRepository = RepositoryFactory.GetInstance().GetRepository<IHDDCopyInfoRepository, HDDCopyInfo>();

            HDDCopyInfo newHDDCopyInfo = new HDDCopyInfo(0, currentMachineNo, currentConnectNo, currentOriginalHDD, currentHDDNo, Editor);

            currentHDDRepository.Add(newHDDCopyInfo, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
    }
}
