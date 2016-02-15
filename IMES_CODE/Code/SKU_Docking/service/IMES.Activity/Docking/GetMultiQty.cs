﻿// INVENTEC corporation (c)2012 all rights reserved. 
// Description:检查CheckMBSplit 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-13   Kerwin                       create
// 2012-03-01   Kerwin                       ITC-1360-0995
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.EcrVersion;
using IMES.DataModel;
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
    ///     1.若MB不是连板的子板，且已经做了先测后切的设置，则允许通过，，否则报错：该MB需进入MB Split
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
    public partial class GetMultiQty : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GetMultiQty()
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
            string MBCode = currenMB.MBCode;
            string CheckCode = currenMB.Sn.Substring(5, 1);
            if (currenMB.Sn.Substring(5, 1) == "M" || currenMB.Sn.Substring(5, 1) == "B")
            {
                CheckCode = currenMB.Sn.Substring(6, 1);
            }
            if ("0123456789".IndexOf(CheckCode) == -1)
            {
                IMBRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                MBCodeDef resultMBCode = CurrentRepository.GetMBCode(MBCode);

                if (resultMBCode != null)
                {
                    if (resultMBCode.Qty > 1)
                    {
                        CurrentSession.AddValue(Session.SessionKeys.MultiQty, resultMBCode.Qty);
                    }
                }
            }

            IPartRepository CurrentPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

            IPart currentPart = CurrentPartRepository.Find(currenMB.Model);
            if (currentPart == null || currentPart.Descr == null)
            {
                throw new FisException("CHK223", new string[] { currenMB.Sn });
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.FamilyName, currentPart.Descr);
            }

            CurrentSession.AddValue(Session.SessionKeys.IECVersion, "");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "ECR Label");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenMB.Sn);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenMB.Sn);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, Line + " " + currenMB.PCBModelID);
            return base.DoExecute(executionContext);
        }

    }
}

