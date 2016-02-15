using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;


namespace IMES.CheckItemModule.CQ.HDDFrame.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.HDDFrame.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "HDDFrame";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展2阶，得到第一阶是PL的part其下阶(注意Qty需要相乘) [ PL->VC]，即PL和VC，
            IFlatBOM ret = null;
            var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            String vendor_code_info_value_string = "";
            String descr_info_value_string = "";

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
            //第一阶的PartInfo中的Descr描述PartInfo.InfoValue(InfoType='Descr')  Like ( '%Frame%'）且第一阶的Descr描述为( 'JGS'）
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_jgs = ((BOMNode)node).Part.Descr.Trim().Contains("JGS");
            bool contain_Frame = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            foreach (PartInfo part_info in part_infos)
            {
                if (part_info.InfoType.Equals("Descr"))
                {
                    contain_Frame = part_info.InfoValue.Contains("HDD Branket");
                    break;
                }
            }
            if (is_jgs && contain_Frame)
                return true;
            return false;
        }
    }
}
