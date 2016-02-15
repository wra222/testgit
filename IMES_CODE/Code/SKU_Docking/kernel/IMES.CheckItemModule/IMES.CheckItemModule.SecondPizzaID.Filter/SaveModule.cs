// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.SecondPizzaID.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.SecondPizzaID.Filter.dll")]
    public class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            // IMES_FA..ProductInfo.InfoValue (InfoType= ‘KIT2’)
            //还需要更新IMES_PAK..PizzaStatus (Station – DDD Kitting 6 站号后两位)
            if (part_unit == null)
            {
                throw new ArgumentNullException();
            }
            if (part_owner == null)
            {
                throw new ArgumentNullException();
            }
            //if (part_unit != null)
            //{
//                var product = (Product)part_owner;
//                //if (product != null)
//                //{
//                    Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
//                    if (session == null)
//                    {
//                        throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
//                    }
                    Session session = SessionManager.GetInstance.GetSession(key, Session.SessionType.Product);
                    if (session == null)
                    {
                        throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
                    }
                    Product product = (Product)session.GetValue(Session.SessionKeys.Product);
                    string pizza_id = ((PartUnit) part_unit).Sn.Trim();
                    if (pizza_id.Length == 10)
                    {
                        pizza_id = pizza_id.Substring(0, 9);
                    }
                    IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                    product.SetExtendedProperty("KIT2", pizza_id, session.Editor);
                    product_repository.Update(product, session.UnitOfWork);
                    IPizzaRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
                    PizzaStatus pizza_status = pizza_repository.GetPizzaStatus(pizza_id);
                    if (pizza_status != null)
                    {
                        pizza_status.StationID = station.Substring(station.Length - 2, 2);
                        Pizza pizza = product.PizzaObj;
                        if (pizza != null)
                        {
                            pizza.UpdatePizzaStatus(pizza_status);
                        }
                    }
                //}
            //}
            //else
            //{
            //    throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.SecondPizzaID.Filter.SaveModule.Save" });
            //}
        }
    }
}
