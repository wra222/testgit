﻿// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.PizzaPart.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.PizzaPart.Filter.dll")]
    public class MatchModule : IMatchModule
    {
        public Object Match(string subject, object bomItem, string station)
        {
            if (subject == null)
            {
                throw new ArgumentNullException();
            }
            if (bomItem == null)
            {
                throw new ArgumentNullException();
            }
            PartUnit ret = null;
            IList<IPart> flat_bom_items = ((IFlatBOMItem)bomItem).AlterParts;
            if (flat_bom_items != null)
            {
                foreach (IPart flat_bom_item in flat_bom_items)
                {
                    //For Mantis 499
                    if (subject.Trim().StartsWith("ISH"))
                    {
                        if (flat_bom_item.PN.Trim().Length >3)
                        {
                            string part_pn = flat_bom_item.PN.Trim();
                            if (flat_bom_item.PN.Substring(0, 3).Equals("DIB") || flat_bom_item.PN.Substring(0, 3).Equals("MMI"))
                            {
                                part_pn = flat_bom_item.PN.Trim().Substring(3, flat_bom_item.PN.Length - 3);
                            }
//                            string ISH_part_pn = flat_bom_item.PN.Trim().Substring(6, flat_bom_item.PN.Length - 6);
                            if (subject.Trim().StartsWith(part_pn))
                            {
                                ret = new PartUnit(flat_bom_item.PN.Trim(), subject.Trim(), flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                break;
                            }
                        }
                      
                    }
                
                    //For Mantis 410
                    //if (subject.Trim().StartsWith("ISH") &&  subject.Trim().StartsWith(flat_bom_item.PN))
                    //{
                    //    ret = new PartUnit(flat_bom_item.PN.Trim(), subject.Trim(), flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                    //    break;
                    //}
                    //For Mantis 410
                    if (flat_bom_item.PN.Equals(subject.Trim()))
                    {
                        ret = new PartUnit(subject.Trim(), string.Empty, flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                        break;
                    }
                }
                if (ret == null)
                {
                    foreach (IPart flat_bom_item in flat_bom_items)
                    {
                        if (flat_bom_item.PN.Substring(0, 3).Equals("DIB"))
                        {

                            string part_pn = flat_bom_item.PN.Substring(3, flat_bom_item.PN.Length -3);
                            if (part_pn.Equals(subject.Trim()))
                            {
                                ret = new PartUnit(flat_bom_item.PN.Trim(), string.Empty, flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                break;
                            }
                        }
                    }
                    if (ret == null)
                    {
                        foreach (IPart flat_bom_item in flat_bom_items)
                        {
                            string part_pn = flat_bom_item.PN.Trim();
                           // string ISH_part_pn = flat_bom_item.PN.Trim();
                            if (part_pn.Length > 3)
                            {
                                part_pn = part_pn.Substring(3, part_pn.Length - 3);
                                if (part_pn.Length > 10)
                                {
                                    part_pn = part_pn.Substring(0, 10);
                                }
                    //            ISH_part_pn = flat_bom_item.PN.Trim().Substring(3, part_pn.Length - 3);
                                //For Mantis 410
                                //if (subject.Trim().StartsWith("ISH") && subject.Trim().StartsWith(ISH_part_pn))
                                //{
                                //    ret = new PartUnit(flat_bom_item.PN.Trim(), subject.Trim(), flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                //    break;
                                //}
                                //For Mantis 410

                                if (part_pn.Equals(subject.Trim()) || part_pn.Equals(subject.Trim()))
                                {
                                    ret = new PartUnit(flat_bom_item.PN.Trim(), string.Empty, flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
}
