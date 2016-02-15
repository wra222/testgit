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
    /// 使用此界面來维护Model资料.
    /// </summary>
    public interface IPartManager
    {

        /// <summary>
        /// 取得所有family数据的list(按Family列的字母序排序)
        /// </summary>
        /// <returns></returns>
        IList<FamilyDef> GetFamilyInfoList();

        IList<string> GetPartInfoValueByPartDescr(string desc, string infoType1,string infoType2,string infoValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        ///   select distinct RTRIM(b.InfoValue) 
        ///  from Part a, PartInfo b
        ///  where a.PartNo = b.PartNo
        ///  and a.BomNodeType=@Type 
        /// and b.InfoType=@Type 
        /// order by InfoValue
        ///根据infotype获取part表与partinfo表的infoValue数据
        IList<string> GetInfoValue(string Type);

        #region added by wangshaohua on 2011-11-14

        /// <summary>
        /// 从part type表获取PartNodeType
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        IList<PartTypeDef> getPartNodeType(string tp);

        /// <summary>
        /// 根据part node type获取part数据列表,按照partno排序
        /// </summary>
        /// <param name="PartType"></param>
        /// <returns></returns>
        IList<PartDef> getListByPartType(string PartType);

        /// <summary>
        /// 根据bomNodeType获取part数据列表,按照partno排序
        /// </summary>
        /// <param name="bomNode"></param>
        /// <returns></returns>
        IList<PartDef> getLstByBomNode(string bomNode);

        /// <summary>
        /// 根据partno获取part列表
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<PartDef> getLstByPartNo(string partNo);

        /// <summary>
        /// 修改part数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="partNo"></param>
        void updatePart(PartDef obj, string partNo);

        /// <summary>
        /// 添加一条part记录
        /// </summary>
        /// <param name="obj"></param>
        void addPart(PartDef obj);

        /// <summary>
        /// 根据partno删除一条记录
        /// </summary>
        /// <param name="partNo"></param>
        void deletePart(string partNo);

        /// <summary>
        /// 根据type获取所有Description的数据列表
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        IList<DescTypeInfo> getDescriptionList(string tp);

        /// <summary>
        /// 写入PartInfo表一条数据
        /// </summary>
        /// <param name="obj"></param>
        void addPartInfo(PartInfoMaintainInfo obj);

        /// <summary>
        /// 根据InfoType删除PartInfo一条数据
        /// </summary>
        /// <param name="item"></param>
        void deletePartInfo(string item);

        /// <summary>
        /// 根据partno infotype infovalue获取partinfo列表
        /// </summary>
        /// <param name="partno"></param>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        IList<PartInfoMaintainInfo> getLstPartInfo(string partno, string infoType, string infoValue);


        /// <summary>
        /// 修改partinfo数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="partno"></param>
        void updatePartInfo(PartInfoMaintainInfo obj, string partno, string infoType, string infoValue);


        /// <summary>
        /// 联合查询partinfo与parttype表
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        IList<PartTypeAndPartInfoValue> GetPartTypeAndPartInfoValueListByPartNo(string partNo, string partType);



        #endregion

        #region 原part方法
        /*
        /// <summary>
        /// 取得全部PartType List
        /// </summary>
        /// <param name="PartNo"></param>
        /// <returns>Part Type List</returns>
        IList<PartTypeMaintainInfo> GetPartTypeList();

        /// <summary>
        /// 取得全部PartType List
        /// </summary>
        /// <param name="PartNo">PartType</param>
        /// <returns>Part Type Description List</returns>
        IList<PartTypeDescMaintainInfo> GetPartTypeDescList(string PartType);

        /// <summary>
        /// 依据PartNo查询Part，支持 like 'PartNo%'
        /// </summary>
        /// <param name="PartNo">PartNo</param>
        /// <returns>Part List</returns>
        IList<PartMaintainInfo> GetPartList(string PartNo);

        /// <summary>
        /// 依据PartNo， get Part
        /// </summary>
        /// <param name="ModelId">PartNo</param>
        /// <returns>PartMaintainInfo</returns>
        PartMaintainInfo GetPart(string PartNo);

        /// <summary>
        /// 新增一条Part的记录数据
        /// </summary>
        /// <param name="Object">Object</param>
        //void AddPart(PartMaintainInfo Object);                 



        /// <summary>
        /// 保存一条Part的记录数据
        /// </summary>
        /// <param name="MB_SNo">Object</param>
        void SavePart(PartMaintainInfo Object);


        /// <summary>
        /// 删除一条Part的记录数据
        /// </summary>
        /// <param name="ModelId">PartNo</param>
        void DeletePart(string PartNo);


        /// <summary>
        /// 取得PartNo下的所有AssemblyCode数据的list(按Family、Model、Region列的字符序排序)
        /// </summary>
        /// <param name="FamilyId">PartNo</param>
        /// <returns>AssemblyCode List</returns>
        IList<AssemblyCodeMaintainInfo> GetAssemblyCodeList(string PartNo);

        /// <summary>
        /// 取得Model, PartNo下的所有AssemblyCode数据的list(按Assembly code列的字符序排序)
        /// </summary>
        /// <param name="FamilyId">PartNo</param>
        /// <returns>AssemblyCode List</returns>
        IList<AssemblyCodeMaintainInfo> GetAssemblyCodeList(string Model, string PartNo);


        /// <summary>
        /// 取得一条AssemblyCode的记录数据
        /// </summary>
        /// <param name="FamilyId">strId</param>
        /// <returns>AssemblyCodeMaintainInfo</returns>
        AssemblyCodeMaintainInfo GetAssemblyCode(string strId);


        /// <summary>
        /// 新增一条AssemblyCode的记录数据
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns></returns>
        int AddAssemblyCode(AssemblyCodeMaintainInfo Object);



        /// <summary>
        /// 保存一条AssemblyCode的记录数据
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns></returns>
        int SaveAssemblyCode(AssemblyCodeMaintainInfo Object);



        /// <summary>
        /// 删除一条AssemblyCode的记录数据
        /// </summary>
        /// <param name="FamilyId">strId</param>
        /// <returns></returns>
        void DeleteAssemblyCode(string strId);

        /// <summary>
        /// 依据PartNo，取得相关PartInfo的记录数据
        /// </summary>
        /// <param name="FamilyId">PartNo</param>
        /// <returns>PartInfoMaintainInfo list</returns>
        //IList<PartInfoMaintainInfo> GetPartInfoList(string PartNo, string PartType);


        /// <summary>
        /// 新增一条PartInfo的记录数据，返回id
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns>part info id</returns>
        int AddPartInfo(string PartNo, PartInfoMaintainInfo Object);

        /// <summary>
        /// 修改一条PartInfo的记录数据，返回id
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns></returns>
        void SavePartInfo(string PartNo, PartInfoMaintainInfo Object);

        /// <summary>
        /// 删除一条PartInfo的记录
        /// </summary>
        /// <param name="FamilyId">Object</param>
        /// <returns></returns>
        void DeletePartInfo(string PartNo, PartInfoMaintainInfo Object);

       

        /// <summary>
        /// 取得所有region数据的list(按region列的字母序排序)
        /// </summary>
        /// <returns></returns>
        IList<RegionMaintainInfo> GetRegionList();

        /// <summary>
        /// 所有的Flag栏位为1的PartNo，来自于Part数据表，按照part no排序。
        /// </summary>
        /// <returns></returns>
        //IList<PartMaintainInfo> GetPartList();

        /// <summary>
        /// 根据assembly code，从AssemblyCode表中取得part，其中包含editor, cdt, udt信息等
        /// </summary>
        /// <returns></returns>
        PartMaintainInfo getPartByAssemblyCode(string strAssemblyCode);

        /// <summary>
        /// 从PartInfo表中取得数据
        /// </summary>
        /// <returns></returns>
        IList<PartTypeAttributeAndPartInfoValueMaintainInfo> GetPartTypeAttributeAndPartInfoValueListByPartNo(string partNo, string partType);

        /// <summary>
        /// 从AssemblyCodeInfo表中取得数据
        /// </summary>
        /// <returns></returns>
        IList<PartTypeAttributeAndPartInfoValueMaintainInfo> GetPartTypeAttributeAndPartInfoValueListByAssemblyCode(string assemblyCode, string partType);

        /// <summary>
        /// 新增一条AssemblyCodeInfo的记录数据
        /// </summary>
        /// <returns></returns>
        int AddAssemblyCodeInfo(AssemblyCodeInfoMaintainInfo Object);

        /// <summary>
        /// 修改一条AssemblyCodeInfo的记录数据
        /// </summary>
        /// <returns></returns>
        void SaveAssemblyCodeInfo(AssemblyCodeInfoMaintainInfo Object);

        /// <summary>
        /// 删除一条AssemblyCodeInfo的记录数据
        /// </summary>
        /// <returns></returns>
        void DeleteAssemblyCodeInfo(AssemblyCodeInfoMaintainInfo Object);
         */
        #endregion


    }

}
