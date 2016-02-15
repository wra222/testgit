// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-03-07   210003                       ITC-1360-1175
// 2011-03-07   210003                       ITC-1360-1181
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.CQ.CT.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CT.Filter.dll")]
    public class Filter : IFilterModule, ITreeTraversal
    {
        private const string part_check_type = "CT";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IFlatBOM ret = null;
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            HierarchicalBOM bom = (HierarchicalBOM)hierarchical_bom;
            Hashtable share_parts_set = new Hashtable();
            Hashtable share_part_no_set = new Hashtable();
            Hashtable part_no_set = new Hashtable();
            Hashtable qty_set = new Hashtable();
            Hashtable check_typ_set = new Hashtable();
            Hashtable descr_parts_set = new Hashtable();
            TreeTraversal tree_traversal = new TreeTraversal();
            IList<KPVendorCode> kpVCList = new List<KPVendorCode>();
            try
            {
                IList<QtyParts> check_conditon_nodes_case_one = tree_traversal.BreadthFirstTraversal(bom.Root, "VK->P1->KP", "P1", this,"VK");
                //if (check_conditon_nodes_case_one != null && check_conditon_nodes_case_one.Count > 0)
                //{
                //    foreach (QtyParts check_conditon_node in check_conditon_nodes_case_one)
                //    {
                //        //替代料：
                //        //对于P1 Part 来说，Part.Descr 相同的记录是替代料，替代料的料号以逗号分隔，显示在Part No / Item Name列（包括主料）；取替代料中Qty 最大的P1 Part 为主料
                //        //Qty：
                //        //对于Model-> VK-> P1-> KP 这种结构，P1 Part 的数量是VK Part 在ModelBOM 定义的数量乘以P1 Part在ModelBOM 定义的数量
                //        //对于Model -> VK -> KP 这种结构，P1 Part 的数量是VK Part 在ModelBOM 定义的数量乘以KP Part在ModelBOM 定义的数量


                //        FlatBOMItem item = new FlatBOMItem(check_conditon_node.Qty, part_check_type, check_conditon_node.Parts);
                //        flat_bom_items.Add(item);
                //    }  
                //}

                if (check_conditon_nodes_case_one != null && check_conditon_nodes_case_one.Count > 0)
                {
                    foreach (QtyParts bm_check_conditon_node in check_conditon_nodes_case_one)
                    {
                        if (bom.FirstLevelNodes != null) 
                        {
                            IList<IBOMNode> bom_nodes = bom.FirstLevelNodes;
                            foreach (IBOMNode bom_node in bom_nodes)
                            {
                                if (bom_node.Part != null && bom_node.Part.BOMNodeType.Equals("VK"))
                                {
                                    IList<IBOMNode> vk_child_nodes = bom_node.Children;
                                    if (vk_child_nodes != null)
                                    {
                                        foreach (IBOMNode vk_child_node in vk_child_nodes)
                                        {
                                            //if (vk_child_node.Part  != null && vk_child_node.Part.BOMNodeType.Equals("P1"))
                                            //Flter out Battery ....
                                            if (vk_child_node.Part != null && vk_child_node.Part.BOMNodeType.Equals("P1") && !vk_child_node.Part.Descr.ToUpper().Contains("BATT"))
                                            {
                                              
                                                if (PartCompare(bm_check_conditon_node.Parts, vk_child_node.Part))
                                                {
                                                    String share_material_key = vk_child_node.Part.PN;
                                                    if (!string.IsNullOrEmpty(share_material_key))
                                                    {
                                                        if (!check_typ_set.ContainsKey(share_material_key))
                                                        {
                                                            check_typ_set.Add(share_material_key,"P1");
                                                        }

                                                        IList<IBOMNode> p1_child_nodes = vk_child_node.Children;
                                                        if (p1_child_nodes != null)
                                                        {
                                                            foreach (IBOMNode p1_child_node in p1_child_nodes)
                                                            {
                                                                if (p1_child_node.Part != null &&
                                                                    p1_child_node.Part.BOMNodeType.Equals("KP"))
                                                                {
                                                                    IList<PartInfo> part_infos =p1_child_node.Part.Attributes;
                                                                    if (part_infos != null && part_infos.Count > 0)
                                                                    {
                                                                        foreach (PartInfo part_info in part_infos)
                                                                        {
                                                                            if (part_info.InfoType.Equals("VendorCode"))
                                                                            {
                                                                                //vk_child_node.Part.AddAttribute(part_info);
                                                                                kpVCList.Add(new KPVendorCode
                                                                                {
                                                                                    PartNo = vk_child_node.Part.PN,
                                                                                    VendorCode = part_info.InfoValue
                                                                                });
                                                                           
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (share_parts_set.ContainsKey(share_material_key))
                                                        {
                                                            part_no_set[share_material_key] += "," + vk_child_node.Part.PN;
                                                            ((IList<IPart>) share_parts_set[share_material_key]).Add(vk_child_node.Part);
                                                            int qty = getMaxQty(check_conditon_nodes_case_one,vk_child_node.Part);
                                                            if ((int) qty_set[share_material_key] < qty)
                                                            {
                                                                qty_set[share_material_key] = qty;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            part_no_set.Add(share_material_key, vk_child_node.Part.PN);
                                                            IList<IPart> parts = new List<IPart>();
                                                            parts.Add(vk_child_node.Part);
                                                            share_parts_set.Add(share_material_key, parts);
                                                            int qty = getMaxQty(check_conditon_nodes_case_one,vk_child_node.Part);
                                                            qty_set.Add(share_material_key, qty);
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



                IList<QtyParts> check_conditon_nodes_case_two = tree_traversal.BreadthFirstTraversal(bom.Root, "VK->KP", "KP", this,"VK");
                IBOMRepository bom_repository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

                if (check_conditon_nodes_case_two != null)
                {
                    foreach (QtyParts qty_part in check_conditon_nodes_case_two)
                    {
                        IList<IPart> parts = qty_part.Parts;
                        if (parts != null)
                        {
                            foreach (IPart part in parts)
                            {
                                IList<IBOMNode> parent_boms = bom_repository.GetParentBomNode(part.PN);
                                if (parent_boms != null)
                                {
                                    foreach (IBOMNode flat_bom_item in parent_boms)
                                    {
                                        IPart parent_part = flat_bom_item.Part;
                                        if (parent_part != null && parent_part.BOMNodeType.Trim().Equals("P1"))
                                        {
                                            //IList<IPart> p1_parts = new List<IPart>();
                                            //p1_parts.Add(part);
                                            //var item = new FlatBOMItem(qty_part.Qty, part_check_type, p1_parts);
                                            //item.PartNoItem = part.PN;
                                            //item.Descr = part.Descr;
                                            //flat_bom_items.Add(item);
                                            IList<PartInfo> part_infos = part.Attributes;
                                            if (part_infos != null && part_infos.Count > 0)
                                            {
                                                foreach (PartInfo part_info in part_infos)
                                                {
                                                    if (part_info.InfoType.Equals("VendorCode"))
                                                    {
                                                        //parent_part.AddAttribute(part_info);
                                                        kpVCList.Add(new KPVendorCode
                                                        {
                                                            PartNo = parent_part.PN,
                                                            VendorCode = part_info.InfoValue
                                                        });
                                                    }
                                                }
                                            }
                                            String share_material_key = parent_part.PN;
                                            if (!string.IsNullOrEmpty(share_material_key))
                                            {
                                                if (!check_typ_set.ContainsKey(share_material_key))
                                                {
                                                    check_typ_set.Add(share_material_key, "P1");
                                                }
                                                if (share_parts_set.ContainsKey(share_material_key))
                                                {
                                                    part_no_set[share_material_key] += "," + parent_part.PN;
                                                    ((IList<IPart>) share_parts_set[share_material_key]).Add(parent_part);
                                                    //int qty = getMaxQty(check_conditon_nodes_case_two, parent_part);
                                                    int qty = qty_part.Qty;
                                                    if ((int)qty_set[share_material_key] < qty)
                                                    {
                                                        qty_set[share_material_key] = qty;
                                                    }
                                                }
                                                else
                                                {
                                                    part_no_set.Add(share_material_key, parent_part.PN);
                                                    IList<IPart> sparts = new List<IPart>();
                                                    sparts.Add(parent_part);
                                                    share_parts_set.Add(share_material_key, sparts);
                                                    //int qty = getMaxQty(check_conditon_nodes_case_two, parent_part);
                                                    int qty = qty_part.Qty;
                                                    qty_set.Add(share_material_key, qty);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                List<QtyParts> c2_check_conditon_nodes = new List<QtyParts>();
                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        if (bom.FirstLevelNodes.ElementAt(i).Part != null)
                        {
                            if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("C2"))
                            {
                                IList<PartInfo> part_infos = bom.FirstLevelNodes.ElementAt(i).Part.Attributes;
                                Boolean have_vendor_code = false;

                                if (part_infos != null && part_infos.Count > 0)
                                {
                                    foreach (PartInfo part_info in part_infos)
                                    {
                                        if (part_info.InfoType.Equals("VendorCode"))
                                        {
                                            have_vendor_code = true;
                                            break;
                                        }
                                    }
                                }
                                
                                Hashtable dib_share_part_infos = new Hashtable();
                                IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                                if (!have_vendor_code)
                                {
                                    if (bom.FirstLevelNodes.ElementAt(i).Part.PN.Length>=3 &&  bom.FirstLevelNodes.ElementAt(i).Part.PN.Trim().Substring(0, 3).Equals("DIB"))
                                    {
                                        string part_pn = bom.FirstLevelNodes.ElementAt(i).Part.PN.Trim();
                                       // IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                                        IPart dib_part;
                                          dib_part  = repository.GetPartByPartNo(part_pn.Substring(3, part_pn.Length-3));
                                          //For ICC: If part no format:DIBxxxxxx-yyyy,Remove the last character
                                          //              e.g. Change DIB709986-0011 to 709986-001
                                          if (dib_part == null)
                                          {
                                              if (part_pn.Split('-').Length == 2 && part_pn.Split('-')[1].Length == 4)
                                              {
                                                 dib_part = repository.GetPartByPartNo(part_pn.Substring(3, part_pn.Length - 4));
                                              }
                                           }
                                         //For ICC

                                        if (dib_part != null)
                                        {
                                            IList<PartInfo> dib_part_infos = dib_part.Attributes;
                                            if (dib_part_infos != null && dib_part_infos.Count > 0)
                                            {
                                                foreach (PartInfo dib_part_info in dib_part_infos)
                                                {
                                                    if (dib_part_info.InfoType.Equals("VendorCode"))
                                                    {
                                                        have_vendor_code = true;
                                                        //bom.FirstLevelNodes.ElementAt(i).Part.AddAttribute(dib_part_info);
                                                        kpVCList.Add(new KPVendorCode
                                                        {
                                                            PartNo = bom.FirstLevelNodes.ElementAt(i).Part.PN,
                                                            VendorCode = dib_part_info.InfoValue
                                                        });
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        //IPart part = bom.FirstLevelNodes.ElementAt(i).Part;
//                                        IList<PartInfo> sub_part_infos = bom.FirstLevelNodes.ElementAt(i).Part.Attributes;
//                                        if (sub_part_infos != null && sub_part_infos.Count > 0)
//                                        {
//                                            PartInfo need_vendor_code = null;
//                                            foreach (PartInfo sub_part_info in sub_part_infos)
//                                            {
//                                                if (sub_part_info.InfoType.Equals("SUB"))
//                                                {
//                                                    string[] part_pns = sub_part_info.InfoValue.Trim().Split(';');
//                                                    for (int j = 0; j < part_pns.Length;j++ )
//                                                    {
//                                                        if (part_pns[j].Length > 3 && part_pns[j].Substring(0, 3).Equals("DIB"))
//                                                        {
//                                                            IPart sub_dib_part = repository.GetPartByPartNo(part_pns[j].Substring(3, part_pns[j].Length - 3));
//                                                            if (sub_dib_part != null)
//                                                            {
//                                                                IList<PartInfo> sub2_part_infos = sub_dib_part.Attributes;
//                                                                if (sub2_part_infos != null && sub2_part_infos.Count > 0)
//                                                                {
//                                                                    foreach (PartInfo sub2_part_info in sub2_part_infos)
//                                                                    {
//                                                                        if (sub2_part_info.InfoType.Equals("VendorCode"))
//                                                                        {
//                                                                            have_vendor_code = true;
//                                                                            need_vendor_code = sub2_part_info;
////                                                                            if (dib_share_part_infos.ContainsKey(part_pns[j]))
////                                                                            {
////                                                                                ((IList<PartInfo>)dib_share_part_infos[part_pns[j]]).Add(sub2_part_info);
////                                                                            }
////                                                                            else
////                                                                            {
////                                                                                IList<PartInfo> dib_tmp_part_infos = new List<PartInfo>();
////                                                                                dib_tmp_part_infos.Add(sub2_part_info);
////                                                                                dib_share_part_infos.Add(part_pns[j], dib_tmp_part_infos);
////                                                                            }
//                                                                            break;
//                                                                        }
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                        else
//                                                        {
//                                                            IPart sub_dib_part = repository.GetPartByPartNo(part_pns[j]);
//                                                            if (sub_dib_part != null)
//                                                            {
//                                                                IList<PartInfo> sub2_part_infos = sub_dib_part.Attributes;
//                                                                if (sub2_part_infos != null && sub2_part_infos.Count > 0)
//                                                                {
//                                                                    foreach (PartInfo sub2_part_info in sub2_part_infos)
//                                                                    {
//                                                                        if (sub2_part_info.InfoType.Equals("VendorCode"))
//                                                                        {
//                                                                            have_vendor_code = true;
//                                                                            need_vendor_code = sub2_part_info;
////                                                                            if (dib_share_part_infos.ContainsKey(part_pns[j]))
////                                                                            {
////                                                                                ((IList<PartInfo>)dib_share_part_infos[part_pns[j]]).Add(sub2_part_info);
////                                                                            }
////                                                                            else
////                                                                            {
////                                                                                dib_share_part_infos.Add(part_pns[j], sub2_part_info);
////                                                                            }
//                                                                            break;
//                                                                        }
//                                                                    }
//                                                                }
//                                                            }
//                                                        }
//                                                    }
//                                                }
//                                            }
//                                            if (need_vendor_code != null)
//                                            {
//                                                bom.FirstLevelNodes.ElementAt(i).Part.AddAttribute(need_vendor_code);
//                                            }
//                                        }
                                    }
                                }
 
                                if (have_vendor_code)
                                {
                                    List<IPart> parts = new List<IPart>();
                                    parts.Add(bom.FirstLevelNodes.ElementAt(i).Part);
//                                    if (dib_share_part_infos.Count > 0)
//                                    {
//                                        foreach (DictionaryEntry de in dib_share_part_infos)
//                                        {
//                                            IPart dib_part = repository.GetPartByPartNo((string)de.Key);
//                                            if (dib_part != null)
//                                            {
//                                                IList<PartInfo> dib_share_part_set = (IList<PartInfo>) dib_share_part_infos[de.Key];
//                                                foreach (PartInfo dib_share_part_info in dib_share_part_set)
//                                                {
//                                                    dib_part.AddAttribute(dib_share_part_info);
//                                                }
//                                                parts.Add(dib_part);
//                                            }
//                                        }
//                                    }
                                    QtyParts bom_item = new QtyParts(bom.FirstLevelNodes.ElementAt(i).Qty, parts);
                                    c2_check_conditon_nodes.Add(bom_item);
                                }
                            }
                        }
                    }
                }


                if (c2_check_conditon_nodes.Count > 0)
                {
                    foreach (QtyParts c2_check_conditon_node in c2_check_conditon_nodes)
                    {
                        if (bom.FirstLevelNodes != null)
                        {
                            IList<IBOMNode> bom_nodes = bom.FirstLevelNodes;
                            foreach (IBOMNode bom_node in bom_nodes)
                            {
                                if (bom_node.Part != null && bom_node.Part.BOMNodeType.Equals("C2"))
                                {
                                    String share_material_key = bom_node.Part.PN;

                                    if (!check_typ_set.ContainsKey(share_material_key))
                                    {
                                        check_typ_set.Add(share_material_key, "C2");
                                    }
                                    if (PartCompare(c2_check_conditon_node.Parts, bom_node.Part))
                                    {
                                        Boolean exist_share_part = false;
                                        //parts.Add(part);
                                        IList<PartInfo> part_infos = bom_node.Part.Attributes;
                                        if (part_infos != null && part_infos.Count > 0)
                                        {
                                            foreach (PartInfo part_info in part_infos)
                                            {
                                                if (part_info.InfoType.Equals("SUB"))
                                                {
//                                                    exist_share_part = true;
                                                    String[] share_parts = part_info.InfoValue.Trim().Split(';');
                                                    if (share_parts.Length > 0)
                                                    {
                                                        string share_part_no = part_info.InfoValue.Trim();
                                                        share_part_no = share_part_no.Replace("DIB", "");
                                                        if (share_part_no_set.ContainsKey(bom_node.Part.PN))
                                                        {
                                                            share_part_no_set[bom_node.Part.PN] += "," + share_part_no.Replace(';',',');
                                                        }
                                                        else
                                                        {
                                                            share_part_no_set.Add(bom_node.Part.PN, share_part_no.Replace(';', ','));
                                                        }
                                                        
                                                        IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                                                        IList<IPart> parts = new List<IPart>();
//                                                            parts.Add(bom_node.Part);
                                                        for (int j = 0; j < share_parts.Length; j++)
                                                        {
                                                            IPart share_part = null;
                                                            if (share_parts[j].Length >=3 && share_parts[j].Substring(0,3).Equals("DIB"))
                                                            {
                                                                share_part = repository.GetPartByPartNo(share_parts[j]);
                                                                IPart real_part = repository.GetPartByPartNo(share_parts[j].Substring(3, share_parts[j].Length - 3));
                                                               //For ICC
                                                                if (real_part == null)
                                                                {
                                                                    if (share_parts[j].Split('-').Length == 2 && share_parts[j].Split('-')[1].Length == 4)
                                                                    {
                                                                        real_part = repository.GetPartByPartNo(share_parts[j].Substring(3, share_parts[j].Length - 4));
                                                                    }
                                                                }
                                                                //For ICC

                                                                if (share_part != null && real_part != null)
                                                                {
                                                                    IList<PartInfo> sub2_part_infos = real_part.Attributes;
                                                                    if (sub2_part_infos != null && sub2_part_infos.Count > 0)
                                                                    {
                                                                        foreach (PartInfo sub2_part_info in sub2_part_infos)
                                                                        {
                                                                            if (sub2_part_info.InfoType.Equals("VendorCode"))
                                                                            {
                                                                               // share_part.AddAttribute(sub2_part_info);
                                                                                kpVCList.Add(new KPVendorCode
                                                                                {
                                                                                    PartNo = share_part.PN,
                                                                                    VendorCode = sub2_part_info.InfoValue
                                                                                });
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                share_part = repository.GetPartByPartNo(share_parts[j]);
                                                            }
                                                            if (share_part != null)
                                                            {
                                                                parts.Add(share_part);
                                                            }
                                                        }
                                                        String share_parts_code = bom_node.Part.PN;
                                                        if (!share_parts_set.ContainsKey(share_parts_code))
                                                        {
                                                            share_parts_set.Add(share_parts_code, parts);
                                                            qty_set.Add(share_parts_code, bom_node.Qty);
                                                            descr_parts_set.Add(share_parts_code, bom_node.Part.Descr);
                                                        }
                                                        else
                                                        {
//                                                                ((IList<IPart>)share_parts_set[share_parts_code]).Add(bom_node.Part);
                                                            if (parts.Count > 0)
                                                            {
                                                                foreach (IPart part in parts)
                                                                {
                                                                    ((IList<IPart>)share_parts_set[share_parts_code]).Add(part);      
                                                                }
                                                            }
                                                            
                                                            if (!((String)descr_parts_set[share_parts_code]).Contains(bom_node.Part.Descr))
                                                            {
                                                                descr_parts_set[share_parts_code] += "," + bom_node.Part.Descr;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
//                                        if (!exist_share_part)
//                                        {
                                            if (!share_parts_set.ContainsKey(bom_node.Part.PN))
                                            {
                                                IList<IPart> parts = new List<IPart>();
//												if (bom_node.Part.PN.Substring(0,3).Equals("DIB"))
//												{
//                                                    IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
//                                                    IPart part = repository.GetPartByPartNo(bom_node.Part.PN.Substring(3, bom_node.Part.PN.Length - 3));
//                                                    parts.Add(part);
//												}
//												else
//												{
                                                    parts.Add(bom_node.Part);
//												}

                                                share_parts_set.Add(bom_node.Part.PN, parts);
                                                qty_set.Add(bom_node.Part.PN, bom_node.Qty);
                                                descr_parts_set.Add(bom_node.Part.PN, bom_node.Part.Descr);
                                            }
                                            else
                                            {
//                                                if (bom_node.Part.PN.Substring(0, 3).Equals("DIB"))
//                                                {
//                                                    IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
//                                                    IPart part = repository.GetPartByPartNo(bom_node.Part.PN.Substring(3, bom_node.Part.PN.Length - 3));
//                                                    ((IList<IPart>)share_parts_set[bom_node.Part.PN]).Add(part);
//                                                }
//                                                else
//                                                {
                                                    ((IList<IPart>)share_parts_set[bom_node.Part.PN]).Insert(0, bom_node.Part);
//                                                }
                                                
                                                if (!((String)descr_parts_set[bom_node.Part.PN]).Contains(bom_node.Part.Descr))
                                                {
                                                    descr_parts_set[bom_node.Part.PN] += "," + bom_node.Part.Descr;
                                                }
                                            }
//                                        }
//                                        else
//                                        {
//                                            //开头不是DIB的Part，要将自身加入到share_parts_set中。
//                                            if (!bom_node.Part.PN.Substring(0, 3).Equals("DIB"))
//                                            {
//                                                ((IList<IPart>)share_parts_set[bom_node.Part.PN]).Add(bom_node.Part);
//                                                if (!((String)descr_parts_set[bom_node.Part.PN]).Contains(bom_node.Part.Descr))
//                                                {
//                                                    descr_parts_set[bom_node.Part.PN] += "," + bom_node.Part.Descr;
//                                                }
//                                            }
//                                        }
                                    }
                                }
                            }
                        }
                    }
                }



                if (share_parts_set.Count > 0)
                {
                    foreach (DictionaryEntry  de in share_parts_set)
                    {
                        var flat_bom_item = new FlatBOMItem((int)qty_set[de.Key],part_check_type , (IList<IPart>)de.Value);
                        flat_bom_item.Tag = kpVCList;
                        if (share_part_no_set.ContainsKey(de.Key))
                        {
//                            if (((String)de.Key).Substring(0,3).Equals("DIB"))
//                            {
//                                flat_bom_item.PartNoItem = ((string)share_part_no_set[de.Key]).Replace("DIB","");
//                            }
//                            else
//                            {
                                flat_bom_item.PartNoItem = (String)de.Key + "," + (string)share_part_no_set[de.Key];
                                flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
//                            }
                        }
                        else
                        {
                            if (part_no_set.ContainsKey(de.Key))
                            {
                                flat_bom_item.PartNoItem = ((String)part_no_set[de.Key]).Replace("DIB", "");         
                            }
                            else
                            {
                                flat_bom_item.PartNoItem = ((string)de.Key).Replace("DIB", "");     
                            }
                        }
                        
                        flat_bom_item.Tp = (string) check_typ_set[de.Key];
                        if (descr_parts_set.ContainsKey(de.Key))
                        {
                            flat_bom_item.Descr = (string)descr_parts_set[de.Key];
                        }
                        else
                        {
                            flat_bom_item.Descr = (String)de.Key;    
                        }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool CheckCondition(object node)
        {
            //Model 的直接下阶BomNodeType = 'VK'，Remark = 'PK' (IMES_GetData..Part.Remark) (且存在VC)
            //union
            //Model 的直接下阶BomNodeType = 'C2' (且存在VC)
            if (node == null)
            {
                return false;
            }
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_vk = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("VK");
            bool contain_pk = ((BOMNode)node).Part.Remark.Trim().Contains("PK");
//            bool is_kp = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("KP");
            bool is_c2 = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("C2");
            bool exist_vc = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            if (part_infos != null)
            {
                foreach (var part_info in part_infos)
                {
                    if (!exist_vc)
                    {
                        if (part_info.InfoType.Equals("VendorCode"))
                        {
                            exist_vc = true;
                            continue;
                        }
                    }
                }
            }
//            if ((is_vk && contain_pk && exist_vc))
            if (is_vk && contain_pk)
            {
                return true;
            }
//            if (exist_vc && is_kp)
//            {
//                return true;
//            }
            if (is_c2 && exist_vc)
            {
                return true;
            }
            return false;
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
        private int getMaxQty(IList<QtyParts> qty_parts,IPart part)
        {
            int ret = 0;
            if (qty_parts != null && part != null)
            {
                foreach (QtyParts qty_part in qty_parts)
                {
                    Boolean is_equal = false;
                    if (qty_part.Parts != null)
                    {
                        foreach (IPart apart in qty_part.Parts)
                        {
                            if (apart.PN.Trim().Equals(part.PN.Trim()))
                            {
                                is_equal = true;
                            }
                        }
                    }
                    if (is_equal)
                    {
                        if (ret < qty_part.Qty)
                        {
                            ret = qty_part.Qty;
                        }
                    }
                }
            }
            return ret;
        }
    }
}
