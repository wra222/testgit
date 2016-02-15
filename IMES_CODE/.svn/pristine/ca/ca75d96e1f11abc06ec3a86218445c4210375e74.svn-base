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
using System.Collections.Generic;
using IMES.Infrastructure;
using IMES.Infrastructure.Extend;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PCA.PCBVersion;
using IMES.Infrastructure.FisObjectRepositoryFramework;


namespace IMES.Activity
{
    /// <summary>
    /// Get PC Version
    /// </summary>
    public partial class GetPCBVersion : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public GetPCBVersion()
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
            MB currenMB = CurrentSession.GetValue(Session.SessionKeys.MB) as MB;
            string family = currenMB.Family.Trim();
            string mbCode = currenMB.MBCode.Trim();
            IPCBVersionRepository pcbRep = RepositoryFactory.GetInstance().GetRepository<IPCBVersionRepository>();
            IList<PCBVersion> pcbVerList = pcbRep.GetPCBVersion(family, mbCode);
            CurrentSession.AddValue(ExtendSession.SessionKeys.HasPCBVer, false);
            if (pcbVerList.Count > 0)
            {
                string pcbVer = (string)CurrentSession.GetValue(ExtendSession.SessionKeys.PCBVer);
                if (string.IsNullOrEmpty(pcbVer))
                {
                    FisException e = new FisException("SA002", new string[] { ExtendSession.SessionKeys.PCBVer });
                    throw e;
                }

                PCBVersion pcbVerObj = pcbRep.Find(new string[] { family, mbCode, pcbVer });
                if (pcbVerObj == null)
                {
                    FisException e = new FisException("SA003", new string[] { family, mbCode, pcbVer });
                    throw e;
                }
                currenMB.CUSTVER = pcbVer;
                CurrentSession.AddValue(ExtendSession.SessionKeys.HasPCBVer, true);
                CurrentSession.AddValue(ExtendSession.SessionKeys.Revision, pcbVerObj.CTVer);
                CurrentSession.AddValue(ExtendSession.SessionKeys.Supplier, pcbVerObj.Supplier);
            }
               
            return base.DoExecute(executionContext);
        }
	}
}
