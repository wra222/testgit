using System;
using System.Collections.Generic;
using System.Collections;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SaveForPCAOQCCosmetic : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SaveForPCAOQCCosmetic()
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
            var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            int RepairID = 0;

            IList<PcboqcrepairInfo> repairLst = new List<PcboqcrepairInfo>();
            PcboqcrepairInfo cond1 = new PcboqcrepairInfo();
            cond1.status = "0";
            cond1.pcbno = mb.Sn;
            repairLst = mbRepository.GetPcboqcrepairInfoListOrderByCdtDesc(cond1);
            if (repairLst != null && repairLst.Count > 0)
            {
                PcboqcrepairInfo cond = new PcboqcrepairInfo();
                PcboqcrepairInfo setValue = new PcboqcrepairInfo();
                cond.id = repairLst[0].id;

                setValue.pcbno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN);
                setValue.lotNo = (string)CurrentSession.GetValue(Session.SessionKeys.LotNo);
                setValue.station = mb.MBStatus.Station;
                setValue.remark = (string)CurrentSession.GetValue(Session.SessionKeys.Remark);
                setValue.status = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
                setValue.editor = this.Editor;
                setValue.udt = DateTime.Now;

                mbRepository.UpdatePcboqcrepairInfo(setValue, cond);
                RepairID = repairLst[0].id;
            }
            else
            {
                PcboqcrepairInfo item = new PcboqcrepairInfo();
                item.pcbno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN);
                item.lotNo = (string)CurrentSession.GetValue(Session.SessionKeys.LotNo);
                item.station = mb.MBStatus.Station;
                item.remark = (string)CurrentSession.GetValue(Session.SessionKeys.Remark);
                item.status = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
                item.cdt = item.udt = DateTime.Now;
                item.editor = this.Editor;

                mbRepository.InsertPcboqcrepairInfo(item);
                RepairID = item.id;
            }




            Pcboqcrepair_DefectinfoInfo cond2 = new Pcboqcrepair_DefectinfoInfo();
            cond2.pcboqcrepairid = RepairID;

            IList<Pcboqcrepair_DefectinfoInfo> infoLst = new List<Pcboqcrepair_DefectinfoInfo>();
            infoLst = mbRepository.GetPcboqcrepairDefectinfoInfoList(cond2);
            if (infoLst != null && infoLst.Count > 0)
            {
                mbRepository.DeletePcboqcrepairDefectinfo(cond2);
            }


            IList<string> defectLst = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DefectList);
            foreach (string temp in defectLst)
            {
                Pcboqcrepair_DefectinfoInfo it = new Pcboqcrepair_DefectinfoInfo();
                it.defect = temp;
                it.cdt = DateTime.Now;
                it.editor = this.Editor;
                it.status = (string)CurrentSession.GetValue(Session.SessionKeys.VCode);
                it.pcboqcrepairid = RepairID;

                mbRepository.InsertPcboqcrepairDefectinfo(it);
            }

            return base.DoExecute(executionContext);
        }
    }
}
