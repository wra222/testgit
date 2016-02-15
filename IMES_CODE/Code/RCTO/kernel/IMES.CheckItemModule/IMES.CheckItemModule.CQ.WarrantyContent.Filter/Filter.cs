﻿// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
//          Date                          Reason                            Name                        
// ==========   =======================      ============
// 2014-07-17                         Create                         He.jia-you 

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
using IMES.FisObject.Common.Model;

namespace IMES.CheckItemModule.CQ.WarrantyContent.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.WarrantyContent.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "WarrantyContent";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            IFlatBOM ret = null;
            bool NeedCheckContent = false;
            HierarchicalBOM bom = (HierarchicalBOM)hierarchical_bom;
            try
            {
                IProduct product = GetProduct(main_object);
                string CustSN = product.CUSTSN;
                if (!string.IsNullOrEmpty(product.DeliveryNo))
                {
                    string deliveryNo = product.DeliveryNo;
                    IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                    // string configID = dnRep.GetDeliveryInfoValue(deliveryNo, "ConfigID");
                    string reg = dnRep.GetDeliveryInfoValue(deliveryNo, "RegId");
                    if (reg != null && reg.Length == 3)
                    { reg = reg.Substring(1, 2); }
                    else
                    { reg = ""; }
                    string tp = dnRep.GetDeliveryInfoValue(deliveryNo, "ShipTp");
                    if ((CheckDomesticDN(reg)) && (tp == "CTO"))
                    {
                        NeedCheckContent = true;

                    }
                }
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                IList<ConstValueTypeInfo> lstConst = partRep.GetConstValueTypeList("PrintContentPartNoList");
                if (bom.FirstLevelNodes != null)
                {
                    IList<IBOMNode> NodeLst = bom.FirstLevelNodes;
                    if (NodeLst != null && NodeLst.Count > 0 && lstConst != null && lstConst.Count > 0)
                    {
                        foreach (IBOMNode ibomnode in NodeLst)
                        {
                            var partLabelList = (from p in lstConst
                                                 where p.value == ibomnode.Part.PN
                                                 select p.value).ToList();
                            if (partLabelList != null && partLabelList.Count > 0)
                            {
                                NeedCheckContent = true;
                                break;
                            }
                        }
                    }
                }
                if (NeedCheckContent)
                {
                    string virtualPN = "WarrantyContent" + "/" + CustSN;
                    string descr = "Check WarrantyContent";
                    IPart part = new Part()
                    {
                        PN = virtualPN,      //PN要跟 flat_bom_item.PartNoItem值一樣                  
                        CustPn = "",
                        Remark = "",
                        Descr = descr,
                        Descr2 = "",
                        Type = part_check_type,
                        Udt = DateTime.Now,
                        Cdt = DateTime.Now
                    };
                    var flat_bom_item = new FlatBOMItem(qty, part_check_type, new List<IPart>() { part });
                    flat_bom_item.PartNoItem = virtualPN;
                    flat_bom_item.Tp = part_check_type;
                    flat_bom_item.Descr = descr;
                    flat_bom_item.ValueType = "W"+CustSN; //存放Part match時檢查的值
                    flat_bom_items.Add(flat_bom_item);
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
        private bool CheckDomesticDN(string regId)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
             IList<string> domesticRegIdList= partRep.GetConstValueTypeList("CheckContentRegID").Select(x => x.value).ToList();
             return domesticRegIdList!=null && domesticRegIdList.Contains(regId);
        }
    }
}

