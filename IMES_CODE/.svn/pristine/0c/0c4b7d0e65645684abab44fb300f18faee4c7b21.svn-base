/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Generate Child MB
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-17   Kerwin            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 保存产生的MBNO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-SA-UC PCA ICT Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.MBNOList中每个MBNO
    ///             1.创建MB对象
    ///             2.保存MB对象
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.ModelName
    ///         Session.SMTMONO
    ///         Session.DateCode
    ///         Session.MBNOList
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
    ///         insert PCB
    ///         insert PCBStatus
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class UpdateMBForICTInput : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public UpdateMBForICTInput()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var CurrentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);

            string ecr = CurrentSession.GetValue(Session.SessionKeys.ECR) as string;
            string iecVer = CurrentSession.GetValue(Session.SessionKeys.IECVersion) as string;
            CurrentMB.ECR = ecr;
            CurrentMB.IECVER = iecVer;

            string mac = CurrentSession.GetValue(Session.SessionKeys.MAC) as string;
            if (!string.IsNullOrEmpty(mac))
            {
                CurrentMB.MAC = mac;
            }
            string dateCode = CurrentSession.GetValue(Session.SessionKeys.DCode) as string;
            if (!string.IsNullOrEmpty(dateCode))
            {
                CurrentMB.DateCode = dateCode;
            }

            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            string eep = CurrentSession.GetValue(Session.SessionKeys.EEP) as string;
            if (!string.IsNullOrEmpty(eep))
            {
                IMES.FisObject.PCA.MB.MBInfo eepInfo = new IMES.FisObject.PCA.MB.MBInfo(0, CurrentMB.Sn, "EEPROM", eep, Editor, DateTime.Now, DateTime.Now);
                CurrentMB.AddMBInfo(eepInfo);
            }

            string mbct = CurrentSession.GetValue(Session.SessionKeys.MBCT) as string;
            if (!string.IsNullOrEmpty(mbct))
            {
                IMES.FisObject.PCA.MB.MBInfo mbctInfo = new IMES.FisObject.PCA.MB.MBInfo(0, CurrentMB.Sn, "MBCT", mbct, Editor, DateTime.Now, DateTime.Now);
                CurrentMB.AddMBInfo(mbctInfo);

                FruDetInfo newFruDet = new FruDetInfo();
                newFruDet.sno = mbct;
                newFruDet.snoId = CurrentMB.Sn;
                newFruDet.tp = "VC";
                newFruDet.editor = Editor;
                newFruDet.cdt = DateTime.Now;
                newFruDet.udt = newFruDet.cdt;
                mbRepository.InsertFruDetInfoDefered(CurrentSession.UnitOfWork, newFruDet);

                MBLog newCTLog = new MBLog(0, CurrentMB.Sn, CurrentMB.Model, "SH", 1, "SA SHIPPING LABEL", Editor, DateTime.Now);
                CurrentMB.AddLog(newCTLog);
            }

            mbRepository.Update(CurrentMB, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
    }
}
