﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.FisObject.Common.Repair;
using System;

namespace IMES.FisObject.Common.Part
{
    /// <summary>
    /// Part Repository 接口
    /// </summary>
    public interface IPartRepository : IRepository<IPart>
    {
        /// <summary>
        /// 晚加载PartInfo
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        IPart FillPartInfos(IPart part);

        /// <summary>
        /// 根据AssemblyCode查找AssemblyCode对象列表.
        /// </summary>
        /// <param name="assCode"></param>
        /// <returns></returns>
        IList<AssemblyCode> FindAssemblyCode(string assCode);

        #region . For CommonIntf  .

        //IList<VGAInfo> GetVGAList();

        //IList<FANInfo> GetFANList();

        /// <summary>
        /// 获得PartType列表
        /// </summary>
        /// <returns></returns>
        IList<PartTypeInfo> GetPartTypeList();

        /// <summary>
        /// 根据Part type获取PartType对象
        /// </summary>
        /// <returns></returns>
        PartType GetPartType(string type);

        /// <summary>
        /// 取得PPID类型信息列表
        /// </summary>
        /// <returns>PPID类型信息列表</returns>
        IList<PPIDTypeInfo> GetPPIDTypeList();

        /// <summary>
        /// 取得PPID描述信息列表
        /// </summary>
        /// <returns>PPID描述信息列表</returns>
        IList<PPIDDescriptionInfo> GetPPIDDescriptionList(string PPIDTypeId);

        /// <summary>
        /// 取得PartNo信息列表
        /// </summary>
        /// <param name="PPIDTypeId">PPID类型标识</param>
        /// <param name="PPIDDescrptionId">PPID描述标识</param>
        /// <returns>PartNo信息列表</returns>
        IList<PartNoInfo> GetPartNoList(string PPIDTypeId, string PPIDDescrptionId);

        /// <summary>
        /// 取得KP类型列表
        /// </summary>
        /// <returns>KP类型列表</returns>
        IList<KPTypeInfo> GetKPTypeList();

        #endregion

        /// <summary>
        /// 获取指定PartType的Match,Check,Save设定
        /// </summary>
        /// <param name="partType">part类型</param>
        /// <param name="customer">customer</param>
        /// <returns>Match,Check,Save设定</returns>
        /// <remark>需要提供缓存，以PartType为Key</remark>
        IList<PartCheck> GetPartCheck(string partType, string customer);

        /// <summary>
        /// 晚加载PartCheck的MatchRule
        /// </summary>
        /// <param name="partCheck"></param>
        /// <returns></returns>
        PartCheck FillMatchRule(PartCheck partCheck);

        //IList<IPart> GetPartsByType(string type);

        //IList<IPart> GetPartsByTypeAndDescr(string type, string descr);

        /// <summary>
        /// 获得PartForbidden列表
        /// </summary>
        /// <param name="family"></param>
        /// <param name="model"></param>
        /// <param name="descr"></param>
        /// <param name="pn"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<PartForbidden> GetPartForbidden(string family, string model, string descr, string pn, string code);

        ///// <summary>
        ///// 根据PartInfo表的InfoType和InfoValue获取Part对象	
        ///// </summary>
        ///// <param name="infoType"></param>
        ///// <param name="infoValue"></param>
        ///// <returns></returns>
        //IList<IPart> GetPartByInfoTypeValue(string infoType, string infoValue);

        ////1.--get vendor
        ////declare @CTNO varchar(20)
        ////select Vendor from  IMES_GetData..Part where PartNo=(select IECPn from IMES_FA..PartSN where IECSN=@CTNO)
        //IList<string> GetVendorListByCTNO(string ctno);

        /// <summary>
        /// 根据AssemblyCode和InfoType获得AssemblyCode
        /// </summary>
        /// <param name="assemblyCode"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        string GetAssemblyCodeInfo(string assemblyCode, string infoType);

        /// <summary>
        /// select distinct AssemblyCode from IMES_GetData..AssemblyCode where PartNo IN (Select Value from IMES_GetData..ModelInfo where Name='PN' and Model='') and Model=''
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<string> GetAssemblyCodesByModel(string model);

        /// <summary>
        /// select distinct AssemblyCode from AssemblyCode where PartNo=''
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<string> GetAssemblyCodesByPartNo(string partNo);

        /// <summary>
        /// 根据PartNo查找BSParts
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        BSParts FindBSParts(string partNo);

        /// <summary>
        /// select PartType from PartCheck where Customer=@customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<string> GetPartTypeListByCustomerFromPartCheck(string customer);

        #region For Switch Board

        /// <summary>
        /// 取得Switch Board的Family信息列表
        /// SELECT DISTINCT b.InfoValue as Family
        /// FROM IMES_GetData..Part a, IMES_GetData..PartInfo b, IMES_GetData..PartInfo c
        /// WHERE a.PartNo = b.PartNo
        ///    AND a.PartNo = c.PartNo 
        ///    AND a.PartType = 'SB'
        ///    AND a.Descr = 'TPM'
        ///    AND LEFT(a.PartNo, 3) = '111'
        ///    AND b.InfoType = 'MN'
        ///    AND c.InfoType = 'MDL'
        ///    ORDER BY Family
        /// </summary>
        /// <returns>Switch Board的Family信息列表</returns>
        IList<string> GetFamilyList();

        /// <summary>
        ///取得Family的PCB信息列表 
        ///  SELECT DISTINCT b.InfoValue as PCB
        ///  FROM IMES_GetData..Part a, IMES_GetData..PartInfo b, IMES_GetData..PartInfo c
        ///  WHERE a.PartNo = b.PartNo
        ///  AND a.PartNo = c.PartNo
        ///  AND a.PartType = 'SB'
        ///  AND a.Descr = 'TPM'
        ///  AND LEFT(a.PartNo, 3) = '111'
        ///  AND b.InfoType = 'MDL'
        ///  AND c.InfoType = 'MN'
        ///  AND c.InfoValue = @Family
        ///  ORDER BY PCB 
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> GetPCBListByFamily(string family);

        /// <summary>
        /// 取得PCB的111信息列表
        /// SELECT DISTINCT a.PartNo as [111]
        ///  FROM IMES_GetData..Part a, IMES_GetData..PartInfo b, IMES_GetData..PartInfo c
        ///  WHERE a.PartNo = b.PartNo
        ///  AND a.PartNo = c.PartNo
        ///  AND a.PartType = 'SB'
        ///  AND a.Descr = 'TPM'
        ///  AND LEFT(a.PartNo, 3) = '111'
        ///  AND b.InfoType = 'MDL'
        ///  AND b.InfoValue = @PCB
        ///  AND c.InfoType = 'MN'
        ///  AND c.InfoValue = @Family
        ///  ORDER BY [111]
        /// </summary>
        /// <param name="pcb"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> Get111ListByPCB(string pcb, string family);

        /// <summary>
        ///  取得111的FruNo信息列表
        ///  SELECT ISNULL(InfoValue, '') as [FRUNO] FROM IMES_GetData..PartInfo
        ///  WHERE PartNo = @PartNo
        ///  AND InfoType = 'FRUNO'
        /// </summary>
        /// <param name="pn111"></param>
        /// <returns></returns>
        string GetFruNoBy111(string pn111);

        #endregion

        #region For Maintain

        /// <summary>
        /// 根据PN前缀获得Part的列表
        /// </summary>
        /// <param name="partNoPreStr"></param>
        /// <returns></returns>
        IList<IPart> GetPartListByPreStr(string partNoPreStr);

        /// <summary>
        /// 根据PN获得AssemblyCode列表
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<AssemblyCode> GetAssemblyCodeList(string partNo);

        /// <summary>
        /// 新增AssemblyCode
        /// </summary>
        /// <param name="Object"></param>
        void AddAssemblyCode(AssemblyCode Object);

        /// <summary>
        /// 修改AssemblyCode
        /// </summary>
        /// <param name="Object"></param>
        void UpdateAssemblyCode(AssemblyCode Object);

        /// <summary>
        /// 获得指定PartNo和AssemblyCode的assemblyCode的ID列表
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="assemblyCode"></param>
        /// <returns></returns>
        IList<int> CheckExistedAssemblyCode(string partNo, string assemblyCode);

        /// <summary>
        /// 删除AssemblyCode
        /// </summary>
        /// <param name="assemblyCodeId"></param>
        void DeleteAssemblyCode(int assemblyCodeId);

        ////参考sql如下：
        ////select PartTypeAttribute.Code as Item, Case When A.InfoValue is null then '' else A.InfoValue End Case as Content 
        ////From (select Part.PartType, PartInfo.InfoType, PartInfo.InfoValue 
        ////        from Part 
        ////        inner join PartInfo 
        ////        on Part.PartNo = PartInfo.PartNo where Part.PartNo = ?) as A 
        ////Right Outer Join PartTypeAttribute 
        ////On A.PartType = PartTypeAttribute.PartType and A.InfoType = PartTypeAttribute.Code
        ////where PartTypeAttribute.PartType = 'LCM'
        //IList<PartTypeAttributeAndPartInfoValue> GetPartTypeAttributeAndPartInfoValueList(string partNo, string partType);

        /// <summary>
        /// 取得全部PartType List
        /// 栏位包括Group和Part Type、Editor、Cdt、Udt
        /// 按Group、Part Type列的字符序排序
        /// </summary>
        /// <returns></returns>
        IList<PartType> GetPartTypeObjList();

        /// <summary>
        /// 依据PartType，取得全部PartTypeDescription List
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        IList<PartTypeDescription> GetPartTypeDescriptionList(string partType);

        /// <summary>
        /// 根据AssemblyCode ID获得AssemblyCode
        /// </summary>
        /// <param name="assCodeId"></param>
        /// <returns></returns>
        AssemblyCode FindAssemblyCodeById(int assCodeId);

        /// <summary>
        /// 根据model和partNo获得AssemblyCode列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<AssemblyCode> GetAssemblyCodeList(string model, string partNo);

        /// <summary>
        /// 14)取得partCheck列表
        /// SELECT [Customer],[PartType],[ValueType],[NeedSave],[NeedCheck], Editor,Cdt,Udt,[ID]
        ///   FROM [IMES_GetData].[dbo].[PartCheck] order by [Customer],[PartType],[ValueType]
        /// </summary>
        /// <returns></returns>
        DataTable GetPartCheckList();

        /// <summary>
        /// 15)PartType列表,
        /// SELECT [PartType]
        ///  FROM [IMES_GetData].[dbo].[PartType] ORDER BY [PartType]
        /// </summary>
        /// <returns></returns>
        DataTable GetPartTypes();

        /// <summary>
        /// 16)Value Type列表
        /// SELECT [Code]      
        ///   FROM [IMES_GetData].[dbo].[PartTypeAttribute]
        /// where PartType='partType' ORDER BY [Code]
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        DataTable GetValueTypeList(string partType);

        /// <summary>
        /// 17)
        /// SELECT [ID]
        ///   FROM [IMES_GetData].[dbo].[PartCheck]
        /// WHERE [Customer]='customer' AND [PartType]='partType' AND [ValueType]='valueType'
        /// AND [ID]<>id
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="partType"></param>
        /// <param name="valueType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetExistPartCheck(string customer, string partType, string valueType, int id);

        /// <summary>
        /// 17')
        /// SELECT [ID]
        ///   FROM [IMES_GetData].[dbo].[PartCheck]
        /// WHERE [Customer]='customer' AND [PartType]='partType' AND [ValueType]='valueType'
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="partType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        DataTable GetExistPartCheck(string customer, string partType, string valueType);

        /// <summary>
        /// 18)删除PartCheck
        /// </summary>
        /// <param name="item"></param>
        void DeletePartCheck(PartCheck item);

        /// <summary>
        /// 19) 新增PartCheck
        /// </summary>
        /// <param name="item"></param>
        void AddPartCheck(PartCheck item);

        /// <summary>
        /// 20)更新PartCheck
        /// </summary>
        /// <param name="item"></param>
        void SavePartCheck(PartCheck item);

        /// <summary>
        /// 21)取得MatchRule列表
        /// SELECT [RegExp],[PnExp],[PartPropertyExp],[ContainCheckBit],Editor,Cdt,Udt,[ID],[PartCheckID]    
        ///   FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
        /// where [PartCheckID]=partCheckID
        /// order by [RegExp],[PnExp],[PartPropertyExp]
        /// </summary>
        /// <param name="partCheckID"></param>
        /// <returns></returns>
        DataTable GetMatchRuleByPartCheckID(int partCheckID);

        /// <summary>
        /// 22)删除一条PartCheckMatchRule
        /// DELETE FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
        ///  WHERE ID=id
        /// </summary>
        /// <param name="item"></param>
        void DeletePartCheckMatchRule(PartCheckMatchRule item);

        /// <summary>
        /// 23)添加一条PartCheckMatchRule
        /// </summary>
        /// <param name="item"></param>
        void AddPartCheckMatchRule(PartCheckMatchRule item);

        /// <summary>
        /// 24)更新一条PartCheckMatchRule
        /// </summary>
        /// <param name="item"></param>
        void SavePartCheckMatchRule(PartCheckMatchRule item);

        /// <summary>
        /// 25)
        /// SELECT [ID]
        ///   FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
        /// Where [RegExp]='regExp' AND [PartPropertyExp]='partPropertyExp' AND [PnExp]='pnExp' AND ID<>id 
        /// 当regExp=""时，[RegExp]='regExp'条件变为[RegExp]='regExp' OR [RegExp] is Null 
        /// 当partPropertyExp =""时，[partPropertyExp]='partPropertyExp' 条件变为[PartPropertyExp]='partPropertyExp' OR [PartPropertyExp] is Null
        /// 当pnExp=""时，[PnExp]='pnExp'条件变为[PnExp]='pnExp' OR [PnExp] is Null
        /// </summary>
        /// <param name="regExp"></param>
        /// <param name="partPropertyExp"></param>
        /// <param name="pnExp"></param>
        /// <param name="partCheckID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetExistPartCheckMatchRule(string regExp, string partPropertyExp, string pnExp, int partCheckID, int id);

        /// <summary>
        /// 25')
        /// SELECT [ID]
        ///   FROM [IMES_GetData].[dbo].[PartCheckMatchRule]
        /// Where [RegExp]='regExp' AND [PartPropertyExp]='partPropertyExp' AND [PnExp]='pnExp'
        /// 当regExp=""时，[RegExp]='regExp'条件变为[RegExp]='regExp' OR [RegExp] is Null 
        /// 当partPropertyExp =""时，[partPropertyExp]='partPropertyExp' 条件变为[PartPropertyExp]='partPropertyExp' OR [PartPropertyExp] is Null
        /// 当pnExp=""时，[PnExp]='pnExp'条件变为[PnExp]='pnExp' OR [PnExp] is Null
        /// </summary>
        /// <param name="regExp"></param>
        /// <param name="partPropertyExp"></param>
        /// <param name="pnExp"></param>
        /// <param name="partCheckID"></param>
        /// <returns></returns>
        DataTable GetExistPartCheckMatchRule(string regExp, string partPropertyExp, string pnExp, int partCheckID);

        /// <summary>
        /// 28)SELECT distinct [PartType] FROM [IMES_GetData_Datamaintain].[dbo].[PartCheck] WHERE [Customer]='customer' order by [PartType]
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        DataTable GetCustomerPartTypeList(string customer);

        /// <summary>
        /// 所有的Flag栏位为1的PartNo，来自于Part数据表，按照part no排序。
        /// </summary>
        /// <returns></returns>
        IList<IPart> GetPartList();

        /// <summary>
        /// 根据assembly code，取得part，其中包含editor, cdt, udt信息等。
        /// </summary>
        /// <param name="strAssemblyCode"></param>
        /// <returns></returns>
        IPart GetPartByAssemblyCode(string strAssemblyCode);

        /// <summary>
        /// 查询等于指定partNo, family, model, region而不等于指定assemblyCodeId的AssemblyCode的数目.
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="family"></param>
        /// <param name="model"></param>
        /// <param name="region"></param>
        /// <param name="assemblyCodeId"></param>
        /// <returns></returns>
        int CheckExistedAssemblyCode(string partNo, string family, string model, string region, string assemblyCodeId);

        /// <summary>
        /// 参考sql如下：
        /// select PartTypeAttribute.Code as Item, PartTypeAttribute.Description as Description,  isNull(A.InfoValue, '') as Content,
        ///         A.Editor  as Editor, A.Cdt as Cdt
        /// From (select PartNo, InfoType, InfoValue, Editor, Cdt from PartInfo where PartNo = ？) as A 
        /// Right Outer Join PartTypeAttribute 
        /// On A.InfoType = PartTypeAttribute.Code
        /// where PartTypeAttribute.PartType = ?
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        IList<PartTypeAttributeAndPartInfoValue> GetPartTypeAttributeAndPartInfoValueListByPartNo(string partNo, string partType);

        /// <summary>
        /// 参考sql如下：
        /// select PartTypeAttribute.Code as Item, PartTypeAttribute.Description as Description,  isNull(A.InfoValue, '') as Content, 
        ///         A.Editor as Editor, A.Cdt as Cdt
        /// From (select AssemblyCode, InfoType, InfoValue, Editor, Cdt from AssemblyCodeInfo where AssemblyCode = ？) as A 
        /// Right Outer Join PartTypeAttribute 
        /// On A.InfoType = PartTypeAttribute.Code
        /// where PartTypeAttribute.PartType = ?
        /// </summary>
        /// <param name="assemblyCode"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        IList<PartTypeAttributeAndPartInfoValue> GetPartTypeAttributeAndPartInfoValueListByAssemblyCode(string assemblyCode, string partType);

        /// <summary>
        /// 新增AssemblyCodeInfo
        /// </summary>
        /// <param name="item"></param>
        void AddAssemblyCodeInfo(AssemblyCodeInfo item);

        /// <summary>
        /// 修改AssemblyCodeInfo
        /// </summary>
        /// <param name="item"></param>
        void SaveAssemblyCodeInfo(AssemblyCodeInfo item);

        /// <summary>
        /// 删除AssemblyCodeInfo
        /// </summary>
        /// <param name="item"></param>
        void DeleteAssemblyCodeInfo(AssemblyCodeInfo item);

        /// <summary>
        /// 查询系统中所有已维护的Region值,按Region列的字母序排序
        /// </summary>
        /// <returns></returns>
        IList<Region> GetRegionList();

        //public IList<Region> GetAllRegion()

        /// <summary>
        /// 新增Region
        /// </summary>
        /// <param name="region"></param>
        void AddRegion(Region region);

        /// <summary>
        /// 参考sql
        /// update Region 
        /// set Region=?,
        ///     Description=?,
        ///     Editor=?,
        ///     Udt=? 
        /// where Region=? 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="regionname"></param>
        void UpdateRegion(Region region, string regionname);

        /// <summary>
        /// 删除Region
        /// </summary>
        /// <param name="region"></param>
        void DeleteRegionByName(string region);

        /// <summary>
        /// 判断Region是否存在
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        bool IFRegionIsExists(string region);

        /// <summary>
        /// 参考sql
        /// select * from Model where Region=?
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        bool IFRegionIsInUse(string region);

        /// <summary>
        /// 新增PartType
        /// </summary>
        /// <param name="item"></param>
        void AddPartType(PartType item);

        /// <summary>
        /// 根据strOldPartType更新PartType记录
        /// </summary>
        /// <param name="item"></param>
        /// <param name="strOldPartType"></param>
        void SavePartType(PartType item, string strOldPartType);

        /// <summary>
        /// 根据strOldPartType更新PartType记录
        /// </summary>
        /// <param name="item"></param>
        /// <param name="strOldPartType"></param>
        void SavePartType(PartType item);

        /// <summary>
        /// 根据PartType删除PartType记录
        /// </summary>
        /// <param name="partType"></param>
        void DeletePartTypeByPartType(string partType);

        void DeletePartTypeByPartType(int id);

        void DeletePartTypeAttAndDescByPartType(string partType);

        /// <summary>
        /// 根据PartType取得PartTypeAttribute表对应的记录
        /// 栏位包括Code和Description、Editor、Cdt、Udt
        /// 按Code列的字符序排序
        /// </summary>
        /// <param name="strPartType"></param>
        /// <returns></returns>
        IList<PartTypeAttribute> GetPartTypeAttributes(string strPartType);

        /// <summary>
        /// 新增PartTypeAttribute
        /// </summary>
        /// <param name="item"></param>
        void AddPartTypeAttribute(PartTypeAttribute item);

        /// <summary>
        /// 根据strPartType, strOldCode保存PartTypeAttribute
        /// </summary>
        /// <param name="item"></param>
        /// <param name="strOldPartType"></param>
        /// <param name="strOldCode"></param>
        void SavePartTypeAttribute(PartTypeAttribute item, string strOldPartType, string strOldCode);

        /// <summary>
        /// 根据strPartType, strOldCode删除PartTypeAttribute
        /// </summary>
        /// <param name="strPartType"></param>
        /// <param name="strCode"></param>
        void DeletePartTypeAttribute(string strPartType, string strCode);

        /// <summary>
        /// 新增PartTypeDesc
        /// </summary>
        /// <param name="item"></param>
        void AddPartTypeDesc(PartTypeDescription item);

        /// <summary>
        /// 保存PartTypeDesc
        /// </summary>
        /// <param name="item"></param>
        void SavePartTypeDesc(PartTypeDescription item);

        /// <summary>
        /// 删除PartTypeDesc
        /// </summary>
        /// <param name="item"></param>
        void DeletePartTypeDesc(PartTypeDescription item);

        /// <summary>
        /// 如果strDescID == "", where 子句不包含该查询条件
        /// 否则where 中包含条件 PartTypeDesciption.ID != strDescID
        /// </summary>
        /// <param name="strPartType"></param>
        /// <param name="strDesc"></param>
        /// <param name="strDescID"></param>
        /// <returns></returns>
        int CheckExistedDesc(string strPartType, string strDesc, string strDescID);

        /// <summary>
        /// 取得PartTypeMapping表的PartTypeMapping.FISType = strPartType记录
        /// 按字符序排序
        /// </summary>
        /// <param name="strPartType"></param>
        /// <returns></returns>
        IList<PartTypeMapping> GetPartTypeMappingList(string strPartType);

        /// <summary>
        /// 新增PartTypeMapping
        /// </summary>
        /// <param name="item"></param>
        void AddPartTypeMapping(PartTypeMapping item);

        /// <summary>
        /// 修改PartTypeMapping
        /// </summary>
        /// <param name="item"></param>
        void SavePartTypeMapping(PartTypeMapping item);

        /// <summary>
        /// 删除PartTypeMapping
        /// </summary>
        /// <param name="item"></param>
        void DeletePartTypeMapping(PartTypeMapping item);

        /// <summary>
        /// 如果strID == "", where 子句不包含该查询条件
        /// 否则where 中包含条件 PartTypeMapping.ID != strID
        /// </summary>
        /// <param name="strFISType"></param>
        /// <param name="strSAPType"></param>
        /// <param name="strID"></param>
        /// <returns></returns>
        int CheckExistedSAPType(string strFISType, string strSAPType, string strID);

        /// <summary>
        /// SELECT DISTINCT Descr as Family
        /// FROM IMES_GetData..Part 
        /// WHERE LEFT(PartNo, 3) = '111'
        /// AND PartType IN ('MB', 'VB', 'SB')
        /// ORDER BY Family
        /// 
        /// 现在要变成
        /// SA的Family仍然从Part表中取Description，但对应的PartNo改为以“131”开头，并且PartType只能是“MB”
        /// </summary>
        /// <returns></returns>
        IList<string> GetFamilyListForIECVersion();

        /// <summary>
        /// 从PartForbidden取得等于family的记录
        /// 栏位包括Model、Description、Part No、Assembly Code、Status、Editor、Cdt、Udt
        /// 按Model、Description、Part No、Assembly Code栏位排序
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<PartForbidden> GetPartForbiddenListByFamily(string family);

        /// <summary>
        /// 返回在PartForbidden表中等于Model、Description、PartNo和AssemblyCode的值对的数量
        /// 参考sql如下：
        /// select count(*) from PartForbidden where Model=? and Description = ? and PartNo = ? and AssemblyCode=?
        /// </summary>
        /// <param name="strModel"></param>
        /// <param name="strDescription"></param>
        /// <param name="strPartNo"></param>
        /// <param name="strAssemblyCode"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        int CheckExistedPartForbidden(string strModel, string strDescription, string strPartNo, string strAssemblyCode, string family);

        /// <summary>
        /// 新增PartForbidden
        /// </summary>
        /// <param name="item"></param>
        void AddPartForbidden(PartForbidden item);

        /// <summary>
        /// 修改PartForbidden
        /// </summary>
        /// <param name="item"></param>
        void SavePartForbidden(PartForbidden item);

        /// <summary>
        /// 删除PartForbidden
        /// </summary>
        /// <param name="?"></param>
        void DeletePartForbidden(PartForbidden item);

        /// <summary>
        /// 根据ID获得PartForbidden信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PartForbidden GetPartForbidden(int id);

        /// <summary>
        /// 参考sql：
        /// SELECT PartType, Descr FROM IMES_GetData..Part
        ///     WHERE PartNo = ? 
        ///         AND Flag <> '0'
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IPart GetPartByPartNo(string partNo);

        /// <summary>
        /// 参考sql如下：
        /// select PartNo Name from Part
        /// 按PartNo 列的字符序排序
        /// </summary>
        /// <returns></returns>
        IList<string> GetPartNoList();

        /// <summary>
        /// 参考sql如下：
        /// select Distinct InfoType from PartInfo
        /// 按InfoType列的字符序排序
        /// </summary>
        /// <returns></returns>
        IList<string> GetPartInfoList();

        #region Defered

        void AddAssemblyCodeDefered(IUnitOfWork uow, AssemblyCode Object);

        void UpdateAssemblyCodeDefered(IUnitOfWork uow, AssemblyCode Object);

        void DeleteAssemblyCodeDefered(IUnitOfWork uow, int assemblyCodeId);

        void DeletePartCheckDefered(IUnitOfWork uow, PartCheck item);

        void AddPartCheckDefered(IUnitOfWork uow, PartCheck item);

        void SavePartCheckDefered(IUnitOfWork uow, PartCheck item);

        void DeletePartCheckMatchRuleDefered(IUnitOfWork uow, PartCheckMatchRule item);

        void AddPartCheckMatchRuleDefered(IUnitOfWork uow, PartCheckMatchRule item);

        void SavePartCheckMatchRuleDefered(IUnitOfWork uow, PartCheckMatchRule item);

        void AddAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item);

        void SaveAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item);

        void DeleteAssemblyCodeInfoDefered(IUnitOfWork uow, AssemblyCodeInfo item);

        void AddRegionDefered(IUnitOfWork uow, Region region);

        void UpdateRegionDefered(IUnitOfWork uow, Region region, string regionname);

        void DeleteRegionByNameDefered(IUnitOfWork uow, string region);

        void AddPartTypeDefered(IUnitOfWork uow, PartType item);

        void SavePartTypeDefered(IUnitOfWork uow, PartType item, string strOldPartType);

        void DeletePartTypeByPartTypeDefered(IUnitOfWork uow, string partType);

        void AddPartTypeAttributeDefered(IUnitOfWork uow, PartTypeAttribute item);

        void SavePartTypeAttributeDefered(IUnitOfWork uow, PartTypeAttribute item, string strOldPartType, string strOldCode);

        void DeletePartTypeAttributeDefered(IUnitOfWork uow, string strPartType, string strCode);

        void AddPartTypeDescDefered(IUnitOfWork uow, PartTypeDescription item);

        void SavePartTypeDescDefered(IUnitOfWork uow, PartTypeDescription item);

        void DeletePartTypeDescDefered(IUnitOfWork uow, PartTypeDescription item);

        void AddPartTypeMappingDefered(IUnitOfWork uow, PartTypeMapping item);

        void SavePartTypeMappingDefered(IUnitOfWork uow, PartTypeMapping item);

        void DeletePartTypeMappingDefered(IUnitOfWork uow, PartTypeMapping item);

        void AddPartForbiddenDefered(IUnitOfWork uow, PartForbidden item);

        void SavePartForbiddenDefered(IUnitOfWork uow, PartForbidden item);

        void DeletePartForbiddenDefered(IUnitOfWork uow, PartForbidden item);

        #endregion

        #endregion

        /// <summary>
        /// 1)取得AssetCheckRuleList
        /// SELECT Code AS [Ast Type],CheckTp AS [Check Type], 
        /// CASE WHEN Station='65' THEN '65 After MVS' 
        /// WHEN Station='8C' THEN '8C Combine Pizza' 
        /// WHEN Station='85' THEN '85 Unit Weight' END AS [Check Station],
        /// CustName AS [Cust Name], CheckItem AS [Check Item],Editor,Cdt,Udt,[Id] FROM AstRule (NOLOCK) ORDER BY Code
        /// </summary>
        /// <returns></returns>
        DataTable GetAssetCheckRuleList();

        /// <summary>
        /// get all AstRule 
        /// </summary>
        /// <returns></returns>
        IList<DataModel.AstRuleInfo> GetAstRule();

        /// <summary>
        /// get all AstRule  by Condition
        /// </summary>
        /// <returns></returns>
        IList<DataModel.AstRuleInfo> GetAstRuleByCondition(DataModel.AstRuleInfo condition);

        /// <summary>
        /// insert AssetRule
        /// </summary>
        /// <param name="item"></param>
        void AddAstRule(AstRuleInfo item);
        /// <summary>
        /// update AstRue
        /// </summary>
        /// <param name="item"></param>
        void UpdateAstRule(AstRuleInfo item);
        /// <summary>
        /// delete AstRule table
        /// </summary>
        /// <param name="id"></param>
        void DeleteAstRule(int id);

        /// <summary>
        /// 2) Delete(int id)
        /// 删除一条AssetCheckRule记录
        /// </summary>
        /// <param name="id"></param>
        void DeleteAssetCheckRule(int id);

        /// <summary>
        /// 3) 取得AstType下拉列表
        /// SELECT DISTINCT [Descr]     
        ///   FROM [Part]
        /// WHERE NodeType='AT' ORDER BY [Descr]
        /// 

        ////SELECT DISTINCT Descr AS [Ast Type]
        ////FROM Part NOLOCK
        ////WHERE BomNodeType = 'AT'
        ////ORDER BY Descr

        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        IList<string> GetAstTypes(string nodeType);

        /// <summary>
        /// 4)添加一条记录
        /// 返回后item中的id已经被填充
        /// </summary>
        /// <param name="item"></param>
        void AddAssetRule(AssetRule item);

        /// <summary>
        /// 5)取得存在条件下的记录
        /// SELECT [Id]   
        ///  FROM [AssetRule]
        /// WHERE [AstType]='[AstType]' AND [Station]='[Station]' AND [CheckType]='[CheckType]' AND [CustName]='[CustName]' AND [CheckItem]='[CheckItem]'
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        DataTable ExistAssetRule(AssetRule item);

        /// <summary>
        /// 1、 取得全部battery信息 select * from OlymBattery order by HPPN
        /// </summary>
        /// <param name="batteryVC"></param>
        /// <returns></returns>
        IList<OlymBatteryInfo> GetBatteryInfoList();

        /// <summary>
        /// 2、按左端匹配原则搜索Battery数据表中HPPN栏位 select * from OlymBattery where HPPN like "'" +batteryVC+"%'" order by HPPN
        /// </summary>
        /// <param name="batteryVC"></param>
        /// <returns></returns>
        IList<OlymBatteryInfo> GetBatteryInfoList(string batteryVC);

        /// <summary>
        /// 3、 根据batteryVC取得Battery的记录数据 select * from OlymBattery where HPPN=batteryVC
        /// </summary>
        /// <param name="batteryVC"></param>
        /// <returns></returns>
        IList<OlymBatteryInfo> GetExistBattery(string batteryVC);

        /// <summary>
        /// 4、 保存一条Battery的记录数据(update) update OlymBattery　所有字段whereHPPN=oldBattery
        /// </summary>
        /// <param name="item"></param>
        /// <param name="oldBattery"></param>
        void ChangeBattery(OlymBatteryInfo item, string oldBattery);

        /// <summary>
        /// 5、Add(OlymBattery item, IUnitOfWork uow)
        /// </summary>
        /// <param name="item"></param>
        void AddBattery(OlymBatteryInfo item);

        /// <summary>
        /// 6、Remove(OlymBattery item, IUnitOfWork uow)
        /// </summary>
        /// <param name="item"></param>
        void RemoveBattery(OlymBatteryInfo item);

        /// <summary>
        /// 7、OlymBattery Find(String batteryVC)
        /// </summary>
        /// <param name="batteryVC"></param>
        /// <returns></returns>
        OlymBatteryInfo FindBattery(string batteryVC);

        /// <summary>
        /// 取得全部InternalCOA信息
        /// 按照type,code排序
        /// </summary>
        /// <returns></returns>
        IList<InternalCOAInfo> FindAllInternalCOA();

        /// <summary>
        /// 根据code取得InternalCOA的记录数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<InternalCOAInfo> GetExistInternalCOA(string code);

        /// <summary>
        /// Add InternalCOA
        /// </summary>
        /// <param name="item"></param>
        void AddInternalCOA(InternalCOAInfo item);

        /// <summary>
        /// Remove InternalCOA
        /// </summary>
        /// <param name="item"></param>
        void RemoveInternalCOA(InternalCOAInfo item);

        /// <summary>
        /// Find InternalCOA
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        InternalCOAInfo FindInternalCOA(int id);

        /// <summary>
        /// 1      取得前段起包含于PartNo字符串的PODLabelPart数据的list(按Family列的字母序排序)
        ///       select * from PODLabelPart t
        ///       where CHARINDEX(RTRIM(t.PartNo),PartNo)=1
        ///       order by family
        /// </summary>
        /// <param name="PartNo"></param>
        /// <returns></returns>
        IList<PODLabelPartDef> GetPODLabelPartListByPartNo(string partNo);

        /// <summary>
        /// 2     取得某family和包含partno字符串的PODLabelPart数据的list
        ///      select *
        ///      from PODLabelPart t
        ///      where t.Family = Family
        ///      and CHARINDEX(RTRIM(t.PartNo),PartNo) = 1
        /// </summary>
        /// <param name="PartNo"></param>
        /// <param name="Family"></param>
        /// <returns></returns>
        IList<PODLabelPartDef> GetListByPartNoAndFamily(string partNo, string family);

        /// <summary>
        /// 3      取得所有PODLabelPart数据的list(按Family列的字母序排序)
        ///       select * from PODLabelPart
        ///       order by family
        /// </summary>
        /// <returns></returns>
        IList<PODLabelPartDef> GetPODLabelPartList();

        /// <summary>
        /// SELECT * FROM dbo.PODLabelPart WHERE @Model LIKE (Rtrim(PartNo) +'%')
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<PODLabelPartDef> GetPodLabelPartListByPartNoLike(string model);

        /// <summary>
        /// 4     修改一条PODLabelPart的记录数据(update)
        ///      update PODLabelPart
        ///      set
        ///      Family = @Family,
        ///      ParNo = @PartNo,
        ///      Editor = @Editor,
        ///      Cdt = @Cdt,
        ///      Udt = @Udt
        ///      where PartNo = partNo
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="partNo"></param>
        void UpdatePODLabelPart(PODLabelPartDef obj, string partNo);

        /// <summary>
        /// 5    插入一条新的PODLabelPart数据
        ///      
        ///      insert into  PODLabelPart
        ///      values(@Family,@PartNo,@Editor,@Cdt,@Udt) 
        /// </summary>
        /// <param name="obj"></param>
        void AddPODLabelPart(PODLabelPartDef obj);

        /// <summary>
        /// 6  "删除一条PODLabelPart的记录数据
        ///     delete from PODLabelPart
        ///     where PartNo = partNo
        /// </summary>
        /// <param name="partNo"></param>
        void DeletePODLabelPart(string partNo);

        /// <summary>
        /// 6,填充TypeDescr下拉框的项:
        /// SQL:select distinct Descr from Part (nolock) where(BomNodeType='C2' or BomNodeType='P1') and GetPartInfo(“FU”)='Y'
        /// 
        /// select distinct a.Descr from Part a, PartInfo b where a.PartNo=b.PartNo and (a.BomNodeType='C2' or a.BomNodeType='P1') and b.InfoType=“FU” and b.InfoValue='Y
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllTypeDescr();

        /// <summary>
        /// 7,填充PartNo下拉框中的项:
        /// select DISTINCT CASE CHARINDEX('-',a.PartNo) WHEN 0 THEN a.PartNo ELSE LEFT(a.PartNo,CHARINDEX('-',a.PartNo)-1) END from  Part (nolock) a, PartInfo b where (a.PartNo=b.PartNo and (BomNodeType='C2' or BomNodeType='P1') and b.InfoType=“FU” and b.InfoValue='Y’ and Descr='"&trim(TypeDescr.value )&"'
        /// </summary>
        /// <param name="typeDescr"></param>
        /// <returns></returns>
        IList<string> GetPartNoByTypeDescr(string typeDescr);

        /// <summary>
        /// 8,
        /// Sql:select DISTINCT Name from station where stationtype='PAKKitting' order by name
        /// </summary>
        /// <param name="stationtype"></param>
        /// <returns></returns>
        IList<string> GetAllPAKikittingStationName(string stationtype);

        /// <summary>
        /// 1. 获取PartType表的所有记录 
        /// select * from PartType order by Code
        /// </summary>
        /// <returns></returns>
        IList<PartTypeDef> GetPartTypeDefList();

        /// <summary>
        /// 2. 根据id取出相应数据
        /// select * from PartType where ID=id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PartTypeDef GetPartTypeInfo(int id);

        /// <summary>
        /// 3.取得PartType表中的所有TP值
        /// select TP from PartType order by TP
        /// </summary>
        /// <returns></returns>
        IList<string> GetTPList();

        /// <summary>
        /// 4.根据TP取得PartType表的Code值
        /// select Code from PartType where TP=tp order by Code
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        IList<string> GetCodeListByTp(string tp);

        /// <summary>
        /// 5.添加一条记录
        /// </summary>
        /// <param name="partTypeDef"></param>
        void AddPartType(PartTypeDef partTypeDef);

        /// <summary>
        /// 6.更新指定的记录，所有字段 where ID=id
        /// </summary>
        /// <param name="item"></param>
        void UpdatePartType(PartTypeDef item);

        /// <summary>
        /// 7.删除指定Id的数据 where ID=id
        /// </summary>
        /// <param name="id"></param>
        void DeletePartType(int id);

        /// <summary>
        /// 根据PartType删除PartType记录
        /// </summary>
        /// <param name="partType"></param>
        void DeletePartType(string partType);

        /// <summary>
        /// 8.根据code取出所有PartType的记录
        /// select * from PartType where CODE=code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<PartTypeDef> GetPartTypeByCode(string code);

        /// <summary>
        /// 9.取得PartType 表中TP=tp的所有数据
        /// select * from ParType where TP=tp order by Code
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        IList<PartTypeDef> GetPartTypeListByTp(string tp);

        /// <summary>
        /// 1,SQL:select * from AssetRange order by code;
        /// </summary>
        /// <returns></returns>
        IList<AssetRangeInfo> GetAllAssetRanges();

        /// <summary>
        /// 2,SQL:insert into AssetRange values ( [Item中的各个字段...]);
        /// </summary>
        /// <param name="item"></param>
        void AddAssetRangeItem(AssetRangeInfo item);

        /// <summary>
        /// 3,SQL:根据ID,更新Item中的字段.
        /// </summary>
        /// <param name="item"></param>
        void UpdateAssetRangeItem(AssetRangeInfo item);

        /// <summary>
        /// 4,SQL:根据ID,删除记录.
        /// </summary>
        /// <param name="id"></param>
        void DeleteAssetRangeItem(int id);

        /// <summary>
        /// 1,SQL:select * from CELDATA order by ZMOD;
        /// </summary>
        /// <returns></returns>
        IList<CeldataInfo> GetAllCeldatas();

        /// <summary>
        /// 2,SQL:insert into CELDATA values ( [Item中的各个字段...]);
        /// </summary>
        /// <param name="item"></param>
        void AddCeldataItem(CeldataInfo item);

        /// <summary>
        /// 4,SQL:根据ZMOD,删除记录.
        /// </summary>
        /// <param name="ZMOD"></param>
        void DeleteCeldataItem(string zmod);

        /// <summary>
        /// 1.PartType表的数据需求
        /// 1.1从part type表获取PartNodeType
        /// select *
        /// from PartType(NOLOCK) 
        /// where Tp=tp order by Indx
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        IList<PartTypeDef> GetPartNodeType(string tp);

        /// <summary>
        /// 2.Part表的数据需求
        /// 2.1.根据part node type获取part数据列表,按照partno排序
        /// select * from part
        /// where PartType = PartType 
        /// order by partno
        /// </summary>
        /// <param name="partType"></param>
        /// <returns></returns>
        IList<PartDef> GetListByPartType(string partType);
 
        /// <summary>
        /// 2.2.修改part数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="partNo"></param>
        void UpdatePartByPartNo(PartDef obj, string partNo);

        /// <summary>
        /// 2.3.添加一条part记录
        /// </summary>
        /// <param name="obj"></param>
        void AddPart(PartDef obj);

        /// <summary>
        /// 2.4.根据partno删除一条记录
        /// </summary>
        /// <param name="partNo"></param>
        void DeletePart(string partNo);

        /// <summary>
        /// 3 DescType表的数据需求
        /// 3.1.根据type获取所有Description的数据列表
        /// select * from DescType 
        /// where TP = tp
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        IList<DescTypeInfo> GetDescriptionList(string tp);
        
        /// <summary>
        /// 4.PartInfo表的数据需求
        /// 4.1.写入PartInfo表一条数据
        /// </summary>
        /// <param name="obj"></param>
        void AddPartInfo(PartInfo obj);

        /// <summary>
        /// 4.2.根据InfoType删除PartInfo一条数据
        /// delete from PartInfo where InfoType = item
        /// </summary>
        /// <param name="infoType"></param>
        void DeletePartInfo(string infoType);

        /// <summary>
        /// 4.3.修改partinfo数据
        /// 根据partno、infoType、infoValue修改partInfo数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="partno"></param>
        void UpdatePartInfo(PartInfo obj, string partno, string infoType, string infoValue);

        /// <summary>
        /// 5.联合查询partinfo与parttype表，此类似方法(GetPartTypeAttributeAndPartInfoValueListByPartNo)在程序中已经存在：
        /// SELECT partType.Code AS Item, partType.Description AS Description, ISNULL(A.InfoValue,'') 
        /// AS Content, A.Editor AS Editor, A.Cdt AS Cdt, A.ID, A.PartNo, A.Udt 
        /// FROM (SELECT PartNo, InfoType, InfoValue, Editor, Cdt, ID, Udt 
        /// FROM PartInfo WHERE PartNo=@PartNo) AS A 
        /// RIGHT OUTER JOIN partType 
        /// ON A.InfoType=partType.Code 
        /// WHERE partType.Code=@Code
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="partType"></param>
        /// <returns></returns>
        IList<PartTypeAndPartInfoValue> GetPartTypeAndPartInfoValueListByPartNo(string partNo, string partType);

        /// <summary>
        /// 1.   添加: 根据bomnode获取part列表
        /// select * from part where BomNodeType = bomNode
        /// </summary>
        /// <param name="bomNode"></param>
        /// <returns></returns>
        IList<PartDef> GetLstByBomNode(string bomNode);

        /// <summary>
        /// 2.   添加: 根据partNo获取part列表  
        /// select * from part where partNo = partNo
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<PartDef> GetLstByPartNo(string partNo);

        /// <summary>
        /// 5    添加: 根据partno、infoType、infoValue获取partInfo数据
        /// </summary>
        /// <param name="partno"></param>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        IList<PartInfo> GetLstPartInfo(string partno,string infoType,string infoValue);

        void DeletePartInfoByPN(string partNo);

        /// <summary>
        /// 1.partinfo与part表的联合查询
        /// getCode(string family);
        /// Select distinct(b.InfoValue) from Part a, PartInfo b 
        /// where a.Descr=@family
        /// and a.PartNo=b.PartNo 
        /// and b.InfoType=’MB’ 
        /// order by InfoValue
        /// </summary>
        /// <param name="descr"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetPartInfoValueByPartDescr(string descr, string infoType);

        /// <summary>
        /// Select distinct(b.InfoValue) from Part a, PartInfo b , PartInfo c
        /// where 
        /// a.Descr=@family 
        /// and a.PartNo=b.PartNo 
        /// and b.InfoType='MB'
        /// and a.PartNo = c.PartNo 
        /// and c.InfoType = 'MAC'
        /// and c.InfoValue = 'T'
        /// order by b.InfoValue
        /// </summary>
        /// <param name="descr"></param>
        /// <param name="infoType"></param>
        /// <param name="infoType2"></param>
        /// <param name="infoValue2"></param>
        /// <returns></returns>
        IList<string> GetPartInfoValueByPartDescr(string descr, string infoType, string infoType2, string infoValue2);

        /// <summary>
        /// SELECT [Begin],[End] FROM AssetRange WHERE Code=@Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<AssetRangeInfo> GetAssetRangesByCode(string code);

        /// <summary>
        /// 7. Part类型 ： 
        /// 查找在IMES_FA..Product_Part 表中与当前Product 绑定的Parts 中存在BomNodeType为'AT' ，Descr 属性为'ATSN1' 或'ATSN4'或'ATSN7'的Part记录;
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="bomNodeType"></param>
        /// <param name="descrs"></param>
        /// <returns></returns>
        IList<PartDef> GetPartByBomNodeTypeAndDescr(string productId, string bomNodeType, string[] descrs);

        /// <summary>
        /// 8. Part类型 ： 
        /// 查找在IMES_FA..Product_Part 表中与当前Product 绑定的Parts 中存在BomNodeType为'AT' 的Part记录;
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        IList<PartDef> GetPartByBomNodeType(string productId, string bomNodeType);

        /// <summary>
        /// 11.在IMES_FA..Product_Part 表中取PartSn 以字符'C' 开头BomNodeType为'KP' 的记录的PartSn
        /// </summary>
        /// <param name="partSnPrefix"></param>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        IList<string> GetPartSnListFromProductPart(string partSnPrefix, string bomNodeType);

        IList<string> GetPartSnListFromProductPart(string partSnPrefix, string bomNodeType, string productID);

        /// <summary>
        /// 2.根据infotype获取part表与partinfo表的infoValue数据
        /// select distinct RTRIM(b.InfoValue) 
        /// from Part a, PartInfo b
        /// where a.PartNo = b.PartNo
        /// and a.BomNodeType=@Type 
        /// and b.InfoType=@Type 
        /// order by InfoValue
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<string> GetInfoValue(string type);

        /// <summary>
        /// 使用Code = @CustomerSN and Type = 'SN' 或者Code = @DeliveryNo and Type = 'DN'查询InternalCOA 表 存在记录时
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckExistInternalCOA(string code, string type);

        /// <summary>
        /// select  ID, Type, Value, Description, Editor, Cdt, Udt
       ///where Type=@Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<ConstValueTypeInfo> GetConstValueTypeList(string type);

        /// <summary>
        /// select  ID, Type, Value, Description, Editor, Cdt, Udt
        ///where Type=@Type and Value=@Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<ConstValueTypeInfo> GetConstValueTypeList(string type, string value);

        /// <summary>
        /// update ConstValueType
        /// SET  Type, Value, Description, Editor, Cdt, Udt
        ///where ID=@ID
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void UpdateConstValueType(ConstValueTypeInfo info);

        /// <summary>
        /// Insert ConstValueType(Type, Value, Description, Editor, Cdt, Udt)
        ///values(Type, Value, Description, Editor, Cdt, Udt)
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void InsertConstValueType(ConstValueTypeInfo info);


        // <summary>
        /// Insert ConstValueType(Type, Value, Description, Editor, Cdt, Udt)
        /// select @Type, Data, @Descr, @Editor, getdate(),getdate()
        ///  from @stringList
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void InsertMultiConstValueType(string type, IList<string> values, string descr, string editor );

        /// <summary>
        /// Delete ConstValueType
        /// where ID =@ID
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void RemoveConstValueType(int id);

        /// <summary>
        /// Delete ConstValueType
        /// where Type in (@Type)
        /// </summary>
        /// <param> </param>
        /// <returns></returns>
        void RemoveMultiConstValueType(string type, string value);

        /// <summary>
        /// select  distinct Type from ConstValueType        
        /// </summary>
        /// <returns></returns>
        IList<string> GetConstValueTypeList();


        /// <summary>
        /// Select EptName,Descr From FaException OA、OFT、Other、PE
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<ConstValueInfo> GetConstValueListByType(string type);

        /// <summary>
        /// 首先在IMES_FA..Product_Part 表中取PartSn 以字符'C' 开头BomNodeType 为'KP' 的记录Part
        /// </summary>
        /// <param name="partSnPrefix"></param>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        IList<ProductPart> GetProductPart(string partSnPrefix, string bomNodeType);

        IList<ProductPart> GetProductPart(string partSnPrefix, string bomNodeType, string prodId);

        /// <summary>
        /// 1. Update [ForceNWC], Station为returnstation 
        /// Update [ForceNWC] 
        /// [ForceNWC]= ReturnWC# , [PreStation]= '45'
        /// Where [ProductID]= ProductID# 
        /// </summary>
        /// <param name="forceNwc"></param>
        /// <param name="preStation"></param>
        /// <param name="productId"></param>
        void UpdateForceNWCByProductID(string forceNwc, string preStation, string productId);

        /// <summary>
        /// 实现功能：返回KeyPartType列表（select Name from IMES2012_GetData..ConstValue where Type='ChangeKP'）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<string> GetConstValueNameListByType(string type);

        /// <summary>
        /// Update [ForceNWC] SET [ForceNWC]= ReturnWC# ,[PreStation]= 'RK'Where [ProductID]= ProductID#
        /// setValue哪个字段赋值就有更新哪个字段
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateForceNWC(ForceNWCInfo setValue, ForceNWCInfo condition);

        void InsertForceNWC(ForceNWCInfo item);

        /// <summary>
        /// SQL：select Value  FROM SysSetting
        /// where name=’SITECODE’;
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<string> GetValueFromSysSettingByName(string name);

        /// <summary>
        /// ex: Select Name,Value From ConstValue WHERE Type=’FaException’ 
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.   
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<ConstValueInfo> GetConstValueInfoList(ConstValueInfo condition);

        /// <summary>
        /// Select distinct Type fromConstValue order by Type
        /// </summary>
        /// <returns></returns>
        IList<string> GetTypeListFromConstValue();

        /// <summary>
        /// SELECT  *  FROM  ConstValue  Where  Type = '[Type]'  And  Name<>''  order by Name
        /// </summary>
        /// <param name="eqCondition"></param>
        /// <param name="neqCondition"></param>
        /// <returns></returns>
        IList<ConstValueInfo> GetConstValueListByType(ConstValueInfo eqCondition, ConstValueInfo neqCondition);

        /// <summary>
        /// //向ConstValue表增加记录
        /// </summary>
        /// <param name="obj"></param>
        void AddConstValue(ConstValueInfo item);
     
        /// <summary>
        /// 更新ConstValue表某条记录
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateConstValue(ConstValueInfo setValue, ConstValueInfo condition);

        /// <summary>
        /// 删除ConstValue数据
        /// </summary>
        /// <param name="condition"></param>
        void RemoveConstValue(ConstValueInfo condition);

        /// <summary>
        /// Part数据表中Part.PartNo(Part.BomNodeType='P1'  and  Part.Descr like 'CommonParts%')
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="descrPrefix"></param>
        /// <returns></returns>
        IList<IPart> GetPartsByBomNodeTypeAndLikeDescr(string bomNodeType, string descrPrefix);

        /// <summary>
        /// select InfoValue from PartInfo where PartNo = @Pno and InfoType=@InfoType
        /// </summary>
        /// <param name="partno"></param>
        /// <param name="infotype"></param>
        /// <returns></returns>
        string GetPartInfoValue(string partno, string infotype);

        /// <summary>
        /// 判断[ForceNWC]表是否有指定ProductID的记录的接口
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.   
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool CheckExistForceNWC(ForceNWCInfo condition);

        IList<ForceNWCInfo> GetForceNWCListByCondition(ForceNWCInfo condition);

        /// <summary>
        /// Check by SP
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="prodId"></param>
        /// <param name="checkItem"></param>
        /// <returns></returns>
        int CheckBySP(string spName, string prodId, string checkItem);

        void AddSysSettingInfo(SysSettingInfo item);

        void UpdateSysSettingInfo(SysSettingInfo setValue, SysSettingInfo condition);

        /// <summary>
        /// 1.加载cmbQryFamily 数据:
        /// select distinct Descr from Part nolock where BomNodeType = 'MB' order by Descr
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        IList<string> GetDescrListFromPartByBomNodeType(string bomNodeType);

        /// <summary>
        /// SELECT Value + ' ' + Description as [CauseItem] 
        /// FROM ConstValue 
        /// WHERE Type=@type AND LEFT(Value, 1) LIKE '[0-9]' AND Description <> ''
        /// ORDER BY Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<string> GetCauseItemListByType(string type);

        /// <summary>
        /// select COUNT(*) from KeyPartRepair where ProductID='<ctno>' and Status=<status>
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCountOfKpRepairCount(KeyPartRepairInfo condition);

        /// <summary>
        /// select * from KeyPartRepair_DefectInfo where KeyPartRepairID in (select ID from KeyPartRepair where ProductID='<ctno>' and Status=0)
        /// </summary>
        /// <param name="ctno"></param>
        /// <returns></returns>
        IList<RepairInfo> GetKPRepairDefectList(string ctno);

        /// <summary>
        /// 在KeyPartRepair表中添加一条记录，返回其ID
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void AddKPRepair(KeyPartRepairInfo item);

        /// <summary>
        /// 在KeyPartRepair_DefectInfo表中添加一条记录
        /// </summary>
        /// <param name="item"></param>
        void AddKPRepairDefect(RepairDefect item);

        /// <summary>
        /// 更新KeyPartRepair_DefectInfo表中的记录
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateKPRepairDefect(RepairDefect setValue, RepairDefect condition);

        /// <summary>
        /// 对于KeyPartRepair表中记录，更新其值，同时更新记录的UDT
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateKPRepair(KeyPartRepairInfo setValue, KeyPartRepairInfo condition);

        /// <summary>
        /// select COUNT(*) from KeyPartRepair a(nolock), KeyPartRepair_DefectInfo b(nolock) where a.ID = b.KeyPartRepairID and a.ProductID='<ctno>' and isnull(Cause,'') = ''
        /// count=0返回true
        /// count>0返回false
        /// </summary>
        /// <param name="ctno"></param>
        /// <returns></returns>
        bool CheckIfKPRepairFinished(string ctno);

        /// <summary>
        /// select * from KeyPartRepair where ProductID='…' and Status=… ...order by Udt desc
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<KeyPartRepairInfo> GetKPRepairList(KeyPartRepairInfo condition);

        /// <summary>
        /// 输入参数：string[] Pnlist， @InfoType
        /// SELECT DISTINCT  InfoValue FROM PartInfo (nolock)
        ///       WHERE PartNo in (Pnlist)
        ///       AND InfoType = 'VendorCode'
        ///       ORDER BY InfoValue
        /// </summary>
        /// <param name="pnlist"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetInfoValueList(string[] pnlist, string infoType);

        /// <summary>
        /// select a.InfoValue as IECPN from PartInfo a, PartInfo b
        /// where a.PartNo = b.PartNo 
        /// and a.InfoType in ('RDESC','RRRDESC','RRRRRDESC')
        /// and b.InfoValue = left('[CTNo]',5)
        /// </summary>
        /// <param name="infoTypes"></param>
        /// <param name="ctno"></param>
        /// <returns></returns>
        IList<string> GetPartInfoValueByInfoTypesAndInfoValuePrefix(string[] infoTypes, string ctno);

        /// <summary>
        /// 1、查询PartInfo
        /// PartInfo.PartNo = Part.PartNo and PartInfo.InfoValue=Left([CT No],5)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PartInfo> GetPartInfoList(PartInfo condition);

        /// <summary>
        /// select * from AssetRange
        /// where [Begin]<=@begin and [End]>= @end and ID != @id and LEN(Begin)=LEN(@BeginNo)
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<AssetRangeInfo> GetAssetRangeInfoByRangeAndId(string begin, string end, int id);

        /// <summary>
        /// select * from  AssetRange 
        /// where [Begin]>= @BeginNo and [End]<= @EndNo and ID != @id and LEN(Begin)=LEN(@BeginNo)
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<AssetRangeInfo> GetAssetRangeInfoByRangeAndIdReversely(string begin, string end, int id);

        IList<SysSettingInfo> GetSysSettingInfoes(SysSettingInfo condition);

        #region Defered

        void DeleteAssetCheckRuleDefered(IUnitOfWork uow, int id);

        void AddAssetRuleDefered(IUnitOfWork uow, AssetRule item);

        void ChangeBatteryDefered(IUnitOfWork uow, OlymBatteryInfo item, string oldBattery);

        void AddBatteryDefered(IUnitOfWork uow, OlymBatteryInfo item);

        void RemoveBatteryDefered(IUnitOfWork uow, OlymBatteryInfo item);

        void AddInternalCOADefered(IUnitOfWork uow, InternalCOAInfo item);

        void RemoveInternalCOADefered(IUnitOfWork uow, InternalCOAInfo item);

        void UpdatePODLabelPartDefered(IUnitOfWork uow, PODLabelPartDef obj, string partNo);

        void AddPODLabelPartDefered(IUnitOfWork uow, PODLabelPartDef obj);

        void DeletePODLabelPartDefered(IUnitOfWork uow, string partNo);

        void AddPartTypeDefered(IUnitOfWork uow, PartTypeDef partTypeDef);

        void UpdatePartTypeDefered(IUnitOfWork uow, PartTypeDef item);

        void DeletePartTypeDefered(IUnitOfWork uow, int id);

        void DeletePartTypeDefered(IUnitOfWork uow, string partType);

        void AddAssetRangeItemDefered(IUnitOfWork uow, AssetRangeInfo item);

        void UpdateAssetRangeItemDefered(IUnitOfWork uow, AssetRangeInfo item);

        void DeleteAssetRangeItemDefered(IUnitOfWork uow, int id);

        void UpdatePartByPartNoDefered(IUnitOfWork uow, PartDef obj, string partNo);

        void AddPartDefered(IUnitOfWork uow, PartDef obj);

        void DeletePartDefered(IUnitOfWork uow, string partNo);

        void AddPartInfoDefered(IUnitOfWork uow, PartInfo obj);

        void DeletePartInfoDefered(IUnitOfWork uow, string infoType);

        void UpdatePartInfoDefered(IUnitOfWork uow, PartInfo obj, string partno, string infoType, string infoValue);

        void DeletePartInfoByPNDefered(IUnitOfWork uow, string partNo);

        void UpdateForceNWCByProductIDDefered(IUnitOfWork uow, string forceNwc, string preStation, string productId);

        void UpdateForceNWCDefered(IUnitOfWork uow, ForceNWCInfo setValue, ForceNWCInfo condition);

        void InsertForceNWCDefered(IUnitOfWork uow, ForceNWCInfo item);

        void AddSysSettingInfoDefered(IUnitOfWork uow, SysSettingInfo item);

        void UpdateSysSettingInfoDefered(IUnitOfWork uow, SysSettingInfo setValue, SysSettingInfo condition);

        void AddKPRepairDefered(IUnitOfWork uow, KeyPartRepairInfo item);

        void AddKPRepairDefectDefered(IUnitOfWork uow, RepairDefect item);

        void UpdateKPRepairDefectDefered(IUnitOfWork uow, RepairDefect setValue, RepairDefect condition);

        void UpdateKPRepairDefered(IUnitOfWork uow, KeyPartRepairInfo setValue, KeyPartRepairInfo condition);

        void AddConstValueDefered(IUnitOfWork uow, ConstValueInfo item);

        void UpdateConstValueDefered(IUnitOfWork uow, ConstValueInfo setValue, ConstValueInfo condition);

        void RemoveConstValueDefered(IUnitOfWork uow, ConstValueInfo condition);

        #endregion


        #region for FailKPCollection and another function
        void AddFailKPCollection(FailKPCollectionInfo item);
        void UpdateFailKPCollection(FailKPCollectionInfo item);
        void DeleteFailKPCollection(int ID);
        IList<FailKPCollectionInfo> GetFailKPCollection(DateTime date, string pdLine);
        IList<int> ExistInFailKPCollection(FailKPCollectionInfo item);

        IList<AssetRangeCodeInfo> GetDuplicateAssetRange(string code, string begin, string end);
        int GetAssetRangeLength(string code);


        IList<AssetRangeInfo> GetAssetRangeByCode(string code);
        IList<string> GetCodeListInAssetRange();
        AssetRangeCodeInfo GetAssetRangeByStatus(string code,string[] status);
        void SetAssetRangeStatus(int id, string status);
        void SetAssetRangeStatusDefered(IUnitOfWork uow, int id,string status);

        void DeleteSysSettingInfo(int id);

        IList<CheckItemTypeInfo> GetCheckItemType();
        IList<CheckItemTypeInfo> GetCheckItemType(CheckItemTypeInfo condition);
        void UpdateCheckItemType(CheckItemTypeInfo item);
        void InsertCheckItemType(CheckItemTypeInfo item);
        void DeleteCheckItemType(string name); 

        #endregion

        #region for AssemblyVC
        IList<AssemblyVCInfo> GetAssemblyVC(AssemblyVCInfo condition);
        void UpdateAssemblyVC(AssemblyVCInfo item);
        void InsertAssemblyVC(AssemblyVCInfo item);
        void DeleteAssemblyVC(long id); 
        #endregion

		#region PartForbid
        IList<PartForbidRuleInfo> GetPartForbid(PartForbidRuleInfo condition);
        void AddPartForbid(PartForbidRuleInfo item);
        void UpdatePartForbid(PartForbidRuleInfo item);
        void DeletePartForbid(long id);
        IList<PartForbidPriorityInfo> GetPartForbidPriority(string customer, string line, string family, string model, string productId, string status);
        IList<PartForbidPriorityInfo> GetPartForbidWithFirstPriority(string customer, string pdLine, string family, string model, string productId);
        bool CheckPartForbid(IList<PartForbidPriorityInfo> partForbidList,
                                                 string bomNodeType,
                                                 string vendorCode,
                                                 string partNo,
                                                 out string noticeMsg);
        #endregion

        #region get multi-part
        IList<IPart> FindPart(IList<string> partNoList);
        #endregion

        /// <summary>
        /// for Remove Cache item 
        /// </summary>
        /// <param name="partNoList"></param>
        void RemoveCacheByKeyList(IList<string> partNoList);

        #region for maintain Part
        void CopyPart(string scrPartNo, string destPartNo, int flag, string editor);
        void CopyPartlDefered(IUnitOfWork uow, string scrPartNo, string destPartNo, int flag, string editor);
        #endregion
    }
}
