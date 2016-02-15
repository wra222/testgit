// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// Known issues:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using System.ComponentModel.Composition;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;


namespace IMES.CheckItemModule.Camera.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.Camera.Filter.dll")]
    public class Filter : IFilterModule
    {
        //private int qty = 1;//问题：Pizza Kitting。该情况下，如何计算qty。
          private const string part_check_type = "Camera";
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
            Hashtable share_part_no_set = new Hashtable();
            Hashtable qty_share_parts_set = new Hashtable();
            Hashtable descr_parts_set = new Hashtable();
            Hashtable check_typ_set = new Hashtable();
            //问题：station参数用不上。因为在hierarchicalBOM中，没有与station相关的字段。
            IFlatBOM ret = null;
            //int qty = 0;
            //var parts = new List<IPart>();
     
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            if (checkCleanRoomModel(bom, station)) //FA 3M Station CleanRoom not check Camera
                {
                    return null;
                }
           
            try
            {
                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                        if (part != null && part.BOMNodeType.Trim().Equals("PL") && "TPM".Equals(part.Descr) && (0 != part.PN.IndexOf("151")))
                        {

                            if (CheckCondition(bom.FirstLevelNodes.ElementAt(i)))
                            {
                                string vendorcodeValue = "";
								Boolean exist_share_part = false;
                                IList<PartInfo> part_infos = part.Attributes;
                                if (part_infos != null && part_infos.Count > 0)
                                {
                                    foreach (PartInfo part_info in part_infos)
                                    {
                                        if (part_info.InfoType.Equals("VendorCode") && !string.IsNullOrEmpty(part_info.InfoValue)) {
											vendorcodeValue = part_info.InfoValue;
										}
                                        /*else if (part_info.InfoType.Equals("SUB") && !string.IsNullOrEmpty(part_info.InfoValue))
                                        {
                                            exist_share_part = true;

                                            String[] share_parts = part_info.InfoValue.Trim().Split(';');
                                            if (share_parts.Length > 0)
                                            {
                                                string share_part_no = part_info.InfoValue.Trim();
                                                //                                                share_part_no = share_part_no.Replace("DIB", "");
                                                //                                                share_part_no_set.Add(part.PN, share_part_no.Replace(';', ','));
                                                if (share_part_no_set.ContainsKey(part.PN))
                                                {
                                                    share_part_no_set[part.PN] += "," + share_part_no.Replace(';', ',');
                                                }
                                                else
                                                {
                                                    share_part_no_set.Add(part.PN, share_part_no.Replace(';', ','));
                                                }
                                                IPartRepository repository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                                                IList<IPart> parts = new List<IPart>();
                                                parts.Add(part);
                                                for (int j = 0; j < share_parts.Length; j++)
                                                {
                                                    IPart share_part = repository.GetPartByPartNo(share_parts[j]);
                                                    if (share_part != null)
                                                    {
                                                        parts.Add(share_part);
                                                    }
                                                }
                                                String share_parts_code = part.PN;
                                                if (!share_parts_set.ContainsKey(share_parts_code))
                                                {
                                                    share_parts_set.Add(share_parts_code, parts);
                                                    qty_share_parts_set.Add(share_parts_code, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                                    descr_parts_set.Add(share_parts_code, part.Descr);
                                                    check_typ_set.Add(share_parts_code, part.BOMNodeType.Trim());
                                                }
                                                else
                                                {
                                                    ((IList<IPart>)share_parts_set[share_parts_code]).Add(part);
                                                    if (!((String)descr_parts_set[share_parts_code]).Contains(part.Descr))
                                                    {
                                                        descr_parts_set[share_parts_code] += "," + part.Descr;
                                                    }
                                                }
                                            }
                                        }*/
                                    }
                                }

                                if (string.IsNullOrEmpty(vendorcodeValue))
                                    continue;

                                if (!exist_share_part)
                                {
                                    if (!share_parts_set.ContainsKey(vendorcodeValue))
                                    {
                                        IList<IPart> parts = new List<IPart>();
                                       
                                        parts.Add(part);
                                      
                                        share_parts_set.Add(vendorcodeValue, parts);
                                        qty_share_parts_set.Add(vendorcodeValue, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                        descr_parts_set.Add(vendorcodeValue, part.Descr);
                                        check_typ_set.Add(vendorcodeValue, part.BOMNodeType.Trim());
                                    }
                                    else
                                    {
                                     
                                        ((IList<IPart>)share_parts_set[vendorcodeValue]).Add(part);
                            
                                        if (!((String)descr_parts_set[vendorcodeValue]).Contains(part.Descr))
                                        {
                                            descr_parts_set[vendorcodeValue] += "," + part.Descr;
                                        }
                                    }
                                }
                          
                            }
                        }
                    }

                    if (share_parts_set.Count > 0)
                    {
                        FlatBOMItem flat_bom_item = null;
                        string partNoItem = "";

                        foreach (DictionaryEntry de in share_parts_set)
                        {
                            if (flat_bom_item == null)
                            {
                                flat_bom_item = new FlatBOMItem((int)qty_share_parts_set[de.Key], part_check_type, (IList<IPart>)de.Value);
                                flat_bom_item.Tp = (string)check_typ_set[de.Key];
                                flat_bom_item.Descr = (string)descr_parts_set[de.Key];
                            }
                            else
                            {
                                IList<IPart> parts = share_parts_set[de.Key] as IList<IPart>;
                                if (null != parts)
                                {
                                    foreach (IPart part in parts)
                                        flat_bom_item.AddAlterPart(part);
                                }
                            }

                            if (partNoItem == "")
                                partNoItem = (string)de.Key;
                            else
                                partNoItem += "," + de.Key;
                        }

                        flat_bom_item.PartNoItem = partNoItem;
                        flat_bom_items.Add(flat_bom_item);
                    }
                    if (flat_bom_items.Count > 0)
                    {
                        ret = new FlatBOM(flat_bom_items);
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
            if (node == null)
            {
                return false;
            }
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            if (part_infos != null)
            {
                foreach (var part_info in part_infos)
                {
                    if ("Descr".Equals(part_info.InfoType) && "Camera".Equals(part_info.InfoValue))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool checkCleanRoomModel(IHierarchicalBOM curBOM,string station)
        {

            bool ret = false;
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IList<string> domesticRegIdList = partRep.GetConstValueTypeList("NeedExceptCameraStation").Select(x => x.value).ToList();
            if (domesticRegIdList != null && domesticRegIdList.Contains(station))
            {

                IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;
                if (bomNodeLst != null && bomNodeLst.Count > 0)
                {
                    var PartPLList = (from p in bomNodeLst
                                      where p.Part.BOMNodeType.ToUpper() == "PL" &&
                                                p.Part.Attributes != null &&
                                                p.Part.Attributes.Count > 0
                                      select p.Part).ToList();
                    if (PartPLList != null && PartPLList.Count > 0)
                    {
                        foreach (IPart part in PartPLList)
                        {
                            var attrList = (from p in part.Attributes
                                            where !string.IsNullOrEmpty(p.InfoType) &&
                                                      p.InfoType == "TYPE" &&
                                                      !string.IsNullOrEmpty(p.InfoValue) &&
                                                      p.InfoValue.ToUpper().StartsWith("TOUCH")
                                            select p).ToList();
                            if (attrList != null && attrList.Count > 0)
                            {
                                ret = true;
                                break;
                            }
                        }
                    }
                }
            }
            return ret;
        }
    
    }
}
