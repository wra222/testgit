using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using IMES.Infrastructure.Util;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;


namespace IMES.FisObject.PAK.Carton
{
    public interface ICartonRepository : IRepository<Carton>
    {
        
         IList<AvailableDelivery> GetAvailableDNList(string model, string dnStatus, int offsetShipdate);
         IList<AvailableDelivery> GetAvailableDNListWithSingle(string model, string dnStatus, int offsetShipdate);
         IList<AvailableDelivery> GetAvailableDNListByPo(string model, string poNo, string dnStatus, int offsetShipdate);
         IList<AvailableDelivery> GetAvailableDNListWithSingleByPo(string model, string poNo, string dnStatus, int offsetShipdate);

         DeliveryCarton BindDeliveryCarton(string cartonSN, AvailableDelivery availableDN, string editor);
         void BindCartonProduct(string cartonSN, string productId, string dn, string remark, string editor);
         void BindCartonProductDefered(IUnitOfWork uow,string cartonSN, string productId, string dn, string remark, string editor);
        // void unBindDeliveryCarton(string cartonSN);
        //void unBindDeliveryCartonDefered(IUnitOfWork uow, string cartonSN);

        void unBindCarton(string cartonSN,string editor);
        void unBindCartonDefered(IUnitOfWork uow, string cartonSN,string editor);

        void unBindCartonByDn(string dn,string editor);
        void unBindCartonByDnDefered(IUnitOfWork uow, string dn,string editor);
       
         string BindPalletWithDN(string deliveryNo);
         string BindPalletWithDN(IList<string> deliveryNoList);

         void RollBackAssignCarton(string cartonSN,string editor);
         void RollBackAssignCartonDefered(IUnitOfWork uow,string cartonSN,string editor);

         IList<string> GetReserveCartonByProdId(string productId);
         //CartonProduct UpdateCartonAbortStatus(string CartonSN); 
         IList<string> GetCartonSNListByPalletNo(string palletNo,bool includeReserveStatus);
         IList<string> GetCartonSNListByDeliveryNo(string deliveryNo, bool includeReserveStatus);

         void RemoveEdiPackingData(string boxId);
         void RemoveEdiPackingDataDefered(IUnitOfWork uow, string boxId);
         void RemoveEdiODMSession(IList<string> custSNList);
         void RemoveEdiODMSessionDefered(IUnitOfWork uow, IList<string> custSNList);

         void ClearSnoIdInShipBoxDet(string cartonSN,string editor);
         void ClearSnoIdInShipBoxDetDefered(IUnitOfWork uow, string cartonSN, string editor);

         IList<CartonProductInfo> GetCartonProductByExcludeDN(string dn);
         IList<Delivery> GetDeliveryByNoList(IList<string> dnNoList);

         void UpdateCartonPreStationByDn(string dn, string editor);
         void UpdateCartonPreStationByDnDefered(IUnitOfWork uow, string dn, string editor);

         void UpdateCartonStatusByDn(string dn, string station, int status, string line, string editor);
         void UpdateCartonStatusByDnDefered(IUnitOfWork uow, string dn, string station, int status, string line, string editor);

         void InsertCartonLogByDn(string dn, string station, int status, string line, string editor);
         void InsertCartonLogByDnDefered(IUnitOfWork uow, string dn, string station, int status, string line, string editor);

         Carton GetCartonByBoxId(string boxId);
         Carton GetCartonByPalletNo(string palletNo);
         int GetAssignedPalletCartonQty(string palletNo);

        ////unpack 1.product.CaronSN, UnitWeight.CaronWeight,PalletNo,DeliveryNo
        ////             2.productInfo UCC/BoxId
        ////             3.BoxShipDet
        ////             4.Pallet loc
        ////             5. reassign ProductAtrr CartonLocation
        // void unpackProductByCartonSN(string cartonSN);
        // void unpackProductByCartonSNDefered(IUnitOfWork uow, string cartonSN);

        // void unpackProductInfoByCartonSN(string cartonSN);
        // void unpackProductInfoByCartonSNDefered(IUnitOfWork uow, string cartonSN);

        // void unpackBoxShipDetByCartonSN(string cartonSN);
        // void unpackBoxShipDetByCartonSNDefered(IUnitOfWork uow, string cartonSN);

         IList<AssignedDeliveryPalletInfo> GetAssignedDeliveryPalletCartonQty(string palletNo,bool includeReserveStatus);

         #region fill data interface
         void FillCurrentStation(Carton carton);
         void FillCartonInfo(Carton carton);
         void FillCartonQCLog(Carton carton);
         void FillCartonLog(Carton carton);
         void FillDeliveryCarton(Carton carton);
         void FillCartonProduct(Carton carton);
         #endregion

    }
}
