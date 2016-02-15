
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
using IMES.DataModel;

namespace IMES.Activity
{

    /// <summary>
    /// 
    /// </summary>
    public partial class CheckProductForVirtualMo : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public CheckProductForVirtualMo()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            string mo = (string)CurrentSession.GetValue(Session.SessionKeys.MONO);
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            if (mo.Substring(0, 1) != "V")
            {
                List<string> param = new List<string>();
                throw new FisException("CHK926", param);
            }

            IList<string> list = new List<string>();
            list = productRepository.GetProdIDListByMO(mo);
            if (list != null && list.Count > 0)
            {
                List<string> param = new List<string>();
                param.Add(mo);
                throw new FisException("CHK912", param);
            }
            
            return base.DoExecute(executionContext);
        }
    }
}