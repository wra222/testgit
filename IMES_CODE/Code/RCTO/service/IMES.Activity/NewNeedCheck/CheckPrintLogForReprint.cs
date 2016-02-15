/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckPrintLog
 * UI:CI-MES12-SPEC-FA-UI ITCND Check.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC ITCND Check.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-11   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using System.ComponentModel;
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Activity
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ITCND Check
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
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
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class CheckPrintLogForReprint : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public CheckPrintLogForReprint()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            Product newProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string printlabel = (string)CurrentSession.GetValue(Session.SessionKeys.PrintLogName);
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

            string prodid =newProduct.ProId;
            //string name = (string)CurrentSession.GetValue(Session.SessionKeys.PrintLogName);
            bool bFlag = false;
            if (printlabel == "ICASA label")
            {
                IList<string> tmp1 = bomRep.GetPnListByModelAndBomNodeType(newProduct.Model, "PL", "Anatel label");
                if (tmp1.Count > 0)
                {
                    printlabel = "Anatel label";
                }
                else
                {
                    printlabel = "ICASA label";
                }
            }

            bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(printlabel, prodid);
            if (!bFlag)
            {
                FisException ex;
                List<string> erpara = new List<string>();
                //erpara.Add("无打印记录,不能执行Reprint操作");
                ex = new FisException("CHK270", erpara);
                throw ex;
            }
            else
            {   //ReprintLog
                if (printlabel == "MasterLabel"){
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);
                }
                else{
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogName, printlabel);
                }
                CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, prodid);
            }

            return base.DoExecute(executionContext);
        }
    }
}