// INVENTEC corporation (c)2012 all rights reserved. 
// Description:
//      Insert  BorrowLog
//                   
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-10   Kerwin                       create
// Known issues:
using System.Collections.Generic;
using System.Linq;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PCA.MB;
using System.ComponentModel;
using IMES.DataModel;
using System;

namespace IMES.Activity
{

    /// <summary>
    /// 用于记录到Borrow Log
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///          CI-MES12-SPEC-SA-UC MB Borrow Control
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    /// </para>      
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    ///         Session.Borrower
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///        
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         borrow Log
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository    
    /// </para> 
    /// </remarks>
    public partial class InsertBorrowLog : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public InsertBorrowLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// insert borrow log
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {

            var CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            string model = "10";

            if (currenMB == null)
            {
                model = (string)CurrentSession.GetValue(Session.SessionKeys.ModelName);
            }
            else
            {
                if (currenMB.MBStatus.Station == "P0" || currenMB.MBStatus.Station == "09")
                {
                    model = currenMB.MBStatus.Station;
                }
            }
            
            string borrower = (string)CurrentSession.GetValue(Session.SessionKeys.Borrower);

            IList<BorrowLog> borrowLog = CurrentRepository.GetBorrowLogBySno(Key, "B");
            if (borrowLog != null && borrowLog.Count > 0)
            {
                var ex2 = new FisException("CHK125", new string[] { });
                throw ex2;
            }

            BorrowLog item = new BorrowLog();
            item.ID = int.MinValue;
            item.Status = this.Status;
            item.Sn = Key;
            item.Model = model;
            item.Borrower = borrower;
            item.Lender = this.Editor;
            item.Returner = "";
            item.Acceptor = "";
            item.Bdate = DateTime.Now;

            var CurrentProductRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            CurrentProductRepository.AddBorrowLogDefered(CurrentSession.UnitOfWork, item);
            return base.DoExecute(executionContext);
        }


        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(InsertBorrowLog));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(InsertBorrowLog.StatusProperty)));
            }
            set
            {
                base.SetValue(InsertBorrowLog.StatusProperty, value);
            }
        }

    }
}

