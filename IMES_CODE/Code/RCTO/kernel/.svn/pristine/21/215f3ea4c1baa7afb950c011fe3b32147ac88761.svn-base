using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.DataModel;

namespace IMES.FisObject.Common.Material
{
    public interface IMaterialRepository : IRepository<Material>
    {
        void AddMultiMaterialCT(IList<string> materialCTList,
                                               string materialType,
                                                string lotNo,
                                                string stage,
                                                string line,
                                                string preStatus,
                                                string curStatus,
                                                string editor);

        void AddMultiMaterialCTDefered(IUnitOfWork uow,
                                                            IList<string> materialCTList,
                                                           string materialType,
                                                            string lotNo,
                                                            string stage,
                                                            string line,
                                                            string preStatus,
                                                            string curStatus,
                                                            string editor);

        void AddMultiMaterialLog(IList<string> materialCTList,                                              
                                               string action,
                                               string stage,
                                               string line,
                                               string preStatus,
                                               string curStatus,
                                               string comment, 
                                               string editor);

        void AddMultiMaterialLogDefered(IUnitOfWork uow,
                                                            IList<string> materialCTList,                                                          
                                                            string action,
                                                            string stage,
                                                            string line,
                                                            string preStatus,
                                                            string curStatus,
                                                            string comment,
                                                            string editor);


        void UpdateMultiMaterialStatus(IList<string> materialCTList,
                                                          string preStatus,
                                                          string curStatus,
                                                           string editor);

        void UpdateMultiMaterialStatusDefered(IUnitOfWork uow,
                                                                       IList<string> materialCTList,
                                                                       string preStatus,
                                                                       string curStatus,
                                                                       string editor);
        void UpdateMultiMaterialCurStatus(IList<string> materialCTList,                                                        
                                                                string curStatus,
                                                                string editor);

        void UpdateMultiMaterialCurStatusDefered(IUnitOfWork uow,
                                                                       IList<string> materialCTList,                                                                      
                                                                       string curStatus,
                                                                       string editor);


        void AddMultiMaterialCurStatusLog(IList<string> materialCTList,
                                              string action,
                                              string stage,
                                              string line,                                              
                                              string curStatus,
                                              string comment,
                                              string editor);

        void AddMultiMaterialCurStatusLogDefered(IUnitOfWork uow,
                                                            IList<string> materialCTList,
                                                            string action,
                                                            string stage,
                                                            string line,                                                            
                                                            string curStatus,
                                                            string comment,
                                                            string editor);

        void AddMultiMaterialCT(IList<string> materialCTList,
                                             string materialType,
                                              string lotNo,
                                              string stage,
                                              string line,
                                              string preStatus,
                                              string curStatus,
                                              string model,
                                              string deliveryNo,
                                              string palletNo,
                                              string cartonSN,
                                              string pizzaId,
                                              string shipMode,  
                                              string editor);

        void AddMultiMaterialCTDefered(IUnitOfWork uow,
                                                        IList<string> materialCTList,
                                                        string materialType,
                                                         string lotNo,
                                                         string stage,
                                                          string line,
                                                            string preStatus,
                                                            string curStatus,
                                                             string model,
                                                             string deliveryNo,
                                                             string palletNo,
                                                             string cartonSN,
                                                             string pizzaId,
                                                             string shipMode,
                                                             string editor);

        void UpdateDnPalletbyMultiCartonSN(IList<string> cartonSNList,
                                                                string deliveryNo,
                                                                string palletNo, 
                                                                string preStatus, 
                                                                string status,
                                                                 string line,
                                                                 string editor);
        void UpdateDnPalletbyMultiCartonSNDefered(IUnitOfWork uow, 
                                                                            IList<string> cartonSNList, 
                                                                            string deliveryNo, 
                                                                            string palletNo, 
                                                                            string preStatus, 
                                                                            string status,
                                                                             string line,            
                                                                            string editor);

        void UpdateDnPalletbyMultiCT(IList<string> materialCTList,
                                                               string deliveryNo,
                                                               string palletNo,
                                                               string cartonSN,
                                                               string preStatus,
                                                               string status,
                                                                string line,
                                                                string editor);
        void UpdateDnPalletbyMultiCTDefered(IUnitOfWork uow,
                                                                            IList<string> materialCTList,
                                                                            string deliveryNo,
                                                                            string palletNo,
                                                                            string cartonSN,
                                                                            string preStatus,
                                                                            string status,
                                                                             string line,
                                                                            string editor);

        IList<Material> GetMaterialByType(string materialType);
        IList<Material> GetMaterialByLotNo(string materialType, string lotNo);
        IList<Material> GetMaterial(Material condition);
        int GetCombinedMaterialLotQty(string materialType,string lotNo);
        IList<MaterialLotQtyInfo> GetMaterialLotQtyGroupStatus(string materialType, string lotNo);
        IList<Material> GetMaterialByMultiCT(IList<string> materialCTList);
        IList<string> GetMaterialCTbyAttribute(string attrName, string attrValue);
        int GetCombinedMaterialQty(string materialType, string cartonSn, string deliveryNo, string palletNo);
        IList<CombinedPalletCarton> GetCartonQtywithCombinedPallet(string deliveryNo);

        //Unpack By Carton /Delivery
        void UnpackByCatonSN(string cartonSN,
                                             //string preStatus,                               
                                             string status,
                                             string editor);

        void UnpackByCatonSNDefered(IUnitOfWork uow,
                                                         string cartonSN,
                                                        //string preStatus,                                            
                                                         string status,
                                                         string editor);

        void UnpackByDeliveryNo(string deliveryNo,
                                                 //string preStatus,                                    
                                                string status,
                                                string editor);

        void UnpackByDeliveryNoDefered(IUnitOfWork uow,
                                                            string deliveryNo,
                                                            //string preStatus,                                                
                                                            string status,
                                                            string editor);

       //Check Delivery Qty on Transaction
        void CheckDnQtyAndUpdateDnStatus(string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode);
        void CheckDnQtyAndUpdateDnStatusDefered(IUnitOfWork uow, string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode);

        #region FillData
        void FillMaterialLog(Material material);
        void FillMaterialLot(Material material);
        void FillMaterialAttr(Material material);
        void FillMaterialAttrLog(Material material);
        #endregion
    }
}
