using System.ComponentModel.Composition;
using IMES.CheckItemModule.Interface;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Management.Instrumentation;
using System.Configuration;
using IMES.Infrastructure.Repository._Schema;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using IMES.DataModel;
using System.Text.RegularExpressions;

namespace IMES.CheckItemModule.TPCB.Filter
{

    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.TPCB.Filter.dll")]
    public class CheckModule : ICheckModule
    {
        public void Check(object part_unit, object bom_item, string station)
        {
            if (part_unit != null)
            {
                //没有结合其它Product
                string partSn = ((PartUnit)part_unit).Sn.Trim();
                IProductRepository product_repository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
                IProduct product = product_repository.GetProductByIdOrSn(partSn);
                if (product != null)
                {
                    if (product.ProId != ((PartUnit)part_unit).ProductId) //将会在PartUnit中增加ProId。
                    {
                        throw new FisException("CHK184", new string[] { });
                    }
                }
                if (!string.IsNullOrEmpty(partSn))
                {

                    if (IsCheckBateryStation(station) && IscheckTPCBModel(part_unit) && IscheckTPCBPN(part_unit))
                    {
                        if (!CheckBatteryTestLog(partSn))
                        {
                            throw new FisException("No TPCB test log!!");
                        }
                    }
                }


            }
        }
        private bool IsNoCheckBatteryFamily(object part_unit)
        {
            PartUnit pn = ((PartUnit)part_unit);

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
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            return partRep.GetConstValueTypeList("NoCheckTPCBFamily").Select(x => x.value).ToList().Contains(product.Family);

        }

        private bool IsCheckBateryStation(string station)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            return  partRep.GetConstValueTypeList("FACheckTPCBStation").Select(x => x.value).ToList().Contains(station);
        }

        private bool CheckBatteryTestLog(string batterySn)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BatteryDBServer"].ToString();
            string strSQL = @"IF EXISTS(SELECT 1 from Batt_Temp where CT=@CT)
                                                         SELECT 1
                                                        ELSE
                                                         SELECT 0 ";
            SqlParameter sqlPara = new SqlParameter("@CT", SqlDbType.VarChar, 32);
            sqlPara.Direction = ParameterDirection.Input;
            sqlPara.Value = batterySn;
            object data = SqlHelper.ExecuteScalar(connectionString, System.Data.CommandType.Text, strSQL, sqlPara);
            return ((int)data == 1);
        }

        private bool IscheckTPCBPN(object part_unit)
        {
            bool needcheckpn = false;
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> pnreg = partRep.GetConstValueTypeList("CheckTPCBPNReg").Select(x => x.value).ToList();
            if (pnreg != null && pnreg.Count > 0)
            {
                Regex Regex1 = new Regex(pnreg[0]);
               needcheckpn= Regex1.IsMatch(((PartUnit)part_unit).Pn.Trim());
            }
            return needcheckpn;

        }

        private bool IscheckTPCBModel(object part_unit)
        {
            PartUnit pn = ((PartUnit)part_unit);
            bool needcheckpn = false;
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
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> pnreg = partRep.GetConstValueTypeList("CheckTPCBTestLogModelReg").Select(x => x.value).ToList();
            if (pnreg != null && pnreg.Count > 0)
            {
                foreach (string modelreg in pnreg)
                {
                    Regex Regex1 = new Regex(modelreg);
                    if (Regex1.IsMatch(product.Model))
                    {
                        needcheckpn = true;
                        break;
                    }
                }
            }
            return needcheckpn;
        }
    }
}
