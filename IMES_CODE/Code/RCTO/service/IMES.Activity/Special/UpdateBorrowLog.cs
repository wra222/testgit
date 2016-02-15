// INVENTEC corporation (c)2012 all rights reserved. 
// Description: Update BorrowLog
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-12   Kerwin                       create
// 2012-01-20   Kerwin                       ITC-1360-150,ITC-1360-154,ITC-1360-160
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
using IMES.FisObject.PCA.MB;

namespace IMES.Activity
{

    /// <summary>
    /// 用于更新Borrow Log
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         CI-MES12-SPEC-SA-UC MB Borrow Control
    ///         
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    ///         Session.Lender
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
    public partial class UpdateBorrowLog : BaseActivity
    {
        ///<summary>
        /// 构造函数
        ///</summary>
        public UpdateBorrowLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Activtiy所在Status
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(UpdateBorrowLog));

        /// <summary>
        /// BorrowLog Status
        /// </summary>
        [DescriptionAttribute("Status")]
        [CategoryAttribute("Status Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(UpdateBorrowLog.StatusProperty)));
            }
            set
            {
                base.SetValue(UpdateBorrowLog.StatusProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var CurrentRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();

            string Returner = CurrentSession.GetValue(Session.SessionKeys.Borrower) as string;

            IList<BorrowLog> borrowLogList = CurrentRepository.GetBorrowLogBySno(Key, "B");
            if (borrowLogList != null && borrowLogList.Count > 0)
            {
                CurrentSession.AddValue(Session.SessionKeys.ReturnWC, borrowLogList[0].Model);
            }
            else
            {
                var ex2 = new FisException("CHK126", new string[] { });
                throw ex2;
            }

            BorrowLog item = new BorrowLog();

            item.Status = this.Status;
            item.Sn = Key;
            item.Returner = Returner;
            item.Acceptor = this.Editor;
            item.Rdate = DateTime.Now;
            CurrentRepository.UpdateBorrowLogDefered(CurrentSession.UnitOfWork, item,"B");
            return base.DoExecute(executionContext);
        }
    }
}
