/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Generate repair list of KP.
 * 
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ===========================
 * 2012-07-27   itc202017                    create
 * Known issues:
 */
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.FisObject.Common.TestLog;
using IMES.Infrastructure.Extend;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{

    /// <summary>
    /// Generate repair list of KP.
    /// </summary>
    public partial class GenerateKPRepairList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerateKPRepairList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Generate repair list of KP.
        /// </summary>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository>();
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<IqcCause1Info> iqcList = new List<IqcCause1Info>();
            IqcCause1Info cond1 = new IqcCause1Info();
            cond1.ctLabel = Key;
            IqcCause1Info cond_neq = new IqcCause1Info();
            cond_neq.status = "1";
            iqcList = defectRepository.GetIqcCause1InfoList(cond1, cond_neq);
            if (iqcList.Count <= 0)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(Key);
                ex = new FisException("CHK233", erpara);    //CT：XXX没有不良记录.
                throw ex;
            }

            KeyPartRepairInfo cond2 = new KeyPartRepairInfo();
            cond2.productID = Key;
            cond2.status = 0;
            if (partRepository.GetCountOfKpRepairCount(cond2) <= 0)
            {
                KeyPartRepairInfo kprInfo = new KeyPartRepairInfo();
                kprInfo.productID = this.Key;
                kprInfo.model = "";
                kprInfo.type = "KP";
                kprInfo.line = "";
                kprInfo.station = "";
                kprInfo.status = 0;
                kprInfo.testLogID = 0;
                kprInfo.logID = 0;
                kprInfo.editor = Editor;
                kprInfo.cdt = DateTime.Now;
                kprInfo.udt = DateTime.Now;
                partRepository.AddKPRepair(kprInfo);

                foreach (IqcCause1Info tmp in iqcList)
                {
                    RepairDefect defect = new RepairDefect();
                    defect.RepairID = kprInfo.id;
                    defect.Type = "KP";
                    defect.DefectCodeID = tmp.mpDefect;
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
            }

            return base.DoExecute(executionContext);
        }
    }
}
