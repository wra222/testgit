// INVENTEC corporation (c)2012 all rights reserved. 
// Description:检查ECR是否存在 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-13   Kerwin                       create
// 2012-02-14   Kerwin                       ITC-1360-0417
// 2012-02-15   Kerwin                       ITC-1360-0449
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.EcrVersion;
using IMES.FisObject.Common.Part;

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
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     1.	select Count(*) from EcrVersion where Family=@Family and MBCode = left(@MBSno,2) and ECR = @ECR
    ///          若不存在，则报错：ECR不存在
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
    public partial class CheckECR : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckECR()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查ECR是否存在
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            IPartRepository CurrentPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            
            IPart currentPart = CurrentPartRepository.Find(currenMB.Model);
            IEcrVersionRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IEcrVersionRepository, EcrVersion>();
            if (currentPart ==null ||currentPart.Descr == null)
            {
                throw new FisException("CHK223", new string[] { currenMB.Sn });
            }
            else {
                CurrentSession.AddValue(Session.SessionKeys.FamilyName, currentPart.Descr);
            }
            string ecr = CurrentSession.GetValue(Session.SessionKeys.ECR) as string;

            IList<EcrVersion> ECRList = CurrentRepository.GetECRVersionByFamilyMBCodeAndECR(currentPart.Descr, currenMB.MBCode, ecr);
            if (ECRList == null || ECRList.Count == 0)
            {
                throw new FisException("ICT001", new string[] { currenMB.Sn });
            }
            else {
                CurrentSession.AddValue(Session.SessionKeys.IECVersion, ECRList[0].IECVer);
            }

             CurrentSession.AddValue(Session.SessionKeys.PrintLogName,"ECR Label");
             CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenMB.Sn);
             CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenMB.Sn);
             CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr,Line+" "+currenMB.PCBModelID);
                               
            return base.DoExecute(executionContext);
        }

    }
}

