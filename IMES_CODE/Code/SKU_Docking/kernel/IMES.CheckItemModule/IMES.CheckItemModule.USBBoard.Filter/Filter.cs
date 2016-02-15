// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.USBBoard.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.USBBoard.Filter.dll")]
    public class Filter : IFilterModule
    {
        private int _qty;
        private const string part_check_type = "USBBoard";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //
			IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IFlatBOM ret = null;
            var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            String pn_value_string = "";
            String descr_info_value_string = "";

            if (bom.FirstLevelNodes != null)
            {
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("PL"))
                    {
                        if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                        {
                            IPart part = bom.FirstLevelNodes.ElementAt(i).Part;
							IPart p = repository.GetPartByPartNo(part.PN);
							if (null == p)
								continue;
							
                            IList<PartInfo> part_infos = bom.FirstLevelNodes.ElementAt(i).Part.Attributes;
                            
                            if (part_infos != null)
                            {
                                foreach (PartInfo part_info in part_infos)
                                {
                                    if (part_info.InfoType.Equals("Descr"))
                                    {
                                        if (descr_info_value_string.Length == 0)
                                        {
                                            descr_info_value_string = part_info.InfoValue;
                                        }
                                        else
                                        {
                                            if (!descr_info_value_string.Contains(part_info.InfoValue))
                                            {
                                                descr_info_value_string += "," + part_info.InfoValue;
                                            }
                                        }
                                    }
                                }
                            }
                            
							if (!pn_value_string.Contains(part.PN))
							{
								if (pn_value_string.Length == 0)
									pn_value_string = part.PN;
								else
									pn_value_string += "," + part.PN;
							}
							
							parts.Add(part);
							_qty = bom.FirstLevelNodes.ElementAt(i).Qty;
                            
                        }
                    }
                }
            }
            if (parts.Count > 0)
            {
                if (bom.FirstLevelNodes != null && bom.FirstLevelNodes.Count > 0)
                {
//                    _qty = bom.FirstLevelNodes.ElementAt(0).Qty;
                    var flat_bom_item = new FlatBOMItem(_qty, part_check_type, parts);
                    flat_bom_item.PartNoItem = pn_value_string;
                    flat_bom_item.Descr = descr_info_value_string;
                    IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
                    flat_bom_items.Add(flat_bom_item);
                    ret = new FlatBOM(flat_bom_items);
                }
            }
            return ret;
        }
        public bool CheckCondition(object node)
        {
            //
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_descr = ((BOMNode)node).Part.Descr.Trim().Contains("USBboard");
            if (is_descr)
                return true;
            return false;
        }
    }
}
