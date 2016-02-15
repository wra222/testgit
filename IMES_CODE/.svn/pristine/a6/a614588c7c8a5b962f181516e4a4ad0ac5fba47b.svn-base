// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Update PCBLot表
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
    /// Update PCBLot.Status=0 where Status=1 and PCBNo=@MBSN；
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
    ///       	若为不良品, Update PCBLot
    ///         Condition：
    ///         Update PCBLot.Status=0 where Status=1 and PCBNo=@MBSN；
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MBSN
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.LotNo
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
	public partial class UpdatePCBLot: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdatePCBLot()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Update PCBLot
        /// Update PCBLot.Status=0 where Status=1 and PCBNo=@MBSN；
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mbsno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN);

            IMBRepository iMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
           
            // 条件
            PcblotInfo conPcblot = new PcblotInfo();
            conPcblot.pcbno = mbsno;
            conPcblot.status = "1";

            IList<PcblotInfo> pcblotLst = new List<PcblotInfo>();
            pcblotLst =iMBRepository.GetPcblotInfoList(conPcblot);
            IList<string> pcblotnoList = new List<string>();
            foreach (PcblotInfo ipcblot in pcblotLst)
            {
                pcblotnoList.Add(ipcblot.lotNo);
            }

            // 赋值
            PcblotInfo setPcblot = new PcblotInfo();
            setPcblot.status = "0";
            setPcblot.editor = Editor;
            setPcblot.udt = DateTime.Now;

            iMBRepository.UpdatePCBLotInfoDefered(CurrentSession.UnitOfWork, setPcblot, conPcblot);

            CurrentSession.AddValue(Session.SessionKeys.LotNoList, pcblotnoList);
           
            return base.DoExecute(executionContext);
        }

        
	}
}
