
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
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.MO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{

    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateMoForVirtualMo : BaseActivity
    {
        private static Object _syncObj = new Object();
        /// <summary> 
        /// </summary>
        public UpdateMoForVirtualMo()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            var mo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);

            

            lock (_syncObj)
            {
                IMORepository moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository, MO>();

                mo.Status = "C";
                moRepository.Update(mo, CurrentSession.UnitOfWork);
            }
            return base.DoExecute(executionContext);
        }
    }
}