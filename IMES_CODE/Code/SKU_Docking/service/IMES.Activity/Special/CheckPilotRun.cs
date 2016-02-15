// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      1.	检查MB是否为Pilot Run
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
    /// 检查MB是否为Pilot Run
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-SA-UC PCA ICT Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     1.select Remark from EcrVersion where MBCode= left(@MBSno,2) and Ecr=@ecr
    ///       若Remark不为空或不为Null，则提示“This MB is Polit Run
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
    public partial class CheckPilotRun : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckPilotRun()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查MB是否是PolitRun
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            bool IsPolitRun = false;
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            string ecr = CurrentSession.GetValue(Session.SessionKeys.ECR) as string;
            IEcrVersionRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();
            IList<EcrVersion> ECRList = CurrentRepository.GetECRVersionByMBCodeAndECR(currenMB.MBCode, ecr);
            if (ECRList != null && ECRList.Count > 0)
            {
                if (!string.IsNullOrEmpty(ECRList[0].Remark)) {
                    IsPolitRun = true;
                }
            }
            CurrentSession.AddValue(Session.SessionKeys.IsPolitRun, IsPolitRun);
            return base.DoExecute(executionContext);
        }

    }
}

