// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-25   vincent          create
//2013-03-13    Vincent           release
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
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckWWANAndPCID: BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckWWANAndPCID()
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
            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            string model = product.Model;
            string family = product.ModelObj.Family.FamilyName;
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<ConstValueTypeInfo> consWWANlist = partRep.GetConstValueTypeList("NotCheckWWAN");
            CurrentSession.AddValue("IsCheckWWAN", "Y");
            if (
                  consWWANlist.Any(x => x.value.Equals(model)) || consWWANlist.Any(x => x.value.Equals(family))
                )
            {
                CurrentSession.AddValue("IsCheckWWAN", "N");
            }

            IList<ConstValueTypeInfo> consPCIDlist = partRep.GetConstValueTypeList("NotCheckPCID");
            CurrentSession.AddValue("IsCheckPCID", "Y");
            if (
                  consPCIDlist.Any(x => x.value.Equals(model)) || consPCIDlist.Any(x => x.value.Equals(family))
                )
            {
                CurrentSession.AddValue("IsCheckPCID", "N");
            }
            return base.DoExecute(executionContext);

        }
    }
}

