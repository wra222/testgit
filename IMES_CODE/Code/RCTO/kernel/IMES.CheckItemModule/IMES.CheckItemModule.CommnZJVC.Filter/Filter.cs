using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure;
using IMES.DataModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CommnZJVC.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CommnZJVC.Filter.dll")]
    public class Filter : IFilterModule, ITreeTraversal
    {
        public const string part_check_type = "CommnZJVC";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IFlatBOM ret = null;
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            //var kp_parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            Hashtable qty_set = new Hashtable();
            Hashtable share_parts_set = new Hashtable();
            Hashtable descr_kp_part_set = new Hashtable();
            Hashtable check_typ_set = new Hashtable();
            Hashtable rule_set = new Hashtable();

            try
            {
                UtilityCommonImpl utl = UtilityCommonImpl.GetInstance();
                IList<CheckItemTypeRuleDef> filteredChkItemRules = new List<CheckItemTypeRuleDef>();
                IList<CheckItemTypeRuleDef> lstChkItemRule = utl.GetCheckItemTypeRule(main_object, part_check_type, station);
                if (null == lstChkItemRule || lstChkItemRule.Count == 0)
                {
                    // 未維護CheckItemTypeRule，請先維護!
                    throw new FisException("CHK1151", new string[] { part_check_type });
                }

                int qty = 0;
                IList<IBOMNode> bom_nodes = bom.FirstLevelNodes;
                foreach (IBOMNode bom_node in bom_nodes)
                {
                    if (UtilityCommonImpl.GetInstance().CheckByItemTypeRule(bom_node.Part, lstChkItemRule, out filteredChkItemRules))
                    {
                        if (!share_parts_set.ContainsKey(bom_node.Part.Descr))
                        {
                            qty = bom_node.Qty;
                            qty_set.Add(bom_node.Part.Descr, bom_node.Qty);

                            IList<IPart> share_parts = new List<IPart>();
                            share_parts.Add(bom_node.Part);
                            share_parts_set.Add(bom_node.Part.Descr, share_parts);

                            string _partNoItem = "";
                            _partNoItem += "," + bom_node.Part.PN;
                            descr_kp_part_set.Add(bom_node.Part.Descr, _partNoItem);
                            check_typ_set.Add(bom_node.Part.Descr, bom_node.Part.Type);
                            rule_set.Add(bom_node.Part.Descr, filteredChkItemRules);
                        }
                        else
                        {
                            if (qty != bom_node.Qty)
                            {
                                // @PartDescr 的共用料收料數量不一致，請聯繫SIE!
                                throw new FisException("CHK1152", new string[] { bom_node.Part.Descr });
                            }
                            IList<IPart> share_parts = (IList<IPart>)share_parts_set[bom_node.Part.Descr];
                            share_parts.Add(bom_node.Part);

                            string _partNoItem = (string)descr_kp_part_set[bom_node.Part.Descr];
                            _partNoItem += "," + bom_node.Part.PN;
                            descr_kp_part_set[bom_node.Part.Descr] = _partNoItem;
                        }
                    }
                }

                foreach (DictionaryEntry de in share_parts_set)
                {
                    string _partNoItem = (string)descr_kp_part_set[de.Key];

                    filteredChkItemRules = (IList<CheckItemTypeRuleDef>)rule_set[de.Key];

                    var kp_flat_bom_item = new FlatBOMItem((int)qty_set[de.Key], part_check_type, (IList<IPart>)share_parts_set[de.Key]);
                    kp_flat_bom_item.PartNoItem = _partNoItem.Substring(1);
                    kp_flat_bom_item.Descr = (string)de.Key;
                    kp_flat_bom_item.CheckItemTypeRuleList = filteredChkItemRules;
                   // kp_flat_bom_item.NeedSave = "Y" == filteredChkItemRules[0].NeedCommonSave;
                   // kp_flat_bom_item.NeedCheckUnique = "Y" == filteredChkItemRules[0].NeedUniqueCheck;
                   // kp_flat_bom_item.NeedCommonSave = "Y" == filteredChkItemRules[0].NeedCommonSave;

                    flat_bom_items.Add(kp_flat_bom_item);
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
            return true;
        }

    }


}
