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
using IMES.FisObject.Common.Station;
using IMES.FisObject.FA.Product;
using System.Collections.Generic;
using IMES.FisObject.PAK.Carton;
using IMES.Infrastructure.Extend;
using IMES.DataModel;
using IMES.FisObject.PAK.StandardWeight;

namespace IMES.Activity
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UpdateModelWeightWithCarton : BaseActivity
	{  
        /// <summary>
        /// 
        /// </summary>
		public UpdateModelWeightWithCarton()
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

         
            //4.	如果Product Model 的UnitWeight (IMES_GetData..ModelWeight) 属性不存在则记录该属性；如果该属性值的Udt 不是当天，则更新该记录的UnitWeightValue，Editor 和Udt；
            IProduct currentProduct = prodList[0];
            IModelWeightRepository currentModelWeightRepository = RepositoryFactory.GetInstance().GetRepository<IModelWeightRepository, ModelWeight>();
            ModelWeight currentModelWeight = currentModelWeightRepository.Find(currentProduct.Model);

            //ITC-1360-1299: 如果Product Model 的UnitWeight (IMES_GetData..ModelWeight)属性，没有新Insert或Update时，不需要更新（Insert）HP EDI 数据（PAK_SkuMasterWeight_FIS）
            CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, false);
            

            if (currentModelWeight == null )
            {
                ModelWeight newModelWeight = new ModelWeight();
                newModelWeight.Model = currentProduct.Model;
                newModelWeight.UnitWeight = actualUnitWeight;
                newModelWeight.CartonWeight = actualCartonWeight;
                newModelWeight.SendStatus = "";
                newModelWeight.Remark = "New";
                newModelWeight.Editor = this.Editor;
                newModelWeight.Udt = DateTime.Now;

                currentModelWeightRepository.Add(newModelWeight, CurrentSession.UnitOfWork);

                CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
                CurrentSession.AddValue("SetWeight", newModelWeight.UnitWeight);
            }
            else if (currentModelWeight.UnitWeight.Equals(0))
            {
                currentModelWeight.Model = currentProduct.Model;
                currentModelWeight.UnitWeight = actualUnitWeight;
                currentModelWeight.CartonWeight = actualCartonWeight;
                currentModelWeight.SendStatus = "";
                currentModelWeight.Remark = "0";
                currentModelWeight.Editor = this.Editor;
                currentModelWeight.Udt = DateTime.Now;

                currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);

                CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
                CurrentSession.AddValue("SetWeight", currentModelWeight.UnitWeight);

            }
            //ITC-1360-1302
            //if (currentModelWeight.Udt.ToShortDateString().ToString() != DateTime.Now.ToShortDateString().ToString())
            else if (currentModelWeight.Udt.ToShortDateString().ToString() != DateTime.Now.ToShortDateString().ToString() && carton.Status == CartonStatusEnum.Full)
            {
                currentModelWeight.Model = currentProduct.Model;
                currentModelWeight.Remark = currentModelWeight.UnitWeight.ToString();
                currentModelWeight.UnitWeight = actualUnitWeight;
                currentModelWeight.CartonWeight = actualCartonWeight;
                currentModelWeight.SendStatus = "";               
                currentModelWeight.Editor = this.Editor;
                currentModelWeight.Udt = DateTime.Now;

                currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);

                CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
                CurrentSession.AddValue("SetWeight", currentModelWeight.UnitWeight);
            }
            //当 Product 为同机型机器当天的第50台Pass Unit Weight 站的机器，需要取这50台机器的Product.UnitWeight 的平均值，更新ModelWeight 对应记录的UnitWeight，Editor 和Udt
            //else
            //{
            //    IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //    if (49 == productRepository.GetCountOfCurrentDayByModelAndStation(currentProduct.Model, "85"))
            //    {
            //        decimal avrWeight = productRepository.GetAverageModelWeightOfCurrentDay(currentProduct.Model, "85");
            //        decimal thisWeight = (decimal)CurrentSession.GetValue(Session.SessionKeys.ActuralWeight);
            //        currentModelWeight.Model = currentProduct.Model;
            //        currentModelWeight.UnitWeight = (avrWeight * 49 + thisWeight) / 50;
            //        currentModelWeight.CartonWeight = actualCartonWeight;
            //        currentModelWeight.Editor = this.Editor;
            //        currentModelWeight.Udt = DateTime.Now;

            //        currentModelWeightRepository.Update(currentModelWeight, CurrentSession.UnitOfWork);

            //        CurrentSession.AddValue(Session.SessionKeys.IsCheckPass, true);
            //        CurrentSession.AddValue("SetWeight", currentModelWeight.UnitWeight);
            //    }
            //}


            return base.DoExecute(executionContext);

        }
	}
}
