// INVENTEC corporation (c)2011all rights reserved. 
// Description:  类似于.net提供的TransactionScopeTransactionScopeActivity, 用于定义数据库事务的范围，
//                  将包含于其中的Activity中的数据库操作作为一个事务提交，与TransactionScopeActivity的
//                  的区别在于，事务在该Activity结束时才开始，而不是一开始就打开事务。
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-06   YangWeihua              create
using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using IMES.Infrastructure;

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
    public partial class DBScope : SequenceActivity
    {
        /// <summary>
        /// Key of Session
        /// </summary>
        public static DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(DBScope));

        /// <summary>
        /// Key of Session
        /// </summary>
        [DescriptionAttribute("Key")]
        [CategoryAttribute("DBScopeArguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Key
        {
            get
            {
                return ((string)(base.GetValue(DBScope.KeyProperty)));
            }
            set
            {
                base.SetValue(DBScope.KeyProperty, value);
            }
        }

        /// <summary>
        /// Station
        /// </summary>
        public static DependencyProperty StationProperty = DependencyProperty.Register("Station", typeof(string), typeof(DBScope));

        /// <summary>
        /// wc
        /// </summary>
        [DescriptionAttribute("Station")]
        [CategoryAttribute("DBScopeArguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Station
        {
            get
            {
                return ((string)(base.GetValue(DBScope.StationProperty)));
            }
            set
            {
                base.SetValue(DBScope.StationProperty, value);
            }
        }

        /// <summary>
        /// Type of Session
        /// </summary>
        public static DependencyProperty SessionTypeProperty = DependencyProperty.Register("SessionType", typeof(int), typeof(DBScope));

        /// <summary>
        /// Type of Session
        /// </summary>
        [DescriptionAttribute("SessionType")]
        [CategoryAttribute("DBScopeArguments")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int SessionType
        {
            get
            {
                return ((int)(base.GetValue(DBScope.SessionTypeProperty)));
            }
            set
            {
                base.SetValue(DBScope.SessionTypeProperty, value);
            }
        }

        ///<summary>
        /// Constructor
        ///</summary>
        public DBScope()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        protected override void OnSequenceComplete(ActivityExecutionContext executionContext)
        {
            //get session
            Session session = null;
            session = SessionManager.GetInstance.GetSession(Key, (Session.SessionType)Enum.Parse(typeof(Session.SessionType), SessionType.ToString()));

            //skip
            if (session != null && session.Exception == null)
            {
                //get session UOW
                var uow = session.UnitOfWork;
                uow.Commit();
                //commit transactionscope
            }
            base.OnSequenceComplete(executionContext);
        }

    }

}
