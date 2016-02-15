using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using System;
using IMES.FisObject.Common.FisBOM;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common;
namespace IMES.CheckItemModule.CRLCM.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CRLCM.Filter.dll")]
    public class SaveModule : ISaveModule
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
            Session session = (Session)pu.CurrentSession;
            string prdID = pu.Sn;
            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<IMES.DataModel.TbProductStatus> stationList = productRepository.GetProductStatus(new List<string> {prdID});
            productRepository.UpdateProductPreStationDefered(session.UnitOfWork, stationList);

            //***** Update Product Status
            IProduct product= productRepository.Find(prdID);
            if (product == null)
            { return; }
            var newStatus = new IMES.FisObject.FA.Product.ProductStatus();
            newStatus.Udt= DateTime.Now;
            newStatus.StationId = "CR32";
            newStatus.Status = IMES.FisObject.Common.Station.StationStatus.Pass;
            newStatus.Line = session.Line;
            newStatus.Editor = session.Editor;
            newStatus.TestFailCount = 0;
            newStatus.ReworkCode = "";
            product.UpdateStatus(newStatus);
                //  productRepository.Update(product, session.UnitOfWork);
            //***** Insert ProductLog
            var productLog = new ProductLog
            {
                Model = product.Model,
                Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                Editor = session.Editor,
                Line = session.Line,
                Station = "CR32",
                Cdt = DateTime.Now
            };

            product.AddLog(productLog);
            productRepository.Update(product, session.UnitOfWork);

        }
    }
}
