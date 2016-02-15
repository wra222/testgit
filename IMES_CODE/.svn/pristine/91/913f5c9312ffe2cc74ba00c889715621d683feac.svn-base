using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CQ.OfflineHDD.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.OfflineHDD.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
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

            string itemType = "HDD";
            var product = (Product)part_owner;
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }

            string sessionIsBackUpHDD = session.GetValue("IsBackUpHDD") as string;
            bool IsBackUpHDD = "Y".Equals(sessionIsBackUpHDD);

            if (!IsBackUpHDD)
            {
                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();

                //product_repository.BackUpProductPartByCheckItemTypeDefered(session.UnitOfWork, product.ProId, session.Editor, itemType);

                ProductPart ppEq = new ProductPart();
                ppEq.ProductID = product.ProId;
                ppEq.CheckItemType = itemType;

                ProductPart ppNEq = new ProductPart();

                product_repository.BackUpProductPartDefered(session.UnitOfWork, session.Editor, ppEq, ppNEq);
                product_repository.DeleteProductPartDefered(session.UnitOfWork, ppEq, ppNEq);

                session.AddValue("IsBackUpHDD", "Y");
            }

            //var product_part = new ProductPart();
            //product_part.BomNodeType = ((PartUnit)part_unit).Type;
            //product_part.Iecpn = ((PartUnit)part_unit).IECPn;
            //product_part.PartSn = ((PartUnit)part_unit).Sn;
            //product_part.PartType = ((PartUnit)part_unit).ValueType;
            //product_part.Station = station;
            //product_part.CustomerPn = ((PartUnit)part_unit).CustPn;
            //product_part.Editor = session.Editor;
            //product_part.ValueType = ((PartUnit)part_unit).ValueType;
            //product_part.ProductID = ((PartUnit)part_unit).ProductId;
            //product_part.PartID = ((PartUnit)part_unit).Pn;

            //product.AddPart(product_part);
            
            //product_repository.Update(product, session.UnitOfWork);

            //}
            //else
            //{
            //    throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.HDD.Filter.SaveModule.Save" });
            //}
        }
    }
}
