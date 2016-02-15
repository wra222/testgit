using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;

namespace IMES.CheckItemModule.CTNonBattery.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CTNonBattery.Filter.dll")]
    public class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
/*
            if (part_unit == null)
            {
                throw new ArgumentNullException();
            }
            if (part_owner == null)
            {
                throw new ArgumentNullException();
            }
            Session session = SessionManager.GetInstance.GetSession(key, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            Product product = (Product)session.GetValue(Session.SessionKeys.Product);
            Pizza pizza = product.PizzaObj;
            IPizzaRepository pizza_repository = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>();
            if (pizza != null)
            {
                IProductPart part = new ProductPart();
                part.PartID = ((PartUnit)part_unit).Pn;
                part.PartSn = ((PartUnit)part_unit).Sn;
                part.BomNodeType = ((PartUnit)part_unit).Type;
                part.CheckItemType = ((PartUnit)part_unit).ItemType;
                part.PartType = ((PartUnit)part_unit).ValueType;
                part.Station = station;
                part.Editor = session.Editor;
                pizza.AddPart(part);
                pizza_repository.Update(pizza, session.UnitOfWork);
            }*/
        }
    }
}
