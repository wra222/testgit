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

using IMES.FisObject.Common.PartSn;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.MO;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// Check CTO Bom for IPC
    /// </summary>
    public partial class CheckCTOBom : BaseActivity
    {
        private static List<string> num = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        /// <summary>
        /// Constructor
        /// </summary>
        public CheckCTOBom()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check CTO Bom
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IPartRepository ipartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string> siteList = ipartRepository.GetValueFromSysSettingByName("Site");
            string site = (siteList != null && siteList.Count > 0 && !string.IsNullOrEmpty(siteList[0])) ? siteList[0].Trim() : "IPC";
            if (site != "IPC")
            {
                return base.DoExecute(executionContext);
            }


            string model = "";
            MO mo = (MO)CurrentSession.GetValue(Session.SessionKeys.ProdMO);

            if (mo == null)
            {
                IProduct prod = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
                if (prod != null)
                {
                    model = prod.Model;
                }
            }
            else
            {
                model = mo.Model;
            }


            if (!string.IsNullOrEmpty(model) &&
                model.StartsWith("PC") &&
                num.Contains(model.Substring(6, 1)))  //CTO Model & IPC site need check 
            {
                if (!prodRep.ExistCTOBom(model))
                {
                    throw new FisException("TRC001",new string[]{model});
                }

            }

            return base.DoExecute(executionContext);
        }
    }
}
