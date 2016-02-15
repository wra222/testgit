// INVENTEC corporation (c)2011 all rights reserved. 
// Description:保存各种类型的PrintLog 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-11-03   Kerwin                       create
// Known issues:
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.PrintLog;
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
    ///      应用于各个批量产生序号的站, 记录产生序号的Log
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.产生序号的Activity,记录Session.PrintLogDescr,Session.PrintLogBegNo,Session.PrintLogEndNo保存在Session变量中
    ///         2.WritePrintLog将保存在Session中间变量中的log内容存入数据库;
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.PrintLogDescr
    ///         Session.PrintLogBegNo
    ///         Session.PrintLogEndNo
    ///         Session.PrintLogName
    ///         
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///        无 
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         insert PrintLog
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IPrintLogRepository
    /// </para> 
    /// </remarks>
    public partial class WritePrintLog : BaseActivity
    {

        /// <summary>
        /// constructor
        /// </summary>
        public WritePrintLog()
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
 
            var item = new PrintLog
                           {
                               Name = CurrentSession.GetValue(Session.SessionKeys.PrintLogName).ToString(),
                               BeginNo = CurrentSession.GetValue(Session.SessionKeys.PrintLogBegNo).ToString(),
                               EndNo = CurrentSession.GetValue(Session.SessionKeys.PrintLogEndNo).ToString(),
                               Descr = CurrentSession.GetValue(Session.SessionKeys.PrintLogDescr).ToString(),
                               Editor = this.Editor
                           };
            item.Station = this.Station??"";
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            repository.Add(item, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);
        }
    }
}