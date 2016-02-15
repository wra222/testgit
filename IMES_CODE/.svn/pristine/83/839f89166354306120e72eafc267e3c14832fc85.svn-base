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
using IMES.FisObject.PAK.Carton;
using IMES.Infrastructure.Extend;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Part;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;

namespace IMES.Activity
{


    /// <summary>
    /// 計算UnitWeightWithCarton
    /// </summary>
  
    public partial class CalUnitWeightWithCarton : BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
        public CalUnitWeightWithCarton()
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
            decimal actualCartonWeight = (decimal) CurrentSession.GetValue(ExtendSession.SessionKeys.ActualCartonWeight);
            decimal acturalWeight=0;
           
            Carton carton = (Carton)CurrentSession.GetValue(Session.SessionKeys.Carton);
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            
           
            //滿箱
            if (carton.Status == CartonStatusEnum.Full)
            {
                acturalWeight = decimal.Round(actualCartonWeight/carton.Qty,2);
            }
            else //未滿箱
            {
                              
                //②　填充物重量=ConstValue.Value where Type='FillerWeight' and Name=Family
                IList<ConstValueInfo>  valueList=partRepository.GetConstValueListByType("FillerWeight");
                var fillWeightList = (from p in valueList
                                      where p.name == currentProduct.Family
                                      select p.value).ToList();
                if (fillWeightList.Count == 0)
                {
                    throw new FisException("CHK986", new string[] {"FillerWeight","ConstValue",currentProduct.Family} );
                }

                //④　PartNo=Model下面Tp=PL and UPPER(Descr) LIKE ‘%Carton%'的PartNo
                IHierarchicalBOM curBOM = bomRepository.GetHierarchicalBOMByModel(currentProduct.Model);
                if (curBOM == null)
                {
                    throw new FisException("CHK986", new string[] { "Model ", "BOM", currentProduct.Model });
                }
                IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;

                //var partList = (from p in bomNodeLst
                //                       where p.Part.BOMNodeType == "PL" && p.Part.Descr.ToUpper().IndexOf("CARTON") >= 0
                //                        select p.Part.PN).ToList();
                // Modify by Benson for  Chen, Li Request at 2013-04-18
                //Descr=BSAM Carton

                IList<string> partList=null;
                IFamilyRepository famliyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository, Family>();
                FamilyInfoDef fcond = new FamilyInfoDef();
                fcond.family = currentProduct.Family;
                fcond.name = "Category";
                IList<FamilyInfoDef> famValList = famliyRep.GetExistFamilyInfo(fcond);
                if (famValList.Count > 0 && famValList[0].value == "Tablet")
                {
                     partList = (from p in bomNodeLst
                                 where p.Part.BOMNodeType == "PL" && p.Part.Descr.Trim() == "Tablet Carton"
                                    select p.Part.PN).ToList();

                    if (partList.Count == 0)
                    {
                        // throw new FisException("CHK986", new string[] { "TP=PL and  Upper(Descr) like '%CARTON%'", "BOM", currentProduct.Model });
                        throw new FisException("CHK986", new string[] { "TP=PL and  p.Part.Descr.Trim()=='Tablet Carton'", "BOM", currentProduct.Model });

                    }
                }
                else
                {
                     partList = (from p in bomNodeLst
                                    where p.Part.BOMNodeType == "PL" && p.Part.Descr.Trim() == "BSAM Carton"
                                    select p.Part.PN).ToList();

                    if (partList.Count == 0)
                    {
                        // throw new FisException("CHK986", new string[] { "TP=PL and  Upper(Descr) like '%CARTON%'", "BOM", currentProduct.Model });
                        throw new FisException("CHK986", new string[] { "TP=PL and  p.Part.Descr.Trim()=='BSAM Carton'", "BOM", currentProduct.Model });

                    }
                }
              

                string partNo = partList[0];

                //③　外箱重量=ConstValue.Value where Type='CartonWeight' and Name=PartNo

                decimal fillWeight =   decimal.Parse(fillWeightList[0]) * (carton.FullQty - carton.Qty);

                IList<ConstValueInfo> valueList1 = partRepository.GetConstValueListByType("CartonWeight");
                var cartonWeightList = (from p in valueList1
                                                      where p.name == partNo
                                                     select p.value).ToList();
                if (cartonWeightList.Count == 0)
                {
                    throw new FisException("CHK986", new string[] { "CartonWeight","ConstValue", partNo });
                }

                decimal cartonWeight = (decimal.Parse(cartonWeightList[0]) * (carton.FullQty - carton.Qty)) / carton.FullQty;

                acturalWeight = decimal.Round( (actualCartonWeight - fillWeight - cartonWeight) / carton.Qty,2);
            }

            CurrentSession.AddValue(Session.SessionKeys.ActuralWeight, acturalWeight);
            return base.DoExecute(executionContext);
          
        }
	}
}
