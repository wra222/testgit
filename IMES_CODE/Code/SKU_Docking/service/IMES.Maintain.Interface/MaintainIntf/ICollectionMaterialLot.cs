using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using System.Collections;
namespace IMES.Maintain.Interface.MaintainIntf
{
    /// <summary>
    /// Collection Material Lot
    /// </summary>
    public interface ICollectionMaterialLot
    {
        void CheckBoxId(string boxId);
        void AddMaterialBox(string boxId, string specNo, string lotNo, int qty,string materialType,string feedType,string status, string editor);
        List<string> GetMaterialByLot(string lotNo, string materialType);
        void CheckExistCT(string ct);
        void CheckExistCtAndLotNo(string ct,string lotNo,string station);
     //   void CheckExistLotNo(string lotNo);
        void AddMaterialCtList(IList<string> ctList, string materialType, string lotNo, string status, string preStatus, string stage, string line, string editor);
        List<string> GetLotInfoByCT(string ct, string materialType, string station,string processType);
         void UpdateMaterialByCtList(IList<string> ctList,string stage,string editor,string station,string action,string line);
    }
}
