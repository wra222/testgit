// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Update Lot表
// UC: UC-MES12-SPEC-SA-UC PCA OQC Output  
// UI: UI-MES12-SPEC-SA-UC PCA OQC Output                     
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-01   Chen Xu                      create
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
    /// Update Lot.Qty=Qty-1 where LotNo = (Select LotNo from PCBLot where PCBNo=@MBSN and Status=1)
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
    ///         Update Lot.Qty=Qty-1 where LotNo = (Select LotNo from PCBLot where PCBNo=@MBSN and Status=1)
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
	public partial class UpdateLot: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateLot()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Update Lot
        /// Update Lot.Qty=Qty-1 where LotNo = (Select LotNo from PCBLot where PCBNo=@MBSN and Status=1)
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mbsno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN);

            IList<string> pcblotnoList = new List<string>();

            pcblotnoList =(IList<string>) CurrentSession.GetValue(Session.SessionKeys.LotNoList);

            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            foreach (string ilotno in pcblotnoList)
            {
                LotInfo conLotInfo = new LotInfo();
                conLotInfo.lotNo = ilotno;

                LotInfo setLotInfo = new LotInfo();
                setLotInfo.qty = 1; // Qty赋要减的值
                setLotInfo.editor = Editor;
                setLotInfo.udt = DateTime.Now;

                iMBRepository.UpdateLotInfoForDecQtyDefered(CurrentSession.UnitOfWork, setLotInfo, conLotInfo);

                PcblotcheckInfo checkInfo = new PcblotcheckInfo();
                checkInfo.lotNo = ilotno;
                checkInfo.pcbno = mbsno;
                checkInfo.status = "0";
                checkInfo.editor = Editor;
                iMBRepository.InsertPcblotcheckInfoDefered(CurrentSession.UnitOfWork, checkInfo);
            }
            
            return base.DoExecute(executionContext);
        }

        
	}
}
