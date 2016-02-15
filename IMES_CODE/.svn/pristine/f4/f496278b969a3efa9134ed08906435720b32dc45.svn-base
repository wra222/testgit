using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.USBBoard.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.USBBoard.Filter.dll")]
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
            
            IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
			if (parts != null)
			{
				foreach (IPart part in parts)
				{
					if (part.PN.Trim().Equals(subject.Trim()))
					{
						ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type,string.Empty, part.CustPn,((FlatBOMItem) bomItem).CheckItemType);
						break;
					}
				}
			}
            
            return ret;
        }
    }
}
