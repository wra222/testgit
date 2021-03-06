﻿// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-04-11   210003                      ITC-1360-1650
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
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.FisObject.Common.Model;
using IMES.FisObject.FA.Product;
using System.Text.RegularExpressions;

namespace IMES.CheckItemModule.TabletPL.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TabletPL.Filter.dll")]
    public class Filter : IFilterModule, ITreeTraversal
    {
     //   private int _qty;
        private const string part_check_type = "TabletPL";
      //  private Session _currentSession;
   //     private IList<CheckItemTypeRuleDef> _lstChkItemRule;
      //  private string _station;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hierarchical_bom"></param>
        /// <param name="station"></param>
        /// <param name="main_object"></param>
        /// <returns></returns>
        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
       
        //    _lstChkItemRule = null;
      //      _station = station;
           Session session= GetSession(main_object);
           IList<CheckItemTypeRuleDef> lstChkItemRule = GetCheckItemTypeRule(session);
        //   SetCheckItemTypeRule(session);
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
            try
            {
                if (bom.FirstLevelNodes != null)
                {
                    for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                    {
                        IPart part = ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Part;
                        // if (part != null && (part.BOMNodeType.Trim().Equals("PL") || part.BOMNodeType.Trim().Equals("C2") || part.BOMNodeType.Trim().Equals("VK")))
                        if (part != null && CheckBomNodeType(bom.FirstLevelNodes.ElementAt(i).Part, lstChkItemRule))
                        {

                            if (NewCheckCondition(bom.FirstLevelNodes.ElementAt(i),session,lstChkItemRule))
                            {
                                Boolean exist_share_part = false;
                                IList<PartInfo> part_infos = part.Attributes;
                                if (part_infos != null && part_infos.Count > 0)
                                {
                                    foreach (PartInfo part_info in part_infos)
                                    {
                                        if (part_info.InfoType.Equals("SUB"))
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
                                                    //                                                    IPart share_part = repository.GetPartByPartNo(share_parts[j]);
                                                    IPart share_part = null;
                                                    bool isC2WithoutVC = false;
                                                    //part
                                                    //  if (share_parts[j].Substring(0, 3).Equals("DIB") && part.BOMNodeType.Trim().Equals("C2"))
                                                    if (share_parts[j].Substring(0, 3).Equals("DIB") && CheckBomNodeType(part,lstChkItemRule))
                                                    {
                                                        share_part = repository.GetPartByPartNo(share_parts[j].Substring(3, share_parts[j].Length - 3));
                                                        if (share_part == null)
                                                        {
                                                            share_part = repository.GetPartByPartNo(share_parts[j]);
                                                        }
                                                        if (share_part != null)
                                                        {
                                                            IList<PartInfo> c2_dib_share_part_infos = share_part.Attributes;
                                                            if (c2_dib_share_part_infos != null)
                                                            {
                                                                bool have_vendor_code = false;
                                                                foreach (PartInfo c2_dib_share_part_info in c2_dib_share_part_infos)
                                                                {
                                                                    if (c2_dib_share_part_info.InfoType.Equals("VendorCode"))
                                                                    {
                                                                        have_vendor_code = true;
                                                                    }
                                                                }
                                                                if (!have_vendor_code)
                                                                {
                                                                    isC2WithoutVC = true;
                                                                    parts.Add(share_part);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        share_part = repository.GetPartByPartNo(share_parts[j]);
                                                    }
                                                    if (share_part != null && !isC2WithoutVC)
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
                                        }
                                    }
                                }

                                if (!exist_share_part)
                                {
                                    if (!share_parts_set.ContainsKey(part.PN))
                                    {
                                        IList<IPart> parts = new List<IPart>();
                                        parts.Add(part);
                                        //                                        }
                                        share_parts_set.Add(part.PN, parts);
                                        qty_share_parts_set.Add(part.PN, ((BOMNode)bom.FirstLevelNodes.ElementAt(i)).Qty);
                                        descr_parts_set.Add(part.PN, part.Descr);
                                        check_typ_set.Add(part.PN, part.BOMNodeType.Trim());
                                    }
                                    else
                                    {

                                        ((IList<IPart>)share_parts_set[part.PN]).Add(part);
                                        //                                        }
                                        if (!((String)descr_parts_set[part.PN]).Contains(part.Descr))
                                        {
                                            descr_parts_set[part.PN] += "," + part.Descr;
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
                            var flat_bom_item = new FlatBOMItem((int)qty_share_parts_set[de.Key], part_check_type, (IList<IPart>)de.Value);
                            if (share_part_no_set.ContainsKey(de.Key))
                            {
                                if (((string)de.Key).Substring(0, 3).Equals("DIB"))
                                {
                                    flat_bom_item.PartNoItem = ((string)de.Key).Substring(3, ((string)de.Key).Length - 3) + "," + (string)share_part_no_set[de.Key];
                                    flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
                                }
                                else
                                {
                                    flat_bom_item.PartNoItem = de.Key + "," + (string)share_part_no_set[de.Key];
                                    flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
                                }
                            }
                            else
                            {
                                //                                flat_bom_item.PartNoItem = (string)de.Key;
                                if (((string)de.Key).Substring(0, 3).Equals("DIB"))
                                {
                                    flat_bom_item.PartNoItem = ((string)de.Key).Substring(3, ((string)de.Key).Length - 3);
                                    flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
                                }
                                else
                                {
                                    flat_bom_item.PartNoItem = (string)de.Key;
                                    flat_bom_item.PartNoItem = flat_bom_item.PartNoItem.Replace("DIB", "");
                                }
                            }
                            flat_bom_item.Tp = (string)check_typ_set[de.Key];
                            flat_bom_item.Descr = (string)descr_parts_set[de.Key];
                            flat_bom_items.Add(flat_bom_item);
                        }
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
            //test
        
            //test
            return ret;
        }

        private Session GetSession(object main_object)
        {
          string objType=  main_object.GetType().ToString();
          Session.SessionType sessionType = Session.SessionType.Product;
          Session _currentSession=null;
          if (main_object.GetType().Equals(typeof(IMES.FisObject.FA.Product.Product)))
          {
              IProduct prd = (IProduct)main_object;
              _currentSession = SessionManager.GetInstance.GetSession
                                             (prd.ProId, sessionType);
          }
          else if(main_object.GetType().Equals(typeof(IMES.Infrastructure.Session)))
          {
              _currentSession = (Session)main_object;
          }

          if (_currentSession == null)
          {
              throw new FisException("Can not get session in "+part_check_type + " module");
          }
          return _currentSession;
        }
        private Boolean PartCompare(IList<IPart> parts, IPart part)
        {
            if (part != null && parts != null)
            {
                foreach (var apart in parts)
                {
                    if (part.PN.Trim().Equals(apart.PN.Trim()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool NewCheckCondition(object node, Session session, IList<CheckItemTypeRuleDef> lstChkItemRule)
        {
            //以及第一阶是KP的part的第一阶及其下阶[KP->VC],即KP和VC，第一阶的Descr描述为Descr like 'Fiber%'
            if (((BOMNode)node).Part == null || lstChkItemRule == null)
            {
                return false;
            }
            if (lstChkItemRule.Count == 0)
            { return false; }
            bool isBTDL = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            if (part_infos != null)
            {
                foreach (var part_info in part_infos)
                {
                    if (part_info.InfoType.Equals("Descr") && part_info.InfoValue.Trim().Equals("BTDL"))
                    {
                        isBTDL = true;
                        break;
                    }
                }
            }


            foreach (CheckItemTypeRuleDef chk in lstChkItemRule)
            {
                string[] keyArr = chk.PartDescr.Split(',');
                foreach (string key in keyArr)
                {
                    // if (part_info.InfoType.Equals("Descr") && part_info.InfoValue.Trim().Equals("BTDL"))
                    if (isBTDL && ((BOMNode)node).Part.Descr.Trim().StartsWith(key))
                    {
                        session.AddValue(part_check_type + "Regx" + key, chk.MatchRule);
                        session.AddValue(part_check_type + "SaveRule" + key, chk.SaveRule);

                        return true;
                    }
                }
            }
            return false;
            //bool is_contain_hdd = ((BOMNode)node).Part.Descr.Trim().Contains("GriphicCard ");
            //int start_point = ((BOMNode)node).Part.Descr.Trim().IndexOf("GriphicCard ");

            //if (is_contain_hdd && start_point == 0)
            //    return true;
            //return false;
        
        }
        public bool CheckCondition(object node)
        {
            return true;
        }
        private bool CheckBomNodeType(IPart part, IList<CheckItemTypeRuleDef> lstChkItemRule)
        {

            if (lstChkItemRule.Count == 0 || lstChkItemRule == null)
            { return false; }
            
            //   Regex Regex1 = new Regex(pattern);
            foreach (CheckItemTypeRuleDef chk in lstChkItemRule)
            {
                string pattern = chk.BomNodeType;
                Regex Regex1 = new Regex(pattern);
                if (part.BOMNodeType == chk.BomNodeType || Regex1.IsMatch(part.BOMNodeType))
                {return true ;}
            }
            return false;
        }
        private IList<ShareMaterialType> FilterShareMaterialType(IList<QtyParts> qty_parts)
        {
            var ret = new List<ShareMaterialType>();
            if (qty_parts != null && qty_parts.Count > 0)
            {
                foreach (QtyParts qty_part in qty_parts)
                {
                    if (qty_part.Parts != null)
                    {
                        foreach (IPart part in qty_part.Parts)
                        {
                            var material_type = new ShareMaterialType { Qty = qty_part.Qty, Descr = part.Descr };
                            if (ret.Count == 0)
                            {
                                ret.Add(material_type);
                            }
                            else
                            {
                                bool is_exist = false;
                                foreach (ShareMaterialType share_material_type in ret)
                                {
                                    if (share_material_type.Descr.Trim().Equals(material_type.Descr.Trim()) && share_material_type.Qty == material_type.Qty)
                                    {
                                        is_exist = true;
                                    }
                                }
                                if (!is_exist)
                                {
                                    ret.Add(material_type);
                                }
                            }
                            //if (!ret.Contains(material_type))
                            //{
                            //    ret.Add(material_type);
                            //}
                        }
                    }
                }
            }
            return ret;
        }

        private IList<CheckItemTypeRuleDef> GetCheckItemTypeRule(Session session)
        {
            var bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IList<CheckItemTypeRuleDef> _lstChkItemRule = null;
            string line = session.Line;
            if (line.Length > 1)
            {
                line = line.Substring(0, 1);
            }
            string model = (string)session.GetValue(Session.SessionKeys.ModelName);
            if (string.IsNullOrEmpty(model))
            {
                IProduct currentProduct = session.GetValue(Session.SessionKeys.Product) as IProduct;
                if (currentProduct != null)
                {
                    model = currentProduct.Model;
                }
            }
            string family = (string)session.GetValue(Session.SessionKeys.FamilyName);
            if (string.IsNullOrEmpty(family))
            {
                if (!string.IsNullOrEmpty(model))
                {
                    IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                    family = modelRep.Find(model).FamilyName;
                }

            }

            IList<CheckItemTypeRuleDef> lst =
                bomRepository.GetCheckItemTypeRuleWithPriority(part_check_type, line, session.Station, family);
            if (lst.Count > 0)
            {
                var min = lst.Select(p => p.Priority).Min();
                _lstChkItemRule = lst.Where(p => p.Priority == min).ToList();
            }
            return _lstChkItemRule;
        }
    }
    
    internal class ShareMaterialType
    {
        public int Qty;
        public String Descr;
    }
}
