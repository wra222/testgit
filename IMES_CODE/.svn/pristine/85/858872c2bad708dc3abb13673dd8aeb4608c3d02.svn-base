using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.CheckItemModule.Utility;

namespace IMES.CheckItemModule.CTNonBattery.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CTNonBattery.Filter.dll")]
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
            if (subject.Length >5)
            {
                IList<IPart> flat_bom_items = ((IFlatBOMItem) bomItem).AlterParts;
                IList<KPVendorCode> kpVCList = (IList<KPVendorCode>)((IFlatBOMItem)bomItem).Tag;
                if (kpVCList == null)
                {
                    return ret;
                }

                ICOAStatusRepository coa_repository =
                    RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                COAStatus coa_status = coa_repository.GetCoaStatus(subject.Trim());
                //长14
                //在IMES_PAK..COAStatus (COASN)表中不存在
                //前5位与Vendor Code 相同
                //是Office COA
                if (coa_status == null)
                {
                    foreach (IPart flat_bom_item in flat_bom_items)
                    {
                        if (kpVCList.Any(x => x.PartNo == flat_bom_item.PN &&
                                                       x.VendorCode.IndexOf(subject.Substring(0, 5)) == 0))
                        {
                            ret = new PartUnit(flat_bom_item.PN, subject.Trim(), flat_bom_item.BOMNodeType, flat_bom_item.Type, string.Empty, flat_bom_item.CustPn, ((IFlatBOMItem)bomItem).CheckItemType);
                            break;
                        }

                        //IList<PartInfo> part_infos = flat_bom_item.Attributes;
                        //foreach (PartInfo part_info in part_infos)
                        //{
                        //    if (part_info.InfoType.Equals("VendorCode"))
                        //    {
                        //        //对于Vendor CT，则遍历Part List 找到存在Vendor Code 属性的记录.
                        //        if (part_info.InfoValue.IndexOf(subject.Substring(0, 5)) == 0)
                        //        {
                        //            ret = new PartUnit(flat_bom_item.PN, subject.Trim(), flat_bom_item.BOMNodeType,flat_bom_item.Type, string.Empty, flat_bom_item.CustPn,((IFlatBOMItem) bomItem).CheckItemType);
                        //            break;
                        //        }
                        //    }
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
