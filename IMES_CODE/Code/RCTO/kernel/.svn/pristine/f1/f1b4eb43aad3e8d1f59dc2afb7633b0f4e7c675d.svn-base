// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-03-20   210003                       ITC-1360-1531
// Known issues:

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.AIO.CRLCM.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.AIO.CRLCM.Filter.dll")]
    public class Filter : IFilterModule 
    {
        private int _qty;
        private const string part_check_type = "ACRLCM";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展2阶，得到第一阶是PL的part其下阶(注意Qty需要相乘) [ PL->VC]，即PL和VC，
            IFlatBOM ret = null;
            var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            String vendor_code_info_value_string = "";
            String descr_info_value_string = "";
            var bom = (HierarchicalBOM)hierarchical_bom;
            if (bom.FirstLevelNodes != null)
            {
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("BM"))
                    {
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                        {
                            IList<PartInfo> part_infos = bom.FirstLevelNodes.ElementAt(i).Part.Attributes;
                            Boolean is_vc = false;
                            if (part_infos != null)
                            {
                                foreach (PartInfo part_info in part_infos)
                                {
                                    if (part_info.InfoType.Equals("VendorCode"))
                                    {
                                        is_vc = true;
                                        if (vendor_code_info_value_string.Length == 0)
                                        {
                                            vendor_code_info_value_string = part_info.InfoValue;
                                        }
                                        else
                                        {
                                            if (!vendor_code_info_value_string.Contains(part_info.InfoValue))
                                            {
                                                vendor_code_info_value_string += "," + part_info.InfoValue;
                                            }
                                        }
                                    }
                                    if (part_info.InfoType.Equals("Descr"))
                                    {
                                        if (descr_info_value_string.Length == 0)
                                        {
                                            descr_info_value_string = part_info.InfoValue;
                                        }
                                        else
                                        {
                                            if (!descr_info_value_string.Contains(part_info.InfoValue))
                                            {
                                                descr_info_value_string += "," + part_info.InfoValue;
                                            }
                                        }
                                    }
                                }
                            }
                            if (is_vc)
                            {
                                parts.Add(bom.FirstLevelNodes.ElementAt(i).Part);
                                _qty = bom.FirstLevelNodes.ElementAt(i).Qty;
                            }

                        }
                    }
                }
            }
            if (parts.Count > 0)
            {
                if (bom.FirstLevelNodes != null && bom.FirstLevelNodes.Count > 0)
                {
                    //                    _qty = bom.FirstLevelNodes.ElementAt(0).Qty;
                    var flat_bom_item = new FlatBOMItem(_qty, part_check_type, parts);
                    flat_bom_item.PartNoItem = vendor_code_info_value_string;
                    flat_bom_item.Descr = descr_info_value_string;
                    IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                    flat_bom_items.Add(flat_bom_item);
                    ret = new FlatBOM(flat_bom_items);
                }
            }
            return ret;
        }
        public bool CheckCondition(object node)
        {
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            IList<IBOMNode> child_nodes =((BOMNode)node).Children;
            bool ISHDDchild_node = false;
            if (child_nodes != null)
            {
                 foreach (IBOMNode child_node in child_nodes)
                   {
                       if (child_node.Part != null && child_node.Part.BOMNodeType.Equals("VC") && child_node.Part.Descr=="LCM")
                       {
                           ISHDDchild_node = true;
                            break;
                       }
                   }       
            }
            bool is_jgs = ((BOMNode)node).Part.Descr.Trim().Equals("LCM");

            return is_jgs && ISHDDchild_node;
        }
    }
}
