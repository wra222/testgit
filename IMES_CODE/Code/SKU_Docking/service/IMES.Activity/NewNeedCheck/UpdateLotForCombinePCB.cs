// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Update Lot表
// UC: UC-MES12-SPEC-SA-UC CombinePCBforLot  
// UI: UI-MES12-SPEC-SA-UI CombinePCBforLot                      
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-07-11   Kaisheng,Zhang               create
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
    using IMES.FisObject.Common.TestLog;

    /// <summary>
    /// Update Lot.Qty=Qty+1 where LotNo = (Select LotNo from PCBLot where PCBNo=@MBSN and Status=1)
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      UC-MES12-SPEC-SA-UC CombinePCBforLot 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         Update Lot
    ///         Condition：
    ///         Update Lot.Qty=Qty+1 where LotNo = (Select LotNo from PCBLot where PCBNo=@MBSN and Status=1)
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
	public partial class UpdateLotForCombinePCB: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateLotForCombinePCB()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Update Lot
        /// Update Lot.Qty=Qty+1 where LotNo = (Select LotNo from PCBLot where PCBNo=@MBSN and Status=1)
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //string mbsno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN);
            var mb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            //FisException ex;
            List<string> erpara = new List<string>();

            string mbsno = mb.Sn;
            string SelectLotNo =(string)CurrentSession.GetValue(Session.SessionKeys.LotNo);

            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
           
            

            LotInfo conLotInfo = new LotInfo();
            conLotInfo.lotNo = SelectLotNo;

            LotInfo setLotInfo = new LotInfo();
            setLotInfo.qty = 1; // Qty赋要加的值
            setLotInfo.editor = Editor;
            setLotInfo.udt = DateTime.Now;

            iMBRepository.UpdateLotInfoForIncQtyDefered(CurrentSession.UnitOfWork, setLotInfo, conLotInfo);

            PcblotInfo checkInfo = new PcblotInfo();
            checkInfo.lotNo = SelectLotNo;
            checkInfo.pcbno = mbsno;
            checkInfo.status = "1";
            checkInfo.editor = Editor;
            //iMBRepository.InsertPcblotcheckInfoDefered(CurrentSession.UnitOfWork, checkInfo);
            iMBRepository.InsertPCBLotInfoDefered(CurrentSession.UnitOfWork, checkInfo);
            TestLog tItem;
            tItem = new TestLog(0, mbsno, this.Line, null, "15", new List<TestLogDefect>(), TestLog.TestLogStatus.Pass,"" , null,null,"Combine PCB In Lot",this.Editor, "MB", DateTime.Now);
            mb.AddTestLog(tItem);
            iMBRepository.Update(mb, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }

        
	}
}
