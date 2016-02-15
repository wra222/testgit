using System.Collections.Generic;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using System;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;

namespace IMES.FisObject.Common.ModelBOM
{
   
    public interface IModelBOMRepository : IRepository<BOMNodeData>
    {
       
        #region For Maintain
        /// <summary>
        /// get Bom Tree Structure 
        ///  DataTable column Name: Id, Material, Component, Descr, IsPart, IsModel, level
        /// </summary>
        /// <param name="model"></param>
        /// <param name="limitCount"></param>
        /// <param name="getStatus"></param>
        /// <returns></returns>
        DataTable GetTreeTable(string model, int limitCount, ref int getStatus);


        /// <summary>
        /// Select * from ModelBom where Material='" + parentCode + "' and Component='" + oldCode + "'
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="oldCode"></param>
        /// <returns></returns>
        IList<BOMNodeData> findModelBomByMaterialAndComponent(String parentCode, String oldCode);


        /// <summary>
        /// SELECT PartNo as Code,Descr from Part where PartNo='code' AND Flag=1 AND AutoDL='Y'
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetExistPartNo(string code);


        /// <summary>
        /// SELECT Material AS Code, Material_group AS PartType FROM ModelBOM where Material='code' AND Flag=1
        /// UNION SELECT [PartNo] AS Code ,[PartType]  AS PartType    
        ///   FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        /// where Flag=1 AND [PartNo]='code'
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetPartTypeInfoByCode(string code);

        /// <summary>
        /// SELECT [Model] AS PartNo,'' AS Descr,
        ///        0 AS WHERETYPE
        ///       ,[BOMApproveDate] AS [BOMApproveDate]
        ///       ,[Editor]
        ///       ,[Cdt]
        ///       ,[Udt]
        ///   FROM [IMES_GetData_Datamaintain].[dbo].[Model]
        /// WHERE [Model]= 'pn'
        /// union 
        /// SELECT [PartNo] AS PartNo,Descr AS Descr,
        ///        1 AS WHERETYPE,
        ///        GetDate() AS [BOMApproveDate]
        ///       ,[Editor]
        ///       ,[Cdt]
        ///       ,[Udt]
        ///   FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        /// WHERE [PartNo]='pn' AND Flag=1
        /// order by WHERETYPE
        /// </summary>
        /// <param name="pn"></param>
        /// <returns></returns>
        DataTable GetMaterialInfo(string pn);

        /// <summary>
        /// ���ModelBOM
        /// </summary>
        /// <param name="modelBOMId"></param>
        /// <returns></returns>
        BOMNodeData GetModelBOM(int modelBOMId);

        DataTable GetComponentByMaterial(string code);

        DataTable GetComponentByMaterial(IList<string> codeList);


        /// <summary>
        /// SELECT distinct a.Material, b.Descr, c.Flag, c.ApproveDate  FROM ModelBOM  AS a 
        /// left outer join 
        /// (SELECT Descr, PartNo
        ///   FROM [IMES_GetData_Datamaintain].[dbo].[Part]
        /// WHERE Flag=1) AS b
        /// ON a.Material=b.PartNo
        /// left outer join 
        /// (Select 'Model' AS Flag, BOMApproveDate AS ApproveDate, Model from Model) AS c
        /// on a.Material=c.Model 
        /// where a.Component In 'currentComponents' and a.Flag=1
        /// </summary>
        /// <param name="currentComponents"></param>
        /// <returns></returns>
        DataTable GetParentInfo(IList<string> currentComponents);


        DataTable GetSubModelBOMByCode(IList<int> idList);


         /// <summary>
        /// ModelBOM�t�s?
        /// </summary>
        /// <param name="oldCode"></param>
        /// <param name="newCode"></param>
        /// <param name="editor"></param>
        void SaveModelBOMAs(string oldCode, string newCode, string editor);

        void SaveModelBOMAsDefered(IUnitOfWork uow, string oldCode, string newCode, string editor);

         /// <summary>
        /// string strSql = "SELECT Material,Material_group FROM ModelBOM where Material='" + code + "'";
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetMaterialById(string code);


        void DeleteRefreshModelByModelDefered(IUnitOfWork uow, string model, string editor);


        void AddRefreshModelDefered(IUnitOfWork uow, string model, string editor);



        void AddModelBOMDefered(IUnitOfWork uow, BOMNodeData item);

        void DeleteModelBomByMaterialAndComponentDefered(IUnitOfWork uow, string parentCode, string oldCode);

        void UpdateModelBOMDefered(IUnitOfWork uow, BOMNodeData item);

        void UpdateGroupQuantityDefered(IUnitOfWork uow, string qty, string groupNo, string editor);

        void RemoveModelBOMByIdDefered(IUnitOfWork uow, int id, string editor);

        void SetNewAlternativeGroupDefered(IUnitOfWork uow, IList<int> ids, string editor);



        #endregion


        #region Implementation of IRepository<BOMNodeData>
          // need implement find method
        #endregion

        #region For new functions

        void CacheUpdate_ForBOM(string component);
        void CacheUpdate_ForBOMDefered(IUnitOfWork uow, string component);

        #region BOMNodeRelation

        IList<BOMNodeRelation> FindBOMNodeRelationParents(string ChildBOMNodeType);

        IList<BOMNodeRelation> FindBOMNodeRelationChild(string ParentType);

        IList<BOMNodeRelation> FindBOMNodeRelationByPair(string BOMNodeType, string ChildBOMNodeType);

        DataTable FindBOMNodeRelationByRoot(string TYPE);

        DataTable FindBOMNodeRelationParentsByRoot(string TYPE, string ChildType);

        IList<BOMNodeRelation> GetBOMNodeRelation();


        string AddBOMNodeRelation(BOMNodeRelation r);

        void UpdateBOMNodeRelation(BOMNodeRelation r);

        void UpdateBOMNodeRelation(BOMNodeRelation r, int ID);

        void DeleteBOMNodeRelation(string BOMNodeType, string ChildBOMNodeType);

        void DeleteBOMNodeRelation(int ID);

        DataTable GetTreeTableByID(string ID);


        // Data to check BOMNodeRelation in ModelBOM
        DataTable GetBomNodeType(string ParentPartNo, string ChildPartNo);
        DataTable GetChildBOMTypes(string ParentType);

        IList<string> GetModelsFromModelBOM(string modelNo, int rowCount);
        IList<string> GetPartsFromModelBOM(string partNo, int rowCount);

        #endregion

        #endregion

    }
}