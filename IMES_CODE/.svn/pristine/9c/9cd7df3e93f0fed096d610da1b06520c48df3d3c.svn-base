/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description:CI-MES12-SPEC-SA-UC MB Label Print.docx
 *             update SMTMO
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-01-03   zhu lei           Create 
 * 
 * Known issues:
 */

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
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.MBMO ;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.Repository.PCA;
using IMES.FisObject.PCA.MBChangeLog;

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
    ///         应用于
    ///         MB Label Print
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
    public partial class GenerateMBSn : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public GenerateMBSn()
		{
			InitializeComponent();
		}

        /// <summary>
        /// GenerateMBSn
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string moNo;
            string model;
            string dateCode;
            string custSn;
            var mbmo = (IMBMO)CurrentSession.GetValue(Session.SessionKeys.MBMO);
            if (mbmo == null)
            {
                var CurrentMB = (MB)CurrentSession.GetValue(Session.SessionKeys.MB);
                moNo = CurrentMB.SMTMO;
                model = CurrentMB.Model;
                dateCode = CurrentMB.DateCode;
                custSn = ""; //CurrentMB.CustSn; //由于改过Kenel后报错,jiali Add

            }
            else
            {
                moNo = mbmo.MONo;
                model = mbmo.Model;
                dateCode = CurrentSession.GetValue(Session.SessionKeys.DateCode).ToString();
                //由于改过Kenel后报错,jiali Add
                custSn = "";// CurrentSession.Customer;
            }

            IList MBNOList = new ArrayList();
            MBNOList = (IList)CurrentSession.GetValue(Session.SessionKeys.MBNOList);

            var MBObjectList = new List<IMB>();

            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
           
            // add check replaceMB case
            bool isReplaceMB=false;
            if (CurrentSession.GetValue(ExtendSession.SessionKeys.IsReplaceMB)!=null)
            {
                isReplaceMB = (bool)CurrentSession.GetValue(ExtendSession.SessionKeys.IsReplaceMB);
            }
            if (!isReplaceMB)
            {
                foreach (string item in MBNOList)
                {
                    string mac = "";
                    string uuid = "";
                    string ecr = "";
                    string iecVer = "";
                    string custVer = "";
                    string cvsn = "";
                    if (this.Station == "VG")
                    {
                        ecr = "00000";
                    }
                    //由于改过Kenel后报错,jiali Add custSn
                    MB mb = new MB(item, moNo, custSn, model, dateCode, mac, uuid, ecr, iecVer, custVer, cvsn, DateTime.Now, DateTime.Now);
                    MBStatus mbStatus = new MBStatus(item, this.Station, MBStatusEnum.Pass, this.Editor, this.Line, DateTime.Now, DateTime.Now);
                    mb.MBStatus = mbStatus;

                    mbRepository.Add(mb, CurrentSession.UnitOfWork);
                    MBObjectList.Add(mb);
                }
            }
            else
            {

                
                string oldMBSN = (string)CurrentSession.GetValue(Session.SessionKeys.OldMB);
                string newMBSN = MBNOList[0].ToString();
                string reason = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.ChangMBReason);
                // Change to one Child MB SN for only one
                //newMBSN = newMBSN.Substring(0, 5) + oldMBSN.Substring(5, 1) + newMBSN.Substring(6, 4);
                if (newMBSN.Substring(5, 1) == "M")
                {
                    if (oldMBSN.Substring(5,1) =="M")
                        newMBSN = newMBSN.Substring(0, 6) + oldMBSN.Substring(6, 1) + newMBSN.Substring(7, 4);
                    else
                        newMBSN = newMBSN.Substring(0, 6) + oldMBSN.Substring(5, 1) + newMBSN.Substring(7, 4);
                }
                else
                {
                    if (oldMBSN.Substring(5, 1) == "M")
                       newMBSN = newMBSN.Substring(0, 5) + oldMBSN.Substring(6, 1) + newMBSN.Substring(6, 4);
                    else
                       newMBSN = newMBSN.Substring(0, 5) + oldMBSN.Substring(5, 1) + newMBSN.Substring(6, 4);
                }
                MBNOList[0] = newMBSN;
               
                //Jiali Add
                MBChangeLogRepository mbchange = new MBChangeLogRepository();
                MBChangeLog mbchangelog = new MBChangeLog(oldMBSN, newMBSN, reason, Editor, DateTime.Now);
                mbchange.Add(mbchangelog, CurrentSession.UnitOfWork);

                mbRepository.ReplaceMBSn(oldMBSN, newMBSN);
                MBObjectList.Add(mbRepository.Find(newMBSN));
                              
            }
            CurrentSession.AddValue(Session.SessionKeys.MBList, MBObjectList);

           return base.DoExecute(executionContext);
        }
	}
}
