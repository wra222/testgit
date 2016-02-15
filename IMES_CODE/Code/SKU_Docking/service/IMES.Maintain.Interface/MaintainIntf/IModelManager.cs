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
    /// <summary>
    /// [Model] 实现如下功能：
    /// 使用此界面來维护Model资料.
    /// </summary>
    public interface IModelManager
    {
        /// <summary>
        /// 取得Family下的所有Model数据的list(按Model列的字母序排序)
        /// </summary>
        /// <param name="FamilyId">FamilyId</param>
        /// <returns>Model List</returns>
        IList<ModelMaintainInfo> GetModelList(string FamilyId);

        /// <summary>
        /// 取得一条Model的记录数据
        /// </summary>
        /// <param name="ModelId">ModelId</param>
        /// <returns>ModelMaintainInfo</returns>
        ModelMaintainInfo GetModel(string strModelName);

        /// <summary>
        /// 新增一条Model的记录数据,若Model名称已重复，则提示业务异常
        /// </summary>
        /// <param name="Object">Object</param>
        void AddModel(ModelMaintainInfo Object);


        /// <summary>
        /// 保存一条Model的记录数据,若与其它Model名称已重复，则提示业务异常
        /// </summary>
        /// <param name="MB_SNo">Object</param>
        /// <param name="defectList">oldModelName</param>
        void SaveModel(ModelMaintainInfo Object, string oldModelName);

        /// <summary>
        /// 删除一条Model及modelInfo的记录数据,需判断一条Model是否与其它表关联（MO表），有则提示业务异常
        /// </summary>
        /// <param name="ModelId">ModelName</param>
        void DeleteModel(string strModelName);

        /// <summary>
        /// 取得Model下的所有ModelInfo数据的list(按Name列的字母序排序)
        /// </summary>
        /// <param name="FamilyId">strModelName</param>
        /// <returns>ModelInfo List</returns>
        IList<ModelInfoNameAndModelInfoValueMaintainInfo> GetModelInfoList(string strModelName);
        
        /// <summary>
        /// 取得一条ModelInfo的记录数据
        /// </summary>
        /// <param name="FamilyId">strModelInfoId</param>
        /// <returns>ModelInfo</returns>
        ModelInfoMaintainInfo GetModelInfo(string strModelInfoId);

        /// <summary>
        /// 新增一条ModelInfo的记录数据,返回id
        /// </summary>
        /// <param name="FamilyId">strModelInfoId</param>
        /// <returns>ModelInfo</returns>
        long AddModelInfo(ModelInfoMaintainInfo Object);

        /// <summary>
        /// 保存一条ModelInfo的记录数据,返回id
        /// </summary>
        /// <param name="FamilyId">strModelInfoId</param>
        /// <returns>ModelInfo</returns>
        void SaveModelInfo(ModelInfoMaintainInfo Object);

        /// <summary>
        /// 删除一条ModelInfo的记录数据
        /// </summary>
        /// <param name="FamilyId">strModelInfoId</param>
        /// <returns>ModelInfo</returns>
        void DeleteModelInfo(ModelInfoMaintainInfo Object);

        /// <summary>
        /// 通过model取得所属的family
        /// </summary>
        /// <param name="strModelName">strModelName</param>
        /// <returns>FamilyName</returns>
        string GetFamilyNameByModel(string strModelName);

        /// <summary>
        /// 取得匹配的所有Model数据的list(按Model列的字母序排序)，支持左匹配 like 'model%'
        /// </summary>
        /// <param name="strModelName">strModelName</param>
        /// <returns>ModelMaintainInfo list</returns>
        IList<ModelMaintainInfo> GetModelListByModel(string strFamilyName, string strModelName);

        /// <summary>
        /// 系统中所有已维护的ShipType值,按ShipType列的字母序排序
        /// </summary>
        /// <param name=""></param>
        /// <returns>ShipTypeMaintainInfo list</returns>
        IList<ShipTypeMaintainInfo> GetShipTypeList();

        /// <summary>
        /// 取ModelInfoName中所有记录,按Region、Info Name列的字母序排序
        /// </summary>
        /// <param name=""></param>
        /// <returns>ModelInfoNameMaintainInfo list</returns>
        IList<ModelInfoNameMaintainInfo> GetModelInfoNameList();


        /// <summary>
        /// 新增一条ModelInfoName的记录数据，返回id
        /// </summary>
        /// <param name="">ModelInfoNameMaintainInfo</param>
        /// <returns>ModelInfoName id</returns>
        int AddModelInfoName(ModelInfoNameMaintainInfo Object);

        /// <summary>
        /// 修改一条ModelInfoName的记录数据
        /// </summary>
        /// <param name="">ModelInfoNameMaintainInfo</param>
        /// <returns></returns>
        void SaveModelInfoName(ModelInfoNameMaintainInfo Object);

        /// <summary>
        /// 删除一条ModelInfoName的记录数据
        /// </summary>
        /// <param name="">ModelInfoNameMaintainInfo</param>
        /// <returns></returns>
        void DeleteModelInfoName(ModelInfoNameMaintainInfo Object);

        /// <summary>
        /// 保存一条Model Price的记录
        /// </summary>
        /// <param name="">ModelMaintainInfo</param>
        /// <returns></returns>
        void SaveModelPrice(ModelMaintainInfo Object);

        IList<RegionInfo> GetRegionList();

        IList<ConstValueInfo> GetConstValueListByType(String type);
    }

}
