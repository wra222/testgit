using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.OOA.Filter
{
    [Export(typeof(IMatchModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.OOA.Filter.dll")]
    class MatchModule : IMatchModule
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
            if (subject.Length == 14)
            {
                IList<IPart> flat_bom_items = ((IFlatBOMItem) bomItem).AlterParts;
                ICOAStatusRepository coa_repository =
                    RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                COAStatus coa_status = coa_repository.GetCoaStatus(subject.Trim());
                //是Office COA
                if (coa_status != null)
                {
                    if (!(coa_status.Status.Equals("P1") || coa_status.Status.Equals("A2") || coa_status.Status.Equals("A3")))
                    {
                        throw new FisException("CHK170", new string[] {});
                    }
                    Boolean is_chk171 = false;
                    foreach (IPart flat_bom_item in flat_bom_items)
                    {
                        if (flat_bom_item.BOMNodeType.Equals("P1"))
                        {
                            IList<PartInfo> part_infos = flat_bom_item.Attributes;
                            Boolean is_ooa = false;
                            if (part_infos != null)
                            {
                                foreach (PartInfo part_info in part_infos)
                                {
                                    if (part_info.InfoType.Trim().Equals("DESC"))
                                    {
                                        if (part_info.InfoValue.Trim().Equals("OOA"))
                                        {
                                            is_ooa = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (is_ooa)
                            {
                                //如果有应该只有1条。
                                //                            if (!(coa_status.Status.Equals("P1") || coa_status.Status.Equals("A2") || coa_status.Status.Equals("A3")))
                                //                            {
                                //                                throw new FisException("CHK170", new string[] { });
                                //                            }
                                if (coa_status.IECPN.Equals(flat_bom_item.PN))
                                {
                                    is_chk171 = true;
                                    //                                throw new FisException("CHK171", new string[] { });
                                }
                                var last_or_default = flat_bom_items.LastOrDefault();
                                if (last_or_default != null && last_or_default.Equals(flat_bom_item))
                                {
                                    if (is_chk171)
                                    {
                                        ret = new PartUnit(flat_bom_item.PN, subject.Trim(), flat_bom_item.BOMNodeType,flat_bom_item.Type, coa_status.IECPN, flat_bom_item.CustPn,((IFlatBOMItem) bomItem).CheckItemType);
                                        break;
                                    }
                                    //                                else
                                    //                                {
                                    //                                    throw new FisException("CHK171", new string[] { });
                                    //                                }
                                }
                            }
                        }


                    }
                }
                else //否则用户输入的是Vendor CT 或者Part No
                {
                    foreach (IPart flat_bom_item in flat_bom_items)
                    {
                        IList<PartInfo> part_infos = flat_bom_item.Attributes;
                        if (part_infos != null)
                        {
                            foreach (PartInfo part_info in part_infos)
                            {
                                if (part_info.InfoType.Equals("VendorCode"))
                                {
                                    //对于Vendor CT，则遍历Part List 找到存在Vendor Code 属性的记录.
                                    if (part_info.InfoValue.Trim().Equals(subject.Trim().Substring(0, 5)))
                                    {
                                        ret = new PartUnit(flat_bom_item.PN, string.Empty, flat_bom_item.BOMNodeType,flat_bom_item.Type, coa_status.IECPN, flat_bom_item.CustPn,((IFlatBOMItem) bomItem).CheckItemType);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
}
