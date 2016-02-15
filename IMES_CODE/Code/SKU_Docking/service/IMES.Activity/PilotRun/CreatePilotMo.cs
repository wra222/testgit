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
    public partial class CreatePilotMo : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CreatePilotMo()
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
            string pilotMoSuffix = (string)CurrentSession.GetValue(Session.SessionKeys.PilotMoSuffix);
            if (string.IsNullOrEmpty(pilotMoSuffix))
            {
                FisException e = new FisException("CHK975", new string[] { Session.SessionKeys.PilotMoSuffix });
                throw e;
            }

            List<string> moIdList =(List<string>)CurrentSession.GetValue(Session.SessionKeys.VirtualMOList);
            if (moIdList == null || moIdList.Count == 0)
            {
                FisException e = new FisException("CHK975", new string[] { Session.SessionKeys.VirtualMOList });
                throw e;
            }


            PilotMoInfo pilotMo = (PilotMoInfo)CurrentSession.GetValue(Session.SessionKeys.PilotMo);
            if (pilotMo ==null)
            {
                FisException e = new FisException("CHK975", new string[] { Session.SessionKeys.PilotMo });
                throw e;
            }

            string pilotMoNo = moIdList[0] + pilotMoSuffix.Trim();
            pilotMo.mo = pilotMoNo;
            pilotMo.editor = this.Editor;
            pilotMo.combinedQty = 0;
            pilotMo.combinedState = PilotMoCombinedStateEnum.Empty.ToString();
            pilotMo.state = PilotMoStateEnum.Open.ToString();
            pilotMo.cdt = DateTime.Now;
            pilotMo.udt = DateTime.Now;

            IMORepository moRep = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
            moRep.InsertPilotMoDefered(CurrentSession.UnitOfWork, pilotMo);           
            
            return base.DoExecute(executionContext);          
        }
	}
    
}
