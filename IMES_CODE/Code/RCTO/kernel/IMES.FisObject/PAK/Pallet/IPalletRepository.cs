﻿// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-27   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.PAK.DN;
using System.Data;

namespace IMES.FisObject.PAK.Pallet
{
    /// <summary>
    /// Pallet对象Repository接口
    /// </summary>
    public interface IPalletRepository : IRepository<Pallet>
    {
        /// <summary>
        /// 晚加载PalletLog
        /// </summary>
        /// <param name="currentPallet"></param>
        /// <returns></returns>
        Pallet FillPalletLogs(Pallet currentPallet);

        /// <summary>
        /// 根据PalletNo获得DeliveryPallet列表
        /// </summary>
        /// <param name="PalletNo"></param>
        /// <returns></returns>
        IList<DeliveryPallet> GetDeliveryPallet(string PalletNo);

        /// <summary>
        /// 调用PAK库中的SP:op_Plt_upload_to_SAP
        /// </summary>
        /// <param name="PalletNo"></param>
        void UpdatePltWeightToSAP(string PalletNo);

        /// <summary>
        /// 根据DnNo, PalletNo获得DeliveryPallet列表
        /// </summary>
        /// <param name="DnNo"></param>
        /// <param name="PalletNo"></param>
        /// <returns></returns>
        IList<DeliveryPallet> GetDeliveryPalletByDNAndPallet(string DnNo, string PalletNo);

        //SELECT [Id],[Forwarder],[Date],[MAWB],[Driver],[TruckID],[Editor],[Cdt],[Udt]
        //FROM [dbo].[Forwarder]
        //WHERE TruckID = @TruckID
        IList<ForwarderInfo> GetForwarderInfoByTruckID(string truckID);

        //SELECT * FROM PickIDCtrl 
        //WHERE TruckID = @TruckID and Dt = @Date
        IList<PickIDCtrlInfo> GetPickIDByTruckIDAndDate(string truckID, DateTime date);

        //SELECT * FROM MAWB
        //WHERE MAWB = @MAWB
        IList<MAWBInfo> GetMAWBInfoByMAWB(string MAWB);

        //SELECT * FROM MAWB
        //WHERE (MAWB = @MAWB or HAWB = @MAWB)
        IList<MAWBInfo> GetMAWBInfoByMAWBorHAWB(string MAWB);

        //Insert PickIDCtrl 
        void InsertPickIDCtrl(PickIDCtrlInfo pickIDCtrlInfo);

        //INSERT FwdPlt
        void InsertFwdPlt(FwdPltInfo fwdPltInfo);

        //SELECT * FROM PickIDCtrl 
        //WHERE PickID = @pickID and Dt = @date
        IList<PickIDCtrlInfo> GetPickIDCtrlInfoByPickIDAndDate(string pickID, DateTime date);

        //SELECT * FROM FwdPlt
        //WHERE PickID = @pickID AND Status = @status and Dt = @date
        IList<FwdPltInfo> GetFwdPltInfosByPickIDAndStatusAndDate(string pickID, string status, DateTime date);

        //update FwdPlt set Status = @status
        //where PickID = @pickID and Plt = @pltno
        void UpdateFwdPltStatus(string pickID, string pltNo, string status, string editor);

        void InsertPalletId(PalletIdInfo item);

        /// <summary>
        /// 1.清理FwdPlt 旧有数据
        /// 接口参考：
        /// public 
        /// 实现参考：
        /// DELETE FROM FwdPlt WHERE PickID = @pickID
        /// </summary>
        /// <param name="pickID"></param>
        /// <returns></returns>
        int RemoveFwdPltByPickID(string pickID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pickID"></param>
        /// <returns></returns>
        IList<PickIDCtrlInfo> GetPickIDCtrlInfoByPickID(string pickID);

        #region Defered

        void UpdatePltWeightToSAPDefered(IUnitOfWork uow, string PalletNo);

        void InsertPickIDCtrlDefered(IUnitOfWork uow, PickIDCtrlInfo pickIDCtrlInfo);

        void InsertFwdPltDefered(IUnitOfWork uow, FwdPltInfo fwdPltInfo);

        void UpdateFwdPltStatusDefered(IUnitOfWork uow, string pickID, string pltNo, string status, string editor);

        void InsertPalletIdDefered(IUnitOfWork uow, PalletIdInfo item);

        #endregion

        #region . For CommonIntf  .

        /// <summary>
        /// 根据DNId获得Pallet列表
        /// </summary>
        /// <param name="DNId"></param>
        /// <returns></returns>
        IList<PalletInfo> GetPalletList(string DNId);

        #endregion

        #region . For Maintain  .

        /// <summary>
        /// 根据family获得PalletWeight列表
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<PalletWeight> GetAllPalletWeightByFamily(string family);

        /// <summary>
        /// 根据ID删除PalletWeight
        /// </summary>
        /// <param name="id"></param>
        void DeletePalletWeightByID(int id);

        /// <summary>
        /// 新增PalletWeight
        /// </summary>
        /// <param name="palletWeight"></param>
        void AddPalletWeight(PalletWeight palletWeight);

        /// <summary>
        /// 参考sql
        /// update PalletWeight 
        /// set Family=?,
        ///     Region=?,
        ///     QTY=?,
        ///     Weight=?,
        ///     Tolerance=?,
        ///     Editor=?,
        ///     Udt=?
        /// where ID=?
        /// </summary>
        /// <param name="palletWeight"></param>
        void UpdatePalletWeight(PalletWeight palletWeight);

        /// <summary>
        /// 参考sql 
        /// select * from PalletWeight where Family=? and Region=?
        /// </summary>
        /// <param name="family"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        bool IFPalletWeightIsExists(string family, string region);

        /// <summary>
        /// 添加一条Forwarder
        /// id被写回在item里面返回
        /// </summary>
        /// <param name="item"></param>
        void AddForwarder(ForwarderInfo item);

        /// <summary>
        /// 取得查询列表
        /// SELECT [Date], [Forwarder],[MAWB],[Driver],[TruckID],[Editor],CONVERT(char(10), [Cdt], 21) as [UploadDate],CONVERT(char(10), [Udt], 21) as Udt, Id  
        ///         FROM [Forwarder]
        ///         WHERE CONVERT(char(10), Cdt, 21) BETWEEN @StartDate AND @EndDate
        ///         ORDER BY [Cdt]
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        DataTable GetForwarderList(string startDate, string endDate);

        /// <summary>
        /// 更新Forwarder
        /// UPDATE [Forwarder]
        ///  SET [Driver] = @NewDriver,
        ///  [TruckID] = @NewTruckID,
        ///  [Editor] = @Editor,
        ///  [Udt] = GETDATE()
        ///  WHERE [Id] = @Id
        /// </summary>
        /// <param name="item"></param>
        void UpdateForwarder(ForwarderInfo item);
 
        /// <summary>
        /// 删除数据
        /// DELETE FROM [IMES_PAK].[dbo].[Forwarder] WHERE [Forwarder] = @Id
        /// </summary>
        /// <param name="item"></param>
        void DeleteForwarder(ForwarderInfo item);

        /// <summary>
        /// 取得存在的Forwarder相关数据
        /// SELECT Id FROM [Forwarder]
        /// WHERE [Forwarder]=@Forwarder AND [Date]=@Date AND [MAWB]=@MAWB AND [Driver]=@Driver AND [TruckID]=@TruckID
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        DataTable GetExistForwarder(ForwarderInfo item);

        /// <summary>
        /// 添加多条数据到Forwarder, 并且添加成的记录将id写回到参数列表的单元中
        /// for(int i=0;i < items.Count;i++)
        ///{
        ///    ForwarderInfo item=items[i];
        ///    //如果不存在这记录，就添加，id被写回列表
        ///    if not exists (SELECT Id FROM [Forwarder] WHERE [Forwarder]=@Forwarder AND [Date]=@Date AND [MAWB]=@MAWB AND [Driver]=@Driver AND [TruckID]=@TruckID) 
        ///    {
        ///       AddForwarder(ForwarderInfo item)；
        ///       items[i]中被存回相应的id  
        ///    }
        ///    //如果已经存在这记录，就不添加了，并且id不写回，以区分哪些是后来添加成功的 
        ///}
        /// </summary>
        /// <param name="items"></param>
        void ImportForwarders(IList<ForwarderInfo> items);

        /// <summary>
        /// 根据Pallet No删除PalletID
        /// </summary>
        /// <param name="id"></param>
        void DeletePalletIDByPalletNo(string palletNo);

        #region Defered

        void DeletePalletWeightByIDDefered(IUnitOfWork uow, int id);

        void AddPalletWeightDefered(IUnitOfWork uow, PalletWeight palletWeight);

        void UpdatePalletWeightDefered(IUnitOfWork uow, PalletWeight palletWeight);

        void AddForwarderDefered(IUnitOfWork uow, ForwarderInfo item);

        void UpdateForwarderDefered(IUnitOfWork uow, ForwarderInfo item);

        void DeleteForwarderDefered(IUnitOfWork uow, ForwarderInfo item);

        void ImportForwardersDefered(IUnitOfWork uow, IList<ForwarderInfo> items);

        #endregion

        #endregion

        /// <summary>
        /// 1..获取所有的ChepPallet信息。
        /// select * from ChepPallet order by ChepPalletNo;
        /// </summary>
        /// <returns></returns>
        IList<ChepPalletInfo> GetChepPalletList();

        /// <summary>
        /// 2.获取一条ChepPallet 信息
        /// select ChepPalletNo,Editor,Cdt,Udt from ChepPallet where ChepPalletNo=chepPalletNo;
        /// </summary>
        /// <param name="chepPalletNo"></param>
        /// <returns></returns>
        ChepPalletInfo GetChepPalletInfo(string chepPalletNo);

        /// <summary>
        /// 3.添加一条ChepPallet信息
        /// insert into ChepPallet values (item.chepPalletNo,item.editor,item.cdt,item.udt);
        /// </summary>
        /// <param name="item"></param>
        void AddGetChepPalletInfo(ChepPalletInfo item);

        /// <summary>
        /// 4.删除一条ChepPallet数据
        /// delete from ChepPallet where ID=@id
        /// </summary>
        /// <param name="id"></param>
        void DeleteChepPalletInfo(int id);

        /// <summary>
        /// 1.取得所有的PalletStatndard信息
        /// select *  from PalletStatndard order by FullQty
        /// </summary>
        /// <returns></returns>
        IList<PalletQtyInfo> GetQtyInfoList();

        /// <summary>
        /// 2.根据fullQty取得PalletStatndard的一条记录
        /// select *  from PalletStatndard where ID=@id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PalletQtyInfo GetQtyInfo(int id);

        /// <summary>
        /// 3.添加一条记录
        /// insert into PalletStatndard
        /// values ('pqInfo.fullQty','pqInfo.tireQty','pqInfo.mediumQty','pqInfo.litterQty',''pqInfo.editor','pqInfo.cdt','pqInfo.udt')
        /// </summary>
        /// <param name="pqInfo"></param>
        void AddQtyInfo(PalletQtyInfo pqInfo);

        /// <summary>
        /// 4.删除一条记录
        /// delete from PalletStatndard where ID=@id
        /// </summary>
        /// <param name="id"></param>
        void DeleteQtyInfo(int id);

        /// <summary>
        /// 5.更新一条记录
        /// update PalletStatndard
        /// set fullQty=pqInfo.fullQty,tireQty='pqInfo.tireQty',mediumQty='pqInfo.mediumQty',litterQty='pqInfo.litterQty',
        /// editor=''pqInfo.editor',udt='pqInfo.udt'
        /// where ID=id
        /// </summary>
        /// <param name="pqInfo"></param>
        /// <param name="id"></param>
        void UpdateQtyInfo(PalletQtyInfo pqInfo, int id);

        /// <summary>
        /// UPDATE PAK_BTLocMas SET [Status] = 'Close' WHERE SnoId = @Location
        /// </summary>
        /// <param name="status"></param>
        /// <param name="snoId"></param>
        void UpdateStatusForPakBtLocMas(string status, string snoId);
        void UpdateStatusForPakBtLocMas(string status, string snoId, string editor);

        /// <summary>
        /// Query PakBtLocMasInfos by model and status ORDER BY SnoId
        /// </summary>
        /// <param name="model"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<PakBtLocMasInfo> GetPakBtLocMasInfos(string model, string status);

        /// <summary>
        /// Insert SnoDet_BTLoc
        /// </summary>
        /// <param name="item"></param>
        void InsertSnoDetBtLocInfo(SnoDetBtLocInfo item);

        /// <summary>
        /// Query SnoDetBtLocInfos by any 'AND' conditions.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<SnoDetBtLocInfo> GetSnoDetBtLocInfosByCondition(SnoDetBtLocInfo condition);

        /// <summary>
        /// Update PAK_BTLocMas SET CmbQty = CmbQty + 1, Editor = @editor WHERE (SnoId = @snoId) 
        /// </summary>
        /// <param name="snoId"></param>
        /// <param name="editor"></param>
        void UpdateForIncPakBtLocMas(string snoId, string editor);

        /// <summary>
        /// Update PAK_BTLocMas SET CmbQty = CmbQty + 1 WHERE (SnoId = @snoId) 
        /// </summary>
        /// <param name="snoId"></param>
        void UpdateForIncPakBtLocMas(string snoId);

        /// <summary>
        /// select PalletNo from Pallet where UCC = @UCCID
        /// </summary>
        /// <param name="ucc"></param>
        /// <returns></returns>
        string GetPalletNoByUcc(string ucc);

        /// <summary>
        /// SELECT @pqty2=TierQty FROM PalletStandard WHERE FullQty=@PalletQty
        /// 按照PalletTotal得到一层数量PalletTier：
        /// PalletStandard.TierQty where FullQty=PalletTotal
        /// </summary>
        /// <param name="fullQty"></param>
        /// <returns></returns>
        int GetTierQtyFromPalletQtyInfo(string fullQty);

        /// <summary>
        /// SELECT @carrier = a.InfoValue FROM DeliveryInfo a (nolock), Delivery_Pallet b (nolock)
        /// WHERE a.DeliveryNo = b.DeliveryNo
        ///  AND b.Pallet = @PalletNo
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<string[]> GetInfoTypeInfoValuePairsFromDeliveryInfo(string palletNo);

        /// <summary>
        /// select top 1 * from PAK_BTLocMas where Model=@Model and Status=@Status order by SnoId asc
        /// </summary>
        /// <param name="model"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        PakBtLocMasInfo GetPakBtLocMasInfo(string model, string status);

        /// <summary>
        /// 1. Get Packed Pallet Qty，then display
        /// Packed Pallet Qty – 这里指的是当天已经完成称重的Pallet 数量
        /// ISNULL（IMES_PAK..Pallet.Weight, ''） <> '' 表示该Pallet 已经完成称重
        /// IMES_PAK..Pallet.Udt>=CONVERT(char(10),GETDATE(),111) 表示当天完成
        /// </summary>
        /// <returns></returns>
        int GetQtyOfPackedPalletToday();

        /// <summary>
        /// 根据fullQty取得表PalletStatndard的相应记录where FullQty= fullQty
        /// </summary>
        /// <param name="fullQty"></param>
        /// <returns></returns>
        IList<PalletQtyInfo> GetPalletByFullQty(string fullQty);
        
        /// <summary>
        /// 使用Pallet No 查询IMES_PAK..Delivery_Pallet 表，得到Shipment，然后按照如下方法确定是否需要提示用户Scan 2D Barcode
        ///SELECT @setnum=DOC_SET_NUMBER FROM HP_EDI..[PAK.PAKComn] (NOLOCK) WHERE InternalID=@shipment 
        ///IF EXISTS (SELECT * FROM HP_EDI..[PAK.PAKRT] 
	    /// WHERE DOC_SET_NUMBER=@setnum AND XSL_TEMPLATE_NAME like '%Verizon2D%' 
		/// AND DOC_CAT='Pallet Ship Label- Pack ID Single' )
        /// BEGIN
	    ///     SELECT '1'	
        /// END
        ///ELSE 
        /// BEGIN 
    	///     SELECT '0'	
        /// END
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        bool IsScan2DBarCode(string palletNo);

        /// <summary>
        /// DELETE PAKODMSESSION FROM PAKODMSESSION O
        /// inner join IMES2012_FA.dbo.Product as P on O.SERIAL_NUM =P.CUSTSN
        /// WHERE P.DeliveryNo =@DeliveryNo and (P.Model LIKE 'PC%' or P.Model LIKE 'QC%')
        /// </summary>
        /// <param name="dn"></param>
        void UnPackPakOdmSessionByDeliveryNo(string dn);

        /// <summary>
        /// DELETE PAK_PackkingData WHERE InternalID = @DeliveryNo
        /// </summary>
        /// <param name="dn"></param>
        void UnPackPackingDataByDeliveryNo(string dn);

        /// <summary>
        /// @qty=count(SnoId) from Dummy_ShipDet (nolock) where PLT=@plt
        /// </summary>
        /// <param name="plt"></param>
        /// <returns></returns>
        int GetCountOfDummyShipDetByPlt(string plt);

        /// <summary>
        /// insert WH_PLTLog values (@plt,'IN',@user,getdate())
        /// INSERT WH_PLTLog VALUES (@PalletNo,'OT',@PickID,GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void InsertWhPltLog(WhPltLogInfo item);

        /// <summary>
        /// UPDATE WH_PLTMas set WC='IN',Editor=@user,Udt=getdate() where PLT=@plt
        /// UPDATE WH_PLTMas set WC='RW',Editor=@user,Udt=getdate() where PLT=@plt
        /// UPDATE WH_PLTMas SET WC='OT',Editor=@PickID,Udt=GETDATE()WHERE PLT=@PalletNo 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="plt"></param>
        void UpdateWhPltMas(WhPltMasInfo item, string plt);

        /// <summary>
        /// SELECT * FROM [WH_PLTMas] WHERE PLT =@PalletNo
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        WhPltMasInfo GetWHPltMas(string palletNo);

        /// <summary>
        /// insert WH_PLTMas values (@plt,'IN',@user,getdate(),getdate())
        /// </summary>
        /// <param name="item"></param>
        void InsertWhPltMas(WhPltMasInfo item);

        /// <summary>
        /// b) 当@plt存在于Dummy_ShipDet时
        /// </summary>
        /// <param name="plt"></param>
        /// <returns></returns>
        IList<DummyShipDetInfo> GetDummyShipDetListByPlt(string plt);

        /// <summary>
        /// PAK_WHPLT_Type.BOL from PAK_WHPLT_Type WHERE PLT=@plt
        /// </summary>
        /// <param name="plt"></param>
        /// <returns></returns>
        IList<PakWhPltTypeInfo> GetPakWhPltTypeListByPlt(string plt);

        /// <summary>
        /// a.首先需要先删除相同PLT的记录
        /// </summary>
        /// <param name="plt"></param>
        void DeletePakWhPltTypeByPlt(string plt);

        /// <summary>
        /// 写入PAK_WHPLT_Type
        /// </summary>
        /// <param name="item"></param>
        void InsertPakWhPltTypeInfo(PakWhPltTypeInfo item);

        /// <summary>
        /// 1）当按照以下条件(PAK_WH_LocMas where BOL=@bol  and PLT1='')不能得到记录时
        /// 1）select @col=Col,@loc=Loc  from PAK_WH_LocMas where BOL=@bol and PLT1=''order by Loc desc
        /// </summary>
        /// <param name="bol"></param>
        /// <param name="plt1"></param>
        /// <returns></returns>
        IList<PakWhLocMasInfo> GetPakWhLocMasListByBolAndPlt1(string bol, string plt1);

        /// <summary>
        /// select @n=count(PLT) from PAK_WHPLT_Type where BOL=@bol and Tp='F'
        /// </summary>
        /// <param name="bol"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        int GetCountOfPakWhPltType(string bol, string tp);

        /// <summary>
        /// 3）select @col=Col,@loc=Loc from dbo.PAK_WH_LocMas where BOL=''and PLT1=''and charindex(@carrier,Carrier)>0 order by Loc desc,Col
        /// </summary>
        /// <param name="bol"></param>
        /// <param name="plt1"></param>
        /// <param name="carrier"></param>
        /// <returns></returns>
        IList<PakWhLocMasInfo> GetPakWhLocMasListByBolAndPlt1AndCarrier(string bol, string plt1, string carrier);

        /// <summary>
        /// update PAK_WH_LocMas set BOL=@bol where Col=@col and Loc=@loc
        /// </summary>
        /// <param name="bol"></param>
        /// <param name="col"></param>
        /// <param name="loc"></param>
        void UpdatePakWhLocBolByColAndLoc(string bol, string col, int loc);

        /// <summary>
        /// 2） insert WH_PLTLocLog values(@plt, rtrim(@col)+rtrim(@loc), '',getdate())
        /// </summary>
        /// <param name="item"></param>
        void InsertWhPltLocLogInfo(WhPltLocLogInfo item);

        /// <summary>
        /// update PAK_WH_LocMas set BOL=@bol,PLT1=@plt,Udt=getdate() where Col=@col and Loc=@loc
        /// </summary>
        /// <param name="item"></param>
        /// <param name="col"></param>
        /// <param name="loc"></param>
        void UpdatePakWhLocByColAndLoc(PakWhLocMasInfo item, string col, int loc);

        /// <summary>
        /// 1）update PAK_WH_LocMas set PLT1='',PLT2='' where (PLT1=@plt or PLT2=@plt) and @plt<>''
        /// </summary>
        /// <param name="plt"></param>
        void UpdatePakWhLocByPltForClearPlt1AndPlt2(string plt);

        /// <summary>
        /// SELECT * FROM PoPlt WHERE PLT=@PLT
        /// </summary>
        /// <param name="plt"></param>
        /// <returns></returns>
        IList<PoPltInfo> GetPoPltByPlt(string plt);

        /// <summary>
        /// 在DOA..PoPlt表中增加记录
        /// </summary>
        /// <param name="item"></param>
        void InsertPoPlt(PoPltInfo item);

        /// <summary>
        /// 在DOA..PoData表中增加记录
        /// </summary>
        /// <param name="item"></param>
        void InsertPoData(PoDataInfo item);

        /// <summary>
        /// Pallet List：Delivery_Pallet按照ShipmentNo等于选择的DN对应的ShipmentNo作为查询条件，得到所有的pallet，在Product得到已绑定这个Pallet的unit，相同的pallet作合并，能得到该pallet的Total Qty和已绑定unit的Qty(OK Qty)，Diff Qty=Total Qty-OK Qty。按照pallet 排序	
        ///
        /// Pallet List部分需要新增接口，需求如下：
        /// 入参：string shipment；
        /// 返回值：实现左单元格中的相关功能，返回一个按照PalletNo排序的IList<（新结构类型）>，
        /// 此处定义的新结构类型的成员中至少包含PalletNo、TotalQty、OKQty、DiffQty（DiffQty = TotalQty - OKQty）
        /// 
        /// TotalQty = SUM(Delivery_Pallet.DeliveryQty)
        /// OKQty = COUNT(Product)
        /// </summary>
        /// <param name="shipmentNo"></param>
        /// <returns></returns>
        IList<PalletCapacityInfo> GetPalletCapacityInfoList(string shipmentNo);

        /// <summary>
        /// Scanned Qty：从Product表里得到PalletNo=@plt的记录数
        /// </summary>
        /// <param name="plt"></param>
        /// <returns></returns>
        int GetCountOfBoundProduct(string plt);

        /// <summary>
        /// 入参：string dn
        /// 功能：删除Pallet表中相关数据
        /// </summary>
        /// <param name="dn"></param>
        void DeletePalletsByDn(string dn);

        /// <summary>
        /// 入参：string shipmentNo
        /// 功能：删除Pallet表中相关数据
        /// </summary>
        /// <param name="shipmentNo"></param>
        void DeletePalletsByShipmentNo(string shipmentNo);

        /// <summary>
        /// IMES_PAK..ChepPallet.PalletNo = @PalletNo
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<ChepPalletInfo> GetChepPalletListByPalletNo(string palletNo);

        /// <summary>
        /// SELECT @bol=RTRIM(BOL) FROM PAK_WH_LocMas WHERE PLT1=RTRIM(@PalletNo) or PLT2=RTRIM(@PalletNo)
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<string> GetBolFromPakWhLocMasByPlt1AndPlt2(string palletNo);

        //UPDATE WH_PLTMas SET WC='OT',Editor=@PickID,Udt=GETDATE()WHERE PLT=@PalletNo 
        //void UpdateWhPltMas(WhPltMasInfo item, string plt);

        //INSERT WH_PLTLog VALUES (@PalletNo,'OT',@PickID,GETDATE())
        // void InsertWhPltLog(WhPltLogInfo item);

        /// <summary>
        /// UPDATE PAK_WH_LocMas SET BOL='',Editor=@PickID,Udt=GETDATE() 
        /// WHERE BOL=RTRIM(@bol) and @bol<>'' and PLT1=''
        /// </summary>
        /// <param name="bol"></param>
        void UpdatePakWhLocByPltForClearBol(string bol, string editor);

        /// <summary>
        /// UPDATE PAK_WH_LocMas SET BOL='',PLT1='',PLT2='',Editor=@PickID,Udt=GETDATE() 
        /// WHERE PLT1=RTRIM(@PalletNo) or PLT2=RTRIM(@PalletNo)
        /// </summary>
        /// <param name="bol"></param>
        /// <param name="palletNo"></param>
        void UpdatePakWhLocByPltForClearBolAndPlt1AndPlt2(string palletNo);

        /// <summary>
        /// UPDATE PAK_WHLoc_TMP SET PLT='',Tp='',BOL='' WHERE PLT=RTRIM(@PalletNo)
        /// </summary>
        /// <param name="palletNo"></param>
        void UpdatePakWhLocTmpForClearPltAndTpAndBol(string palletNo);

        /// <summary>
        /// UPDATE PAK_WHLoc_TMP SET BOL='' WHERE BOL=RTRIM(@bol) and @bol<>'' and PLT=''
        /// </summary>
        /// <param name="bol"></param>
        void UpdatePakWhLocTmpForClearBol(string bol);

        /// <summary>
        /// DELETE ChepPallet WHERE PalletNo=@ChepPalletNo
        /// </summary>
        /// <param name="palletNo"></param>
        void DeleteChepPalletInfo(string palletNo);

        /// <summary>
        /// DELETE Pallet_RFID WHERE PLT=@PalletNo and RFIDCode=@ChepPalletNo
        /// </summary>
        /// <param name="plt"></param>
        /// <param name="rfidCode"></param>
        void DeletePalletRfid(string plt, string rfidCode);

        /// <summary>
        /// INSERT Pallet_RFID
        //SELECT @PalletNo,@ChepPalletNo,@carrier,@user,getdate(),getdate()
        /// </summary>
        /// <param name="item"></param>
        void InsertPalletRfid(PalletRfidInfo item);

        /// <summary>
        /// 找到，则返回查找到的DummyShipDetInfo类型数据；找不到，不要抛异常，返回Null
        /// </summary>
        /// <param name="snoId"></param>
        /// <returns></returns>
        DummyShipDetInfo GetDummyShipDet(string snoId);

        /// <summary>
        /// 满足BOL=传入的bol的Dummy_ShipDet
        /// </summary>
        /// <param name="bol"></param>
        /// <returns></returns>
        IList<DummyShipDetInfo> GetDummyShipDetListByBol(string bol);

        /// <summary>
        /// 1. Delivery_Pallet表
        /// 查询IMES_PAK..Delivery_Pallet 表,取得shipmentNoList对应的DeliveryPallet记录集
        /// </summary>
        /// <param name="shipmentNoList"></param>
        /// <returns></returns>
        IList<DeliveryPallet> GetDeliveryPalletByShipmentNoList(IList<string> shipmentNoList);
 
        /// <summary>
        /// 按照pallet得到Location：PAK_LocMas.SnoId where Pno=@plt
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        string GetSnoIdFromPakLocMasByPno(string pno);

        /// <summary>
        /// 13.使用条件Type,PartNo查询IMES_PAK..PAK_CHN_TW_Light表,得到的记录的Model 字段值;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="partno"></param>
        /// <returns></returns>
        IList<string> GetModelByTypeAndPartNo(string type, string partno);

        /// <summary>
        /// 14.使用Type AND PartNo AND Model查询IMES_PAK..PAK_CHN_TW_Light 表存在记录;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="partno"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<PakChnTwLightInfo> GetModelbyTypeAndPartNoAndFamily(string type, string partno, string model);

        /// <summary>
        /// 放在栈板上的Product Model以及数量
        /// </summary>
        /// <param name="pltNo"></param>
        /// <returns></returns>
        IList<ModelStatistics> GetByModelStatisticsForSinglePallet(string pltNo);

        /// <summary>
        /// ii:  使用Model LIKE @family3 + '%' 查询IMES_FA..PAK_CHN_TW_Light 表存在记录
        /// </summary>
        /// <param name="modelPrefix"></param>
        /// <returns></returns>
        IList<PakChnTwLightInfo> GetPakChnTwLightInfoListByLikeModel(string modelPrefix);

        /// <summary>
        /// SELECT a.PartNo, a.Descr, 1
        ///  FROM PAK_CHN_TW_Light a (nolock), ModelBOM b (nolock)
        ///  WHERE a.Model = @model AND b.Material = @Pno AND b.Component = a.PartNo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pno"></param>
        /// <returns></returns>
        IList<PakChnTwLightInfo> GetPakChnTwLightInfoListByModelAndPno(string model, string pno);

        /// <summary>
        /// SELECT a.PartNo, a.Descr, 1
        /// FROM PAK_CHN_TW_Light a (nolock), ModelBOM b (nolock)
        /// WHERE a.Model = LEFT(@model,CHARINDEX(' ',@model)-1) AND b.Material = @Pno AND b.Component = a.PartNo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pno"></param>
        /// <returns></returns>
        IList<PakChnTwLightInfo> GetPakChnTwLightInfoListByModelAndPno2(string model, string pno);

        /// <summary>
        /// SELECT DISTINCT Type FROM PAK_CHN_TW_Light (nolock)WHERE Model=@family2
        /// SELECT DISTINCT Type FROM PAK_CHN_TW_Light (nolock)WHERE Model=@family2
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<string> GetTypeListFromPakChnTwLightByModel(string model);

        /// <summary>
        /// 1. 当使用Pno=@plt and Tp='PakLoc' and FL=@floor (@floor 来自页面用户的选择)查询PAK_LocMas表存在记录时
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="tp"></param>
        /// <param name="fl"></param>
        /// <returns></returns>
        bool CheckExistPakLocMas(string pno, string tp, string fl);

        /// <summary>
        /// 并更新PAK_LocMas表中满足Pno=@plt and Tp='PakLoc' 条件的记录的PdLine字段值
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="pno"></param>
        /// <param name="tp"></param>
        void UpdatePakLocMasForPdLine(string pdLine, string pno, string tp);

        /// <summary>
        /// 按照SnoId 字段转为整型逆序排序，取第一条记录的SnoId 字段值保存到变量@loc 中
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="tp"></param>
        /// <param name="fl"></param>
        /// <returns></returns>
        IList<PakLocMasInfo> GetPakLocMasList(string pno, string tp, string fl);

        /// <summary>
        /// 并更新PAK_LocMas表中满足SnoId=@loc and Tp='PakLoc' and Pno='' 条件的记录的Pno 字段值为@plt，PdLine 字段值为Product Id 的第一位字符，Udt 为当前时间
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="newPno"></param>
        /// <param name="oldPno"></param>
        /// <param name="tp"></param>
        void UpdatePakLocMasForPdLineAndPno(string pdLine, string newPno, string oldPno, string tp, string snoId);

        /// <summary>
        /// 1. 查询PAK_LocMas表，取满足条件Tp='PakLoc' and Pno=@plt 记录的SnoId 字段值保存到变量@loc 中
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="tp"></param>
        /// <param name="fl"></param>
        /// <returns></returns>
        IList<PakLocMasInfo> GetPakLocMasList(string pno, string tp);

        /// <summary>
        /// Almighty
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PakLocMasInfo> GetPakLocMasList(PakLocMasInfo condition);

        /// <summary>
        /// Almighty too
        /// </summary>
        /// <param name="eqCondition"></param>
        /// <param name="neqCondition"></param>
        /// <returns></returns>
        IList<PakLocMasInfo> GetPakLocMasList(PakLocMasInfo eqCondition, PakLocMasInfo neqCondition);

        /// <summary>
        /// 并更新PAK_LocMas表中满足条件Pno=@plt and Tp='PakLoc' 记录的Pno 字段值为''
        /// </summary>
        /// <param name="newPno"></param>
        /// <param name="oldPno"></param>
        /// <param name="tp"></param>
        void UpdatePakLocMasForPno(string newPno, string oldPno, string tp);

        /// <summary>
        /// INSERT HP_EDI.dbo.PAKODMSESSION
        /// VALUES (@SN,'ODM_TEXT1',RTRIM(@plt),@mrpdesc,@desc,1)
        /// </summary>
        /// <param name="item"></param>
        void InsertPakOdmSession(PakOdmSessionInfo item);

        /// <summary>
        /// IF EXISTS (SELECT * FROM HP_EDI..[PAK.PAKRT] 
        /// WHERE DOC_SET_NUMBER=@setnum 
        /// AND XSL_TEMPLATE_NAME like '%Verizon2D%' 
        /// AND DOC_CAT='Pallet Ship Label- Pack ID Single')
        /// </summary>
        /// <param name="docCat"></param>
        /// <param name="docSetNumer"></param>
        /// <returns></returns>
        bool CheckExistFromPakDotPakRtByDocCatAndDocSetNumer(string docCat, string docSetNumer, string xslTemplateNameInnerStr);

        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<WhPltWeightInfo> GetWhPltWeightList(WhPltWeightInfo condition);

        /// <summary>
        /// setValue哪个字段赋值就有更新哪个字段
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateWhPltWeight(WhPltWeightInfo setValue, WhPltWeightInfo condition);

        /// <summary>
        /// 取得所有Pallet 重量
        /// 参考方法：
        /// SELECT DISTINCT PalletNo as SnoId, convert(decimal(9,1), ISNULL(RTRIM([Weight]), '0')) as KG
        /// FROM Pallet nolock
        /// </summary>
        /// <returns></returns>
        IList<PalletNoAndWeight> GetAllDistinctPalletNoAndWeights();

        /// <summary>
        /// SELECT a.PalletNo, SUM(CONVERT(decimal(9,1), ISNULL(a.UnitWeight, '0')))
        /// FROM IMES_FA..Product a (nolock), #plt b(nolock)
        /// WHERE a.PalletNo = b.PLT
        /// GROUP BY a.PalletNo
        /// 
        /// LD: 重复计算有意义么？这样的SUM肯定会多.
        /// </summary>
        /// <param name="plts"></param>
        /// <returns></returns>
        IList<PalletNoAndWeight> GetPalletNoAndWeightConsiderProduct(IList<PalletNoAndWeight> plts);

        /// <summary>
        /// 使用family查询IMES_PAK..ChinaLabel这个表
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<ChinaLabelInfo> GetChinaLabel(ChinaLabelInfo condition);

        /// <summary>
        /// 使用UPPER(Family) like @family3 +'%' 查询IMES_PAK..ChinaLabel表存在记录
        /// </summary>
        /// <param name="familyPrefix"></param>
        /// <returns></returns>
        IList<ChinaLabelInfo> GetChinaLabelByLikeFamily(string familyPrefix);

        /// <summary>
        /// 2.use IMES2012_KIT
        /// select * from dbo.Kitting_Loc_PLMapping_St
        /// where PdLine=@line and Station=@station and LightNo=@lightno
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="lightno"></param>
        /// <returns></returns>
        IList<KittingLocPLMappingStInfo> GetKitLocPLMapST(string line, string station, short lightno);

        /// <summary>
        /// 3.
        /// update dbo.Kitting_Location_FV
        /// set ConfigedLEDStatus=@configstatus，RunningLEDStatus=@runningStatus，LEDValues=@ledvalue
        /// where TagID=@tagid
        /// </summary>
        /// <param name="tagid"></param>
        /// <param name="configstatus"></param>
        /// <param name="runningStatus"></param>
        /// <param name="ledvalue"></param>
        void UpdateKitLocationFVOn(string tagid, bool configstatus, bool runningStatus, string ledvalue);

        /// <summary>
        /// update dbo.Kitting_Location_FV
        /// set ConfigedLEDStatus=@configstatus，RunningLEDStatus=@runningStatus，LEDValues=@ledvalue
        /// where TagID IN (@tagid1,@tagid2,...,@tagidN) 
        /// </summary>
        /// <param name="tagids"></param>
        /// <param name="configstatus"></param>
        /// <param name="runningStatus"></param>
        /// <param name="ledvalue"></param>
        void UpdateKitLocationFVOn(string[] tagids, bool configstatus, bool runningStatus, string ledvalue);

        /// <summary>
        /// 4.
        /// update dbo.Kitting_Location_FV
        /// set ConfigedLEDStatus=configstatus，RunningLEDStatus=@runningStatus 
        /// where TagID=@tagid
        /// </summary>
        /// <param name="tagid"></param>
        /// <param name="configstatus"></param>
        /// <param name="runningStatus"></param>
        void UpdateKitLocationFVOff(string tagid, bool configstatus, bool runningStatus);

        /// <summary>
        /// setValue哪个字段赋值就有更新哪个字段
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateKittingLocationFaXInfo(KittingLocationFaXInfo setValue, int configedLEDStatus, int runningLEDStatus, int comm, KittingLocationFaXInfo condition, int[] proritySet);

        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<KittingLocPLMappingInfo> GetKittingLocPLMappingInfoList(KittingLocPLMappingInfo condition);

        /// <summary>
        /// 2. 得到库里的栈板数			select count(PLT) from WH_PLTMas (nolock) where  WC='IN'
        /// </summary>
        /// <param name="wc"></param>
        /// <returns></returns>
        int GetCountOfWhPltMas(string wc);

        /// <summary>
        /// 4.得到7天以上入库的栈板数据显示在界面的table区域			首先获取小于7天入库的栈板
        /// select PLT into #p from WH_PLTMas where WC='IN' and Cdt<dateadd(dd,-7,getdate())
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        IList<WhPltMasInfo> GetWhPltMasList(string wc, int days);
        
        /// <summary>
        /// SP: op_PackingData
        /// Parameters
        /// 
        /// OLD:
        /// 
        /// @snoid char(9),    // --Product Id
        /// @model char(12),   //--Product Model
        /// @dn char(16),      //--Delivery
        /// @plt char(12),     //--Pallet
        /// @loc varchar(10),  //--Location
        /// @pltqty int        //--Select Sum(DeliveryQty) from Delivery_Pallet where PalletNo = @plt
        /// 
        /// NEW:
        /// 
        /// @snoID char(9),		--Product Id
        /// @Model char(12),	--Product Model
        /// @Delivery char(16),	
        /// @Pallet char(12),	
        /// @Location varchar(10),
        /// @pltqty int, 
        /// @BoxId varchar(20),
        /// @UCC varchar(20)
        /// </summary>
        /// <param name="snoid"></param>
        /// <param name="model"></param>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="loc"></param>
        /// <param name="pltqty"></param>
        /// <param name="boxid"></param>
        /// <param name="ucc"></param>
        void CallOp_PackingData(string snoid, string model, string dn, string plt, string loc, int pltqty, string boxid, string ucc);

        /// <summary>
        /// 调用HP_EDI.dbo.op_PackingData_20111031 @BoxId,@dn,@plt,@cn,@cdtEDI实现
        /// Parameters:
        /// @BoxId – Box Id
        /// @dn – Delivery No
        /// @plt – Pallet No
        /// @cn – Carton No
        /// @cdtEDI - GETDATE()
        /// </summary>
        /// <param name="boxid"></param>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="cn"></param>
        /// <param name="cdt"></param>
        void CallOp_PackingData(string boxid, string dn, string plt, string cn, DateTime cdt);

        /// <summary>
        /// 1,SQL:select * from COMSetting order by name;
        /// </summary>
        /// <returns></returns>
        IList<COMSettingInfo> GetAllCOMSetting();

        /// <summary>
        /// 2,SQL:delete from COMSetting where id=[id]
        /// </summary>
        /// <param name="id"></param>
        void RemoveCOMSettingItem(int id);

        /// <summary>
        /// 3,SQL:insert into COMSetting (item各个字段);
        /// </summary>
        /// <param name="item"></param>
        void AddCOMSettingItem(COMSettingInfo item);

        /// <summary>
        /// 4,SQL: update COMSetting set [Item各个字段] where id=[item.id]
        /// </summary>
        /// <param name="item"></param>
        void UpdateCOMSettingItem(COMSettingInfo item);

        /// <summary>
        /// 5,SQL:select * from COMSetting where name=[name]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IList<COMSettingInfo> FindCOMSettingByName(string name);

        /// <summary>
        /// Remove Pallets
        /// </summary>
        /// <param name="pltNos"></param>
        void DeletePallets(IList<string> pltNos);

        /// <summary>
        /// Remove Pallet Attributes
        /// </summary>
        /// <param name="pltNos"></param>
        void DeletePalletAttrs(IList<string> pltNos);

        /// <summary>
        /// setValue哪个字段赋值就有更新哪个字段
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePakLocMasInfo(PakLocMasInfo setValue, PakLocMasInfo condition);

        /// <summary>
        /// Call SP [IMES_DismantlePalletWeight] @PalletOrDn varchar(20),@Editor varchar(30)
        /// </summary>
        /// <param name="palletOrDn"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        IList<DNPalletWeight> DismantlePalletWeight(string palletOrDn,string editor);

        /// <summary>
        /// Call SP [IMES_QueryPalletWeight] @PalletOrDn varchar(20)
        /// </summary>
        /// <param name="palletOrDn"></param>
        /// <returns></returns>
        IList<DNPalletWeight> QueryPalletWeight(string palletOrDn);

        /// <summary>
        /// Select PalletNo, Editor, Cdt,Udt from Pallet where Udt between @From and @To ORDER BY Udt, PalletNo
        /// select PalletNo, Editor, Cdt,Udt from Pallet where Udt between '2012/02/04' and '2012/02/07' and Station = 'DT' 
        /// from,to 格式为 YYYY/MM/DD,
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<DTPallet> GetDTPalletByUdt(string from, string to, string station);

        /// <summary>
        /// 2) 执行存储过程 rpt_PAKKittingLoc_up @pdline char(2)
        /// </summary>
        /// <param name="pdline"></param>
        /// <returns></returns>
        DataTable ExecSpRptPAKKittingLocUp(string pdline);

        /// <summary>
        /// 3) 执行存储过程 rpt_KittingLoc_up @pdline char(2)
        /// </summary>
        /// <param name="pdline"></param>
        /// <returns></returns>
        DataTable ExecSpRptKittingLocUp(string pdline);

        /// <summary>
        /// 4) 执行存储过程 [rpt_KittingLoc] @code varchar(25), @kittingType varchar(20), @isLine char(4)
        /// </summary>
        /// <param name="pdline"></param>
        /// <returns></returns>
        DataTable ExecSpRptKittingLoc(string code, string kittingType, string isLine);

        /// <summary>
        /// 当栈板号等于PAK_WH_LocMas.PLT1或PLT2时
        /// </summary>
        /// <param name="plt1Orplt2"></param>
        /// <returns></returns>
        IList<PakWhLocMasInfo> GetPakWhLocMasListByPlt1OrPlt2(string plt1Orplt2);

        /// <summary>
        /// 执行存储过程[rpt_KittingLoc_up] 参数为  @pdline char(2), @family varchar(40)
        /// </summary>
        /// <param name="line"></param>
        /// <param name="family"></param>
        DataTable ExecSpRptKittingLocUp(string line, string family);

        string GetAutoAssignPallet(string deliveryNo);

        /// <summary>
        /// 1.Select PLT, Editor, Cdt from WH_PLTLog where WC IN ('DT','RD') and Cdt between @From and @To ORDER BY Cdt, PLT 
        /// </summary>
        /// <param name="wcs"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IList<WhPltLogInfo> GetWhPltLogInfoList(string[] wcs, DateTime from, DateTime to);

        /// <summary>
        /// 2.Select PLT, Editor, Cdt from WH_PLTLog where WC IN ('DT','RD') and PLT = @PalletNo ORDER BY Cdt, PLT
        /// </summary>
        /// <param name="wcs"></param>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<WhPltLogInfo> GetWhPltLogInfoList(string[] wcs, string palletNo);

        /// <summary>
        /// 3.Select top 1 * from WH_PLTLog where PLT = @PalletNo ORDER BY Cdt desc
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        WhPltLogInfo GetWhPltLogInfoNewestly(string palletNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="palletNo"></param>
        void DeleteDummyDetInfoByPlt(string palletNo);

        /// <summary>
        /// SELECT @btloc = Sno FROM SnoDet_BTLoc WHERE SnoId = @snoid
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetSnoListFromSnoDetBtLoc(SnoDetBtLocInfo condition);

        /// <summary>
        /// SELECT @cmbqty = COUNT(SnoId) FROM SnoDet_BTLoc WHERE Sno = @btloc and Status = 'In'
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCountOfSnoIdFromSnoDetBtLoc(SnoDetBtLocInfo condition);

        /// <summary>
        /// UPDATE SnoDet_BTLoc SET Status = 'Out',Udt = GETDATE(),Editor=@user WHERE SnoId = @snoid
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateSnoDetBtLoc(SnoDetBtLocInfo setValue, SnoDetBtLocInfo condition);

        /// <summary>
        /// UPDATE PAK_BTLocMas SET CmbQty=@cmbqty WHERE SnoId = @btloc
        /// UPDATE PAK_BTLocMas SET [Status] = 'OPEN',Model = 'Other' WHERE SnoId = @btloc
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePakBtLocMas(PakBtLocMasInfo setValue, PakBtLocMasInfo condition);

        /// <summary>
        /// 1）PAK_CHN_TW_Light表: 获取该表所有记录集数据；
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PakChnTwLightInfo> GetPakChnTwLightInfoList(PakChnTwLightInfo condition);

        /// <summary>
        /// 2）PAK_CHN_TW_Light表: 添加一条记录；
        /// </summary>
        /// <param name="condition"></param>
        void InsertPakChnTwLightInfo(PakChnTwLightInfo condition);

        /// <summary>
        /// 3）PAK_CHN_TW_Light表: 修改一条记录；
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePakChnTwLightInfo(PakChnTwLightInfo setValue, PakChnTwLightInfo condition);

        /// <summary>
        /// 4）PAK_CHN_TW_Light表: 删除一条记录；
        /// </summary>
        /// <param name="condition"></param>
        void DeletePakChnTwLightInfo(PakChnTwLightInfo condition);

        /// <summary>
        /// SELECT @loc=SnoId FROM PAK_LocMas(NOLOCK) WHERE Tp='WHLoc' AND Pno='' ORDER BY CONVERT(int, SnoId)
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="pno"></param>
        /// <returns></returns>
        string GetMinSnoIdByTpAndPno(string tp, string pno);

        int GetAResultForConsolidatedByPalletNo(string palletNo);

        /// <summary>
        /// SELECT COUNT(DISTINCT b.PLT)
        /// FROM FwdPlt a (NOLOCK) LEFT JOIN Pallet_RFID b (NOLOCK)
        ///       ON a.Plt = b.PLT
        /// WHERE a.PickID = @PickId
        ///       AND a.Status = 'Out'
        ///       AND CONVERT(char(10), a.Udt, 111) = CONVERT(char(10), GETDATE(), 111) 
        /// </summary>
        /// <param name="pickId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int GetChepPalletQty(string pickId, string status);

        /// <summary>
        /// IMES_PickCardCheck
        /// @TruckID varchar(50),--Truck ID
        /// @Date varchar(10) -- Date
        ///     IF @ProductsQty <> @PalletVerifyQty
        ///     BEGIN
        ///         SELECT '0'
        ///         RETURN
        ///     END
        ///     ELSE
        ///     BEGIN
        ///         SELECT '1'
        ///         RETURN
        ///     END
        /// 返回1 Pass
        /// 返回0 fail
        /// 
        /// 该方法调用永勃的存储过程。根据存储过程返回结果：
        ///          0 false
        ///          1 true
        /// </summary>
        /// <param name="truckID"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        bool PickCardCheck(string truckID, string strDate);

        /// <summary>
        /// 1）SELECT * FROM PLTStandard WHERE PLTNO=@PLT
        /// 2）SELECT * FROM PLTStandard
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PltstandardInfo> GetPltstandardInfoList(PltstandardInfo condition);

        /// <summary>
        /// 3）SELECT Descr FROM PLTSpecification NOLOCK ORDER BY ID
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PltspecificationInfo> GetPltspecificationInfoList(PltspecificationInfo condition);

        /// <summary>
        /// 5）update PLTStandard表的通用方法
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePltstandardInfo(PltstandardInfo setValue, PltstandardInfo condition);

        /// <summary>
        /// 6）insert PLTStandard表的通用方法
        /// </summary>
        /// <param name="item"></param>
        void AddPltstandardInfo(PltstandardInfo item);

        /// <summary>
        /// 删除PLTStandard表
        /// </summary>
        /// <param name="condition"></param>
        void DeletePltstandardInfo(PltstandardInfo condition);

        /// <summary>
        /// 检索PalletLog表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<PalletLogInfo> GetPalletLogInfoList(PalletLogInfo condition);

        /// <summary>
        /// Delete PalletLog – 删除该Pallet Pass Pallet Verify_RCTO 的Log
        /// </summary>
        /// <param name="condition"></param>
        void DeletePalletLog(PalletLogInfo condition);

        /// <summary>
        /// SELECT DISTINCT FL AS Floor FROM PAK_LocMas
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetFlListFromPakLacMas(PakLocMasInfo condition);

        /// <summary>
        /// select COUNT(PalletNo) from Pallet where PalletNo in ( select PalletNo from Delivery_Pallet where 
        ///DeliveryNo in (select DeliveryNo from Delivery_Pallet where PalletNo =@palletno)) and Weight<>0
        /// </summary>
        /// <param name="palletno"></param>
        /// <returns></returns>
        bool CheckAllPalletWeightOnDeliverysByPallet(string palletno);

        #region . OnTrans .

        Pallet Find_OnTrans(object key);

        #endregion

        #region . Defered .

        void AddGetChepPalletInfoDefered(IUnitOfWork uow, ChepPalletInfo item);

        void DeleteChepPalletInfoDefered(IUnitOfWork uow, int id);

        void AddQtyInfoDefered(IUnitOfWork uow, PalletQtyInfo pqInfo);

        void DeleteQtyInfoDefered(IUnitOfWork uow, int id);

        void UpdateQtyInfoDefered(IUnitOfWork uow, PalletQtyInfo pqInfo, int id);

        void UpdateStatusForPakBtLocMasDefered(IUnitOfWork uow, string status, string snoId);

        void UpdateStatusForPakBtLocMasDefered(IUnitOfWork uow, string status, string snoId, string editor);

        void InsertSnoDetBtLocInfoDefered(IUnitOfWork uow, SnoDetBtLocInfo item);

        void UpdateForIncPakBtLocMasDefered(IUnitOfWork uow, string snoId, string editor);

        void UpdateForIncPakBtLocMasDefered(IUnitOfWork uow, string snoId);

        void UnPackPakOdmSessionByDeliveryNoDefered(IUnitOfWork uow, string dn);

        void UnPackPackingDataByDeliveryNoDefered(IUnitOfWork uow, string dn);

        void InsertWhPltLogDefered(IUnitOfWork uow, WhPltLogInfo item);

        void UpdateWhPltMasDefered(IUnitOfWork uow, WhPltMasInfo item, string plt);

        void InsertWhPltMasDefered(IUnitOfWork uow, WhPltMasInfo item);

        void DeletePakWhPltTypeByPltDefered(IUnitOfWork uow, string plt);

        void InsertPakWhPltTypeInfoDefered(IUnitOfWork uow, PakWhPltTypeInfo item);

        void UpdatePakWhLocBolByColAndLocDefered(IUnitOfWork uow, string bol, string col, int loc);

        void InsertWhPltLocLogInfoDefered(IUnitOfWork uow, WhPltLocLogInfo item);

        void UpdatePakWhLocByColAndLocDefered(IUnitOfWork uow, PakWhLocMasInfo item, string col, int loc);

        void UpdatePakWhLocByPltForClearPlt1AndPlt2Defered(IUnitOfWork uow, string plt);

        void InsertPoPltDefered(IUnitOfWork uow, PoPltInfo item);

        void InsertPoDataDefered(IUnitOfWork uow, PoDataInfo item);

        void DeletePalletsByDnDefered(IUnitOfWork uow, string dn);

        void DeletePalletsByShipmentNoDefered(IUnitOfWork uow, string shipmentNo);

        void UpdatePakWhLocByPltForClearBolDefered(IUnitOfWork uow, string bol, string editor);

        void UpdatePakWhLocByPltForClearBolAndPlt1AndPlt2Defered(IUnitOfWork uow, string palletNo);

        void UpdatePakWhLocTmpForClearPltAndTpAndBolDefered(IUnitOfWork uow, string palletNo);

        void UpdatePakWhLocTmpForClearBolDefered(IUnitOfWork uow, string bol);

        void DeleteChepPalletInfoDefered(IUnitOfWork uow, string palletNo);

        void DeletePalletRfidDefered(IUnitOfWork uow, string plt, string rfidCode);

        void InsertPalletRfidDefered(IUnitOfWork uow, PalletRfidInfo item);

        void UpdatePakLocMasForPdLineDefered(IUnitOfWork uow, string pdLine, string pno, string tp);

        void UpdatePakLocMasForPdLineAndPnoDefered(IUnitOfWork uow, string pdLine, string newPno, string oldPno, string tp, string snoId);

        void UpdatePakLocMasForPnoDefered(IUnitOfWork uow, string newPno, string oldPno, string tp);

        void InsertPakOdmSessionDefered(IUnitOfWork uow, PakOdmSessionInfo item);

        void UpdateWhPltWeightDefered(IUnitOfWork uow, WhPltWeightInfo setValue, WhPltWeightInfo condition);

        void UpdateKitLocationFVOnDefered(IUnitOfWork uow, string tagid, bool configstatus, bool runningStatus, string ledvalue);

        void UpdateKitLocationFVOnDefered(IUnitOfWork uow, string[] tagids, bool configstatus, bool runningStatus, string ledvalue);

        void UpdateKitLocationFVOffDefered(IUnitOfWork uow, string tagid, bool configstatus, bool runningStatus);

        void UpdateKittingLocationFaXInfoDefered(IUnitOfWork uow, KittingLocationFaXInfo setValue, int configedLEDStatus, int runningLEDStatus, int comm, KittingLocationFaXInfo condition, int[] proritySet);

        void CallOp_PackingDataDefered(IUnitOfWork uow, string snoid, string model, string dn, string plt, string loc, int pltqty, string boxid, string ucc);

        void CallOp_PackingDataDefered(IUnitOfWork uow, string boxid, string dn, string plt, string cn, DateTime cdtEdi);

        void RemoveCOMSettingItemDefered(IUnitOfWork uow, int id);

        void AddCOMSettingItemDefered(IUnitOfWork uow, COMSettingInfo item);

        void UpdateCOMSettingItemDefered(IUnitOfWork uow, COMSettingInfo item);

        void DeletePalletsDefered(IUnitOfWork uow, IList<string> pltNos);

        void DeletePalletAttrsDefered(IUnitOfWork uow, IList<string> pltNos);

        void UpdatePakLocMasInfoDefered(IUnitOfWork uow, PakLocMasInfo setValue, PakLocMasInfo condition);

        void DeleteDummyDetInfoByPltDefered(IUnitOfWork uow, string palletNo);

        void UpdateSnoDetBtLocDefered(IUnitOfWork uow, SnoDetBtLocInfo setValue, SnoDetBtLocInfo condition);

        void UpdatePakBtLocMasDefered(IUnitOfWork uow, PakBtLocMasInfo setValue, PakBtLocMasInfo condition);

        void InsertPakChnTwLightInfoDefered(IUnitOfWork uow, PakChnTwLightInfo item);

        void UpdatePakChnTwLightInfoDefered(IUnitOfWork uow, PakChnTwLightInfo setValue, PakChnTwLightInfo condition);

        void DeletePakChnTwLightInfoDefered(IUnitOfWork uow, PakChnTwLightInfo condition);

        void RemoveFwdPltByPickIDDefered(IUnitOfWork uow, string pickID);

        void UpdatePltstandardInfoDefered(IUnitOfWork uow, PltstandardInfo setValue, PltstandardInfo condition);

        void AddPltstandardInfoDefered(IUnitOfWork uow, PltstandardInfo item);

        void DeletePltstandardInfoDefered(IUnitOfWork uow, PltstandardInfo condition);

        void DeletePalletLogDefered(IUnitOfWork uow, PalletLogInfo condition);

        #endregion


        #region for MIC update data
        IList<PalletAttr> GetPalletAttr(string palletNo);
        PalletAttr GetPalletAttr(string palletNo, string attrName);
        IList<PalletAttrLog> GetPalletAttrLog(string palletNo);
        IList<PalletAttrLog> GetPalletAttrLog(string palletNo, string attrName);
        void UpdateAttr(string palletNo, string attrName, string attrValue, string descr, string editor);
        void UpdateAttrDefered(IUnitOfWork uow, string palletNo, string attrName, string attrValue, string descr, string editor);

        IList<FwdPltInfo> GetFwdPltInfosByPickIDAndStatusAndDate(string pickID, string status, DateTime date, string notAllowStatus);
        void UpdatePakLocMasById(int id, string palletNo, string pdLine, string editor);
        void UpdatePakLocMasByIdDefered(IUnitOfWork uow, int id, string palletNo, string pdLine, string editor);
        #endregion

        #region get Docking Pallet
        Pallet FindWithDocking(string  PalletNo);
        #endregion
    }
}
