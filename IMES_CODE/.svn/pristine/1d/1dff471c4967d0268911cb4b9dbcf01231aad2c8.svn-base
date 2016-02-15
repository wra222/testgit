// INVENTEC corporation (c)2012 all rights reserved. 
// Description:检查MB是否已经打印了标签 
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-13   Kerwin                       create
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.EcrVersion;
using IMES.FisObject.Common.PrintLog;

namespace IMES.Activity
{

    /// <summary>
    /// 检查MB是否已经打印了标签
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      CI-MES12-SPEC-SA-UC PCA ICT Input
    ///      CI-MES12-SPEC-SA-UC PCA ICT Input For Docking
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///     select  BegNo  from IMES2012_GetData..PrintLog where @MBSno between BegNo and EndNo and Name='MB'
    ///     若不存在记录，则报错：“沒有該MB的打印紀錄,請確認該MB序號是否正確
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
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         MB
    ///         
    /// </para> 
    /// </remarks>
    public partial class CheckExistPrintLog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckExistPrintLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 检查MB是否已经打印了标签
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;

            var CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();

            //Vincent for performance reason
            //IList<PrintLog> PrintLogList = CurrentRepository.GetPrintLogListByRange(currenMB.Sn, "MB");
            //if (PrintLogList == null || PrintLogList.Count == 0)

             if(!CurrentRepository.CheckPrintLogListByRange(currenMB.Sn, "MB"))  
            {
                throw new FisException("ICT002", new string[] { currenMB.Sn });
            }
            return base.DoExecute(executionContext);
        }

    }
}

