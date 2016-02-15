/* INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Activity基类
 *                         
 * Update: 
 * Date         Name                         Reason 
 * ==========   =======================      ==========================
 * 2009-11-26   Yang.Weihua, LiuDong                implement DoExecute method
 * Known issues:
 * Updates:
 *  2010-2-4 fix bug ITC-1103-0171
 */

using System;
using System.ComponentModel;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using log4net;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    /// Activity的基类，提供系统中所有Activity的公共属性和行为
    /// </summary>
    public partial class BaseActivity : System.Workflow.ComponentModel.Activity
    {

        //private Session session = null;

        /// <summary>
        /// Session 键值, MBSn/ProductID/...
        /// </summary>
        public static DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(BaseActivity));


        /// <summary>
        /// Session 键值, MBSn/ProductID/...
        /// </summary>
        [DescriptionAttribute("Key")]
        [CategoryAttribute("InArguments Of Base")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Key
        {
            get
            {
                return ((string)(base.GetValue(BaseActivity.KeyProperty)));
            }
            set
            {
                base.SetValue(BaseActivity.KeyProperty, value);
            }
        }

        /// <summary>
        /// Activtiy所在Station
        /// </summary>
        public static DependencyProperty StationProperty = DependencyProperty.Register("Station", typeof(string), typeof(BaseActivity));

        /// <summary>
        /// Activtiy所在Station
        /// </summary>
        [DescriptionAttribute("Station")]
        [CategoryAttribute("InArguments Of Base")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Station
        {
            get
            {
                return ((string)(base.GetValue(BaseActivity.StationProperty)));
            }
            set
            {
                base.SetValue(BaseActivity.StationProperty, value);
            }
        }

        /// <summary>
        /// 线别
        /// </summary>
        public static DependencyProperty LineProperty = DependencyProperty.Register("Line", typeof(string), typeof(BaseActivity));

        /// <summary>
        /// 线别
        /// </summary>
        [DescriptionAttribute("Line")]
        [CategoryAttribute("InArguments Of Base")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Line
        {
            get
            {
                return ((string)(base.GetValue(BaseActivity.LineProperty)));
            }
            set
            {
                base.SetValue(BaseActivity.LineProperty, value);
            }
        }



        /// <summary>
        /// 操作人员
        /// </summary>
        public static DependencyProperty EditorProperty = DependencyProperty.Register("Editor", typeof(string), typeof(BaseActivity));

        /// <summary>
        /// 操作人员
        /// </summary>
        [DescriptionAttribute("Editor")]
        [CategoryAttribute("InArguments Of Base")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Editor
        {
            get
            {
                return ((string)(base.GetValue(BaseActivity.EditorProperty)));
            }
            set
            {
                base.SetValue(BaseActivity.EditorProperty, value);
            }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public static DependencyProperty CustomerProperty = DependencyProperty.Register("Customer", typeof(string), typeof(BaseActivity));

        /// <summary>
        /// 客户
        /// </summary>
        [DescriptionAttribute("Customer")]
        [CategoryAttribute("InArguments Of Base")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Customer
        {
            get
            {
                return ((string)(base.GetValue(BaseActivity.CustomerProperty)));
            }
            set
            {
                base.SetValue(BaseActivity.CustomerProperty, value);
            }
        }

        /// <summary>
        ///  Session 类型
        /// </summary>
        public static DependencyProperty SessionTypeProperty = DependencyProperty.Register("SessionType", typeof(int), typeof(BaseActivity));

        /// <summary>
        ///  Session 类型
        /// </summary>
        [DescriptionAttribute("SessionType")]
        [CategoryAttribute("InArguments Of Base")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int SessionType
        {
            get
            {
                return ((int)(base.GetValue(BaseActivity.SessionTypeProperty)));
            }
            set
            {
                base.SetValue(BaseActivity.SessionTypeProperty, value);
            }
        }

        ///<summary>
        /// Constructor
        ///</summary>
        public BaseActivity()
        {
            InitializeComponent();
        }

        private readonly static ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly static string MsgDoExecute = " {0} Activity.DoExecute()";
        private readonly static string MsgPreExecute = " {0} Activity.PreExecute()";
        private readonly static string MsgPostExecute = " {0} Activity.PostExecute()";
        private readonly static string MsgErrorInExecute = "ErrorInExecute() Exception caught in {0}";
        private readonly static string MsgErrorWF = "ErrorInExecute() FISException caught in {0}  StopWF:{1} ErrorText:{2}";
        private readonly static string MsgFinallyExecute = " {0} Activity.FinallyExecute()";
        private readonly static string MsgNull = " {0} {1} {2} {3} is null.";
        private readonly static string MsgEmpty = " {0} {1} {2} {3} is empty.";
        private readonly static string MsgThisCustomer = "this.Customer";
        private readonly static string MsgThisEditor = "this.Editor";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                PreExecute();
                if (this.isSkip(CurrentSession))
                    return base.Execute(executionContext);
                DoExecute(executionContext);
                PostExecute();
            }
            catch (FisException ex)
            {
                //RollBackTransWhenTerminated();
                ErrorInExecute(ex);
                this.copeExceptionInTwoWays(ex, CurrentSession);
            }
            catch (Exception ex)
            {
                //RollBackTransWhenTerminated();
                ErrorInExecute(ex);
                if (ex.InnerException is FisException)
                {
                    this.copeExceptionInTwoWays((FisException)ex.InnerException, CurrentSession);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                FinallyExecute();
            }
            //return base.Execute(executionContext);
            return ActivityExecutionStatus.Closed;
        }

        private static void RollBackTransWhenTerminated()
        {
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                logger.Error("Begin: BaseActivity::RollBackTransWhenTerminated() For SqlTransactionManager.Rollback()");
                SqlTransactionManager.Rollback();
                logger.Error("End  : BaseActivity::RollBackTransWhenTerminated() For SqlTransactionManager.Rollback()");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                logger.Error("Begin: BaseActivity::RollBackTransWhenTerminated() For SqlTransactionManager.Dispose~End()");
                SqlTransactionManager.Dispose();
                SqlTransactionManager.End();
                logger.Error("End  : BaseActivity::RollBackTransWhenTerminated() For SqlTransactionManager.Dispose~End()");
            }
        }

        /// <summary>
        /// virtual method,must imply
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal virtual ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.DebugFormat(" {0} Activity.DoExecute()", this.Name);
            logger.DebugFormat(MsgDoExecute, this.Name);
            //CheckNotNullAndLog(this.Customer, "this.Customer", logger);
            //CheckNotNullAndLog(this.Editor, "this.Editor", logger);
            //CheckNotNullAndLog(this.Customer, MsgThisCustomer, logger);
            //CheckNotNullAndLog(this.Editor, MsgThisEditor, logger);
            return new ActivityExecutionStatus();
        }

        /// <summary>
        /// execute before Exceute function
        /// </summary>
        protected internal virtual void PreExecute()
        {
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.DebugFormat(" {0} Activity.PreExecute()", this.Name);
            logger.DebugFormat(MsgPreExecute, this.Name);
            //CheckNotNullAndLog(this.Customer, "this.Customer", logger);
            //CheckNotNullAndLog(this.Editor, "this.Editor", logger);
            CheckNotNullAndLog(this.Customer, MsgThisCustomer, logger);
            CheckNotNullAndLog(this.Editor, MsgThisEditor, logger);
        }

        /// <summary>
        /// execute after execute function
        /// </summary>
        protected internal virtual void PostExecute()
        {
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.DebugFormat(" {0} Activity.PostExecute()", this.Name);
            logger.DebugFormat(MsgPostExecute, this.Name);
            //CheckNotNullAndLog(this.Customer, "this.Customer", logger);
            //CheckNotNullAndLog(this.Editor, "this.Editor", logger);
        }

        /// <summary>
        /// when meet an error,execute this function
        /// </summary>
        /// <param name="ex"></param>
        protected internal virtual void ErrorInExecute(Exception ex)
        {
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //string msg = string.Format("Exception caught in {0}", this.Name);
            string msg = string.Format(MsgErrorInExecute, this.Name);
            logger.Error(msg, ex);
            //CheckNotNullAndLog(this.Customer, "this.Customer", logger);
            //CheckNotNullAndLog(this.Editor, "this.Editor", logger);
            //CheckNotNullAndLog(this.Customer, MsgThisCustomer, logger);
            //CheckNotNullAndLog(this.Editor, MsgThisEditor, logger);
        }
        /// <summary>
        /// catch FisException log
        /// </summary>
        /// <param name="ex"></param>
        protected internal virtual void ErrorInExecute(FisException ex)
        {
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //string msg = string.Format("Exception caught in {0}  StopWF:{1} ErrorText:{2}", this.Name, ex.stopWF.ToString(), ex.mErrmsg);
            string msg = string.Format(MsgErrorWF, this.Name, ex.stopWF.ToString(), ex.mErrmsg);
            //logger.Error(msg);
            logger.Error(msg, ex);
            
            //CheckNotNullAndLog(this.Customer, "this.Customer", logger);
            //CheckNotNullAndLog(this.Editor, "this.Editor", logger);
        }

        /// <summary>
        /// execute finally
        /// </summary>
        protected internal virtual void FinallyExecute()
        {
            //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //logger.DebugFormat(" {0} Activity.FinallyExecute()", this.Name);
            logger.DebugFormat(MsgFinallyExecute, this.Name);
            //CheckNotNullAndLog(this.Customer, "this.Customer", logger);
            //CheckNotNullAndLog(this.Editor, "this.Editor", logger);
            CheckNotNullAndLog(this.Customer, MsgThisCustomer, logger);
            CheckNotNullAndLog(this.Editor, MsgThisEditor, logger);
        }

        private void CheckNotNullAndLog(string value, string name, ILog logger)
        {
            if (value == null)
                //logger.ErrorFormat(" {0} {1} {2} {3} is null.", 
                logger.ErrorFormat(MsgNull, 
                                                  this.Name,
                                                  string.IsNullOrEmpty(this.Line)?string.Empty:this.Line ,
                                                  string.IsNullOrEmpty(this.Station)?string.Empty:this.Station,
                                                  name);
            else if (value.Trim() == string.Empty)
                //logger.ErrorFormat(" {0} {1} {2} {3} is empty.", this.Name,
                logger.ErrorFormat(MsgEmpty, this.Name,
                                                  string.IsNullOrEmpty(this.Line) ? string.Empty : this.Line,
                                                  string.IsNullOrEmpty(this.Station) ? string.Empty : this.Station,
                                                  name);
        }

        /// <summary>
        /// get inheritd class instance
        /// </summary>
        /// <returns></returns>
        protected internal virtual System.Workflow.ComponentModel.Activity GetInheritedClassInst()
        {
            return this;
        }

        /// <summary>
        /// Session for Current workflow
        /// </summary>
        protected internal Session CurrentSession
        {
            get
            {
                return SessionManager.GetInstance.GetSession(Key, (Session.SessionType)Enum.Parse(typeof(Session.SessionType), SessionType.ToString()));
            }
        }


        private void copeExceptionInTwoWays(FisException e, Session session)
        {
            if ( e.stopWF == false && session != null)
            {
                session.Exception = e;
            }
            else
            {
                throw e;
            }
        }
        private bool isSkip(Session session)
        {
            return (session != null && session.Exception != null);
        }

        /// <summary>
        /// Get root workflow
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        protected System.Workflow.ComponentModel.Activity GetRootWorkflow(System.Workflow.ComponentModel.Activity activity)
        {
            if (activity.Parent != null)
            {
                var wf = (CompositeActivity)GetRootWorkflow(activity.Parent);
                return wf;
            }
            return activity;
        }

        #region . MainObject Method

        /// <summary>
        /// Get Repair Target
        /// </summary>
        /// <returns></returns>
        protected IRepairTarget GetRepairTarget()
        {
            return (IRepairTarget)this.GetMainObject();
        }

        /// <summary>
        /// Update Repair Target
        /// </summary>
        /// <param name="target"></param>
        protected void UpdateRepairTarget(IRepairTarget target)
        {
            this.UpdateMainObject(target);
        }

        /// <summary>
        /// Get Main Object
        /// </summary>
        /// <returns></returns>
        protected ICheckObject GetCheckObject()
        {
            object mainObj = this.GetMainObject();
            return (ICheckObject)mainObj;
        }

        /// <summary>
        /// Used in PartCheck to get part owner
        /// </summary>
        /// <returns></returns>
        virtual protected IPartOwner GetPartOwner()
        {
            object mainObj = this.GetMainObject();

            //PVS站需要通过Product找到Pizza，此时PartOwner和MainObject不再是同一个对象Product是MainObject，Pizza是PartOwner
            if (this.Station.Equals("8E") || this.Station.Equals("PZ"))   //PVS
            {
                Pizza newPizza = (Pizza)CurrentSession.GetValue(Session.SessionKeys.Pizza);
                if (newPizza == null)
                {
                    newPizza = ((IProduct)mainObj).PizzaObj;
                }
                return (IPartOwner)newPizza;
            }
            else
            {
                return (IPartOwner)mainObj;
            }
        }

        /// <summary>
        /// update part owner
        /// </summary>
        /// <param name="owner"></param>
        protected void UpdatePartOwner(IPartOwner owner)
        {
            //PVS站需要通过Product找到Pizza，此时PartOwner和MainObject不再是同一个对象Product是MainObject，Pizza是PartOwner
            if (this.Station.Equals("8E") || this.Station.Equals("PZ"))   //PVS
            {
                IPizzaRepository repPizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                repPizza.Update((Pizza)owner, CurrentSession.UnitOfWork);
            }
            else
            {
                this.UpdateMainObject(owner);
            }
        }

        /// <summary>
        /// update main object
        /// </summary>
        /// <param name="obj"></param>
        protected void UpdateMainObject(Object obj)
        {
            switch ((Session.SessionType)Enum.Parse(typeof(Session.SessionType), SessionType.ToString()))
            {
                case Session.SessionType.MB:
                    IMBRepository repMB = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    repMB.Update((IMB)obj, CurrentSession.UnitOfWork);
                    break;
                case Session.SessionType.Product:
                    IProductRepository repProduct = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    repProduct.Update((IProduct)obj, CurrentSession.UnitOfWork);
                    break;
                default:
                    break;
            }
        }

        private Object GetMainObject()
        {
            Object obj = null;
            switch (CurrentSession.Type)
            {
                case Session.SessionType.MB:
                    obj = CurrentSession.GetValue(Infrastructure.Session.SessionKeys.MB);
                    break;
                case Session.SessionType.Product:
                    obj = CurrentSession.GetValue(Infrastructure.Session.SessionKeys.Product);
                    break;
                default:
                    break;
            }
            return obj;
        }

        #endregion

    }

}
