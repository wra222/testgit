// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
//          Date                          Reason                            Name                        
// ==========   =======================      ============
// 2014-05-20                         Create                         Vincent 
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.PAK.DN;

namespace IMES.CheckItemModule.CQ.AstRule.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.AstRule.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "AstRule";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            
            IList<AstRuleInfo> share_parts_set = new List<AstRuleInfo>();
        
            IFlatBOM ret = null;
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            HierarchicalBOM bom = (HierarchicalBOM)hierarchical_bom;

            try
            {
                if (bom.FirstLevelNodes != null)
                {
                    IList<string> astCategoryList = new List<string> { "AT", "PP" }; 
                    List<astItemRule> astItemRuleList = new List<astItemRule>();
                    var atPartList = (from p in bom.FirstLevelNodes
                                      where astCategoryList.Contains(p.Part.BOMNodeType)
                                      select p.Part);

                    foreach (IPart part in atPartList)
                    {
                        var avPart = (from p in part.Attributes
                                      where p.InfoType == "AV"
                                      select p.InfoValue).FirstOrDefault();
                        if (!string.IsNullOrEmpty(avPart))
                        {
                            astItemRuleList.Add(new astItemRule {  Code =part.Descr,
                                                               AVPartNo = avPart,
                                                               Part = part
                            });
                        }
                    }
                    IProduct product=GetProduct(main_object);
                    IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    foreach (astItemRule item in astItemRuleList)
                    {
                        IList<AstRuleInfo> astruleList = prodRep.GetAstRuleByCodeAndStationAndCust(item.Code, station, item.AVPartNo);
                        if (astruleList.Count > 0)
                        {
                            foreach (AstRuleInfo rule in astruleList)
                            {
                                AstRuleInfo info = CheckAstRule(rule, product, item.Part);
                                if (info != null)
                                {
                                    share_parts_set.Add(info);
                                }
                            }
                        }
                        else  //料號找不到後再找CustName為空白
                        {
                            astruleList = prodRep.GetAstRuleByCodeAndStationAndCust(item.Code, station, "");
                            foreach (AstRuleInfo rule in astruleList)
                            {
                                AstRuleInfo info = CheckAstRule(rule, product, item.Part);
                                if (info != null)
                                {
                                    share_parts_set.Add(info);
                                }
                            }
                        }
                    }
                  
                }

                if (share_parts_set.Count > 0)
                {
                    foreach (AstRuleInfo ruleInfo in share_parts_set)
                    {
                        IPart part = new Part()
                        {
                            PN = ruleInfo.code + "/" + ruleInfo.custName,      //PN要跟 flat_bom_item.PartNoItem值一樣                  
                            CustPn = ruleInfo.custName,
                             Remark="",
                            Descr = ruleInfo.custName + "/" + ruleInfo.checkItem,
                             Descr2="",
                            Type = ruleInfo.code,
                             Udt= DateTime.Now,
                             Cdt=DateTime.Now
                        };
                        var flat_bom_item = new FlatBOMItem(qty, part_check_type, new List<IPart>(){part});
                        flat_bom_item.PartNoItem = ruleInfo.code + "/" + ruleInfo.custName; 
                        flat_bom_item.Tp =  ruleInfo.code;
                        flat_bom_item.Descr = ruleInfo.custName + "/" + ruleInfo.checkItem;
                        flat_bom_item.ValueType = ruleInfo.remark; //存放Part match時檢查的值
                        flat_bom_items.Add(flat_bom_item);
                    }
                }
                if (flat_bom_items.Count > 0)
                {
                    ret = new FlatBOM(flat_bom_items);
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return ret;
        }        

        private IProduct GetProduct(object main_object)
        {
            string objType = main_object.GetType().ToString();
            IMES.Infrastructure.Session session = null;
            IProduct iprd = null;
            if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
                iprd = (IProduct)main_object;
            }
            else if (main_object.GetType().Equals(typeof(IMES.Infrastructure.Session)))
            {
                session = (IMES.Infrastructure.Session)main_object;
                iprd = (IProduct)session.GetValue(Session.SessionKeys.Product);
            }

            if (iprd == null)
            {
                throw new FisException("Can not get Product object in " + part_check_type + " module");
            }
            return iprd;
        }

        private AstRuleInfo CheckAstRule(AstRuleInfo rule, IProduct product, IPart part)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
            IPartRepository     partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            AstRuleInfo ret = rule;
            if (ret.checkTp == "PrintLog")
            {
                if ( !string.IsNullOrEmpty(ret.checkItem))
                {
                    var printRep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
                                     
                    
                    if (!printRep.CheckPrintLogListByRange(product.ProId,ret.checkItem, ret.code) &&
                        !printRep.CheckPrintLogListByRange(product.CUSTSN, ret.checkItem,ret.code))
                    { 
                        
                        //throw error
                        throw new FisException("CQCHK1062", new string[] { product.ProId, ret.code, ret.checkItem });
                    }
                }
                else
                {
                     throw new FisException("CQCHK1064", new string[] { product.ProId, ret.code});
                }

                return null;

            }
            else if (ret.checkTp == "Value")
            {
                Delivery dn;
                ret.remark = "";
                switch (ret.checkItem)
                {
                    case "PoNo":
                        if (!string.IsNullOrEmpty(product.DeliveryNo))
                        {
                            dn = dnRep.Find(product.DeliveryNo);
                            if (dn != null && !string.IsNullOrEmpty(dn.PoNo))
                            {
                                ret.remark = dn.PoNo;
                            }
                        }
                        break;
                    case "CustPo":
                        if (!string.IsNullOrEmpty(product.DeliveryNo))
                        {
                            ret.remark = dnRep.GetDeliveryInfoValue(product.DeliveryNo, "CustPo");
                        }
                        break;
                    case "UUID":
                         ret.remark =  prodRep.GetProductInfoValue(product.ProId, "UUID");
                        break;
                    case "MAC":
                        ret.remark = product.MAC;
                        break;                    
                    case "WM":
                        ret.remark = prodRep.GetProductInfoValue(product.ProId, "WM");
                        if (!string.IsNullOrEmpty(ret.remark))
                        {
                            ret.remark = ret.remark.Replace(":", "");     //去掉字符':' 
                        }
                        break;
                    case "SN":
                        ret.remark = product.CUSTSN;
                        break;
                    
                    case "CHAR":
                        IList<ConstValueInfo> valueList =
                            partRep.GetConstValueListByType("ASTCHAR").Where(x => x.value.Trim() != "" && x.name == ret.custName).ToList();
                        if (valueList != null && valueList.Count > 0)
                        {
                            ret.remark = valueList[0].value;
                        }
                        break;                 
                    case "AST":
                        //跟之前邏輯有些不同
                        var astSn=( from p in product.ProductParts
                                         where p.BomNodeType== ret.tp &&
                                                   p.PartType== ret.code
                                          select p.PartSn).FirstOrDefault();
                        ret.remark =astSn;                       

                        break;
                    case "PartNo":
                        ret.remark = part.PN;
                        break;
                    default:                      
                        break;
                }

                if (string.IsNullOrEmpty(ret.remark))
                {
                    //throw error
                    throw new FisException("CQCHK1063", new string[] { product.ProId, ret.code, ret.checkItem });
                }

                return ret;
            }
            else if (ret.checkTp == "ProductLog")
            {
                if (!string.IsNullOrEmpty(ret.checkItem))
                {
                    IList<ProductLog> productLogList = product.ProductLogs;   
                    if (!productLogList.Any(x=>x.Station== ret.checkItem && x.Line == ret.code))
                    {
                        //throw error
                        throw new FisException("CQCHK1066", new string[] { product.ProId, ret.code, ret.checkItem });
                    }
                }
                else
                {
                    throw new FisException("CQCHK1067", new string[] { product.ProId, ret.code });
                }

                return null;
            }
            else
            {
                 throw new FisException("CQCHK1065", new string[] { product.ProId,ret.checkTp});                
            }
           
        }

        private class astItemRule
        {
            public string Code;
            public string AVPartNo;
            public IPart Part;
        }
    }
}
