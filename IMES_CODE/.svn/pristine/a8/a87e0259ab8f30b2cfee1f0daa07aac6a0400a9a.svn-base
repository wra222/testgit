// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date                 Name                         Reason 
// ============================================
// 2013-05-29       Vincent                       Create
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.KBCheck.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.KBCheck.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const string part_check_type = "KBCheck";
        private int _qty = 1;
        private string _descr = "";
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
            //Collect KB Part
            IList<IProductPart> prodPartList = prod.ProductParts;
            if (prodPartList == null || prodPartList.Count == 0)
            {
                // need change code
                throw new FisException("FIL004", new string[] { prod.ProId });
            }
            var kbPart = (from item in prodPartList
                          where item.CheckItemType == "KB"
                          select item).OrderByDescending(p => p.Udt).ToList();
            if (kbPart == null || kbPart.Count == 0)
            {
                //need change error Code
                throw new FisException("FIL004", new string[] { prod.ProId});
            }

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IPart part = partRep.Find(kbPart[0].PartID);
            parts.Add(part);

            _descr = "KBCheck"; //part.Descr;
            _partNo = kbPart[0].PartSn.Substring(0, 5);

            #endregion

            #region Generate FlatBom with filter Part for return
            if (parts.Count > 0)
            {
                var flat_bom_item = new FlatBOMItem(_qty, part_check_type, parts);
                flat_bom_item.Descr = _descr;
                flat_bom_item.PartNoItem = _partNo;
                flat_bom_item.ValueType = kbPart[0].PartSn;
                IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                flat_bom_items.Add(flat_bom_item);
                ret = new FlatBOM(flat_bom_items);
            }
            #endregion
            return ret;
        }
    }  
}
