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

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
	public partial class WriteDefectComponentLog : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public WriteDefectComponentLog()
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
            IList<DefectComponentInfo> defectComponentInfoList = (IList<DefectComponentInfo>)session.GetValue("DefectComponentInfo");
            foreach (DefectComponentInfo item in defectComponentInfoList)
            {
                DefectComponentLogInfo saveItem = new DefectComponentLogInfo();
                saveItem.ActionName = ActionName;
                saveItem.ComponentID = item.ID;
                saveItem.RepairID = item.RepairID;
                saveItem.PartSn = item.PartSn;
                saveItem.Customer = item.Customer;
                saveItem.Model = item.Model;
                saveItem.Family = item.Family;
                saveItem.DefectCode = item.DefectCode;
                saveItem.DefectDescr = item.DefectDescr;
                saveItem.ReturnLine = item.ReturnLine;
                saveItem.Remark = "";
                saveItem.BatchID = item.BatchID;
                saveItem.Comment = item.Comment;
                saveItem.Status = item.Status;
                saveItem.Editor = this.Editor;
                saveItem.Cdt = DateTime.Now;
                miscRep.InsertDataWithIDDefered<DefectComponentLog, DefectComponentLogInfo>(session.UnitOfWork,saveItem);
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ActionNameProperty = DependencyProperty.Register("ActionName", typeof(string), typeof(WriteDefectComponentLog));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("ActionNameProperty")]
        [CategoryAttribute("InArguments Of WriteDefectComponentLog")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue("")]
        public string ActionName
        {
            get
            {
                return ((string)(base.GetValue(ActionNameProperty)));
            }
            set
            {
                base.SetValue(ActionNameProperty, value);
            }
        }
	}
}
