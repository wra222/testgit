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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;

namespace IMES.CheckItemModule.TPCB.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TPCB.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "TPCB";

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
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i), main_object))
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
        public bool CheckCondition(object node, object main_object)
        {
            //第一阶的PartInfo中的Descr描述为( 'TPCB'）和第一阶的Descr描述为( 'JGS'）并且第一阶的PartNo的前三码不是'151'
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_jgs = ((BOMNode)node).Part.Descr.Trim().Equals("JGS");
            bool is_151 = ((BOMNode)node).Part.PN.Substring(0, 3).Equals("151");
            bool is_btcb = false;
            bool is_virtualbtcb = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            foreach (PartInfo part_info in part_infos)
                if (part_info.InfoType.Trim().Equals("Descr") && part_info.InfoValue.Trim().Equals("TPCB"))
                {
                    is_btcb = true;
                    break;
                }
            string vcdescrAttribute = ((BOMNode)node).Part.GetAttribute("VCDescr");
            if (!string.IsNullOrEmpty(vcdescrAttribute)
                && vcdescrAttribute.ToUpper().Contains("VIRTUAL"))
            {
                is_virtualbtcb = true;
            }
            //默认需要屏蔽掉151 的PN,如果有维护就需要展出
            if (NeedFiler151PN(GetProduct(main_object)))
            {
                is_151 = false;
            }
            if (is_jgs && is_btcb && !is_151 && !is_virtualbtcb)
                return true;
            return false;
        }
        private bool NeedFiler151PN( IProduct p)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            return partRep.GetConstValueTypeList("TPCBNeedFilter151PN").Where(x => x.value == p.Model || x.value == p.Family).Any();
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
