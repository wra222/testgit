/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Dismantle WM
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 
 * Known issues:Any restrictions about this file 
 */


using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
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
using IMES.FisObject.Common.Process;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Part;
using System.Data;
using System.Data.SqlClient;
using IMES.Infrastructure.Repository._Schema;
namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ChangeDNOnly : BaseActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public ChangeDNOnly()
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
            string ChangeToDN = CurrentSession.GetValue("ChangeToDN") as string;
            if ((null != CurrentSession.GetValue("prodsToChangeToDN")) && !string.IsNullOrEmpty(ChangeToDN))
            {
                IDeliveryRepository currentRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery newDelivery = currentRepository.Find(ChangeToDN);
                if (newDelivery == null)
                {
                    throw new FisException("CHK190", new string[] { ChangeToDN });//DN¤£¦s¦b
                }

                IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                ArrayList productList = CurrentSession.GetValue("prodsToChangeToDN") as ArrayList;

                if ((productRep.GetCombinedQtyByDN(ChangeToDN) + productList.Count) > newDelivery.Qty)
                {
                    throw new FisException("CHK875", new string[] { });
                }
                
                foreach (var item in productList)
                {
                    productRep.Update((IProduct)item, CurrentSession.UnitOfWork);
                }
				if ((productRep.GetCombinedQtyByDN(ChangeToDN) + productList.Count) == newDelivery.Qty)
				{
                    newDelivery.Status = "87";
                    newDelivery.Udt = DateTime.Now;
                    IDeliveryRepository DeliveryRepository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    DeliveryRepository.Update(newDelivery, CurrentSession.UnitOfWork);
				}
            }
            return base.DoExecute(executionContext);
        }
    }
}
