// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Update Lot,PcbLot表
// UC: UC-MES12-SPEC-SA-UC PCA OQC Input  
// UI: UI-MES12-SPEC-SA-UC PCA OQC Input                     
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-07-05   207003                    create
// Known issues:

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
    /// Update Lot.Qty=Qty+1 where LotNo =@LotNo
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      UC-MES12-SPEC-SA PCA OQC Output
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///       	若为不良品, Update Lot
    ///         Condition：
    ///         Update Lot.Qty=Qty+1 where LotNo =@LotNo
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.LotNo
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IMBRepository
    ///              IMB
    /// </para> 
    /// </remarks>
	public partial class UpdateLotForFAMBReturn: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateLotForFAMBReturn()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Update Lot,PCBLot
        /// Update Lot.Qty=Qty+1 where LotNo =@LotNo
        /// Update PCBLot.Status=1, Udt=GetDate() where ID=@ID
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB currentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
			IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

			PcblotInfo conPcblot = new PcblotInfo();
            conPcblot.pcbno = currentMB.Sn;
            conPcblot.status = "0";

            IList<PcblotInfo> pcblotLst = new List<PcblotInfo>();
            pcblotLst = currentMBRepository.GetPcblotInfoList(conPcblot);

            if (pcblotLst == null || pcblotLst.Count<=0)
			{
                //若@LotNo为空或者NULL，则不需要进行下面的操作，和贯伟确认，pcblotLst为空，不做操作
			}
			else
			{
                foreach (PcblotInfo ipcblot in pcblotLst)
                {
                    LotInfo conLotInfo = new LotInfo();
                    conLotInfo.lotNo = ipcblot.lotNo;

                    //Update Lot.Qty=Qty+1 where LotNo =@LotNo
                    LotInfo setLotInfo = new LotInfo();
                    setLotInfo.qty = 1; // Qty赋要增的值
                    setLotInfo.editor = Editor;
                    setLotInfo.udt = DateTime.Now;
                    currentMBRepository.UpdateLotInfoForIncQtyDefered(CurrentSession.UnitOfWork, setLotInfo, conLotInfo);

                    //Update PCBLot.Status=1, Udt=GetDate() where ID=@ID
                    // 条件
                    PcblotInfo consetPcblot = new PcblotInfo();
                    consetPcblot.id = ipcblot.id;
                    // 赋值
                    PcblotInfo setPcblot = new PcblotInfo();
                    setPcblot.status = "1";
                    setPcblot.editor = Editor;
                    setPcblot.udt = DateTime.Now;
                    currentMBRepository.UpdatePCBLotInfoDefered(CurrentSession.UnitOfWork, setPcblot, consetPcblot);
                    break;
                }
               
			}
            return base.DoExecute(executionContext);
        }
	}
}
