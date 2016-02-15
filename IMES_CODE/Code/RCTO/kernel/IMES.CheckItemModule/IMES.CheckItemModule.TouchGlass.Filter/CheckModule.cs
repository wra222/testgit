using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Management.Instrumentation;
using System.Configuration;
//using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using IMES.DataModel;
using System;
using IMES.FisObject.Common.Material;
namespace IMES.CheckItemModule.TouchGlass.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TouchGlass.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object part_unit, object bom_item, string station)
        {
            if (part_unit != null)
            {
                //没有结合其它Product
                string partSn = ((PartUnit)part_unit).Sn.Trim();
                if (!string.IsNullOrEmpty(partSn))
                {
                   CheckTouckGlassTime(part_unit);
                }

                //IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                //IProduct product = product_repository.GetProductByIdOrSn(partSn);
                //if (product != null)
                //{
                //    if (product.ProId != ((PartUnit)part_unit).ProductId) //将会在PartUnit中增加ProId。
                //    {
                //        throw new FisException("CHK184", new string[] { });
                //    }
                //}
                
                //IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();

                //string ct5 = partSn.Substring(0, 5);
                //string sn5 = ((PartUnit)part_unit).Pn;

                //AssemblyVCInfo ai = new AssemblyVCInfo();
                //ai.combineVC = ct5;
                //ai.vc = sn5;
                //IList<AssemblyVCInfo> lstAI = partRepository.GetAssemblyVC(ai);
                //if (lstAI == null || lstAI.Count == 0)
                //{
                //    throw new FisException("CQCHK0004", new string[] { });
                //}

            }

        }

        private void CheckTouckGlassTime(object part_unit)
        {
            PartUnit pn = ((PartUnit)part_unit);
            string partSn = pn.Sn.Trim();
            Session session = (Session)pn.CurrentSession;
            //Session session = SessionManager.GetInstance.GetSession(((PartUnit)part_unit).ProductId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            IProduct product = (IProduct)session.GetValue(Session.SessionKeys.Product);
            if (product == null)
            {
                throw new FisException("No product object in session");
            }
            int TouckGlassTime = 0;
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string>  listtouchbymodel= partRep.GetConstValueListByType("TouchGlassCheckTime").Where(x => x.name == product.Model).Select(y => y.value).ToList();
            if (listtouchbymodel != null && listtouchbymodel.Count > 0)
            {
                TouckGlassTime = Convert.ToInt32(listtouchbymodel[0]);
            }
            else
            {
                 listtouchbymodel = partRep.GetConstValueListByType("TouchGlassCheckTime").Where(x => x.name == product.Family).Select(y => y.value).ToList();
                 if (listtouchbymodel != null && listtouchbymodel.Count > 0)
                 {
                     TouckGlassTime = Convert.ToInt32(listtouchbymodel[0]);
                 }

            }
            if (TouckGlassTime > 0)//need check touchglass time
            {
                var materialRep = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository>();
                Material material = materialRep.Find(partSn);
                if (material == null)
                {
                    throw new FisException("CQCHK0011", new string[] { partSn });
                }
                DateTime materialctcdt = material.Cdt;
                DateTime nowtime = DateTime.Now;
                if ((nowtime - materialctcdt).TotalMinutes < TouckGlassTime)
                {
                    throw new FisException("CQCHK50120", new string[] { (nowtime - materialctcdt).TotalMinutes.ToString(), TouckGlassTime.ToString()});
                }
              
            }

            

        }
    }
}
