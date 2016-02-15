using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;

namespace IMES.CheckItemModule.LCMCable.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.LCMCable.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            return;
            //if (part_unit == null)
            //{
            //    throw new ArgumentNullException();
            //}
            //if (part_owner == null)
            //{
            //    throw new ArgumentNullException();
            //}
            ////if (part_unit != null)
            ////{
            //    var product = (Product)part_owner;
            //    Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            //    if (session == null)
            //    {
            //        throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            //    }
            //    var product_part = new ProductPart();
            //    product_part.BomNodeType = ((PartUnit)part_unit).Type;
            //    product_part.Iecpn = ((PartUnit)part_unit).IECPn;
            //    product_part.PartSn = ((PartUnit)part_unit).Sn;
            //    product_part.PartType = ((PartUnit)part_unit).ValueType;
            //    product_part.Station = station;
            //    product_part.CustomerPn = ((PartUnit)part_unit).CustPn;
            //    product_part.Editor = session.Editor;
            //    product_part.ValueType = ((PartUnit)part_unit).ValueType;
            //    product_part.ProductID = ((PartUnit)part_unit).ProductId;
            //    product_part.PartID = ((PartUnit)part_unit).Pn;

            //    product.AddPart(product_part);

            //    IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //    product_repository.Update(product, session.UnitOfWork);

            ////}
            ////else
            ////{
            ////    throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.BTCB.Filter.SaveModule.Save" });
            ////}
        }
    }
}
