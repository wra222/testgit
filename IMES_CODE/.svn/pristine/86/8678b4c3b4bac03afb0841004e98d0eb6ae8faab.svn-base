// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 记录到ProductLog表
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   207006                       create
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
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;

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
    ///         应用于105Borrow and Return站
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///          
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
    ///         IProductRepository    
    /// </para> 
    /// </remarks>
    public partial class WriteBorrowLog : BaseActivity
    {
        ///<summary>
        /// 构造函数
        ///</summary>
        public WriteBorrowLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(WriteBorrowLog));

        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(WriteBorrowLog.StatusProperty)));
            }
            set
            {
                base.SetValue(WriteBorrowLog.StatusProperty, value);
            }
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                       

            var productObj = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            string borrower = (string)CurrentSession.GetValue(Session.SessionKeys.Borrower);

            IList<BorrowLog> borrowLog = productRepository.GetBorrowLogBySno(productObj.ProId);
            if (!(borrowLog == null || borrowLog.Count == 0))
            {
                foreach (BorrowLog bl in borrowLog)
                {
                    if (bl.Status.ToUpper() == "B")
                    {
                        var ex2 = new FisException("CHK125", new string[] { });
                        throw ex2;
                    }
                }
               
            }

            BorrowLog item = new BorrowLog();

            item.Status = this.Status;
            item.Sn = productObj.ProId;
            item.Model = productObj.Model;
            item.Borrower = borrower;
            item.Lender = this.Editor;

            productRepository.AddBorrowLogDefered(CurrentSession.UnitOfWork, item);
            return base.DoExecute(executionContext);
        }
    }
}
