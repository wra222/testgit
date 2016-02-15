// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Vincent          create
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
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.PAK.Carton;
using IMES.Infrastructure.Extend;
using IMES.DataModel;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateUnitWeightWithCarton : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public UpdateUnitWeightWithCarton()
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

            Carton carton = (Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
            if (carton == null)
            {
                throw new FisException("No Session Key [" + Session.SessionKeys.Carton + "] value");
            }

            IList<IProduct> prodList = (IList<IProduct>)CurrentSession.GetValue(Session.SessionKeys.ProdList);
            IList<string> prodIdList = (IList<string>)CurrentSession.GetValue(Session.SessionKeys.NewScanedProductIDList);
            decimal actualCartonWeight = (decimal)CurrentSession.GetValue(ExtendSession.SessionKeys.ActualCartonWeight);
            decimal actualUnitWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight); 
            ICartonRepository cartonRep = RepositoryFactory.GetInstance().GetRepository<ICartonRepository, Carton>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            foreach (IProduct item in prodList)
            {
                item.UnitWeight = actualUnitWeight;
                item.CartonWeight = actualCartonWeight;                
                prodRep.Update(item, CurrentSession.UnitOfWork);                  
       
                //需不需要???
                prodRep.InsertUnitWeightLogDefered(CurrentSession.UnitOfWork, new UnitWeightLog
                                                                                                                                    {
                                                                                                                                        productID = item.ProId,
                                                                                                                                        unitWeight = actualUnitWeight,                                                                                                                                        
                                                                                                                                        editor = Editor,
                                                                                                                                        cdt = DateTime.Now
                                                                                                                                    });
            }

            carton.Weight = actualCartonWeight;
            cartonRep.Update(carton, CurrentSession.UnitOfWork);

            return base.DoExecute(executionContext);

        }
	}
}
