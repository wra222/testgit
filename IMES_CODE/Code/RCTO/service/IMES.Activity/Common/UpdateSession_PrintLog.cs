// INVENTEC corporation (c)2011 all rights reserved. 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.Common.Line;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;

namespace IMES.Activity
{
    /// <summary>
    /// 请参考.\Common\Common Rule.docx 文档中的相关描述.没找到相应的文档。
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
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
    ///         无
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         Session.MB
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class UpdateSession_PrintLog : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
		public UpdateSession_PrintLog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Get Product Object and Put it into Session.SessionKeys.DNInfoValue
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            if (!string.IsNullOrEmpty(this.SessionPrintLogName))
			{
			    string printLogName = CurrentSession.GetValue(this.SessionPrintLogName) as string;
                if (!string.IsNullOrEmpty(printLogName))
                {
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogName, printLogName);
                }
            }
			
			if (!string.IsNullOrEmpty(this.SessionPrintLogBegNo))
			{
			    string printLogBegNo = CurrentSession.GetValue(this.SessionPrintLogBegNo) as string;
                if (!string.IsNullOrEmpty(printLogBegNo))
                {
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogBegNo, printLogBegNo);
                }
            }
			
			if (!string.IsNullOrEmpty(this.SessionPrintLogEndNo))
			{
			    string printLogEndNo = CurrentSession.GetValue(this.SessionPrintLogEndNo) as string;
                if (!string.IsNullOrEmpty(printLogEndNo))
                {
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogEndNo, printLogEndNo);
                }
            }

            if (!string.IsNullOrEmpty(this.SessionPrintLogDescr))
            {
                string printLogDescr = CurrentSession.GetValue(this.SessionPrintLogDescr) as string;
                if (!string.IsNullOrEmpty(printLogDescr))
                {
                    CurrentSession.AddValue(Session.SessionKeys.PrintLogDescr, printLogDescr);
                }
            }
			
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 输入的类型:string
        /// </summary>
        public static DependencyProperty SessionPrintLogNameProperty = DependencyProperty.Register("SessionPrintLogName", typeof(string), typeof(UpdateSession_PrintLog));
        /// <summary>
        /// SessionPrintLogName
        /// </summary>
        [DescriptionAttribute("SessionPrintLogName")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SessionPrintLogName
        {
            get
            {
                return ((string)(base.GetValue(UpdateSession_PrintLog.SessionPrintLogNameProperty)));
            }
            set
            {
                base.SetValue(UpdateSession_PrintLog.SessionPrintLogNameProperty, value);
            }
        }
		
		/// <summary>
        /// 输入的类型:string
        /// </summary>
        public static DependencyProperty SessionPrintLogBegNoProperty = DependencyProperty.Register("SessionPrintLogBegNo", typeof(string), typeof(UpdateSession_PrintLog));
        /// <summary>
        /// SessionPrintLogBegNo
        /// </summary>
        [DescriptionAttribute("SessionPrintLogBegNo")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SessionPrintLogBegNo
        {
            get
            {
                return ((string)(base.GetValue(UpdateSession_PrintLog.SessionPrintLogBegNoProperty)));
            }
            set
            {
                base.SetValue(UpdateSession_PrintLog.SessionPrintLogBegNoProperty, value);
            }
        }
		
		/// <summary>
        /// 输入的类型:string
        /// </summary>
        public static DependencyProperty SessionPrintLogEndNoProperty = DependencyProperty.Register("SessionPrintLogEndNo", typeof(string), typeof(UpdateSession_PrintLog));
        /// <summary>
        /// SessionPrintLogEndNo
        /// </summary>
        [DescriptionAttribute("SessionPrintLogEndNo")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SessionPrintLogEndNo
        {
            get
            {
                return ((string)(base.GetValue(UpdateSession_PrintLog.SessionPrintLogEndNoProperty)));
            }
            set
            {
                base.SetValue(UpdateSession_PrintLog.SessionPrintLogEndNoProperty, value);
            }
        }
		
        /// <summary>
        /// 输入的类型:string
        /// </summary>
        public static DependencyProperty SessionPrintLogDescrProperty = DependencyProperty.Register("SessionPrintLogDescr", typeof(string), typeof(UpdateSession_PrintLog));
        /// <summary>
        /// InfoType
        /// </summary>
        [DescriptionAttribute("SessionPrintLogDescr")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string SessionPrintLogDescr
        {
            get
            {
                return ((string)(base.GetValue(UpdateSession_PrintLog.SessionPrintLogDescrProperty)));
            }
            set
            {
                base.SetValue(UpdateSession_PrintLog.SessionPrintLogDescrProperty, value);
            }
        }
	}
}
