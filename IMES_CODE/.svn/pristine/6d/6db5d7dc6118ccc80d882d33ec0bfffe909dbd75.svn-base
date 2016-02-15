using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using System.Data;

namespace IMES.FisObject.Common.MO
{
    /// <summary>
    /// Product订单的Repository接口
    /// </summary>
    public interface IMORepository : IRepository<MO>
    {
        /// <summary>
        /// select ID from MO where Qty-Print_Qty > 0 and Status='H' and Udt>dateadd(day,-30,getdate()) and Model=model# and MO.SAPStatus=’’order by ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<MOInfo> GetMOListFor014(string model);

        /// <summary>
        /// 根据机器Model号，取得MO信息列表
        /// </summary>
        /// <param name="modelId">机器Model号码</param>
        /// <returns>MO信息列表</returns>
        IList<MOInfo> GetMOList(string modelId);

        /// <summary>
        /// MO.Prt_Qty>0 and MO.Status='H' and MO.SAPStatus=''
        /// </summary>
        /// <returns></returns>
        IList<MOInfo> GetMOListFor013();

        /// <summary>
        /// 得到最小可用的Mo
        /// select @newmo=Min(ID)from MO where ModelID=@model and Qty-Print_Qty>0 and Print_Qty>=0 and Status='H'
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        MO GetMinUsableMO(string model);

        /// <summary>
        /// 使Print_Qty增加指定数量
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="count"></param>
        void IncreaseMOPrintedQty(MO mo, short count);

        /// <summary>
        /// 使Print_Qty减少指定数量
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="count"></param>
        void DecreaseMOPrintedQty(MO mo, short count);

        /// <summary>
        /// SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM IMES_GetData..MO
        /// WHERE Model = @Model
        ///     AND LEFT(MO, 1) = 'V'
        ///     AND Cdt BETWEEN CONVERT(varchar, GETDATE(),111) AND CONVERT(varchar, DATEADD(day, 1, GETDATE()), 111)
        /// ORDER BY MO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<MO> GetVirtualMObyModel(string model);

        /// <summary>
        /// From MO where SAPStatus<>’CLOSE’ and Status=’H’ and Qty<Print_Qty by model
        /// 显示以下信息：
        /// MO / Qty / StartDate，按照StartDate降序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<MOInfo> GetOldMOListFor094(string model);

        /// <summary>
        /// 按照Model得到SAPStatus<>’CLOSE’ and Status=’H’ and Qty>Print_Qty by model 按照mo排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<MOInfo> GetNewMOListFor094(string model);

        /// <summary>
        /// 得到StartDate最靠前的可用的Mo
        /// SELECT TOP 1 MO FROM IMES_GetData..MO WHERE Model = @NewModel AND Qty > Print_Qty AND Status = 'H' ORDER BY StartDate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        MO GetUsableMOOrderByStartDate(string model);

        /// <summary>
        /// select ID from MO where Qty-Print_Qty > 0 and  Status='H' and  Udt>dateadd(day,-10,getdate()) and StartDate>dateadd(day,-3,getdate())  and Model=model# and  MO.SAPStatus=’’ order by ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<MOInfo> GetMOListFor014ConsiderStartDate(string model);

        /// <summary>
        /// select Qty,Qty-Print_Qty from MO where Mo=mo#
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        int[] GetQtyAndRemainedOfMo(string mo);

        /// <summary>
        /// 使Transfer_Qty减少指定数量
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="count"></param>
        void DecreaseMOTransferQty(MO mo, short count);

        /// <summary>
        /// SELECT [MO]
        /// FROM [IMES2012_GetData].[dbo].[MO] 
        /// where [SAPStatus]='' 
        /// and [Qty]>[Print_Qty] 
        /// and [Status]='H' 
        /// and convert(varchar(8),Udt,112)>convert(varchar(8),dateadd(day,-10,getdate()),112) 
        /// and Convert(varchar(8),StartDate,112)>Convert(varchar(8),dateadd(day,-3,getdate()),112) 
        /// and [Model]=model# 
        /// Order By MO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<MOInfo> GetMOListFor014ConsiderStartDateOrderByMo(string model);

        /// <summary>
        /// 2.Get MO by Model（含Virtual Mo和真实Mo）
        /// SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM IMES_GetData..MO
        /// WHERE Model = @Model
        /// AND Cdt BETWEEN CONVERT(varchar, GETDATE(),111) AND CONVERT(varchar, DATEADD(day, 1, GETDATE()), 111)
        /// ORDER BY MO
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        IList<MO> GetVirtualMOAndRealMObyModel(string model);
     
        /// <summary>
        /// SELECT MO, Model, CreateDate, StartDate, Qty, Print_Qty FROM MO a，Model b
        /// WHERE a.Model=b.Model and b.Family=@Family and  Qty>Print_Qty order by MO
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<MOInfo> GetMOListByFamily(string family);

        /// <summary>
        /// Execute JOB
        /// </summary>
        /// <param name="jobName"></param>
        void ExecuteMoUploadJOB(string jobName);

        /// <summary>
        /// UPDATE [HPIMES].[dbo].[MO]
        /// SET [CustomerSN_Qty]=[CustomerSN_Qty]+1
        /// From Product a,MO b WHERE a.MO=b.MO and a.ProductID=ProductID#
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="count"></param>
        void UpdateMoForIncreaseCustomerSnQty(string productId, short count);

        /// <summary>
        /// 1.select distinct Model.Model from Model, MO 
        ///     where Model.Family = [Family]
        ///     and Model.Model = MO.Model
        ///     and Print_Qty ＜ Qty 
        ///     order by Model.Model
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> GetModelListFromMo(string family);

        /// <summary>
        /// 2.select * from MO 
        ///    where Model=''
        ///    and Print_Qty ＜ Qty
        ///    order by Udt desc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<MOInfo> GetMOByModel(string model);

        #region Product Plan
        /// <summary>
        /// select ID, PdLine, ShipDate, Family, Model, PlanQty, AddPrintQty, PrePrintQty, Editor, Cdt, Udt 
        ///from ProductPlan
        ///where PdLine = @line and
        ///ShipDate =@shipdate
        /// </summary>
        /// <param name="Line">Phyical Line</param>
        /// <param name="ShipDate">ShipDate</param>
        /// <returns></returns>

        IList<ProductPlanInfo> GetProductPlanByLineAndShipDate(string line, DateTime shipDate);

        /// <summary>
        //select a.ID, 'Summary' as [Action], a.PdLine, a.ShipDate, a.Family, a.Model, a.PlanQty, a.AddPrintQty, 
        //                                               (b.Qty - b.Print_Qty) as RemainQty, 
        //                                               isnull(dbo.GetMOStationQty(@station, b.MO), 0) as NonInputQty, 
        //                                               (b.Print_Qty - isnull(dbo.GetMOStationQty('F0', b.MO), 0)) as InputQty,
        //                                               a.Editor, a.Cdt, a.Udt 
        //                                            from ProductPlan a
        //                                            inner join MO b on b.MO = dbo.GetPlanMONo(a.ID, a.PdLine)
        //                                            where a.PdLine = @line and
        //                                                  a.ShipDate =@shipDate
        /// </summary>
        /// <param name="Line">Phyical Line</param>
        /// <param name="ShipDate">ShipDate</param>
        /// <param name="Station">Station</param>
        /// <returns></returns>

        IList<ProductPlanLog> GetProductPlanMOByLineAndShipDate(string line, DateTime shipDate, string station);


        /// <summary>
        //select ID, [Action], PdLine, ShipDate, Family, Model, PlanQty, AddPrintQty, 
        //                                                   0 as RemainQty, 0 as NonInputQty, 0 as InputQty, 
        //                                                   Editor, Cdt, Cdt as Udt
        //                                            from ProductPlanLog
        //                                            where [Action] =@action and ShipDate=@shipDate and PdLine=@line
        //                                            order by ID
        /// </summary>
        /// <param name="Line">Phyical Line</param>
        /// <param name="ShipDate">ShipDate</param>
        /// <param name="Action">Action</param>
        /// <returns></returns>

        IList<ProductPlanLog> GetProductPlanLogByLineAndShipDateAndAction(string line, DateTime shipDate, string action);


        /// <summary>
        /// declare @ProductPlanUpload TbProductPlan 
        ///  exec IMES_ProductPlanCheckInsert @ProductPlanUpload 
        /// </summary>
        /// <param name="ProdPlanList"> TVP parameter structure</param>
        IList<ProductPlanLog> UploadProductPlan(IList<TbProductPlan> prodPlanList, string combinePO);

        /// <summary>
        /// declare @ProductPlanUpload TbProductPlan 
        ///  exec IMES_ProductPlanCheckInsert @ProductPlanUpload 
        /// </summary>
        /// <param name="ProdPlanList"> TVP parameter structure</param>
        IList<ProductPlanLog> UploadProductPlan_Revise(IList<TbProductPlan> prodPlanList, string combinePO);

        /// <summary>
        /// SELECT distinct a.[Family]
        /// ,a.[Descr]
        ///,a.[CustomerID]
        ///FROM [Family] a, ProductPlan b, MO c
        ///Where a.Family = b.Family 
        /// and b.PdLine = left(@PdLine,1) 
        /// and b.ShipDate=@ShipDate
        /// and GetPlanMONo(b.ID, b.PdLine) = c.MO 
        ///and c.Qty > c.Print_Qty 
         /// and c.[Status] = 'H'
        /// </summary>
        /// <param name="line"></param>
        /// <param name="shipdate"></param>
        IList<ProductPlanFamily> GetProductPlanFamily(string line, DateTime shipdate);

        /// <summary>
        /// SELECT a.[Model], b.[ID]
        ///FROM [Model] a, ProductPlan b, MO c
        ///Where a.Family = @Family 
        ///      and a.Model = b.Model
        ///      and b.PdLine = left(@PdLine,1) 
        ///      and b.ShipDate=@ShipDate
        ///      and GetPlanMONo(b.ID, b.PdLine) = c.MO 
        ///      and c.Qty > c.Print_Qty 
        ///      and c.[Status] = 'H' 
        ///Order by a.[Model]
        /// </summary>
        /// <param name="line"></param>
        /// <param name="shipdate"></param>
        /// <param name="Model"></param>
        /// <returns></returns>

        IList<ProductPlanInfo> GetProductPlanModel(string line, DateTime shipdate, string family);

        /// <summary>
        /// SELECT a.*, b.PlanQty 
        ///FROM  MO a, ProductPlan b
        ///Where b.[ID] = @ID and 
        ///      a.MO= dbo. GetPlanMONo(b.ID, b.PdLine)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MOPlanInfo GetPlanMO(int id);


        #endregion

        #region . Defered .

        void IncreaseMOPrintedQtyDefered(IUnitOfWork uow, MO mo, short count);

        void DecreaseMOPrintedQtyDefered(IUnitOfWork uow, MO mo, short count);

        void DecreaseMOTransferQtyDefered(IUnitOfWork uow, MO mo, short count);

        void ExecuteMoUploadJOBDefered(IUnitOfWork uow, string jobName);

        void UpdateMoForIncreaseCustomerSnQtyDefered(IUnitOfWork uow, string productId, short count);

        #endregion

        #region For Maintain

        /// <summary>
        /// 选项包括Status栏位不是“C”(已出货)的所有MO
        /// </summary>
        /// <returns></returns>
        IList<MO> GetNonCMOList();

        /// <summary>
        /// SELECT distinct(MO), Qty, PrintQty as StartQty, Udt FROM MO where Model='" + model + "' AND Status <>'C'
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DataTable GetMoByModel(string model);

        #endregion

        #region  for auto assigne MO
        /// <summary>
        /// order by CreateDate, (Qty-PrintQty) desc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string GetActiveMO(string model);
        /// <summary>
        ///  select top 1  MO
        ///  from MO WITH (UPDLOCK,ROWLOCK,READPAST) 
        ///  where  Model ='1510B0967701'   and
        ///   Status='H'
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        string GetActiveMOWithNoneLock(string model);
        void AssignedMO(string mo);
        void AssignedMODefered(IUnitOfWork uow, string mo);

        void RollbackAssignedMO(string mo, string productId);


        #endregion

        #region PilotMo table
        void InsertPilotMo(PilotMoInfo item);
        void InsertPilotMoDefered(IUnitOfWork uow, PilotMoInfo item);
        void UpdatePilotMo(PilotMoInfo item);
        void UpdatePilotMoDefered(IUnitOfWork uow, PilotMoInfo item);
        void DeletePilotMo(string mo);
        void DeletePilotMoDefered(IUnitOfWork uow, string mo);

        IList<PilotMoInfo> SearchPilotMo(PilotMoInfo condition);
        IList<PilotMoInfo> SearchPilotMo(PilotMoInfo condition, IList<string> combinedState);
        IList<PilotMoInfo> SearchPilotMo(PilotMoInfo condition, DateTime beginCdt, DateTime endCdt);


        PilotMoInfo GetPilotMo(string mo);
        PilotMoInfo GetAndLockPilotMo(string mo);
        void CheckAndCombinedPilotMo(string mo, int qty, string editor);
        void CheckAndCombinedPilotMoDefered(IUnitOfWork uow, string mo, int qty, string editor);

        #endregion

        #region for assign PoNo
        bool CheckModelBindPoNo(string model);
        IList<string> GetBindPoNoByModel(string model);
        #endregion
    }
}
