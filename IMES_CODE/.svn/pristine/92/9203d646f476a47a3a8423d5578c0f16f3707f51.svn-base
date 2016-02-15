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
    /// <summary>
    /// [Model] 实现如下功能：
    /// 使用此界面來维护PartType资料.
    /// </summary>
    public interface IPartTypeManager
    {
        /// <summary>
        /// 取得全部PartType List
        /// </summary>
        /// <param name="PartNo"></param>
        /// <returns>Part Type List</returns>
        IList<PartTypeMaintainInfo> GetPartTypeList();

        /// <summary>
        /// 取得全部PartTypeDesc List
        /// </summary>
        /// <param name="PartNo">PartType</param>
        /// <returns>Part Type Description List</returns>
        IList<PartTypeDescMaintainInfo> GetPartTypeDescList(string PartType);
        
        /// <summary>
        /// 取得全部PartTypeAttribute List
        /// </summary>
        /// <param name="PartNo">PartType</param>
        /// <returns>PartTypeAttribute List</returns>
        IList<PartTypeAttributeMaintainInfo> GetPartTypeAttributeList(string PartType);

        /// <summary>
        /// 取得全部PartTypeDesc List
        /// </summary>
        /// <param name="PartNo">PartType</param>
        /// <returns>Part Type Mapping List</returns>
        IList<PartTypeMappingMaintainInfo> GetPartTypeMappingList(string PartType);
        
        /// <summary>
        /// 新增一条PartType的记录数据
        /// </summary>
        /// <param name="PartTypeMaintainInfo">Object</param>
        void AddPartType(PartTypeMaintainInfo Object);                 

        
        /// <summary>
        /// 保存一条PartType的记录数据
        /// </summary>
        /// <param name="PartTypeMaintainInfo">Object</param>
        /// <param name="string">strOldPartType</param>
        void SavePartType(PartTypeMaintainInfo Object, string strOldPartType);                 


        /// <summary>
        /// 删除一条PartType的记录数据
        /// </summary>
        /// <param name="string">strOldPartType</param>
        void DeletePartType(string strOldPartType);                  


        /// <summary>
        /// 新增一条Desc的记录数据
        /// </summary>
        /// <param name="PartTypeDescMaintainInfo">Object</param>
        /// <returns></returns>
        int AddPartTypeDescription(PartTypeDescMaintainInfo Object);



        /// <summary>
        /// 保存一条Desc的记录数据
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns></returns>
        void SavePartTypeDescription(PartTypeDescMaintainInfo Object);          



        /// <summary>
        /// 删除一条Desc的记录数据
        /// </summary>
        /// <param name="PartTypeDescriptionId">strId</param>
        /// <returns></returns>
        void DeletePartTypeDescription(string strId);


        /// <summary>
        /// 新增一条PartTypeAttribute的记录数据
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns>part info id</returns>
        void AddPartTypeAttribute(PartTypeAttributeMaintainInfo Object);

        /// <summary>
        /// 修改一条PartTypeAttribute的记录数据
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns></returns>
        void SavePartTypeAttribute(string strOldCode, PartTypeAttributeMaintainInfo Object);          

        /// <summary>
        /// 删除一条PartTypeAttribute的记录
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns></returns>
        void DeletePartTypeAttribute(string strPartType, string strOldCode);

        /// <summary>
        /// 新增一条Desc的记录数据
        /// </summary>
        /// <param name="Mapping">Object</param>
        /// <returns></returns>
        int AddPartTypeMapping(PartTypeMappingMaintainInfo Object);



        /// <summary>
        /// 保存一条Desc的记录数据
        /// </summary>
        /// <param name="Mapping">Object</param>
        /// <returns></returns>
        void SavePartTypeMapping(PartTypeMappingMaintainInfo Object);



        /// <summary>
        /// 删除一条Desc的记录数据
        /// </summary>
        /// <param name="PartTypeMappingId">strId</param>
        /// <returns></returns>
        void DeletePartTypeMapping(string strId);

    }

}
