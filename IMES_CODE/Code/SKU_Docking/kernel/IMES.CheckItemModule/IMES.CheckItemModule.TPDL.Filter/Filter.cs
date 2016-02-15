﻿// INVENTEC corporation (c)2009 all rights reserved. 
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
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.TPDL.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TPDL.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "TPDL";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展2阶，得到第一阶是PL的part其下阶(注意Qty需要相乘) [ PL->VC]，即PL和VC，第一阶的PartInfo中的Descr描述为( 'TPDL'）和第一阶的Descr描述为( 'JGS'）
            IFlatBOM ret = null;

            var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;

            String vendor_code_info_value_string = "";
            String descr_info_value_string = "";
            try
            {
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

                                    }
                                }
                                if (is_vc)
                                {
                                    string partDescr = bom.FirstLevelNodes.ElementAt(i).Part.GetAttribute("Descr");
                                    if (descr_info_value_string.Length == 0)
                                    {
                                        descr_info_value_string = partDescr;
                                    }
                                    else
                                    {
                                        if (!descr_info_value_string.Contains(partDescr))
                                        {
                                            descr_info_value_string += "," + partDescr;
                                        }
                                    }
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
            }
            catch (Exception e)
            {
                throw;
            }

            return ret;
        }
 
        public bool CheckCondition(object node)
        {
            //PC 阶下存在PS Part
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            //bool is_tpdl = ((BOMNode)node).Part.Descr.Trim().Equals("TPDL");
            bool is_tpdl = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            if (part_infos != null)
            {
                foreach (var part_info in part_infos)
                {
                    if (part_info.InfoType.Equals("Descr") && part_info.InfoValue.Trim().Equals("TPDL"))
                    {
                        is_tpdl = true;
                        break;
                    }
                }
            }
            bool is_jgs = ((BOMNode)node).Part.Descr.Trim().Equals("JGS");
            if (is_tpdl && is_jgs)
                return true;
            return false;
        }
    }
}