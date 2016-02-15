// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-03-05   210003                       ITC-1360-1041
// 2012-03-05   210003                       ITC-1360-0455
// 2012-03-15   210003                       ITC-1360-1478
// Known issues:
using System;
using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.VGA.Filter
{
    [Export(typeof(ISaveModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.VGA.Filter.dll")]
    class SaveModule : ISaveModule
    {
        public void Save(object part_unit, object part_owner, string station, string key)
        {
            //Insert ProductInfo  InfoType='VGA'
            if (part_unit == null)
            {
                throw new ArgumentNullException();
            }
            if (part_owner == null)
            {
                throw new ArgumentNullException();
            }

            var product = (Product)part_owner;
            Session session = SessionManager.GetInstance.GetSession(product.ProId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            //var product_info = new ProductInfo();
            //product_info.InfoType = "VGA";
            //product.ProductInfoes.Add(product_info);
            product.SetExtendedProperty("VGA", ((PartUnit)part_unit).Sn.Trim(), session.Editor);
            IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            product_repository.Update(product, session.UnitOfWork);


            var mb_repository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IMB mb = mb_repository.Find(((PartUnit)part_unit).Sn.Trim());
            if (mb != null)
            {
                //PCA..PCBStatus.Station = 32
                mb.MBStatus.Station = "32";
                mb.MBStatus.Status = MBStatusEnum.Pass;
                //记录MB Log  Insert PCA..PCBLog   WC=３２
                MBLog mb_log = new MBLog(0, mb.Sn, mb.Model, "32", (int) MBStatusEnum.Pass, session.Line, session.Editor,new DateTime());
                mb.AddLog(mb_log);
                mb_repository.Update(mb, session.UnitOfWork);
                //IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                product_repository.Update(product, session.UnitOfWork);
//                if (product.IsCDSI)
//                {
//                    SpecialDetInfo item = new SpecialDetInfo{snoId = product.ProId,tp = "CDSI",sno1 = product.MAC,cdt = new DateTime(),id = 0,udt = new DateTime()};
//                    product_repository.InsertSpecialDet(item);
//                }
            }
        }
    }
}
