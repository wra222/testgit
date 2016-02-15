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

namespace IMES.CheckItemModule.CQ.SmartCard.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.SmartCard.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "SmartCard";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();

            IFlatBOM ret = null;
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            HierarchicalBOM bom = (HierarchicalBOM)hierarchical_bom;
            Hashtable hQtyPNofSC = new Hashtable();
            try
            {
                if (bom.FirstLevelNodes != null)
                {

                    IList<IBOMNode> bomNodeList = bom.FirstLevelNodes;

                    var smartCardPartList = from p in bomNodeList
                                            where p.Part.Attributes.Any(x => x.InfoType == "Descr" && x.InfoValue.ToUpper() == "SMART CARD")
                                            select new { Qty = p.Qty, PartNo = p.Part.PN };
                    foreach (var item in smartCardPartList)
                    {
                        if (hQtyPNofSC.Contains(item.Qty))
                        {
                            hQtyPNofSC[item.Qty] = (string)hQtyPNofSC[item.Qty] + "," + item.PartNo;
                        }
                        else
                        {
                            hQtyPNofSC.Add(item.Qty, item.PartNo);
                        }
                    }

                    foreach (DictionaryEntry de in hQtyPNofSC)
                    {
                        IPart part = new Part()
                        {
                            PN = (string)de.Value,      //PN要跟 flat_bom_item.PartNoItem值一樣                  
                            CustPn = "",
                            Remark = "",
                            Descr = "Smart Card Reader",
                            Descr2 = "",
                            Type = "SmartCard",
                            Udt = DateTime.Now,
                            Cdt = DateTime.Now
                        };

                        var flat_bom_item = new FlatBOMItem((int)de.Key, part_check_type, new List<IPart>() { part });
                        flat_bom_item.PartNoItem = (string)de.Value;
                        flat_bom_item.Tp = "SmartCard";
                        flat_bom_item.Descr = "Check Smart Card Reader";
                        flat_bom_item.ValueType = (string)de.Value; //存放Part match時檢查的值
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

    }
}
