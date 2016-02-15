// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-02-29   210003                       ITC-1360-0934
// Known issues:

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.TouchScreen.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TouchScreen.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "TouchScreen";

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
                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("PL"))
                    {
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                        {
                            Boolean is_vc = false;
                            IList<PartInfo> part_infos = bom.FirstLevelNodes.ElementAt(i).Part.Attributes;
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
            //第一阶的Descr描述为( 'Touch screen'）
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_touch_screen = ((BOMNode)node).Part.Descr.Trim().Equals("Touch screen");
            int start_point_touch_screen = ((BOMNode)node).Part.Descr.Trim().IndexOf("Touch screen");
            if ((is_touch_screen && start_point_touch_screen == 0))
                return true;
            return false;
        }
    }
}
