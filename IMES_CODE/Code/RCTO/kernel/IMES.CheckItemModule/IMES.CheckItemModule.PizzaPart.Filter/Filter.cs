// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-03-05   210003                       ITC-1360-1077
// 2011-03-05   210003                       ITC-1360-1234
// 2011-03-05   210003                       ITC-1360-1444
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.DataModel;

namespace IMES.CheckItemModule.PizzaPart.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.PizzaPart.Filter.dll")]
    public class Filter : IFilterModule
    {
        //private int qty = 1;//问题：Pizza Kitting。该情况下，如何计算qty。
        private const string part_check_type = "PizzaPart";
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
            Hashtable share_part_no_set = new Hashtable();
            Hashtable qty_share_parts_set = new Hashtable();
            Hashtable descr_parts_set = new Hashtable();
            Hashtable check_typ_set = new Hashtable();
            //问题：station参数用不上。因为在hierarchicalBOM中，没有与station相关的字段。
            IFlatBOM ret = null;
            //int qty = 0;
            //var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            try
            {
                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part = ((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Part;
                        if (part != null && (part.BOMNodeType.Trim().Equals("PL") || part.BOMNodeType.Trim().Equals("C2") || part.BOMNodeType.Trim().Equals("VK")))
                        {

                            if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                            {
                                Boolean exist_share_part = false;
                                IList<PartInfo> part_infos = part.Attributes;
                                if (part_infos != null && part_infos.Count > 0)
                                {
                                    foreach (PartInfo part_info in part_infos)
                                    {
                                        if (part_info.InfoType.Equals("SUB"))
                                        {
                                            exist_share_part = true;

                                            String[] share_parts = part_info.InfoValue.Trim().Split(';');
                                            if (share_parts.Length > 0)
                                            {
                                                string share_part_no = part_info.InfoValue.Trim();
//                                                share_part_no = share_part_no.Replace("DIB", "");
//                                                share_part_no_set.Add(part.PN, share_part_no.Replace(';', ','));
                                                if (share_part_no_set.ContainsKey(part.PN))
                                                {
                                                    share_part_no_set[part.PN] += "," + share_part_no.Replace(';', ',');
                                                }
                                                else
                                                {
                                                    share_part_no_set.Add(part.PN, share_part_no.Replace(';', ','));
                                                }
                                                IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                                                IList<IPart> parts = new List<IPart>();
                                                parts.Add(part);
                                                for (int j = 0; j < share_parts.Length; j++)
                                                {
//                                                    IPart share_part = repository.GetPartByPartNo(share_parts[j]);
                                                    IPart share_part = null;    
                                                    bool isC2WithoutVC = false;
                                                    if (share_parts[j].Substring(0, 3).Equals("DIB") && part.BOMNodeType.Trim().Equals("C2"))
                                                    {
                                                        share_part = repository.GetPartByPartNo(share_parts[j].Substring(3, share_parts[j].Length - 3));
                                                        if (share_part == null)
                                                        {
                                                            //For ICC
                                                            if (share_parts[j].Split('-').Length == 2 && share_parts[j].Split('-')[1].Length == 4)
                                                            {
                                                                share_part = repository.GetPartByPartNo(share_parts[j].Substring(3, share_parts[j].Length - 4));
                                                            }
                                                            //For ICC
                                                            if (share_part == null)
                                                            {
                                                                share_part = repository.GetPartByPartNo(share_parts[j]);
                                                            }
                                                           
                                                        }
                                                        if (share_part != null)
                                                        {
                                                            IList<PartInfo> c2_dib_share_part_infos = share_part.Attributes;
                                                            if (c2_dib_share_part_infos != null)
                                                            {
                                                                bool have_vendor_code = false;
                                                                foreach (PartInfo c2_dib_share_part_info in c2_dib_share_part_infos)
                                                                {
                                                                    if (c2_dib_share_part_info.InfoType.Equals("VendorCode"))
                                                                    {
                                                                        have_vendor_code = true;
                                                                    }
                                                                }
                                                                if (!have_vendor_code)
                                                                {
                                                                    isC2WithoutVC = true;
                                                                    parts.Add(share_part);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        share_part = repository.GetPartByPartNo(share_parts[j]);
                                                    }
                                                    if (share_part != null && !isC2WithoutVC)
                                                    {
                                                        parts.Add(share_part);
                                                    }
                                                }
                                                String share_parts_code = part.PN;
                                                if (!share_parts_set.ContainsKey(share_parts_code))
                                                {
                                                    share_parts_set.Add(share_parts_code, parts);
                                                    qty_share_parts_set.Add(share_parts_code,((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                                    descr_parts_set.Add(share_parts_code, part.Descr);
                                                    check_typ_set.Add(share_parts_code, part.BOMNodeType.Trim());
                                                }
                                                else
                                                {
                                                    ((IList<IPart>)share_parts_set[share_parts_code]).Add(part);
                                                    if (!((String)descr_parts_set[share_parts_code]).Contains(part.Descr))
                                                    {
                                                        descr_parts_set[share_parts_code] += "," + part.Descr;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (!exist_share_part)
                                {
                                    if (!share_parts_set.ContainsKey(part.PN))
                                    {
                                        IList<IPart> parts = new List<IPart>();
//                                        if (part.PN.Substring(0, 3).Equals("DIB"))
//                                        {
//                                            IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
//                                            IPart dib_part = repository.GetPartByPartNo(part.PN.Substring(3, part.PN.Length - 3));
//                                            if (dib_part != null)
//                                            {
//                                                parts.Add(dib_part);
//                                            }
//                                            else
//                                            {
//                                                parts.Add(part);
//                                            }
//                                        }
//                                        else
//                                        {
                                            parts.Add(part);
//                                        }
                                        share_parts_set.Add(part.PN, parts);
                                        qty_share_parts_set.Add(part.PN,((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Qty);
                                        descr_parts_set.Add(part.PN, part.Descr);
                                        check_typ_set.Add(part.PN, part.BOMNodeType.Trim());
                                    }
                                    else
                                    {
//                                        ((IList<IPart>) share_parts_set[part.PN]).Add(part);
//                                        if (part.PN.Substring(0, 3).Equals("DIB"))
//                                        {
//                                            IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
//                                            IPart dib_part = repository.GetPartByPartNo(part.PN.Substring(3, part.PN.Length - 3));
//                                            ((IList<IPart>)share_parts_set[part.PN]).Add(dib_part);
//                                        }
//                                        else
//                                        {
                                            ((IList<IPart>)share_parts_set[part.PN]).Add(part);
//                                        }
                                        if (!((String) descr_parts_set[part.PN]).Contains(part.Descr))
                                        {
                                            descr_parts_set[part.PN] += "," + part.Descr;
                                        }
                                    }
                                }
//                                else
//                                {
//                                    
//                                }
                            }
                        }
                    }

                    if (share_parts_set.Count > 0)
                    {
                        foreach (DictionaryEntry de in share_parts_set)
                        {
                            var flat_bom_item = new FlatBOMItem((int)qty_share_parts_set[de.Key], part_check_type, (IList<IPart>)de.Value);
                            if (share_part_no_set.ContainsKey(de.Key))
                            {
                                if (((string)de.Key).Substring(0, 3).Equals("DIB"))
                                {
                                    flat_bom_item.PartNoItem = ((string)de.Key).Substring(3, ((string)de.Key).Length - 3) + "," + (string)share_part_no_set[de.Key];
                                    flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
                                }
                                else
                                {
                                    flat_bom_item.PartNoItem = de.Key + "," + (string)share_part_no_set[de.Key];
                                    flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
                                }
                            }
                            else
                            {
//                                flat_bom_item.PartNoItem = (string)de.Key;
                                if (((string)de.Key).Substring(0, 3).Equals("DIB"))
                                {
                                    flat_bom_item.PartNoItem = ((string)de.Key).Substring(3, ((string)de.Key).Length - 3);
                                    flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
                                }
                                else
                                {
                                    flat_bom_item.PartNoItem = (string)de.Key;
                                    flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
                                }
                            }
                            flat_bom_item.Tp = (string) check_typ_set[de.Key];
                            flat_bom_item.Descr = (string)descr_parts_set[de.Key];
                            flat_bom_items.Add(flat_bom_item);
                        }
                    }
                    if (flat_bom_items.Count > 0)
                    {
                        ret = new FlatBOM(flat_bom_items);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }

            ReplaceOldOfficeCD(ret, (IProduct)main_object);
            return ret;
        }
        public bool CheckCondition(object node)
        {
            //n  PL 还要求IMES_GetData..Part.Descr LIKE '%Carton%'，并且TYPE属性='BOX' (IMES_GetData..PartInfo.InfoValue；Condition:InfoType = 'TYPE')
            //union
            //Model 的直接下阶 BomNodeType= 'C2' 且不存在VC
            if (node == null)
            {
                return false;
            }
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            string descr = ((BOMNode) node).Part.GetAttribute("Descr");
            bool contain_carton = descr.Contains("Carton");
            bool is_box = false;
            bool is_c2 = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("C2");
            bool exist_vc = false;
            IList<PartInfo> part_infos = ((BOMNode) node).Part.Attributes;
            bool is_vk = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("VK");
            bool contain_pk = ((BOMNode)node).Part.Remark.Trim().Contains("PK");
            bool exist_kp_vc = false;
            if (part_infos != null)
            {
                foreach (var part_info in part_infos)
                {
                    if (contain_carton)
                    {
                        if (!is_box)
                        {
                            if (part_info.InfoType == "TYPE" && part_info.InfoValue == "BOX")
                            {
                                is_box = true;
                                continue;
                            }
                        }
                    }
                    if (is_c2 || is_vk)
                    {
                        if (!exist_vc)
                        {
                            if (part_info.InfoType.Equals("VendorCode"))
                            {
                                exist_vc = true;
                                continue;
                            }
                        }
                        if (!exist_vc)
                        {
                            if (((BOMNode)node).Part.PN.Trim().Substring(0, 3).Equals("DIB"))
                            {
                                string part_pn = ((BOMNode)node).Part.PN.Trim();
                                IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                                IPart dib_part = repository.GetPartByPartNo(part_pn.Substring(3, part_pn.Length - 3));
                                //For ICC
                                if (dib_part == null && part_pn.Split('-').Length == 2 && part_pn.Split('-')[1].Length == 4)
                                {
                                    dib_part = repository.GetPartByPartNo(part_pn.Substring(3, part_pn.Length - 4));
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
                                                exist_vc = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (is_vk && contain_pk)
            {
                IList<IBOMNode> vk_child_nodes = ((BOMNode) node).Children;
                if (vk_child_nodes != null)
                {
                    foreach (IBOMNode vk_child_node in vk_child_nodes)
                    {
                        if (vk_child_node.Part != null && vk_child_node.Part.BOMNodeType.Equals("P1"))
                        {
                            IList<IBOMNode> p1_child_nodes = vk_child_node.Children;
                            if (p1_child_nodes != null)
                            {
                                foreach (IBOMNode p1_child_node in p1_child_nodes)
                                {
                                    if (p1_child_node.Part != null && p1_child_node.Part.BOMNodeType.Equals("KP"))
                                    {
                                        IList<PartInfo> kp_part_infos = p1_child_node.Part.Attributes;
                                        if (kp_part_infos != null && kp_part_infos.Count > 0)
                                        {
                                            foreach (PartInfo kp_part_info in kp_part_infos)
                                            {
                                                if (kp_part_info.InfoType.Equals("VendorCode"))
                                                {
                                                    exist_kp_vc = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (exist_kp_vc)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        if (exist_kp_vc)
                        {
                            break;
                        }
                    }
                }
            }
            if ((contain_carton && is_box) || (is_c2 && !exist_vc) || (is_vk && contain_pk && !exist_kp_vc))
                return true;
            return false;
        }

        private void ReplaceOldOfficeCD(IFlatBOM bom, IProduct prod )
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            if (prod != null)
            {
                try
                {
                    bool bFindOldPartNo = false;
                    string imgPartNo = ((string)prod.GetExtendedProperty("IMG")).Trim();
                    IList<ConstValueInfo> valueList = partRep.GetConstValueListByType("OfficeCD");
                    var partNo = (from p in valueList
                                  where p.name == imgPartNo
                                  select p).ToList();

                    if (partNo != null && partNo.Count > 0 &&
                        !string.IsNullOrEmpty(partNo[0].value) &&
                         !string.IsNullOrEmpty(partNo[0].description))
                    {
                        string[] oldPartNo = partNo[0].value.Split(new char[] { '~' });
                        string newPartNo = partNo[0].description.Trim();

                        foreach (FlatBOMItem item in bom.BomItems)
                        {
                            var oldPart = (from p in item.AlterParts
                                           where oldPartNo.Contains(p.PN)
                                           select p).ToList();
                            if (oldPart != null && oldPart.Count > 0)
                            {

                                bFindOldPartNo = true;
                                IPart newPart = GetNewOfficePart(newPartNo, item);
                                if (newPart != null)
                                {
                                    item.AlterParts.Clear();
                                    item.AlterParts.Add(newPart);
                                }

                            }

                            if (bFindOldPartNo)
                                break;
                        }
                    }
                }
                catch
                {
                }

            }
        }

        private IPart GetNewOfficePart(string partNo,FlatBOMItem bomItem)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IPart part = partRep.Find(partNo);
            if (part != null)
            {
                string infoValue = part.GetAttribute("SUB");
                bomItem.Descr = part.Descr;
                bomItem.Tp = part.BOMNodeType;
                bomItem.PartNoItem = infoValue.Replace(";", ",");
            }
            return part;
        }
    }
}
