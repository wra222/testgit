// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-06-19   200038                       Create
// Known issues:
//

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.VirtualTPCB.Filter
{
    [Export(typeof(IFilterModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.VirtualTPCB.Filter.dll")]
    public class Filter : IFilterModule
    {
        private const string PartCheckType = "VirtualTPCB";

        public object FilterBOM(object hierarchicalBOM, string station, object mainObject)
        {
            IList<IFlatBOMItem> bomItems = new List<IFlatBOMItem>();
            if (hierarchicalBOM == null)
            {
                throw new ArgumentNullException();
            }

            var bom = (HierarchicalBOM)hierarchicalBOM;
            var plNodes = bom.GetFirstLevelNodesByNodeType("PL");
            if (plNodes == null)
            {
                return null;
            }
            foreach (IBOMNode bomNode in plNodes)
            {
                if (IsVirtualTPCB(bomNode))
                {
                    IPart part = bomNode.Part;
                    IFlatBOMItem bomItem = new FlatBOMItem(bomNode.Qty, PartCheckType, new List<IPart> {part});
                    bomItem.PartNoItem = part.GetAttribute("VendorCode");
                    bomItem.Descr = part.GetAttribute("Descr");
                    bomItems.Add(bomItem);
                }
            }
            if (bomItems.Count > 0)
            {
                return new FlatBOM(bomItems);
            }

            return null;
        }

        /// <summary>
        ///bom节点Part的Descr描述为Descr为( 'JGS'）
        ///bom节点Part的PartInfo中的Descr描述为( 'TPCB'）
        ///bom节点Part的PartInfo中的Upper(VCDescr)包含('VIRTUAL')
        /// </summary>
        /// <param name="bomNode"></param>
        /// <returns></returns>
        private bool IsVirtualTPCB(IBOMNode bomNode)
        {
            if (bomNode == null)
            {
                return false;
            }
            IPart part = bomNode.Part;
            if (part == null)
            {
                return false;
            }
            if (string.Compare(part.Descr, "JGS") != 0)
            {
                return false;
            }
            string descrAttribute = part.GetAttribute("Descr");
            if (string.Compare(descrAttribute, "TPCB") != 0 && descrAttribute.IndexOf("KITK")!=0)
            {
                return false;
            }
            string vcdescrAttribute = part.GetAttribute("VCDescr");
            if (string.IsNullOrEmpty(vcdescrAttribute) 
                || !vcdescrAttribute.ToUpper().Contains("VIRTUAL"))
            {
                return false;
            }
            return true;
        }

    }
}
