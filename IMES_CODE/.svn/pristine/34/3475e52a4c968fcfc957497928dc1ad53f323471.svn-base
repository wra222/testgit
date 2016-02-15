using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.AIO.CRLCM.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.AIO.CRLCM.Filter.dll")]
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

            PartUnit pu = (PartUnit)part_unit;
            Session session = pu.CurrentSession as Session;
            if (session == null)
            {
                throw new Exception("Can not get Session instance from SessionManager!");
            }

            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            Product product = session.GetValue(Session.SessionKeys.Product) as Product;
            if (product == null)
            {
                throw new Exception("Can not get Product from Session!");
            }
            product.CUSTSN = pu.Sn;
            productRepository.Update(product, session.UnitOfWork);
            
        }
    }
}
