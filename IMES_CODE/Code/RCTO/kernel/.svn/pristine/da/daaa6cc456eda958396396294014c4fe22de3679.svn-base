using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;

namespace IMES.CheckItemModule.MB.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.MB.Filter.dll")]
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

            if (Is10CharSn(subject) || Is11CharSn(subject))
            {

            }
            else
            {
                return null;
            }

            PartUnit ret = null;
            //MBCode match   MBSNO的前两码
            //if (subject != null)
            //{
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                if (parts != null)
                {
                    foreach (IPart part in parts)
                    {
                        if (part.BOMNodeType.Trim().Equals("MB"))
                        {
                            IList<PartInfo> part_infos = part.Attributes;
                            if (part_infos != null)
                            {
                                foreach (PartInfo part_info in part_infos)
                                {
                                    if (part_info.InfoType.Trim().Equals("MB"))
                                    {
                                        string mbCode = part_info.InfoValue.Trim();
                                        if (!string.IsNullOrEmpty(mbCode)
                                            && string.Compare(mbCode, 0, subject.Trim(), 0, mbCode.Length) == 0)
                                        {
                                            ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type,
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
            //}
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
            if ((subject.Length == 10 || subject.Length == 11) && (string.Compare(subject.Substring(4, 1), "M") == 0 || string.Compare(subject.Substring(4, 1), "B") == 0))
            {
                return true;
            }
            return false;
        }
    }
}
