using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;

namespace IMES.CheckItemModule.DockingBaseSN.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.DockingBaseSN.Filter.dll")]
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
            var product = (Product)part_owner;
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            string sn = ((PartUnit)part_unit).Sn.Trim();

            IProductRepository prodRep =
                     RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            prodRep.ExecSpForNonQueryDefered(session.UnitOfWork,
                                                                      SqlHelper.ConnectionString_PAK,
                                                                      "CombineBaseUpdateBaseSn",
                                                                      new[] { new SqlParameter("SN", sn), 
                                                                                 new SqlParameter("Line", product.Status.Line),
                                                                                  new SqlParameter("Editor", product.Status.Editor)});

        }
    }
}
