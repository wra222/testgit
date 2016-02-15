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
	public partial class UpdateDefectComponentBatchStatus : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateDefectComponentBatchStatus()
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
            DateTime now = DateTime.Now;            
            IList<DefectComponentInfo> defectComponentInfoList =utl.IsNull< IList<DefectComponentInfo>>(session,"DefectComponentInfo");
            DefectComponentBatchStatusInfo item = new DefectComponentBatchStatusInfo();
            switch (TypeFrom)
            { 
                case TypeFromEnum.Insert:
                    string batchID = (string)session.GetValue("BatchID") ?? "";
                    string returnLine = (string)session.GetValue("ReturnLine") ?? "";
                    int totalQty = (int)(session.GetValue("TotalQty") ?? "0");
                    IList<string> vendorList = defectComponentInfoList.Select(x => x.Vendor).Distinct().ToList();                   
                    item.BatchID = batchID;
                    item.Status = this.Status;
                    item.PrintDate = now;
                    item.Vendor = vendorList[0];
                    item.ReturnLine = returnLine;
                    item.TotalQty = totalQty;
                    item.Editor = this.Editor;
                    item.Cdt = now;
                    item.Udt = now; 
                    miscRep.InsertDataDefered<DefectComponentBatchStatus, DefectComponentBatchStatusInfo>(session.UnitOfWork,item);
                    break;
                case TypeFromEnum.Update:
                    item = utl.IsNull<DefectComponentBatchStatusInfo>(session, "DefectComponentBatchStatusInfo");
                    DefectComponentBatchStatusInfo condition = new DefectComponentBatchStatusInfo();
                    condition.BatchID = item.BatchID;
                    item.Status = this.Status;
                    item.Udt = now; 
                    miscRep.UpdateDataDefered<DefectComponentBatchStatus, DefectComponentBatchStatusInfo>(session.UnitOfWork, condition, item);
                    break;
            }
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// Type的来源，共有三种Vendor，PartSN，ID
        /// </summary>
        public static DependencyProperty TypeFromProperty = DependencyProperty.Register("TypeFrom", typeof(TypeFromEnum), typeof(UpdateDefectComponentBatchStatus));

        /// <summary>
        /// 共有2种Insert/Update 
        /// </summary>
        [DescriptionAttribute("TypeFromProperty")]
        [CategoryAttribute("InArguments Of UpdateDefectComponentBatchStatus")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(TypeFromEnum.Insert)]
        public TypeFromEnum TypeFrom
        {
            get
            {
                return ((TypeFromEnum)(base.GetValue(UpdateDefectComponentBatchStatus.TypeFromProperty)));
            }
            set
            {
                base.SetValue(UpdateDefectComponentBatchStatus.TypeFromProperty, value);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public static DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(string), typeof(UpdateDefectComponentBatchStatus), new PropertyMetadata("00"));

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
        /// 共有2种Insert/Update 
        /// </summary>
        public enum TypeFromEnum
        {
            /// <summary>
            /// 从Session.Vendor中获取
            /// </summary>
            Insert = 1,

            /// <summary>
            /// 从Session.PartSN中获取
            /// </summary>
            Update = 2
        }
	}
}
