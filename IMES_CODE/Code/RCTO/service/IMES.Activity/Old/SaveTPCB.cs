// INVENTEC corporation (c)2010 all rights reserved. 
// Description: 保存TPCB信息
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2010-04-15   Chen Xu (eB1-4)              create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.TPCB;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 保存TPCB信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         100 TPCBCollection
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          从Session获得Family，Pdline，PartNo检查以此三项作为该业务主键的数据在数据库TPCB中是否存在，
	///			 若存在，则Update该数据的VCode，Editor，Cdt；
	///          若不存在，则Save该新数据 TPCBInfo（Family, PdLine, Type, PartNo, Vendor, Dcode, Editor）
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///		    1.系统异常：
	///		    2.业务异常：     
    ///         
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.TPCBInfo （全部信息）
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///        Session.TPCBInfoLst
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///        Update TPCB
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         ITPCBInfoRepository
    ///         TPCB_Info
    /// </para> 
    /// </remarks>
    
    
    public partial class SaveTPCB: BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SaveTPCB()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 保存TPCB信息
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns>TPCBInfoLst</returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
     
            ITPCBInfoRepository TPCBInfoRepository = RepositoryFactory.GetInstance().GetRepository<ITPCBInfoRepository, TPCB_Info>();
            TPCBInfo CurrentTPCBInfo = new TPCBInfo();
            CurrentTPCBInfo.Family = (string)CurrentSession.GetValue(Session.SessionKeys.FamilyName);
            CurrentTPCBInfo.PdLine = (string)CurrentSession.GetValue(Session.SessionKeys.LineCode);
            CurrentTPCBInfo.Type = (string)CurrentSession.GetValue(Session.SessionKeys.TPCBType);
            CurrentTPCBInfo.PartNo = (string)CurrentSession.GetValue(Session.SessionKeys.PartNo);
            CurrentTPCBInfo.Vendor = (string)CurrentSession.GetValue(Session.SessionKeys.VendorSN); 
            CurrentTPCBInfo.DCode = (string)CurrentSession.GetValue(Session.SessionKeys.DCode); 
            CurrentTPCBInfo.Editor = (string)CurrentSession.GetValue(Session.SessionKeys.Editor);
            TPCBInfoRepository.SaveTPCB(CurrentTPCBInfo);
            //CurrentSession.AddValue(Session.SessionKeys.CreateDateTime, CurrentTPCBInfo.Cdt);   // add "cdt" as return value
            IList<TPCBInfo> ReturnCurrentTPCBInfoLst = TPCBInfoRepository.Query(CurrentTPCBInfo.Family, CurrentTPCBInfo.PdLine, CurrentTPCBInfo.PartNo);
            CurrentSession.AddValue(Session.SessionKeys.TPCBInfoLst, ReturnCurrentTPCBInfoLst);
            return base.DoExecute(executionContext);
        }
	}
}
