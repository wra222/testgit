using System;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.ATSN3.Filter
{
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "ATSN3";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展1阶，得到第一阶是AT的partNo [ AT]，
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
                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("AT"))
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
            //并且第一阶的Descr描述为'ATSN3', 
            //在Product_Part表中找到此Part和Product结合的PartSN（Product_Part.PartID=PartNo and Product_Part.ProductID=ProductID# and Product_Part.BomNodeType='AT'）。后半句不属于Filter．  
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_atsn1 = ((BOMNode)node).Part.Descr.Trim().Equals("ATSN3");
            if (is_atsn1)
                return true;
            return false;
        }
    }
}
