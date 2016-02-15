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
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.SecondPizzaID.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.SecondPizzaID.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const int qty = 1; //问题：Pizza Kitting。该情况下，如何计算qty。
        private const string part_check_type = "2ndPizzaID";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            Hashtable share_parts_set = new Hashtable();
            Hashtable check_typ_set = new Hashtable();
            IFlatBOM ret = null;
            if (station.Trim().Equals("PKOK"))
            {
                //问题：station参数用不上。因为在hierarchicalBOM中，没有与station相关的字段。

                //var parts = new List<IPart>();
                if (hierarchical_bom == null)
                {
                    throw new ArgumentNullException();
                }
                var bom = (HierarchicalBOM) hierarchical_bom;
                try
                {
                    if (bom.FirstLevelNodes != null)
                    {
                        for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                        {
                            if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                            {
                                IPart part = ((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Part;

                                if (!share_parts_set.ContainsKey(part.PN))
                                {
                                    IList<IPart> parts = new List<IPart>();
                                    parts.Add(part);
                                    share_parts_set.Add(part.PN, parts);
                                    check_typ_set.Add(part.PN, part.BOMNodeType.Trim());
                                }
                                else
                                {
                                    ((IList<IPart>) share_parts_set[part.PN]).Add(part);
                                }
                                break;
                            }
                        }
                    }
                    if (share_parts_set.Count > 0)
                    {
                        IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                        foreach (DictionaryEntry de in share_parts_set)
                        {
                            var flat_bom_item = new FlatBOMItem(qty, part_check_type, (IList<IPart>)de.Value);
                            flat_bom_item.PartNoItem = bom.Model;
                            flat_bom_item.Tp = (string) check_typ_set[de.Key];
                            flat_bom_item.Descr = "Second Pizza";
                            flat_bom_items.Add(flat_bom_item);
                        }
                        ret = new FlatBOM(flat_bom_items);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return ret;
        }
        public bool CheckCondition(object node)
        {
            //Model 的直接下阶存在Part Type 为'CN','C5','V2' 的记录
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_cn = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("CN");
            bool is_c5 = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("C5");
            bool is_v2 = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("V2");
            if (is_cn || is_c5 || is_v2)
                return true;
            return false;
        }
    }
}
