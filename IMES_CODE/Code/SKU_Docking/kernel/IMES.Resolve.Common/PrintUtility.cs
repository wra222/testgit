using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Text.RegularExpressions;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.PrintItem;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;

namespace IMES.Resolve.Common
{
    /// <summary>
    /// Print Utiltiy
    /// </summary>
	public class PrintUtility
	{
        private Model _model = null;
        private Delivery _delivery = null;
        private IPart _part = null;
        private IHierarchicalBOM _bom = null;
        private string _customer = null;
        private string _moNo = null;
        private string _modelName = null;
        private string _partNo = null;
        private string _dnNo = null;
        private IProduct _product = null;
        private IMB _mb = null;
        private Session _session = null;

        private static readonly IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
        private static readonly IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
        private static readonly IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository>();
        private static readonly IBOMRepository bomRep =RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        private static readonly ILabelTypeRepository labelTypeRep = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository>();
        /// <summary>
        /// constructure
        /// </summary>
        /// <param name="session">session object</param>       
        /// <param name="modelName"> model name</param>
        /// <param name="moNo">MO Id</param>
        /// <param name="dnNo">Delivery No</param>
        /// <param name="partNo">Part No</param>
        /// <param name="customer">Customer</param>
        public PrintUtility(Session session, 
                                  string modelName, string moNo,
                                  string dnNo, string partNo, 
                                  string customer)
        {
            _session = session;
            _product =session.GetValue(Session.SessionKeys.Product) as Product;
            _mb = session.GetValue(Session.SessionKeys.MB) as MB;

            if (_product != null)
            {
                _customer = _product.Customer;
                if (string.IsNullOrEmpty(modelName) ||
                   modelName == _product.Model)
                {
                    modelName = _product.Model;
                    _model = _product.ModelObj;
                }               

                if (string.IsNullOrEmpty(moNo))
                {
                    moNo = _product.MO;
                }

                if (string.IsNullOrEmpty(dnNo))
                {
                    dnNo = _product.DeliveryNo;
                }

                //if (!string.IsNullOrEmpty(dnNo))
                //    _delivery = dnRep.Find(dnNo);
            }
            else if (_mb != null)
            {
                _customer = _mb.Customer;
                if (string.IsNullOrEmpty(modelName) &&
                    !string.IsNullOrEmpty(_mb.MB1397))
                {
                    modelName = _mb.MB1397;
                    _model = _mb.Model1397Obj;
                }

                if (string.IsNullOrEmpty(moNo))
                {
                    moNo = _mb.SMTMO;
                }
               

                if (string.IsNullOrEmpty(dnNo))
                {
                    dnNo = _mb.DeliveryNo;
                }

                //if (!string.IsNullOrEmpty(dnNo))
                //    _delivery = dnRep.Find(dnNo);

                if (string.IsNullOrEmpty(partNo))
                {
                    partNo = _mb.Model;
                }
            }

            _moNo = moNo;
            _modelName= modelName;
            _dnNo = dnNo;
            _partNo = partNo;

            if(string.IsNullOrEmpty(_customer))
            {
                _customer = customer;
            }            

            if(!string.IsNullOrEmpty(partNo))
            {
                _part =partRep.Find(partNo);
            }

            if (_model == null && !string.IsNullOrEmpty(_modelName))
            {
               _model= modelRep.Find(_modelName);
            }

            if (_delivery == null && !string.IsNullOrEmpty(_dnNo))
            {
                _delivery = dnRep.Find(_dnNo);
            }

        }
        /// <summary>
        /// Get Print Template
        /// </summary>
        /// <param name="labelType"></param>
        /// <param name="ruleMode"></param>
        /// <param name="printMode"></param>
        /// <returns></returns>             
        public PrintTemplate GetPrintTemplate(string labelType, int ruleMode, int printMode)
        {
           
            #region 檢查是否有分配過PrintTemplate by MO or DN
            if (ruleMode == 0) //by MO store resolved LabelType
            {
                if (string.IsNullOrEmpty(_moNo))
                {
                    ruleMode = -1;  //Print activity 未設置mo,不儲存找到Template name
                }
                //Check Mo_Label exists
                else 
                {
                    PrintTemplate printTemplate = labelTypeRep.GetPrintTemplateByMo(_moNo, labelType);
                    if (printTemplate != null)
                    {
                        return printTemplate;
                    }
                }
            }

            if (ruleMode == 1) // by PO Store resolved LabelType
            {
                if (string.IsNullOrEmpty(_dnNo))
                {
                    ruleMode = -1; //Print activity 未設置dn,不儲存找到Template name
                }
                else
                {
                    //Check PO_Label exists             
                    PrintTemplate printTemplate = labelTypeRep.GetPrintTemplateByPo(_dnNo, labelType);
                    if (printTemplate != null)
                    {
                        return printTemplate;
                    }
                }
            }
            #endregion

            #region 抓取PrintTemplate 設置Rule
            IList<LabelTemplateRuleDef> templateRuleList= labelTypeRep.GetLabelTemplateRule(labelType);
            if (templateRuleList == null || templateRuleList.Count == 0)
            {
                throw new FisException("CQCHK1078", new List<string> { labelType });  //沒有設置Template Rule Set
            }
            #endregion

            bool hasOrgTemplateName = false;
            #region Check TemplateName 為@開頭名稱使用動態抓取模板名稱@TemplateName example: @modelInfo.RDB,@product.CUSTSN
            var needResolveNameList = templateRuleList.Where(x => x.TemplateName.StartsWith("@"));
            if (_model != null && needResolveNameList.Count()>0)
            {
                IList<LabelTemplateRuleDef> needRemovedList = new List<LabelTemplateRuleDef>();
                foreach (LabelTemplateRuleDef item in needResolveNameList)
                {
                    //string name = resolveTemplateNameFromModelInfo(_model.Attributes, item.TemplateName.Substring(1));
                    string name = ResolveValue.GetValueWithoutError(_session, 
                                                                            _product, 
                                                                            _mb, 
                                                                            _delivery, 
                                                                            _part, 
                                                                            item.TemplateName.Substring(1), 
                                                                            '.');
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (printMode == 3 ||    //Bartender
                            printMode == 4)    //BartenderSrv
                        {
                            if (!name.ToLower().EndsWith(".btw"))
                            {
                                name = name + ".btw";
                            }
                        }
                        item.OrgTemplateName = item.TemplateName;
                        item.TemplateName = name;
                        hasOrgTemplateName = true;
                    }
                    else // 找不到值 Remove selection List
                    {
                        needRemovedList.Add(item);                      
                    }
                }

                if (needRemovedList.Count > 0)
                {
                    foreach (LabelTemplateRuleDef item in needRemovedList)
                    {
                        templateRuleList.Remove(item);
                    }
                }
            }
            #endregion

            string selectedTemplateName = "";           
            
            var templateList =templateRuleList.Select(x=>x.TemplateName).Distinct();
         
            var notMatchList = templateRuleList.Where(x => string.IsNullOrEmpty(x.AttributeName) ||
                                                                                     string.IsNullOrEmpty(x.AttributeValue) ||
                                                                                      !checkAndSetRule(x)).
                                                                                      Select(x => x.TemplateName).ToList();

             var selectedTemplateList= templateList.Except(notMatchList).ToList();
             if (selectedTemplateList == null || selectedTemplateList.Count == 0)
             {
                 return null;
             }
             else if (selectedTemplateList.Count == 1)
             {
                 selectedTemplateName = selectedTemplateList[0];
             }
             else // 最後修改修先等級比較高
             {
                 var availableTemplateList = templateRuleList.Where(x => selectedTemplateList.Contains(x.TemplateName)).
                                                            OrderByDescending(x => x.Udt).First();
                 selectedTemplateName = availableTemplateList.TemplateName;
             }

            //假如從ModelInfo抓取模板不需要保存到MO_Label/PO_Label
             if (hasOrgTemplateName)
             {
                 string name = templateRuleList.Where(x => x.TemplateName == selectedTemplateName)
                                                                .Select(x => x.OrgTemplateName).FirstOrDefault();
                 PrintTemplate printTemplate = null;
                 if (!string.IsNullOrEmpty(name))
                 {
                     printTemplate = labelTypeRep.GetPrintTemplate(labelType,name);
                 }
                 else
                 {
                     printTemplate = labelTypeRep.GetPrintTemplate(labelType,selectedTemplateName);
                 }

                  printTemplate.TemplateName = selectedTemplateName;
                  return printTemplate;
             }
             else
             {
                 if (ruleMode == 0) //by MO store resolved LabelType
                 {
                     labelTypeRep.InsertMOLabel(_moNo, labelType, selectedTemplateName);
                 }
                 else if (ruleMode == 1)
                 {
                     labelTypeRep.InsertPOLabel(_dnNo, labelType, selectedTemplateName);
                 }
                 return labelTypeRep.GetPrintTemplate(labelType,selectedTemplateName);
             }
            
        }
        
        //select a.LabelType, a.TemplateName,c.Mode, c.AttributeName, c.AttributeValue, a.Cdt
        //from PrintTemplate a, 
        //     LabelRule b, 
        //     LabelRuleSet c
        //where b.RuleID = c.RuleID and
        //      a.TemplateName =b.TemplateName
        //order by a.TemplateName, b.RuleID 

        //Mode=0 =>Customer
        //1 =>Model          2=>ModelInfo
        //3 =>Delivery  	  4=>DeliveryInfo
        // 5 =>Part           6=>PartInfo
        private bool checkAndSetRule(LabelTemplateRuleDef ruleDef)
        {
            if (ruleDef.Mode == "0") //Check Customer
            {
                #region Check Customer
                if (Regex.IsMatch( _customer,ruleDef.AttributeValue))
                {
                    ruleDef.IsChecked = "Y";
                    return true;
                }
                #endregion
            } 
            else if (ruleDef.Mode == "1")   //Check Model
            {
                #region Check Model
                if (_model != null)
                {
                    if (checkModel(_model, ruleDef.AttributeName, ruleDef.AttributeValue))
                    {
                        ruleDef.IsChecked = "Y";
                        return true;
                    }                   
                }
                #endregion
            }
            else if (ruleDef.Mode == "2") //Check ModelInfo
            {
                #region ModelInfo
                if (_model != null)
                {
                    if (checkModelInfo(_model.Attributes, ruleDef.AttributeName, ruleDef.AttributeValue))
                    {
                        ruleDef.IsChecked = "Y";
                        return true;
                    }                   
                }
                #endregion
            }
            else if (ruleDef.Mode == "3") //Check Delivery
            {
                #region Check Delivery
                if (_delivery != null)
                {
                    if (checkDelivery(_delivery, ruleDef.AttributeName, ruleDef.AttributeValue))
                    {
                        ruleDef.IsChecked = "Y";
                        return true;
                    }                   
                }
                #endregion
            }
            else if (ruleDef.Mode == "4")
            {
                #region Check DeliveryInfo
                if (_delivery != null)
                {
                    if (checkDeliveryInfo(_delivery.DeliveryInfoes, ruleDef.AttributeName, ruleDef.AttributeValue))
                    {
                        ruleDef.IsChecked = "Y";
                        return true;
                    }                   
                }
                #endregion
            }
            else if (ruleDef.Mode == "5")
            {
                #region Check Part
                if (_part != null)
                {
                    if (checkPart(_part, ruleDef.AttributeName, ruleDef.AttributeValue))
                    {
                        ruleDef.IsChecked = "Y";
                        return true;
                    }
                }
                else if (!string.IsNullOrEmpty(_modelName))
                {
                    IHierarchicalBOM bom = getBOM();
                    if (bom.FirstLevelNodes.Any(x => checkPart(x.Part, ruleDef.AttributeName, ruleDef.AttributeValue)))
                    {
                         ruleDef.IsChecked = "Y";
                        return true;
                    }
                }
                #endregion
            }
            else  if (ruleDef.Mode == "6")
            {
                #region check PartInfo
                if (_part != null)
                {
                    if (checkPartInfo(_part.Attributes, ruleDef.AttributeName, ruleDef.AttributeValue))
                    {
                        ruleDef.IsChecked = "Y";
                        return true;
                    }
                }
                else if (!string.IsNullOrEmpty(_modelName))
                {    //檢查PC階下層所有料的PartInfo
                    IHierarchicalBOM bom = getBOM();
                    if (bom != null)
                    {
                        IList<IBOMNode> bomNodeList = bom.FirstLevelNodes;
                         if (bomNodeList.Any(x=>checkPartInfo(x.Part.Attributes, ruleDef.AttributeName, ruleDef.AttributeValue)))
                         {
                              ruleDef.IsChecked = "Y";
                              return true;
                         }
                    }
                }   
                #endregion
            }

            return false;

        }

        private  bool checkModel(Model model, string name, string value)
        {
            bool ret = false;
            switch (name.ToUpper())
            {
                case "FAMILY":
                    ret = Regex.IsMatch(model.FamilyName, value);
                    break;
                case "MODEL":
                    ret = Regex.IsMatch(model.ModelName, value);
                    break;
                case "CUSTPN":
                    ret = Regex.IsMatch(model.CustPN, value);
                    break;
                case "REGION":
                    ret = Regex.IsMatch(model.Region, value);
                    break;
                case "SHIPTYPE":
                    ret = Regex.IsMatch(model.ShipType, value);
                    break;
                default:
                    break;
            }
            return ret;
        }

        private  bool checkModelInfo(IList<IMES.FisObject.Common.Model.ModelInfo> infoList, string name, string value)
        { 
            var exists = infoList.FirstOrDefault(x => x.Name == name && Regex.IsMatch(x.Value, value));
            return exists==null?false:true;
        }

        private bool checkDelivery(Delivery dn, string name, string value)
        {
            bool ret = false;
            switch (name.ToUpper())
            {
                case "DELIVERYNO":
                    ret = Regex.IsMatch(dn.DeliveryNo, value);
                    break;
                case "SHIPMENTNO":
                    ret = Regex.IsMatch(dn.ShipmentNo, value);
                    break;
                case "PONO":
                    ret = Regex.IsMatch(dn.PoNo, value);
                    break;
                case "MODEL":
                    ret = Regex.IsMatch(dn.ModelName, value);
                    break;               
                default:
                    break;
            }
            return ret;
        }

        private bool checkDeliveryInfo(IList<IMES.FisObject.PAK.DN.DeliveryInfo> infoList, string name, string value)
        {
            var exists = infoList.FirstOrDefault(x => x.InfoType== name && Regex.IsMatch(x.InfoValue, value));
            return exists == null ? false : true;
        }

        private bool checkPart(IPart part, string name, string value)
        {
            bool ret = false;
            switch (name.ToUpper())
            {
                case "PARTNO":
                     ret = Regex.IsMatch(part.PN, value);
                    break;
                case "DESCR":
                    ret = Regex.IsMatch(part.Descr, value);
                    break;
                case "BOMNODETYPE":
                    ret = Regex.IsMatch(part.BOMNodeType, value);
                    break;
                case "PARTTYPE":
                    ret = Regex.IsMatch(part.Type, value);
                    break;
                case "CUSTPARTNO":
                    ret = Regex.IsMatch(part.CustPn, value);
                    break;
                default:
                    break;
            }
            return ret;
        }

        private bool checkPartInfo(IList<PartInfo> infoList, string name, string value)
        {
            var exists = infoList.FirstOrDefault(x => x.InfoType == name && Regex.IsMatch(x.InfoValue, value));
            return exists == null ? false : true;
        }

        private  IHierarchicalBOM getBOM()
        {
             if(_bom==null && !string.IsNullOrEmpty(_modelName))
             {
                 _bom = bomRep.GetHierarchicalBOMByModel(_modelName);
             }
            return _bom;
        }

        private string resolveTemplateNameFromModelInfo(IList<IMES.FisObject.Common.Model.ModelInfo> infoList,string name)
        {

            return infoList.Where(x => x.Name == name).Select(y => y.Value).FirstOrDefault();
        }
	}
}
