/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/CheckBindForPL
 * UI:CI-MES12-SPEC-PAK-UI PackingList.docx –2011/10/10 
 * UC:CI-MES12-SPEC-PAK-UC PackingList.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-12-16   zhanghe               (Reference Ebook SourceCode) Create

* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
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
    public partial class CheckBindForPL : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckBindForPL()
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
            IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            string type = (string)CurrentSession.GetValue("Doc_type");
            IList<VShipmentPakComnInfo> comnList = (IList<VShipmentPakComnInfo>)CurrentSession.GetValue("ComnList");
            //ITC-1360-1089 "Pack List-Addition"->"Pack List- Addition"
            if (type == "Pack List- Addition" || type == "ALL")
            {
                IList<string> tList = new List<string>();
                foreach (VShipmentPakComnInfo temp in comnList)
                {
                    tList.Add(temp.internalID);
                }
                string dn = (string)CurrentSession.GetValue(Session.SessionKeys.DeliveryNo);
                if (!DeliveryRepository.CheckExistPakPackkingData(tList))
                {
                    FisException ex;
                    List<string> erpara = new List<string>();
                    erpara.Add(dn);
                    ex = new FisException("CHK821", erpara);
                    throw ex;
                }
            }
            return base.DoExecute(executionContext);
        }
    }
}

