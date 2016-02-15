using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.CheckItemModule.Utility;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.TRIS.PL.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TRIS.PL.Filter.dll")]
    public class Filter : IFilterModule, ITreeTraversal
    {
        private int _qty;
        private const string part_check_type = "TRIS-PL";
        private const string partType = "PL";

        public object FilterBOM(object hierarchical_bom, string station, object main_object)
        {
            //根据Model展2阶，得到第一阶是KP的part其下阶(注意Qty需要相乘) [ KP->VC]，即KP和VC，
            IFlatBOM flatBom = new FlatBOM();
            var parts = new List<IPart>();
            if (hierarchical_bom == null)
            {
                throw new ArgumentNullException();
            }
            
            var bom = (HierarchicalBOM)hierarchical_bom;
            if (bom.FirstLevelNodes != null)
            {
                for (int i = 0; i < bom.FirstLevelNodes.Count; i++)
                {
                    if (bom.FirstLevelNodes.ElementAt(i).Part.BOMNodeType.Equals("KP"))
                    {
                        bool bChecked = false;
                        IBOMNode bomNode = bom.FirstLevelNodes.ElementAt(i);
                        bChecked = CheckCondition(bomNode);
                        if (bChecked)
                        {
                            IPart part = bomNode.Part;
                            bool createNew = true;
                            /*if (!string.IsNullOrEmpty(bomNode.AlternativeItemGroup))
                            {
                                foreach (IFlatBOMItem item in flatBom.BomItems)
                                {
                                    if (string.Compare(
                                            item.AlternativeItemGroup,
                                            bomNode.AlternativeItemGroup) == 0
                                        )
                                    {
                                        item.AddAlterPart(part);
                                        createNew = false;
                                        break;
                                    }
                                }
                            }*/
                            if (createNew)
                            {
                                //create new bomitem
                                var flatBOMItem = new FlatBOMItem(1, part_check_type, new List<IPart>());
                                flatBOMItem.AlterParts.Add(part);
                                flatBOMItem.AlternativeItemGroup = bomNode.AlternativeItemGroup;
                                flatBom.AddBomItem(flatBOMItem);
                            }
                        }
                    }
                }
            }
            if (flatBom.BomItems.Count > 0)
            {
                foreach (IFlatBOMItem item in flatBom.BomItems)
                {
                    item.Tp = item.AlterParts.First().BOMNodeType;
                    item.Descr = item.AlterParts.First().Descr;
                    string pnString = string.Empty;
                    foreach (IPart part in item.AlterParts)
                    {
                        if (pnString.Length == 0)
                        {
                            pnString = GetVenderCodeValue(part);
                        }
                        else
                        {
                            pnString += "," + GetVenderCodeValue(part);
                        }
                    }
                    item.PartNoItem = pnString;
                }
                return flatBom;
            }
            return null;
        }

        private string GetVenderCodeValue(IPart part)
        {
            IList<PartInfo> part_infos = part.Attributes;
            if (part_infos != null)
            {
                foreach (PartInfo part_info in part_infos)
                {
                    if (part_info.InfoType.Equals("VendorCode"))
                    {
                        return part_info.InfoValue;
                    }
                }
            }
            return "";
        }

        public bool CheckCondition(object node)
        {
            // 檢查 partType ; 要有 VendorCode
            if (((BOMNode)node).Part == null)
            {
                return false;
            }
            bool is_type = ((BOMNode)node).Part.Type.Equals(partType);
            bool is_VendorCode = false;
            IList<PartInfo> part_infos = ((BOMNode)node).Part.Attributes;
            foreach (PartInfo part_info in part_infos)
                if (part_info.InfoType.Trim().Equals("VendorCode") && !string.IsNullOrEmpty(part_info.InfoValue))
                {
                    is_VendorCode = true;
                    break;
                }
            if (is_type && is_VendorCode)
                return true;
            return false;
        }
    }
}
