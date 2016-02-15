using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.TabletC1C2C3C4.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TabletC1C2C3C4.Filter.dll")]
    public class MatchModule : IMatchModule
    {
        public object Match(string subject, object bomItem, string station)
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
            //14码，前5位等于VC
            
            string vendor_code = "";
          //  if (subject.Length == 14)
          // {
                vendor_code = subject;
       //    }
                if (vendor_code.Length < 6)
                {
                    return ret;
                }
    //    if (vendor_code.Length == 14)
       //   {
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                if (parts != null)
                {
                    foreach (IPart part in parts)
                    {
                        IList<PartInfo> part_infos = part.Attributes;
                        if (part_infos != null)
                        {
                            foreach (PartInfo part_info in part_infos)
                            {
                                if (part_info.InfoType.Equals("VendorCode") && !string.IsNullOrEmpty(part_info.InfoValue))
                                {
                                    int len = part_info.InfoValue.Length;
                                   // if (part_info.InfoValue.Trim().Equals(vendor_code.Trim().Substring(0, 5)))
                                    if (vendor_code.Trim().Length>=len && part_info.InfoValue.Trim().Equals(vendor_code.Trim().Substring(0, len)))
                                   
                                    {
                                        ret = new PartUnit(part.PN, vendor_code.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                                        break;
                                    }
                                }
                            }
                            if (ret != null)
                            {
                                break;
                            }
                        }
                    }
                }
         //  }
                if (ret == null)
                {
                    ret = (PartUnit)MatchPartNo(subject, bomItem, station);
                }
           return ret;
        }

        private string GetInfoType(IList<PartInfo> part_infos, string infoName)
        {
            string ret = "";
            foreach (PartInfo part_info in part_infos)
            {
                if (part_info.InfoType.Equals(infoName))
                {
                    ret = part_info.InfoValue.Trim();
                    break;
                }
            }
            return ret;
        }

        public Object MatchPartNo(string subject, object bomItem, string station)
        {
            string vendorCode = "";
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
                    vendorCode = GetInfoType(flat_bom_item.Attributes, "VendorCode");
                    if (string.IsNullOrEmpty(vendorCode) && flat_bom_item.PN.Equals(subject.Trim()))
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

                            string part_pn = flat_bom_item.PN.Substring(3, flat_bom_item.PN.Length - 3);
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
                            if (part_pn.Length > 3)
                            {
                                part_pn = part_pn.Substring(3, part_pn.Length - 3);
                                if (part_pn.Length > 10)
                                {
                                    part_pn = part_pn.Substring(0, 10);
                                }
                                if (part_pn.Equals(subject.Trim()) || part_pn.Equals(subject.Trim()))
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
                                string TabletKBCT = subject.Trim();
                               if (subject.Trim().Length>10)
                               {
                                   subject = subject.Substring(0, 10);
                               }
                                    if (part_pn.Equals(subject.Trim()) || part_pn.Equals(subject.Trim()))
                                    {
                                        ret = new PartUnit(flat_bom_item.PN.Trim(), TabletKBCT, flat_bom_item.BOMNodeType, flat_bom_item.Type, "", flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
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
