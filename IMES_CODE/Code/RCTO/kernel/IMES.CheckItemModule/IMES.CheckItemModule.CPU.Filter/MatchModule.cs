using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.CPU.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CPU.Filter.dll")]
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
			
			if (subject.Length >= 90)
			{
                // 取第77位(含)後的13位
                subject = subject.Substring(76, 13);
			}
			
            //前5位等于VC
            if (subject.Length == 13 || subject.Length == 12 || subject.Length == 14)
            {
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                if (parts != null)
                {
                    foreach (IPart part in parts)
                    {
                        //IList<PartInfo> part_infos = part.Attributes;
                        //if (part_infos != null)
                        //{
                            //foreach (PartInfo part_info in part_infos)
                            //{
                                //if (part_info.InfoType.Equals("VendorCode"))
                                //{
                                //    if (part_info.InfoValue.Trim().Equals(subject.Trim().Substring(0, 5)))
                                //    {
                                        ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type,
                                                           string.Empty, part.CustPn,
                                                           ((FlatBOMItem)bomItem).CheckItemType);
                                        break;
                                //    }
                                //}
                            //}
                        //}
                        //if (ret != null)
                        //{
                        //    break;
                        //}
                    }
                }
            }
            return ret;
        }
    }
}
