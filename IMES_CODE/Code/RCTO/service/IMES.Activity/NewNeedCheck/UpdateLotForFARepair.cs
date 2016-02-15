using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateLotForFARepair : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public UpdateLotForFARepair()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            PcblotInfo conPcblot = new PcblotInfo();
            conPcblot.pcbno = currentMB.Sn;
            conPcblot.status = "1";

            IList<PcblotInfo> pcblotLst = new List<PcblotInfo>();
            pcblotLst = currentMBRepository.GetPcblotInfoList(conPcblot);
            foreach (PcblotInfo ipcblot in pcblotLst)
            {
                LotInfo conLotInfo = new LotInfo();
                conLotInfo.lotNo = ipcblot.lotNo;

                LotInfo setLotInfo = new LotInfo();
                setLotInfo.qty = 1;
                setLotInfo.editor = Editor;
                setLotInfo.udt = DateTime.Now;
                currentMBRepository.UpdateLotInfoForDecQtyDefered(CurrentSession.UnitOfWork, setLotInfo, conLotInfo);
            }

            if (pcblotLst == null || pcblotLst.Count <= 0)
            {
                //若@LotNo为空或者NULL，则不需要进行下面的操作，和贯伟确认，pcblotLst为空，不做操作
            }
            else
            {
                //Update PCBLot.Status=0 where Status=1 and PCBNo=@MBSN
                // 条件
                PcblotInfo consetPcblot = new PcblotInfo();
                consetPcblot.pcbno = currentMB.Sn;
                consetPcblot.status = "1";
                // 赋值
                PcblotInfo setPcblot = new PcblotInfo();
                setPcblot.status = "0";
                setPcblot.editor = Editor;
                setPcblot.udt = DateTime.Now;
                currentMBRepository.UpdatePCBLotInfoDefered(CurrentSession.UnitOfWork, setPcblot, consetPcblot);
            }
            return base.DoExecute(executionContext);
        }
    }
}