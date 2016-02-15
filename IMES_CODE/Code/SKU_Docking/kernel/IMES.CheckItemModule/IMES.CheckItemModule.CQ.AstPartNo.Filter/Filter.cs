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
using IMES.CheckItemModule.Utility;

namespace IMES.CheckItemModule.CQ.AstPartNo.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.AstPartNo.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "AstPartNo";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            IFlatBOM ret = null;
            try
            {
                   UtilityCommonImpl utl= UtilityCommonImpl.GetInstance();
                   Session session=utl.GetSession(main_object,part_check_type);
                  IList<AstDefineInfo> astDefineInfo =(IList<AstDefineInfo>) session.GetValue(Session.SessionKeys.NeedCombineAstDefineList);
                  if (astDefineInfo==null || astDefineInfo.Count==0)
                  {
                      return ret;
                  }
                  IList<IPart> astPartInfo =(IList<IPart>) session.GetValue(Session.SessionKeys.NeedCombineAstPartList);
                  if (astPartInfo==null || astPartInfo.Count==0)
                  {
                      return ret;
                  }

                  var share_parts_set = astPartInfo.Where(x => astDefineInfo.Any(y => y.AstType == x.BOMNodeType &&
                                                                                                                        y.AstCode == x.Descr &&
                                                                                                                        y.CheckUnique=="N" &&
                                                                                                                        y.NeedScanSN == "Y")).ToList();
                    

                if (share_parts_set.Count > 0)
                {
                    foreach (IPart part in share_parts_set)
                    {
                        var flat_bom_item = new FlatBOMItem(qty, part_check_type, new List<IPart>(){part});
                        flat_bom_item.PartNoItem =part.PN; 
                        flat_bom_item.Tp =  part.BOMNodeType;
                        flat_bom_item.Descr = part.Descr;
                        flat_bom_item.ValueType = ""; //存放Part match時檢查的值
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
                                     
                    
                    if (!printRep.CheckPrintLogListByRange(product.ProId,ret.checkItem) &&
                        !printRep.CheckPrintLogListByRange(product.CUSTSN, ret.checkItem))
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
                                         where p.BomNodeType=="AT" &&
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
                    if (!productLogList.Any(x=>x.Station== ret.checkItem))
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

        public bool checkUnique(string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                return true;
            }
            else if (comment.ToUpper().Contains("UNIQUE=N"))
            {
                return false;
            }
            else
            {
                return true;
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
