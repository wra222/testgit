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
namespace IMES.CheckItemModule.CQ.CheckBattery.Filter
{
    [Export(typeof(ICheckModule))]
    [ExportMetadata("ProgramName", "IMES.CheckItemModule.CQ.CheckBattery.Filter.dll")]
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
                    if (product.ProId != ((PartUnit) part_unit).ProductId) //将会在PartUnit中增加ProId。
                    {
                        throw new FisException("CHK184", new string[] {});
                    }
                }
                if (!string.IsNullOrEmpty(partSn)
                    && partSn.Length == 14
                    && string.Compare(partSn, 0, "6", 0, 1) == 0)
                {
                    string hppn = partSn.Substring(0, 5);
                    IPartRepository partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    var btInfo = partRepository.FindBattery(hppn);
                    if (btInfo == null || string.IsNullOrEmpty(btInfo.hssn))
                    {
                        throw new FisException("CHK873", new[] { hppn });
                    }
                }
                if (IsCheckBateryStation(station) && !IsNoCheckBatteryFamily(part_unit))
                {
                    if (!CheckBatteryTestLog(partSn))
                    {
                        throw new FisException("No battery test log!!");
                    }
                }


            }
        }
        private bool IsNoCheckBatteryFamily(object part_unit)
        {
            PartUnit pn = ((PartUnit)part_unit);

            Session session =(Session) pn.CurrentSession;
            //Session session = SessionManager.GetInstance.GetSession(((PartUnit)part_unit).ProductId, Session.SessionType.Product);
            if (session == null)
            {
                throw new InstanceNotFoundException("Can not get Session instance from SessionManager!");
            }
            IProduct product =(IProduct) session.GetValue(Session.SessionKeys.Product);
            if (product == null)
            { 
                throw new FisException("No product object in session"); 
            }
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            return    partRep.GetConstValueTypeList("NoCheckBatteryFamily").Select(x => x.value).ToList().Contains(product.Family);
          
        }


        private bool IsCheckBateryStation(string station)
        {
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IList<string> tmpList = partRep.GetValueFromSysSettingByName("PAKCheckBatteryStation");
            if (tmpList.Count > 0)
            {
                return string.Compare(tmpList[0].Trim(), station, true) == 0;
            }
            return false;
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
    }
}
