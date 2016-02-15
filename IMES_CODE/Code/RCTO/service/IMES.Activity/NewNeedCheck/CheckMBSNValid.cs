/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for RCTO MB Change Page
* UI:CI-MES12-SPEC-SA-UI RCTO MB Change.docx –2012/6/15 
* UC:CI-MES12-SPEC-SA-UC RCTO MB Change.docx –2012/6/11            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2012-8-1    Jessica Liu           Create
* Known issues:
* TODO：
* ITC-1428-0010, Jessica Liu, 2012-9-7
*/

using System;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.Linq;
using IMES.FisObject.Common.PrintLog;


namespace IMES.Activity
{
    /// <summary>
    /// 检查MBSN是否正确
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      RCTO MB Change
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.若不存在PrintLog.Name = ’RCTO MB Label’ and PrintLog.BegNo=[MBSN]，则报错：“错误的MBSN”
    ///         2.填写RePrintLog打印内容
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.SessionKeys.MBSN
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
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              
    /// </para> 
    /// </remarks>
    public partial class CheckMBSNValid : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckMBSNValid()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// 检查MBSN是否正确
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mbSno = (string)CurrentSession.GetValue(Session.SessionKeys.MBSN);

            PrintLog condition = new PrintLog();
            condition.Name = "RCTO MB Label";
            condition.BeginNo = mbSno;

            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            IList<PrintLog> printLogList = repository.GetPrintLogListByCondition(condition);

            List<string> erpara = new List<string>();
            //ITC-1428-0010, Jessica Liu, 2012-9-7
            if (printLogList == null || printLogList.Count == 0)
            {
                erpara.Add(mbSno);
                throw new FisException("CHK904", erpara);      //错误的MBSN
            }

            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "RCTO MB Label");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, mbSno);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, mbSno);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "Reprint");

            return base.DoExecute(executionContext);
        }
    }
}
