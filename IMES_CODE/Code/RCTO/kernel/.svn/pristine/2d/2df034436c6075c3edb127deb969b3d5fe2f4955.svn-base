using System;
using System.Collections.Generic;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.ATSN1.Filter
{
    class MatchModule : IMatchModule
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
            //刷入的AST=Product_Part.PartSN
            //在Product_Part表中找到此Part和Product结合的PartSN（Product_Part.PartID=PartNo and Product_Part.ProductID=ProductID# and Product_Part.BomNodeType='AT'）。后半句不属于Filter．

            if (subject.Length == 14)
            {
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                foreach (IPart part in parts)
                {
                    if (part.PN.Trim().Equals(subject.Trim()))
                    {
                        ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                    }
                }
            }
            return ret;
        }
    }
}
