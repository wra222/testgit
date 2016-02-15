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
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.DockingBaseSN.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingBaseSN.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const int qty = 1;
        private const string part_check_type = "DockingBaseSN";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
           
            IFlatBOM ret = null;

            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            var bomNodeList = bom.FirstLevelNodes;
            //PB階下所有料號，即為需要收集的數量(無共用料情況).ModelBom.Qty 為每顆料需收集的數量
            try
            {
                if (bomNodeList != null)
                {
                    var partList = bomNodeList.Where(x => x.Part.BOMNodeType == "PB")
                                                            .Select(y => new { Part = y.Part, Qty = y.Qty });
                    if (partList.Count() > 0)
                    {
                        foreach (var item in partList )
                        {
                            var flat_bom_item = new FlatBOMItem(item.Qty, part_check_type, new List<IPart> { item.Part});
                            flat_bom_item.PartNoItem = item.Part.PN;
                            flat_bom_item.Tp = part_check_type;
                            flat_bom_item.Descr =  item.Part.Descr;
                            flat_bom_items.Add(flat_bom_item);
                        }

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
    }
}
