// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-03-03   210003                       ITC-1360-0831
// 2012-03-09   210003                       ITC-1360-1198
// 2012-03-09   210003                       UC changed(yongbo)
// 2012-03-15   210003                       ITC-1360-1474
// 2012-03-16   210003                       ITC-1360-1491
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.FirstPizzaID.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.FirstPizzaID.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int qty = 1;
        private string part_check_type = "1stPizzaID";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
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
//                            IPart part = ((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Part;
//                            parts.Add(part);
                            IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                            if (!share_parts_set.ContainsKey(part.PN))
                            {
                                IList<IPart> parts = new List<IPart>();
                                parts.Add(part);
                                share_parts_set.Add(part.PN, parts);
                            }
                            else
                            {
                                ((IList<IPart>)share_parts_set[part.PN]).Add(part);
                            }
                            break;
                        }
                    }
                }

                if (share_parts_set.Count > 0)
                {
                    foreach (DictionaryEntry de in share_parts_set)
                    {
                        var flat_bom_item = new FlatBOMItem(qty, part_check_type, (IList<IPart>)de.Value);
                        flat_bom_item.PartNoItem = "";// (string)de.Key;
                        flat_bom_item.Tp = "";
                        flat_bom_item.Descr = "C-Kit";// +(string)de.Key
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
            //若Model 阶下存在Remark 前两位为PK 或者Remark 中含有DESC 属性前两位为PK 的VK / C2 Part 或者Model 阶下存在Descr LIKE 'OOA%' 的P1 

            if (((BOMNode)node).Part == null)
            {
                return false;
            }
//            bool contain_remark_pk = ((BOMNode)node).Part.Remark.Trim().Contains("PK");
            bool contain_remark_pk = false;
            bool contain_desc_pk = false;
            //1.若Model 阶下存在Remark 前两位为PK 的VK / C2 Part；
            if (((BOMNode)node).Part.BOMNodeType.Trim().Equals("VK") || ((BOMNode)node).Part.BOMNodeType.Trim().Equals("C2"))
            {
                string remark = ((BOMNode) node).Part.Remark.Trim();
                if (!string.IsNullOrEmpty(remark))
                {
                    if (remark.Substring(0, 2).Equals("PK"))
                    {
                        contain_remark_pk = true;
                    }
                }
//                2.若Model 阶下存在Remark 中含有DESC 属性前两位为PK 的VK / C2 Part 
                IList<PartInfo> part_infos = ((BOMNode) node).Part.Attributes;
                if (part_infos != null)
                {
                    foreach (PartInfo part_info in part_infos)
                    {
                        if (part_info.InfoType.Trim().Equals("DESC"))
                        {
                            string info_value = part_info.InfoValue.Trim();
                            if (info_value.Substring(0, 2).Equals("PK"))
                            {
                                contain_desc_pk = true;
                                break;
                            }
                        }
                    }
                }
            }

            //3.Model 阶下存在Descr LIKE 'OOA%' 的P1 Part.
            int ooa_point = ((BOMNode)node).Part.Descr.Trim().IndexOf("OOA");
            bool contain_ooa = ((BOMNode)node).Part.Descr.Trim().Contains("OOA");
            bool is_p1 = ((BOMNode)node).Part.BOMNodeType.Trim().Equals("P1");
            if (contain_remark_pk || contain_desc_pk  || (ooa_point == 0 && contain_ooa && is_p1))
                return true;
            return false;
        }
    }
}
