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
using IMES.FisObject.PAK.Pizza;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BolDnForPackingList : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BolDnForPackingList()
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
            int dnIndex = (int)CurrentSession.GetValue(Session.SessionKeys.DnIndex);

            IList<string> dnList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.DeliveryList);

            CurrentSession.AddValue("Data", dnList[dnIndex]);

            dnIndex++;

            CurrentSession.AddValue(Session.SessionKeys.DnIndex, dnIndex);
            //CurrentSession.GetValue("Data");
            return base.DoExecute(executionContext);
        }
    }
}

