// INVENTEC corporation (c)2012 all rights reserved. 
// Description:获取PdLine的Qty
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
using IMES.FisObject.Common.Part;
using System;
using IMES.DataModel;

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
    public partial class GetPCAICTCount : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetPCAICTCount()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取PdLine的Qty
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMBRepository CurrentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            IList<int> resultList = CurrentMBRepository.GetQtyListFromPcaIctCountByCdtAndPdLine(new DateTime(1900, 1, 1, 0, 0, 0, 0), Line);
            if (resultList == null || resultList.Count == 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.Qty, 0);
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.Qty, resultList[0]);
            }

            return base.DoExecute(executionContext);
        }

        private static object _syncRoot_GetSeq = new object();
    }
}

