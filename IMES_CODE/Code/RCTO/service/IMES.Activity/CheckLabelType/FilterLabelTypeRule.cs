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
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using System.Collections.Generic;
using IMES.FisObject.Common.PrintItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using System.Text.RegularExpressions;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;

namespace IMES.Activity
{
    /// <summary>
    /// 使用LabelTypeRule 檢查and Filter LabelType
    /// </summary>
    public partial class FilterLabelTypeRule : BaseActivity
	{
        /// <summary>
        /// 使用LabelTypeRule 檢查and Filte LabelType
        /// </summary>
		public FilterLabelTypeRule()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Get Model From Product or PCB 
        /// </summary>     
        public enum ModelFromEnum
        {
            /// <summary>
            /// from Product
            /// </summary>
            Product = 1,

            /// <summary>
            /// from PCB
            /// </summary>
            PCB = 2           
        }

        /// <summary>
        /// Get Model ModelFrom
        /// </summary>
        public static DependencyProperty ModelFromProperty = DependencyProperty.Register("ModelFrom", typeof(ModelFromEnum), typeof(FilterLabelTypeRule), new PropertyMetadata(ModelFromEnum.Product));

        /// <summary>
        /// 绑定到 ProductPart or PCBPart or PizzaPart
        /// </summary>
        [DescriptionAttribute("ModelFrom")]
        [CategoryAttribute("InArugment")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public ModelFromEnum ModelFrom
        {
            get
            {
                return ((ModelFromEnum)(base.GetValue(FilterLabelTypeRule.ModelFromProperty)));
            }
            set
            {
                base.SetValue(FilterLabelTypeRule.ModelFromProperty, value);
            }
        }

        /// <summary>
        /// 使用LabelTypeRule 檢查and Filte LabelType Exexute 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
           
            var PrintItemList = (IList<PrintItem>)CurrentSession.GetValue(Session.SessionKeys.PrintItems);
            
            //0.No Print Item List do nothing
            if (PrintItemList == null || PrintItemList.Count == 0)
            {
                return base.DoExecute(executionContext);
            }

            //1.Get LabelTypeRule
            var LabelTypeList = (from item in PrintItemList
                                              select item.LabelType).ToList();

            ILabelTypeRepository LabelTypeRep = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
            IList<LabelTypeRuleDef> labelTypeRuleList= LabelTypeRep.GetLabeTypeRuleByLabelType(LabelTypeList);
             IDeliveryRepository deliveryRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            if (labelTypeRuleList.Count > 0)
            {
                
                IMB CurrentMB = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
                Product CurrentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);

                string family;
                string model;
                Delivery delivery=null;
                IList<IMES.FisObject.Common.Model.ModelInfo>  modelInfoList= new  List<IMES.FisObject.Common.Model.ModelInfo>();
                if (this.ModelFrom== ModelFromEnum.PCB)
                {
                    family = CurrentMB.Family;
                    model = CurrentMB.Model;                   
                }
                else
                {
                    family = CurrentProduct.Family;
                    model = CurrentProduct.Model;
                    modelInfoList = CurrentProduct.ModelObj.Attributes;
                    if (!string.IsNullOrEmpty(CurrentProduct.DeliveryNo))
                    {
                        delivery = deliveryRep.Find(CurrentProduct.DeliveryNo);
                    }                    
            
                }

                var NotMatchedLabelTypes = (from rule in labelTypeRuleList
                                                         where !CheckStation(rule, this.Station) ||
                                                                    !CheckFamilyAndModel(rule, family, model, delivery, modelInfoList) ||
                                                                    !CheckBom(rule, model, this.ModelFrom)
                                                         select rule.LabelType).ToList();

                var resultPrintItemList = (from item in PrintItemList
                                                       where !NotMatchedLabelTypes.Contains(item.LabelType)
                                                      select item).ToList();

                CurrentSession.AddValue(Session.SessionKeys.PrintItems, resultPrintItemList);
                
            }

            return base.DoExecute(executionContext);
        }

        private bool CheckFamilyAndModel(LabelTypeRuleDef rule, string family, string model, Delivery delivery,
            IList<IMES.FisObject.Common.Model.ModelInfo> modelInfos)
        {
           
            if (!string.IsNullOrEmpty(rule.Family) && !Regex.IsMatch(family,rule.Family)) 
            {               
                return false;
            }

            if (!string.IsNullOrEmpty(rule.Model) && !Regex.IsMatch(model, rule.Model))
            {
                return false;
            }

            IList<PartInfo> checkModelInfos = new List<PartInfo>();
            if (!string.IsNullOrEmpty(rule.ModelConstValue))
            {
                checkModelInfos = GetConstValue(rule.ModelConstValue);
            }

            if (checkModelInfos.Count > 0)
            {
                if (
                    modelInfos.Count == 0 || 
                    !CheckModelInfo(modelInfos, checkModelInfos)
                    )
                {
                    return false;
                }                
            }

            IList<PartInfo> checkDeliveryInfos = new List<PartInfo>();
            if (!string.IsNullOrEmpty(rule.DeliveryConstValue))
            {
                checkDeliveryInfos = GetConstValue(rule.DeliveryConstValue);
            }
            if (checkDeliveryInfos.Count > 0)
            {
                if (
                    delivery==null ||
                    !CheckDeliveryInfo(delivery.DeliveryInfoes, checkDeliveryInfos)
                    )
                {
                    return false;
                }
               
            }

            return true;
        }

        private bool CheckStation(LabelTypeRuleDef rule, string station)
        {
            if (!string.IsNullOrEmpty(rule.Station) && !Regex.IsMatch(station, rule.Station))
            {
                return false;
            }

            return true;
        }

        private bool CheckBom(LabelTypeRuleDef rule, string model,ModelFromEnum modeFrom)
        {
            
            if (rule.BomLevel <= 0) return true;

            if(
                string.IsNullOrEmpty(rule.BomNodeType) &&
                string.IsNullOrEmpty(rule.PartDescr) &&
                string.IsNullOrEmpty(rule.PartNo) &&
                string.IsNullOrEmpty(rule.PartType) && 
                string.IsNullOrEmpty(rule.PartConstValue)  
                )
            { 
                return true;
            }

            IList<PartInfo> checkPartInfos=new List<PartInfo>();
            if (!string.IsNullOrEmpty(rule.PartConstValue))
            {
                checkPartInfos = GetConstValue(rule.PartConstValue);
            }

            if (modeFrom == ModelFromEnum.PCB) // PCB 131 model
            {
                IPartRepository PartRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
                IPart part = PartRep.Find(model);
                if (part == null)
                {
                    return false;
                }
                return CheckThisLevelPart(part, rule, checkPartInfos);
            }
            else  // for  BOM
            {
                IBOMRepository BomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

                IHierarchicalBOM Bom = BomRep.GetHierarchicalBOMByModel(model);
                if (Bom == null)
                {
                    return false;
                }

                IList<IBOMNode> BomParts = Bom.FirstLevelNodes;

                return CheckBomPart(BomParts, rule, 1, checkPartInfos);
            }
        }

        private bool CheckThisLevelPart(IPart part,
                                                               LabelTypeRuleDef rule,
                                                              IList<PartInfo> checkPartInfos)
        {
            
             if (!string.IsNullOrEmpty(rule.BomNodeType) && rule.BomNodeType != part.BOMNodeType)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(rule.PartNo) &&  !Regex.IsMatch(part.PN, rule.PartNo ))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(rule.PartDescr) && !Regex.IsMatch(part.Descr, rule.PartDescr))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(rule.PartType) && !Regex.IsMatch(part.Type, rule.PartType))
            {
                return false;
            }

            if (checkPartInfos.Count > 0)
            {
               return CheckThisLevelPartInfo(part.Attributes, checkPartInfos);                
            }

            return true;          

        }

         private bool CheckThisLevelPartInfo(IList<PartInfo> partInfos,
                                                                    IList<PartInfo> checkPartInfos)
         {
             
             //var matched= (from item in partInfos
             //                        join check in checkPartInfos 
             //                        on  new {name=item.InfoType.Trim(), value= item.InfoValue.Trim()  }  equals
             //                              new {name=check.InfoType.Trim(), value=check.InfoValue.Trim()}
             //                        select  item.InfoType).ToList();
             // if (matched.Count == checkPartInfos.Count)
             //{
             //    return true;
             //}
             //else
             //{
             //   return false;  
             //}

             bool allMatched = true;
             foreach (PartInfo info in checkPartInfos)
             {
                 var matched = (from item in partInfos
                                            where item.InfoType.Equals(info.InfoType) &&
                                                       Regex.IsMatch(item.InfoValue, info.InfoValue)
                                            select item.InfoType).ToList();
                 if (matched.Count == 0)
                 {
                     allMatched = false;
                     break;
                 }
             }

             return allMatched;            

         }


         private bool CheckModelInfo(IList<IMES.FisObject.Common.Model.ModelInfo> modelInfos,
                                                       IList<PartInfo> checkModelInfos)
         {

            

             bool allMatched = true;
             foreach (PartInfo info in checkModelInfos)
             {
                 var matched = (from item in modelInfos
                                where item.Name.Equals(info.InfoType) &&
                                           Regex.IsMatch(item.Value, info.InfoValue)
                                select item.Name).ToList();
                 if (matched.Count == 0)
                 {
                     allMatched = false;
                     break;
                 }
             }

             return allMatched;

         }

         private bool CheckDeliveryInfo(IList<DeliveryInfo> deliveryInfos,
                                                        IList<PartInfo> checkDeliveryInfos)
         {

           
             bool allMatched = true;
             foreach (PartInfo info in checkDeliveryInfos)
             {
                 var matched = (from item in deliveryInfos
                                where item.InfoType.Equals(info.InfoType) &&
                                           Regex.IsMatch(item.InfoValue, info.InfoValue)
                                select item.InfoType).ToList();
                 if (matched.Count == 0)
                 {
                     allMatched = false;
                     break;
                 }
             }

             return allMatched;

         }
         private bool CheckBomPart(IList<IBOMNode> BomParts,
                                                      LabelTypeRuleDef rule, 
                                                      int Curlevel,
                                                      IList<PartInfo> checkPartInfos  )
        {
            
            foreach (IBOMNode material in BomParts)
            {
                if (rule.BomLevel == Curlevel)
                {
                    if (CheckThisLevelPart(material.Part, rule, checkPartInfos))
                    {
                        return true;
                    }
                }
                else if (rule.BomLevel < Curlevel) // over Bom level
                {
                    return false;
                }
                else   // go to next Bom Level check                  
                {
                    if (CheckBomPart(material.Children, rule, Curlevel + 1, checkPartInfos))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        
        //private IList<PartInfo> ParsePartInfo(string infoType, string infoValue)
        //{
        //     IList<PartInfo> ret = new List <PartInfo>();
        //    string[] names = infoType.Split(new char[] {',',';'});
        //    string[] values=infoValue.Split(new char[] {',',';'});
        //    if (names.Length== values.Length)
        //    {
        //        for ( int i =0; i<names.Length; ++i)
        //        {
        //            if (!string.IsNullOrEmpty(names[i]) && !string.IsNullOrEmpty(values[i])) 
        //            {
        //                ret.Add ( new PartInfo { InfoType= names[i], InfoValue=values[i] });
        //            }
        //        }                
        //    }
        //    return ret;
        //}

        private IList<PartInfo> GetConstValue(string constValueType)
        {
            IList<PartInfo> ret = new List<PartInfo>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();


           IList<ConstValueInfo> valueList=  partRep.GetConstValueInfoList(new ConstValueInfo
                                                                                                                            {
                                                                                                                                type = constValueType
                                                                                                                            });
          
           foreach (ConstValueInfo item in valueList)
           {
               ret.Add(new PartInfo { InfoType = item.name, InfoValue = item.value }); 
           }        

           return ret;
        }
	}
}
