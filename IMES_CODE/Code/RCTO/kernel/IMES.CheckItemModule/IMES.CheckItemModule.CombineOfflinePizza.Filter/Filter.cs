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


namespace IMES.CheckItemModule.CombineOfflinePizza.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CombineOfflinePizza.Filter.dll")]
    public class Filter : IFilterModule
    {
        //private int qty = 1;//问题：Pizza Kitting。该情况下，如何计算qty。
          private const string part_check_type = "CombineOfflinePizza";
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
                        IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                        if (part != null &&  part.BOMNodeType.Trim().Equals("C5") )
                        {

                            if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                            {
                                Boolean exist_share_part = false;
                                IList<PartInfo> part_infos = part.Attributes;
                                if (part_infos != null && part_infos.Count > 0)
                                {
                                    foreach (PartInfo part_info in part_infos)
                                    {
                                        if (part_info.InfoType.Equals("SUB") && !string.IsNullOrEmpty(part_info.InfoValue))
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
                                                    //bool isC5WithoutVC = false;
                                                    if (part.BOMNodeType.Trim().Equals("C5"))
                                                    {
                                                        share_part = repository.GetPartByPartNo(share_parts[j].Substring(3, share_parts[j].Length - 3));
                                                        if (share_part == null)
                                                        {
                                                            share_part = repository.GetPartByPartNo(share_parts[j]);
                                                        }
                                                        //if (share_part != null)
                                                        //{
                                                        //    IList<PartInfo> c2_dib_share_part_infos = share_part.Attributes;
                                                        //    if (c2_dib_share_part_infos != null)
                                                        //    {
                                                        //        bool have_vendor_code = false;
                                                        //        foreach (PartInfo c2_dib_share_part_info in c2_dib_share_part_infos)
                                                        //        {
                                                        //            if (c2_dib_share_part_info.InfoType.Equals("VendorCode"))
                                                        //            {
                                                        //                have_vendor_code = true;
                                                        //            }
                                                        //        }
                                                        //        if (!have_vendor_code)
                                                        //        {
                                                        //            isC2WithoutVC = true;
                                                        //            parts.Add(share_part);
                                                        //        }
                                                        //    }
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        share_part = repository.GetPartByPartNo(share_parts[j]);
                                                    }
                                                    parts.Add(share_part);
                                                    //if (share_part != null && !isC2WithoutVC)
                                                    //{
                                                    //    parts.Add(share_part);
                                                    //}
                                                }
                                                String share_parts_code = part.PN;
                                                if (!share_parts_set.ContainsKey(share_parts_code))
                                                {
                                                    share_parts_set.Add(share_parts_code, parts);
                                                    qty_share_parts_set.Add(share_parts_code, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
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
                                       
                                        parts.Add(part);
                                      
                                        share_parts_set.Add(part.PN, parts);
                                        qty_share_parts_set.Add(part.PN, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                        descr_parts_set.Add(part.PN, part.Descr);
                                        check_typ_set.Add(part.PN, part.BOMNodeType.Trim());
                                    }
                                    else
                                    {
                                     
                                        ((IList<IPart>)share_parts_set[part.PN]).Add(part);
                            
                                        if (!((String)descr_parts_set[part.PN]).Contains(part.Descr))
                                        {
                                            descr_parts_set[part.PN] += "," + part.Descr;
                                        }
                                    }
                                }
                          
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
                            flat_bom_item.Tp = (string)check_typ_set[de.Key];
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

           return ret;
        }
        public bool CheckCondition(object node)
        {
            return true;
        }

    
    }
}
