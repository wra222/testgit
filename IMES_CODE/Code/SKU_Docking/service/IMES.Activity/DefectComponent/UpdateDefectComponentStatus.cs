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
	public partial class UpdateDefectComponentStatus: BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateDefectComponentStatus()
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
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
            Session session = CurrentSession;       
            string status=this.Status;
            if (string.IsNullOrEmpty(this.Status))
            {
                status = utl.IsNullOrEmpty<string>(session, "Status");
            }
            string comment = (string)session.GetValue("Comment");

            IList<DefectComponentInfo> defectComponentInfoList = utl.IsNull< IList<DefectComponentInfo>>(session,"DefectComponentInfo");
             DateTime now = DateTime.Now;     
            foreach (DefectComponentInfo item in defectComponentInfoList)
            {
               
                item.Status = status;               
                if (this.IsResetBatchID)
                {
                    item.BatchID = "";
                    item.ReturnLine = "";
                }
                item.Comment = comment;               
                item.Editor = this.Editor;
                item.Udt = now;
                DefectComponentInfo condition = new DefectComponentInfo();
                condition.ID = item.ID;
                miscRep.UpdateDataByIDDefered<DefectComponent, DefectComponentInfo>(session.UnitOfWork,condition, item);                
            }
           
            return base.DoExecute(executionContext);
        }


        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(UpdateDefectComponentStatus));

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

        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty IsResetBatchIDProperty = DependencyProperty.Register("IsResetBatchID", typeof(bool), typeof(UpdateDefectComponentStatus), new PropertyMetadata(true));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("IsResetBatchID")]
        [CategoryAttribute("InArguments Of CheckDefectComponentStatus")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public bool IsResetBatchID
        {
            get
            {
                return ((bool)(base.GetValue(IsResetBatchIDProperty)));
            }
            set
            {
                base.SetValue(IsResetBatchIDProperty, value);
            }
        }
	}
}
