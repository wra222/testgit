﻿using System;
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

namespace IMES.CheckItemModule.Environment.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.Environment.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "Environment";
        private string partno = "";
        private string partvalue = "";
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
                if (bom.FirstLevelNodes != null)
                {
                    IList<IBOMNode> NodeLst = bom.FirstLevelNodes;
                    if (NodeLst != null && NodeLst.Count > 0)
                    {
                        foreach (IBOMNode ibomnode in NodeLst)
                        {
                            IPart currentPart = ibomnode.Part;
                            if (currentPart.BOMNodeType == "PL" && currentPart.Descr == "ENVIRONMENT label")
                            {
                                partvalue = currentPart.GetAttribute("AV");
                                NeedCheckContent = true;
                                partno=currentPart.PN;
                                break;
                            }

                        }
                    }
                }
                if (NeedCheckContent)
                {
                  
                    string descr = "Check Environment";
                    IPart part = new Part()
                    {
                        PN = partno,      //PN要跟 flat_bom_item.PartNoItem值一樣                  
                        CustPn = "",
                        Remark = "",
                        Descr = descr,
                        Descr2 = "",
                        Type = part_check_type,
                        Udt = DateTime.Now,
                        Cdt = DateTime.Now
                    };
                    var flat_bom_item = new FlatBOMItem(qty, part_check_type, new List<IPart>() { part });
                    flat_bom_item.PartNoItem = partno;
                    flat_bom_item.Tp = part_check_type;
                    flat_bom_item.Descr = descr;
                    flat_bom_item.ValueType = partvalue; //存放Part match時檢查的值
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
     
    }
}