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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Part.PartPolicy;
using System.Reflection;
using IMES.Common;

namespace IMES.Activity
{
    /// <summary>
    /// 檢查Change Model 收集的料
    /// </summary>
	public partial class CheckAssignModel: BaseActivity
	{
        /// <summary>
        /// Constructor
        /// </summary>
        public CheckAssignModel()
		{
			InitializeComponent();
		}

        
        /// <summary>
        /// Check main logical 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;
            ActivityCommonImpl utl = ActivityCommonImpl.Instance;
             
            IProduct prod = utl.IsNull< IProduct>(session,Session.SessionKeys.Product);
            string newModel = utl.IsNullOrEmpty<string>(session, "theModel");
            string newFamily = utl.IsNullOrEmpty<string>(session, "theFamily"); 

            string oldModel = prod.Model;
            IList<ConstValueInfo> constValueList=null;
            ConstValueInfo matchedValue= null;
            if (utl.TryConstValue("CheckAssignModelBOM", prod.Family, out constValueList, out matchedValue))
            {
                IList<IProductPart> prodPartList = prod.ProductParts;
                if (prodPartList == null || prodPartList.Count == 0)
                {
                    return base.DoExecute(executionContext);
                }

                IList<IProductPart> needCheckPartList =null; 
                if ( !string.IsNullOrEmpty(matchedValue.value))
                {
                    var excludeCheckTypeList= matchedValue.value.Split(new char[]{'~'}).ToList();
                    needCheckPartList= prodPartList.Where(x=>!excludeCheckTypeList.Contains(x.CheckItemType)).ToList();
                }
                else
                {
                    needCheckPartList= prodPartList;
                }

                if (needCheckPartList.Count > 0)
                {
                    IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                    HierarchicalBOM newBom = (HierarchicalBOM)bomRep.GetHierarchicalBOMByModel(newModel);
                    if (newBom == null)
                    {
                        throw new FisException("CHK1117", new List<string> { newModel});                       
                    }

                    //抓取禁用料
                    IList<PartForbidPriorityInfo> partForbidPriorityInfoList = partRep.GetPartForbidWithFirstPriority(this.Customer,
                                                                                                                                                               prod.Status.Line,
                                                                                                                                                                newFamily,
                                                                                                                                                                newModel,
                                                                                                                                                                prod.ProId);

                    string orgStation = session.Station;
                    IPartPolicyRepository prtPcyRep = RepositoryFactory.GetInstance().GetRepository<IMES.FisObject.Common.Part.PartPolicy.IPartPolicyRepository>();
                    foreach (IProductPart item in needCheckPartList)
                    {
                        PartPolicy policy = (PartPolicy) prtPcyRep.GetPolicy(item.CheckItemType);
                        if (policy == null)
                        {
                           throw new FisException("IPartPolicy is null");                            
                        }
                        session.Station = item.Station;
                        IFlatBOM flatBom = policy.FilterBOM(newBom, item.Station, session);
                        if (flatBom == null || 
                            flatBom.BomItems==null || 
                            flatBom.BomItems.Count == 0)
                        {
                            //throw new FisException("CHK1118", new List<string> { newModel, oldModel, item.CheckItemType, item.PartSn, item.PartID }); 
                            session.Station = orgStation;
                            throw new FisException("CHK1117", new List<string> { newModel + " : " + item.CheckItemType });  
                        }

                        PartUnit partUnit = flatBom.Match(item.PartSn, item.Station);
                        if (partUnit == null)
                        {
                            session.Station = orgStation;
                            throw new FisException("CHK1118", new List<string> { newModel, oldModel, item.CheckItemType, item.PartSn, item.PartID });                            
                        }

                        if (policy.NeedPartForbidCheck && 
                            partForbidPriorityInfoList != null && 
                            partForbidPriorityInfoList.Count>0)
                        {
                            string noticeMsg = "";
                            if (partRep.CheckPartForbid(partForbidPriorityInfoList, partUnit.Type, partUnit.Sn, partUnit.Pn, out noticeMsg))
                            {
                                session.Station = orgStation;
                                throw new FisException("CHK1105", new string[] { partUnit.Type, partUnit.Pn, partUnit.Sn, noticeMsg ?? "" });
                            }
                        }
                    }
                    session.Station = orgStation;
                }
            }      
	        return base.DoExecute(executionContext);
        }
	}
}

