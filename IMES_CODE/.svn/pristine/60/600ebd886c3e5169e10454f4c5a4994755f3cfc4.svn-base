// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-03-07   210003                       ITC-1360-1175
// 2011-03-07   210003                       ITC-1360-1181
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel.Composition;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.FisObject.PAK.BSam;

namespace IMES.CheckItemModule.Battery.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.BsamBattery.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const string part_check_type = "BsamBattery";
        private int _qty=1;
        private string _descr="";
        private string _partNo = "";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IFlatBOM ret = null;
          
            var parts = new List<IPart>();
            #region check input parameter
            //main_object 需要是IProduct
            IProduct prod = (IProduct)main_object;
            if (prod == null)
            {
                throw new FisException("FIL001", new string[] { part_check_type, "Product" });
            }
            if (hierarchical_bom == null)
            {
                throw new FisException("FIL001", new string[] { part_check_type, "BOM" });
            }
            var bom = (HierarchicalBOM)hierarchical_bom;

            if (bom.FirstLevelNodes == null || bom.FirstLevelNodes.Count == 0)
            {
                throw new FisException("FIL001", new string[] { part_check_type, "BOM" });
            }

            #endregion

            #region Check Bom structure & Filter Part
            //Check BSam Model

            IBSamRepository bsamRep = RepositoryFactory.GetInstance().GetRepository<IBSamRepository>();
            BSamModel  bsamModel=bsamRep.GetBSamModel(prod.Model);
            if (bsamModel == null)
            {
                return null;
            }

            var matchBomNode = (from item in bom.FirstLevelNodes
                              where ((BOMNode)item).Part.BOMNodeType.Equals("PL") && 
                                           ((BOMNode)item).Part.Type.Equals("BSAM Dummy Battery") 
                              select  item).ToList();
            foreach (IBOMNode item in matchBomNode)
            {
                //_qty=item.Qty;
                parts.Add(item.Part);

                _descr = item.Part.Descr;
                _partNo = _partNo + (string.IsNullOrEmpty(_partNo) ? "" : ",") + item.Part.PN;
            }


            #endregion

            #region Generate FlatBom with filter Part for return
            if (parts.Count > 0)
            {
                var flat_bom_item = new FlatBOMItem(_qty, part_check_type, parts);
                flat_bom_item.Descr = _descr;
                flat_bom_item.PartNoItem = _partNo;
                IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                flat_bom_items.Add(flat_bom_item);
                ret = new FlatBOM(flat_bom_items);                
            }
            #endregion
            return ret;
        }
    }
}
