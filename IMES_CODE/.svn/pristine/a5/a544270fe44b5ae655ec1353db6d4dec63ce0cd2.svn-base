// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
//2012-03-06   210003                        ITC-1360-1001
//2012-03-09   210003                        ITC-1360-1057
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.OOA.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.OOA.Filter.dll")]
    public class Filter : IFilterModule
    {
//        private int qty = 1; //问题：Pizza Kitting。该情况下，如何计算qty。
        private const string part_check_type = "OOA";
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
            Hashtable share_part_no_set = new Hashtable();
            Hashtable qty_share_parts_set = new Hashtable();
            Hashtable descr_parts_set = new Hashtable();
            IFlatBOM ret = null;
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
                        if (part.BOMNodeType.Trim().Equals("P1"))
                        {

                            if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                            {
                                Boolean exist_share_part = false;
                                //parts.Add(part);
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
                                                share_part_no_set.Add(part.PN, share_part_no.Replace(';', ','));
                                                IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                                                IList<IPart> parts = new List<IPart>();
                                                parts.Add(part);
                                                for (int j = 0; j < share_parts.Length; j++)
                                                {
                                                    IPart share_part = repository.GetPartByPartNo(share_parts[j]);
                                                    if (share_part != null)
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
                }
                if (share_parts_set.Count > 0)
                {
                    foreach (DictionaryEntry de in share_parts_set)
                    {
                        var flat_bom_item = new FlatBOMItem((int)qty_share_parts_set[de.Key], part_check_type, (IList<IPart>)de.Value);
                        if (share_part_no_set.ContainsKey(de.Key))
                        {
                            flat_bom_item.PartNoItem = de.Key + "," + (string)share_part_no_set[de.Key];
                        }
                        else
                        {
                            flat_bom_item.PartNoItem = (string)de.Key;
                        }
                        flat_bom_item.Tp = "P1";
                        flat_bom_item.Descr = (string) descr_parts_set[de.Key];
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
        public bool CheckCondition(object node)
        {
            //第一阶P1 还要求DESC 属性='OOA'  (IMES_GetData..PartInfo.InfoValue；Condition:InfoType = 'DESC')
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            //bool is_ps = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("P1");
            //bool is_desc_ooa = ((BOMNode)node).Part.Descr.Trim().Equals("OOA");
            bool is_part_info_descr = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            if (part_infos == null)
            {
                return false;
            }
            foreach (PartInfo part_info in part_infos)
            {
                if (part_info.InfoType.Trim().Equals("DESC"))
                {
                    if (part_info.InfoValue.Trim().Equals("OOA"))
                    {
                        is_part_info_descr = true;
                        break;
                    }
                }
            }
            if (is_part_info_descr)
                return true;
            return false;
        }
    }
}
