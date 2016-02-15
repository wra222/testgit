using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.PrivacyFilter.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.PrivacyFilter.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private const string part_check_type = "PrivacyFilter";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
//            Hashtable qty_share_parts_set = new Hashtable();
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
//                            parts.Add(part);
                            Boolean exist_share_part = false;
                            //parts.Add(part);
                            IList<PartInfo> part_infos = part.Attributes;
                            if (part_infos != null && part_infos.Count > 0)
                            {
                                foreach (PartInfo part_info in part_infos)
                                {

                                    if (part_info.InfoType.Equals("SUB"))
                                    {
                                        String[] share_parts = part_info.InfoValue.Trim().Split(';');
                                        if (share_parts.Length > 0)
                                        {
                                            exist_share_part = true;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                        for (int j = 0; j < share_parts.Length; j++)
                                        {
                                            string key = part.PN + "," + share_parts[j];
                                            if (!share_parts_set.ContainsKey(key))
                                            {
                                                IList<IPart> parts = new List<IPart>();
                                                parts.Add(part);
                                                share_parts_set.Add(key, parts);
//                                                qty_share_parts_set.Add(key, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                            }
                                            else
                                            {
                                                ((IList<IPart>)share_parts_set[key]).Add(part);
                                            }
                                        }
                                    }
                                }
                            }
                            if (!exist_share_part)
                            {
                                if (!share_parts_set.ContainsKey(part.PN))
                                {
                                    IList<IPart> parts = new List<IPart>();
                                    parts.Add(part);
                                    share_parts_set.Add(part.PN, parts);
//                                    qty_share_parts_set.Add(part.PN, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                }
                                else
                                {
                                    ((IList<IPart>)share_parts_set[part.PN]).Add(part);
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
//                        flat_bom_item.PartNoItem = (string)de.Key;
                        if (((string)de.Key).Substring(0, 3).Equals("DIB"))
                        {
                            flat_bom_item.PartNoItem = ((string)de.Key).Substring(3, ((string)de.Key).Length - 3);
                        }
                        else
                        {
                            flat_bom_item.PartNoItem = (string)de.Key;
                        }
                        flat_bom_item.Tp = "C2";
                        flat_bom_item.Descr = "Privacy Filter " + (string)de.Key;
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
            if (((BOMNode)node).Part.Descr.Trim().Equals("Privacy Filter") && ((BOMNode)node).Part.BOMNodeType.Trim().Equals("C2"))
                return true;
            return false;
        }
    }
}
