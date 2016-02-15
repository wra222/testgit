using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using System.Linq;
using IMES.FisObject.Common.Process;
namespace IMES.CheckItemModule.CRLCM.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CRLCM.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object part_unit, object bom_item, string station)
        {
            IProcessRepository CurrentProcessRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
            if (part_unit != null)
            {
                PartUnit pu=(PartUnit)part_unit;
                var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct partProduct = null;
                partProduct = productRepository.Find(pu.Sn);
                if (partProduct != null)
                { 
                    Session session = (Session)pu.CurrentSession;
                    string firstLine = "";
                    if (!string.IsNullOrEmpty(session.Line))
                    {
                        firstLine = session.Line.Substring(0, 1);
                    }
                    IList<ModelProcess> currentModelProcess = CurrentProcessRepository.GetModelProcessByModelLine(partProduct.Model, firstLine);
                    if (currentModelProcess == null || currentModelProcess.Count == 0)
                    {
                        CurrentProcessRepository.CreateModelProcess(partProduct.Model, session.Editor, firstLine);
                    }
                    CurrentProcessRepository.SFC(session.Line, session.Customer, "CR32", pu.Sn, "Product");
                }
   
            }
        }
   }
}
