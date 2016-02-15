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
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.CheckItemModule.Utility;

namespace IMES.CheckItemModule.DockingMB.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingMB.Filter.dll")]
    public class MatchModule: IMatchModule
    {
        /// <summary>
        /// 10/11码，第5码为’M’，刷入数据的前2码为MBCode
        /// </summary>
        /// <param name="subject">value to be matched</param>
        /// <param name="bomItem">bomItem</param>
        /// <param name="station">station</param>
        /// <returns></returns>
        public object Match(string subject, object bomItem, string station)
        {
            PartUnit ret = null;
            if (subject == null)
            {
                throw new ArgumentNullException();
            }
            if (bomItem == null)
            {
                throw new ArgumentNullException();
            }

            //10/11码，第5码为’M’，刷入数据的前2码为MBCode;
            //支持11码不带校验位的MB, 第6码为’M’，刷入数据的前3码为MBCode            
            if (Is10CharSn(subject) || Is11CharSn(subject))
            {
                
            }
            else
            {
                return null;
            }


            //刷入数据的前2/3码为MBCode
            IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
            if (parts != null)
            {
                IList<KPVendorCode> kpVCList = (IList<KPVendorCode>)((IFlatBOMItem)bomItem).Tag;
                if (kpVCList == null)
                {
                    return ret;
                }
                foreach (IPart part in parts)
                {
                    if (part.BOMNodeType.Trim().Equals("MB"))
                    {
                        //string mbCode = part.GetAttribute("MB");
                        string mbCode = kpVCList.Where(x => x.PartNo == part.PN)
                                                                .Select(x => x.VendorCode).FirstOrDefault();
                        if (MatchMBCode(subject, mbCode))
                        {
                            ret = new PartUnit(part.PN, subject.Trim(), 
                                                part.BOMNodeType, part.Type,
                                                string.Empty, part.CustPn,
                                                ((FlatBOMItem)bomItem).CheckItemType);
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        private bool Is11CharSn(string subject)
        {
            if (subject.Length == 11 && (string.Compare(subject.Substring(5, 1), "M") == 0 || string.Compare(subject.Substring(5, 1), "B") == 0))
            {
                return true;
            }
            return false;
        }

        bool Is10CharSn(string subject)
        {
            if ((subject.Length == 10 || subject.Length == 11) && (string.Compare(subject.Substring(4, 1), "M") == 0 ||string.Compare(subject.Substring(4, 1), "B") == 0))
            {
                return true;
            }
            return false;
        }

        bool MatchMBCode(string subject, string mbCode)
        {
            if (!string.IsNullOrEmpty(mbCode))
            {
                string[] mbCodes = mbCode.Split(',');
                if (mbCodes.Length > 0)
                {
                    for (int i = 0; i < mbCodes.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(mbCodes[i])
                            && string.Compare(mbCodes[i], 0, subject, 0, mbCodes[i].Length) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
