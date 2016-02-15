using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pizza;
using System.Collections;
using System.Collections.Generic;
namespace IMES.CheckItemModule.CombineOfflinePizza.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CombineOfflinePizza.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object part_unit, object bom_item, string station)
        {
            PartUnit pu;
            pu = (PartUnit)part_unit;
            IPizzaRepository currentPizzaRepository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            if (!string.IsNullOrEmpty(pu.Sn) && pu.Sn.Length == 14 && HaveVendorCode(bom_item))
            {
                IList<PizzaPart> pizzaPartList = currentPizzaRepository.GetPizzaPartsByValue(pu.Sn);
                if (pizzaPartList.Count > 0)
                {
                    List<string> erpara = new List<string>();
                    erpara.Add("Part");
                    erpara.Add(pu.Sn);
                    erpara.Add(pizzaPartList[0].PizzaID);
                    var ex = new FisException("CHK134", erpara);
                    throw ex;

                }
            }
          
            return;
        }
        private bool HaveVendorCode(object bom_item)
        {
      
            IList<IPart> flat_bom_items = ((IFlatBOMItem)bom_item).AlterParts;
            if (flat_bom_items != null)
            {
                foreach (IPart flat_bom_item in flat_bom_items)
                {
                    IList<PartInfo> part_infos = flat_bom_item.Attributes;
                    foreach (PartInfo part_info in part_infos)
                    {
                        if (part_info.InfoType.Equals("VendorCode"))
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
