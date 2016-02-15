// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-05-21   200038                       Create
// Known issues:
//

using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.MAC.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.MAC.Filter.dll")]
    public class SaveModule: ISaveModule
    {
        /// <summary>
        ///  Update Product.MAC
        /// </summary>
        /// <param name="partUnit">part Unit</param>
        /// <param name="partOwner">part owner</param>
        /// <param name="station">station</param>
        /// <param name="key">session key</param>
        public void Save(object partUnit, object partOwner, string station, string key)
        {

            if (partUnit == null)
            {
                throw new ArgumentNullException();
            }
            if (partOwner == null)
            {
                throw new ArgumentNullException();
            }

            var product = (Product)partOwner;
            product.MAC = ((PartUnit)partUnit).Sn;
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            productRepository.Update(product, session.UnitOfWork);
        }
    }
}
