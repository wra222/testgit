using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;


namespace IMES.FisObject.Common.Model
{
    public interface IModelRepositoryEx : IModelRepository
    {
        #region for maintain

        IList<ModelMaintainInfo> GetModelListByPartialModelNo(string modelNo, int rowCount);

        IList<ModelInfoNameAndModelInfoValueMaintainInfo> GetModelInfoNameAndModelInfoValueListByModels(IList<string> models);

        void DeleteModelsInfo(string infoName, IList<string> models);
        void DeleteModelsInfoDefered(IUnitOfWork uow,string infoName, IList<string> models);
        void UpdateModelsInfo(string infoName, string infoValue, IList<string> models);
        void UpdateModelsInfoDefered(IUnitOfWork uow,string infoName, string infoValue, IList<string> models);

        IList<string> GetExistedModelsFromModelInfoByModels(string infoName, IList<string> models);

        void DeleteModelEx(string model);
        void DeleteModelExDefered(IUnitOfWork uow, string model);
        IList<string> GetModelsFromProduct(string modelNo, int rowCount);

        #endregion

    }
}
