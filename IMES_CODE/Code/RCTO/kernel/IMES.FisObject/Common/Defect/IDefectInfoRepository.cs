using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.Common.Defect
{
    public interface IDefectInfoRepository : IRepository<DefectInfo>
    {
        #region . For CommonIntf  .

        /// <summary>
        /// 获得Cause列表
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        IList<CauseInfo> GetCauseList(string customer, string stage);

        /// <summary>
        /// 获得MajorPart列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<MajorPartInfo> GetMajorPartList(string customer);

        /// <summary>
        /// 获得Component列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<ComponentInfo> GetComponentList(string customer);

        /// <summary>
        /// 获得Obligation列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<ObligationInfo> GetObligationList(string customer);

        /// <summary>
        /// 获得Responsibility列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<ResponsibilityInfo> GetResponsibilityList(string customer);

        /// <summary>
        /// 获得4M列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<_4MInfo> Get4MList(string customer);

        /// <summary>
        /// 获得Cover列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<CoverInfo> GetCoverList(string customer);

        /// <summary>
        /// 获得Uncover列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<UncoverInfo> GetUncoverList(string customer);

        /// <summary>
        /// 获得TrackingStatus列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<TrackingStatusInfo> GetTrackingStatusList(string customer);

        /// <summary>
        /// 获得Distribution列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<DistributionInfo> GetDistributionList(string customer);

        /// <summary>
        /// 获得Mark列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<MarkInfo> GetMarkList(string customer);

        #endregion

        /// <summary>
        /// 根据Type获得DefectInfo列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<IMES.DataModel.DefectInfo> FindDefectInfoesByType(string type);

        /// <summary>
        /// 根据Type和Customer获得DefectInfo列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<IMES.DataModel.DefectInfo> FindDefectInfoesByType(string type, string customer);

        /// <summary>
        /// 获得DefectInfo的描述信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        string TransToDesc(string type, string code);

        /// <summary>
        /// 1,根据Type查询符合条件的Repair Info 信息
        /// SQL:SELECT Code, Descr, Editor, Cdt, Udt FROM IMES_GetData..DefectInfo WHERE Type=@Type ORDER BY Code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<DefectInfoDef> GetRepairInfoByCondition(string type); 

        /// <summary>
        /// 2,添加一条记录
        /// INSERT IMES_GetData..DefectInfo (Code, Description, Type, CustomerID, Editor, Cdt, Udt) VALUES(@Code, @Descr, ‘<PUB>’, @Editor, GETDATE(), GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void AddRepairInfoItem(DefectInfoDef item);

        /// <summary>
        /// SQL:IF EXISTS(SELECT * FROM IMES_GetData..DefectInfo WHERE Type = @Type AND Code = @Code)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool CheckExistDefectInfo(string type, string code);

        /// <summary>
        /// 3,更新一条记录
        /// UPDATE IMES_GetData..DefectInfo SET Description = @Descr, Editor = @Editor, Udt = GETDATE()
        /// WHERE Type = @Type AND Code = @Code
        /// </summary>
        /// <param name="item"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        void UpdateRepairInfoItem(DefectInfoDef item, string code, string type);

        /// <summary>
        /// 4,删除选中的记录
        /// SQL:DELETE FROM IMES_GetData..DefectInfo WHERE Type = @Type AND Code= @Code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        void RemoveRepairInfoItem(string code, string type);

        /// <summary>
        /// 返回查询记录的个数
        /// SQL:SELECT count(*) FROM IMES_GetData..DefectInfo WHERE Type = @Type AND Code = @Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        int GetCountOfRepairInfoByTypeAndCode(string type,string code);

        /// <summary>
        /// SELECT * FROM IMES_GetData..DefectInfo WHERE Type = @Type AND Code = @Code
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<IMES.DataModel.DefectInfoDef> GetDefectInfoByTypeAndCode(string type, string code);

        /// <summary>
        /// 2.获取defectinfo的defect列表
        /// select distinct Code from DefectInfo order by Code
        /// </summary>
        /// <returns></returns>
        IList<string> GetDefectInfo();

        /// <summary>
        ///  update DefectInfo where Id=@Id
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        void UpdateRepairInfoItemById(DefectInfoDef item, int id);

        /// <summary>
        /// Delete DefectInfo where Id=@Id
        /// </summary>
        /// <param name="id"></param>
        void RemoveRepairInfoItemById(int id);

        /// <summary>
        /// CheckExist by type and code and CustomerId
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        bool CheckExistDefectInfo(string type, string code,string customerId);


        #region . Defered .

        void AddRepairInfoItemDefered(IUnitOfWork uow, DefectInfoDef item);

        void UpdateRepairInfoItemDefered(IUnitOfWork uow, DefectInfoDef item, string code, string type);

        void RemoveRepairInfoItemDefered(IUnitOfWork uow, string code, string type);

        #endregion
    }
}
