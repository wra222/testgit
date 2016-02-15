// INVENTEC corporation (c)2011 all rights reserved. 
// Description:保存各种类型的RePrintLog 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-03   Kerwin                       create
// Known issues:
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.ReprintLog;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.Activity
{
    /// <summary>
    /// 保存各种类型的PrintLog
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.将保存在指定Session中间变量中的log内容存入数据库;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Reason
    ///         Session.PrintLogDescr
    ///         Session.PrintLogBegNo
    ///         Session.PrintLogEndNo
    ///         Session.PrintLogName
    ///         Session.reason
    /// </para> 
    /// <para>    
    /// 中间变量：
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         insert RePrintLog
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPrintLogRepository
    /// </para> 
    /// </remarks>
    public partial class WriteRePrintLog : BaseActivity
    {


        /// <summary>
        /// constructor
        /// </summary>
        public WriteRePrintLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Write Reprintlog to db
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            var log = new ReprintLog
                           {
                               LabelName = session.GetValue(Session.SessionKeys.PrintLogName) as string,
                               BegNo = session.GetValue(Session.SessionKeys.PrintLogBegNo) as string,
                               EndNo = session.GetValue(Session.SessionKeys.PrintLogEndNo) as string,
                               Descr = session.GetValue(Session.SessionKeys.PrintLogDescr) as string,
                               Reason = "[" + this.Station + "] " + ((string)session.GetValue(Session.SessionKeys.Reason)) ?? "",
                               Editor = this.Editor
                           };

            var rep = RepositoryFactory.GetInstance().GetRepository<IReprintLogRepository, ReprintLog>();
            rep.Add(log, session.UnitOfWork);
            return base.DoExecute(executionContext);
        }
    }
}
