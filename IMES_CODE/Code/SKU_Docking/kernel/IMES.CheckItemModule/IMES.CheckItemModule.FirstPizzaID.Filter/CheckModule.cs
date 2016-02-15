using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.ComponentModel.Composition;

namespace IMES.CheckItemModule.FirstPizzaID.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.FirstPizzaID.Filter.dll")]
    class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            //1.(IMES_PAK..PizzaStatus.Station <> 'OK' 和'MP'，否则报告错误：“Pizza 状态不正确！”
            //2.不能被其他Product占用（Product.PizzaId）；
            //3.必须与当前Product.PizzaId的值一致；            
            if (partUnit != null)
            {
                string pizza_id = ((PartUnit) partUnit).Sn;
                if (!string.IsNullOrEmpty(pizza_id))
                {
                    IPizzaRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    PizzaStatus pizza_status = pizza_repository.GetPizzaStatus(pizza_id);
                    if (pizza_status != null)
                    {
                        if (!pizza_status.StationID.Equals("OK") && !pizza_status.StationID.Equals("MP"))
                        {
                            throw new FisException("CHK173", new string[] {});
                        }
                    }
                    else
                    {
                        throw new FisException("CHK173", new string[] { });
                    }

                    IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    IProduct prod = product_repository.Find(((PartUnit) partUnit).ProductId);
                    if (prod != null && string.Compare(prod.PizzaID, pizza_id) == 0)
                    {
                    }
                    else
                    {
                        //pizza未与当前product绑定
                        throw new FisException("CHK869",new string[]{});
                    }

                    IProduct bindingProduct = product_repository.GetProductByPizzaID(pizza_id);
                    if (bindingProduct.ProId != ((PartUnit) partUnit).ProductId)
                    {
                        //pizza已同其他product绑定
                        throw new FisException("CHK184",new string[]{});
                    }
                }
                else
                {
                    throw new FisException("CHK172",new string[]{});
                }
            }
            else
            {
                throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.FirstPizzaID.Filter.CheckModule.Check" });
            }
        }
    }
}
