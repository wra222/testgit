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
using IMES.DataModel;

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
    public partial class CheckPrintLog : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public CheckPrintLog()
        {
            InitializeComponent();
        }
        //ITC-1360-1264
        /// <summary>
        /// 输入Print Log Name
        /// </summary>
        public static DependencyProperty PrintLogNameProperty = DependencyProperty.Register("PrintLogName", typeof(string), typeof(CheckPrintLog));

        /// <summary>
        /// 输入Print Log Name
        /// </summary>
        [DescriptionAttribute("PrintLogName")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string PrintLogName
        {
            get
            {
                return ((string)(base.GetValue(CheckPrintLog.PrintLogNameProperty)));
            }
            set
            {
                base.SetValue(CheckPrintLog.PrintLogNameProperty, value);
            }
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            if (CurrentProduct != null)
            {
                string prodid = CurrentProduct.ProId;
                bool bFlag = false;
                string name = "";
                if (String.IsNullOrEmpty(PrintLogName))
                {
                    name = "WWAN Label Print";
                    bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(name, prodid);
                    if (!bFlag)
                    {
                        bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(name, CurrentProduct.CUSTSN);
                    }
                }
                else
                {
                    name = PrintLogName;
                    if (name == "PrdId")//Travel Card Reprint
                    {
                        bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(name, CurrentProduct.MO);
                    }
                    else if (name == "WWAN Label Print")//ITCNDCheck Reprint
                    {
                        bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(name, prodid);
                        if (!bFlag)
                        {
                            bFlag = repository.CheckExistPrintLogByLabelNameAndDescr(name, CurrentProduct.CUSTSN);
                        }
                    }
                }

                if (!bFlag)
                {   //printLog
                    CurrentSession.AddValue(Session.SessionKeys.MaintainAction, "1");
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(prodid);
                    ex = new FisException("CHK844", erpara);
                    throw ex;
                }
                else
                {   //ReprintLog
                    if (name == "PrdId")
                    {
                        PrintLog cond = new PrintLog();
                        cond.Name = name;
                        cond.Descr = CurrentProduct.MO;
                        IList<PrintLog> list = new List<PrintLog>();
                        list = repository.GetPrintLogListByCondition(cond);
                        bool bFind = false;
                        foreach(PrintLog temp in list)
                        {
                            string beg = temp.BeginNo;
                            string end = temp.EndNo;

                            if((prodid.Substring(0, 3) == beg.Substring(0, 3)) && 
                                (prodid.Substring(0, 3) == end.Substring(0, 3)))
                            {
                                if ((Convert.ToInt64(prodid.Substring(3, 6)) >= Convert.ToInt64(beg.Substring(3, 6))) &&
                                    (Convert.ToInt64(prodid.Substring(3, 6)) <= Convert.ToInt64(end.Substring(3, 6))))
                                {
                                    bFind = true;
                                    break;
                                }
                            }
                        }
                        if (bFind == false)
                        {
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(prodid);
                            ex = new FisException("CHK843", erpara);
                            throw ex;
                        }
                        string reason = (string)CurrentSession.GetValue(Session.SessionKeys.Reason);
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogName, name);
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, prodid);
                        CurrentSession.AddValue(Session.SessionKeys.Reason, reason);
                        CurrentSession.AddValue(Session.SessionKeys.MaintainAction, "0");

                        IProductRepository ip = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                        string sLine = "";
                        ProductStatusInfo statusInfo = ip.GetProductStatusInfo(prodid);
                        if (statusInfo.pdLine != null)
                        {
                            sLine = statusInfo.pdLine;
                        }
                        CurrentSession.AddValue(Session.SessionKeys.LineCode, sLine);
                        CurrentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, prodid);
                    }
                    else if (name == "WWAN Label Print")
                    {
                        string reason = (string)CurrentSession.GetValue(Session.SessionKeys.Reason);
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogName, name);
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, prodid);
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, prodid);
                        CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, prodid);
                        CurrentSession.AddValue(Session.SessionKeys.Reason, reason);
                        CurrentSession.AddValue(Session.SessionKeys.MaintainAction, "0");

                        IProductRepository ip = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                        string sLine = "";
                        ProductStatusInfo statusInfo = ip.GetProductStatusInfo(prodid);
                        if (statusInfo.pdLine != null)
                        {
                            sLine = statusInfo.pdLine;
                        }
                        CurrentSession.AddValue(Session.SessionKeys.LineCode, sLine);
                        CurrentSession.AddValue(Session.SessionKeys.ProductIDOrCustSN, this.Key);
                    }
                }
            }

            return base.DoExecute(executionContext);
        }
    }
}