// created by itc205033

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.Common.Model;
using IMES.DataModel;
//using IMES.Station.Interface.CommonIntf;

namespace IMES.Maintain.Interface.MaintainIntf
{
 
    public interface IModelManagerEx
    {

        IList<ModelMaintainInfo> GetModelListByPartialModelNo(string modelNo, int rowCount);

        IList<ModelInfoNameAndModelInfoValueMaintainInfo> GetModelInfoNameAndModelInfoValueListByModels(IList<string> models);

        void DeleteModelsInfo(string infoName, IList<string> models);
        void UpdateModelsInfo(string infoName, string infoValue, IList<string> models);
        IList<string> GetExistedModelsFromModelInfoByModels(string infoName, IList<string> models, ref IList<string> notExistedModels);

        bool ModelExist(string modelId);

        void DeleteModel(string model, string editor);
        IList<string> GetModelsFromProduct(string modelNo, int rowCount);

        IList<string> GetCustomerByFamily(string family);
    }
  
}
