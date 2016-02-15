/*
 * INVENTEC corporation (c)2012 all rights reserved. 
 * Description: RCTO Change Single
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2012-07-16   Kerwin            Create 
 * 
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.DataModel;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Utility.Generates.intf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Utility.Generates.impl;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.PAK.Pallet;


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
    ///         CI-MES12-SPEC-SA-UC PCA ICT Input For RCTO 非链板
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         对Session.MB
    ///             1.修改MBSn
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
    ///         Session.MB
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
    ///         Update PCB/PCBInfo/PCB_Part/PCBStatus/PCBLog/PCBTestLog Set PCBNo = @RCTOMBSn where PCBNo= @MBSn
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class PalletSaveForRCTO : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public PalletSaveForRCTO()
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

            string PalletNo =(string) CurrentSession.GetValue(Session.SessionKeys.Pallet) ;
            
            IPalletRepository PalletRepository = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            Pallet CurrentPallet = PalletRepository.Find(PalletNo);
            CurrentPallet.Editor = Editor;

            PalletLog newPalletLog = new PalletLog();
            newPalletLog.Editor = Editor;
            newPalletLog.Line = "";
            newPalletLog.Station = Station;
            CurrentPallet.AddLog(newPalletLog);

            CurrentPallet.Station = Station;
            CurrentPallet.Editor = Editor;


            PalletRepository.UpdatePakLocMasForPno("", PalletNo, "PakLoc");
            PalletRepository.Update(CurrentPallet, CurrentSession.UnitOfWork);

            
            return base.DoExecute(executionContext);
        }

    
    }
}
