// INVENTEC corporation (c)201all rights reserved. 
// Description:  Insert [rpt_PCARep]
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-12  itc202017             (Reference Ebook SourceCode) Create
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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// Insert [rpt_PCARep]
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
    ///         A、	获取TP
    ///         B、	Insert [rpt_PCARep]
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
    ///         rpt_PCARep
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    /// </para> 
    /// </remarks>
    public partial class InsertRpt_PCARep : BaseActivity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsertRpt_PCARep()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Insert [rpt_PCARep]
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMB mb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            string mbStn = mb.MBStatus.Station;
            string tp = "";
            if (mbStn == "10" || mbStn == "15" || mbStn == "19") tp = "SA";
            if (mbStn == "25" || mbStn == "33" || mbStn == "33A") tp = "MP";

            MBRptRepair item = new MBRptRepair();
            item.Tp = tp;
            item.Remark = "~";
            item.Mark = "1";
            item.Status = "0";
            item.UserName = CurrentSession.Editor;
            mb.AddRptRepair(item);
            return base.DoExecute(executionContext);
        }

    }
}
