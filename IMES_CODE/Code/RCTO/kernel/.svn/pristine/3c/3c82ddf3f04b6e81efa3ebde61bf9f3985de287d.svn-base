// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2011-03-05   210003                       ITC-1360-0787
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.DN;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using System.ComponentModel.Composition;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.RoyaltyPaper.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.RoyaltyPaper.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const int qty = 1;
        private const string part_check_type = "RoyaltyPaper";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
            IFlatBOM ret = null;
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            try
            {
                string[] param = {
                                    "IMES.CheckItemModule.RoyaltyPaper.Filter.FilterBOM",
                                    "mainObject"
                                 };
                if (null == main_object)
                    return ret;
                    //throw new FisException("CHK156", param);
                var product = (Product)main_object;
                IDeliveryRepository delivery_repository = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository, Delivery>();
                Delivery delivery = delivery_repository.GetDelivery(product.DeliveryNo.Trim());
                if (delivery != null)
                {
                    if (delivery.PoNo.IndexOf("BF3") == 0 || delivery.PoNo.IndexOf("BFD") == 0)
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
                                    }
                                    else
                                    {
                                        ((IList<IPart>)share_parts_set[part.PN]).Add(part);
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
                        flat_bom_item.PartNoItem = (string)de.Key;
                        flat_bom_item.Tp = "P1";
                        flat_bom_item.Descr = "ROYALTY PAPER " + (string)de.Key;
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
            //SKU 对应的Delivery 的PoNo (IMES_PAK..Delivery.PoNo) 以'BF3' 或者'BFD' 为前缀，当PC 阶下存在UPPER(Descr) = 'ROYALTY PAPER'的P1 Part
            //问题：如何找到PoNo
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            if (((BOMNode)node).Part.BOMNodeType.Trim().Equals("P1"))
            {
                bool is_royalty_paper = ((BOMNode)node).Part.Descr.Trim().ToUpper().Equals("ROYALTY PAPER");
                if (is_royalty_paper)
                    return true;
            }

            return false;
        }
    }
}
