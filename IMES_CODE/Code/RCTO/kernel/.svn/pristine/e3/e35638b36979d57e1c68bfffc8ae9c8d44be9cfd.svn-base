using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.COA;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.OOA.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.OOA.Filter.dll")]
    class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            // 如果Office COA 的状态(IMES_PAK..COAStatus.Status)不是'P1' / 'A2' / 'A3' ，则报告错误：“The Office COA cannot be used!”
            //如果Office COA 的IECPN 与该记录的Part No 不同，则报告错误：“Office COA part number is not correct!”
            if (partUnit != null)
            {
                string sn = ((PartUnit)partUnit).Sn;
                if (!string.IsNullOrEmpty(sn))
                {
                    ICOAStatusRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<ICOAStatusRepository, COAStatus>();
                    COAStatus coa_status = pizza_repository.GetCoaStatus(sn);
                    if (!(coa_status.Status.Equals("P1") || coa_status.Status.Equals("A2") || coa_status.Status.Equals("A3")))
                    {
                        throw new FisException("CHK180", new string[] { sn });
                    }
                    bool is_royalty = false;
                    IList<IPart> parts = ((FlatBOMItem)bomItem).AlterParts;
                    foreach (IPart part in parts)
                    {
                        if (coa_status.IECPN.Equals(part.PN))
                        {
                            is_royalty = true;
                        }
                    }
                    if (!is_royalty)
                    {
                        throw new FisException("CHK181", new string[] { coa_status.IECPN });
                    }
                }
                else
                {
                    throw new FisException("CQCHK0019", new string[] {"OOA","" });
                }
            }
            else
            {
                throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.OOA.Filter.CheckModule.Check" });

            }
        }
    }
}
