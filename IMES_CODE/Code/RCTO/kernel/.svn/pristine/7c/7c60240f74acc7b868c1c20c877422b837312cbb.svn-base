using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Material;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.CPU.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CPU.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            //return;
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

            string snCpu = ((PartUnit)part_unit).Sn;

            string CheckMaterialStatus = session.GetValue("CheckMaterialCpuStatus") as string;
            if ("Y".Equals(CheckMaterialStatus))
            {
                IMaterialRepository MaterialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
                Material mat = session.GetValue("MaterialCpu") as Material;

                MaterialLog mlog = new MaterialLog();
                mlog.Action = "Combine Key Parts";
                mlog.Cdt = DateTime.Now;
                mlog.Comment = "";
                mlog.Editor = session.Editor;
                mlog.Line = session.Line;
                mlog.MaterialCT = snCpu;
                mlog.PreStatus = mat.Status;
                mlog.Stage = "FA";
                mlog.Status = "Assembly";

                mat.AddMaterialLog(mlog);

                mat.PreStatus = mat.Status;
                mat.Status = "Assembly";
                mat.Udt = DateTime.Now;
                MaterialRepository.Update(mat, session.UnitOfWork);

            }

            session.AddValue("OldCVSN", product.CVSN);
            product.CVSN = snCpu;
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

            IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            product_repository.Update(product, session.UnitOfWork);

            ////}
            ////else
            ////{
            ////    throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.CPU.Filter.SaveModule.Save" });
            ////}
        }
    }
}
