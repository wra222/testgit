//2010/1/12  fix bug ITC-1103-0091
using System;
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
using IMES.FisObject.Common.PrintLog;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;


namespace IMES.Activity
{
  
    public partial class WritePrintLogEnhance : BaseActivity
    {

   
        public static DependencyProperty LogTypeProperty = DependencyProperty.Register("LogType", typeof(string), typeof(WritePrintLogEnhance));

        [DescriptionAttribute("LogType")]
        [CategoryAttribute("LogType Category")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string LogType
        {
            get
            {
                return ((string)(base.GetValue(WritePrintLogEnhance.LogTypeProperty)));
            }
            set
            {
                base.SetValue(WritePrintLogEnhance.LogTypeProperty, value);
            }
        }

        public WritePrintLogEnhance()
        {
            InitializeComponent();
        }

        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
              string LogName="";
              if(string.IsNullOrEmpty(LogType))
              {LogName= CurrentSession.GetValue(Session.SessionKeys.PrintLogName).ToString();}
              else
              { LogName=LogType;}
            var item = new PrintLog
            {
                Name = LogName,
                BeginNo = CurrentSession.GetValue(Session.SessionKeys.PrintLogBegNo).ToString(),
                EndNo = CurrentSession.GetValue(Session.SessionKeys.PrintLogEndNo).ToString(),
                Descr = CurrentSession.GetValue(Session.SessionKeys.PrintLogDescr).ToString(),
                Editor = this.Editor
            };
            var repository = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
            repository.Add(item, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);

        }
    }
}
