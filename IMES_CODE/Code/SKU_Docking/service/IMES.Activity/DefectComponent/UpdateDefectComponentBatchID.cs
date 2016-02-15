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
	public partial class UpdateDefectComponentBatchID : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public UpdateDefectComponentBatchID()
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
            string batchID = utl.IsNullOrEmpty<string>(session, "BatchID");
            string returnLine = utl.IsNullOrEmpty<string>(session, "ReturnLine");
            IList<DefectComponentInfo> defectComponentInfoList = utl.IsNull<IList<DefectComponentInfo>>(session,"DefectComponentInfo");
            
            DateTime now = DateTime.Now;
            foreach (DefectComponentInfo item in defectComponentInfoList)
            {
                item.BatchID = batchID;
                item.ReturnLine = returnLine;
                item.Udt = now;
                item.Editor = this.Editor;
                DefectComponentInfo condition = new DefectComponentInfo();
                condition.ID = item.ID;
                miscRep.UpdateDataByIDDefered<DefectComponent, DefectComponentInfo>(session.UnitOfWork, condition, item);               
            }
            
            return base.DoExecute(executionContext);
        }
	}
}
