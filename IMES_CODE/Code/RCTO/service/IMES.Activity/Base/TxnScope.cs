// INVENTEC corporation (c)2011all rights reserved. 
// Description:  类似于.net提供的TransactionScopeTransactionScopeActivity, 用于定义数据库事务的范围，
//                  将包含于其中的Activity中的数据库操作作为一个事务提交，与TransactionScopeActivity的
//                  的区别在于，事务在该Activity结束时才开始，而不是一开始就打开事务。
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-15   Vincent              create
//2013-03-13    Vincent           release
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    ///  类似于.net提供的TransactionScopeTransactionScopeActivity, 用于定义数据库事务的范围，
    ///  将包含于其中的Activity中的数据库操作作为一个事务提交，与TransactionScopeActivity的
    ///  的区别在于，事务在该Activity结束时才开始，而不是一开始就打开事务。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         SequenceActivity
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于各个执行写库操作且需要应用事务的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.获取session.UnitOfWork变量;
    ///         2.执行UnitOfWork.Commit()
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
    ///         依赖各站业务
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         无
    /// </para> 
    /// </remarks>
    public partial class TxnScope : SequenceActivity
    {
        /// <summary>
        /// Key of Session
        /// </summary>
        public static DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(TxnScope));

        /// <summary>
        /// Key of Session
        /// </summary>
        [DescriptionAttribute("Key")]
        [CategoryAttribute("TxnScopeArguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Key
        {
            get
            {
                return ((string)(base.GetValue(TxnScope.KeyProperty)));
            }
            set
            {
                base.SetValue(TxnScope.KeyProperty, value);
            }
        }

        /// <summary>
        /// Station
        /// </summary>
        public static DependencyProperty StationProperty = DependencyProperty.Register("Station", typeof(string), typeof(TxnScope));

        /// <summary>
        /// wc
        /// </summary>
        [DescriptionAttribute("Station")]
        [CategoryAttribute("TxnScopeArguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Station
        {
            get
            {
                return ((string)(base.GetValue(TxnScope.StationProperty)));
            }
            set
            {
                base.SetValue(TxnScope.StationProperty, value);
            }
        }

        /// <summary>
        /// Type of Session
        /// </summary>
        public static DependencyProperty SessionTypeProperty = DependencyProperty.Register("SessionType", typeof(int), typeof(TxnScope));

        /// <summary>
        /// Type of Session
        /// </summary>
        [DescriptionAttribute("SessionType")]
        [CategoryAttribute("TxnScopeArguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int SessionType
        {
            get
            {
                return ((int)(base.GetValue(TxnScope.SessionTypeProperty)));
            }
            set
            {
                base.SetValue(TxnScope.SessionTypeProperty, value);
            }
        }

        ///<summary>
        /// Constructor
        ///</summary>
        public TxnScope()
        {
            InitializeComponent();
        }

        //protected override ActivityExecutionStatus Cancel(ActivityExecutionContext executionContext)
        //{

        //    return base.Cancel(executionContext);
        //}

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="executionContext"></param>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SqlTransactionManager.Begin();
            return base.Execute(executionContext);
        }

        /// <summary>
        /// HandleFault
        /// </summary>        
        protected override ActivityExecutionStatus HandleFault(ActivityExecutionContext executionContext, Exception exception)
        {
            try
            {
                SqlTransactionManager.Rollback();
                SqlTransactionManager.Dispose();
                return base.HandleFault(executionContext, exception);
            }
            catch
            {
                return base.HandleFault(executionContext, exception);
            }
            finally
            {
                //int count = SqlTransactionManager.GetCurrentSqlTransactionManagerInfo().Embeded;
                //for (int i = 0; i < count; i++)
                //{
                //    SqlTransactionManager.End();
                //}
                this.ResetTxnCount(0);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        protected override void OnSequenceComplete(ActivityExecutionContext executionContext)
        {
            try
            {
                //get session
                Session session = null;
                session = SessionManager.GetInstance.GetSession(Key, (Session.SessionType)Enum.Parse(typeof(Session.SessionType), SessionType.ToString()));

                //none skip then comit uow session
                if (session != null && session.Exception == null)
                {
                    //get session UOW
                    var uow = session.UnitOfWork;
                    uow.Commit();
                    //commit transactionscope
                }

                //int count = SqlTransactionManager.GetCurrentSqlTransactionManagerInfo().Embeded;

                //for (int i = 1; i < count; i++)
                //{
                //    SqlTransactionManager.End();
                //}
                this.ResetTxnCount(1);

                if (session != null && session.Exception == null)
                {
                    SqlTransactionManager.Commit();
                }
                else  //Skip Activity need rollback
                {
                    SqlTransactionManager.Rollback();
                }

            }
            catch (Exception e)
            {
                //int count=SqlTransactionManager.GetCurrentSqlTransactionManagerInfo().Embeded;
                
                //for (int i = 1; i < count; i++)
                //{
                //     SqlTransactionManager.End();
                //}
                this.ResetTxnCount(1);
                SqlTransactionManager.Rollback();
                throw e;
            }
            finally
            {
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
            }
            base.OnSequenceComplete(executionContext);
        }

        private void ResetTxnCount(int remainQty)
        {
            int count = SqlTransactionManager.GetCurrentSqlTransactionManagerInfo().Embeded;
            for (int i = remainQty; i < count; i++)
            {
                SqlTransactionManager.End();
            }
        }

        
    }

}
