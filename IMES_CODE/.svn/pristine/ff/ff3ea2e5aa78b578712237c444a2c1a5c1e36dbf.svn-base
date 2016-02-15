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
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Carton;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Activity
{
    /// <summary>
    /// Check COO Label Type
    /// </summary>
	public partial class CheckCOOLabelPrint: BaseActivity
	{
        /// <summary>
        /// 
        /// </summary>
		public CheckCOOLabelPrint()
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
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            //IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            //Model curModel = modelRep.Find(curProduct.Model);
            string family = curProduct.Family;
            
            string labelTemp = "COO Label";

            //Get ConstValue Type ='SpecialCOOLabelTypeRule'
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.FisBOM.IBOMRepository>();
            IList<ConstValueInfo> valueList = partRep.GetConstValueListByType("SpecialCOOLabelTypeRule");

            //1. Check Family rule
            var familyLabelList = (from p in valueList
                                          where p.name == family
                                       select p.value).ToList();
            if (familyLabelList == null || familyLabelList.Count == 0)
            {
                //2.Chek Bom PartNo
                IHierarchicalBOM bom = null;
                bom = bomRep.GetHierarchicalBOMByModel(curProduct.Model);
                IList<IBOMNode>  bomNodeLst = bom.FirstLevelNodes;
              
                if (bomNodeLst != null && bomNodeLst.Count>0 )
                {
                    foreach (IBOMNode ibomnode in bomNodeLst)
                    {                       
                        var partLabelList = (from p in valueList
                                             where p.name == ibomnode.Part.PN
                                             select p.value).ToList();
                        if (partLabelList != null && partLabelList.Count > 0)
                        {
                            labelTemp = partLabelList[0].Trim();
                            break;
                        }
                    }
                }
            }
            else
            {
                labelTemp = familyLabelList[0].Trim();
            }

           // //下列Family 为Consumer Family:            
            //if (family == "HARBOUR 1.0" || family == "HARBOUR 1.1" || family == "ST133I 1.0" || family == "ST133I 1.1"
            //    || family == "ST133I 1.2" || family == "ST145A 1.0" || family == "ST145A 1.1" || family == "ST145A 1.2"
            //    || family == "ST145I 1.0" || family == "ST145I 1.1" || family == "ST145I 1.2" || family == "ROMEO 1.0"
            //    || family == "ROMEO 1.1" || family == "ROMEO 1.2" || family == "ROMEO 2.0" || family == "ZIDANE 1.0"
            //    || family == "ZIDANE 1.1" || family == "ZIDANE 1.2" || family == "ZIDANE 2.0" || family == "ZIDANE 2.1"
            //    || family == "MURRAY 1.1" || family == "MURRAY 1.2" || family == "MURRAY 1.2" || family == "JIXI 1.0"
            //    || family == "JIXI 2.0" || family == "JIXI 2.1" || family == "JIXI 2.2" || family == "JACKMAND 1.0"
            //    || family == "JACKMANU 1.0" || family == "JACKMAND 1.1" || family == "JACKMANU 1.1" || family == "JACKMAND 1.2"
            //    || family == "JACKMANU 1.2" || family == "PARKERU 1.0" || family == "PARKERD 1.0" || family == "PARKERU 1.1"
            //    || family == "PARKERD 1.1" || family == "PARKERU 1.2" || family == "PARKERD 1.2" || family == "KITTY 1.0"
            //    || family == "HELLO 1.0" || family == "VUITTON 1.0" || family == "LAUREN 1.0" || family == "VUITTON 1.1"
            //    || family == "LAUREN 1.1")
            //{
            //    labelTemp = "COO Label-2";
                
            //}
            //else if (family == "BANDIT 1.0")
            //{
            //    labelTemp = "COO Label-3";
               
            //}
            CurrentSession.AddValue("COOLabel", labelTemp);

            return base.DoExecute(executionContext);

        }
	}
}
