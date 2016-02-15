using System;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.PP.Filter
{
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "PP";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展1阶，得到第一阶是PP的partNo [ PP]，
            IFlatBOMItem ret = null;
            var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            if (bom.FirstLevelNodes != null)
            {
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("PP"))
                    {
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                        {
                            IPart part = ((BOMNode) bom.FirstLevelNodes.ElementAt(i)).Part;
                            parts.Add(part);
                        }
                    }
                }
            }
            if (parts.Count > 0)
            {
                if (bom.FirstLevelNodes != null && bom.FirstLevelNodes.Count > 0)
                {
                    _qty = bom.FirstLevelNodes.ElementAt(0).Qty;
                    ret = new FlatBOMItem(_qty, part_check_type, parts);
                }
            }
            return ret;
        }
        public bool CheckCondition(object node)
        {
            //第一阶的Remark不为( 'PA'）和第一阶的Remark不为( 'CPQ'）即（a.Remark<>'PA' and a.Remark<>'CPQ'）
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_not_pa = ((BOMNode)node).Part.Remark.Trim().Equals("PA");
            bool is_not_cpq = ((BOMNode)node).Part.Remark.Trim().Equals("CPQ");
            if (is_not_pa && is_not_cpq)
                return true;
            return false;
        }
    }
}
