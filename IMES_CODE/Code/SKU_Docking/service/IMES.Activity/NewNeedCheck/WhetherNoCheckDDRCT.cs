// INVENTEC corporation (c)2009 all rights reserved. 
//                   
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class WhetherNoCheckDDRCT : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public WhetherNoCheckDDRCT()
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
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
			Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
			
            string WhetherNoCheckDDRCT = "N";
            IList<ConstValueTypeInfo> retLst = partRep.GetConstValueTypeList("NoCheckDDRCT");
            if (retLst != null)
                foreach (ConstValueTypeInfo ti in retLst)
                {
                    if (currentProduct.Family.Equals(ti.value))
                    {
                        WhetherNoCheckDDRCT = "Y";
                        break;
                    }
                }
            CurrentSession.AddValue("WhetherNoCheckDDRCT", WhetherNoCheckDDRCT);

            return base.DoExecute(executionContext);
        }
    }
}

