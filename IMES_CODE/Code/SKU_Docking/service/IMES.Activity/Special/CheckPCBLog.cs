// INVENTEC corporation (c)2012 all rights reserved. 
// Description:检查是否已经投线，若投线，则报错：该MB已投线，不能再投 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-13   Kerwin                       create
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.EcrVersion;

namespace IMES.Activity
{

    /// <summary>
    /// 检查ECR是否存在
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-SA-UC PCA ICT Input
    ///      CI-MES12-SPEC-SA-UC PCA ICT Input For Docking
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     select top 1 Status from PCBLog where PCBNo=@MBSno and Station='10' and Status='1' order by Cdt
    ///     若投线，则报错：该MB已投线，不能再投 
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         MB
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckPCBLog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPCBLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查是否已经投线，若投线，则报错：该MB已投线，不能再投
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;

            IMBRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IList<MBLog> MBLogList = CurrentRepository.GetMBLog(currenMB.Sn,Station, 1);
            if (MBLogList != null && MBLogList.Count > 0)
            {
                throw new FisException("ICT003", new string[] { currenMB.Sn });
            }
            return base.DoExecute(executionContext);
        }

    }
}

