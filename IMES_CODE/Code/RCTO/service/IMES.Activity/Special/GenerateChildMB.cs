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
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.PrintLog;

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
    public partial class GenerateChildMB : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public GenerateChildMB()
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

            string moNo = CurrentMB.SMTMO;
            string model = CurrentMB.Model;
            string custSn = "";
            string custVer = "";
            string cvsn = "";
            string uuid = "";

            bool IsRCTO = (bool)CurrentSession.GetValue(Session.SessionKeys.IsRCTO);
            string CheckCode = CurrentSession.GetValue(Session.SessionKeys.CheckCode) as string;

            string dateCode = CurrentSession.GetValue(Session.SessionKeys.DCode) as string;
            string ecr = CurrentSession.GetValue(Session.SessionKeys.ECR) as string;
            string iecVer = CurrentSession.GetValue(Session.SessionKeys.IECVersion) as string;

            var MACList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.MACList);
            var MBCTList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.MBCTList);
            var EEPList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.EEPList);

            var NewMBSnList = CurrentSession.GetValue(Session.SessionKeys.MBNOList) as List<string>;

            var MBNOList = CurrentSession.GetValue(Session.SessionKeys.MBNOList) as List<string>;
            if (IsRCTO && CheckCode !="R") {
                MBNOList = CurrentSession.GetValue(Session.SessionKeys.RCTOChildMBSnList) as List<string>;
            }
            var MBObjectList = new List<IMB>();
            CurrentSession.AddValue(Session.SessionKeys.MBList, MBObjectList);

            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            IPrintLogRepository LogRepository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();


            for (int i = 0; i < MBNOList.Count; i++)
            {

                string mac = "";
                if (MACList != null)
                {
                    mac = MACList[i];
                }
                MB mb = new MB(MBNOList[i], moNo, custSn, model, dateCode, mac, uuid, ecr, iecVer, custVer, cvsn, DateTime.Now, DateTime.Now);
                MBStatus mbStatus = new MBStatus(MBNOList[i], this.Station, MBStatusEnum.Pass, this.Editor, this.Line, DateTime.Now, DateTime.Now);
                mb.MBStatus = mbStatus;

                if (IsRCTO && CheckCode != "R")
                {
                    IMES.FisObject.PCA.MB.MBInfo RCTOChangeInfo = new IMES.FisObject.PCA.MB.MBInfo(0, mb.Sn, "RCTOChange", NewMBSnList[i], Editor, DateTime.Now, DateTime.Now);
                    mb.AddMBInfo(RCTOChangeInfo);
                }

                if (EEPList != null)
                {
                    IMES.FisObject.PCA.MB.MBInfo eepInfo = new IMES.FisObject.PCA.MB.MBInfo(0, mb.Sn, "EEPROM", EEPList[i], Editor, DateTime.Now, DateTime.Now);
                    mb.AddMBInfo(eepInfo);
                }

                if (MBCTList != null)
                {
                    IMES.FisObject.PCA.MB.MBInfo mbctInfo = new IMES.FisObject.PCA.MB.MBInfo(0, mb.Sn, "MBCT", MBCTList[i], Editor, DateTime.Now, DateTime.Now);
                    mb.AddMBInfo(mbctInfo);
                    FruDetInfo newFruDet = new FruDetInfo();
                    newFruDet.sno = MBCTList[i];
                    newFruDet.snoId = mb.Sn;
                    newFruDet.tp = "VC";
                    newFruDet.editor = Editor;
                    newFruDet.cdt = DateTime.Now;
                    newFruDet.udt = newFruDet.cdt;
                    mbRepository.InsertFruDetInfoDefered(CurrentSession.UnitOfWork, newFruDet);

                    MBLog newCTLog = new MBLog(0, mb.Sn, mb.Model, "SH", 1, "SA SHIPPING LABEL", Editor, DateTime.Now);
                    mb.AddLog(newCTLog);
                }

                MBLog newLog = new MBLog(0, mb.Sn, mb.Model, Station, 1, Line, Editor, DateTime.Now);
                mb.AddLog(newLog);
                mbRepository.Add(mb, CurrentSession.UnitOfWork);
                MBObjectList.Add(mb);

                var item = new PrintLog
                {
                    Name = CurrentSession.GetValue(Session.SessionKeys.PrintLogName).ToString(),
                    BeginNo = MBNOList[i],
                    EndNo = MBNOList[i],
                    Descr = CurrentSession.GetValue(Session.SessionKeys.PrintLogDescr).ToString(),
                    Editor = this.Editor
                };
                LogRepository.Add(item, CurrentSession.UnitOfWork);
            }

            return base.DoExecute(executionContext);
        }
    }
}
