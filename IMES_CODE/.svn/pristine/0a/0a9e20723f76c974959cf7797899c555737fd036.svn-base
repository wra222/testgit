// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
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

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CheckDDRCT : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckDDRCT()
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
            bool existDDRCT = false;
            string ddrct = CurrentSession.GetValue("DDRCT") as string;
            if (null != ddrct){
                Product currentProduct = ((Product)CurrentSession.GetValue(Session.SessionKeys.Product));
                IList<IProductPart> productParts = currentProduct.ProductParts;
                foreach (ProductPart part in productParts)
                {
                    if ("DDR".Equals(part.CheckItemType) && ddrct.Equals(part.PartSn))
                    {
                        existDDRCT = true;
                        break;
                    }
                }
            }
            if (!existDDRCT)
            {
                List<string> errpara = new List<string>();
                FisException e = new FisException("CHK1014", errpara); // 请核对流程卡
                throw e;
            }
            
            return base.DoExecute(executionContext);
        }
    }
}

