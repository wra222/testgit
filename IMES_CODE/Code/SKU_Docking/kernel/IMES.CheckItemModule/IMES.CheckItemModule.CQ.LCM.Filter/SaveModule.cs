using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CQ.LCM.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.LCM.Filter.dll")]
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

            Session session = SessionManager.GetInstance.GetSession(key, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            PartUnit pu = (PartUnit)part_unit;

            if (null != session.GetValue("IsCleanRoomModel") && ((bool)session.GetValue("IsCleanRoomModel")))
            {
                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct product = productRepository.GetProductByIdOrSn(pu.Sn);
                if (product == null)
                {
                    throw new Exception("IsCleanRoomModel, Can not get Product instance by Part.Sn when Save !");
                }
                string prdID = product.ProId;
                
                IList<IMES.DataModel.TbProductStatus> stationList = productRepository.GetProductStatus(new List<string> { prdID });
                productRepository.UpdateProductPreStationDefered(session.UnitOfWork, stationList);

                //***** Update Product Status
                var newStatus = new IMES.FisObject.FA.Product.ProductStatus();
                newStatus.Udt = DateTime.Now;
                newStatus.StationId = "CR32";
                newStatus.Status = IMES.FisObject.Common.Station.StationStatus.Pass;
                newStatus.Line = product.Status.Line;
                newStatus.Editor = session.Editor;
                newStatus.TestFailCount = 0;
                newStatus.ReworkCode = "";
                product.UpdateStatus(newStatus);

                //***** Insert ProductLog
                var productLog = new ProductLog
                {
                    Model = product.Model,
                    Status = IMES.FisObject.Common.Station.StationStatus.Pass,
                    Editor = session.Editor,
                    Line = product.Status.Line,
                    Station = "CR32",
                    Cdt = DateTime.Now
                };
                product.AddLog(productLog);

                productRepository.Update(product, session.UnitOfWork);
            }

        }
    }
}
