using System;
using System.Collections.Generic;
using System.Collections;
using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.DataModel;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Defect;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckAndGetMBInfo : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckAndGetMBInfo()
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

            //若该MB的PCBStatus.Status=0，则报错：“MBSN：XXX已经存在不良，请去修护区修护”
            if (mb.MBStatus.Status == MBStatusEnum.Fail)
            {
                List<string> errpara = new List<string>();
                errpara.Add(mb.Sn);
                throw new FisException("CHK905", errpara);
            }

            //若该MB存在PCBRepair.Status=0的记录，则报错：“请先Key出修护的MB，再刷画面”
            IList<Repair> repairs = new List<Repair>();
            repairs = mb.Repairs;
            foreach (Repair temp in repairs)
            {
                if (temp.Status == Repair.RepairStatus.NotFinished)
                {
                    List<string> errpara = new List<string>();
                    throw new FisException("CHK907", errpara);
                }
            }


            //若该MB的PCBStatus.Station 非（15或者16或者17或者31A或者31或者30），则报错：“MBSN：XXX不该出现在Q区”
            if (!(mb.MBStatus.Station == "15" || mb.MBStatus.Station == "16"
                || mb.MBStatus.Station == "17" || mb.MBStatus.Station == "31A"
                || mb.MBStatus.Station == "31" || mb.MBStatus.Station == "30"))
            {
                List<string> errpara = new List<string>();
                errpara.Add(mb.Sn);
                throw new FisException("CHK906", errpara);
            }


            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            //[Station] = PCBStatus.Station + ‘  ’ + Station.Descr
            IList<string> stationLst = new List<string>();
            stationLst = mbRepository.GetStationListFromPcbStatus(mb.Sn);
            if (stationLst != null && stationLst.Count > 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.StationDescr, stationLst[0]);
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.StationDescr, "");
            }

            //[LotNo] = Top 1 PCBLot.LotNo where PCBNo=@MBSn and Status=1 order by Cdt desc
            IList<PcblotInfo> lots = new List<PcblotInfo>();
            PcblotInfo cond = new PcblotInfo();
            cond.pcbno = mb.Sn;
            cond.status = "1";
            lots = mbRepository.GetPcblotInfoList(cond);
            if (lots != null && lots.Count > 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.LotNo, lots[0].lotNo);
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.LotNo, "");
            }


            //[PdLine]=PCBStatus.Line + ‘  ’ +Line.Descr
            IList<string> lineLst = new List<string>();
            lineLst = mbRepository.GetLineListFromPcbStatus(mb.Sn);
            if (lineLst != null && lineLst.Count > 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.LineCode, lineLst[0]);
            }
            else
            {
                CurrentSession.AddValue(Session.SessionKeys.LineCode, "");
            }

            //若PCBOQCRepair中存在（Status=0 and PCBNo=@MBSN order by Cdt desc），获取Top 1数据，显示如下:
            IList<PcboqcrepairInfo> repairLst = new List<PcboqcrepairInfo>();
            PcboqcrepairInfo cond1 = new PcboqcrepairInfo();
            cond1.status = "0";
            cond1.pcbno = mb.Sn;
            repairLst = mbRepository.GetPcboqcrepairInfoListOrderByCdtDesc(cond1);
            if (repairLst != null && repairLst.Count > 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.Remark, repairLst[0].remark);
                CurrentSession.AddValue(Session.SessionKeys.RepairDefectID, repairLst[0].id);

                //获取如下数据，显示在Defect List
                //Select * from PCBOQCRepair_DefectInfo where PCBOQCRepairID = @PCBOQCRepairID order by Cdt 
                IList<Pcboqcrepair_DefectinfoInfo> infoLst = new List<Pcboqcrepair_DefectinfoInfo>();
                Pcboqcrepair_DefectinfoInfo cond2 = new Pcboqcrepair_DefectinfoInfo();
                cond2.pcboqcrepairid = repairLst[0].id;
                infoLst = mbRepository.GetPcboqcrepairDefectinfoInfoList(cond2);


                IDefectRepository defectRepository = RepositoryFactory.GetInstance().GetRepository<IDefectRepository, Defect>();
                IList<string> defectLst = new List<string>();
                IList<string> descLst = new List<string>();
                foreach (Pcboqcrepair_DefectinfoInfo temp in infoLst)
                {
                    defectLst.Add(temp.defect);
                    IList<DefectCodeInfo> defectcodeLst = new List<DefectCodeInfo>();
                    DefectCodeInfo cond3 = new DefectCodeInfo();
                    cond3.Defect = temp.defect;
                    defectcodeLst = defectRepository.GetDefectCodeInfoList(cond3);
                    if (defectcodeLst != null && defectcodeLst.Count > 0)
                    {
                        descLst.Add(defectcodeLst[0].Descr);
                    }
                    else
                    {
                        descLst.Add("");
                    }
                }
                CurrentSession.AddValue("DefectLst", defectLst);
                CurrentSession.AddValue("DescLst", descLst);
            }
            else
            {
                IList<string> defectLst = new List<string>();
                IList<string> descLst = new List<string>();
                CurrentSession.AddValue(Session.SessionKeys.Remark, "");
                CurrentSession.AddValue(Session.SessionKeys.RepairDefectID, -1);
                CurrentSession.AddValue("DefectLst", defectLst);
                CurrentSession.AddValue("DescLst", descLst);
            }


            return base.DoExecute(executionContext);
        }
    }
}
