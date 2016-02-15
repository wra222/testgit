// INVENTEC corporation (c)201all rights reserved. 
// Description:  各站workflow基类
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-06   YangWeihua              create
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.Infrastructure;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.Activity
{
    /// <summary>
    /// 各站workflow基类, 用于定义各站wf的公有属性
    /// </summary>
    public class BaseFlow : SequentialWorkflowActivity
    {
        /// <summary>
        /// Workflow对应的Session对象引用
        /// </summary>
        /// <remarks></remarks>
        private Session _session;
        private System.Workflow.ComponentModel.FaultHandlersActivity faultHandlersActivity1;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public BaseFlow()
        { }

         private void InitializeComponent() 
         {
            this.CanModifyActivities = true;
            this.faultHandlersActivity1 = new FaultHandlersActivity();
            //
            //faultHandlersActivity1
            //
            this.faultHandlersActivity1.Name = "faultHandlersActivity1";
            //
            //FisBaseFlow
            //
            this.Activities.Add(this.faultHandlersActivity1);
            this.Name = "BaseFlow";
            this.CanModifyActivities = false;

        }

        /// <summary>
        /// Station
        /// </summary>
        public static DependencyProperty StationProperty = DependencyProperty.Register("Station", typeof(string), typeof(BaseFlow));

        /// <summary>
        /// Station
        /// </summary>
        [DescriptionAttribute("Station")]
        [CategoryAttribute("InArgumentsOfBase")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Station
        {
            get
            {
                return ((string)(base.GetValue(BaseFlow.StationProperty)));
            }
            set
            {
                base.SetValue(BaseFlow.StationProperty, value);
            }
        }

        /// <summary>
        /// Key of associated session
        /// </summary>
        public static DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(string), typeof(BaseFlow));

        /// <summary>
        /// Key of associated session
        /// </summary>
        [DescriptionAttribute("Key")]
        [CategoryAttribute("InArgumentsOfBase")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Key
        {
            get
            {
                return ((string)(base.GetValue(BaseFlow.KeyProperty)));
            }
            set
            {
                base.SetValue(BaseFlow.KeyProperty, value);
            }
        }
        
        /// <summary>
        /// Editor
        /// </summary>
        public static DependencyProperty EditorProperty = DependencyProperty.Register("Editor", typeof(string), typeof(BaseFlow));

        /// <summary>
        /// Editor
        /// </summary>
        [DescriptionAttribute("Editor")]
        [CategoryAttribute("InArgumentsOfBase")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Editor
        {
            get
            {
                return ((string)(base.GetValue(BaseFlow.EditorProperty)));
            }
            set
            {
                base.SetValue(BaseFlow.EditorProperty, value);
            }
        }

        /// <summary>
        /// Product Line
        /// </summary>
        public static DependencyProperty PdLineProperty = DependencyProperty.Register("PdLine", typeof(string), typeof(BaseFlow));

        /// <summary>
        /// Product Line
        /// </summary>
        [DescriptionAttribute("PdLine")]
        [CategoryAttribute("InArgumentsOfBase")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string PdLine
        {
            get
            {
                return ((string)(base.GetValue(BaseFlow.PdLineProperty)));
            }
            set
            {
                base.SetValue(BaseFlow.PdLineProperty, value);
            }
        }

        /// <summary>
        /// Customer
        /// </summary>
        public static DependencyProperty CustomerProperty = DependencyProperty.Register("Customer", typeof(string), typeof(BaseFlow));

        /// <summary>
        /// Customer
        /// </summary>
        [DescriptionAttribute("Customer")]
        [CategoryAttribute("InArgumentsOfBase")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string Customer
        {
            get
            {
                return ((string)(base.GetValue(BaseFlow.CustomerProperty)));
            }
            set
            {
                base.SetValue(BaseFlow.CustomerProperty, value);
            }
        }

        /// <summary>
        /// Type of associated session
        /// </summary>
        public static DependencyProperty SessionTypeProperty = DependencyProperty.Register("SessionType", typeof(int), typeof(BaseFlow));

        /// <summary>
        /// Type of associated session
        /// </summary>
        [DescriptionAttribute("SessionType")]
        [CategoryAttribute("InArgumentsOfBase")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int SessionType
        {
            get
            {
                return ((int)(base.GetValue(BaseFlow.SessionTypeProperty)));
            }
            set
            {
                base.SetValue(BaseFlow.SessionTypeProperty, value);
            }
        }

        /// <summary>
        /// Associated session
        /// </summary>
        public Session CurrentFlowSession
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        /// <summary>
        /// 将Activity捕获到的异常放入session中,供bll检查, 该方法使异
        /// 常不会继续向外传递, 从而保证workflow的执行不会被异常中断
        /// </summary>
        /// <param name="e">需要放入session的FisException对象</param>
        /// <remarks></remarks>
        public void raiseSessionException(FisException e)
        {
            e.stopWF = false;
            _session.Exception = e;
        }

        /// <summary>
        /// 将Activity捕获到的异常根据设定SessionKeys.RaiseExceptionToSession进行不同处理：
        /// 1. 放入session中,供bll检查, 该方法使异
        ///    常不会继续向外传递, 从而保证workflow的执行不会被异常中断
        /// 2. 直接抛出异常，从而使workflow的执行被异常中断
        /// </summary>
        /// <param name="e"></param>
        public void raiseException(FisException e)
        {
            if (_session.GetValue(Session.SessionKeys.RaiseExceptionToSession) != null)
            {
                raiseSessionException(e);
            }
            else
            {
                throw e;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            try
            {
                ActivityExecutionStatus status = base.Execute(executionContext);
                return status;
            }
            finally
            {
                // Commit it if there's any outstanding transaction
                if (SqlTransactionManager.IsStillSomeConn() || SqlTransactionManager.IsStillSomeTrans() || SqlTransactionManager.inScopeTag)
                {
                    var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    logger.ErrorFormat("IsStillSomeConn: {0}, IsStillSomeTrans: {1}.", SqlTransactionManager.IsStillSomeConn(), SqlTransactionManager.IsStillSomeTrans());
                    var info = SqlTransactionManager.GetCurrentSqlTransactionManagerInfo();
                    logger.ErrorFormat("Embeded: {0}, ErrOccured: {1}, InScopeTag: {2}.", info.Embeded, info.ErrOccured, info.InScopeTag);
                    SqlTransactionManager.ResetCompulsorily();
                }
            }
        }
    }

}

