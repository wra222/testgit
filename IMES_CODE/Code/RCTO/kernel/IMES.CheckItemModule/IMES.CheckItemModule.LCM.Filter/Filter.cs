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
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.LCM.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.LCM.Filter.dll")]
    public class Filter : IFilterModule, ITreeTraversal
    {
        private const string part_check_type = "LCM";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchical_bom"></param>
        /// <param name="station"></param>
        /// <param name="main_object"></param>
        /// <returns></returns>
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展3阶，得到第一阶是BM和P1的part其下阶和下下阶(注意Qty需要相乘) [ BM->KP->VC]或者[ P1->KP->VC]，即KP和VC，
            //以及第一阶是KP的part的第一阶及其下阶[KP->VC],即KP和VC
            IFlatBOM ret = null;
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            //var kp_parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            var tree_traversal = new TreeTraversal();
            Hashtable share_parts_set = new Hashtable();
            Hashtable descr_kp_part_set = new Hashtable();
            Hashtable info_value_string_set = new Hashtable();
            try
            {
                //处理BM->KP情况
                IList<QtyParts> bm_check_conditon_nodes = tree_traversal.BreadthFirstTraversal(bom.Root, "BM->KP", "KP", this, "BM");
                IList<ShareMaterialType> material_type = FilterShareMaterialType(bm_check_conditon_nodes);
                if (bm_check_conditon_nodes != null && bm_check_conditon_nodes.Count > 0)
                {
                    foreach (QtyParts bm_check_conditon_node in bm_check_conditon_nodes)
                    {
                        if (bom.FirstLevelNodes != null) //当bom.Root.Part不为空时，Root是首阶
                        {
                            IList<IBOMNode> bom_nodes = bom.FirstLevelNodes;
                            foreach (IBOMNode bom_node in bom_nodes)
                            {
                                if (bom_node.Part != null && bom_node.Part.BOMNodeType.Equals("BM"))
                                {
                                    IList<IBOMNode> child_nodes = bom_node.Children;
                                    if (child_nodes != null)
                                    {
                                        foreach (IBOMNode child_node in child_nodes)
                                        {
                                            if (child_node.Part != null && child_node.Part.BOMNodeType.Equals("KP") && child_node.Part.Attributes != null)
                                            {
                                                if (PartCompare(bm_check_conditon_node.Parts, child_node.Part))
                                                {
                                                    if (material_type.Count > 0)
                                                    {
                                                        foreach (ShareMaterialType type in material_type)
                                                        {
                                                            if (child_node.Part.Descr.Equals(type.Descr) && bm_check_conditon_node.Qty == type.Qty)
                                                            {
                                                                String share_material_key = type.Qty.ToString() + type.Descr;

                                                                if (!descr_kp_part_set.ContainsKey(share_material_key))
                                                                {
                                                                    descr_kp_part_set.Add(share_material_key, child_node.Part.Descr);
                                                                }
                                                                else
                                                                {
                                                                    if (!((String)descr_kp_part_set[share_material_key]).Contains(child_node.Part.Descr))
                                                                    {
                                                                        if (!((string)descr_kp_part_set[share_material_key]).Contains(child_node.Part.Descr))
                                                                        {
                                                                            descr_kp_part_set[share_material_key] += "," + child_node.Part.Descr;    
                                                                        }
                                                                    }
                                                                }

                                                                IList<PartInfo> part_infos = child_node.Part.Attributes;
                                                                if (part_infos != null && part_infos.Count > 0)
                                                                {
                                                                    foreach (PartInfo part_info in part_infos)
                                                                    {
                                                                        if (part_info.InfoType.Equals("VendorCode"))
                                                                        {
                                                                            if (!info_value_string_set.ContainsKey(share_material_key))
                                                                            {
                                                                                info_value_string_set.Add(share_material_key, part_info.InfoValue);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (!((String)info_value_string_set[share_material_key]).Contains(part_info.InfoValue))
                                                                                {
                                                                                    info_value_string_set[share_material_key] += "," + part_info.InfoValue;
                                                                                }
                                                                            }

                                                                            if (!share_parts_set.ContainsKey(share_material_key))
                                                                            {
                                                                                IList<IPart> share_parts = new List<IPart>();
                                                                                share_parts.Add(child_node.Part);
                                                                                share_parts_set.Add(share_material_key, share_parts);
                                                                            }
                                                                            else
                                                                            {
                                                                                ((IList<IPart>)share_parts_set[share_material_key]).Add(child_node.Part);
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
                    //if (material_type.Count > 0)
                    //{
                    //    foreach (ShareMaterialType type in material_type)
                    //    {
                    //        String share_material_key = type.Qty.ToString() + type.Descr;
                    //        var bm_flat_bom_item = new FlatBOMItem(type.Qty, part_check_type, (IList<IPart>)share_parts_set[share_material_key]);
                    //        bm_flat_bom_item.PartNoItem = (String)info_value_string_set[share_material_key];
                    //        bm_flat_bom_item.Descr = (String)descr_kp_part_set[share_material_key];
                    //        flat_bom_items.Add(bm_flat_bom_item);
                    //    }
                    //}
                }
                //处理P1->KP情况
                //descr_kp_part_set.Clear();
                //info_value_string_set.Clear();
                //share_parts_set.Clear();
                IList<QtyParts> p1_check_conditon_nodes = tree_traversal.BreadthFirstTraversal(bom.Root, "P1->KP", "KP", this, "P1");
                IList<ShareMaterialType> p1_material_type = FilterShareMaterialType(p1_check_conditon_nodes);
                foreach (ShareMaterialType type in p1_material_type)
                {
                    Boolean is_add = true;
                    foreach (ShareMaterialType mtype in material_type)
                    {
                        if (mtype.Descr.Trim().Equals(type.Descr.Trim()) && mtype.Qty == type.Qty)
                        {
                            is_add = false;
                            break;
                        }
                    }
                    if (is_add)
                    {
                        material_type.Add(type);
                    }
                }
                if (p1_check_conditon_nodes != null && p1_check_conditon_nodes.Count > 0)
                {
                    foreach (QtyParts bm_check_conditon_node in p1_check_conditon_nodes)
                    {
                        if (bom.FirstLevelNodes != null)
                        {
                            IList<IBOMNode> bom_nodes = bom.FirstLevelNodes;
                            foreach (IBOMNode bom_node in bom_nodes)
                            {
                                if (bom_node.Part.BOMNodeType.Equals("P1"))
                                {
                                    IList<IBOMNode> child_nodes = bom_node.Children;
                                    if (child_nodes != null)
                                    {
                                        foreach (IBOMNode child_node in child_nodes)
                                        {
                                            if (child_node.Part != null && child_node.Part.BOMNodeType.Equals("KP") && child_node.Part.Attributes != null)
                                            {
                                                if (PartCompare(bm_check_conditon_node.Parts, child_node.Part))
                                                {
                                                    if (material_type.Count > 0)
                                                    {
                                                        foreach (ShareMaterialType type in material_type)
                                                        {
                                                            if (child_node.Part.Descr.Equals(type.Descr) && bm_check_conditon_node.Qty == type.Qty)
                                                            {
                                                                String share_material_key = type.Qty.ToString() + type.Descr;


                                                                if (!descr_kp_part_set.ContainsKey(share_material_key))
                                                                {
                                                                    descr_kp_part_set.Add(share_material_key, child_node.Part.Descr);
                                                                }
                                                                else
                                                                {
                                                                    if (!((String)descr_kp_part_set[share_material_key]).Contains(child_node.Part.Descr))
                                                                    {
                                                                        if (!((string)descr_kp_part_set[share_material_key]).Contains(child_node.Part.Descr))
                                                                        {
                                                                            descr_kp_part_set[share_material_key] += "," + child_node.Part.Descr;    
                                                                        }
                                                                    }
                                                                }

                                                                IList<PartInfo> part_infos = child_node.Part.Attributes;
                                                                if (part_infos != null && part_infos.Count > 0)
                                                                {
                                                                    foreach (PartInfo part_info in part_infos)
                                                                    {
                                                                        if (part_info.InfoType.Equals("VendorCode"))
                                                                        {
                                                                            if (!info_value_string_set.ContainsKey(share_material_key))
                                                                            {
                                                                                info_value_string_set.Add(share_material_key, part_info.InfoValue);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (!((String)info_value_string_set[share_material_key]).Contains(part_info.InfoValue))
                                                                                {
                                                                                    info_value_string_set[share_material_key] += "," + part_info.InfoValue;
                                                                                }
                                                                            }
                                                                            if (!share_parts_set.ContainsKey(share_material_key))
                                                                            {
                                                                                IList<IPart> share_parts = new List<IPart>();
                                                                                share_parts.Add(child_node.Part);
                                                                                share_parts_set.Add(share_material_key, share_parts);
                                                                            }
                                                                            else
                                                                            {
                                                                                ((IList<IPart>)share_parts_set[share_material_key]).Add(child_node.Part);
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
                    //if (material_type.Count > 0)
                    //{
                    //    foreach (ShareMaterialType type in material_type)
                    //    {
                    //        String share_material_key = type.Qty.ToString() + type.Descr;
                    //        if (share_parts_set.ContainsKey(share_material_key))
                    //        {
                    //            var p1_flat_bom_item = new FlatBOMItem(type.Qty, part_check_type, (IList<IPart>)share_parts_set[share_material_key]);
                    //            p1_flat_bom_item.PartNoItem = (String)info_value_string_set[share_material_key];
                    //            p1_flat_bom_item.Descr = (String)descr_kp_part_set[share_material_key];
                    //            flat_bom_items.Add(p1_flat_bom_item);
                    //        }
                    //    }
                    //}
                }
                //处理首阶为KP情况
                //descr_kp_part_set.Clear();
                //info_value_string_set.Clear();
                //share_parts_set.Clear();
                List<IBOMNode> collect_gather_node = new List<IBOMNode>();
                List<QtyParts> kp_check_conditon_nodes = new List<QtyParts>();
                //IList<QtyParts> kp_check_conditon_nodes = tree_traversal.BreadthFirstTraversal(bom.Root, "KP", "KP", this, "KP");
                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        if (bom.FirstLevelNodes.ElementAt(i).Part != null)
                        {
                            if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("KP"))
                            {
                                if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                                {
                                    collect_gather_node.Add(bom.FirstLevelNodes.ElementAt(i));
                                    List<IPart> parts = new List<IPart>();
                                    parts.Add(bom.FirstLevelNodes.ElementAt(i).Part);
                                    QtyParts bom_item = new QtyParts(bom.FirstLevelNodes.ElementAt(i).Qty, parts);
                                    kp_check_conditon_nodes.Add(bom_item);
                                }
                            }
                        }
                    }
                }

                IList<ShareMaterialType> kp_material_type = FilterShareMaterialType(kp_check_conditon_nodes);
                foreach (ShareMaterialType type in kp_material_type)
                {
                    Boolean is_add = true;
                    foreach (ShareMaterialType mtype in material_type)
                    {
                        if (mtype.Descr.Trim().Equals(type.Descr.Trim()) && mtype.Qty == type.Qty)
                        {
                            is_add = false;
                            break;
                        }
                    }
                    if (is_add)
                    {
                        material_type.Add(type);
                    }
                }
                if ( kp_check_conditon_nodes.Count > 0)
                {
                    foreach (QtyParts bm_check_conditon_node in kp_check_conditon_nodes)
                    {
                        if (bom.FirstLevelNodes != null)
                        {
                            for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                            {
                                if (bom.FirstLevelNodes.ElementAt(i).Part != null)
                                {
                                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("KP") && bom.FirstLevelNodes.ElementAt(i).Part.Attributes != null)
                                    {
                                        IBOMNode kp_node = bom.FirstLevelNodes.ElementAt(i);
                                        //if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                                        if (PartCompare(bm_check_conditon_node.Parts, kp_node.Part))
                                        {
                                            if (material_type.Count > 0)
                                            {
                                                foreach (ShareMaterialType type in material_type)
                                                {
                                                    if (kp_node.Part.Descr.Equals(type.Descr) && bm_check_conditon_node.Qty == type.Qty)
                                                    {
                                                        String share_material_key = type.Qty.ToString() + type.Descr;

                                                        if (!descr_kp_part_set.ContainsKey(share_material_key))
                                                        {
                                                            descr_kp_part_set.Add(share_material_key, kp_node.Part.Descr);
                                                        }
                                                        else
                                                        {
                                                            if (!((String)descr_kp_part_set[share_material_key]).Contains(kp_node.Part.Descr))
                                                            {
                                                                descr_kp_part_set[share_material_key] += "," + kp_node.Part.Descr;
                                                            }
                                                        }

                                                        IList<PartInfo> part_infos = kp_node.Part.Attributes;
                                                        if (part_infos != null && part_infos.Count > 0)
                                                        {
                                                            foreach (PartInfo part_info in part_infos)
                                                            {
                                                                if (part_info.InfoType.Equals("VendorCode"))
                                                                {
                                                                    if (!info_value_string_set.ContainsKey(share_material_key))
                                                                    {
                                                                        info_value_string_set.Add(share_material_key, part_info.InfoValue);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (!((String)info_value_string_set[share_material_key]).Contains(part_info.InfoValue))
                                                                        {
                                                                            info_value_string_set[share_material_key] += "," + part_info.InfoValue;
                                                                        }
                                                                    }
                                                                    if (!share_parts_set.ContainsKey(share_material_key))
                                                                    {
                                                                        IList<IPart> share_parts = new List<IPart>();
                                                                        share_parts.Add(kp_node.Part);
                                                                        share_parts_set.Add(share_material_key, share_parts);
                                                                    }
                                                                    else
                                                                    {
                                                                        ((IList<IPart>)share_parts_set[share_material_key]).Add(kp_node.Part);
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
                    //if (material_type.Count > 0)
                    //{
                    //    foreach (ShareMaterialType type in material_type)
                    //    {
                    //        String share_material_key = type.Qty.ToString() + type.Descr;
                    //        if (share_parts_set.ContainsKey(share_material_key))
                    //        {
                    //            var kp_flat_bom_item = new FlatBOMItem(type.Qty, part_check_type, (IList<IPart>)share_parts_set[share_material_key]);
                    //            kp_flat_bom_item.PartNoItem = (String)info_value_string_set[share_material_key];
                    //            kp_flat_bom_item.Descr = (String)descr_kp_part_set[share_material_key];
                    //            flat_bom_items.Add(kp_flat_bom_item);
                    //        }
                    //    }
                    //}
                }
                if (material_type.Count > 0)
                {
                    foreach (ShareMaterialType type in material_type)
                    {
                        String share_material_key = type.Qty.ToString() + type.Descr;
                        if (share_parts_set.ContainsKey(share_material_key))
                        {
                            var kp_flat_bom_item = new FlatBOMItem(type.Qty, part_check_type, (IList<IPart>)share_parts_set[share_material_key]);
                            kp_flat_bom_item.PartNoItem = (String)info_value_string_set[share_material_key];
                            kp_flat_bom_item.Descr = (String)descr_kp_part_set[share_material_key];
                            flat_bom_items.Add(kp_flat_bom_item);
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
        public bool CheckCondition(object node)
        {
            //第一阶的Descr描述为(Descr like 'LCM%'）
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_contain_lcm = ((BOMNode)node).Part.Descr.Trim().Contains("LCM");
            int start_point_lcm = ((BOMNode)node).Part.Descr.Trim().IndexOf("LCM");

            if (is_contain_lcm && start_point_lcm == 0)
                return true;
            return false;
        }
        private IList<ShareMaterialType> FilterShareMaterialType(IList<QtyParts> qty_parts)
        {
            var ret = new List<ShareMaterialType>();
            if (qty_parts != null && qty_parts.Count > 0)
            {
                foreach (QtyParts qty_part in qty_parts)
                {
                    if (qty_part.Parts != null)
                    {
                        foreach (IPart part in qty_part.Parts)
                        {
                            var material_type = new ShareMaterialType { Qty = qty_part.Qty, Descr = part.Descr };
                            if (ret.Count == 0)
                            {
                                ret.Add(material_type);
                            }
                            else
                            {
                                bool is_exist = false;
                                foreach (ShareMaterialType share_material_type in ret)
                                {
                                    if (share_material_type.Descr.Trim().Equals(material_type.Descr.Trim()) && share_material_type.Qty == material_type.Qty)
                                    {
                                        is_exist = true;
                                    }
                                }
                                if (!is_exist)
                                {
                                    ret.Add(material_type);
                                }
                            }
                            //if (!ret.Contains(material_type))
                            //{
                            //    ret.Add(material_type);
                            //}
                        }
                    }
                }
            }
            return ret;
        }
    }
    internal class ShareMaterialType
    {
        public int Qty;
        public String Descr;
    }
}
