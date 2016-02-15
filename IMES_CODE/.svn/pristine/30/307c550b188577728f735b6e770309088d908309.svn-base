/*
* INVENTEC corporation ?2011 all rights reserved. 
* Description:Activity for Online Generate AST Page
* UI:CI-MES12-SPEC-FA-UI Online Generate AST .docx –2012/4/9 
* UC:CI-MES12-SPECFA-UC Online Generate AST .docx –2012/4/9            
* Update: 
* Date        Name                  Reason 
* ==========  ===================== =====================================
* 2011-11-21   Jessica Liu           (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* ITC-1360-1619, Jessica Liu, 2012-4-9
* ITC-1413-0012, Jessica Liu, 2012-6-15
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
    /// 检查是否打印过AST Label
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      Online Generate AST
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.生成序号
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：CHK206
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.custsn
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.PrintLogDescr
    ///         Session.PrintLogBegNo
    ///         Session.PrintLogEndNo
    ///         Session.PrintLogName
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IPrintLogRepository
    ///              PrintLog
    /// </para> 
    /// </remarks>
    public partial class CheckASTReprint : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckASTReprint()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 产生Asset SN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var currenProduct = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            var Custsn = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);

            PrintLog condition = new PrintLog();
            condition.Name = "AT";
            condition.BeginNo = currenProduct.ProId;
            //ITC-1360-1619, Jessica Liu, 2012-4-9
            //condition.Descr = "ATSN3";
            //ITC-1413-0012, Jessica Liu, 2012-6-21
            //condition.Descr = "ATSN";

            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            IList<PrintLog> printLogList = repository.GetPrintLogListByCondition(condition);

            //FisException ex;
            List<string> erpara = new List<string>();
            if (printLogList.Count == 0)
            {
                erpara.Add(Custsn);
                throw new FisException("CHK206", erpara);
            }
            CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, currenProduct.ProId);

            //ITC-1413-0012, Jessica Liu, 2012-6-15
            /*
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, printLogList[0].EndNo);

            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "AT");
            
            //ITC-1360-1619, Jessica Liu, 2012-4-9
            //CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN3");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, "ATSN");
            */
            CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, currenProduct.ProId);
            CurrentSession.AddValue(Session.SessionKeys.PrintLogName, "AT");
            CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, printLogList[0].EndNo);

            return base.DoExecute(executionContext);
        }
    }
}
