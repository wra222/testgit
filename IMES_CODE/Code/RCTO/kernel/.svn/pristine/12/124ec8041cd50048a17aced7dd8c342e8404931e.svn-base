using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;


namespace IMES.CheckItemModule.CQ.HDDFrame.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.HDDFrame.Filter.dll")]
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

            //1）10码或14码或15码，前5位等于VC

            //3）若Len=17,前2码为'SN'，则截取后15码，作为Match与保存的数据
            if (subject.Length == 14 || subject.Length == 10 || subject.Length == 15 || subject.Length == 17 || subject.Length == 18)
            {
                if (subject.Length == 17)
                {
                    if (subject.Substring(0, 2).CompareTo("SN") == 0)
                    {
                        subject = subject.Substring(2);
                    }
                    else
                    {
                        return null;
                    }
                }
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
                                    if (part_info.InfoValue.Trim().Equals(subject.Trim().Substring(0, 5)))
                                    {
                                        ret = new PartUnit(part.PN, subject.Trim(), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
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
            //2）如大于90码  从77码开始取14码，取得的14码的前5位等于VC，截取的Key为Match与Save的PartSn
            /*if (subject.Length > 90)
            {
                IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                if (parts != null)
                {
                    foreach (IPart part in parts)
                    {
                        ret = new PartUnit(part.PN, subject.Substring(76, 14), part.BOMNodeType, part.Type, string.Empty, part.CustPn, ((FlatBOMItem)bomItem).CheckItemType);
                        break;
                    }
                }
            }*/
            return ret;
        }
    }
}
