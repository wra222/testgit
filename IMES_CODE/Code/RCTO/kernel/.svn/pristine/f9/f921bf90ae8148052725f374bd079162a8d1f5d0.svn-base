// INVENTEC corporation (c)2009 all rights reserved. 
// Description: COAStatus对象Repository接口
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-26   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;

namespace IMES.FisObject.PAK.COA
{
    /// <summary>
    /// COAStatus对象Repository接口
    /// </summary>
    public interface ICOAStatusRepository : IRepository<COAStatus>
    {
        /// <summary>
        /// 晚加载COALog
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        COAStatus FillCOALogs(COAStatus item);

        /// <summary>
        /// Update COAMas(更新MSCOA 状态，WC = 'A2')
        /// </summary>
        /// <param name="item"></param>
        void UpdateCOAMas(COAMasInfo item);

        /// <summary>
        /// Insert CSNLog(记录CN Card Log，Tp = 'CNCard' ，WC = 'A2'，IsPass = '0') 
        /// </summary>
        /// <param name="item"></param>
        void InsertCSNLog(CSNLogInfo item);

        /// <summary>
        /// Update CSNMas(更新MSCOA 状态，WC = 'P1')
        /// </summary>
        /// <param name="item"></param>
        void UpdateCSNMas(CSNMasInfo item);

        /// <summary>
        /// get COAStatus range
        /// </summary>
        /// <param name="beginNo">begin No</param>
        /// <param name="endNo">end No</param>
        /// <returns></returns>
        IList<COAStatus> GetCOAStatusRange(string beginNo, string endNo);

        /// <summary>
        /// Insert COALog
        /// </summary>
        /// <param name="newLog"></param>
        void InsertCOALog(COALog newLog);

        /// <summary>
        /// 记录入COATrans_Log
        /// </summary>
        /// <param name="newLog"></param>
        void InsertCOATransLog(COATransLog newLog);

        /// <summary>
        /// 获取该COA No的最新COALog记录的Station
        /// </summary>
        /// <param name="coano"></param>
        /// <returns></returns>
        string GetStationOfNewestCOALog(string coano);

        /// <summary>
        /// update COAStatus by PK.
        /// </summary>
        /// <param name="item"></param>
        void UpdateCOAStatus(IMES.FisObject.PAK.COA.COAStatus item);

        /// <summary>
        /// get CSNMas collection by range
        /// </summary>
        /// <param name="beginNo">begin No</param>
        /// <param name="endNo">end No</param>
        /// <returns></returns>
        IList<CSNMasInfo> GetCSNMasRange(string beginNo, string endNo);

        /// <summary>
        /// 1,保存数据到TmpTable中.(TmpTable（结构基本与COAReceive同，多一个栏位PC）)
        /// </summary>
        /// <param name="dataLst"></param>
        void SaveTXTIntoTmpTable(IList<TmpTableInfo> dataLst);

        /// <summary>
        /// 2,清除当前Client端 位于TmpTable表中的记录
        /// SQL:delete from tmptable where pc =[pc]
        /// </summary>
        /// <param name="pc"></param>
        void RemoveTmpTableItem(string pc);

        /// <summary>
        /// 3,查找TmpTable中的数据.
        /// SQL:select from tmptable where pc=[pc]
        /// </summary>
        /// <param name="pc"></param>
        /// <returns></returns>
        IList<TmpTableInfo> GetTmpTableItem(string pc);

        /// <summary>
        /// 4,保存记录到COAReceive表中,
        /// </summary>
        /// <param name="items"></param>
        void SaveItemIntoCOAReceiveTable(IList<COAReceive> items);

        /// <summary>
        /// 为COAReceive表每一COA Range记录BegNo/EndNo范围内的号码创建一个新的COA记录到COAStatus数据表
        /// </summary>
        /// <param name="items"></param>
        void SaveItemIntoCOAStatusTable(IList<COAStatus> items);

        /// <summary>
        /// 查询COAReceive表中所有的记录
        /// order by BegSN,EndSN
        /// </summary>
        /// <returns></returns>
        IList<COAReceive> GetAllCOAReceivingItems();

        #region Not Implement(210003)

        /// <summary>
        /// 按coa_sn检索IMES_PAK..COAStatus.Status表。
        /// 没有查询到数据，返回null;
        /// </summary>
        /// <param name="coa_sn"></param>
        /// <returns></returns>
        COAStatus GetCoaStatus(string coa_sn);

        /// <summary>
        /// 由csn2得到CSNMas信息。
        /// </summary>
        /// <param name="csn2"></param>
        /// <returns>没有返回null。</returns>
        CSNMasInfo GetCsnMas(string csn2);

        #endregion

        /// <summary>
        /// 1）若目标状态为“P0”或“D1”，则查找有无与BegNo相同IECPN而Cdt更早且COASN更小的记录存在
        /// IMES_PAK..COAStatus
        /// </summary>
        /// <param name="statuses"></param>
        /// <param name="iecPn"></param>
        /// <param name="cdtEnd"></param>
        /// <param name="coaSnEnd"></param>
        /// <returns></returns>
        IList<COAStatus> GetCOAStatusListByStatusAndIecPn(string[] statuses, string iecPn, DateTime cdtEnd, string coaSnEnd);

        /// <summary>
        /// 添加 CSNMas
        /// </summary>
        /// <param name="item"></param>
        void InsertCSNMas(CSNMasInfo item);

        /// <summary>
        /// select MAX CSN1 from CSNMas
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        string GetMaxCSN1FromCSNMas(CSNMasInfo condition);

        void UpdateCSNMas(CSNMasInfo setValue, CSNMasInfo condition);

        /// <summary>
        /// UPDATE COAReturn SET Status = @cause, Editor = @user, Udt = GETDATE() WHERE COASN = @coano
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateCOAReturnInfo(COAReturnInfo setValue, COAReturnInfo condition);

        /// <summary>
        /// 如果该COA No在COAReturn表中存在,但不存在Status为空的记录,则报告错误:"该COA No 已Scrap!"
        /// </summary>
        /// <param name="eqCondition"></param>
        /// <param name="isNullCondition"></param>
        /// <returns></returns>
        IList<COAReturnInfo> GetCOAReturnInfoList(COAReturnInfo eqCondition, COAReturnInfo isNullCondition);

        /// <summary>
        /// Insert ECOAReturn
        /// </summary>
        /// <param name="item"></param>
        void InsertEcoareturn(EcoareturnInfo item);

        /// <summary>
        /// INSERT INTO COAReturn (CUSTSN, COASN, OriginalStatus, Status, Line, Editor, Cdt, Udt)
        /// </summary>
        /// <param name="item"></param>
        void InsertCOAReturn(COAReturnInfo item);

        #region . Defered .

        void UpdateCOAMasDefered(IUnitOfWork uow, COAMasInfo item);

        void InsertCSNLogDefered(IUnitOfWork uow, CSNLogInfo item);

        void UpdateCSNMasDefered(IUnitOfWork uow, CSNMasInfo item);

        void InsertCOALogDefered(IUnitOfWork uow, COALog newLog);

        void InsertCOATransLogDefered(IUnitOfWork uow, COATransLog newLog);

        void UpdateCOAStatusDefered(IUnitOfWork uow, IMES.FisObject.PAK.COA.COAStatus item);

        void SaveTXTIntoTmpTableDefered(IUnitOfWork uow, IList<TmpTableInfo> dataLst);

        void RemoveTmpTableItemDefered(IUnitOfWork uow, string pc);

        void SaveItemIntoCOAReceiveTableDefered(IUnitOfWork uow, IList<COAReceive> items);

        void SaveItemIntoCOAStatusTableDefered(IUnitOfWork uow, IList<COAStatus> items);

        void InsertCSNMasDefered(IUnitOfWork uow, CSNMasInfo item);

        void UpdateCSNMasDefered(IUnitOfWork uow, CSNMasInfo setValue, CSNMasInfo condition);

        void UpdateCOAReturnInfoDefered(IUnitOfWork uow, COAReturnInfo setValue, COAReturnInfo condition);

        void InsertEcoareturnDefered(IUnitOfWork uow, EcoareturnInfo item);

        void InsertCOAReturnDefered(IUnitOfWork uow, COAReturnInfo item);

        #endregion
    }
}
