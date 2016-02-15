// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
//2014-06-06    Vincent           release
// Known issues:
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
using IMES.FisObject.Common;
using IMES.FisObject.PAK.Pallet;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Extend;
using System.Collections.Generic;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.MO;
using IMES.DataModel;


namespace IMES.Activity
{

    /// <summary>
    /// Check PilotMo State
    /// </summary>  
    public partial class CheckAndCombinedPilotMo : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CheckAndCombinedPilotMo()
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
            string pilotMo =  (string)CurrentSession.GetValue(Session.SessionKeys.PilotMoNo);
            if (string.IsNullOrEmpty(pilotMo))
            {
                FisException e = new FisException("CHK975", new string[] { Session.SessionKeys.PilotMoNo });                
                throw e;
            }

            if (CurrentSession.GetValue(Session.SessionKeys.PilotMoQty) == null)
            {
                FisException e = new FisException("CHK975", new string[] { Session.SessionKeys.PilotMoQty });
                throw e;
            }
            int pilotQty = (int)CurrentSession.GetValue(Session.SessionKeys.PilotMoQty);

            IMORepository moRep = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
            moRep.CheckAndCombinedPilotMoDefered(CurrentSession.UnitOfWork, pilotMo, pilotQty, this.Editor);           
            
            return base.DoExecute(executionContext);          
        }
	}
  
}
