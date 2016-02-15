// INVENTEC corporation (c)2011 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2013-02-07   Benson          create
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
using IMES.FisObject.PAK.BSam;
using IMES.Infrastructure.Extend;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MoveInBSamLoc : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public MoveInBSamLoc()
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
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IBSamRepository bsamRep = RepositoryFactory.GetInstance().GetRepository<IBSamRepository, BSamLocation>();
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

            BSamModel bsamModel = (BSamModel) CurrentSession.GetValue(ExtendSession.SessionKeys.BSamModel);
            if (bsamModel == null)
            {
                bsamModel = bsamRep.GetBSamModel(currentProduct.Model);
            }
            if (bsamModel == null)
            {
                FisException e = new FisException("CHK983", new string[] { currentProduct.Model });               
                throw e;
            }
            BSamLocation loc = null;
            string prodLocId= currentProduct.GetAttributeValue("CartonLocation");
            if (!string.IsNullOrEmpty(prodLocId))
            {
                
                prodLocId=prodLocId.TrimEnd();
                //int locId = 0;
                //if (int.TryParse(prodLocId, out  locId))
                //{
                    //loc = bsamRep.Find(locId);
                    loc = bsamRep.Find(prodLocId);
                    if (loc != null)
                    {
                        if (loc.Model.Trim() != currentProduct.Model.Trim())
                        {
                            prodLocId = prodLocId + " (BSamLocation.Model:{0}!=Product.Model:{1}";
                            prodLocId = string.Format(loc.Model, currentProduct.Model);
                            FisException e = new FisException("CHK989", new string[] { currentProduct.ProId, currentProduct.CUSTSN, prodLocId });
                            throw e;
                        }

                        IList<string> productIds = loc.ProductIDs;
                        if (productIds.Count != loc.Qty)
                       {
                            prodLocId =prodLocId + " (BSamLocation.Qty:{0}!=AssignQThty:{1}";
                            prodLocId = string.Format(prodLocId, loc.Qty.ToString(), productIds.Count.ToString());
                            FisException e = new FisException("CHK989", new string[] { currentProduct.ProId, currentProduct.CUSTSN, prodLocId });
                            throw e;
                        }
                    }
                    else
                    {
                        FisException e = new FisException("CHK989", new string[] { currentProduct.ProId, currentProduct.CUSTSN, prodLocId});
                        throw e;
                    }
                //}
                //else
                //{                    
                //     throw new Exception("ProductAttr.CartonLocation value is not a valid integer") ;
                //}
            }

            if (loc == null)
            {
                loc = bsamRep.GetAndAssignMoveInLoc(bsamModel.A_Part_Model, bsamModel.QtyPerCaton, CurrentSession.Editor);
            }

            if (loc == null)
            {
                FisException e = new FisException("CHK987", new string[] { currentProduct.Model , currentProduct.CUSTSN});
                throw e;
            }

            currentProduct.SetAttributeValue("CartonLocation", loc.LocationId.Trim(), CurrentSession.Editor, CurrentSession.Station, "");
            CurrentSession.AddValue(ExtendSession.SessionKeys.BSamLoc, loc);
            prodRep.Update(currentProduct, CurrentSession.UnitOfWork);
            return base.DoExecute(executionContext);

        }
	}
}
