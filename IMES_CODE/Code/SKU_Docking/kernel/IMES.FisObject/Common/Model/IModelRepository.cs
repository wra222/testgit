using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.FisObject.Common.Part;
using prod = IMES.FisObject.FA.Product;

namespace IMES.FisObject.Common.Model
{
    public interface IModelRepository : IRepository<Model>
    {
        #region . For CommonIntf  .

        /// <summary>
        /// 根据family获得Model列表
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IList<IMES.DataModel.ModelInfo> GetModelList(string familyId);

        /// <summary>
        /// 只列出可以产生ProdId的MO对应的model，即MO.Status=’H’ and MO.Qty>MO.Print_Qty and  MO.SAPStatus=’’, AND Model.Status=1 按照Model排序
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IList<IMES.DataModel.ModelInfo> GetModelListFor014_RecentOneMonth(string familyId);

        /// <summary>
        /// 只列出可以产生ProdId的MO对应的model，即MO.Status=’H’ and MO.Qty>MO.Print_Qty and  MO.SAPStatus=’’, AND Model.Status=1 按照Model排序
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IList<IMES.DataModel.ModelInfo> GetModelListFor014(string familyId);

        /// <summary>
        /// GetData..[Part] – Tp = ‘MA’， 按照Descr 排序显示Descr
        /// </summary>
        /// <returns></returns>
        IList<FamilyInfo> GetFamilyListFor008();

        /// <summary>
        /// GetData..[Part] – Tp = ‘MA’， Descr = @Family 为条件
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IList<_1397LevelInfo> Get1397ListFor008(string familyId);

        /// <summary>
        /// SELECT Model FROM IMES_GetData..Model WHERE Status = '1' ORDER BY Model
        /// </summary>
        /// <returns></returns>
        IList<IMES.DataModel.ModelInfo> GetModelListFor102();

        #endregion

        /// <summary>
        /// 晚加载Model
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Model FillModelAttributes(Model item);

        /// <summary>
        /// 得到已抽样数量
        /// select @cnt=count(*) from QCStatus where Tp='PIA' and PdLine=@pdline and Pno=model# and convert(char(10),Cdt,111)=convert(char(10),getdate(),111) 
        /// @pdline：Product_Status.LineID 
        /// </summary>
        /// <returns>已抽样数量</returns>
        int GetSampleCount(string line, string model);
        
        int GetSampleCount(string line, string model, string type);

        /// <summary>
        /// 根据family得到model且Model对应的MO存在 SAPStatus<>’CLOSE’ and Status=’H’ and Qty<Print_Qty
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IList<IMES.DataModel.ModelInfo> GetModelListFor094(string familyId);

        /// <summary>
        /// 2.--get family
        /// declare @CTNO varchar(20)
        /// select Family from IMES_GetData..Model where Model=(select Model from IMES_GetData..MO where MO=(select top 1 MO from IMES_GetData..MoBOM where PartNo=(select IECPn from IMES_FA..PartSN where IECSN=@CTNO)))
        /// IList<string> GetFamilyListByCTNO(string ctno);
        /// Get [Code] by [Model],
        /// 参数@Model
        /// SELECT RTRIM(a.Value) + '-' + CASE (CHARINDEX(' ', Family) - 1) WHEN -1 THEN Family
        ///        ELSE SUBSTRING(Family, 1, (CHARINDEX(' ', Family) - 1)) END as [KittingCode]
        ///        FROM IMES_GetData..ModelInfo a, IMES_GetData..Model b
        ///        WHERE a.Model = b.Model
        ///            AND a.Name = 'DM2'
        ///            AND a.Model = @Model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string GetKittingCodeByModel(string model);

        /// <summary>
        /// 查找CUST（其中@cust是Product.Model对应的ModelInfo中Name=’Cust’对应的值）已用过的最大号 
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="infoName"></param>
        /// <returns></returns>
        string GetMaxModelInfoValue(string modelName, string infoName);

        /// <summary>
        /// 5.根据model和name，取IMES_GetData..ModelInfo记录 (class: IMES.FisObject.Common.Model.ModelInfo);
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<IMES.FisObject.Common.Model.ModelInfo> GetModelInfoByModelAndName(string model, string name);

        /// <summary>
        /// SQL：select Code from HPWeekCode (nolock)
        /// where convert(char(10),getdate(),111) >= Substring(Descr,1,10)
        /// and convert(char(10),getdate(),111)<=Substring(Descr,12,10)
        /// </summary>
        /// <returns></returns>
        IList<string> GetCodeFromHPWeekCodeInRangeOfDescr();

        /// <summary>
        /// SELECT * as [HP Week Code] FROM HPWeekCode
        /// WHERE CONVERT(char(10),GETDATE()-1,111) >= SUBSTRING(Descr,1,10) 
        /// AND CONVERT(char(10),GETDATE()-1,111)<=SUBSTRING(Descr,12,10)
        /// </summary>
        /// <returns></returns>
        IList<HpweekcodeInfo> GetHPWeekCodeInRangeOfDescr();

        /// <summary>
        /// SELECT [Model] FROM [IMES2012_GetData].[dbo].[Model] where Family=@family and [Status]=1 ORDER By [Model]
        /// </summary>
        /// <param name="family"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<string> GetModelListByFamly(string family, int status);

        /// <summary>
        /// 1.SELECT [Model]
        /// FROM [IMES2012_GetData].[dbo].[Model] where Family=@family and [Status]=1 ORDER By [Model]
        /// </summary>
        /// <param name="family"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<IMES.DataModel.ModelInfo> GetModelListByFamilyAndStatus(string family, int status);
        
        /// <summary>
        /// SELECT Model FROM ModelInfo WHERE [Name] = '<arg1>' AND [Value] = '<arg2>'
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IList<string> GetModelByNameAndValue(string name, string value);

        /// <summary>
        /// 1.根据Model获取[Current Station]，Station倒序排列
        /// 条件是：（未进包装（不存在69的成功过站Log）且已经产生CUSTSN（Product.CUSTSN<>’’）的Product记录）
        /// select Rtrim(c.Station) +' '+RTRIM (c.Descr ) as Descr, c.Station ,COUNT(*) as Qty
        /// from ProductStatus a, Product b,Station c
        /// where a.ProductID = b.ProductID
        /// and b.Model = @Model1
        /// and b.ProductID not in (
        /// select ProductID from ProductLog
        /// where Model = @Model1
        /// and Station = '69'
        /// and Status = 1)
        /// and b.CUSTSN <>''
        /// and a.Status = 1
        /// and a.Station = c.Station
        /// Group by c.Station,c.Descr 
        /// order by c.Station desc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DataTable GetCurrentStationList(string model);

        /// <summary>
        /// SELECT @cnt=count(DISTINCT ProductID) FROM QCStatus WHERE Tp='PAQC' 
        /// AND LEFT(Line,1)=LEFT(@pdline,1)
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="pdline"></param>
        /// <returns></returns>
        int GetCountOfQcStatus(string tp, string pdline);

        /// <summary>
        /// 获取抽检周期内，进行过PAQC 抽检的Product 总数
        /// SELECT @cnt=count(DISTINCT ProductID) FROM QCStatus WHERE Tp='PAQC'
        /// AND LEFT(Line, 1) = LEFT(@pdline, 1)
        /// AND Cdt >= @StartDate AND Cdt < @EndDate
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="pdline"></param>
        /// <returns></returns>
        int GetCountOfQcStatus(string tp, string pdline, DateTime startTime, DateTime endTime);

        /// <summary>
        /// PAQC Sorting接口需求
        /// Previous Fail Time
        /// SELECT * FROM PAQCSorting
        /// WHERE Status = 'O'
        /// AND LEFT(Line, 1) = LEFT(@Line, 1)
        /// AND Station = @Station
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<PaqcsortingInfo> GetPreviousFailTimeList(string line, string station);

        /// <summary>
        /// SELECT * FROM PAQCSorting WHERE Station = @Station AND LEFT(Line, 1) = LEFT(@Line, 1)   
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<PaqcsortingInfo> GetPaqcsortingInfoList(string line, string station);

        /// <summary>
        /// ex: SELECT * FROM PAQCSorting where id = @id
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PaqcsortingInfo> GetPaqcsortingInfoList(PaqcsortingInfo condition);

        /// <summary>
        /// 2)Pass Qty
        /// SELECT COUNT(CUSTSN) FROM PAQCSorting_Product NOLOCK
        /// WHERE PAQCSortingID = @PAQCSortingID
        /// AND Status=1
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCountOfPqacSortingProduct(PaqcsortingProductInfo condition);

        /// <summary>
        /// SELECT * FROM PAQCSorting_Product WHERE PAQCSortingID = @PAQCSortingID AND Status = '2'
        /// SELECT * FROM PAQCSorting_Product WHERE PAQCSortingID = @PAQCSortingID AND Status <> 2
        /// </summary>
        /// <param name="eqCondition"></param>
        /// <param name="neqCondition"></param>
        /// <returns></returns>
        IList<PaqcsortingProductInfo> GetPaqcsortingProductInfoList(PaqcsortingProductInfo eqCondition, PaqcsortingProductInfo neqCondition);

        /// <summary>
        /// 3)Least Pass Qty
        /// SELECT COUNT(CUSTSN) FROM PAQCSorting_Product NOLOCK
        /// WHERE PAQCSortingID = @PAQCSortingID
        /// AND Status=1
        /// AND Cdt>@PreviousFailTime
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="previousFailTime"></param>
        /// <returns></returns>
        int GetCountOfPqacSortingProduct(PaqcsortingProductInfo condition, DateTime previousFailTime);

        /// <summary>
        /// Insert PAQCSorting_Product
        /// INSERT INTO [PAQCSorting_Product]([PAQCSortingID],[CUSTSN],[Status],[Editor],[Cdt])
        /// VALUES(@PAQCSortingID, @CustomerSN, 1, @Editor, GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void InsertPqacSortingProductInfo(PaqcsortingProductInfo item);

        /// <summary>
        /// Update PAQCSorting
        /// UPDATE PAQCSorting SET Stauts = 'C', Editor = @Editor, Udt = GETDATE() WHERE ID = @PAQCSortingID
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePqacSortingInfo(PaqcsortingInfo setValue, PaqcsortingInfo condition);

        /// <summary>
        /// Insert PAQCSorting
        /// </summary>
        /// <param name="item"></param>
        void InsertPqacSortingInfo(PaqcsortingInfo item);

        /// <summary>
        /// SELECT @StartTime = MIN(Cdt) FROM PAQCSorting_Product NOLOCK WHERE PAQCSortingID = @PAQCSortingID
        /// </summary>
        /// <param name="paqcSortingId"></param>
        /// <returns></returns>
        DateTime GetMinCdtFromPaqcSortingProduct(int paqcSortingId);

        /// <summary>
        /// INSERT INTO PAQCSorting_Product (PAQCSortingID, CUSTSN, Status, Editor, Cdt)		
        /// SELECT @PAQCSortingID, a.CUSTSN, b.Status, b.Editor, b.Cdt
        ///     FROM Product a (NOLOCK), ProductLog b (NOLOCK)
        ///     WHERE a.ProductID = b.ProductID
        ///         AND b.Cdt > @StartTime
        ///         AND LEFT(b.Line, 1) = LEFT(@Line, 1)
        ///         AND b.Station = @Station 
        ///         AND a.CUSTSN NOT IN (SELECT CUSTSN FROM PAQCSorting_Product NOLOCK WHERE PAQCSortingID = @PAQCSortingID)
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="paqcSortingId"></param>
        void InsertIntoPaqCSortingProductFromProductAndProductLog(DateTime startTime, string line, string station, int paqcSortingId);

        /// <summary>
        /// INSERT INTO PAQCSorting_Product (PAQCSortingID, CUSTSN, Status, Editor, Cdt) 
        /// SELECT TOP (@N3) @PAQCSortingID, a.CUSTSN, b.Status, b.Editor, b.Cdt
        /// FROM Product a (NOLOCK), ProductLog b (NOLOCK)
        /// WHERE a.ProductID = b.ProductID
        /// AND b.Cdt > @StartTime
        /// AND LEFT(b.Line, 1) = LEFT(@Line, 1)
        /// AND b.Station = @Station 
        /// AND a.CUSTSN NOT IN (SELECT CUSTSN FROM PAQCSorting_Product NOLOCK WHERE PAQCSortingID = @PAQCSortingID)
        /// ORDER BY b.Cdt
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="paqcSortingId"></param>
        /// <param name="n3"></param>
        /// <returns></returns>
        void InsertIntoPaqCSortingProductFromProductAndProductLog(DateTime startTime, string line, string station, int paqcSortingId, int n3);

        #region . Defered .

        void InsertPqacSortingProductInfoDefered(IUnitOfWork uow, PaqcsortingProductInfo item);

        void UpdatePqacSortingInfoDefered(IUnitOfWork uow, PaqcsortingInfo setValue, PaqcsortingInfo condition);

        void InsertPqacSortingInfoDefered(IUnitOfWork uow, PaqcsortingInfo item);

        void InsertIntoPaqCSortingProductFromProductAndProductLogDefered(IUnitOfWork uow, DateTime startTime, string line, string station, int paqcSortingId);

        #endregion

        #region For Maintain

        /// <summary>
        /// 取得Family下的所有Model数据的list(按Model列的字母序排序)
        /// </summary>
        /// <param name="familyId"></param>
        /// <returns></returns>
        IList<Model> GetModelObjList(string familyId);

        /// <summary>
        /// 根据modelInfoId获得ModelInfo
        /// </summary>
        /// <param name="modelInfoId"></param>
        /// <returns></returns>
        ModelInfo GetModelInfoById(int modelInfoId);

        /// <summary>
        /// 是否有同名记录，若有，返回true，否则,false
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        int CheckExistedModelInfo(string modelId, string attrName);

        /// <summary>
        /// 更新Model (可修改Model)
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="oldModelName"></param>
        void ChangeModel(Model Object, string oldModelName);

        /// <summary>
        /// 从数据库获得Model
        /// </summary>
        /// <param name="ModelId"></param>
        /// <returns></returns>
        Model FindFromDB(string ModelId);

        /// <summary>
        /// 26)SELECT distinct Model.Model
        /// FROM  Family INNER JOIN
        /// Model ON Family.Family = Model.Family WHERE CustomerID ='customer' 
        /// order by Model
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        DataTable GetCustomerModelList(string customer);

        /// <summary>
        /// select distinct Name from ModelInfo order by Name
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllModelInfoName();

        /// <summary>
        /// 取得匹配的所有Model数据的list(按Model列的字母序排序)，支持左匹配 like 'model%'
        /// </summary>
        /// <param name="family"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<Model> GetModelListByModel(string family, string model);

        /// <summary>
        /// 系统中所有已维护的ShipType值,按ShipType列的字母序排序
        /// </summary>
        /// <returns></returns>
        IList<ShipType> GetShipTypeList();

        /// <summary>
        /// 取ModelInfoName中所有记录,按Region、Info Name列的字母序排序
        /// </summary>
        /// <returns></returns>
        IList<ModelInfoName> GetModelInfoNameList();

        /// <summary>
        /// 取得ModelInfoName中等于region和modelInfoName的记录个数
        /// 如果modelInfoNameID== "", where 子句不包含该查询条件
        /// 否则where 中包含条件 ModelInfoName.ID != modelInfoNameID
        /// </summary>
        /// <param name="region"></param>
        /// <param name="modelInfoName"></param>
        /// <param name="modelInfoNameID"></param>
        /// <returns></returns>
        int CheckExistedModelInfoName(string region, string modelInfoName, string modelInfoNameID);

        /// <summary>
        /// 新增一条ModelInfoName的记录数据，返回id
        /// </summary>
        /// <param name="item"></param>
        void AddModelInfoName(ModelInfoName item);

        /// <summary>
        /// 修改一条ModelInfoName的记录数据，返回id
        /// </summary>
        /// <param name="item"></param>
        void SaveModelInfoName(ModelInfoName item);

        /// <summary>
        /// 删除一条ModelInfoName的记录
        /// </summary>
        /// <param name="item"></param>
        void DeleteModelInfoName(ModelInfoName item);

        /// <summary>
        /// 参考sql如下：
        /// select B.Name as Name, B.Description as Description, isNull(A.Value, '') as Value,
        ///         A.Editor  as Editor, A.Cdt as Cdt, A.Udt as Udt, A.ID as ID
        /// From (select Model, Name, Value, Editor, Cdt, Udt, ID from ModelInfo where Model = ?) as A 
        /// Right Outer Join ModelInfoName as B
        /// On A.Name = B.Name
        /// 按Region、Info Name列的字母序排序
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        IList<ModelInfoNameAndModelInfoValue> GetModelInfoNameAndModelInfoValueListByModel(string Model);

        /// <summary>
        /// 新增一条ModelInfo的记录数据，返回id
        /// </summary>
        /// <param name="item"></param>
        void AddModelInfo(ModelInfo item);

        /// <summary>
        /// 修改一条ModelInfo的记录数据
        /// </summary>
        /// <param name="item"></param>
        void SaveModelInfo(ModelInfo item);

        /// <summary>
        /// 删除一条ModelInfo的记录
        /// </summary>
        /// <param name="item"></param>
        void DeleteModelInfo(ModelInfo item);

        /// <summary>
        /// Delete ModelInfoName where Region=?
        /// </summary>
        /// <param name="region"></param>
        void DeleteModelInfoNameByRegion(string region);

        /// <summary>
        /// SELECT Model.Model FROM Family INNER JOIN Model ON Family.Family = Model.Family
        /// WHERE Family.CustomerID = 'customer' AND  Model.Model='model'
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CheckExistModel(string customer, string model);

        /// <summary>
        /// 1.返回值和参数说明:
        /// FamilyDef为IMES.DataModel中的可序列化的结构类型.
        /// SQL语句:
        /// SELECT DISTINCT CASE (CHARINDEX(' ', Family) - 1) WHEN -1 THEN Family
        ///                    ELSE SUBSTRING(Family, 1, (CHARINDEX(' ', Family) - 1)) END AS Family 
        ///          FROM IMES_GetData..Model
        ///          WHERE LEFT(Model, 4) <> '1397'
        ///                    AND ISNULL(Family, '') <> ''
        ///          ORDER BY Family
        /// </summary>
        /// <returns></returns>
        IList<string> GetFamilyList();

        /// <summary>
        /// 参考sql如下：
        /// select Model from Model
        /// 按Model 列的字符序排序
        /// </summary>
        /// <returns></returns>
        IList<string> GetModelList();

        /// <summary>
        /// 取得全部的ShipType 
        /// 参考sql：
        /// select * from ShipType order by ShipType 
        /// </summary>
        /// <returns></returns>
        IList<ShipType> GetAllShipType();

        /// <summary>
        /// 判断ShipType是否已经存在 
        /// 参考sql:
        /// select * from ShipType where ShipType=?
        /// </summary>
        /// <param name="shipType"></param>
        /// <returns></returns>
        bool IfShipTypeIsEXists(string shipType);

        /// <summary>
        /// 更新ShipType
        /// 参考sql:
        /// Update ShipType Set Description=?,Editor=?,Udt=getdate() where ShipType=?
        /// </summary>
        /// <param name="shipType"></param>
        void UpdateShipType(ShipType shipType);

        /// <summary>
        /// 插入新的ShipType纪录
        /// 参考sql:
        /// insert ShipType(ShipType,Description,Editor,Cdt,Udt)values(?,?,?,getdate(),getdate())
        /// </summary>
        /// <param name="shipType"></param>
        void InsertShipType(ShipType shipType);

        /// <summary>
        /// 判断ShipType是否已经被Model使用
        /// 参考sql:
        /// select count(1) from Model where ShipType=?
        /// </summary>
        /// <param name="shipType"></param>
        /// <returns></returns>
        bool IfShipTypeIsInUse(string shipType);

        /// <summary>
        /// 取得ShipType 
        /// 参考sql:
        /// select * from ShipType where ShipType=?
        /// </summary>
        /// <param name="shipType"></param>
        /// <returns></returns>
        ShipType GetShipTypeByKey(string shipType);

        /// <summary>
        /// 删除ShipType 
        /// 参考sql:
        /// delete from ShipType where ShipType=?
        /// </summary>
        /// <param name="shipType"></param>
        void DeleteShipTypeByKey(string shipType);

        /// <summary>
        /// IF EXISTS (SELECT * FROM ModelInfo NOLOCK
        /// WHERE [Name] = 'PN'
        /// AND RIGHT(RTRIM([Value]), 4) = '#ABJ'
        /// AND Model = @Model)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <param name="valuePrefix"></param>
        /// <returns></returns>
        bool CheckExistModelInfo(string name, string model, string valuePrefix);

        #region . Defered .

        void ChangeModelDefered(IUnitOfWork uow, Model Object, string oldModelName);

        void AddModelInfoNameDefered(IUnitOfWork uow, ModelInfoName item);

        void SaveModelInfoNameDefered(IUnitOfWork uow, ModelInfoName item);

        void DeleteModelInfoNameDefered(IUnitOfWork uow, ModelInfoName item);

        void AddModelInfoDefered(IUnitOfWork uow, ModelInfo item);

        void SaveModelInfoDefered(IUnitOfWork uow, ModelInfo item);

        void DeleteModelInfoDefered(IUnitOfWork uow, ModelInfo item);

        void DeleteModelInfoNameByRegionDefered(IUnitOfWork uow, string region);

        void UpdateShipTypeDefered(IUnitOfWork uow, ShipType shipType);

        void InsertShipTypeDefered(IUnitOfWork uow, ShipType shipType);

        void DeleteShipTypeByKeyDefered(IUnitOfWork uow, string shipType);

        #endregion

        #endregion


        #region for ModelChangeQty table
        /// <summary>
        /// order by line, Cdt
        /// </summary>
        /// <param name="model"></param>
        /// <param name="shipDate"></param>
        /// <returns></returns>
        IList<ModelChangeQtyDef> GetModelChangeQtyByModelShipDate(string model, DateTime shipDate);
        /// <summary>
        ///  Query in transaction order by ShipDate, AssignedQty
        /// </summary>
        /// <param name="line"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ModelChangeQtyDef GetActiveModelChangeQty(string line, string model);

        void AddModelChangeQty(ModelChangeQtyDef modeChangeQty);

        void DeleteModelChangeQty(int id);

        void AssignedModelChangeQty(int id);

        void AssignedModelChangeQtyDefered(IUnitOfWork uow, int id);

        void RollbackAssignedModelChangeQty(int id);
       

        #endregion

        #region FAIModel Info
        IList<FAIModelInfo> GetFAIModel(FAIModelInfo condition);
        void UpdateFAIModel(FAIModelInfo item);
        void InsertFAIModel(FAIModelInfo item);
        void DeleteFAIModel(string model);
        bool IsFAIModel(string model);

        void UpdateFAIModelDefered(IUnitOfWork uow, FAIModelInfo item);
        void InsertFAIModelDefered(IUnitOfWork uow, FAIModelInfo item);
        void DeleteFAIModelDefered(IUnitOfWork uow, string model);

        FAIModelInfo GetFAIModelByModelWithTrans(string model);
        FAIModelInfo GetFAIModelByModel(string model);
        void CheckAndSetInFAQtyWithTrans(string model, int inQty, IList<prod.IProduct> productList, string InfoType, string InfoValue, string editor);
        void CheckAndSetInFAQtyWithTransDefered(IUnitOfWork uow, string model, int inQty, IList<prod.IProduct> productList, string InfoType, string InfoValue, string editor);
        void CheckAndSetInPAKQtyWithTrans(string model, int inQty, IList<prod.IProduct> productList, string InfoType, string InfoValue, string editor);
        void CheckAndSetInPAKQtyWithTransDefered(IUnitOfWork uow, string model, int inQty, IList<prod.IProduct> productList, string InfoType, string InfoValue, string editor);

        IList<FAIModelApprovalInfo> GetFAIModelApprovalInfo(string pakState, string model, string actionName);
        IList<FAIModelApprovalInfo> GetFAIModelApprovalInfo(string pakState, string actionName);


        #endregion

        #region FAI Approval
        IList<ApprovalItemInfo> GetApprovalItem(ApprovalItemInfo condition);
        void UpdateApprovalItem(ApprovalItemInfo item);
        void InsertApprovalItem(ApprovalItemInfo item);
        void DeleteApprovalItem(long id);

        IList<ApprovalStatusInfo> GetApprovalStatus(ApprovalStatusInfo condition);
        void UpdateApprovalStatus(ApprovalStatusInfo item);
        void InsertApprovalStatus(ApprovalStatusInfo item);
        void DeleteApprovalStatus(long id);

        IList<UploadFilesInfo> GetUploadFiles(UploadFilesInfo condition);
        void UpdateUploadFiles(UploadFilesInfo item);
        void InsertUploadFiles(UploadFilesInfo item);
        void DeleteUploadFiles(long id);

        IList<ApprovalItemAttrInfo> GetApprovalItemAttr(ApprovalItemAttrInfo condition);
        void UpdateApprovalItemAttr(ApprovalItemAttrInfo item);
        void InsertApprovalItemAttr(ApprovalItemAttrInfo item);
        void DeleteApprovalItemAttr(long id);
        void DeleteApprovalItemAttr(long approvalItemID, string attrName);

        IList<ApprovalContentInfo> GetApprovalContent(string moduleKeyValue, string actionName);
        #endregion

        /// <summary>
        /// for Remove Cache item 
        /// </summary>
        /// <param name="nameList"></param>
        void RemoveCacheByKeyList(IList<string> nameList);


        #region for maintain Model
        void CopyModel(string scrModelName, string destModelName, int status, string editor);
        void CopyModelDefered(IUnitOfWork uow, string scrModelName, string destModelName, int status, string editor);
        #endregion
    }
}
