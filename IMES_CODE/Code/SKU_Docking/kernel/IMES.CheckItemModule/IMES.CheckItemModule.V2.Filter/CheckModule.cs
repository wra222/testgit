using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.V2.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.V2.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            if (partUnit != null)
            {
                //1.没有与其他kit ID绑定
                string partSn = ((PartUnit)partUnit).Sn.Trim();
                IPizzaRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                int count = pizza_repository.GetPizzaPartsCout(partSn);
                if (count > 0)
                {
                    throw new FisException("CHK183", new string[] { partSn });
                }

                //2.Battery Part S/N 的前5位 = OlymBattery.HPPN 为条件查询OlymBattery 表，
                //如果没有记录，或者查询到的记录的HSSN 字段值为'', 则报告错误：
                //“请联系IE Maintain ”+ LEFT(@BatteryPartSN, 5) + ' 的HSTNN 号码!' - @BatteryPartSN 为Battery Part S/N（判断是否为Battery Condition: LEFT(PartSn,1) = ’6’ and LEN(PartSn)=14)）
                if (!string.IsNullOrEmpty(partSn)
                    && partSn.Length == 14
                    && string.Compare(partSn, 0, "6", 0, 1) == 0)
                {
                    string hppn = partSn.Substring(0, 5);
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    var btInfo = partRepository.FindBattery(hppn);
                    if (btInfo == null || string.IsNullOrEmpty(btInfo.hssn))
                    {
                        throw new FisException("CHK873", new[] { hppn });
                    }
                }

            }
            else
            {
                throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.V2.Filter.CheckModule.Check" });
            }
        }
    }
}
