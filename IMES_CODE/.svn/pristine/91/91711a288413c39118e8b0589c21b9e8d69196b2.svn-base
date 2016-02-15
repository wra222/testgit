using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using System.Data;

namespace IMES.FisObject.PCA.MBMO
{
    /// <summary>
    /// 用于访问MBMO对象的Repository接口
    /// </summary>
    public interface IMBMORepository : IRepository<IMBMO>
    {
        /// <summary>
        /// 根据主板型号，取得未完成的制造订单列表。
        /// </summary>
        /// <param name="model">主板型号</param>
        /// <returns>未完成制造订单对象集合</returns>
        IList<IMBMO> GetUnfinishedMOByModel(string model);

        /// <summary>
        /// 取得所有未完成的制造订单
        /// </summary>
        /// <returns>未完成制造订单对象集合</returns>
        IList<IMBMO> GetUnfinishedMO();

        ///// <summary>
        ///// 根据量产/试产获取最大MO
        ///// </summary>
        ///// <param name="isExperiment">是否为试产</param>
        ///// <returns>最大MO</returns>
        //string GetMaxMO(bool isExperiment);

        /// <summary>
        /// 根据前缀获取最大MO
        /// </summary>
        /// <param name="preStr">前缀</param>
        /// <returns>最大MO</returns>
        string GetMaxMO(string preStr);

        /// <summary>
        /// 更新MO最大号(NumControl)
        /// </summary>
        /// <param name="isExperiment">是否为试产</param>
        /// <param name="maxMO">MO最大号</param>
        void SetMaxMO(bool isExperiment, IMBMO maxMO);

        /// <summary>
        /// 获取当天产生的未生成所有MBSn的MBMO
        /// </summary>
        /// <returns></returns>
        IList<IMBMO> GetUnfinishedMOToday();

        /// <summary>
        /// 更新指定mo的printedQty, 在其上加指定的值, 直接写db
        /// </summary>
        /// <param name="mo">mo</param>
        /// <param name="prtQtyInc">需要加的數量</param>
        void UpdateSMTMOPrtQtyForInc(string mo, int prtQtyInc);

        #region . For CommonIntf  .

        /// <summary>
        /// 根据111阶ID获得SMTMO列表
        /// </summary>
        /// <param name="_111LevelId"></param>
        /// <returns></returns>
        IList<SMTMOInfo> GetSMTMOList(string _111LevelId);

        /// <summary>
        /// 根据SMTMO ID获得SMTMO信息
        /// </summary>
        /// <param name="SMTMOId"></param>
        /// <returns></returns>
        SMTMOInfo GetSMTMOInfo(string SMTMOId);

        #endregion

        /// <summary>
        /// select A.SMTMO as MO, B.InfoValue as MBCODE,D.InfoValue as Description, A.IECPartNo as IECPartNo, A.Qty as Qty, A.PrintQty as PrintQty,  A.Remark as Remark,A.Cdt as Cdt
        /// from SMTMO as A, PartInfo as B, Part As C , PartInfo as D
        /// where A.PrintQty < A.Qty 
        /// and convert(varchar(10),A.Cdt,120) = convert(varchar(10),getdate(),120)  
        /// and A.IECPartNo= B.PartNo 
        /// and B.PartNo=C.PartNo 
        /// And B.InfoType IN ('MB','VB','SB')
        /// and A.IECPartNo= D.PartNo 
        /// and D.InfoType='MDL'
        /// </summary>
        /// <returns></returns>
        IList<SMTMOInfo> GetSMTMOInfos();

        /// <summary>
        /// Select DISTINCT Model.Model from Model,MO 
        /// Where Model.Family=@family and MO.Model=Model.Model 
        /// and MO.Status='H' and MO.Qty>MO.Print_Qty and  MO.SAPStatus=''
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> GetModelsByFamilyWithMO(string family);

        /// <summary>
        /// Select DISTINCT Model.Model from Model,MO 
        /// Where Model.Family=@family and MO.Model=Model.Model 
        /// and MO.Status='H' and MO.Qty>MO.Print_Qty and  MO.SAPStatus='' AND MO.Udt>dateadd(day,-30,getdate())
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> GetModelsByFamilyWithMORecentOneMonth(string family);

        /// <summary>
        /// 根据Customer获得Family列表 (最近一个月以内的)
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<FamilyInfo> GetFamilysByCustomerWithMORecentOneMonth(string customer);
        
        /// <summary>
        /// Select MO from MO 
        /// Where Model= @model 
        /// and Qty-Print_Qty > 0 and  Status='H' and Udt>dateadd(day,-30,getdate()) 
        /// and  MO.SAPStatus='' order by MO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<string> GetMOsRecentOneMonthByModel(string model);

        /// <summary>
        /// SELECT [MO] FROM [IMES2012_GetData].[dbo].[MO] where [SAPStatus]='' and [Qty]>[Print_Qty] and [Status ]='H' and [Model]=model# Order By MO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<string> GetMOsRecentOneMonthByModelRegardlessUdt(string model);

        /// <summary>
        /// 根据MoList删除MO,返回其中不符合删除条件的MO列表
        /// 实现逻辑：
        ///         1.delete from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 and PrintQty = 0
        ///         2.select distinct SMTMO from SMTMO WHERE Charindex(SMTMO,@SMTMOS,0)>0 
        /// </summary>
        /// <param name="moList"></param>
        /// <returns></returns>
        IList<string> DeleteSMTMO(IList<string> moList);

        /// <summary>
        /// SELECT SMTMO FROM IMES_PCA..SMTMO WHERE IECPartNo = @111LevelPartNo
        /// AND PrintQty < Qty
        /// ORDER BY SMTMO
        /// </summary>
        /// <param name="_111LevelId"></param>
        /// <returns></returns>
        IList<SMTMOInfo> GetSMTMOListFor002(string _111LevelId);

        /// <summary>
        /// select count(*) from PCB a inner join PCBStatus b on 
        /// a.PCBNo = b.PCBNo where a.SMTMO=@smtmo and Status='1' and  
        /// (b.Station='30' or b.Station='8G')
        /// </summary>
        /// <param name="smtMo"></param>
        /// <returns></returns>
        int GetMBCountByMOAndStatusStation(string smtMo);

        /// <summary>
        /// select count(*) from PCB where a.SMTMO=@smtmo
        /// 两个数相等返回ture
        /// </summary>
        /// <param name="smtMo"></param>
        /// <returns></returns>
        int GetMBCountByMO(string smtMo);

        /// <summary>
        /// Select MO from MO 
        /// Where Model= @model 
        /// and Qty-Print_Qty > 0 and  Status='H' and Udt>dateadd(day,-30,getdate()) 
        /// and MO.SAPStatus='' and StartDate>dateadd(day,-3,getdate()) order by MO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<string> GetMOsRecentOneMonthByModelConsiderStartDate(string model);

        /// <summary>
        /// 3.Get [SMTMO]
        /// SQL：SELECT SMTMO FROM IMES_PCA..SMTMO WHERE IECPartNo = @PartNo
        /// AND PrintQty < Qty ORDER BY SMTMO
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<string> GetSmtMoListByPno(string partNo);

        /// <summary>
        /// 4.Get[MO Qty and Remain Qty]
        /// SELECT Qty , PrintQty 
        /// FROM IMES_PCA..SMTMO WHERE SMTMO = @SMTMO
        /// 
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<SmtmoInfo> GetSmtmoInfoList(SmtmoInfo condition);

        /// <summary>
        /// SMTMO, Qty>PrintQty and Cdt=<Today>
        /// </summary>
        /// <returns></returns>
        IList<SmtmoInfo> GetSmtmoInfoListToday();

        /// <summary>
        /// SELECT [Model]
        ///   FROM [Model],[MO] 
        /// where Model.Model=MO.Model 
        /// and Family=@family 
        /// and Model.[Status]=1 
        /// and [SAPStatus]='' 
        /// and [Qty]>[Print_Qty] 
        /// and MO.[Status]='H' 
        /// and Convert(varchar(8),MO.Udt,112)>convert(varchar(8),dateadd(day,-10,getdate()),112) 
        /// and Convert(varchar(8),MO.StartDate,112)>Convert(varchar(8),dateadd(day,-3,getdate()),112) 
        /// ORDER By [Model]
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<string> GetModelListFromMo(string family);

        /// <summary>
        /// 3.RCTOMBMaintain表中添加一条记录
        /// </summary>
        /// <param name="item"></param>
        void AddRctombmaintainInfo(RctombmaintainInfo item);

        /// <summary>
        /// 4.删除RCTOMBMaintain选中记录：
        /// DELETE FROM RCTOMBMaintain 
        /// WHERE Code = @code and Family=@family and Type=@type
        /// </summary>
        /// <param name="condition"></param>
        void DeleteRctombmaintainInfo(RctombmaintainInfo condition);

        void UpdateRctombmaintainInfo(RctombmaintainInfo setValue, RctombmaintainInfo condition);

        /// <summary>
        /// SELECT MO, a.Model, CreateDate, StartDate, Qty, Print_Qty, CustomerSN_Qty 
        /// FROM MO a, Model b
        /// WHERE a.Model=b.Model 
        /// and b.Family like '%@Family%' 
        /// and a.Model like '%@Model%'
        /// and a.MO like '%@MO%'
        /// and CONVERT(Varchar,a.CreateDate,111)>=CONVERT(Varchar,@startDate,111)
        /// and CONVERT(Varchar,a.CreateDate,111)<=CONVERT(Varchar,@endDate,111)
        /// and Qty>Print_Qty 
        /// and a.Status = 'H'
        /// order by MO
        /// </summary>
        /// <param name="family"></param>
        /// <param name="model"></param>
        /// <param name="mo"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<IMES.FisObject.Common.MO.MO> GetMoModelInfoListFromMoModel(string family, string model, string mo, DateTime startDate, DateTime endDate, string status);

        /// <summary>
        /// Almighty
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<RctombmaintainInfo> GetRctombmaintainInfoList(RctombmaintainInfo condition);

        /// <summary>
        ///   select c.Model from MO a, MO_Excel b, Model c
        ///   where a.Model = b.Model
        ///   and a.Model = c.Model 
        ///   and a.Qty > a.Print_Qty
        ///   and a.Status = 'H'
        ///   and c.Family = '@Family'
        ///   and left(b.Line,1) = LEFT('@Line',1)
        ///   and b.Qty > b.PrintQty
        ///   order by c.Model
        /// </summary>
        /// <param name="status"></param>
        /// <param name="family"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        IList<string> GetModelListWithMoExcel(string status, string family, string line);

        /// <summary>
        /// 2.新提供接口：Update MO_Excel Set PrintQty = PrintQty + [UI 打印的Qty]
        /// Where left(b.Line,1) = LEFT('@Line',1)
        /// And Model = [@Model]
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="line"></param>
        /// <param name="model"></param>
        void IncreasePrintQtyForMoExcel(int qty, string line, string model);

        #region . Defered  .

        void SetMaxMODefered(IUnitOfWork uow, bool isExperiment, IMBMO maxMO);

        void UpdateSMTMOPrtQtyForIncDefered(IUnitOfWork uow, string mo, int prtQtyInc);

        //IList<string> DeleteSMTMODefered(IUnitOfWork uow, IList<string> moList);

        void AddRctombmaintainInfoDefered(IUnitOfWork uow, RctombmaintainInfo item);

        void DeleteRctombmaintainInfoDefered(IUnitOfWork uow, RctombmaintainInfo condition);

        void UpdateRctombmaintainInfoDefered(IUnitOfWork uow, RctombmaintainInfo setValue, RctombmaintainInfo condition);

        void IncreasePrintQtyForMoExcelDefered(IUnitOfWork uow, int qty, string line, string model);

        #endregion

        #region other functions
        /// <summary>
        /// 抓取與今天差天數的STMO
        /// </summary>
        /// <param name="diffDayswithToday"></param>
        /// <returns></returns>
        IList<SmtmoInfo> GetSmtmoInfoListByDay(int diffDayswithToday, string status);
        #endregion

    }
}
