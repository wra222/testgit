using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;

namespace IMES.FisObject.Common.Material
{
    public interface IMaterialBoxRepository : IRepository<MaterialBox>
    {
        IList<MaterialBox> GetMaterialBoxByLot(string materialType, string lotNo);
        IList<MaterialBox> GetMaterialBoxBySpec(string materialType, string specNo);
        MaterialLot GetMaterialLot(string materialType, string lotNo);
        IList<MaterialLot> GetMaterialLotBySpec(string materialType, string SpecNo);
        IList<MaterialBox> GetMaterialBox(MaterialBox condition);
        IList<MaterialBox> GetMaterialBoxbyMultiBoxId(IList<string> boxIdList);
        IList<string> GetMaterialBoxIdByAttribute(string attrName, string attrValue);
        IList<CombinedPalletCarton> GetCartonQtywithCombinedPallet(string deliveryNo);
        int GetCombinedMaterialBoxQty(string materialType, string cartonSn, string deliveryNo, string palletNo);

        void UpdateDnPalletStatusbyMultiBoxId(IList<string> boxIdList,
                                                                    string deliveryNo,
                                                                    string cartonSN, 
                                                                    string palletNo,
                                                                    string status, 
                                                                    string line,
                                                                     string editor);
        void UpdateDnPalletStatusbyMultiBoxIdDefered(IUnitOfWork uow, 
                                                                    IList<string> boxIdList,
                                                                    string deliveryNo,
                                                                    string cartonSN,
                                                                    string palletNo,
                                                                    string status,
                                                                    string line,
                                                                     string editor);


       //Check Delivery Qty on Transaction
        void CheckDnQtyAndUpdateDnStatus(string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode);
        void CheckDnQtyAndUpdateDnStatusDefered(IUnitOfWork uow, string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode);
 

        #region FillData
        void FillMaterialLot(MaterialBox materialBox);
        void FillMaterialBoxAttr(MaterialBox materialBox);
        void FillMaterialBoxAttrLog(MaterialBox materialBox);
        #endregion
    }
}
