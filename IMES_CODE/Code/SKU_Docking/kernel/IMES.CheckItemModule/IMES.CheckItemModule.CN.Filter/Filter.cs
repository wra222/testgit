using System;
// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// Known issues:
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.CN.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CN.Filter.dll")]
    public class Filter : IFilterModule
    {
        //private int qty = 1;//问题：Pizza Kitting。该情况下，如何计算qty。
        private const string part_check_type = "CN";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            
            IFlatBOM ret = null;
            Hashtable share_parts_set = new Hashtable();
            Hashtable qty_set = new Hashtable();
            Hashtable descr_parts_set = new Hashtable();

            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;

            try
            {
                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                        {
                            IPart part = ((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Part;
                            if (part != null)
                            {
                                if (part.PN.Trim().Substring(0, 5).Equals("DUMMY"))
                                {
                                    IList<PartInfo> part_infos = part.Attributes;
                                    if (part_infos != null)
                                    {
                                        foreach (PartInfo part_info in part_infos)
                                        {
                                            if (part_info.InfoType.Trim().Equals("PN"))
                                            {
                                                part.SetKey(part.PN);
                                                part.PN = part_info.InfoValue;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (!part.PN.Substring(0,3).Equals("MMI"))
                                {
                                    if (share_parts_set.ContainsKey(part.PN))
                                    {
                                        ((IList<IPart>)share_parts_set[part.PN]).Add(part);
                                        qty_set[part.PN] = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty;
                                        descr_parts_set[part.PN] = "," + part.Descr;
                                    }
                                    else
                                    {
                                        IList<IPart> parts = new List<IPart>();
                                        parts.Add(part);
                                        share_parts_set.Add(part.PN, parts);
                                        qty_set.Add(part.PN, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                        descr_parts_set.Add(part.PN,part.Descr);
                                    }
                                }
                            }
                        }
                    }
                }
//                if (parts.Count > 0)
//                {
//                    IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
//                    foreach (IPart part in parts)
//                    {
//                        IList<IPart> bom_item_parts = new List<IPart>();
//                        bom_item_parts.Add(part);
//                        var flat_bom_item = new FlatBOMItem(qty, part_check_type, bom_item_parts);
//                        flat_bom_item.PartNoItem = part.PN;
//                        flat_bom_item.Descr = part.Descr;
//                        flat_bom_item.Model = bom.Model;
//                        flat_bom_items.Add(flat_bom_item);
//                    }
//                    ret = new FlatBOM(flat_bom_items);
//                }
                if (share_parts_set.Count > 0)
                {
                    IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                    foreach (DictionaryEntry de in share_parts_set)
                    {
                        var flat_bom_item = new FlatBOMItem((int) qty_set[de.Key], part_check_type, (IList<IPart>)de.Value);
                        flat_bom_item.PartNoItem = (string)de.Key;
                        flat_bom_item.Descr = (string)descr_parts_set[de.Key];
                        flat_bom_item.Model = bom.Model;
                        flat_bom_items.Add(flat_bom_item);
                    }
                    if (flat_bom_items.Count > 0)
                    {
                        ret = new FlatBOM(flat_bom_items);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return ret;
        }
        public bool CheckCondition(object node)
        {
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            //根据Model展下阶得到CN
            bool is_cn = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("CN");
            if (is_cn)
                return true;
            return false;
        }
    }
}
