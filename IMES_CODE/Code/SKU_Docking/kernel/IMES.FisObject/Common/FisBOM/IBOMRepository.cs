using IMES.FisObject.Common.Part;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using System;

namespace IMES.FisObject.Common.FisBOM
{
    public interface IBOMRepository
    {
        IFlatBOM GetModelFlatBOMByStationModel(string customer, string station, string line, string family, string model, object mainObj);
        //IFlatBOM GetMoFlatBOMByStation(string station, string line, string mo);
        //IHierarchicalBOM GetHierarchicalBOMByMo(string mo);
        IHierarchicalBOM GetHierarchicalBOMByModel(string model);
 
        IFlatBOM GetFlatBOMByPartTypeModel(string partType, string model, string station, object mainObj);

        /// <summary>
        /// �ɸ�����part�õ���BOM�ڵ�
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        IList<IBOMNode> GetParentBomNode(string pn);

        /// <summary>
        /// �ɸ�����pnList�õ�BomNodeType=bomNodeType�ĸ�BOM�ڵ�
        /// </summary>
        /// <returns></returns>
        IList<IBOMNode> GetParentBomNodeByPnListAndBomNodeType(IList<string> pnList,string bomNodeType);

        ///// <summary>
        ///// ͨ��mo��ȡbom
        ///// </summary>
        ///// <param name="mo"></param>
        ///// <returns></returns>
        //BOM GetBOM(string mo);

        /////<summary>
        ///// ͨ��mo��station��ȡbom��ע�⣺��Ҫ����PartCheckSetting���й��ˣ�MOBOM�ӻ����л�ȡ
        /////</summary>
        /////<param name="mo"></param>
        /////<param name="station"></param>
        /////<returns></returns>
        //BOM GetBOMByStation(string mo, string station);

        ///// <summary>
        ///// ��ȡModelBOM����BOM��������BOM��������PartCheckSetting���ˣ� ʹ��spʵ��չBOM
        ///// </summary>
        ///// <param name="model">model</param>
        ///// <returns>ModelBOM</returns>
        //BOM GetModelBOM(string model);

        ///// <summary>
        ///// ��ȡModelBOM����BOM����PartCheckSetting���ˣ� ʹ��spʵ��չBOM
        ///// </summary>
        ///// <param name="model">model</param>
        ///// <param name="station">station</param>
        ///// <returns>ModelBOM</returns>
        //BOM GetModelBOM(string model, string station);

        ///// <summary>
        ///// ��ȡָ��ModelBOM��ָ��Part���͵�����Pn
        ///// </summary>
        ///// <param name="model">model</param>
        ///// <param name="partType">partType</param>
        ///// <returns></returns>
        //IList<string> GetPnFromModelBOMByType(string model, string partType);

        ///// <summary>
        ///// ��ȡָ��ModelBOM��ָ��Part���͵�����Pn
        ///// </summary>
        ///// <param name="mo">mo</param>
        ///// <param name="partType">partType</param>
        ///// <returns></returns>
        //IList<string> GetPnFromMoBOMByType(string mo, string partType);

        ///// <summary>
        ///// ��ȡָ��MOBOM��ָ�����͵���Part.Descr����ָ���ַ�����Pn�б�
        ///// select MoBOM.PartNo from MoBOM inner join Part On MoBOM.PartNo = Part.PartNo where MO=? and PartType=? and Descr like '%?%'
        ///// </summary>
        ///// <param name="mo">mo</param>
        ///// <param name="partType">partType</param>
        ///// <param name="descrCondition">Part.Descr������ָ���ַ���</param>
        ///// <returns></returns>
        //IList<string> GetPnFromMoBOMByTypeAndDescrCondition(string mo, string partType, string descrCondition);

        ///// <summary>
        ///// ModelBOM ���Ƿ����1397 �׵��Ͻ�Part��������ڷ���part no�����򷵻ؿ�
        ///// </summary>
        ///// <param name="_111PN"></param>
        ///// <returns></returns>
        //string Get1397NO(string _111PN);

        ///// <summary>
        ///// ���1397�б�
        ///// </summary>
        ///// <param name="_111PN"></param>
        ///// <returns></returns>
        //IList<string> Get1397NOList(string _111PN);

        ///// <summary>
        ///// ͨ��MoBOM��Ϣ�ҵ�Family
        ///// </summary>
        ///// <param name="partNum"></param>
        ///// <returns></returns>
        //Family GetFirstFamilyViaMoBOM(string partNum);

        ///// <summary>
        ///// select Descr,PartNo from IMES_GetData..Part where PartNo IN (select Component from IMES_GetData..ModelBOM where Material=''and Flag=1) order by PartNo,Descr
        ///// </summary>
        ///// <param name="material"></param>
        ///// <returns></returns>
        //DataTable GetPartsViaModelBOM(string material);

        #region PartCheckSetting

        /////<summary>
        ///// ��ȡָ��Customer, model��PartCheckSetting�� ��������վ��Ҫcheck��Part, ����ά�޵�վ
        ///// </summary>
        /////<param name="customer"></param>
        /////<param name="model"></param>
        /////<returns></returns>
        //List<BOMPartCheckSetting> GetPartCheckSetting(string customer, string model);

        /////<summary>
        ///// ��ȡָ��Customer, model��ָ��վ��ҪCheck��Part, ���ڼ���վ
        /////</summary>
        /////<param name="customer"></param>
        /////<param name="model"></param>
        /////<param name="wc"></param>
        /////<returns></returns>
        //List<BOMPartCheckSetting> GetPartCheckSetting(string customer, string model, string wc);

        //void LoadAllPartCheckSetting();

        #endregion

        #region For Maintain

        /////// <summary>
        /////// ����mo���MOBOM�б�
        /////// </summary>
        /////// <param name="mo"></param>
        /////// <returns></returns>
        ////IList<MOBOM> GetMOBOMList(string mo);

        //////IList<MOBOM> GetMOBOMList(string mo, string deviation);

        //////IList<MOBOM> GetMOBOMList(string mo, string deviation, string action);

        /////// <summary>
        /////// ����MO,group��Devilation=1��ѯMOBOM����ȡ��MOBOMList
        /////// </summary>
        /////// <param name="mo"></param>
        /////// <param name="deviation"></param>
        /////// <param name="group"></param>
        /////// <param name="exceptMOBOMId"></param>
        /////// <returns></returns>
        ////IList<MOBOM> GetMOBOMListByGroup(string mo, bool deviation, int group, int exceptMOBOMId);

        /////// <summary>
        /////// ȡ��һ��MOBOM�ļ�¼����
        /////// </summary>
        /////// <param name="id"></param>
        /////// <returns></returns>
        ////MOBOM GetMOBOM(int id);

        ///// <summary>
        ///// �����ͬһMO, Devilation, Part No��MOBOM������
        ///// </summary>
        ///// <param name="mo"></param>
        ///// <param name="deviation"></param>
        ///// <param name="partNo"></param>
        ///// <returns></returns>
        //int CheckMOBOMQty(string mo, bool deviation, string partNo);

        ///// <summary>
        ///// �����ͬһMO, Devilation��MOBOM������
        ///// </summary>
        ///// <param name="mo"></param>
        ///// <param name="deviation"></param>
        ///// <returns></returns>
        //int CheckMOBOMQty(string mo, bool deviation);

        /////// <summary>
        /////// ����MOBOM
        /////// </summary>
        /////// <param name="Object"></param>
        /////// <returns></returns>
        ////int AddMOBOM(MOBOM Object);

        /////// <summary>
        /////// ����MOBOM
        /////// </summary>
        /////// <param name="Object"></param>
        /////// <param name="oldMo"></param>
        ////void UpdateMOBOM(MOBOM Object, string oldMo);

        /////// <summary>
        /////// ɾ��MOBOM
        /////// </summary>
        /////// <param name="item"></param>
        ////void DeleteMOBOM(MOBOM item);

        /////// <summary>
        /////// ���Ƶ�ǰMO�����м�¼�������ǵ�Devilation��λ����Ϊ0
        /////// </summary>
        /////// <param name="mo"></param>
        ////void CopyMOBOM(string mo);

        /////// <summary>
        /////// ����ǰ��ѡ��¼�Ķ�ӦDevilation����0�ļ�¼��Action��λ����Ϊ"DELETE"
        /////// </summary>
        /////// <param name="mo"></param>
        /////// <param name="partNo"></param>
        /////// <param name="deviation"></param>
        ////void UpdateMOBOMForDeleteAction(string mo, string partNo, bool deviation);

        /////// <summary>
        /////// ���ModelBOM
        /////// </summary>
        /////// <param name="modelBOMId"></param>
        /////// <returns></returns>
        ////ModelBOM GetModelBOM(int modelBOMId);

        /////// <summary>
        /////// ����ModelBOM
        /////// </summary>
        /////// <param name="item"></param>
        ////void UpdateModelBOM(ModelBOM item);

        ////void ChangeModelBOM(ModelBOM item, int oldId);

        ///// <summary>
        ///// ʵ�ֵ�SQL   UPDATE ModelBOM SET Alternative_item_group=����+value+���� where Material='" + parent + "' and Component='" + code + "'";
        ///// </summary>
        ///// <param name="value"></param>
        ///// <param name="parent"></param>
        ///// <param name="code"></param>
        //void IncludeItemToAlternativeItemGroup(string value, string parent, string code);

        /////// <summary>
        /////// ����ModelBOM
        /////// </summary>
        /////// <param name="item"></param>
        ////void AddModelBOM(ModelBOM item);

        /////// <summary>
        /////// ɾ��ModelBOM
        /////// </summary>
        /////// <param name="parentCode"></param>
        /////// <param name="code"></param>
        ////void DeleteModelBOMByCode(string parentCode, string code);

        //////   ʵ��AlternativeItemGroup��ͬ������
        //////   ��ʵ�ֵ�SQL���
        //////           string strSql = "UPDATE ModelBOM SET alternative_item_group=(SELECT alternative_item_group 
        //////from ModelBOM where Material='" + parent + "' and Component='" + code1 + "') where Material='" + parent 
        //////+ "' and Component in ";
        //////            strSql += "(SELECT Component from ModelBOM where Material='" + parent + "' and 
        //////Alternative_item_group = (select distinct Alternative_item_group from ModelBOM where Material='" + 
        //////parent + "' and Component='" + code2 + "'))";
        ////void IncludeAllItemToAlternativeItemGroup(string parent, string code1, string code2);

        ///// <summary>
        ///// SELECT Component from ModelBOM where Material='code' and Alternative_item_group = 'itemGroup'
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="itemGroup"></param>
        ///// <returns></returns>
        //IList<string> GetModelBOMByMaterialAndAlternativeItemGroup(string code, string itemGroup);

        ///// <summary>
        ///// UPDATE ModelBOM SET Alternative_item_group='itemGroup1' where Material='code1' and Component in 
        ///// (SELECT Component from ModelBOM where Material='code2' and 
        ///// Alternative_item_group = 'itemGroup2')
        ///// </summary>
        ///// <param name="code1"></param>
        ///// <param name="code2"></param>
        ///// <param name="itemGroup1"></param>
        ///// <param name="itemGroup2"></param>
        //void IncludeAllItemToAlternativeItemGroup(string code1, string code2, string itemGroup1, string itemGroup2);

        ///// <summary>
        ///// ʵ��AlternativeItemGroup��ͬ������
        /////   ��ʵ�ֵ�SQL���
        /////            string strSql = "UPDATE ModelBOM SET Alternative_item_group=(SELECT max(CONVERT(int, 
        /////  Alternative_item_group)) + 1 from ModelBOM) where Material='" + parent + "' and Component='" + code + ";
        ///// </summary>
        ///// <param name="parent"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //string ExcludeAlternativeItem(string parent, string code);

        ///// <summary>
        ///// ɾ����Ԫ��
        ///// string delSql = "DELETE FROM ModelBOM where Material='" + code + "'";
        ///// </summary>
        ///// <param name="code"></param>
        //void DeleteSubModelByCode(string code);

        ///// <summary>
        ///// SELECT distinct Material_group, flag=1 FROM ModelBOM  
        ///// union 
        ///// SELECT DISTINCT PartType , flag=0 
        ///// FROM Part a 
        ///// left join (SELECT DISTINCT Material_group FROM ModelBOM) as b 
        ///// on a.PartType=b.Material_group
        ///// WHERE b.Material_group is null
        ///// order by Material_group
        ///// </summary>
        ///// <returns></returns>
        //DataTable GetModelBOMTypes();

        ///// <summary>
        ///// string strSql = "SELECT distinct Material, BOMApproveDate FROM ModelBOM mb left JOIN Model 
        ///// on (mb.Material = Model.Model) where Material_group='" + parentCode + "'";
        ///// if (match != "")
        ///// {
        /////   strSql += " and Material like '%" + match + "%'";
        ///// }
        ///// strSql += " order by Material";
        ///// </summary>
        ///// <param name="parentCode"></param>
        ///// <param name="match"></param>
        ///// <returns></returns>
        //DataTable GetModelBOMCodes(string parentCode, string match);

        ///// <summary>
        ///// string strSql = "SELECT distinct(Material), Material_group, Component FROM ModelBOM where Component='" + code + "'";
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetParentModelBOMByCode(string code);

        ///// <summary>
        ///// SELECT A.Component AS Material, B.Material_group, A.Quantity, A.Alternative_item_group FROM ModelBOM AS A
        ///// INNER JOIN (
        ///// SELECT DISTINCT Material, Material_group FROM ModelBOM 
        ///// UNION 
        ///// SELECT DISTINCT PartNo AS Material, PartType AS Material_group FROM Part
        ///// LEFT OUTER JOIN (SELECT DISTINCT Material FROM ModelBOM) AS C
        ///// ON Part.PartNo=C.Material WHERE C.Material IS NULL
        ///// ) AS B ON A.Component=B.Material WHERE A.Material='code' ORDER BY A.Material
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetSubModelBOMByCode(string code);

        ///// <summary>
        ///// string strSql = "SELECT distinct Component as Material from ModelBOM where Material='" + code + "'";
        ///// strSql += " and Alternative_item_group='" + alternativeItemGroup + "'";
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="alternativeItemGroup"></param>
        ///// <returns></returns>
        //DataTable GetAlternativeItems(string code, string alternativeItemGroup);

        ///// <summary>
        ///// string strSql = "SELECT distinct(Material), AssemblyCode FROM ModelBOM  left join MO on 
        ///// ModelBOM.Material=MO.Model ";
        ///// strSql += "left join MoBOM on MO.MO=MoBOM.MO ";
        ///// strSql += "where Material in (select Component from ModelBOM where Material='" + code + "')";
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetOffspringModelBOM(string code);

        ///// <summary>
        ///// string strSql = "SELECT distinct(MO), Qty, PrintQty as StartQty, Udt FROM MO where Model='" + model + '";
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //DataTable GetMoBOMByModel(string model);

        ///// <summary>
        ///// strSql = "SELECT Material from ModelBOM where Material = '" + code + "' ";
        ///// strSql += "UNION SELECT PartNo as Material from Part where PartNo='" + code + "'";
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetMaterialsByCode(string code);

        //////string strSql = "SELECT Material FROM Model where Model='" + code + "'";
        ////DataTable GetMaterialByModel(string code);

        ///// <summary>
        ///// string strSql = "SELECT Material,Material_group FROM ModelBOM where Material='" + code + "'";
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetMaterialById(string code);

        ///// <summary>
        ///// strSql = "SELECT Model FROM Model where Model='" + code + "'";
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetModelById(string code);

        ////strSql = "SELECT Component, Quantity FROM ModelBOM where Material='" + model + "'";
        //DataTable GetComponentQuantityByMaterial(string code);

        ///// <summary>
        ///// SELECT Material_group FROM  ModelBOM WHERE Material = 'code' 
        ///// UNION
        ///// SELECT PartType FROM  Part  WHERE PartNo not in (SELECT Material FROM ModelBOM ) AND  PartNo ='code'
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetTypeOfCode(string code);

        ///// <summary>
        ///// SELECT Alternative_item_group from ModelBOM where Material='" + parent + "' and Component='" + code + "'
        ///// </summary>
        ///// <param name="parent"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetAlternativeItemGroupBySpecial(string parent, string code);

        ///// <summary>
        ///// "SELECT distinct Component from ModelBOM where Material='" + code + "'";
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetComponentByMaterial(string code);

        ///// <summary>
        ///// SELECT [PartNo]  FROM [Part] where [PartNo]='code'
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetPartById(string code);

        ///// <summary>
        ///// SELECT [PartNo] FROM [Part] where [PartType] ='type' and PartNo like '%" + match + "%'";
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="match"></param>
        ///// <returns></returns>
        //DataTable GetPartNoByType(string type, string match);

        ///// <summary>
        ///// ��MOBOM��ȡ������Groupֵ
        ///// </summary>
        ///// <returns></returns>
        //int getMaxGroupNo();

        ///// <summary>
        ///// ��MOBOM������mobomId��Ӧ��¼��Groupֵ
        ///// </summary>
        ///// <param name="mobomId"></param>
        ///// <param name="mo"></param>
        ///// <param name="group"></param>
        //void saveGroupNo(int mobomId, string mo, int group);

        ///// <summary>
        ///// ����MOɾ��MOBOM
        ///// </summary>
        ///// <param name="mo"></param>
        //void DeleteMOBOMByMo(string mo);

        ///// <summary>
        ///// ���ָ��MO��MOBOM�е�������ʱ��
        ///// </summary>
        ///// <param name="mo"></param>
        ///// <returns></returns>
        //DateTime getMaxUdt(string mo);

        /////// <summary>
        /////// Select * from ModelBom where Material='" + parentCode + "' and Component='" + oldCode + "'
        /////// </summary>
        /////// <param name="parentCode"></param>
        /////// <param name="oldCode"></param>
        /////// <returns></returns>
        ////IList<ModelBOM> findModelBomByMaterialAndComponent(string parentCode, string oldCode);

        ///// <summary>
        ///// UPDATE ModelBOM SET Alternative_item_group=NULL where Material='" + parent + "' and Component='" + code + "'
        ///// </summary>
        ///// <param name="parent"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //int ExcludeAlternativeItemToNull(string parent, string code);

        ///// <summary>
        ///// ���model=="", [Model]='model'�������[Model]='model' OR [Model] IS NULL��������
        ///// SELECT [Station],[PartType],[ID],[Customer],[Model]
        /////   FROM [IMES_GetData_Datamaintain].[dbo].[PartCheckSetting]
        /////  where [Customer]='customer' AND [Model]='model'
        ///// order by [Station],[PartType]
        ///// </summary>
        ///// <param name="customer"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //DataTable GetPartCheckSettingList(string customer, string model);

        ///// <summary>
        ///// ����PartCheckSetting
        ///// model=""ʱ����null
        ///// </summary>
        ///// <param name="partCheckSetting"></param>
        //void AddPartCheckSetting(PartCheckSetting partCheckSetting);

        ///// <summary>
        ///// ����PartCheckSetting
        ///// model=""ʱ����null
        ///// </summary>
        ///// <param name="partCheckSetting"></param>
        //void SavePartCheckSetting(PartCheckSetting partCheckSetting);

        ///// <summary>
        ///// ɾ��PartCheckSetting
        ///// </summary>
        ///// <param name="partCheckSetting"></param>
        //void DeletePartCheckSetting(PartCheckSetting partCheckSetting);

        ///// <summary>
        ///// �ж�Station��Part Type��ֵ���Ѿ���Part Check List������Part Typeʹ��
        ///// ���model=="", [Model]='model'������Ϊ[Model]='model' OR  [Model] IS NULL��������
        ///// ���station=="", [Station]='station' ������Ϊ[Station]='station'  OR  [Station] IS NULL��������
        ///// ���partType=="", [PartType]='partType'������Ϊ[PartType]='partType' OR  [PartType] IS NULL��������
        ///// ���valueType=="", [ValueType]='valueType'������Ϊ[ValueType]='valueType' OR  [ValueType] IS NULL��������
        ///// SELECT [ID] FROM [IMES_GetData].[dbo].[PartCheckSetting]
        ///// WHERE [Customer]='customer' AND [Model]='model'
        ///// AND [Station]='station' AND [PartType]='partType' AND ValueType='valueType'
        ///// AND ID<>'id'
        ///// </summary>
        ///// <param name="customer"></param>
        ///// <param name="model"></param>
        ///// <param name="station"></param>
        ///// <param name="partType"></param>
        ///// <param name="valueType"></param>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //DataTable GetExistPartCheckSetting(string customer, string model, string station, string partType, string valueType, int id);

        ///// <summary>
        ///// �ж�Station��Part Type��ֵ���Ѿ���Part Check List������Part Typeʹ��
        ///// ���model=="", [Model]='model'������Ϊ[Model]='model' OR  [Model] IS NULL��������
        ///// ���station=="", [Station]='station' ������Ϊ[Station]='station'  OR  [Station] IS NULL��������
        ///// ���partType=="", [PartType]='partType'������Ϊ[PartType]='partType' OR  [PartType] IS NULL��������
        ///// ���valueType=="", [ValueType]='valueType'������Ϊ[ValueType]='valueType' OR  [ValueType] IS NULL��������
        ///// SELECT [ID] FROM [IMES_GetData].[dbo].[PartCheckSetting]
        ///// WHERE [Customer]='customer' AND [Model]='model'
        ///// AND [Station]='station' AND [PartType]='partType' AND ValueType='valueType'
        ///// </summary>
        ///// <param name="customer"></param>
        ///// <param name="model"></param>
        ///// <param name="station"></param>
        ///// <param name="partType"></param>
        ///// <param name="valueType"></param>
        ///// <returns></returns>
        //DataTable GetExistPartCheckSetting(string customer, string model, string station, string partType, string valueType);

        ///// <summary>
        ///// ��ID���PartCheckSetting
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //PartCheckSetting FindPartCheckSettingById(int id);

        ///// <summary>
        ///// SELECT [Model] AS PartNo,'' AS Descr,
        /////        0 AS WHERETYPE
        /////       ,[BOMApproveDate] AS [BOMApproveDate]
        /////       ,[Editor]
        /////       ,[Cdt]
        /////       ,[Udt]
        /////   FROM [IMES_GetData_Datamaintain].[dbo].[Model]
        ///// WHERE [Model]= 'pn'
        ///// union 
        ///// SELECT [PartNo] AS PartNo,Descr AS Descr,
        /////        1 AS WHERETYPE,
        /////        GetDate() AS [BOMApproveDate]
        /////       ,[Editor]
        /////       ,[Cdt]
        /////       ,[Udt]
        /////   FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        ///// WHERE [PartNo]='pn' AND Flag=1
        ///// order by WHERETYPE
        ///// </summary>
        ///// <param name="pn"></param>
        ///// <returns></returns>
        //DataTable GetMaterialInfo(string pn);

        ///// <summary>
        ///// SELECT distinct a.Material, b.Descr, c.Flag, c.ApproveDate  FROM ModelBOM  AS a 
        ///// left outer join 
        ///// (SELECT Descr, PartNo
        /////   FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        ///// WHERE Flag=1) AS b
        ///// ON a.Material=b.PartNo
        ///// left outer join 
        ///// (Select 'Model' AS Flag, BOMApproveDate AS ApproveDate, Model from Model) AS c
        ///// on a.Material=c.Model 
        ///// where a.Component='current' and a.Flag=1
        ///// </summary>
        ///// <param name="current"></param>
        ///// <returns></returns>
        //DataTable GetParentInfo(string current);

        ///// <summary>
        ///// DELETE FROM [IMES_GetData_Datamaintain].[dbo].[ModelBOM]
        ///// where Material='" + parentCode + "' and Component='" + oldCode + "' AND Flag=0
        ///// </summary>
        ///// <param name="parentCode"></param>
        ///// <param name="oldCode"></param>
        //void DeleteModelBomByMaterialAndComponent(string parentCode, string oldCode);

        ///// <summary>
        ///// ����idɾһ��ModelBOM���ı�Flag
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="editor"></param>
        //void RemoveModelBOMById(int id, string editor);

        ///// <summary>
        ///// �蹫����
        ///// UPDATE ModelBOM SET Alternative_item_group=newid() where ID in(idlist) AND Flag=1
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="editor"></param>
        //void SetNewAlternativeGroup(IList<int> id, string editor);

        ///// <summary>
        ///// Delete From RefreshModel where Model='model'AND Editor='editor'
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="editor"></param>
        //void DeleteRefreshModelByModel(string model, string editor);

        ///// <summary>
        ///// SELECT Model FROM RefreshModel where Model='model'AND Editor='editor'
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="editor"></param>
        ///// <returns></returns>
        //DataTable GetExistRefreshModelByModel(string model, string editor);

        ///// <summary>
        ///// INSERT INTO RefreshModel (Model, Editor) VALUES ('model','editor')
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="editor"></param>
        //void AddRefreshModel(string model, string editor);

        ///// <summary>
        ///// SELECT Model FROM RefreshModel WHERE Editor='editor'
        ///// </summary>
        ///// <param name="editor"></param>
        ///// <returns></returns>
        //DataTable GetRefreshModelList(string editor);

        //////����id��һ��ModelBOM
        ////ModelBOM FindModelBOMByID(int id);

        ///// <summary>
        ///// ȡ�����ĸ��ӹ�ϵ�б�
        ///// ���ô洢���� GetModelBOMAutoDL(pn) ȡ�����Ľ��DataTable
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="limitCount"></param>
        ///// <param name="getStatus"></param>
        ///// <returns></returns>
        //DataTable GetTreeTable(string model, int limitCount, ref int getStatus);

        ///// <summary>
        ///// SELECT Material AS Code, Material_group AS PartType FROM ModelBOM where Material='code' AND Flag=1
        ///// UNION SELECT [PartNo] AS Code ,[PartType]  AS PartType    
        /////   FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        ///// where Flag=1 AND [PartNo]='code'
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetPartTypeInfoByCode(string code);

        ///// <summary>
        ///// SELECT a.Component as Material, case when b.dataFromType is null then b.PartType else c.PartType end AS PartType, a.Quantity, 
        /////         a.Alternative_item_group, a.Priority 
        /////         ,a.[Editor],a.[Cdt],a.[Udt],a.ID FROM ModelBOM AS a
        /////         left outer join 
        /////         (
        /////           select distinct Material AS Code, Material_group AS PartType, 0 as dataFromType from ModelBOM where Flag=1
        /////         ) AS b on b.Code=a.Component
        /////         left outer join 
        /////         (
        /////           SELECT [PartNo] AS Code ,[PartType]  AS PartType    
        /////           FROM [IMES_GetData_Datamaintain].[dbo].[Part] where Flag=1
        /////         ) AS c on c.Code=a.Component
        /////         WHERE  a.Flag=1 AND a.ID IN (idList)
        ///// order by a.Alternative_item_group, a.Priority
        ///// </summary>
        ///// <param name="idList"></param>
        ///// <returns></returns>
        //DataTable GetSubModelBOMByCode(IList<int> idList);

        ///// <summary>
        ///// SELECT PartNo as Code,Descr from Part where PartNo='code' AND Flag=1 AND AutoDL='Y'
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //DataTable GetExistPartNo(string code);

        ///// <summary>
        ///// SELECT distinct a.Material, b.Descr, c.Flag, c.ApproveDate  FROM ModelBOM  AS a 
        ///// left outer join 
        ///// (SELECT Descr, PartNo
        /////   FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        ///// WHERE Flag=1) AS b
        ///// ON a.Material=b.PartNo
        ///// left outer join 
        ///// (Select 'Model' AS Flag, BOMApproveDate AS ApproveDate, Model from Model) AS c
        ///// on a.Material=c.Model 
        ///// where a.Component In 'currentComponents' and a.Flag=1
        ///// </summary>
        ///// <param name="currentComponents"></param>
        ///// <returns></returns>
        //DataTable GetParantInfo(IList<string> currentComponents);

        ///// <summary>
        ///// SELECT distinct Component from ModelBOM where Material in 'codeList' And Flag=1;
        ///// </summary>
        ///// <param name="codeList"></param>
        ///// <returns></returns>
        //DataTable GetComponentByMaterial(IList<string> codeList);

        ///// <summary>
        ///// UPDATE [IMES_GetData_Datamaintain].[dbo].[ModelBOM]
        ///// SET  [Quantity] = 'qty'
        ///// ,[Editor] = 'editor'
        ///// ,[Udt] = getdate()
        ///// WHERE [Alternative_item_group]='groupNo' And [Quantity]<> 'qty' and Flag=1
        ///// </summary>
        ///// <param name="qty"></param>
        ///// <param name="groupNo"></param>
        ///// <param name="editor"></param>
        //void UpdateGroupQuantity(string qty, string groupNo, string editor);

        ///// <summary>
        ///// SELECT [Station],[Customer],[Model],[PartType],[ValueType],[Editor],[Cdt],[Udt],[ID]
        /////   FROM [PartCheckSetting]
        ///// order by [Station],[Customer],[Model],[PartType],[ValueType]
        ///// </summary>
        ///// <returns></returns>
        //IList<PartCheckSetting> GetPartCheckSettingList();

        ///// <summary>
        ///// SELECT distinct [ValueType]   
        /////   FROM [IMES_GetData_Datamaintain].[dbo].[PartCheck]
        /////   where Customer='customer' AND PartType='partType' 
        ///// order by [ValueType]
        ///// </summary>
        ///// <param name="customer"></param>
        ///// <param name="partType"></param>
        ///// <returns></returns>
        //DataTable GetValueTypeListByCustomerAndPartType(string customer, string partType);

        //#region Defered

        ////void AddMOBOMDefered(IUnitOfWork uow, MOBOM Object);

        ////void UpdateMOBOMDefered(IUnitOfWork uow, MOBOM Object, string oldMo);

        ////void DeleteMOBOMDefered(IUnitOfWork uow, MOBOM item);

        //void CopyMOBOMDefered(IUnitOfWork uow, string mo);

        //void UpdateMOBOMForDeleteActionDefered(IUnitOfWork uow, string mo, string partNo, bool deviation);

        ////void UpdateModelBOMDefered(IUnitOfWork uow, ModelBOM item);

        ////void ChangeModelBOMDefered(IUnitOfWork uow, ModelBOM item, int oldId);

        //void IncludeItemToAlternativeItemGroupDefered(IUnitOfWork uow, string value, string parent, string code);

        ////void AddModelBOMDefered(IUnitOfWork uow, ModelBOM item);

        //void DeleteModelBOMByCodeDefered(IUnitOfWork uow, string parentCode, string code);

        //void IncludeAllItemToAlternativeItemGroupDefered(IUnitOfWork uow, string parent, string code1, string code2);

        //void ExcludeAlternativeItemDefered(IUnitOfWork uow, string parent, string code);

        //void DeleteSubModelByCodeDefered(IUnitOfWork uow, string code);

        //void saveGroupNoDefered(IUnitOfWork uow, int mobomId, string mo, int group);

        //void DeleteMOBOMByMoDefered(IUnitOfWork uow, string mo);

        //void ExcludeAlternativeItemToNullDefered(IUnitOfWork uow, string parent, string code);

        //void AddPartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting);

        //void SavePartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting);

        //void DeletePartCheckSettingDefered(IUnitOfWork uow, PartCheckSetting partCheckSetting);

        //void DeleteModelBomByMaterialAndComponentDefered(IUnitOfWork uow, string parentCode, string oldCode);

        //void RemoveModelBOMByIdDefered(IUnitOfWork uow, int id, string editor);

        //void SetNewAlternativeGroupDefered(IUnitOfWork uow, IList<int> ids, string editor);

        //void DeleteRefreshModelByModelDefered(IUnitOfWork uow, string model, string editor);

        //void AddRefreshModelDefered(IUnitOfWork uow, string model, string editor);

        //void UpdateGroupQuantityDefered(IUnitOfWork uow, string qty, string groupNo, string editor);

        //#endregion

        #endregion

        //AcAdapMaintain

        /// <summary>
        /// 1,��ѯ���еĴ��ڵ�Ac Adaptor��¼:
        /// sql:select * from AcAdapMaintain order by Assembly
        /// </summary>
        /// <returns></returns>
        IList<ACAdaptor> GetAllACAdaptor();

        /// <summary>
        /// 2,����Assembly����ƥ���Ac Adaptor
        /// sql:select * from AcAdapMaintain where Assembly like "'"+Assembly+"%'" order by Assembly
        /// </summary>
        /// <param name="Assembly"></param>
        /// <returns></returns>
        IList<ACAdaptor> GetACAdaptorByAssembly(string Assembly);

        /// <summary>
        /// 3,ɾ��ѡ��Ac adaptor��¼:
        /// </summary>
        /// <param name="id"></param>
        void DeleteSelectedACAdaptor(int id);

        /// <summary>
        /// 4,����һ���µ�AC adaptor��¼ 
        /// sql:insert into AcAdapMaintain values (?,?,?,?,?); item
        /// </summary>
        /// <param name="item"></param>
        void AddOneAcAdaptor(ACAdaptor item);

        /// <summary>
        /// 5,����һ����¼:
        /// </summary>
        /// <param name="newItem"></param>
        void UpdateOneAcAdaptor(ACAdaptor newItem);

        /// <summary>
        /// ��ѯ����Grade
        /// SQL:select * from HP_Grade order by family,Series
        /// </summary>
        /// <returns></returns>
        IList<GradeInfo> GetAllGrades();

        /// <summary>
        /// ��ѯ����Grade ����Family
        /// select * from HP_Grade where family= [family]
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<GradeInfo> GetGradesByFamily(string family);

        /// <summary>
        /// ����Grade
        /// insert into hp_grade valus [grade]
        /// ID��GradeInfo���󎧻�
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void AddSelectedGrade(GradeInfo item);

        /// <summary>
        /// ɾ��ѡ�е�Grade
        /// delete from hp_grade where id=[id]
        /// </summary>
        /// <param name="id"></param>
        void DeleteSelectedGrade(int id);

        /// <summary>
        /// ����ѡ�е�Grade
        /// </summary>
        /// <param name="item"></param>
        void UpdateSelectedGrade(GradeInfo item);

        /// <summary>
        /// ��ȡ����BTOceanOrder����
        /// 1. Bt_Seashipmentsku
        /// ����model�ֶ�����
        /// </summary>
        /// <returns></returns>
        IList<BTOceanOrder> GetAllBTOceanOrder();

        /// <summary>
        /// ����pdline��model��ȡBTOceanOrder����
        /// 2. Bt_Seashipmentsku
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<BTOceanOrder> GetListByPdLineAndModel(string pdLine, string model);

        /// <summary>
        /// ����һ��BTOceanOrder����
        /// 3. Bt_Seashipmentsku
        /// </summary>
        /// <param name="obj"></param>
        void AddBTOceanOrder(BTOceanOrder obj);

        /// <summary>
        /// ɾ��һ��BTOceanOrder����
        /// 4. Bt_Seashipmentsku
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="model"></param>
        void DeleteBTOceanOrderByPdlineAndModel(string pdLine, string model);

        /// <summary>
        /// �޸�һ��BTOceanOrder����
        /// 5. Bt_Seashipmentsku
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pdLine"></param>
        /// <param name="model"></param>
        void UpdateBTOceanOrderbyPdlineAndModel(BTOceanOrder obj, string pdLine, string model);

        /// <summary>
        /// 1��ȡ��DescType���е�����TPֵ�����ַ�������
        /// </summary>
        /// <returns></returns>
        IList<string> GetTPsFromDescType();

        /// <summary>
        /// 2������Tpȡ�ö�Ӧ��BOM Description���ݵ�list(��Code��λ����)
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        IList<DescTypeInfo> GetBOMDescrsByTp(string tp);

        /// <summary>
        /// 3������codeȡ��BOM Description�ļ�¼����
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<DescTypeInfo> GetBOMDescrsByCode(string code);

        /// <summary>
        /// 4������id[�����û��id�Ͳ���Code]�޸�BOM Description��¼
        /// </summary>
        /// <param name="id"></param>
        void UpdateBOMDescrById(DescTypeInfo item);

        /// <summary>
        /// 5������BOM Description
        /// </summary>
        /// <param name="item"></param>
        void InsertBOMDescr(DescTypeInfo item);

        /// <summary>
        /// 6������id[�����û��id�Ͳ���Code]ɾ��BOM Description
        /// </summary>
        /// <param name="id"></param>
        void DeleteBOMDescrById(int id);

        /// <summary>
        /// 7������id[�����û��id�Ͳ���Code]����һ��BOM Description��¼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DescTypeInfo FindBOMDescrById(int id);

        void DeleteMoBOMByComponent(string component);

        /// <summary>
        /// SELECT B.[PartNo]
        /// FROM [IMES_GetData].[dbo].[ModelBOM] A,Part B     
        /// where A.[Material]=Model and A.[Component]=B.[PartNo] and B.BomNodeType='MB'
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        IList<string> GetPnListByModelAndBomNodeType(string model, string bomNodeType);

        /// <summary>
        /// SELECT B.[PartNo]
        /// FROM [IMES_GetData].[dbo].[ModelBOM] A,Part B     
        /// where A.[Material]=Model and A.[Component]=B.[PartNo] and B.BomNodeType='MB'and Descr='Anatel label'
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        IList<string> GetPnListByModelAndBomNodeType(string model, string bomNodeType, string descr);

        /// <summary>
        /// select * from ModelBOM a (nolock), Part b (nolock) 
        /// where a.Material = @Model
        ///     and a.Component = b.PartNo
        ///     and b.BomNodeType = 'P1'
        ///     and b.Descr LIKE 'ECOA%
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bomNodeType"></param>
        /// <param name="descr"></param>
        /// <returns></returns>
        IList<MoBOMInfo> GetPnListByModelAndBomNodeTypeAndDescr(string model, string bomNodeType, string descrPrefix);

        /// <summary>
        /// ȡ�ü�¼��Descr�ֶ�ֵ
        /// </summary>
        /// <param name="mpno"></param>
        /// <param name="spno"></param>
        /// <returns></returns>
        IList<string> GetCTOBomDescr(string mpno, string spno);

        /// <summary>
        /// ���� MPno=@mpno AND SPno<>@spno
        /// </summary>
        /// <param name="mpno"></param>
        /// <param name="spno"></param>
        /// <returns></returns>
        IList<CtoBomInfo> GetCTOBomList(string mpno, string spno);

        /// <summary>
        /// SELECT @syscode=os_code FROM Bom_Code (nolock) WHERE part_number=@Pno
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        IList<string> GetOsCodeFromBomCode(string pno);

        /// <summary>
        /// SELECT @grade=Grade,@energia=Energia,@espera=Espera FROM HP_Grade WHERE Series=@series
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<GradeInfo> GetGradesBySeries(string family);

        /// <summary>
        /// SELECT @u=RTRIM([USER].dbo.GetStr(Description,'U=')), 
        ///  @i=RTRIM([USER].dbo.GetStr(Description,'I=')) 
        /// FROM IMES_GetData..DescType (NOLOCK) 
        /// WHERE Tp='AD' AND Code=RTRIM(@family)
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<string> GetDescriptionOfDescTypeListByTpAndCode(string tp, string code);

        /// <summary>
        /// SELECT [Material]
        /// FROM [IMES2012_GetData].[dbo].[ModelBOM]
        /// WHERE [Component]=@partNo
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<string> GetMaterialByComponent(string partNo);

        /// <summary>
        /// SELECT PartInfo.InfoValue 
        /// FROM [IMES_GetData].[dbo].[ModelBOM] a,Part b,PartInfo c
        /// where a.[Material]=@model 
        /// and a.[Component]=b.[PartNo] 
        /// and b.BomNodeType=@bomNodeType 
        /// and b.[PartNo]=c.PartNo 
        /// and c.InfoType=@infoType 
        /// and b.[PartNo] not in (select d.PartNo from PartInfo d where d.PartNo=b.[PartNo] and d.InfoType=@notEqInfoType and d.InfoValue=@notEqInfoValue)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bomNodeType"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetPartInfoValueListByModelAndBomNodeTypeAndInfoType(string model, string bomNodeType, string infoType, string notEqInfoType, string notEqInfoValue);

        /// <summary>
        /// ���BOM�д���WWAN���ӡ
        /// If exist(SELECT [ID] FROM [ModelBOM] a, Part b where a.Material=@material  and  a.Component=b.PartNo and b.Descr like '%'' + @descrInStr + %')
        /// </summary>
        /// <param name="material"></param>
        /// <param name="descrInStr"></param>
        /// <returns></returns>
        bool CheckExistModelBOMByMaterialAndPartDescrLike(string material, string descrInStr);

        IList<MoBOMInfo> GetModelBomList(MoBOMInfo condition);

        IList<MoBOMInfo> GetModelBomListByMaterials(string[] pnList);

        IList<MoBOMInfo> GetModelBomListByComponents(string[] pnList);

        /// <summary>
        /// select distinct d.InfoValue from ModelBOM a, Part b, PartInfo c, PartInfo d
        ///     where a.Material = @Model 
        ///     and a.Component = b.PartNo
        ///     and b.PartNo = c.PartNo 
        ///     and b.PartNo = d.PartNo 
        ///     and b.BomNodeType = 'MB' --@bomNodeType
        ///     and c.InfoType = 'VGA'   --@infoType
        ///     and c.InfoValue = 'SV'   --@infoValue
        ///     and d.InfoType = 'MB'    --@infoType2
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bomNodeType"></param>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <param name="infoType2"></param>
        /// <returns></returns>
        IList<string> GetPartInfoValueListByModelAndBomNodeTypeAndInfoTypes(string model, string bomNodeType, string infoType, string infoValue, string infoType2);

        /// <summary>
        /// select distinct d.InfoValue from ModelBOM a, Part b, PartInfo d
        ///     where a.Material = @Model 
        ///     and a.Component = b.PartNo
        ///     and b.PartNo = d.PartNo 
        ///     and b.BomNodeType = 'MB'    --@bomNodeType
        ///     and d.InfoType = 'MB'       --@infoType
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bomNodeType"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetPartInfoValueListByModelAndBomNodeTypeAndInfoType(string model, string bomNodeType, string infoType);

        /// <summary>
        /// select distinct Station from StationCheck
        /// </summary>
        /// <returns></returns>
        IList<StationInfo> GetAllPartCollectionStation();

        /// <summary>
        /// ʵ��SQL���£�
        /// select top 4 d.InfoValue from ModelBOM a, Part b, PartInfo c,PartInfo d
        ///    where a.Material = '<model>'
        ///    and a.Component = b.PartNo 
        ///    and b.PartNo = c.PartNo 
        ///    and b.PartNo = d.PartNo 
        ///    and b.BomNodeType = 'PL'
        ///    and c.InfoType = 'Picture'
        ///    and RTRIM(c.InfoValue) = 'Y'
        ///    and d.InfoType = 'PictureName'
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<string> GetESOPListByModel(string model);

        /// <summary>
        /// 1�����������infotype��infovalue
        /// SELECT a.Material as MPno, a.Component as SPno
        /// FROM ModelBOM a, PartInfo b
        /// WHERE a.Component = b.PartNo
        ///       AND b.InfoType = 'VendorCode'
        ///       AND b.InfoValue LIKE '%' + LEFT(@VendorCT, 5) + '%'
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="vendorCT"></param>
        /// <returns></returns>
        IList<MoBOMInfo> GetModelBomList(string infoType, string vendorCT);

        /// <summary>
        /// SELECT * FROM HP_WWANLabel NOLOCK WHERE LEFT(ModuleNo, CHARINDEX('-', ModuleNo)-1) = @wwanKPAS
        /// </summary>
        /// <param name="wwanKpas"></param>
        /// <returns></returns>
        IList<HpWwanlabelInfo> GetHpWwanlabelInfoByModuleNoPrefix(string wwanKpas);

        /// <summary>
        /// SELECT COUNT(1) FROM [192.168.147.6,998].HPIMES.dbo.ModelBOM nolock , [192.168.147.6,998].HPIMES.dbo.PAK_CHN_TW_Light nolock ON Component=PartNo WHERE Material=@Pno 
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        bool CheckExistMaterialByPno(string pno);

        /// <summary>
        /// select * from ModelBOM a
        /// inner join Part b
        /// on a.Component = b.PartNo 
        /// and b.Descr = @descr
        /// left join Product_Part c
        /// on b.PartNo = c.PartNo 
        /// and c.ProductID = @productId 
        /// where a.Material = @model 
        /// and c.PartSn is null
        /// and a.Flag = 1
        /// and b.Flag = 1 
        /// </summary>
        /// <param name="descr"></param>
        /// <param name="model"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        IList<string> CheckIfExistProductPartWithBom(string descr, string model, string productId);
        

        /// <summary>
        /// Exists(select b.PartNo from ModelBOM a, Part b where a.Component = b.PartNo 
        /// and (b.Descr like '%WWAN%' or b.Descr like '%WIRELESS%') and a.Material=#product.Model)
        /// </summary>
        /// <param name="descrLike1"></param>
        /// <param name="descrLike2"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CheckIfExistModelBomWithPart(string descrLike1, string descrLike2, string model);

        /// <summary>
        ///  for RCTO
        /// </summary>
        /// <param name="model"></param>
        /// <param name="descrLike"></param>
        /// <param name="bomNodeType"></param>
        /// <param name="bomNodeType2"></param>
        /// <returns></returns>
        bool CheckIfExistDoubleBomWithPart(string model, string descrLike, string bomNodeType, string bomNodeType2);

        IList<string> GetPartNoInModelBOMByBomNodeTypeAndStartWithDescr(IList<string> modelList, string bomNodeType, string startWithBomDescr);

        IList<string> GetPartNoInModelBOMByBomNodeTypeAndContainDescr(IList<string> modelList, string bomNodeType, string containBomDescr);
        
        #region Defered

        void DeleteSelectedACAdaptorDefered(IUnitOfWork uow, int id);

        void AddOneAcAdaptorDefered(IUnitOfWork uow, ACAdaptor item);

        void UpdateOneAcAdaptorDefered(IUnitOfWork uow, ACAdaptor newItem);

        void AddSelectedGradeDefered(IUnitOfWork uow, GradeInfo item);

        void DeleteSelectedGradeDefered(IUnitOfWork uow, int id);

        void UpdateSelectedGradeDefered(IUnitOfWork uow, GradeInfo item);

        void AddBTOceanOrderDefered(IUnitOfWork uow, BTOceanOrder obj);

        void DeleteBTOceanOrderByPdlineAndModelDefered(IUnitOfWork uow, string pdLine, string model);

        void UpdateBTOceanOrderbyPdlineAndModelDefered(IUnitOfWork uow, BTOceanOrder obj, string pdLine, string model);

        void UpdateBOMDescrByIdDefered(IUnitOfWork uow, DescTypeInfo item);

        void InsertBOMDescrDefered(IUnitOfWork uow, DescTypeInfo item);

        void DeleteBOMDescrByIdDefered(IUnitOfWork uow, int id);

        void DeleteMoBOMByComponentDefered(IUnitOfWork uow, string component);
       
        #endregion

        #region for RCTO
        /// <summary>
        /// select * from ModelBOM a
        /// inner join Part b
        /// on a.Component = b.PartNo 
        /// and b.Descr = @descr
        /// left join Product_Part c
        /// on b.PartNo = c.PartNo 
        /// and c.ProductID = @productId 
        /// where a.Material = @model 
        /// and c.PartSn is null
        /// and a.Flag = 1
        /// and b.Flag = 1 
        /// </summary>
        /// <param name="descr"></param>
        /// <param name="model"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool CheckIfExistProductPartWithBomForRCTO(string descr, string model, string productId);
        #endregion


        #region for Ast Bom
        /// <summary>
        /// select a.Component
        ///from ModelBOM a, 
        ///     Part b,
        ///     @ASTType c
        ///where a.Component = b.PartNo and
        ///      b.PartType = c.data and
        ///      a.Material = 'PCBC114KD24Y' and
        ///      a.Component like '2TG%'  and
        ///      b.Flag=1
        /// </summary>
        /// <param name="model"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        IList<string> GetAssetPartNo(string model, IList<string> astType);
        #endregion

        #region ChecItemTypeRule

        IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleWithPriority(string itemType, string line, string station, string family);

        IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleWithPriorityByRegex(string itemType, string line, string station, string family);

        IList<CheckItemTypeRuleDef> GetCheckItemTypeRuleByItemType(string itemType);

        IList<string> GetChechItemTypeList();

        bool CheckExistCheckItemTypeRule(string itemType, string line, string station, string family);

        bool CheckExistCheckItemTypeRule(CheckItemTypeRuleDef item);

        void AddCheckItemTypeRule(CheckItemTypeRuleDef itemType);

        void DeleteCheckItemTypeRule(int id);

        void UpdateCheckItemTypeRule(CheckItemTypeRuleDef itemType);

        #endregion

        #region  Part Forbid function
        IFlatBOM GetModelFlatBOMByStationModelAndPartForbid(string customer, string station, 
                                                                                                string line, string family, 
                                                                                                string model, object mainObj, 
                                                                                                string sessionKey);

       
        #endregion

        #region for Docking get Component
        IList<string> GetPnListByModelAndBomNodeType(IList<string> modelList, string bomNodeType, string prefixDescr);
        /// <summary>
        /// select Material from ModelBom where Component=@Component and flag=1 order by Cdt
        /// </summary>
        /// <param name="coponent"></param>
        /// <returns></returns>
        IList<string> WhereUsedComponent(string component);
        #endregion

        #region Get ModelBOM CTE
        IList<ModelBOM> GetModelBOM(string material);
        #endregion

        /// <summary>
        /// for Remove Cache item 
        /// </summary>
        /// <param name="modelNameList"></param>
        void RemoveCacheByKeyList(IList<string> modelNameList);
    }
}