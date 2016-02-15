/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Generate repair list of LCM.
 * 
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ===========================
 * 2012-08-17   itc202017                    create
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.Common.TestLog;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{

    /// <summary>
    /// Generate repair list of LCM.
    /// </summary>
    public partial class GenerateLCMRepairList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateLCMRepairList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Generate repair list of LCM.
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            
            //StepA: 若KeyPartRepair存在未维修完成（Count>0; Condition KeyPartRepair.ProductID=[ProductID] and KeyPartRepair.Status=‘0’）的记录，则不执行Step B
            //StepB: 检查是否存在需要提取的Log，若存在，则基于Test Log生成 Repair  Record
            KeyPartRepairInfo cond = new KeyPartRepairInfo();
            cond.productID = Key;
            cond.status = 0;
            if (partRepository.GetCountOfKpRepairCount(cond) <= 0)
            {
                bool bHasDefect = false;
                IProduct curProduct = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;

                IList<TestLog> tLogs = curProduct.TestLog;
                foreach (TestLog ele in tLogs)
                {
                    if (ele.Station == curProduct.Status.StationId && ele.Type == "PRD" && ele.Status == TestLog.TestLogStatus.Fail)   //0
                    {
                        bHasDefect = true;

                        KeyPartRepairInfo kprInfo = new KeyPartRepairInfo();
                        kprInfo.productID = this.Key;
                        kprInfo.model = curProduct.Model;
                        kprInfo.type = "LCM";
                        kprInfo.line = ele.Line;
                        kprInfo.station = ele.Station;
                        kprInfo.status = 0;
                        kprInfo.testLogID = ele.ID;
                        ProductLog latestLog = null;
                        foreach (ProductLog tmp in curProduct.ProductLogs)
                        {
                            if (tmp.Status == IMES.FisObject.Common.Station.StationStatus.Fail) //0
                            {
                                if (latestLog == null || tmp.Cdt > latestLog.Cdt)
                                {
                                    latestLog = tmp;
                                }
                            }
                        }
                        kprInfo.logID = (latestLog == null ? 0 : latestLog.Id);
                        kprInfo.editor = Editor;
                        kprInfo.cdt = DateTime.Now;
                        kprInfo.udt = DateTime.Now;
                        partRepository.AddKPRepair(kprInfo);

                        foreach (TestLogDefect tmp in ele.Defects)
                        {
                            RepairDefect defect = new RepairDefect();
                            defect.RepairID = kprInfo.id;
                            defect.Type = "LCM";
                            defect.DefectCodeID = tmp.DefectCode;
                            defect.Mark = "0";
                            defect.IsManual = false;
                            defect.Editor = Editor;
                            defect.Cdt = DateTime.Now;
                            defect.Udt = DateTime.Now;
                            defect.Cause = "";
                            defect.Obligation = "";
                            defect.Component = "";
                            defect.Site = "";
                            defect.Location = "";
                            defect.MajorPart = "";
                            defect.Remark = "";
                            defect.VendorCT = "";
                            defect.PartType = "";
                            defect.OldPart = "";
                            defect.OldPartSno = "";
                            defect.NewPart = "";
                            defect.NewPartSno = "";
                            defect.Manufacture = "";
                            defect.VersionA = "";
                            defect.VersionB = "";
                            defect.SubDefect = "";
                            defect.PIAStation = "";
                            defect.Distribution = "";
                            defect._4M = "";
                            defect.Responsibility = "";
                            defect.Action = "";
                            defect.Cover = "";
                            defect.Uncover = "";
                            defect.TrackingStatus = "";
                            defect.MTAID = "";
                            defect.ReturnStation = "";
                            defect.ReturnSign = "";
                            partRepository.AddKPRepairDefect(defect);
                        }
                        break;
                    }
                }
                if (!bHasDefect)
                {
                    string ctno = CurrentSession.GetValue("CTNO") as string;

                    throw new FisException("CHK233", new string[] { ctno });    //CT：XXX没有不良记录.
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}
