/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for RCTO MB Change Page
* UI:CI-MES12-SPEC-SA-UI RCTO MB Change.docx –2012/6/15 
* UC:CI-MES12-SPEC-SA-UC RCTO MB Change.docx –2012/6/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-8-1    Jessica Liu           Create
* Known issues:
* TODO：
* ITC-1428-0027, Jessica Liu, 2012-9-12
*/


using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.PCA.MBMO;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.TestLog;

namespace IMES.Activity
{
    /// <summary>
    /// Save MB for RCTO MB Change
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         RCTO MB Change
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    ///             
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.MB 
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
    ///    
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         
    /// </para> 
    /// </remarks>
    public partial class SaveMBSNo : BaseActivity
	{
        /// <summary>
        /// SaveMB
        /// </summary>
        public SaveMBSNo()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Save MB for RCTO MB Change
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currentMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            string currentNewMBSno = CurrentSession.GetValue(Session.SessionKeys.MBSN) as string;
            
            //2012-9-7, Jessica Liu
            string oldMBSno = currentMB.Sn;

            if (currentMB == null)
            {
                throw new NullReferenceException("MB in session is null");
            }

            bool isMatch = false;
            PCBStatusInfo tempPCBStatusInfo = new PCBStatusInfo();
            PCBStatusInfo tempCond = new PCBStatusInfo();
            IMBRepository CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IList<MBLog> MBLogList = CurrentRepository.GetMBLog(currentMB.Sn, "1A", 1);
            if (MBLogList != null && MBLogList.Count > 0)
            {
                IList<Repair> tempRepairLst = CurrentRepository.GetPcbRepairList(currentMB.Sn, "15");
                if (tempRepairLst != null && tempRepairLst.Count > 0)
                {
                    isMatch = false;
                }
                else
                {
                    isMatch = true;

                    /* 2012-9-7, Jessica Liu, UC变更：Save时，满足条件I不再进行Station=10的设置
                    //update PCBStatus(PCBNo=@NewMBSN, Station=10, Editor, Udt; Condition: PCBNo=@MBSN)                   
                    tempPCBStatusInfo.station = "10";
                    */
                }

                IList<TestLog> lstalltestlog = CurrentRepository.GetPCBTestLogListFromPCBTestLogByType(currentMB.Sn, 1, "M/B");
                if (lstalltestlog != null && lstalltestlog.Count > 0)
                {
                    isMatch = true;
                }
                else
                {
                    isMatch = true;

                    //update PCBStatus(PCBNo=@NewMBSN, Station=10, Editor, Udt; Condition: PCBNo=@MBSN)                   
                    tempPCBStatusInfo.station = "10";
                }

            }
            else
            {
                if (currentMB.MBStatus.Station == "10" || currentMB.MBStatus.Station == "15" || currentMB.MBStatus.Station == "31")
                {
                    isMatch = true;

                    //update PCBStatus(PCBNo=@NewMBSN, Station=10, Editor, Udt; Condition: PCBNo=@MBSN)                   
                    tempPCBStatusInfo.station = "10";
                }
            }

            if (isMatch)
            {
                //update PCB（PCBNo=@NewMBSN, Udt; Codition: PCBNo=@MBSN）           
                PcbEntityInfo tempPcbInfo = new PcbEntityInfo();
                tempPcbInfo.pcbno = currentNewMBSno;
                PcbEntityInfo cond = new PcbEntityInfo();
                cond.pcbno = currentMB.Sn;
                CurrentRepository.UpdatePcb(tempPcbInfo, cond);
                
                //update PCBInfo(PCBNo=@NewMBSN, Editor, Udt; Condition: PCBNo=@MBSN)
                IMES.FisObject.PCA.MB.MBInfo tempMBInfo = new IMES.FisObject.PCA.MB.MBInfo();
                tempMBInfo.PCBID = currentNewMBSno;
                tempMBInfo.Editor = this.Editor;
                tempMBInfo.Udt = DateTime.Now;
                IMES.FisObject.PCA.MB.MBInfo MBCond = new IMES.FisObject.PCA.MB.MBInfo();
                MBCond.PCBID = currentMB.Sn;
                CurrentRepository.UpdatePcbInfo(tempMBInfo, MBCond);

                //update PCBStatus(PCBNo=@NewMBSN, Station=10, Editor, Udt; Condition: PCBNo=@MBSN)
                //或update PCBStatus(PCBNo=@NewMBSN, Editor, Udt; Condition: PCBNo=@MBSN)
                tempPCBStatusInfo.pcbno = currentNewMBSno;
                tempPCBStatusInfo.editor = this.Editor;
                tempPCBStatusInfo.udt = DateTime.Now;
                tempCond.pcbno = currentMB.Sn;
                CurrentRepository.UpdatePCBStatus(tempPCBStatusInfo, tempCond);    


                //Insert PCBInfo （PCBNo=@NewMBSN；InfoType=’RCTOChange’；InfoValue=@MBSN）
                IMES.FisObject.PCA.MB.MBInfo mbctInfo = new IMES.FisObject.PCA.MB.MBInfo(
                            0, 
                            currentNewMBSno, 
                            "RCTOChange", 
                            currentMB.Sn, 
                            this.Editor, 
                            DateTime.Now, 
                            DateTime.Now);
                //2012-9-7, Jessica Liu
                //currentMB.AddMBInfo(mbctInfo);
                CurrentRepository.AddMBInfoesDefered(CurrentSession.UnitOfWork, new IMES.FisObject.PCA.MB.MBInfo[] { mbctInfo });

                //Insert PCBLog（PCBNo=@NewMBSN; Station=PCBStatus.Station; Line=[Select Line]; Status=1）
                //line = string.IsNullOrEmpty(this.Line) ? currentMB.MBStatus.Line : this.Line;
                
                //ITC-1428-0027, Jessica Liu, 2012-9-12
                string currentStation = "";
                if (string.IsNullOrEmpty(tempPCBStatusInfo.station))
                {
                    currentStation = currentMB.MBStatus.Station;
                }
                else
                {
                    currentStation = tempPCBStatusInfo.station;
                }

                var mbLog = new MBLog(
                    0,
                    currentNewMBSno,
                    currentMB.Model,
                    //ITC-1428-0027, Jessica Liu, 2012-9-12
                    //currentMB.MBStatus.Station,
                    currentStation,
                    1,
                    this.Line,  //line,
                    this.Editor,
                    DateTime.Now);
                //2012-9-7, Jessica Liu
                //currentMB.AddLog(mbLog);
                CurrentRepository.AddMBLogsDefered(CurrentSession.UnitOfWork, new MBLog[] { mbLog });

                CurrentRepository.Update(currentMB, CurrentSession.UnitOfWork);
            }
            else
            {
                throw new FisException("CHK944", new string[] { }); //流程错误，请确定MB的状态!
            }

            //2012-9-7, Jessica Liu
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "RCTO MB Label");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currentNewMBSno);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currentNewMBSno);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, oldMBSno);

            return base.DoExecute(executionContext);
        }
	
	}
}
