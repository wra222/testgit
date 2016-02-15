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
using System.Workflow.Activities.Rules;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.FisObject.Common.Misc;
using IMES.Infrastructure.Repository._Metas;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
	public partial class WriteDefectComponentBatchStatusLog : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public WriteDefectComponentBatchStatusLog()
		{
			InitializeComponent();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMiscRepository miscRep = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
            Session session = CurrentSession;

            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            string status = this.Status;
            if (string.IsNullOrEmpty(this.Status))
            {
                status = utl.IsNullOrEmpty<string>(session, "Status");
            }

            IList<DefectComponentInfo> defectComponentInfoList = (IList<DefectComponentInfo>)session.GetValue("DefectComponentInfo");
            IList<string> batchIDList= defectComponentInfoList.Select(x => x.BatchID).Distinct().ToList();
            DateTime now = DateTime.Now;
            foreach (string id in batchIDList)
            {
                DefectComponentBatchStatusLogInfo saveItem = new DefectComponentBatchStatusLogInfo();
                saveItem.BatchID =id;
                saveItem.Editor = this.Editor;
                saveItem.Status = status;
                saveItem.Cdt = now;
                miscRep.InsertDataWithIDDefered<DefectComponentBatchStatusLog, DefectComponentBatchStatusLogInfo>(session.UnitOfWork, saveItem);

            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(WriteDefectComponentBatchStatusLog), new PropertyMetadata("00"));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("StatusProperty")]
        [CategoryAttribute("InArguments Of UpdateDefectComponentStatus")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string Status
        {
            get
            {
                return ((string)(base.GetValue(StatusProperty)));
            }
            set
            {
                base.SetValue(StatusProperty, value);
            }
        }
	}
}
