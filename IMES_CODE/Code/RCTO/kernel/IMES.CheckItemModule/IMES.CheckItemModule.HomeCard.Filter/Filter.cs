// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-12-19   210003                       ITC-1360-0858
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.HomeCard.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.HomeCard.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const int qty = 1;
        private const string part_check_type = "HomeCard";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
            Hashtable descr_share_parts_set = new Hashtable();
            //问题：station参数用不上。因为在hierarchicalBOM中，没有与station相关的字段。
            IFlatBOM ret = null;
//            List<IPart> parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            HierarchicalBOM bom = (HierarchicalBOM)hierarchical_bom;

            try
            {
                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                        {
                            IPart part = ((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Part;
                           // parts.Add(part);
                            if (!share_parts_set.ContainsKey(part.PN))
                            {
                                IList<IPart> parts = new List<IPart>();
                                parts.Add(part);
                                share_parts_set.Add(part.PN, parts);
                                //qty_share_parts_set.Add(part.PN, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                descr_share_parts_set.Add(part.PN, part.Descr);
                            }
                            else
                            {
                                ((IList<IPart>)share_parts_set[part.PN]).Add(part);
                                descr_share_parts_set[part.PN] += part.Descr;
                            }
                        }
                    }
                }
//                if (parts.Count > 0)
//                {
//                    FlatBOMItem flat_bom_item = new FlatBOMItem(qty, part_check_type, parts);
//                    flat_bom_item.PartNoItem = parts.ElementAt(0).PN;
//                    flat_bom_item.Descr = parts.ElementAt(0).Descr;
//                    IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
//                    flat_bom_items.Add(flat_bom_item);
//                    ret = new FlatBOM(flat_bom_items);
//                }
                if (share_parts_set.Count > 0)
                {
                    foreach (DictionaryEntry de in share_parts_set)
                    {
                        var flat_bom_item = new FlatBOMItem(qty, part_check_type, (IList<IPart>)de.Value);
                        flat_bom_item.PartNoItem = (string)de.Key;
                        flat_bom_item.Tp = "P1";
                        flat_bom_item.Descr = (string)descr_share_parts_set[de.Key] + " " + (string)de.Key;
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
            //以及第一阶是KP的part的第一阶及其下阶[KP->VC],即KP和VC，第一阶的Descr描述为Descr like 'HDD%'
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            string desc = ((IBOMNode)node).Part.Descr.ToUpper().Trim();
            bool is_p1 = ((IBOMNode)node).Part.BOMNodeType.Trim().Equals("P1");
            if (desc.Equals("Home Card".ToUpper()) && is_p1)
                return true;
            return false;
        }
    }
}
