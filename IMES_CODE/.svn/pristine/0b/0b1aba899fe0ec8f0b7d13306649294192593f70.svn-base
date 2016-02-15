// INVENTEC corporation (c)201all rights reserved. 
// Description:  If WC='33' Insert [MTA_Mark]
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-12  itc202017             (Reference Ebook SourceCode) Create
// Known issues:
using System;
using System.Collections.Generic;
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
using IMES.DataModel;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// If WC='33' Insert [MTA_Mark]
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      PCA Repair Input
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         若 WC = ‘33’，则 Insert [MTA_Mark]
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         无
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         MTA_Mark
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class InsertMTAMark : BaseActivity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsertMTAMark()
        {
            InitializeComponent();
        }

        /// <summary>
        /// If WC='33' Insert [MTA_Mark]
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            /*
             * Answer to: ITC-1360-0198
             * Description: Insert MTA Mark table if WC=33.
             */
            IMBRepository mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB mb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            if (mb.MBStatus.Station == "33")
            {
                IList<Repair> repairList = mb.Repairs;
                foreach (Repair ele in repairList)
                {
                    if (ele.Station == "33")
                    {
                        IList<RepairDefect> defectList = ele.Defects;
                        foreach (RepairDefect tmp in defectList)
                        {
                            MtaMarkInfo item = new MtaMarkInfo();
                            item.rep_Id = tmp.ID;
                            item.mark = "0";
                            mbRepository.InsertMtaMarkInfoDefered(CurrentSession.UnitOfWork, item);
                        }
                    }
                }
            }
            return base.DoExecute(executionContext);
        }

    }
}
