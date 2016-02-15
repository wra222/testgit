using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure;
using IMES.DataModel;

namespace IMES.FisObject.Common.UPS
{
    public interface IUPSRepository : IRepository<UPSCombinePO>
    {
        bool IsUPSProductID(string productID);
        bool IsUPSModel(string model);
        bool IsUPSModel(string model, DateTime afterReciveDate);
        UPSCombinePO GetAvailablePOWithTrans(string model, IList<string> statusList);
        IList<UPSCombinePO> GetUPSCombinePO(UPSCombinePO condition);
        IList<UPSCombinePO> GetUPSCombinePOByStatus(UPSCombinePO condition, IList<string> statusList);
        IList<UPSPOAVPartInfo> GetAVPartByHPPO(string hppo);
        UPSHPPOInfo GetHPPO(string hppo);
        UPSIECPOInfo GetIECPO(string iecPO, string model);
        IList<UPSIECPOInfo> GetIECPO(string hppo);
    }
}
