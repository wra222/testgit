// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// Known issues:

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.VGA.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.VGA.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "VGA";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展1阶，得到第一阶是MB的part [BomNodeType=MB]的MBCode[PartInfo.InfoValue(InfoType='MB')]，
            IFlatBOM ret = null;
            var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            String mb_info_value_string = "";
            if (bom.FirstLevelNodes != null)
            {
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("MB"))
                    {
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                        {
                            IList<PartInfo> part_infos = bom.FirstLevelNodes.ElementAt(i).Part.Attributes;
                            if (part_infos != null)
                            {
                                foreach (PartInfo part_info in part_infos)
                                {
                                    if (part_info.InfoType.Equals("MB"))
                                    {
                                        if (mb_info_value_string.Length == 0)
                                        {
                                            mb_info_value_string = part_info.InfoValue;
                                        }
                                        else
                                        {
                                            if (!mb_info_value_string.Contains(part_info.InfoValue))
                                            {
                                                mb_info_value_string += "," + part_info.InfoValue;
                                            }
                                        }
                                    }
                                }
                            }
                            _qty = bom.FirstLevelNodes.ElementAt(i).Qty;
                            IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                            parts.Add(part);
                        }
                    }
                }
            }
            if (parts.Count > 0)
            {
                var flat_bom_item = new FlatBOMItem(_qty, part_check_type, parts);
                flat_bom_item.Descr = bom.FirstLevelNodes.ElementAt(0).Part.Descr;
                flat_bom_item.PartNoItem = mb_info_value_string;
                IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                flat_bom_items.Add(flat_bom_item);
                ret = new FlatBOM(flat_bom_items);
            }
            return ret;
        }
        public bool CheckCondition(object node)
        {
            //MBCode[PartInfo.InfoValue(InfoType='MB')]，并且第一阶的PartInfo不存在InfoTyp='VGA' InfoValue='SV'
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_mb = false;
            bool is_vga_sv = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            foreach (PartInfo part_info in part_infos)
            {
                if (part_info.InfoType.Trim().Equals("MB") && !is_mb)
                {
                    if (!is_mb)  is_mb = true;
                    continue;
                }
                if (part_info.InfoType.Trim().Equals("VGA") && part_info.InfoValue.Trim().Equals("SV"))
                {
                    if (!is_vga_sv) is_vga_sv = true;
                    continue;
                }
            }
            if (is_mb && is_vga_sv)
                return true;
            return false;
        }
    }
}
