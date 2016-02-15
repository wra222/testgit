// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.WarrantyCard.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.WarrantyCard.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "WarrantyCard";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();  //<Descr, PartList>
            Hashtable share_part_no_set = new Hashtable(); //<Descr, PartNoItemString(e.g. Pn1,Pn2...)>
            //问题：station参数用不上。因为在hierarchicalBOM中，没有与station相关的字段。
            IFlatBOM ret = null;
//            List<IPart> parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            HierarchicalBOM bom = (HierarchicalBOM)hierarchical_bom;

            try
            {
                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                        {
                            IPart part = ((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Part;
                            //parts.Add(part);
                            if (!share_parts_set.ContainsKey(part.Descr))
                            {
                                IList<IPart> parts = new List<IPart>();
                                parts.Add(part);
                                share_parts_set.Add(part.Descr, parts);
                                if (part.PN.Substring(0, 3).Equals("DIB"))
                                {
                                    share_part_no_set.Add(part.Descr, part.PN.Substring(3, part.PN.Length - 3));
                                }
                                else
                                {
                                    share_part_no_set.Add(part.Descr, part.PN);
                                }
                            }
                            else
                            {
                                ((IList<IPart>)share_parts_set[part.Descr]).Add(part);
                                if (!((string)share_part_no_set[part.Descr]).Contains(part.PN))
                                {
                                    if (part.PN.Substring(0, 3).Equals("DIB"))
                                    {
                                        share_part_no_set[part.Descr] += "," + part.PN.Substring(3, part.PN.Length - 3);
                                    }
                                    else
                                    {
                                        share_part_no_set[part.Descr] += "," + part.PN;
                                    }
                                }
                            }
                        }
                    }
                }
//                if (parts.Count > 0)
//                {
//                    FlatBOMItem flat_bom_item = new FlatBOMItem(qty, part_check_type, parts);
//                    flat_bom_item.PartNoItem = parts.ElementAt(0).PN;
//                    flat_bom_item.Descr = parts.ElementAt(0).Descr;
//                    IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
//                    flat_bom_items.Add(flat_bom_item);
//                    ret = new FlatBOM(flat_bom_items);
//                }
                if (share_parts_set.Count > 0)
                {
                    foreach (DictionaryEntry de in share_parts_set)
                    {
                        var flat_bom_item = new FlatBOMItem(qty, part_check_type, (IList<IPart>)de.Value);
                        flat_bom_item.PartNoItem = (string)share_part_no_set[de.Key];
                        flat_bom_item.Tp = "PR";
                        flat_bom_item.Descr = "Warranty Card";
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
        public bool CheckCondition(object node)
        {
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_here = ((BOMNode)node).Part.Descr.Trim().Contains("Warranty Card");
            int start_point = ((BOMNode)node).Part.Descr.Trim().LastIndexOf("Warranty Card");
            bool is_pr = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("PR");
            if (is_here && start_point == 0 && is_pr)
                return true;
            return false;
        }
    }
}
