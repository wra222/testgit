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
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.FisObject.Common.Model;

namespace IMES.CheckItemModule.TouchGlass.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TouchGlass.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const string part_check_type = "TouchGlass";
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            IList<IFlatBOMItem> flat_bom_items = new List<IFlatBOMItem>();
            Hashtable share_parts_set = new Hashtable();
            Hashtable share_part_no_set = new Hashtable();
            Hashtable qty_share_parts_set = new Hashtable();
            Hashtable descr_parts_set = new Hashtable();
            Hashtable check_typ_set = new Hashtable();
            IFlatBOM ret = null;

            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            var bom = (HierarchicalBOM)hierarchical_bom;
            try
            {
                Session session = GetSession(main_object);
                Product product = session.GetValue(Session.SessionKeys.Product) as Product;
                if (product == null)
                {
                    throw new FisException("Can not get product in " + part_check_type + " session");
                }
                if (string.IsNullOrEmpty(product.CUSTSN) || product.CUSTSN.Length < 5)
                {
                    throw new FisException("Can not get CUSTSN in " + part_check_type + " product");
                }

                IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                string sn5 = product.CUSTSN.Substring(0, 5);
                AssemblyVCInfo ai = new AssemblyVCInfo();
                ai.vc = sn5;
                IList<AssemblyVCInfo> lstAI = partRepository.GetAssemblyVC(ai);

                IList<string> lstVC = new List<string>();
                if (lstAI != null && lstAI.Count > 0)
                {
                    foreach (AssemblyVCInfo a in lstAI)
                    {
                        lstVC.Add(a.combineVC);
                    }
                }
                else
                {
                    throw new FisException("CQCHK0004", new string[] { sn5 });
                }

                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                        if (part != null && part.BOMNodeType.Trim().Equals("PL") && "TPM".Equals(part.Descr) )
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
                                            if (lstVC.Contains(part_info.InfoValue))
                                            {
                                                vendorcodeValue = part_info.InfoValue;
                                            }
										}
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
                    else
                    {
                        throw new FisException("CQCHK0005", new string[] { product.Model });
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
                    if (!string.IsNullOrEmpty(part_info.InfoValue) && "TYPE".Equals(part_info.InfoType) && (part_info.InfoValue.Length >= 5) && "TOUCH".Equals(part_info.InfoValue.Substring(0, 5).ToUpper()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Session GetSession(object main_object)
        {
            string objType = main_object.GetType().ToString();
            Session.SessionType sessionType = Session.SessionType.Product;
            Session _currentSession = null;
            if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
            {
                IProduct prd = (IProduct)main_object;
                _currentSession = SessionManager.GetInstance.GetSession
                                               (prd.ProId, sessionType);
            }
            else if (main_object.GetType().Equals(typeof(IMES.Infrastructure.Session)))
            {
                _currentSession = (Session)main_object;
            }

            if (_currentSession == null)
            {
                throw new FisException("Can not get session in " + part_check_type + " module");
            }
            return _currentSession;
        }

    }
}
