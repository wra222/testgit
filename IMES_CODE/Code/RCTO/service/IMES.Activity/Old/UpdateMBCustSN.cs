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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Extend;

namespace IMES.Activity
{
    public partial class UpdateMBCustSN : BaseActivity
	{
		public UpdateMBCustSN()
		{
			InitializeComponent();
		}


        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mb = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            string CustSN = (string)CurrentSession.GetValue(Session.SessionKeys.CustSN);
            mb.CustSn = CustSN;            
            var mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            mbRepository.Update(mb, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);
        }
	}
}
