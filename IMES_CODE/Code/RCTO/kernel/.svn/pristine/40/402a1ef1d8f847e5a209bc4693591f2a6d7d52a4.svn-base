using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using System.Data;
using IMES.FisObject.Common.Station;

namespace IMES.FisObject.Common.Line
{
    public interface ILineRepository : IRepository<Line>
    {
        #region . For CommonIntf  .
        
        /// <summary>
        /// Get PdLine List by customerId & stationId
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="stationId"></param>
        /// <returns>PdLine Info List</returns>
        IList<PdLineInfo> GetPdLineList(string customerId, string stationId);
        
        /// <summary>
        /// Get PdLine List by customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>PdLine Info List</returns>
        IList<PdLineInfo> GetAllPdLineListByCust(string customerId);

        /// <summary>
        /// select * from Line where Stage=stage order by Line
        /// </summary>
        /// <param name="stage"></param>
        /// <returns>PdLine Info List</returns>
        IList<LineInfo> GetAllPdLineListByStage(string stage);

        /// <summary>
        /// SELECT [Line] AS Code, [Descr] as Line
        /// FROM IMES_GetData..Line
        /// WHERE Stage = @Stage
        /// and CustomerID = @Customer
        /// ORDER BY Code
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        DataTable GetLineByCustomerAndStage(string customer, string stage);

        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<DOAListInfo> GetDOAListListByDn(DOAListInfo condition);

        /// <summary>
        ///  setValue哪个字段赋值就有更新哪个字段
        ///  condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateDOAList(PoDataInfo setValue, PoDataInfo condition);

        /// <summary>
        /// SELECT [Line] FROM [IMES2012_GetData].[dbo].[Line] Where [Stage]='FA' Order By  [Line]
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        IList<string> GetLinesByStage(string stage);

        /// <summary>
        /// SELECT [Line] AS Code, [Descr] as Line FROM IMES_GetData..Line WHERE Stage=@Stage and CustomerID=@Customer ORDER BY Code
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="customer"></param>
        /// <returns>PdLine Info List</returns>
        IList<LineInfo> GetPdLineListByStageAndCustomer(string stage, string customer);

        /// <summary>
        /// SELECT distinct a.Station, a.Descr
        /// FROM IMES2012_GetData..Station a(nolock),IMES2012_GetData..Line_Station b(nolock)
        /// Where a.Station = b.Station 
        /// and rtrim(a.StationType) = 'SATest' 
        /// and b.Status='1' 
        /// and b.Line = [PdLine]
        /// Order By a.Station,a.Descr
        /// </summary>
        /// <param name="line"></param>
        /// <param name="stationType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<IStation> GetStationListByLineAndStationType(string line, string stationType, string status);

        IList<IStation> GetStationListByLineAndStationType(string line, string stationType);

        /// <summary>
        /// SELECT a.[Line] AS Code, [Descr] as Line
        /// FROM IMES_GetData..Line_Station a, IMES_GetData..Line b
        /// WHERE a.Station = @Station
        ///    and a.Line = b.Line
        ///    and CustomerID = @Customer
        /// ORDER BY [Descr]
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<LineInfo> GetLineListByStationAndCustomer(string station, string customer);

        /// <summary>
        /// Select distinct (left(Line, 1)) as Line from Line where Stage=’SA’order by left(Line, 1)
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        IList<string> GetLinePrefixListByStage(string stage);

        /// <summary>
        /// 取得Line的方法：Select distinct(left(Line, 1)) as Line from Line where Stage=’FA’or Stage=’PAK’order by left(Line, 1)
        /// </summary>
        /// <param name="stages"></param>
        /// <returns></returns>
        IList<string> GetLinePrefixListByStages(string[] stages);

        /// <summary>
        /// 取得Line的方法：select distinct LEFT(Line,1) from Line  where (Stage = 'FA' or Stage = 'PAK') and CustomerID = @Customer order by LEFT(Line,1)
        /// </summary>
        /// <param name="stages"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        IList<string> GetLinePrefixListByStages(string[] stages, string customer);

        #region . Defered .

        void UpdateDOAListDefered(IUnitOfWork uow, PoDataInfo setValue, PoDataInfo condition);

        #endregion

        #endregion

        #region For Maintain

        /// <summary>
        /// 1根据Line检索Line_Station 
        /// 参考sql:Status返回0，1就可以，不需转换为Pass，Fail。
        /// select a.ID,a.Station,b.StationType,a.Status,b.Descr,a.Editor,a.Cdt,a.Udt  
        /// from Line_Station a join Station b on a.Station=b.Station  
        /// where a.Line=? 
        /// Order by a.Cdt
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        DataTable GetPaLineStationListByLine(string line);

        /// <summary>
        /// 2取得全部的Stage 
        /// 参考sql:
        /// select Stage from Stage order by Stage 
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllStage();

        /// <summary>
        /// 3取得PdLine
        /// 参考sql: 
        /// select Line from Line where CustomerID=? and Stage=? order by Line 
        /// </summary>
        /// <param name="Cust"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        IList<string> GetLineByCustAndStage(string Cust, string stage);

        /// <summary>
        /// 4删除line和Station的关系
        /// 参考sql:
        /// delete Line_Station where ID=? 
        /// </summary>
        /// <param name="id"></param>
        void DeleteLineStationByID(int id);

        /// <summary>
        /// 5判断line和Station的关系是否已经存在
        /// 参考sql:
        /// select * from Line_Station where Line=? and Station=? 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        bool IFLineStationIsExists(string line, string station);

        /// <summary>
        /// 6添加LineStation,返回新纪录的ID
        /// 参考sql:
        /// insert Line_Station(Line,Station,Status,Editor,Cdt,Udt) values(?,?,?,?,?,?)
        /// </summary>
        /// <param name="lineStation"></param>
        void AddLineStation(LineStation lineStation);

        /// <summary>
        /// 7更新LineStation纪录
        /// 参考sql:
        /// update Line_Station set Line=?,Station=?,Status=?,Editor=?,Udt=? where ID=?
        /// </summary>
        /// <param name="lineStation"></param>
        void UpdateLineStation(LineStation lineStation);

        /// <summary>
        /// SELECT [Line]
        ///      ,[Descr]
        ///       ,[Editor]
        ///       ,[Cdt]
        ///       ,[Udt]
        ///       ,[CustomerID]
        ///       ,[Stage]
        ///   FROM [IMES_GetData_Datamaintain].[dbo].[Line]
        /// where [CustomerID]='CustomerID' AND [Stage]='stage'
        /// order by [Line]
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        DataTable GetLineInfoList(string customer, string stage);

        /// <summary>
        /// SELECT [Stage]
        ///   FROM [IMES_GetData_Datamaintain].[dbo].[Stage]
        /// order by [Stage]
        /// </summary>
        /// <returns></returns>
        DataTable GetStageList();

        /// <summary>
        /// DELETE FROM [Line]
        ///       WHERE Line='line'
        /// </summary>
        /// <param name="line"></param>
        void DeleteLine(string line);

        /// <summary>
        /// IF EXISTS(
        /// SELECT [Line]
        ///         FROM [IMES_GetData_Datamaintain].[dbo].[Line]
        /// where [Line]='line'
        /// )
        /// set @return='True'
        /// ELSE
        /// set @return='False'
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        bool IsExistLine(string line);

        /// <summary>
        /// 新增Line
        /// </summary>
        /// <param name="item"></param>
        void AddLine(Line item);

        /// <summary>
        /// 更新Line
        /// </summary>
        /// <param name="item"></param>
        /// <param name="oldLineId"></param>
        void UpdateLine(Line item, string oldLineId);

        /// <summary>
        /// 1) 
        /// IF EXISTS(
        /// SELECT [Line] 
        /// FROM [iMES2012].[dbo].[Line]
        /// where [Descr]='Descr'
        /// )
        /// set @return='True'
        /// ELSE
        /// set @return='False'
        /// </summary>
        /// <param name="descr"></param>
        /// <returns></returns>
        bool IsExistLineDescr(string descr);

        /// <summary>
        /// 2) 
        /// IF EXISTS(
        /// SELECT [Line] 
        /// FROM [iMES2012].[dbo].[Line]
        /// where [Descr]='descr' AND Line <> 'line'
        /// )
        /// set @return='True'
        /// ELSE
        /// set @return='False'
        /// </summary>
        /// <param name="descr"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        bool IsExistLineDescrExceptLine(string descr, string line);

        #region Defered

        void DeleteLineStationByIDDefered(IUnitOfWork uow, int id);

        void AddLineStationDefered(IUnitOfWork uow, LineStation lineStation);

        void UpdateLineStationDefered(IUnitOfWork uow, LineStation lineStation);

        void DeleteLineDefered(IUnitOfWork uow, string line);

        void AddLineDefered(IUnitOfWork uow, Line item);

        void UpdateLineDefered(IUnitOfWork uow, Line item, string oldLineId);

        #endregion

        #endregion

        #region
        void FillLineSpeed(Line line);
        void FillLineEx(Line line);
        #endregion

        #region for LineSpeed Insert/Delete/Update
        DataTable GetLineByStage(string customer, string stage);
        IList<LineSpeed> GetLineSpeedByLineStation(string customer, string stage, string station);
        void AddLineSpeed(LineSpeed lineSpeed);
        void UpdateLineSpeed(LineSpeed lineSpeed);
        void RemoveLineSpeed(string aliasLine, string station);
        void RemoveLineSpeedByStation(string station);
        void AddLineSpeedDefered(IUnitOfWork uow, LineSpeed lineSpeed);
        void UpdateLineSpeedDefered(IUnitOfWork uow, LineSpeed lineSpeed);
        void RemoveLineSpeedDefered(IUnitOfWork uow,string aliasLine, string station);
        void RemoveLineSpeedByStationDefered(IUnitOfWork uow, string station);
        IList<string> GetAllAliasLine();
        

        #endregion
    }
}
