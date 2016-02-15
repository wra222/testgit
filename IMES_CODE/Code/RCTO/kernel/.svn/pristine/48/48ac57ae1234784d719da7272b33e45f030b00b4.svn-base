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
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.V2.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.V2.Filter.dll")]
    public class Filter : IFilterModule, ITreeTraversal
    {
        //private int qty = 1;//问题：Pizza Kitting。该情况下，如何计算qty。
        private const string part_check_type = "V2";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchical_bom"></param>
        /// <param name="station"></param>
        /// <param name="main_object"></param>
        /// <returns></returns>
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展3阶，得到第一阶是V2的part其下下阶(注意Qty需要相乘) [ V2一定会有VC下阶: V2->P1->KP->VC]
            //界面上显示Part No和Description是P1对应的Part；不同的P1作为多条记录显示在界面上，即相同的P1只显示一条，
            //其VC合并，相同的P1对应的数量认为是一样的，数量只取第一条记录的Qty即可
            //删掉PN前3位是MMI的Part ((GYJ）前三位只是代号，可能有DIB,MMI等)
            IFlatBOM ret = null;
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            //var kp_parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            TreeTraversal tree_traversal = new TreeTraversal();
            Hashtable part_group_set = new Hashtable();
            Hashtable descr_part_set = new Hashtable();
            IList<KPVendorCode> kpVCList = new List<KPVendorCode>();
            //Hashtable info_value_string_set = new Hashtable();
            try
            {
                IList<QtyParts> check_conditon_nodes = tree_traversal.BreadthFirstTraversal("MMI",bom.Root, "V2->P1->KP","P1",this);
                IList<PartGroupType> part_group_type = FilterPartGroupType(check_conditon_nodes);
                if (check_conditon_nodes != null && check_conditon_nodes.Count > 0)
                {
                     List<IFlatBOMItem> items = new List<IFlatBOMItem>();
                    foreach (QtyParts check_conditon_node in check_conditon_nodes)
                    {
                        //FlatBOMItem item = new FlatBOMItem(check_conditon_node.Qty,part_check_type,check_conditon_node.Parts);
                        //items.Add(item);
                        if (bom.FirstLevelNodes != null) //当bom.Root.Part不为空时，Root是首阶
                        {
                            IList<IBOMNode> bom_nodes = bom.FirstLevelNodes;
                            foreach (IBOMNode bom_node in bom_nodes)
                            {
                                if (bom_node.Part.BOMNodeType.Equals("V2"))
                                {
                                    IList<IBOMNode> p1_child_nodes = bom_node.Children;
                                    if (p1_child_nodes != null)
                                    {
                                        foreach (IBOMNode p1_child_node in p1_child_nodes)
                                        {
                                            if (p1_child_node.Part != null && p1_child_node.Part.BOMNodeType.Equals("P1"))
                                            {
                                                if (PartCompare(check_conditon_node.Parts, p1_child_node.Part))
                                                {
                                                    if (part_group_type.Count > 0)
                                                    {
                                                        foreach (PartGroupType type in part_group_type)
                                                        {
                                                            if (p1_child_node.Part.PN.Equals(type.PN))
                                                            {
                                                                String part_group_key = type.PN;

                                                                if (!descr_part_set.ContainsKey(part_group_key))
                                                                {
                                                                    descr_part_set.Add(part_group_key, p1_child_node.Part.Descr);
                                                                }
                                                                else
                                                                {
                                                                    if (!((String)descr_part_set[part_group_key]).Contains(p1_child_node.Part.Descr))
                                                                    {
                                                                        descr_part_set[part_group_key] += "," + p1_child_node.Part.Descr;
                                                                    }
                                                                }
                                                                IList<IBOMNode> kp_child_nodes = p1_child_node.Children;
                                                                //String info_value_string = "";
                                                                if (kp_child_nodes != null)
                                                                {
                                                                    foreach (IBOMNode kp_child_node in kp_child_nodes)
                                                                    {
                                                                        if (kp_child_node.Part != null && kp_child_node.Part.BOMNodeType.Equals("KP"))
                                                                        {
                                                                            IList<PartInfo> part_infos = kp_child_node.Part.Attributes;
                                                                            if (part_infos != null && part_infos.Count > 0)
                                                                            {
                                                                                foreach (PartInfo part_info in part_infos)
                                                                                {
                                                                                    if (part_info.InfoType.Equals("VendorCode"))
                                                                                    {
                                                                                        //p1_child_node.Part.AddAttribute(part_info);
                                                                                        kpVCList.Add(new KPVendorCode
                                                                                        {
                                                                                            PartNo = p1_child_node.Part.PN,
                                                                                            VendorCode = part_info.InfoValue
                                                                                        }
                                                                                            );
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (!part_group_set.ContainsKey(part_group_key))
                                                                {
                                                                    IList<IPart> share_parts = new List<IPart>();
                                                                    share_parts.Add(p1_child_node.Part);
                                                                    part_group_set.Add(part_group_key, share_parts);
                                                                }
                                                                else
                                                                {
//                                                                    ((IList<IPart>)part_group_set[part_group_key]).Add(p1_child_node.Part);
                                                                    IList<IBOMNode> identical_p1_child_nodes = p1_child_node.Children;
                                                                    //String info_value_string = "";
                                                                    if (kp_child_nodes != null)
                                                                    {
                                                                        foreach (IBOMNode identical_p1_child_node in identical_p1_child_nodes)
                                                                        {
                                                                            if (identical_p1_child_node.Part != null && identical_p1_child_node.Part.BOMNodeType.Equals("KP"))
                                                                            {
                                                                                IList<PartInfo> part_infos = identical_p1_child_node.Part.Attributes;
                                                                                if (part_infos != null && part_infos.Count > 0)
                                                                                {
                                                                                    foreach (PartInfo part_info in part_infos)
                                                                                    {
                                                                                        if (part_info.InfoType.Equals("VendorCode"))
                                                                                        {
//                                                                                            p1_child_node.Part.AddAttribute(part_info);
                                                                                            // ((IList<IPart>)part_group_set[part_group_key]).ElementAt(0).AddAttribute(part_info);

                                                                                           kpVCList.Add(new KPVendorCode
                                                                                            {
                                                                                                PartNo = ((IList<IPart>)part_group_set[part_group_key]).ElementAt(0).PN,
                                                                                                VendorCode = part_info.InfoValue
                                                                                            });
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (part_group_type.Count > 0)
                    {
                        foreach (PartGroupType type in part_group_type)
                        {
                            String share_material_key = type.PN;
                            var bm_flat_bom_item = new FlatBOMItem(type.Qty, part_check_type, (IList<IPart>)part_group_set[share_material_key]);
                            bm_flat_bom_item.PartNoItem = type.PN;
                            bm_flat_bom_item.Descr = (String)descr_part_set[share_material_key];
                            flat_bom_items.Add(bm_flat_bom_item);
                        }
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
        //没有用到
        public bool CheckCondition(object node)
        {
            //根据Model展3阶，得到第一阶是V2的part其下下阶(注意Qty需要相乘) [ V2一定会有VC下阶: V2->P1->KP->VC]
            //删掉PN前3位是MMI的Part ((GYJ）前三位只是代号，可能有DIB,MMI等)
            //bool is_CN = ((BOMNode)node).Part.Type.Trim().Equals("V2");

            //if (is_CN)
                return true;
            //return false;
        }
        private Boolean PartCompare(IList<IPart> parts, IPart part)
        {
            if (part != null && parts != null)
            {
                foreach (var apart in parts)
                {
                    if (part.PN.Trim().Equals(apart.PN.Trim()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private IList<PartGroupType> FilterPartGroupType(IList<QtyParts> qty_parts)
        {
            var ret = new List<PartGroupType>();
            if (qty_parts != null && qty_parts.Count > 0)
            {
                foreach (QtyParts qty_part in qty_parts)
                {
                    if (qty_part.Parts != null)
                    {
                        foreach (IPart part in qty_part.Parts)
                        {
                            var part_group_type = new PartGroupType { Qty = qty_part.Qty, PN = part.PN };
                            Boolean have_pn = false;
                            foreach (PartGroupType group_type in ret)
                            {
                                if(group_type.PN.Equals(part_group_type.PN))
                                {
                                    have_pn = true;
                                    break;
                                }
                            }
                            if (!have_pn)
                            {
                                ret.Add(part_group_type);
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
    internal class PartGroupType
    {
        public int Qty;
        public String PN;
    }
}
