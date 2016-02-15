using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;


namespace IMES.FisObject.Common.Part
{
    public interface IPartRepositoryEx : IPartRepository
    {
        IList<string> GetBomNodeTypeList();
        IList<string> GetPartTypeList(string bomNodeType);
        IList<PartDef> GetPartListByPartialPartNo(string partNo, int rowCount);
        IList<PartInfoMaintainInfo> GetPartInfoListByPartNo(string partNo);
        void DeletePartInfoByID(int id);
        void DeletePartInfoByIDDefered(IUnitOfWork uow,int id);

        void DeletePartInfoByID(int id,string partNo);
        void DeletePartInfoByIDDefered(IUnitOfWork uow, int id, string partNo);


        IList<PartDef> GetPartListByPartType(string partType, int rowCount);
        void SavePartEx(PartDef newPart, string oldPartNo);
        void SavePartExDefered(IUnitOfWork uow, PartDef newPart, string oldPartNo);

        IList<string> GetProductsFromProduct_Part(string partNo, int rowCount);
        void DeletePartEx(string partNo);
        void DeletePartExDefered(IUnitOfWork uow, string partNo);

        void InsertCacheUpdate(CacheTypeEnum cacheType, string item);
        void InsertCacheUpdateDefered(IUnitOfWork uow, CacheTypeEnum cacheType, string item); 
    }

    public enum CacheTypeEnum
    {
         Part = 1,
         Family ,
        Model ,
        BOM 
    }
}
