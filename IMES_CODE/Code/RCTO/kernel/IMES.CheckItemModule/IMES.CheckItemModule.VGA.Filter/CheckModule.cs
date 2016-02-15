// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-19   210003                       Create
// 2012-02-28   210003                       ITC-1360-0460
// 2012-03-06   210003                       ITC-1360-1109
// Known issues:

using System.ComponentModel.Composition;
using System.Management.Instrumentation;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Process;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;

namespace IMES.CheckItemModule.VGA.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.VGA.Filter.dll")]
    class CheckModule : ICheckModule
    {
        public void Check(object partUnit, object bomItem, string station)
        {
            //            7.PCA卡站(WC=32)并且Part Check
            //Part Check参见[CI-MES12-SPEC-Common-IMES_HP_PartCheck.xlsx]
            //异常情况：
            //A.	若MB没有找到纪录，则提示” 該主板半制流程尚未完成”
            //B.	若MB已与ProdID绑定，则提示” 該主板已經結合機器！”

            //其他part检查异常不在此描述
            if (partUnit != null)
            {
                Session session = SessionManager.GetInstance.GetSession(((PartUnit)partUnit).ProductId, Session.SessionType.Product);
                if (session == null)
                {
                    throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
                }
                IProcessRepository current_process_repository =
                    RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
                //current_process_repository.SFC(session.Line, session.Customer, station, ((PartUnit)part_unit).Sn, "MB");
                current_process_repository.SFC(session.Line, session.Customer, "32", ((PartUnit)partUnit).Sn, "MB");
                string sn = ((PartUnit)partUnit).Sn;
                if (!string.IsNullOrEmpty(sn))
                {
                    var mb_repository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
                    IMB mb = mb_repository.Find(sn);
                    if (mb == null)
                    {
                        throw new FisException("CHK862", new string[] { });
                    }
                }
                else
                {
                    throw new FisException("CQCHK0019", new string[] {"VGA","" });
                }
                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                var product = (Product)product_repository.GetProductByMBSn(sn);
                if (product != null)
                {
                    throw new FisException("CHK863", new string[] { });
                }

            }
            else
            {
                throw new FisException("CHK174", new string[] { "IMES.CheckItemModule.VGA.Filter.CheckModule.Check" });

            }
        }
    }
}
