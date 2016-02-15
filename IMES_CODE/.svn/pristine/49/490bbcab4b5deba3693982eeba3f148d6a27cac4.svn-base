// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-21   200038                       Create
// Known issues:
//

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.PXNCT.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.PXNCT.Filter.dll")]
    public class Filter: IFilterModule
    {
        private const string VendorCodeAttributeName = "VendorCode";
        private const string PXBomNodeType = "PX";
        private const string PartCheckType = "PXNCT";
        /// <summary>
        ///ModelBOM 中Model 直接下阶中BomNodeType = 'PX' 的Part; 
        /// PartInfo 表中不存在InfoType = 'VendorCode'的记录        
        /// </summary>
        /// <param name="hierarchicalBom"></param>
        /// <param name="station"></param>
        /// <param name="mainObject"></param>
        /// <returns></returns>
        public object FilterBOM(object hierarchicalBom, string station, object mainObject)
        {
            if (hierarchicalBom == null)
            {
                throw new ArgumentNullException();
            }

            var bom = (HierarchicalBOM)hierarchicalBom;
            IList<IBOMNode> pxNodes = bom.GetFirstLevelNodesByNodeType(PXBomNodeType);
            if (pxNodes == null)
            {
                return null;
            }

            IFlatBOM flatBom = new FlatBOM();
            foreach (IBOMNode bomNode in pxNodes)
            {
                if (!this.VendorCodeExists(bomNode))
                {
                    IPart part = bomNode.Part;
                    bool createNew = true;
                    if (!string.IsNullOrEmpty(bomNode.AlternativeItemGroup))
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
                    }
                    if (createNew)
                    {
                        //create new bomitem
                        var flatBOMItem = new FlatBOMItem(1, PartCheckType, new List<IPart>());
                        flatBOMItem.AlterParts.Add(part);
                        flatBOMItem.AlternativeItemGroup = bomNode.AlternativeItemGroup;
                        flatBom.AddBomItem(flatBOMItem);
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
                            pnString = part.PN;
                        }
                        else
                        {
                            pnString += "," + part.PN;
                        }
                    }
                    item.PartNoItem = pnString;
                }
                return flatBom;
            }

            return null;
        }


        public bool VendorCodeExists(IBOMNode node)
        {
            if (((BOMNode)node).Part != null)
            {
                string vendorCode = ((BOMNode)node).Part.GetAttribute(VendorCodeAttributeName);
                if (!string.IsNullOrEmpty(vendorCode))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
