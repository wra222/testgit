using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using System.Data;

namespace IMES.FisObject.Common.Model
{
    public interface IFamilyRepository : IRepository<Family>
    {
        /// <summary>
        /// 根据Family获取Family对象
        /// </summary>
        /// <param name="family">family</param>
        /// <returns>Family对象</returns>
        Family FindFamily(string family);

        /// <summary>
        /// 获得Family列表
        /// </summary>
        /// <returns></returns>
        IList<FamilyInfo> GetFamilyList();

        /// <summary>
        /// 晚加载Family对象
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Model FillFamilyObj(Model item);

        /// <summary>
        /// 根据customer获得Family列表
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<FamilyInfo> FindFamiliesByCustomer(string customer);

        IList<FamilyInfo> FindFamiliesByCustomerOrderByFamily(string customer);

        /// <summary>
        /// 根据Family获取OQCRatio
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        QCRatio GetQCRatio(string family);

        /// <summary>
        /// SELECT [Family] FROM [Family] order by [Family]
        /// </summary>
        /// <returns></returns>
        DataTable GetAllFamily();

        #region For Maintain

        /// <summary>
        /// 取得Customer下的family数据的list(按Family列的字母序排序)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        IList<Family> GetFamilyList(string customerId);

        /// <summary>
        /// 取得所有family数据的list(按Family列的字母序排序)
        /// </summary>
        /// <returns></returns>
        IList<Family> GetFamilyObjList();

        /// <summary>
        /// 更改Family信息
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="oldModelName"></param>
        void ChangeFamily(Family Object, string oldModelName);

        /// <summary>
        /// SELECT C.NewFamily,A.[QCRatio],A.[EOQCRatio],A.[PAQCRatio],A.Editor,C.Model,A.Cdt,A.Udt, C.FamilyKey ,C.Flag FROM [QCRatio] AS A INNER JOIN
        /// (
        /// SELECT Customer AS FamilyKey,0 AS Flag, Customer AS NewFamily, '' AS Model FROM Customer WHERE Customer=@ Customer
        /// UNION  
        /// SELECT [Model] AS FamilyKey,2 AS Flag, [Family] AS NewFamily, [Model] AS Model FROM [Model] WHERE Family in (SELECT Family FROM [Family] WHERE [CustomerID]=@ Customer)
        /// UNION
        /// SELECT Family AS FamilyKey,1 AS Flag, Family AS NewFamily, '' AS Model FROM [Family] WHERE [CustomerID]= @ Customer 
        /// ) AS C ON A.[Family]=C.FamilyKey ORDER BY NewFamily, Model 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        DataTable GetQCRatioList(string customer);

        // SELECT C.NewFamily,A.[QCRatio],A.[EOQCRatio],A.[PAQCRatio],A.Editor,C.Model,A.Cdt,A.Udt, C.FamilyKey ,C.Flag FROM [QCRatio] AS A INNER JOIN
        // (
        // SELECT Customer AS FamilyKey,0 AS Flag, Customer AS NewFamily, '' AS Model FROM Customer WHERE Customer=@Customer
        // UNION  
        // SELECT [Model] AS FamilyKey,3 AS Flag, [Family] AS NewFamily, [Model] AS Model FROM [Model] WHERE Family in (SELECT Family FROM [Family] WHERE [CustomerID]=@Customer)
        // UNION
        // SELECT Family AS FamilyKey,2 AS Flag, Family AS NewFamily, '' AS Model FROM [Family] WHERE [CustomerID]= @Customer 
        // Union
        // SELECT distinct Left(Line,1) AS FamilyKey,1 AS Flag, Left(Line,1) AS NewFamily, '' AS Model FROM [Line] WHERE [CustomerID]= @Customer  and (Stage='FA' or Stage = 'PAK')
        // ) AS C ON A.[Family]=C.FamilyKey ORDER BY NewFamily, Model 
        DataTable GetQCRatioList2(string customer);

        /// <summary>
        /// SELECT A.[Family],A.[QCRatio],A.[EOQCRatio],A.Editor,A.Cdt,A.Udt FROM [QCRatio] AS A INNER JOIN
        /// (
        /// SELECT Family FROM [Family] WHERE [CustomerID]=@Customer 
        /// UNION 
        /// SELECT Customer AS [Family] FROM Customer WHERE Customer=@Customer
        /// Union 
        /// select distinct LEFT(Line,1) as [Family] from Line  where (Stage = 'FA' or Stage = 'PAK') and CustomerID = @Customer
        /// ) AS C ON A.[Family]=C.[Family] ORDER BY [Family]
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        DataTable GetQCRatioList3(string customer);

        /// <summary>
        /// 新增QCRatio
        /// </summary>
        /// <param name="item"></param>
        void AddQCRatio(QCRatio item);

        /// <summary>
        /// 更新QCRatio
        /// </summary>
        /// <param name="item"></param>
        void SaveQCRatio(QCRatio item);

        /// <summary>
        /// 删除QCRatio
        /// </summary>
        /// <param name="item"></param>
        void DeleteQCRatio(QCRatio item);

        /// <summary>
        /// 更新QCRatio (可改主键)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="oldId"></param>
        void UpdateQCRatio(QCRatio item, string oldId);

        /// <summary>
        /// SELECT t.[Family]
        /// FROM [IMES_FA]..[QCRatio] t INNER JOIN(
        /// SELECT Family 
        /// FROM [Family]
        /// WHERE [CustomerID]='customer' 
        /// UNION 
        /// SELECT Customer AS [Family] 
        /// FROM Customer
        /// WHERE Customer='customer'
        /// ) AS C
        /// ON t.[Family]=C.[Family]
        /// WHERE t.[Family]='QCRatioId'
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="QCRatioId"></param>
        /// <returns></returns>
        DataTable GetExistQCRatio(string customer, string QCRatioId);

        /// <summary>
        /// 返回选择到的CustomerID
        /// SELECT [CustomerID]
        ///   FROM dbo.Family
        /// where [Family]='param'
        /// union 
        /// select Customer from dbo.Customer
        /// where Customer='param'
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IList<string> GetMatchedCustomer(string param);

        ////通过model取得所属的family
        //Family GetFamilyByModel(string model);

        /// <summary>
        /// 取得存在的QCRatio
        /// SELECT [Family]
        /// FROM [QCRatio]
        /// WHERE [QCRatio].[Family]=@qcRatioFamily
        /// </summary>
        /// <param name="QCRatioFamily"></param>
        /// <returns></returns>
        DataTable GetExistQCRatio(string qcRatioFamily);

        /// <summary>
        /// 取得family下的Model
        /// SELECT [Model] FROM [Model] WHERE [Family]=@family ORDER BY [Model]
        /// </summary>
        /// <param name="Family"></param>
        /// <returns></returns>
        DataTable GetModelListByFamily(string family);
        
        /// <summary>
        /// sql select family from family where customerD='HP' order by family
        /// </summary>
        /// <param name="customerIds"></param>
        /// <returns></returns>
        IList<string> GetFamilysByCustomer(string[] customerIds);

        /// <summary>
        /// 获取FamilyInfo表记录 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<FamilyInfoDef> GetExistFamilyInfo(FamilyInfoDef condition);

        /// <summary>
        /// 增加一条FamilyInfo表记录
        /// </summary>
        /// <param name="item"></param>
        void AddFamilyInfo(FamilyInfoDef item);

        /// <summary>
        /// 删除指定条件的FamilyInfo表记录
        /// </summary>
        /// <param name="condition"></param>
        void RemoveFamilyInfo(FamilyInfoDef condition);

        /// <summary>
        /// 更新指定条件的FamilyInfo表记录
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateFamilyInfo(FamilyInfoDef setValue, FamilyInfoDef condition);

        /// <summary>
        /// Select count(DISTINCT ProductID) from QCStatus where line=@pdline and model=@model and Tp=’PIA’and Cdt年月=当前年月
        /// </summary>
        /// <param name="line"></param>
        /// <param name="model"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        int GetCountOfQCStatusInCurrentMonth(string line, string model, string tp);

        /// <summary>
        /// 1、select QCRatio.EOQCRatio from QCRatio where QCRatio.Family=@input;
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<int> GetEOQCRatioList(QCRatioInfo condition);
        IList<QCRatioInfo> GetQCRatioInfoList(QCRatioInfo condition);

        #region Defered

        void ChangeFamilyDefered(IUnitOfWork uow, Family Object, string oldModelName);

        void AddQCRatioDefered(IUnitOfWork uow, QCRatio item);

        void SaveQCRatioDefered(IUnitOfWork uow, QCRatio item);

        void DeleteQCRatioDefered(IUnitOfWork uow, QCRatio item);

        void UpdateQCRatioDefered(IUnitOfWork uow, QCRatio item, string oldId);

        void AddFamilyInfoDefered(IUnitOfWork uow, FamilyInfoDef item);

        void RemoveFamilyInfoDefered(IUnitOfWork uow, FamilyInfoDef condition);

        void UpdateFamilyInfoDefered(IUnitOfWork uow, FamilyInfoDef setValue, FamilyInfoDef condition);

        #endregion

        #endregion

        /// <summary>
        /// for Remove Family Cache item 
        /// </summary>
        /// <param name="nameList"></param>
        void RemoveCacheByKeyList(IList<string> nameList);
    }
}
