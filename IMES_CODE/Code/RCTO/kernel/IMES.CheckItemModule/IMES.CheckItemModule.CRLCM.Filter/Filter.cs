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

namespace IMES.CheckItemModule.CRLCM.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CRLCM.Filter.dll")]
    public class Filter : IFilterModule
    {
        //private int qty = 1;//问题：Pizza Kitting。该情况下，如何计算qty。
        private const string part_check_type = "CRLCM";
        private const string descr = "Clean Room";
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //问题：station参数用不上。因为在hierarchicalBOM中，没有与station相关的字段。
            IFlatBOM ret = null;
            Hashtable share_parts_set = new Hashtable();
            Hashtable qty_set = new Hashtable();
            Hashtable descr_parts_set = new Hashtable();
            List<string> pnName = new List<string>();
            string firstPn="";
            int qty = 0;
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
                            IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                            pnName.Add(part.PN);
                            qty = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty;
                            if (firstPn != "" && share_parts_set.ContainsKey(firstPn))
                                    {
                                        ((IList<IPart>)share_parts_set[firstPn]).Add(part);
                               
                                    }
                                    else
                                    {
                                        IList<IPart> parts = new List<IPart>();
                                        parts.Add(part);
                                        share_parts_set.Add(part.PN, parts);
                                        firstPn = part.PN;
                                      
                                    }
                        
                        }
                    }

                   
                    if (share_parts_set.Count > 0)
                    {
                        IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                        foreach (DictionaryEntry de in share_parts_set)
                        {
                            var flat_bom_item = new FlatBOMItem(qty, part_check_type, (IList<IPart>)de.Value);
                            flat_bom_item.PartNoItem = string.Join(",", pnName.ToArray());
                            flat_bom_item.Descr = descr;
                            flat_bom_item.Model = bom.Model;
                            flat_bom_item.Tp = "PL";
                            flat_bom_items.Add(flat_bom_item);
                           
                        }
                        if (flat_bom_items.Count > 0)
                        {
                            ret = new FlatBOM(flat_bom_items);
                        }
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
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            //根据Model展下阶得到PL && Descr=Clean Room
            bool is_cn = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("PL") &&
                                     ((BOMNode)node).Part.Descr.Equals("Clean Room");
          

            if (is_cn)
                return true;
            return false;
        }
    }
}
