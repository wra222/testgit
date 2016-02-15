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
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure.Util;
using IMES.Resolve.Common;

namespace IMES.CheckItemModule.CheckValue.Filter
{
    [Export(typeof(IFilterModuleEx))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CheckValue.Filter.dll")]
    public class Filter : IFilterModuleEx
    {
        private int qty = 1;
        //private string part_check_type = "CheckValue";
        private const string CheckItemTypeRule = "CheckItemTypeRule";
        private const string InputMsg = "Please Input ";
        public object FilterBOMEx(object hierarchical_bom, string station, string checkItemType, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>(); 
            IFlatBOM ret = null;
            try
            { 
                IList<CheckItemTypeRuleDef> filteredChkItemRules = new List<CheckItemTypeRuleDef>();
                UtilityCommonImpl utl = UtilityCommonImpl.GetInstance();
                IList<CheckItemTypeRuleDef> lstChkItemRule = utl.GetCheckItemTypeRule(main_object, checkItemType, station);
                if ( lstChkItemRule!=null && 
                    lstChkItemRule.Count>  0)
               {
                   
                    Session session = null;
                    Delivery dn = null;
                    Part part = null;
                    IProduct product = GetProduct(main_object,out session);
                    IMB mb = GetPCB(main_object , out session);
                    if (product == null && mb == null)
                    {
                        throw new FisException("Can not get Product and PCB object in " + checkItemType + " module");
                    }

                    if (product != null &&
                      !string.IsNullOrEmpty(product.PCBID) &&
                      mb == null)
                    {
                        IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                        mb = mbRep.Find(product.PCBID);
                    }
                    
                    if (product != null && 
                        !string.IsNullOrEmpty(product.DeliveryNo))
                    {
                          IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                          dn=dnRep.Find(product.DeliveryNo);
                    }
                    if (mb != null && mb.ModelObj!=null) 
                    {
                       part=(Part)mb.ModelObj.PartObj;
                    }

                                     
                    int index = 1;
                    foreach (CheckItemTypeRuleDef item in lstChkItemRule)
                    {
                        string virtualPN = null;
                        string descr = null;
                        string matchValue = null;
                        utl.FilterMatchRule.ParseCheckItemTypeRuleGroupName(item);
                        if (!string.IsNullOrEmpty(item.FilterExpression))
                        {
                            if (!ResolveExpression.InvokeCondition(product, dn, CheckItemTypeRule + item.ID.ToString(), item.FilterExpression, true))
                            {
                                continue;
                            }
                        }

                        if (!string.IsNullOrEmpty(item.PartDescr) &&
                            !string.IsNullOrEmpty(item.PartType))
                        {
                            descr = InputMsg + item.PartDescr + GlobalConstName.SlashStr + item.PartType;
                            virtualPN = checkItemType + index.ToString() + GlobalConstName.SlashStr + item.PartDescr + GlobalConstName.SlashStr + item.PartType;
                            matchValue = ResolveValue.GetValueWithoutError(product, mb, dn, part, item.PartDescr, GlobalConstName.DotChar) ?? string.Empty;
                            matchValue = matchValue + ResolveValue.GetValue(product, mb, dn, part, item.PartType, GlobalConstName.DotChar);
                            if (!string.IsNullOrEmpty(matchValue))
                            {
                                var flat_bom_item = createFlatBOMItem(virtualPN, descr,
                                                                                            checkItemType,
                                                                                            matchValue,
                                                                                            item);
                                flat_bom_items.Add(flat_bom_item);
                            }          
                        }
                        else if (!string.IsNullOrEmpty(item.PartDescr))
                        {
                            descr = InputMsg + item.PartDescr;
                            virtualPN = checkItemType + index.ToString() + GlobalConstName.SlashStr + item.PartDescr;
                            matchValue = ResolveValue.GetValueWithoutError(product, mb, dn, part, item.PartDescr, '.');
                            if (!string.IsNullOrEmpty(matchValue))
                            {
                                var flat_bom_item = createFlatBOMItem(virtualPN, descr,
                                                                                            checkItemType,
                                                                                            matchValue,
                                                                                            item);
                                flat_bom_items.Add(flat_bom_item);
                            }                           
                        }
                        else if (!string.IsNullOrEmpty(item.PartType))
                        {
                            descr = InputMsg + item.PartType;
                            virtualPN = checkItemType + index.ToString() + GlobalConstName.SlashStr + item.PartType;
                            matchValue = ResolveValue.GetValue(product, mb, dn, part, item.PartType, GlobalConstName.DotChar);
                            if (!string.IsNullOrEmpty(matchValue))
                            {
                                var flat_bom_item = createFlatBOMItem(virtualPN, descr,
                                                                                            checkItemType,
                                                                                            matchValue,
                                                                                            item);
                                flat_bom_items.Add(flat_bom_item);
                            }

                        }

                        index++;
                    }
                }              
              
                if (flat_bom_items.Count > 0)
                {
                    ret = new FlatBOM(flat_bom_items);
                }
            }
            catch 
            {
                throw;
            }
            return ret;
        }        

        private IProduct GetProduct(object main_object, out Session session)
        {
            string objType = main_object.GetType().ToString();
            session = null;
            IProduct iprd = null;
            Type mainType = main_object.GetType();
            if (mainType.Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
                iprd = (IProduct)main_object;
            }
            else if (mainType.Equals(typeof(IMES.Infrastructure.Session)))
            {
                session = (IMES.Infrastructure.Session)main_object;
                iprd = (IProduct)session.GetValue(Session.SessionKeys.Product);
            }

            //if (iprd == null)
            //{
            //    throw new FisException("Can not get Product object in " + part_check_type + " module");
            //}
            return iprd;
        }

        private IMB GetPCB(object main_object, out Session session)
        {
            string objType = main_object.GetType().ToString();
            session = null;
            IMB iMB = null;
            Type mainType = main_object.GetType();
            if (mainType.Equals(typeof(IMES.FisObject.PCA.MB.IMB)))
            {
                iMB = (IMB)main_object;
            }
            else if (mainType.Equals(typeof(IMES.Infrastructure.Session)))
            {
                session = (IMES.Infrastructure.Session)main_object;
                iMB = (IMB)session.GetValue(Session.SessionKeys.MB);
            }

            //if (iMB == null)
            //{
            //    throw new FisException("Can not get MB object in " + part_check_type + " module");
            //}
            return iMB;
        }

        private FlatBOMItem createFlatBOMItem(string virtualPN, 
                                                                      string descr,  
                                                                     string checkItemType, 
                                                                    string valueType, 
                                                                    CheckItemTypeRuleDef ruleDefine)
        {
            DateTime now = DateTime.Now;
            IPart part = new Part()
            {
                PN = virtualPN,      //PN要跟 flat_bom_item.PartNoItem值一樣                  
                CustPn = string.Empty,
                Remark = string.Empty,
                Descr = descr,
                Descr2 = string.Empty,
                Type = checkItemType,
                Udt = now,
                Cdt = now
            };
            FlatBOMItem flat_bom_item = new FlatBOMItem(qty, checkItemType, new List<IPart>() { part });
            flat_bom_item.PartNoItem = part.PN;
            flat_bom_item.Tp = checkItemType;
            flat_bom_item.Descr = descr;
            flat_bom_item.ValueType = valueType; //存放Part match時檢查的值
            if (ruleDefine != null)
            {
                flat_bom_item.CheckItemTypeRuleList = new List<CheckItemTypeRuleDef> { ruleDefine };
            }
            return flat_bom_item;
        }
    }
}
