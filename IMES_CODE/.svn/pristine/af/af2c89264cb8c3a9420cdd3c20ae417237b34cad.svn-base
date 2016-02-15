
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
    public partial class GetCartonByProduct : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public GetCartonByProduct()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

            if (String.IsNullOrEmpty(CurrentProduct.CartonSN))
            {
                List<string> errpara = new List<string>();
                throw new FisException("CHK801", errpara);
            }
            CurrentSession.AddValue(Session.SessionKeys.Carton, CurrentProduct.CartonSN);            
            return base.DoExecute(executionContext);
        }
    }
}