// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
//using IMES.Station.Interface.CommonIntf;

namespace IMES.Maintain.Interface.MaintainIntf
{
 
    public interface IPartManagerEx
    {
        IList<PartDef> GetPartByPartialPartNo(string partNo, int rowCount);
        IList<PartInfoMaintainInfo> GetPartInfoListByPartNo(string partNo);
        void AddPartInfoForVendorCode(PartInfoMaintainInfo obj);
        void DeletePartInfoByID(int id);
        void DeletePartInfoByID(int id, string partNo);
        void SavePartEx(PartDef newPart, string oldPartNo);
        IList<PartDef> GetPartListByPartType(string partType, int rowCount);

        IList<string> GetProductsFromProduct_Part(string partNo, int rowCount);
        void DeletePart(string partNo, string editor);
    }
  
}
