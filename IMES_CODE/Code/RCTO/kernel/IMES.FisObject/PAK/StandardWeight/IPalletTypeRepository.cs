using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PAK.StandardWeight
{
    public interface IPalletTypeRepository : IRepository<PalletType>
    {
        void RemovePalletType(int id);

        void RemovePalletTypeDefered(IUnitOfWork uow, int id);


        IList<PalletType> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int maxQty, int minQty);
        
        IList<PalletType> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int palletLayer, int maxQty, int minQty);

        IList<PalletType> CheckExistRangeQty(string shipWay, string regId, string stdPltFullQty, int palletLayer, string oceanType, int maxQty, int minQty);

        IList<PalletType> GetPalletType(string shipWay, string regId, string stdPltFullQty, int palletLayer, string oceanType, int qty);

        IList<PalletType> GetPalletType(string shipWay, string regId, string stdPltFullQty, int palletLayer, int qty);

        IList<PalletType> GetPalletType(string shipWay, string regId, string stdPltFullQty, int qty);

        IList<PalletType> GetPalletType(string shipWay, string regId, string stdPltFullQty);

        IList<PalletType> GetPalletType(string shipWay, string regId);

        IList<PalletType> GetPalletType(string shipWay);

        IList<PalletType> GetPalletTypeByRegId(string regId); 
        
        IList<string> GetShipWay();

        IList<string> GetRegId();

    }
}
