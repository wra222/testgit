﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Repair;
//
using IMES.FisObject.Common.TestLog;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using System.Data;

namespace IMES.FisObject.PCA.MB
{

    /// <summary>
    /// 用于访问MB对象的Repository接口
    /// </summary>
    public interface IMBRepository : IRepository<IMB>
    {
        ///// <summary>
        ///// 获取最大MBNO
        ///// </summary>
        ///// <param name="mbCode">MbCode</param>
        ///// <param name="mbType">MbType</param>
        ///// <returns>最大MBNO</returns>
        //string GetMaxMBNO(string mbCode, string mbType);

        /// <summary>
        /// 根据前缀获取最大MBNO
        /// </summary>
        /// <param name="preStr">前缀</param>
        /// <returns>最大MBNO<</returns>
        string GetMaxMBNO(string preStr);

        /// <summary>
        /// 更新MBNO最大号(NumControl)
        /// </summary>
        /// <param name="mbCode">MbCode</param>
        /// <param name="mbType">MbType</param>
        /// <param name="maxMO">MO最大号</param>
        void SetMaxMBNO(string mbCode, string mbType, IMB maxMO);

        /// <summary>
        /// 晚加载MBLogs
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        IMB FillMBLogs(IMB mb);

        /// <summary>
        /// 晚加载MBInfos
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        IMB FillMBInfos(IMB mb);

        /// <summary>
        /// 晚加载MBParts
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        IMB FillMBParts(IMB mb);

        /// <summary>
        /// 晚加载RptRepairs
        /// select * from rpt_PCARep where SnoId=@MBSNo order by Cdt
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        IMB FillMBRptRepairs(IMB mb);
        
        /// <summary>
        /// 晚加载Repairs
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        IMB FillRepairs(IMB mb);

        /// <summary>
        /// 晚加载TestLogs
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        IMB FillTestLogs(IMB mb);

        /// <summary>
        /// 晚加载Repair Defect
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        Repair FillRepairDefectInfo(Repair rep);

        /// <summary>
        /// 晚加载TestLog Defect
        /// </summary>
        /// <param name="testLog"></param>
        /// <returns></returns>
        TestLog FillTestLogDefectInfo(TestLog testLog);

        /// <summary>
        /// 晚加载MB Statu
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        IMB FillStatus(IMB mb);

        /// <summary>
        /// 晚加载Model对象
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        IMB FillModelObj(IMB mb);

        void FillPCBAttributes(IMB item);

        void FillPCBAttributeLogs(IMB item);

        /// <summary>
        /// 删除指定区间内的MB, MBStatus, 直接写db
        /// </summary>
        /// <param name="startSn">startSn</param>
        /// <param name="endSn">endSn</param>
        void DeleteMBSection(string startSn, string endSn);

        /// <summary>
        /// 获取指定区间中的MB列表
        /// </summary>
        /// <param name="startSn">startSn</param>
        /// <param name="endSn">endSn</param>
        /// <returns></returns>
        IList<IMB> GetMBBySection(string startSn, string endSn);

        /// <summary>
        /// 获取指定MO,指定区间中的MB列表
        /// </summary>
        /// <param name="startSn"></param>
        /// <param name="endSn"></param>
        /// <param name="mo"></param>
        /// <returns></returns>
        IList<IMB> GetMBBySectionAndMO(string startSn, string endSn, string mo);

        ///// <summary>
        ///// 生成MO Dismantle log, 直接写db
        ///// </summary>
        ///// <param name="mo">mo</param>
        ///// <param name="mbsnList">mb sn list</param>
        //void InsertMODismantleLog(string mo, IList<string> mbsnList);

        /// <summary>
        /// 根据时间戳和MAC生成UUID
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="currentTime"></param>
        /// <param name="currentUTCTime"></param>
        /// <returns></returns>
        string GenerateUUID(string mac, DateTime currentTime, DateTime currentUTCTime);

        /// <summary>
        /// 检查输入的cpuVendorSn是否已经和MB绑定
        /// 用于CombineMBCPU之前检查CPU是否已与其他MB绑定;
        /// 检查PCB表的CVSN栏位如果等于输入的cpuVendorSn存在返回和它绑定的MBSNO
        /// 不存在返回""
        /// select top 1 PCBNo from PCB where CVSN=@CVSN
        /// </summary>
        /// <param name="cpuVendorSn"></param>
        /// <returns></returns>
        string IsUsedCvsn(string cpuVendorSn);

        /// <summary>
        /// 获取指定mb的指定site,component的维修完成的RepairDefect列表
        /// select * from PCBRepair_DefectInfo inner join PCBRepair on PCBRepair_DefectInfo.PCARepairID = PCBRepair.ID where PCBNo = '' and Component = '' and Site = '' and Cause is not null 
        /// </summary>
        /// <param name="mbsn">mbsn</param>
        /// <param name="site">site</param>
        /// <param name="component">component</param>
        /// <returns>RepairDefect列表</returns>
        IList<RepairDefect> GetRepairDefectBySiteComponent(string mbsn, string site, string component);

        /// <summary>
        /// 获得MB的Type和MB Code
        /// </summary>
        /// <param name="part"></param>
        /// <param name="Mb_Code"></param>
        /// <returns></returns>
        string TryToGetMBType(IPart part, out string Mb_Code);

        /// <summary>
        /// 根据MAC获得MB
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        IMB GetMBByMAC(string mac);

        /// <summary>
        /// 根据MAC获得MB列表
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        IList<IMB> GetMBListByMAC(string mac);

        /// <summary>
        /// 根据PN和Value获得PCB Part的列表
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IList<IProductPart> GetPCBPartsByPartNoAndValue(string partNo, string val);

        /// <summary>
        /// 根据PartSn获得PCB Part的列表
        /// </summary>
        /// <param name="partSn"></param>
        /// <returns></returns>
        IList<IProductPart> GetPCBPartsByPartSn(string partSn);

        /// <summary>
        /// 根据PCBInfo表的InfoType和InfoValue获取PCB对象	
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        IList<MBInfo> GetPCBInfoByTypeValue(string infoType, string infoValue);

        /// <summary>
        /// 查询在MBNOList范围内的MBNO，对应的MBStatus.Station 不为stationId  的MBNO记录集
        /// </summary>
        /// <param name="mbNOList"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        IList<string> FilterMBNOList(IList<string> mbNOList, string stationId);

        /// <summary>
        /// 批删除MB记录
        /// </summary>
        /// <param name="items"></param>
        void RemoveBatch(IList<IMB> items);

        /// <summary>
        /// 批更新MB的Status
        /// </summary>
        /// <param name="mbstts"></param>
        void UpdateMBStatusBatch(IList<MBStatus> mbstts);

        /// <summary>
        /// 批增加MB的Log
        /// </summary>
        /// <param name="mblogs"></param>
        void AddMBLogBatch(IList<MBLog> mblogs);

        /// <summary>
        /// select Station from IMES_PCA..PCBLog where PCBNo='' (Cdt最大的)
        /// </summary>
        /// <param name="pcbno"></param>
        /// <returns></returns>
        string GetTheNewestStationFromPCBLog(string pcbno);

        /// <summary>
        /// Replace Old MB Data with New MB Sno
        /// </summary>
        /// <param name="oldSn">oldSn</param>
        /// <param name="newSn">newSn</param>
        /// <remarks>
        /// 将下列各表中的Old MB Sno 对应记录的PCBNo 栏位Update 为New MB Sno
        /// IMES_PCA..MODismantleLog
        /// IMES_PCA..PCB
        /// IMES_PCA..PCBInfo
        /// IMES_PCA..PCBLog
        /// IMES_PCA..PCBRepair
        /// IMES_PCA..PCBStatus
        /// IMES_PCA..PCBTestLog
        /// IMES_PCA..PCB_Part
        /// IMES_PCA..SnoLog3D
        /// IMES_PCA..TransferToFISList
        /// </remarks>
        void ReplaceMBSn(string oldSn, string newSn);

        /// <summary>
        /// 2、Repair List
        /// 参考方法：
        /// select SnoId,Remark,Username,Cdt,Udt from rpt_PCARep nolock where Tp='BGA' and SnoId=@MBSNo order by Cdt
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="snoId"></param>
        /// <returns></returns>
        IList<RptPcaRepInfo> GetRptPcaRepByTpAndSnoID(string tp, string snoId);

        /// <summary>
        /// if exists(select SnoId from rpt_PCARep (nolock) where SnoId=@MBSno and Tp='BGA' and Remark=@remark)
        /// </summary>
        /// <param name="snoId"></param>
        /// <param name="tp"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        bool CheckExistRptPcaRepBySnoIdAndTpAndRemark(string snoId, string tp, string remark);

        /// <summary>
        /// Insert rpt_PCARep
        /// SnoId = @MBSno
        /// Tp = ‘BGA’
        /// Status= ‘1’
        /// Mark = ‘0’
        /// Remark = [Rework Station]
        /// </summary>
        /// <param name="item"></param>
        void InsertRptPcaRep(RptPcaRepInfo item);

        /// <summary>
        /// 2.添加MB_Test一条数据
        /// addMB_Test(MB_TestDef mbTest);
        /// </summary>
        /// <param name="mbTest"></param>
        void AddMBTest(MBTestDef mbTest);

        /// <summary>
        /// 3.删除一条MB_Test数据
        /// DELETE FROM MB_Test
        /// WHERE Code = @code and Family=@family and Type=@type
        /// </summary>
        /// <param name="code"></param>
        /// <param name="family"></param>
        /// <param name="type"></param>
        void DeleteMBTest(string code, string family, bool type);

        /// <summary>
        /// 4.根据family显示所有testMB数据
        /// select * from MBTest
        /// where family=@family
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<MBTestDef> GetMBTestbyFamily(string family);

        /// <summary>
        /// C for MB 
        /// Update PCA..PCBStatus.Station=30,Status=0
        /// （setValue哪个字段赋值就有更新哪个字段;condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.）
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePCBStatus(PCBStatusInfo setValue, PCBStatusInfo condition);

        /// <summary>
        /// 根据code,family和type获取所有testMb列表
        /// </summary>
        /// <param name="code"></param>
        /// <param name="family"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<MBTestDef> GetMBTestByCodeFamilyAndType(string code, string family, bool type);

        /// <summary>
        /// 1.获取所有MBCFG数据
        /// 按照MBCode字段排序
        /// </summary>
        /// <returns></returns>
        IList<MBCFGDef> GetAllMBCFGLst();

        /// <summary>
        /// 3.添加一条MBCFG数据
        /// </summary>
        /// <param name="mbcfgDef"></param>
        void AddMBCFG(MBCFGDef mbcfgDef);

        /// <summary>
        /// 4.删除一条MBCFG数据
        /// </summary>
        /// <param name="id"></param>
        void DeleteMBCFG(int id);

        /// <summary>
        /// 5.根据mbcode,series和Type修改MBCFG数据
        /// </summary>
        /// <param name="mbcfgDef"></param>
        /// <param name="mbCode"></param>
        /// <param name="series"></param>
        /// <param name="type"></param>
        void UpdateMBCFG(MBCFGDef mbcfgDef,string mbCode,string series,string type);

        /// <summary>
        /// 6.根据MBcode,series,和type获取一条MBCFG数据
        /// </summary>
        /// <param name="mbCode"></param>
        /// <param name="serices"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<MBCFGDef> GetMBCFGByCodeSeriesAndType(string mbCode,string serices,string type);

        /// <summary>
        /// 1,SQL:select * from Maintain_ITCNDefect_Check order by family
        /// </summary>
        /// <returns></returns>
        IList<ITCNDDefectCheckDef> GetAllITCNDDefectChecks();

        /// <summary>
        /// 2,SQL:insert into Maintain_ITCNDDefect_Check values(各个字段)
        /// </summary>
        /// <param name="item"></param>
        void AddITCNDDefectCheck(ITCNDDefectCheckDef item);

        /// <summary>
        /// 3,SQL:delete from Maintain_ITCNDDefect_Chekc where family=[family]
        /// </summary>
        /// <param name="family"></param>
        void RemoveITCNDDefectCheck(string family);

        /// <summary>
        /// SQL: Delete from Maintain_ITCNDDefect_Chekc where family=[family] and code=[code]
        /// </summary>
        /// <param name="family"></param>
        /// <param name="code"></param>
        void RemoveITCNDDefectCheckbyFamilyAndCode(string family, string code);

        /// <summary>
        /// select @Count=COUNT(*) from PCB nolock where PCBNo = @MBSno
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <returns></returns>
        int GetCountOfPCB(string pcbNo);

        /// <summary>
        /// select * from PCBRepair nolock where PCBNo=@MBSno and Station='33'
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<Repair> GetPcbRepairList(string pcbNo, string station);

        /// <summary>
        /// select @cdt=max(Cdt) from PCBRepair nolock where PCBNo=@MBSno and Station='33'
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        DateTime GetMaxCdtFromPCBRepair(string pcbNo, string station);

        /// <summary>
        /// select * from PCBStatus nolock where PCBNo=@MBSno and Station='24'
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<MBStatus> GetPcbStatusList(string pcbNo, string station);

        /// <summary>
        /// select * from Maintain_ITCNDefect_Check where Code=left(@MBSno,2) and Type='M/B'
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<ITCNDDefectCheckDef> GetITCNDDefectChecks(string mbSno, string type);
        //Code=@MBSno
        IList<ITCNDDefectCheckDef> GetITCNDDefectChecks_NotCut(string mbSno, string type);

        /// <summary>
        /// select @remark=Remark from PCBTestLog nolock where PCBNo=@MBSno and Type='M/B' and Status=1 and Station=@Station and Cdt>@cdt
        /// select * from PCBTestLog nolock where PCBNo = @MBSno and Type in ('MB','M/B','RCTO') and Status = ‘0’ and Station=@Station and Cdt>@Rdt
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="types"></param>
        /// <param name="status"></param>
        /// <param name="station"></param>
        /// <param name="beginCdt"></param>
        /// <returns></returns>
        IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, string[] types, int status, string station, DateTime beginCdt);

        IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, string[] types, string station, DateTime beginCdt);

        /// <summary>
        /// Select * from MB_Test nolock where RTRIM(Code) = LEFT(@MBSno,2) and Type=0
        /// select * from MB_Test nolock where Code=left(@MBSno,2) and Type=0
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<MBTestDef> GetMBTestList(string mbSno, bool type);
        //Code=@MBSno
        IList<MBTestDef> GetMBTestList_NotCut(string mbSno, bool type);

        /// <summary>
        /// select @Rdt=MAX(Cdt) from PCBLog nolock where PCBNo = @MBSno
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <returns></returns>
        DateTime GetMaxCdtFromPCBLog(string pcbNo);

        /// <summary>
        /// select * from Maintain_ITCNDefect_Check nolock where Code=left(@MBSno,2)
        /// </summary>
        /// <param name="mbSno"></param>
        /// <returns></returns>
        IList<ITCNDDefectCheckDef> GetITCNDDefectChecks(string mbSno);
        // Code=@MBSno
        IList<ITCNDDefectCheckDef> GetITCNDDefectChecks_NotCut(string mbSno);

        /// <summary>
        /// select * from PCBTestLog nolock where PCBNo=@MBSno and Status='1' and Station=@Station and Cdt>@Rdt
        /// select * from PCBTestLog nolock where PCBNo=@MBSno and Cdt>@Rdt and Station=@Station and Status='1'
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="status"></param>
        /// <param name="station"></param>
        /// <param name="beginCdt"></param>
        /// <returns></returns>
        IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, int status, string station, DateTime beginCdt);

        /// <summary>
        /// select * from PCBTestLog nolock where PCBNo = @MBSno and Station=当前站 and Cdt>PCBLog中当前MB的最大过站时间
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="status"></param>
        /// <param name="station"></param>
        /// <param name="beginCdt"></param>
        /// <returns></returns>
        IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, string station, DateTime beginCdt);

        /// <summary>
        /// select * from PCBInfo nolock where InfoType='OLD MB' and PCBNo in (@OldMBSno,@NewMBSno)
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="pcbNos"></param>
        /// <returns></returns>
        IList<MBInfo> GetMbInfoListByInfoTypeAndPcbNoList(string infoType, string[] pcbNos);

        /// <summary>
        /// update [MTA_Mark] Mark = ‘1’
        /// </summary>
        /// <param name="repairId"></param>
        /// <param name="mark"></param>
        void UpdateMtaMarkByRepairId(int repairId, string mark);

        /// <summary>
        /// Insert MTA_Mark
        /// </summary>
        /// <param name="item"></param>
        void InsertMtaMarkInfo(MtaMarkInfo item);

        /// <summary>
        /// SELECT DISTINCT b.InfoValue as [MB Code], c.InfoValue as [MDL],b.InfoValue + ' ' + c.InfoValue as [DisplayName]
        /// FROM IMES2012_GetData..Part a, IMES2012_GetData..PartInfo b, IMES2012_GetData..PartInfo c
        /// WHERE a.BomNodeType = 'MB'
        ///  AND a.PartNo = b.PartNo
        ///  AND a.PartNo = c.PartNo
        ///  AND b.InfoType = 'MB'
        ///  AND c.InfoType = 'MDL'
        ///  AND c.InfoValue LIKE '%B SIDE'
        /// order by b.InfoValue
        /// 
        /// SELECT DISTINCT b.InfoValue as [MB Code], c.InfoValue as [MDL],b.InfoValue + ' ' + c.InfoValue as [DisplayName]
        /// FROM IMES2012_GetData..Part a, IMES2012_GetData..PartInfo b, IMES2012_GetData..PartInfo c
        /// WHERE a.BomNodeType = @bomNodeType
        ///  AND a.PartNo = b.PartNo
        ///  AND a.PartNo = c.PartNo
        ///  AND b.InfoType = @mbType
        ///  AND c.InfoType = @mdlType
        ///  AND c.InfoValue LIKE '%' + @mdlPostfix
        /// order by b.InfoValue
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mdlType"></param>
        /// <param name="mdlPostfix"></param>
        /// <returns></returns>
        IList<MbCodeAndMdlInfo> GetMbCodeAndMdlInfoList(string bomNodeType, string mbType, string mdlType, string mdlPostfix);

        /// <summary>
        /// select distinct b.PartNo 
        /// from IMES2012_GetData..Part a(nolock),IMES2012_GetData..PartInfo b(nolock) 
        /// where a.PartNo = b.PartNo 
        /// and a.BomNodeType = 'MB'
        /// and b.InfoType = 'MB'
        /// and b.InfoValue = @MBCode
        /// order by b.PartNo
        /// 
        /// select distinct b.PartNo 
        /// from IMES2012_GetData..Part a(nolock),IMES2012_GetData..PartInfo b(nolock) 
        /// where a.PartNo = b.PartNo 
        /// and a.BomNodeType = @bomNodeType 
        /// and b.InfoType = @mbType 
        /// and b.InfoValue = @mbCode
        /// order by b.PartNo
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mbCode"></param>
        /// <returns></returns>
        IList<string> GetPartNoListByInfo(string bomNodeType, string mbType, string mbCode);

        /// <summary>
        /// SELECT TOP 1 b.InfoValue as [MB Code], c.InfoValue as [MDL],b.InfoValue + ' ' + c.InfoValue as [DisplayName]
        /// FROM IMES2012_GetData..Part a, IMES2012_GetData..PartInfo b, IMES2012_GetData..PartInfo c
        /// WHERE a.BomNodeType = 'MB'
        ///  AND a.PartNo = b.PartNo
        ///  AND a.PartNo = c.PartNo
        ///  AND b.InfoType = 'MB'
        ///  AND c.InfoType = 'MDL'
        ///  AND a.PartNo = @pno
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        MbCodeAndMdlInfo GetMbCodeAndMdlInfoByPnoForMB(string pno);

        /// <summary>
        /// select * from PCBRepair where PCBNo=@MBSno and Status=@Status
        /// </summary>
        /// <param name="mbSno"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool ExistPCBRepair(string mbSno, int status);

        /// <summary>
        /// SELECT * BorrowLog WHERE Sn=@Sn and Status=@Status
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<BorrowLog> GetBorrowLogBySno(string sn, string status);

        /// <summary>
        /// UPDATE [IMES_FA].[dbo].[BorrowLog] SET Returner=@Lender, Accepter=@Editor, Status='R', 
        ///  Rdate=GETDATE()
        /// WHERE Sno=@MBSno
        /// AND Status=@Status 
        /// </summary>
        /// <param name="item"></param>
        void UpdateBorrowLog(BorrowLog item, string statusCondition);
        
        /// <summary>
        /// SQL：SELECT DISTINCT b.InfoValue as [MB Code], c.InfoValue as [MDL],b.InfoValue + ' ' + c.InfoValue as [DisplayName]
        /// FROM IMES2012_GetData..Part a, IMES2012_GetData..PartInfo b, IMES2012_GetData..PartInfo c,IMES2012_GetData..PartInfo d,SMTMO e
        /// WHERE a.BomNodeType = 'MB'
        /// AND a.PartNo = b.PartNo
        /// AND a.PartNo = c.PartNo
        /// And a.PartNo = d.PartNo
        /// And a.PartNo = e.IECPartNo
        /// AND b.InfoType = 'MB'
        /// AND c.InfoType = 'MDL'
        /// AND c.InfoValue LIKE '%B SIDE'
        /// And d.InfoType = 'VGA'
        /// And d.InfoValue = 'SV'
        /// AND e.PrintQty ＜ e.Qty
        /// order by b.InfoValue
        ///
        /// SQL：SELECT DISTINCT b.InfoValue as [MB Code], c.InfoValue as [MDL],b.InfoValue + ' ' + c.InfoValue as [DisplayName]
        /// FROM IMES2012_GetData..Part a, IMES2012_GetData..PartInfo b, IMES2012_GetData..PartInfo c,IMES2012_GetData..PartInfo d,SMTMO e
        /// WHERE a.BomNodeType = @bomNodeType
        /// AND a.PartNo = b.PartNo
        /// AND a.PartNo = c.PartNo
        /// And a.PartNo = d.PartNo
        /// And a.PartNo = e.IECPartNo
        /// AND b.InfoType = @mbType
        /// AND c.InfoType = @mdlType
        /// AND c.InfoValue LIKE '%' + @mdlPostfix
        /// And d.InfoType = @vgaType
        /// And d.InfoValue = @vgaValue
        /// AND e.PrintQty ＜ e.Qty
        /// order by b.InfoValue
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="mbType"></param>
        /// <param name="mdlType"></param>
        /// <param name="mdlPostfix"></param>
        /// <param name="vgaType"></param>
        /// <param name="vgaValue"></param>
        /// <returns></returns>
        IList<MbCodeAndMdlInfo> GetMbCodeAndMdlInfoList(string bomNodeType, string mbType, string mdlType, string mdlPostfix, string vgaType, string vgaValue);

        /// <summary>
        /// 1. Update [rpt_PCARep] Status = '1'
        /// setValue哪个字段赋值就有更新哪个字段;condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateRptPcaRepInfo(RptPcaRepInfo setValue, RptPcaRepInfo condition);

        /// <summary>
        /// 2. Update [MTA_Mark] Mark = '1'
        /// setValue哪个字段赋值就有更新哪个字段;condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateMtaMarkInfo(MtaMarkInfo setValue, MtaMarkInfo condition);

        /// <summary>
        /// 3.SQL:EXISTS( select COUNT(1) from PCBRepair a (nolock) inner join PCBRepair_DefectInfo b (nolock) on a.ID=b.PCARepairID 
        /// where left(b.Cause,2)=@causePrefix and a.PCBNo=@mbSno )
        /// </summary>
        /// <param name="causePrefix"></param>
        /// <returns></returns>
        bool CheckExistPcbRepairWithDefectOfCertainCausePrefix(string causePrefix, string mbSno);

        /// <summary>
        /// 4.SQL:select PCBRepair_DefectInfo.ID 
        /// from PCBRepair_DefectInfo inner join PCBRepair on PCBRepair_DefectInfo.PCARepairID = PCBRepair.ID 
        /// where PCBRepair.PCBNo = @MBSno
        /// </summary>
        /// <param name="mbSno"></param>
        /// <returns></returns>
        IList<int> GetPcbRepairDefectInfoIdListWithDefect(string mbSno);

        /// <summary>
        /// select * from PCBLog where PCBNo=@PCBNo and Station=@Station and Status=@Status order by Cdt
        /// </summary>
        /// <param name="pcbno"></param>
        /// <param name="station"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<MBLog> GetMBLog(string pcbno, string station, int status);

        IList<MBLog> GetMBLog(string pcbno, string station);

        /// <summary>
        /// select * from MBCode where MBCode=@MBCode
        /// </summary>
        /// <param name="mbCode"></param>
        /// <returns></returns>
        MBCodeDef GetMBCode(string mbCode);

        /// <summary>
        /// select top 1 InfoValue from PCBInfo where PCBNo=@pcbno and InfoType=@infotype order by ID desc
        /// </summary>
        /// <param name="pcbno"></param>
        /// <param name="infotype"></param>
        /// <returns></returns>
        string GetPCBInfoValue(string pcbno, string infotype);
    
        /// <summary>
        /// select Qty 
        /// from PCAICTCount 
        /// where Cdt='1900-01-01 00:00:00.000' and PdLine=@PdLine
        /// </summary>
        /// <param name="cdt"></param>
        /// <param name="pdLine"></param>
        /// <returns></returns>
        IList<int> GetQtyListFromPcaIctCountByCdtAndPdLine(DateTime cdt, string pdLine);
    
        /// <summary>
        /// Update PCAICTCount
        /// set Qty=0
        /// where PdLine=@PdLine and Cdt='1900-01-01 00:00:00.000'
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="cdt"></param>
        /// <param name="pdLine"></param>
        void UpdateQtyFromPcaIctCountByCdtAndPdLine(int qty, DateTime cdt, string pdLine);

        /// <summary>
        /// insert into PCAICTCount values(@PdLine,@Qty ,getdate())
        /// </summary>
        /// <param name="item"></param>
        void InsertPcaIctCountInfo(PcaIctCountInfo item);

        /// <summary>
        /// select top 1 * from MBCFG where TP=@Type and MBCode=@MBCode order by Udt Desc
        /// </summary>
        /// <param name="mbCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        MBCFGDef GetMBCFG(string mbCode, string type);

        /// <summary>
        /// PCBLog.ID 
        /// Condition：PCBNo=MBSno and Status=0 Order By Cdt Desc
        /// </summary>
        /// <param name="productId">pcbNo</param>
        /// <returns>MBLog</returns>
        MBLog GetLatestFailLog(string pcbNo);

        /// <summary>
        /// PCBLog.ID from PCBLog where PCBNo=#OldMB order by Cdt desc
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <returns></returns>
        MBLog GetLatestFailLogRegardlessStatus(string pcbNo);

        /// <summary>
        /// select * from PCBTestLog nolock where PCBNo = @MBSno  order by cdt desc
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <returns></returns>
        IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo);

        /// <summary>
        /// select MAC,MBCT,HDDV,BIOS from PCATest_check where code = left(@MBSno,2)
        /// </summary>
        /// <param name="mbSno"></param>
        /// <returns></returns>
        IList<PcaTestCheckInfo> GetPcaTestCheckInfoListByCode(string mbSno);
        //code = @MBSno
        IList<PcaTestCheckInfo> GetPcaTestCheckInfoListByCode_NotCut(string mbSno);

        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PcaTestCheckInfo> GetPcaTestCheckInfoListByCode(PcaTestCheckInfo condition);

        /// <summary>
        /// Update PCA..PCBStatus.Station=30
        /// setValue哪个字段赋值就有更新哪个字段;
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="pcbIds"></param>
        void UpdatePcbStatuses(MBStatus setValue, string[] pcbIds);

        /// <summary>
        /// Insert PCA..PCBLog：Station=’DM’Status=1
        /// </summary>
        /// <param name="mbLogs"></param>
        void AddMBLogs(MBLog[] mbLogs);

		void AddMBInfoes(IList<MBInfo> mbInfoes);
 
        /// <summary>
        /// 根据参数 family,code 查询Maintain_ITCNDefect_Check表中的记录.
        /// </summary>
        /// <param name="family"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ITCNDDefectCheckDef CheckExistByFamilyAndCode(string family,string code);

        /// <summary>
        /// 1,SQL: select * from PACTest_Check where Code=[code]
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<PcaTestCheckInfo> CheckSATestCheckRuleExist(string code);

        /// <summary>
        /// 2,SQL: delete from PACTest_Check where id=[id]
        /// </summary>
        /// <param name="id"></param>
        void RemoveSATestCheckRuleItem(int id);

        /// <summary>
        /// 3,将一条item插入到PACTest_Check 表中.
        /// </summary>
        /// <param name="item"></param>
        void AddSATestCheckRuleItem(PcaTestCheckInfo item);

        /// <summary>
        /// 4,update PACTest_Check set [item各个字段].
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        void UpdateTestCheckRuleItem(PcaTestCheckInfo item, int id);

        /// <summary>
        /// SQL : Select * from PCATest_Check
        /// </summary>
        /// <returns></returns>
        IList<PcaTestCheckInfo> GetAllSATestCheckRuleItems();

        /// <summary>
        /// 1)Update PCBLot
        /// Update PCBLot.Status=0 where Status=1 and PCBNo=@MBSN
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePCBLotInfo(PcblotInfo setValue, PcblotInfo condition);
 
        /// <summary>
        /// 2)Update Lot
        /// Update Lot---Update Lot：Status='1' where LotNo=@LotNo
        /// G---Update Lot SET Status='1' where  LotNo=@LotNo
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateLotInfo(LotInfo setValue, LotInfo condition);

        /// <summary>
        /// Update Lot.Qty=Qty-1 where LotNo=@LotNo 
        /// setValue.Qty赋1，其他按需要赋值即可
        /// </summary>
        /// <param name="todec"></param>
        /// <param name="condition"></param>
        void UpdateLotInfoForDecQty(LotInfo setValue, LotInfo condition);

        /// <summary>
        /// 3) getPCBLot
        /// Select * from PCBLot where Status=1 and PCBNo=@MBSN
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PcblotInfo> GetPcblotInfoList(PcblotInfo condition);

        /// <summary>
        /// 获取[LotList]
        /// A．Select LotNo, Type, MBCode，Qty from Lot Where Line=[PdLine] and Status='0'
        /// 1）[Lot]中是否存在@MBCode和@Type记录：
        /// B．select LotNo, Type, MBCode，Qty from Lot where MBcode=left(@MBSno,2) and [Type]= @Type
        /// 2、Check @MBCode和@Type对应的Qty，
        /// B．select LotNo, Type, MBCode，Qty from Lot where MBcode=left(@MBSno,2) and [Type]= @Type（同上）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<LotInfo> GetlotInfoList(LotInfo condition);

        /// <summary>
        /// Insert Lot：LotNo=新产生No, Line=当前Line，Type=@Type, Qty=1, Status=0 
        /// C．insert into lot (lostNo, Line, Type, Qty,status,MBCode) value(@LostNo, @Line, @Type, @Qty,@Status,left(@MBSno,2))
        /// </summary>
        /// <param name="item"></param>
        void InsertLotInfo(LotInfo item);

        /// <summary>
        /// Update Lot：Qty=Qty+1, Editor, Udt where LotNo=@LotNo 
        /// D．Update Lot SET Qty=Qty+1, Editor=@Editor, Udt=now where  LotNo=@LotNo
        /// setValue.Qty赋1，其他按需要赋值即可
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateLotInfoForIncQty(LotInfo setValue, LotInfo condition);

        /// <summary>
        /// Insert PCBLot：LotNo=@LotNo, PCBNo=@MBSN, Status=1 
        /// E．Insert intoPCBLot (lotNo,PCBNo, Status) value(@LostNo,@MBSN,1)
        /// </summary>
        /// <param name="item"></param>
        void InsertPCBLotInfo(PcblotInfo item);

        /// <summary>
        /// 1、获取LotSetting表中PassQty
        /// F--select PassQty,FailQty from LotSetting where Line=@Line
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<LotSettingInfo> GetLotSettingInfoList(LotSettingInfo condition);

        /// <summary>
        /// 2、添加记录
        /// 功能：向LotSetting表中添加一条记录
        /// 入参：DS item
        /// 出参：无
        /// 返回值：void
        /// </summary>
        /// <param name="item"></param>
        void InsertLotSettingInfo(LotSettingInfo item);

        /// <summary>
        /// 3、删除记录
        /// 功能：从LotSetting表中删除Id值为输入值的记录
        /// 入参：string id
        /// 出参：无
        /// 返回值：void
        /// </summary>
        /// <param name="condition"></param>
        void DeleteLotSettingInfo(LotSettingInfo condition);

        /// <summary>
        /// select * from PCBTestLog where PCBNo='输入1' and Station='输入2' and Status='输入3'
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="status"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo, int status, string station);
        /// <summary>
        /// select * from PCBTestLog where PCBNo='输入1' and Station='输入2' 
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="status"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo,  string station);

        IList<TestLog> GetPCBTestLogListFromPCBTestLogByType(string pcbNo, int status, string type);

        /// <summary>
        /// 2、update LotSetting表记录
        /// 功能：从LotSetting表中更新制定记录
        /// 入参：DS item
        /// 出参：无
        /// 返回值：void
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateLotSettingInfo(LotSettingInfo setValue, LotSettingInfo condition);

        void InsertPcblotcheckInfo(PcblotcheckInfo item);

        /// <summary>
        /// 获取PCBLot信息，并显示
        /// select a.PCBNo, ISNULL(b.Status,'0') as Checked from PCBLot a
        ///       left Join PCBLotCheck b
        ///       on a.LotNo = b.LotNo
        ///       and a.PCBNo = b.PCBNo
        ///    where a.LotNo=@LotNo and a.Status=1
        ///    order by a.PCBNo
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        DataTable GetPcbNoAndCheckStatusList(string lotNo, string status);

        /// <summary>
        /// Insert PCBLotCheck Select LotNo, @PCBNo, ‘0’, @Editor, getdate() from PCBLot where PCBNo=@MBSN and Status=1)
        /// </summary>
        /// <param name="condition"></param>
        void InsertPCBLotCheckFromPCBLot(string pcbNo, string editor, PcblotInfo condition);

        /// <summary>
        /// select count(*) from Pcblotcheck where lotNo=@lotNo and status ='1'
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCountOfPcblotCheck(PcblotcheckInfo condition);
        
        /// <summary>
        /// 1.通过PCBStatus.PCBNo获取以下信息
        ///  [Station] = PCBStatus.Station + ‘  ’ + Station.Descr
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <returns></returns>
        IList<string> GetStationListFromPcbStatus(string pcbNo);

        /// <summary>
        /// 1.通过PCBStatus.PCBNo获取以下信息
        ///  [PdLine] = PCBStatus.Line + ‘  ’ +Line.Descr
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <returns></returns>
        IList<string> GetLineListFromPcbStatus(string pcbNo);

        /// <summary>
        /// Select LotNo,Line,Type,Qty,Status,Cdt, MBCode From Lot
        /// Where (Status=1 or Status=2)
        /// And DateDiff(day, Cdt, getdate()) &gt;= @OQCTimeSpan
        /// And Line=@PdLine
        /// And MBCode=@MBCode
        /// And Qty &gt; @LotQty
        /// Order by Cdt
        /// </summary>
        /// <param name="statuses"></param>
        /// <param name="oqcTimeSpanDays"></param>
        /// <param name="pdLine"></param>
        /// <param name="mbCode"></param>
        /// <param name="lotQty"></param>
        /// <returns></returns>
        IList<LotInfo> GetLotList(string[] statuses, int oqcTimeSpanDays, string pdLine, string mbCode, int lotQty);

        /// <summary>
        /// 9.Select * from PCBOQCRepair where Status=0 order by Cdt
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PcboqcrepairInfo> GetPcboqcrepairInfoList(PcboqcrepairInfo condition);

        /// <summary>
        /// 3.获取PCBOQCRepair  条件：Status=0 and PCBNo=@MBSN order by Cdt desc
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PcboqcrepairInfo> GetPcboqcrepairInfoListOrderByCdtDesc(PcboqcrepairInfo condition);

        /// <summary>
        /// 4.获取如下数据，显示在Defect List
        /// Select * from PCBOQCRepair_DefectInfo where PCBOQCRepairID = @PCBOQCRepairID order by Cdt
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<Pcboqcrepair_DefectinfoInfo> GetPcboqcrepairDefectinfoInfoList(Pcboqcrepair_DefectinfoInfo condition);

        /// <summary>
        /// 5.Update PCBOQCRepair
        /// 条件是@RepairID = PCBOQCRepair.ID
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePcboqcrepairInfo(PcboqcrepairInfo setValue, PcboqcrepairInfo condition);

        /// <summary>
        /// 7.Delete PCBOQCRepair_DefectInfo where PCBOQCRepairID = @RepairID
        /// </summary>
        /// <param name="condition"></param>
        void DeletePcboqcrepairInfo(PcboqcrepairInfo condition);

        /// <summary>
        /// 6.Insert PCBOQCRepair
        /// </summary>
        /// <param name="item"></param>
        void InsertPcboqcrepairInfo(PcboqcrepairInfo item);
        
        /// <summary>
        /// 8.Insert PCBOQCRepair_DefectInfo
        /// PCBOQCRepairID=@RepairID
        /// Defect = 刷入Defect
        /// </summary>
        /// <param name="item"></param>
        void InsertPcboqcrepairDefectinfo(Pcboqcrepair_DefectinfoInfo item);

        void DeletePcboqcrepairDefectinfo(Pcboqcrepair_DefectinfoInfo condition);

        /// <summary>
        /// 获得所有SMTLine记录(按Line栏位排序)
        /// </summary>
        /// <returns></returns>
        IList<SMTLineDef> GetSMTLineList();

        /// <summary>
        /// 删除对应的数据
        /// </summary>
        /// <param name="condition"></param>
        void RemoveSMTLine(SMTLineDef condition);
 
        /// <summary>
        /// 获得传入SMTLineDef结构数据在数据库中已有的记录（返回值为null或者DataTable.Rows.Count<=0视为数据库中无对应的记录）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<SMTLineDef> GetExistSMTLine(SMTLineDef condition);

        /// <summary>
        /// 保存code对应数据到数据库（需要底层处理时加上cdt和udt）
        /// </summary>
        /// <param name="item"></param>
        void AddSMTLine(SMTLineDef item);

        /// <summary>
        /// 更新对应的数据，连Line也可更新，调此接口前已经确定newItem.Line不存在且oldItem.Line存在
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void ChangeSMTLine(SMTLineDef setValue, SMTLineDef condition);

        /// <summary>
        /// select Remark,Line from Dept order by Section,substring(Line,4,1), substring(Line,3,1)
        /// </summary>
        /// <returns></returns>
        DataTable GetLineList();

        /// <summary>
        /// 1、Get [Section] Data：
        /// 参考方法：
        /// select distinct(Section) from Dept where Dept=[Dept]
        /// union 
        /// select 'SMT1A'
        /// union
        /// select 'SMT1B'
        /// Note：
        /// 注意：Union和Union ALL 的区别
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetSectionList(DeptInfo condition);

        /// <summary>
        /// 2、Get [Line] List
        /// 参考方法：
        /// select * from Dept 
        ///        where Dept = [Dept]
        ///        and Section like '[Section]%'
        /// order by Dept, Section, Line, FISLine
        /// </summary>
        /// <param name="eqCondition"></param>
        /// <param name="likeCondition"></param>
        /// <returns></returns>
        IList<DeptInfo> GetSectionList(DeptInfo eqCondition, DeptInfo likeCondition);

        /// <summary>
        /// 实现sql：select * from Dept order by Section, substring(Line,4,1), substring(Line,3,1)
        /// </summary>
        /// <returns></returns>
        IList<DeptInfo> GetDeptInfoList();

        /// <summary>
        /// 3、获取表数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<DeptInfo> GetDeptInfoList(DeptInfo condition);

        /// <summary>
        /// 4、删除记录
        /// </summary>
        /// <param name="condition"></param>
        void DeleteDeptInfo(DeptInfo condition);

        /// <summary>
        /// 5、创建记录
        /// </summary>
        /// <param name="item"></param>
        void AddDeptInfo(DeptInfo item);

        /// <summary>
        /// 6、更新记录
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateDeptInfo(DeptInfo setValue, DeptInfo condition);

        /// <summary>
        /// select distinct(Dept) from Dept 
        /// union 
        /// select 'SMT'
        /// </summary>
        /// <returns></returns>
        IList<string> GetDeptList();

        /// <summary>
        /// Family_MB增
        /// </summary>
        /// <param name="item"></param>
        void AddFamilyMbInfo(FamilyMbInfo item);

        /// <summary>
        /// Family_MB删
        /// </summary>
        /// <param name="condition"></param>
        void DeleteFamilyMbInfo(FamilyMbInfo condition);

        /// <summary>
        /// Family_MB改
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void ModifyFamilyMbInfo(FamilyMbInfo setValue, FamilyMbInfo condition);

        /// <summary>
        /// Family_MB查
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<FamilyMbInfo> GetFamilyMbInfoList(FamilyMbInfo condition);

        /// <summary>
        /// Family_MB查
        /// select distinct Family from Family_MB where Family like @family + '%' order by Family
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetFamilyListFromFamilyMbByLike(string familyPrefix);
        
        /// <summary>
        /// Family_MB查
        /// select distinct Family from Family_MB order by Family
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetFamilyListFromFamilyMb();
 
        /// <summary>
        /// //实现sql：select * from SMTTime where Date=<queryDate> order by substring(Line,4,1), substring(Line,3,1)    //只比较年月日
        /// </summary>
        /// <param name="queryDate"></param>
        /// <returns></returns>
        IList<SmttimeInfo> GetSMTTimeInfoList(DateTime queryDate);

        void AddSMTTimeInfo(SmttimeInfo item);

        void DeleteSMTTimeInfo(SmttimeInfo condition);

        bool CheckExistSMTTimeInfo(SmttimeInfo condition);

        void UpdateSMTTimeInfo(SmttimeInfo setValue, SmttimeInfo condition);

        /// <summary>
        /// 实现sql：select * from SMTLine where Line in (<lineList>)
        /// </summary>
        /// <param name="lineList"></param>
        /// <returns></returns>
        IList<SMTLineDef> GetSMTLineInfoListByLineList(IList<string> lineList);

        /// <summary>
        /// 9. 针对指定的Defect,以相同Defect数作条件创建SA的Alarm（无返回值，参数为AlarmSetting对象，SQL语句变量与AlarmSetting对象成员变量相对应）。
        /// 方法：当@type=1 and @defectType=0  @curTime=GETDATE()
        /// INSERT [Alarm] 
        /// SELECT	'SA', DATEADD(HOUR, -@period, @curTime), @curTime, @id, a.Line, 
        ///        a.Station, d.Descr AS Family, b.DefectCodeID AS Defect, 'ALM2' AS ReasonCode,
        ///        'Defect: '+ b.DefectCodeID+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.ID))+' >= '+ @defectQty AS Reason, 'Created', @curTime 
        /// FROM PCBTestLogBack a 
        /// LEFT OUTER JOIN PCBTestLogBack_DefectInfo b 
        /// ON a.ID=b. PCBTestLogBackID 
        /// INNER JOIN PCB c 
        /// ON a.PCBNo=c.PCBNo 
        /// INNER JOIN Part d        
        /// ON c.PCBModelID=d.PartNo 
        /// WHERE a.Status=0 
        /// AND DATEDIFF(HOUR,a.Cdt,@curTime)<=@period 
        /// AND a.Station=@station 
        /// AND b.TriggerAlarm <>1 
        /// AND b.DefectCodeID+',' LIKE @defects 
        /// AND d.Descr=@family 
        /// GROUP BY a.Line, a.Station, d.Descr, b.DefectCodeID 
        /// HAVING COUNT(a.ID) >= @defectQty
        /// 注：AlarmSetting.defects是","分割的字符串。b.DefectCodeID+',' like @defects用in更好。
        /// </summary>
        /// <param name="alarmSetting"></param>
        int CreateAlarmWithSpecifiedDefectForSA(AlarmSettingInfo alarmSetting);

        /// <summary>
        /// 最后，将相关LogDefect记录设置标记以阻止其参与下一轮报警统计
        /// UPDATE PCBTestLogBack_DefectInfo a 
        /// SET a.TriggerAlarm=1 
        /// WHERE a.ID IN 
        /// (
        ///    SELECT a.ID 
        ///    FROM Alarm b 
        ///    LEFT OUTER JOIN PCBTestLogBack c 
        ///    ON b.Line=c.Line 
        ///    AND b.Station=c.Station 
        ///    LEFT OUTER JOIN a 
        ///    ON a.PCBTestLogBackID=c.ID 
        ///    INNER JOIN PCB d 
        ///    ON d.PCBNo=c.PCBNo 
        ///    INNER JOIN Part e 
        ///    ON d.PCBModelID=e.PartNo 
        ///    WHERE b.Stage='SA'
        ///    AND b.Cdt>@curTime 
        ///    AND c.Status=0 
        ///    AND c.Cdt>=DATEADD(HOUR, -@period, @curTime) 
        ///    AND c.Cdt<=@curTime 
        ///    AND a.DefectCodeID=b.Defect 
        ///    AND e.Descr=b.Family
        /// )
        /// </summary>
        /// <param name="alarmSetting"></param>
        /// <param name="alarm_id"></param>
        void UpdateForCreateAlarmWithDefectForSA(AlarmSettingInfo alarmSetting,int alarm_id);

        /// <summary>
        /// 10. 针对Exclude指定的Defect,以相同Defect数作条件创建SA的Alarm（无返回值，参数为AlarmSetting对象，SQL语句变量与AlarmSetting对象成员变量相对应）。
        /// 方法：当@type=1 and @defectType=1 and @defects is not null and @defects<>’’
        /// INSERT [Alarm] 
        /// SELECT	'SA', DATEADD(HOUR, -@period, @curTime), @curTime, @id, a.Line, 
        ///        a.Station, d.Descr AS Family, b.DefectCodeID AS Defect,'ALM2' AS ReasonCode, 
        ///        'Defect: '+ b.DefectCodeID+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.ID))+' >= '+ @defectQty AS Reason, 'Created', @curTime
        /// FROM PCBTestLogBack a 
        /// LEFT OUTER JOIN PCBTestLogBack_DefectInfo b 
        /// ON a.ID=b.PCBTestLogBackID 
        /// INNER JOIN PCB c 
        /// ON a.PCBNo=c.PCBNo 
        /// INNER JOIN Part d 
        /// ON c.PCBModelID=d.PartNo 
        /// WHERE a.Status=0 
        /// AND DATEDIFF(HOUR, a.Cdt, @curTime)<=@period 
        /// AND a.Station=@station 
        /// AND b.TriggerAlarm<>1 
        /// AND b.DefectCodeID+',' NOT LIKE @defects 
        /// AND d.Descr=@family 
        /// GROUP BY a.Line, a.Station, d.Descr, b.DefectCodeID 
        /// HAVING COUNT(a.ID)>=@defectQty
        /// </summary>
        /// <param name="alarmSetting"></param>
        int CreateAlarmWithExcludedDefectForSA(AlarmSettingInfo alarmSetting);

        /// <summary>
        /// 11. 针对All Defect,以相同Defect数作条件（无返回值，参数为AlarmSetting对象，SQL语句变量与AlarmSetting对象成员变量相对应）。
        ///    名称：
        ///    方法：当@type=1 and @defectType=1 and (@defects is null or @defects=’’)
        /// INSERT [Alarm] 
        /// SELECT  'SA', DATEADD(HOUR, -@period, @curTime), @curTime, @id, a.Line, 
        ///        a.Station, d.Descr AS Family, b.DefectCodeID AS Defect, 'ALM2' AS ReasonCode, 
        ///        'Defect: '+ b.DefectCodeID+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.ID))+' >= '+ @defectQty AS Reason, 'Created', @curTime
        /// FROM PCBTestLogBack a 
        /// LEFT OUTER JOIN PCBTestLogBack_DefectInfo b 
        /// ON a.ID=b.PCBTestLogBackID 
        /// INNER JOIN PCB c 
        /// ON a.PCBNo=c.PCBNo 
        /// INNER JOIN Part d 
        /// ON c.PCBModelID=d.PartNo 
        /// WHERE a.Status=0 
        /// AND DATEDIFF(HOUR, a.Cdt, @curTime)<=@period 
        /// AND a.Station=@station 
        /// AND b.TriggerAlarm<>1 
        /// AND d.Descr=@family 
        /// GROUP BY a.Line, a.Station, d.Descr, b.DefectCodeID 
        /// HAVING COUNT(a.ID)>=@defectQty
        /// </summary>
        /// <param name="alarmSetting"></param>
        int CreateAlarmWithAllDefectForSA(AlarmSettingInfo alarmSetting);

        /// <summary>
        /// 8. 以良率作条件创建SA的Alarm（无返回值，参数为AlarmSetting对象，SQL语句变量与AlarmSetting对象成员变量相对应）。 
        /// 方法：当@type=0 @curTime=GETDATE()
        /// SELECT a.Line, COUNT(a.ID) AS Total INTO #temp 
        /// FROM PCBTestLogBack a, PCB b, Part c 
        /// WHERE DATEDIFF(HOUR, a.Cdt, @curTime)<=@period 
        /// AND a.Station=@station 
        /// AND a.PCBNo=b.PCBNo 
        /// AND b.PCBModelID=c.PartNo 
        /// AND c.Descr=@family 
        /// GROUP BY a.Line 
        /// HAVING COUNT(a.ID)>=@minQty
        /// INSERT [Alarm] 
        /// SELECT 'SA', DATEADD(HOUR, -@period, @curTime), @curTime, @id, a.Line, 
        ///        b.Station, d.Descr AS Family, '' AS Defect, 'ALM1' as ReasonCode, 
        ///        CONVERT(VARCHAR,COUNT(b.ID))+' / '+ CONVERT(VARCHAR,a.Total)+' < '+ CONVERT(VARCHAR,@yieldRate) +'%' as Reason, 'Created', GETDATE()
        /// FROM #temp a, PCBTestLogBack b, PCB c, Part d 
        /// WHERE a.Line=b.Line 
        /// AND DATEDIFF(hour, b.Cdt, @curTime)<=@period 
        /// AND b.Station=@station 
        /// AND b.PCBNo=c.PCBNo 
        /// AND c.PCBModelID=d.PartNo 
        /// AND d.Descr=@family 
        /// GROUP BY a.Line, b.Station, d.Descr, a.Total 
        /// HAVING COUNT(b.ID)*100<@yieldRate*a.Total
        /// DROP TABLE #temp 
        /// </summary>
        /// <param name="alarm_setting"></param>
        int CreateAlarmWithYieldForSA(AlarmSettingInfo alarmSetting);

        void UpdatePcbRepair(Repair setValue, Repair condition);

        void AddPcbRepair(Repair item);

        void AddPcbRepairDefect(RepairDefect item);
                
        /// <summary>
        /// PCB（PCBNo=@NewMBSN, Editor,Udt; Codition: PCBNo=@MBSN）
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePcb(PcbEntityInfo setValue, PcbEntityInfo condition);
                
        ///// <summary>
        ///// PCBInfo(PCBNo=@NewMBSN, Editor, Udt; Condition: PCBNo=@MBSN)
        ///// </summary>
        ///// <param name="setValue"></param>
        ///// <param name="condition"></param>
        void UpdatePcbInfo(MBInfo setValue, MBInfo condition);

        ///// <summary>
        ///// PCBStatus(PCBNo=@NewMBSN, Station=10, Editor, Udt; Condition: PCBNo=@MBSN)
        ///// </summary>
        ///// <param name="setValue"></param>
        ///// <param name="condition"></param>
        void UpdatePcbStatus(MBStatus setValue, MBStatus condition);

        ///// <summary>
        ///// 分别获取Station=15和32的PCBTestLog.Remark中MAC的值（Station=15/32 and Status=1 and isnull(Remark,'')<>'' order by Cdt desc）
        ///// </summary>
        ///// <param name="eqCondition"></param>
        ///// <param name="notNullCondition"></param>
        ///// <returns></returns>
        IList<TestLog> GetPCBTestLogInfo(TestLog eqCondition, TestLog notNullCondition);

        ///// <summary>
        ///// Select Remark as Text, Line as Value from Dept order by Line
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <returns></returns>
        DataTable GetRemarkAndLineFromDept(DeptInfo condition);
        /// <summary>
        /// 方法：select pcb.* from Alarm a,PCBTestLogBack p,PCBTestLogBack_DefectInfo pdi,PCB pcb 
        /// where a.id=alarm_info.id 
        /// and a.Line=p.Line 
        /// and a.Station = p.Station 
        /// and p.Status=1 
        /// and (p.Cdt > alarm_info.StartTime and p.Cdt <= alarm_info.EndTime) 
        /// and p.ID=pdi.PCBTestLogBackID 
        /// and pdi.TriggerAlarm=1 
        /// and p.PCBNo=pcb.PCBNo
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PcbEntityInfo> GetPCBWithAlarm(AlarmInfo condition);

        /// <summary>
        /// PCBInfo.InfoValue; Condition: InfoType = 'MBCT2'
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<MBInfo> GetPcbInfoByCondition(MBInfo condition); 

        /// <summary>
        /// select PCBNo from PCBRepair 
        /// where PCBNo=@PCBNo 
        /// and Station<>'10' 
        /// and Station<>'19' 
        /// and Status=1
        /// union
        /// select PCBNo from PCBRepair a, PCBRepair_DefectInfo b 
        /// where a.PCBNo=@PCBNo 
        /// and a.Station='10' 
        /// and a.Status=1
        /// and a.ID=b.PCARepairID
        /// and UPPER(b.Remark) like '%' + @remarkLike + '%'
        /// </summary>
        /// <param name="pcbNo"></param>
        /// <param name="remarkLike"></param>
        /// <returns></returns>
        IList<string> GetPcbNoFromPcbRepairAndPcbRepairDefectInfo(string pcbNo, string remarkLike);

        #region . For CommonIntf  .

        /// <summary>
        /// 根据MB的Id获得Repair列表
        /// </summary>
        /// <param name="MBId"></param>
        /// <returns></returns>
        IList<RepairInfo> GetMBRepairLogList(string MBId);

        /// <summary>
        /// 根据MB的Id获得MB信息
        /// </summary>
        /// <param name="MBId"></param>
        /// <returns></returns>
        IMES.DataModel.MBInfo GetMBInfo(string MBId);

        /// <summary>
        /// Add FruDet
        /// </summary>
        /// <param name="newFruDet"></param>
        void InsertFruDetInfo(FruDetInfo newFruDet);

        #endregion

        #region . Defered  .

        void DeleteMBSectionDefered(IUnitOfWork uow, string startSn, string endSn);

        void SetMaxMBNODefered(IUnitOfWork uow, string mbCode, string mbType, IMB maxMO);

        void RemoveBatchDefered(IUnitOfWork uow, IList<IMB> items);

        void UpdateMBStatusBatchDefered(IUnitOfWork uow, IList<MBStatus> mbstts);

        void AddMBLogBatchDefered(IUnitOfWork uow, IList<MBLog> mblogs);

        /// <summary>
        /// Replace Old MB Data with New MB Sno, defered version
        /// </summary>
        /// <param name="uow">UnitOfWork</param>
        /// <param name="oldSn">oldSn</param>
        /// <param name="newSn">newSn</param>
        /// <remarks>
        /// 将下列各表中的Old MB Sno 对应记录的PCBNo 栏位Update 为New MB Sno
        /// IMES_PCA..MODismantleLog
        /// IMES_PCA..PCB
        /// IMES_PCA..PCBInfo
        /// IMES_PCA..PCBLog
        /// IMES_PCA..PCBRepair
        /// IMES_PCA..PCBStatus
        /// IMES_PCA..PCBTestLog
        /// IMES_PCA..PCB_Part
        /// IMES_PCA..SnoLog3D
        /// IMES_PCA..TransferToFISList
        /// </remarks>
        void ReplaceMBSnDefered(IUnitOfWork uow, string oldSn, string newSn);

        void InsertRptPcaRepDefered(IUnitOfWork uow, RptPcaRepInfo item);

        void AddMBTestDefered(IUnitOfWork uow, MBTestDef mbTest);

        void DeleteMBTestDefered(IUnitOfWork uow, string code, string family, bool type);

        void UpdatePCBStatusDefered(IUnitOfWork uow, PCBStatusInfo setValue, PCBStatusInfo condition);

        void AddMBCFGDefered(IUnitOfWork uow, MBCFGDef mbcfgDef);

        void DeleteMBCFGDefered(IUnitOfWork uow, int id);

        void UpdateMBCFGDefered(IUnitOfWork uow, MBCFGDef mbcfgDef, string mbCode, string series, string type);

        void AddITCNDDefectCheckDefered(IUnitOfWork uow, ITCNDDefectCheckDef item);

        void RemoveITCNDDefectCheckDefered(IUnitOfWork uow, string family);

        void RemoveITCNDDefectCheckbyFamilyAndCodeDefered(IUnitOfWork uow, string family, string code);

        void UpdateMtaMarkByRepairIdDefered(IUnitOfWork uow, int repairId, string mark);

        void InsertMtaMarkInfoDefered(IUnitOfWork uow, MtaMarkInfo item);

        void UpdateBorrowLogDefered(IUnitOfWork uow, BorrowLog item, string statusCondition);

        void UpdateRptPcaRepInfoDefered(IUnitOfWork uow, RptPcaRepInfo setValue, RptPcaRepInfo condition);

        void UpdateMtaMarkInfoDefered(IUnitOfWork uow, MtaMarkInfo setValue, MtaMarkInfo condition);

        void UpdateQtyFromPcaIctCountByCdtAndPdLineDefered(IUnitOfWork uow, int qty, DateTime cdt, string pdLine);

        void InsertPcaIctCountInfoDefered(IUnitOfWork uow, PcaIctCountInfo item);

        void InsertFruDetInfoDefered(IUnitOfWork uow, FruDetInfo newFruDet);

        void UpdatePcbStatusesDefered(IUnitOfWork uow, MBStatus setValue, string[] pcbIds);

        void AddMBLogsDefered(IUnitOfWork uow, MBLog[] mbLogs);
		
        void AddMBInfoesDefered(IUnitOfWork uow, IList<MBInfo> mbInfoes);

        void RemoveSATestCheckRuleItemDefered(IUnitOfWork uow, int id);

        void AddSATestCheckRuleItemDefered(IUnitOfWork uow, PcaTestCheckInfo item);

        void UpdateTestCheckRuleItemDefered(IUnitOfWork uow, PcaTestCheckInfo item, int id);

        void UpdatePCBLotInfoDefered(IUnitOfWork uow, PcblotInfo setValue, PcblotInfo condition);

        void UpdateLotInfoDefered(IUnitOfWork uow, LotInfo setValue, LotInfo condition);

        void UpdateLotInfoForDecQtyDefered(IUnitOfWork uow, LotInfo setValue, LotInfo condition);

        void InsertLotInfoDefered(IUnitOfWork uow, LotInfo item);

        void UpdateLotInfoForIncQtyDefered(IUnitOfWork uow, LotInfo setValue, LotInfo condition);

        void InsertPCBLotInfoDefered(IUnitOfWork uow, PcblotInfo item);

        void InsertLotSettingInfoDefered(IUnitOfWork uow, LotSettingInfo item);

        void DeleteLotSettingInfoDefered(IUnitOfWork uow, LotSettingInfo condition);

        void UpdateLotSettingInfoDefered(IUnitOfWork uow, LotSettingInfo setValue, LotSettingInfo condition);

        void InsertPcblotcheckInfoDefered(IUnitOfWork uow, PcblotcheckInfo item);

        void InsertPCBLotCheckFromPCBLotDefered(IUnitOfWork uow, string pcbNo, string editor, PcblotInfo condition);

        void UpdatePcboqcrepairInfoDefered(IUnitOfWork uow, PcboqcrepairInfo setValue, PcboqcrepairInfo condition);

        void DeletePcboqcrepairInfoDefered(IUnitOfWork uow, PcboqcrepairInfo condition);

        void InsertPcboqcrepairInfoDefered(IUnitOfWork uow, PcboqcrepairInfo item);

        void InsertPcboqcrepairDefectinfoDefered(IUnitOfWork uow, Pcboqcrepair_DefectinfoInfo item);

        void DeletePcboqcrepairDefectinfoDefered(IUnitOfWork uow, Pcboqcrepair_DefectinfoInfo condition);

        void RemoveSMTLineDefered(IUnitOfWork uow, SMTLineDef condition);

        void AddSMTLineDefered(IUnitOfWork uow, SMTLineDef item);

        void ChangeSMTLineDefered(IUnitOfWork uow, SMTLineDef setValue, SMTLineDef condition);

        void DeleteDeptInfoDefered(IUnitOfWork uow, DeptInfo condition);

        void AddDeptInfoDefered(IUnitOfWork uow, DeptInfo item);

        void UpdateDeptInfoDefered(IUnitOfWork uow, DeptInfo setValue, DeptInfo condition);

        void AddFamilyMbInfoDefered(IUnitOfWork uow, FamilyMbInfo item);

        void DeleteFamilyMbInfoDefered(IUnitOfWork uow, FamilyMbInfo condition);

        void ModifyFamilyMbInfoDefered(IUnitOfWork uow, FamilyMbInfo setValue, FamilyMbInfo condition);

        void AddSMTTimeInfoDefered(IUnitOfWork uow, SmttimeInfo item);

        void DeleteSMTTimeInfoDefered(IUnitOfWork uow, SmttimeInfo condition);

        void UpdateSMTTimeInfoDefered(IUnitOfWork uow, SmttimeInfo setValue, SmttimeInfo condition);

        void CreateAlarmWithSpecifiedDefectForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void UpdateForCreateAlarmWithDefectForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void CreateAlarmWithExcludedDefectForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void CreateAlarmWithAllDefectForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void CreateAlarmWithYieldForSADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void UpdatePcbRepairDefered(IUnitOfWork uow, Repair setValue, Repair condition);

        void AddPcbRepairDefered(IUnitOfWork uow, Repair item);

        void AddPcbRepairDefectDefered(IUnitOfWork uow, RepairDefect item);

        void UpdatePcbDefered(IUnitOfWork uow, PcbEntityInfo setValue, PcbEntityInfo condition);

        void UpdatePcbInfoDefered(IUnitOfWork uow, MBInfo setValue, MBInfo condition);

        void UpdatePcbStatusDefered(IUnitOfWork uow, MBStatus setValue, MBStatus condition);

        #endregion


        #region add new interface 
        IList<MbCodeAndMdlInfo> GetMbCodeAndMdlInfoList(string bomNodeType,
                                                                                              string partType,
                                                                                              string mbType,
                                                                                              string mdlType,
                                                                                              string mdlPostfix);
        int GetMbRepairCountByLocationAndStation(string pcbNo,
                                                                                 string station,
                                                                                 string location);

        IList<TestLog> GetPCBTestLogListFromPCBTestLog(string pcbNo,DateTime beginCdt);

        IList<IMB> GetChildMBFromParentMB(string pcbNo);

        void UpdatePCBStatusByMultiMB(IList<string> pcbNoList,
                                                            string station,
                                                            int status,
                                                            string line,
                                                            string editor);

        void UpdatePCBStatusByMultiMBDefered(IUnitOfWork uow,
                                                            IList<string> pcbNoList,
                                                            string station,
                                                            int status,
                                                            string line,
                                                            string editor);

        void UpdatePCBPreStation(IList<TbProductStatus> pcbStatusList);
        void UpdatePCBPreStationDefered(IUnitOfWork uow, IList<TbProductStatus> pcbStatusList);

        void WritePCBLogByMultiMB(IList<string> pcbNoList,
                                                            string station,
                                                            int status,
                                                            string line,
                                                            string editor);

        void WritePCBLogByMultiMBDefered(IUnitOfWork uow,
                                                            IList<string> pcbNoList,
                                                            string station,
                                                            int status,
                                                            string line,
                                                            string editor);

        void UpdateRCTO146MBbyMultiMB(IList<string> pcbNoList,
                                                         string skuModel,
                                                         string pizzaID,
                                                         decimal cartonWeight,
                                                         string cartonSN,
                                                         string deliveryNo,
                                                         string palletNo,
                                                         string shipMode,
                                                        string editor);

        void UpdateRCTO146MBbyMultiMBDefered(IUnitOfWork uow,
                                                        IList<string> pcbNoList,
                                                         string skuModel,
                                                         string pizzaID,
                                                         decimal cartonWeight,
                                                         string cartonSN,
                                                         string deliveryNo,
                                                         string palletNo,
                                                         string shipMode,
                                                         string editor);
        //Unpack By Carton /Delivery
        void UnpackRCTO146MBbyCatonSN(string  cartonSN,
                                                                  string editor);

        void UnpackRCTO146MBbyCatonSNDefered(IUnitOfWork uow,
                                                                           string cartonSN,
                                                                            string editor);

        void UnpackRCTO146MBbyDeliveryNo(string deliveryNo,
                                                                 string editor);

        void UnpackRCTO146MBbyDeliveryNoDefered(IUnitOfWork uow,
                                                                           string deliveryNo,
                                                                            string editor);

        //Combine Carton With Pallet
        void UpdatePalletNobyCaronSn(IList<string> cartonSNList,
                                                             string palletNo,
                                                            string editor);

        void UpdatePalletNobyCaronSnDefered(IUnitOfWork uow,
                                                                         IList<string> cartonSNList,
                                                                         string palletNo,
                                                                          string editor);

        //Get Combined Pallet CartonQty and DeliveryNo By Carton
        IList<RCTO146MBInfo> GetRCTO146MBByCartonSN(IList<string> cartonSNList);
        IList<RCTO146MBInfo> GetRCTO146MBByPalletNo(string palletNo);
        IList<RCTO146MBInfo> GetRCTO146MBByDeliveryNo(string deliveryNo);

        IList<string> GetDeliveryNoByCartonSN(IList<string> cartonSNList);
        int GetCombinedMBQtyWithDeliveryNo(string deliveryNo);
        IList<CombinedPalletCarton> GetCartonQtywithCombinedPallet(string deliveryNo);
        IList<TbProductStatus> GetMBStatus(IList<string> mbSnList);

        //Check Delivery Qty on Transaction
        void CheckDnQtyAndUpdateDnStatus(string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode);
        void CheckDnQtyAndUpdateDnStatusDefered(IUnitOfWork uow, string deliveryNo, int needCombinedQty, string fullCombinedStatus, string errorCode);

        #endregion

        #region for Pilot Run MO Check
        /// <summary>
        /// select top 1 a.PCBNo, a.Status 
        ///                                                 from PCBLog a, 
        ///                                                      PCBInfo b,
        ///                                                      @StationList c 
        ///                                                 where a.PCBNo = b.PCBNo and 
        ///                                                      b.InfoType=@InfoType and 
        ///                                                      b.InfoValue=@InfoValue and 
        ///                                                       a.Station =c.data   and
        ///                                                       a.Status =@Status
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <param name="stationList"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool ExistsPCBInfoAndLogStation(string infoType, string infoValue, IList<string> stationList, int status);
        /// <summary>
        /// delete PCBInfo where PCBNo=@pcbNo and InfoType in @itemtypes
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="itemTypes"></param>
        void RemovePCBInfosByType(string pcbNo, IList<string> itemTypes);
        void RemovePCBInfosByTypeDefered(IUnitOfWork uow, string pcbNo, IList<string> itemTypes);
        #endregion

        #region ASUS CheckSN
        void ExistsCustomSnThrowError(string pcbNo, string customSn);
        void ExistsCustomSnThrowErrorDefered(IUnitOfWork uow, string pcbNo, string customSn);
        #endregion

        #region PCB_Part backup && delete & insert 
        IList<PCBPartInfo> GetPCBPartByPCBNos(IList<string> pcbNoList);
        IList<PCBPartInfo> GetPCBPartByPartSnList(IList<string> partSnList);
        void BackupPCBPart(IList<UnpackPCBPartInfo> unpackPCBInfoList);
        void BackupPCBPartDefered( IUnitOfWork uow, IList<UnpackPCBPartInfo> unpackPCBInfoList);
        void RemovePCBPartByIDs(IList<int> idList);
        void RemovePCBPartByIDsDefered(IUnitOfWork uow, IList<int> idList);
        void RemovePCBPartByPCBNos(IList<string> pcbNoList);
        void RemovePCBPartByPCBNosDefered(IUnitOfWork uow, IList<string> pcbNoList);
        void InsertPCBPart(IList<PCBPartInfo> pcbPartList);
        void InsertPCBPartDefered(IUnitOfWork uow, IList<PCBPartInfo> pcbPartList);
        IList<PCBStatusExInfo> GetPCBPreStation(IList<string> pcbNoList);
        #endregion
    }
}
