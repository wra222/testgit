// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-12-19   210003                       ITC-1360-0886
// Known issues:

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.ServiceDoor.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.ServiceDoor.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "ServiceDoor";

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
                                _qty = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty;
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
            //第一阶的PartInfo中的Descr描述为( 'SERVICE DOOR'）和第一阶的Descr描述为( 'JGS'）
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_jgs = ((BOMNode)node).Part.Descr.Trim().Equals("JGS");
            bool is_servicedoor = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            foreach (PartInfo part_info in part_infos)
                if (part_info.InfoType.Trim().Equals("Descr") && part_info.InfoValue.ToUpper().Trim().Equals("SERVICE DOOR"))
                {
                    is_servicedoor = true;
                    break;
                }
            if (is_jgs && is_servicedoor)
                return true;
            return false;
        }
    }
}
