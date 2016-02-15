using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CQ.Camera.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.Camera.Filter.dll")]
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
            //前5位等于VC
          //  if (subject.Length >= 5) 
            string vendorCode = "";
            string ct = "";
            if (subject.Length >= 91 || subject.Length==14) //FOR  [ICC IMES TEST 0000317]: Combine Keyparts CT 刷入数量
            {
                //Calc CT
                if (subject.Length == 14)
                { vendorCode = subject.Trim().Substring(0, 5); }
                else
                { vendorCode = subject.Trim().Substring(76, 5); }
                //Calc CT
                
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
                                if (part_info.InfoType.Equals("VendorCode"))
                                  {
                                   // if (part_info.InfoValue.Trim().Equals(subject.Trim().Substring(0, 5)))
                                      if (part_info.InfoValue.Trim().Equals(vendorCode))
                                    
                                    {
                                          //FOR  [ICC IMES TEST 0000317]: Combine Keyparts CT 刷入数量
                                        if (subject.Length == 14)
                                        { ct = subject.Trim(); }
                                        else
                                        { ct = subject.Trim().Substring(76, 14); }
                                        ret = new PartUnit(part.PN, ct, part.BOMNodeType, part.Type,
                                                           string.Empty, part.CustPn,
                                                           ((FlatBOMItem) bomItem).CheckItemType);
                                        break;
                                    }
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
            return ret;
        }
    }
}
