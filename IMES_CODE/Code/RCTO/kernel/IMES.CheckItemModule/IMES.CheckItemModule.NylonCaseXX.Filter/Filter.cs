// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-12-19   210003                       ITC-1360-0851
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.NylonCaseXX.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.NylonCaseXX.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private const string part_check_type = "NylonCaseXX";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
            Hashtable descr_share_parts_set = new Hashtable();
            Hashtable share_part_no_set = new Hashtable();
            IFlatBOM ret = null;
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
                            if (!share_parts_set.ContainsKey(part.Descr))
                            {
                                IList<IPart> parts = new List<IPart>();
                                parts.Add(part);
                                share_parts_set.Add(part.Descr, parts);
                                descr_share_parts_set.Add(part.Descr, part.Descr + "_" + part.PN);
                                if (part.PN.Substring(0, 3).Equals("DIB") )
                                {
                                    share_part_no_set.Add(part.Descr, part.PN.Substring(3,part.PN.Length-3));
                                }
                                else
                                {
                                    share_part_no_set.Add(part.Descr, part.PN);
                                }
                            }
                            else
                            {
                                ((IList<IPart>)share_parts_set[part.Descr]).Add(part);
                                if (!((string)share_part_no_set[part.Descr]).Contains(part.PN))
                                {
                                    if (part.PN.Substring(0, 3).Equals("DIB"))
                                    {
                                        share_part_no_set[part.Descr] += "," + part.PN.Substring(3,part.PN.Length-3);
                                    }
                                    else
                                    {
                                        share_part_no_set[part.Descr] += "," + part.PN;
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
                        var flat_bom_item = new FlatBOMItem(qty, part_check_type, (IList<IPart>)de.Value);
                        flat_bom_item.PartNoItem = (string) share_part_no_set[de.Key];
                        flat_bom_item.Tp = "C2";
                        flat_bom_item.Descr = (string) descr_share_parts_set[de.Key];
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
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_here = ((BOMNode)node).Part.Descr.Trim().Contains("NYLON");
            int start_point = ((BOMNode)node).Part.Descr.Trim().LastIndexOf("NYLON");
            bool is_c2 = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("C2");
            if (is_here && start_point == 0 && is_c2)
                return true;
            return false;
        }
    }
}
