// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Pizza对象Repository接口
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using System.Data;

namespace IMES.FisObject.PAK.Pizza
{
    /// <summary>
    /// Pizza对象Repository接口
    /// </summary>
    public interface IPizzaRepository : IRepository<Pizza>
    {
        /// <summary>
        /// 获取Pizaa对象绑定的料
        /// </summary>
        /// <param name="currentPizza"></param>
        /// <returns></returns>
        Pizza FillPizzaParts(Pizza currentPizza);

        /// <summary>
        /// 获取pizza对象的状态
        /// </summary>
        /// <param name="currentPizza"></param>
        /// <returns></returns>
        Pizza FillPizzaStatus(Pizza currentPizza);

        /// <summary>
        /// 从Pizza_Part表根据PartNo和Value取多条记录
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IList<IProductPart> GetPizzaPartsByPartNoAndValue(string partNo, string val);

        /// <summary>
        /// 从Pizza_Part表根据PartSn取多条记录
        /// </summary>
        /// <param name="partSn"></param>
        /// <returns></returns>
        IList<IProductPart> GetPizzaPartsByPartSn(string partSn);

        /// <summary>
        /// 从Pizza_Part表根据PizzaID获取绑定的Part数量
        /// 用在Reprint判断一个Pizza是否有绑定的Parts
        /// </summary>
        /// <param name="pizzaID"></param>
        /// <returns></returns>
        int GetPizzaPartsCout(string pizzaID);

        /// <summary>
        /// select * from Pizza_Part where value = @value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IList<PizzaPart> GetPizzaPartsByValue(string value);
      
        /// <summary>
        /// Doc_Type list : select distinct DOC_CAT from [PAK.PAKRT] where DOC_CAT like 'Pack List%' order by DOC_CAT  desc
        /// </summary>
        /// <param name="docCat"></param>
        /// <returns></returns>
        IList<string> GetDocCatsFromPakPakRT(string docCat);

        /// <summary>
        /// Region list : select distinct REGION from v_PAKComn nolock
        /// </summary>
        /// <returns></returns>
        IList<string> GetRegionsFromVPakComn();

        /// <summary>
        /// Carrier List : select distinct INTL_CARRIER from v_PAKComn nolock
        /// </summary>
        /// <returns></returns>
        IList<string> GetIntlCarrierListFromVPakComn();

        /// <summary>
        /// A. 刷入的是DN
        ///  exists (select * from [PAK_PAKComn] nolock where left(InternalID,10)=@DN)
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        bool CheckExistPakDashPakComnByInternalID(string internalId);

        /// <summary>
        /// B.刷入的是Shipment
        /// exists(select * from v_Shipment_PAKComn nolock where CONSOL_INVOICE=@DN or SHIPMENT = @DN)
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        bool CheckExistVShipmentPakComnByConsolInvoiceOrShipment(string dn);

        /// <summary>
        /// C.输入的是WayBill Number
        /// exists(select * from v_Shipment_PAKComn nolock where WAYBILL_NUMBER=@DN )
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        bool CheckExistVShipmentPakComnByWaybillNo(string waybillNo);

        /// <summary>
        /// A. 刷入的是DN
        /// select @shipdate=ACTUAL_SHIPDATE,@PO_NUM = PO_NUM, @deliveryno = InternalID,@shipment = rtrim(CONSOL_INVOICE),@model = Model,@region = REGION,@sales_chan = SALES_CHAN,@order_type = ORDER_TYPE,@intl_carrier = INTL_CARRIER
        /// from dbo.v_Shipment_PAKComn nolock where left(InternalID,10)=@DN
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<VShipmentPakComnInfo> GetVShipmentPakComnListByLikeInternalID(string internalId);

        /// <summary>
        /// exists(select * from [PAK.PAKEdi850raw] where PO_NUM = @PO_NUM)
        /// </summary>
        /// <param name="poNum"></param>
        /// <returns></returns>
        bool CheckExistPAKDotPAKEdi850RawByPoNum(string poNum);

        /// <summary>
        /// B.刷入的是Shipment
        /// select distinct InternalID,Model,PO_NUM,ACTUAL_SHIPDATE into #3 from v_Shipment_PAKComn where CONSOL_INVOICE=@DN or SHIPMENT = @DN
        /// select distinct InternalID,Model,PO_NUM,ACTUAL_SHIPDATE into #3 from v_Shipment_PAKComn where CONSOL_INVOICE=@DN or SHIPMENT = @DN
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<VShipmentPakComnInfo> GetVShipmentPakComnByConsolInvoiceOrShipment(string dn);

        /// <summary>
        /// if exists(select * from #3 where PO_NUM not in (select PO_NUM from [PAK.PAKEdi850raw]))
        /// </summary>
        /// <returns></returns>
        bool CheckExistPoNumNotInPAKDotPAKEdi850Raw(IList<string> infos);

        /// <summary>
        /// C.输入的是WayBill Number
        /// select distinct InternalID,Model,PO_NUM ,ACTUAL_SHIPDATE  into #1 from v_Shipment_PAKComn where WAYBILL_NUMBER=@DN
        /// select distinct InternalID,Model,PO_NUM ,ACTUAL_SHIPDATE  into #1 from v_Shipment_PAKComn where WAYBILL_NUMBER=@DN
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        IList<VShipmentPakComnInfo> GetVShipmentPakComnListByWaybillNo(string waybillNo);

        /// <summary>
        /// A. 刷入的是DN
        /// select @instr = INSTR_FLAG from [PAK_PAKComn] nolock where left(InternalID,10)=@DN
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<PakDashPakComnInfo> GetPakDashPakComnListByInternalID(string internalId);

        /// <summary>
        /// exists(select * from dbo.PAKEDI_INSTR where PO_NUM = @PO_NUM)
        /// </summary>
        /// <param name="poNum"></param>
        /// <returns></returns>
        bool CheckExistPakEdiInstr(string poNum);

        /// <summary>
        /// B.刷入的是Shipment
        /// select distinct PO_NUM into #SInstr from [PAK_PAKComn] nolock where InternalID in (select InternalID from #3) and upper(INSTR_FLAG) = 'X'
        /// </summary>
        /// <param name="internalIds"></param>
        /// <param name="instrFlag"></param>
        /// <returns></returns>
        IList<string> GetPoNumListFromPakDashPakComn(IList<string> internalIds, string instrFlag);

        /// <summary>
        /// if exists (select * from #SInstr where PO_NUM not in (select PO_NUM from dbo.PAKEDI_INSTR))
        /// </summary>
        /// <param name="instrs"></param>
        /// <returns></returns>
        bool CheckExistPoNumNotInPakEdiInstr(IList<string> instrs);

        /// <summary>
        /// 依据刷入的code获取数据：
        /// 刷入的是DN：
        /// select @InternalID = InternalID from v_Shipment_PAKComn nolock where left(InternalID,10)=@DN
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<string> GetInternalIDsFromVShipmentPakComnListByLikeInternalID(string internalId);

        /// <summary>
        /// 刷入的是Shipment：
        /// select distinct InternalID from v_Shipment_PAKComn where CONSOL_INVOICE=@DN or SHIPMENT = @DN order by InternalID
        /// select @deliveryno = InternalID from v_Shipment_PAKComn nolock where (CONSOL_INVOICE = @DN or SHIPMENT = @DN)
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<string> GetInternalIDsFromVShipmentPakComnListByConsolInvoiceOrShipment(string dn);

        /// <summary>
        /// select @deliveryno = InternalID from v_Shipment_PAKComn nolock where (CONSOL_INVOICE = @DN or SHIPMENT = @DN)and REGION='EMEA'
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        IList<string> GetInternalIDsFromVShipmentPakComnListByConsolInvoiceOrShipmentAndRegion(string dn, string region);

        /// <summary>
        /// 输入的是WayBill Number
        /// select distinct InternalID from v_Shipment_PAKComn where WAYBILL_NUMBER=@DN order by InternalID
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        IList<string> GetInternalIDsFromVShipmentPakComnListByWaybillNo(string waybillNo);

        /// <summary>
        ///  2)得到模板：
        ///  select @doc_set_number = DOC_SET_NUMBER  from [PAK.PAKComn] where left(InternalID,10) = @InternalID的前10位 
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<string> GetDocSetNumListFromPakDashPakComnByLikeInternalID(string internalId);

        /// <summary>
        /// SELECT @templatename = XSL_TEMPLATE_NAME FROM [PAK.PAKRT] WHERE DOC_CAT = @doctpye AND DOC_SET_NUMBER = @doc_set_number
        /// </summary>
        /// <param name="docCat"></param>
        /// <param name="docSetNumer"></param>
        /// <returns></returns>
        IList<string> GetXslTemplateNameListFromPakDashPakComnByDocCatAndDocSetNumer(string docCat, string docSetNumer);

        /// <summary>
        /// 3)生成pdf文件名：
        /// select @delivery=rtrim(left(InternalID,10)),@shipment=rtrim(SHIPMENT),@waybill=rtrim(WAYBILL_NUMBER) from [v_PAKComn] where InternalID = @InternalID
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<VPakComnInfo> GetVPakComnList(string internalId);

        /// <summary>
        /// exists (select * from PAK_PackkingData nolock where InternalID in @deliverynolist)
        /// </summary>
        /// <param name="dnList"></param>
        /// <returns></returns>
        bool CheckExistPakPackkingDataByDnList(IList<string> dnList);

        /// <summary>
        /// if exists(select top 1 * from [PAK_PAKComn] nolock where left(InternalID,10)=@DN and Model like 'PO%')
        /// </summary>
        /// <param name="internalId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CheckExistPakDashPakComnByLikeInternalIDAndModel(string internalId, string model);

        /// <summary>
        /// insert  #DockingPackkingData
        /// select *  from [10.99.183.29].HP_EDI.dbo.[PAK_PackkingData] where InternalID like @DN+'%'
        /// if exists (select top 1 * from #DockingPackkingData)
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<PakPackkingDataInfo> GetPakPackkingDataListByLikeInternalID(string internalId);
   
        /// <summary>
        /// insert PAK_PackkingData_Del
        /// select *,getdate() from PAK_PackkingData where InternalID in (select InternalID from #DockingPackkingData)
        /// </summary>
        /// <param name="internalIds"></param>
        void InsertForBackupPakPackkingData(IList<string> internalIds);
  
        /// <summary>
        /// delete from PAK_PackkingData where InternalID in (select InternalID from #DockingPackkingData)
        /// </summary>
        /// <param name="internalIds"></param>
        void DeletePakPackkingData(IList<string> internalIds);

        /// <summary>
        /// insert PAK_PackkingData select * from #DockingPackkingData
        /// </summary>
        /// <param name="?"></param>
        void InsertPakPackkingData(IList<PakPackkingDataInfo> items);

        /// <summary>
        /// 只针对刷入的是DN这种情况作此检查：
        /// select  @qty=sum(convert( NUMERIC(18,0),PACK_ID_UNIT_QTY)) from PAK_PAKComn where left(InternalID,10) = left(@deliveryno,10)
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        decimal GetCountOfPakDashPakComnByLikeInternalID(string internalId);

        /// <summary>
        /// select @cqty=count(SERIAL_NUM) from PAK_PackkingData where left(InternalID,10) = left(@deliveryno,10)
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        int GetCountOfPakPackkingDataByLikeInternalID(string internalId);

        /// <summary>
        /// exists (select * from dbo.PAK_PackkingData nolock where InternalID = @deliveryno and TRACK_NO_PARCEL <> '')
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        bool CheckExistPakPackkingData(string internalId);

        /// <summary>
        /// select distinct left(a.InternalID,10) from v_PAKComn a,[PAK.PAKEdi850raw] b 
        /// where a.REGION = @region and a.ACTUAL_SHIPDATE between convert(nvarchar(10),@Cdt,120) 
        /// and convert(nvarchar(10),@Edt,120)  and a.PO_NUM=b.PO_NUM
        /// </summary>
        /// <param name="region"></param>
        /// <param name="cdt"></param>
        /// <param name="edt"></param>
        /// <returns></returns>
        IList<string> GetInternalIdsFromVPakComn(string region, DateTime cdt, DateTime edt);

        /// <summary>
        /// select distinct left(a.InternalID,10) from v_PAKComn a,[PAK.PAKEdi850raw] b
        /// where a.REGION = @region and a.ACTUAL_SHIPDATE between convert(nvarchar(10),@Cdt,120) 
        /// and convert(nvarchar(10),@Edt,120)  and a.PO_NUM=b.PO_NUM and a.INTL_CARRIER = @carrier
        /// </summary>
        /// <param name="region"></param>
        /// <param name="cdt"></param>
        /// <param name="edt"></param>
        /// <param name="intlCarrier"></param>
        /// <returns></returns>
        IList<string> GetInternalIdsFromVPakComn(string region, DateTime cdt, DateTime edt, string intlCarrier);

        /// <summary>
        /// Doc_Type list : select distinct DOC_CAT from [PAK.PAKRT] where DOC_CAT like 'Pack List%' AND DOC_CAT <>'Pack List- Transportation' order by DOC_CAT  desc
        /// </summary>
        /// <param name="docCatLike"></param>
        /// <param name="docCatNQ">Not Equal</param>
        /// <returns></returns>
        IList<string> GetDocCatFromPakDotParRt(string docCatLike, string docCatNQ);

        /// <summary>
        /// Region list : select distinct REGION from v_PAKComn nolock
        /// </summary>
        /// <returns></returns>
        IList<string> GetRegionListFromVPakComn();

        /// <summary>
        /// select @PO_NUM = PO_NUM, @deliveryno = InternalID,@shipment = rtrim(CONSOL_INVOICE),@model = Model,@region = REGION,@sales_chan = SALES_CHAN,@order_type = ORDER_TYPE,@intl_carrier = INTL_CARRIER 
        /// from dbo.v_Shipment_PAKComn nolock where InternalID=@DN
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<VShipmentPakComnInfo> GetVShipmentPakComnListByInternalID(string internalId);

        /// <summary>
        /// exists(select * from [PAK.PAKEdi850raw] where PO_NUM = @PO_NUM)
        /// </summary>
        /// <param name="poNum"></param>
        /// <returns></returns>
        bool CheckExistPakDotPakEdi850RawByPoNum(string poNum);

        /// <summary>
        /// select @instr = INSTR_FLAG from [PAK_PAKComn] nolock where InternalID=@DN
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<string> GetInstrFlagListFromPakDotPakComn(string internalId);

        /// <summary>
        /// insert into PAK_SkuMasterWeight_FIS values(@model,@weight,getdate())
        /// insert into PAK_SkuMasterWeight_FIS values(@model,@weight,getdate())
        /// INSERT INTO HP_EDI.dbo.PAK_SkuMasterWeight_FIS VALUES(RTRIM(@model),CONVERT(decimal(10,2),@weight),GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void InsertPakSkuMasterWeightFis(PakSkuMasterWeightFisInfo item);

        /// <summary>
        /// 其中
        /// @weight = convert(decimal(10,3), sum(Weight)/count(*)) from PAK_SkuMasterWeight_FIS where Left(Model,4) = left(@model,4)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        decimal GetAverageWeightFromPakSkuMasterWeightFis(string model);

        /// <summary>
        /// select @weight = Weight from PAK_SkuMasterWeight_FIS where Model = @model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<decimal> GetWeightFromPakSkuMasterWeightFis(string model);

        /// <summary>
        /// select @qty = PACK_ID_LINE_ITEM_BOX_QTY from v_Shipment_PAKComn where InternalID = @deliveryno
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<decimal> GetPackIdLineItemBoxQtyFromPakShipmentWeightFis(string internalId);

        /// <summary>
        /// insert into PAK_ShipmentWeight_FIS values(@DN,'D',convert(decimal(10,3),@qty)*@weight,getdate())
        /// insert into PAK_ShipmentWeight_FIS values(@shipment,'S',convert(decimal(10,3),@qty)*@weight,getdate())
        /// </summary>
        /// <param name="item"></param>
        void InsertPakShipmentWeightFis(PakShipmentWeightFisInfo item);

        /// <summary>
        /// 刷入的是Shipment
        /// exists(select * from dbo.PAK_ShipmentWeight_FIS where Shipment = @DN)
        /// exists(select * from PAK_ShipmentWeight_FIS where Shipment = @shipment)
        /// exists(select * from PAK_ShipmentWeight_FIS where Shipment = left(@DN,10))
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        bool CheckExistPakShipmentWeightFis(string shipment);

        /// <summary>
        /// select @qty = sum(PACK_ID_LINE_ITEM_BOX_QTY) from [v_Shipment_PAKComn] where CONSOL_INVOICE = @shipment
        /// </summary>
        /// <param name="consolInvoice"></param>
        /// <returns></returns>
        decimal GetSumOfPackIdLineItemBoxQtyFromVShipmentPakComn(string consolInvoice);

        /// <summary>
        /// insert DN_PrintList(@dn,@editor, Getdate())
        /// </summary>
        /// <param name="item"></param>
        void InsertDnPrintList(DnPrintListInfo item);

        /// <summary>
        /// 4. 判断机器是否为BT，并更新相关表格(参考Appendix 2.1)
        /// 1)Update   MP_BTOrder  and 
        /// </summary>
        /// <param name="item"></param>
        void UpdateMpBtOrder(MpBtOrderInfo item);

        /// <summary>
        /// Insert  FA_SnoBTDet
        /// </summary>
        /// <param name="item"></param>
        void InsertFaSnobtdet(FaSnobtdetInfo item);

        /// <summary>
        /// B.刷入的是Shipment
        /// exists (select * from [v_Shipment_PAKComn] where REGION='EMEA' and (CONSOL_INVOICE = @DN or SHIPMENT = @DN))
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        bool CheckExistVShipmentPakComnByConsolInvoiceOrShipmentAndRegion(string dn, string region);

        /// <summary>
        /// A. 刷入的是DN
        /// exists (select * from [v_Shipment_PAKComn] where REGION='EMEA' and left(InternalID,10)=@DN)
        /// </summary>
        /// <param name="internalId"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        bool CheckExistVShipmentPakComnByInternalIDAndRegion(string internalId, string region);

        /// <summary>
        /// A. 刷入的是DN
        /// select @CONSOL = CONSOL_INVOICE from v_Shipment_PAKComn nolock where left(InternalID,10) = @DN
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<string> GetConsolInvoiceFromVShipmentPakComn(string internalId);

        /// <summary>
        /// select InternalID into #2 from v_Shipment_PAKComn nolock where CONSOL_INVOICE = @CONSOL
        /// </summary>
        /// <param name="consolInvoice"></param>
        /// <returns></returns>
        IList<string> GetInternalIdsFromVShipmentPakComnByConsolInvoice(string consolInvoice);

        /// <summary>
        /// select @qty = sum(PACK_ID_LINE_ITEM_BOX_QTY) from  v_Shipment_PAKComn  where InternalID in (select * from #2)
        /// select @qty = sum(PACK_ID_LINE_ITEM_BOX_QTY) from [v_Shipment_PAKComn] where InternalID in (select * from #1)
        /// </summary>
        /// <param name="internalIds"></param>
        /// <returns></returns>
        decimal GetSumOfPackIdLineItemBoxQtyFromVShipmentPakComn(IList<string> internalIds);

        /// <summary>
        /// select @cqty = count(*) from dbo.PAK_PackkingData where InternalID in (select * from #2)
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        int GetCountOfPakPackkingData(IList<string> internalIds);

        /// <summary>
        /// select @cqty = count(distinct a.SERIAL_NUM) from dbo.PAK_PackkingData a,#1 b where a.InternalID = b.InternalID
        /// </summary>
        /// <param name="internalIds"></param>
        /// <returns></returns>
        int GetCountOfPakPackkingDataWithDistinctSerialNum(IList<string> internalIds);

        /// <summary>
        /// if exists(select * from PAK..FISTOSAP_WEIGHT where [DN/Shipment] = @DN)
        /// </summary>
        /// <param name="dnDivShipment"></param>
        /// <returns></returns>
        bool CheckExistFisToSpaWeight(string dnDivShipment);

        /// <summary>
        /// insert into PAK_ShipmentWeight_FIS 
        /// select [DN/Shipment],Status,convert(decimal(10,1),KG),Cdt from PAK..FISTOSAP_WEIGHT where [DN/Shipment] = @DN
        /// </summary>
        /// <param name="dnDivShipment"></param>
        /// <returns></returns>
        void CopyFisToSapWeightToPakShipmentWeightFis(string dnDivShipment);

        /// <summary>
        /// 依据刷入不同的code判断方式不同：
        /// A. 刷入的是DN
        /// select @deliveryno = InternalID from [PAK_PAKComn] nolock where left(InternalID,10)=@DN
        /// </summary>
        /// <param name="internalIdLike"></param>
        /// <returns></returns>
        IList<string> GetInternalIdsFromPakDashPakComnByLikeInternalID(string internalIdLike);

        /// <summary>
        /// 若在PAK_SkuMasterWeight_FIS能找到model前4位一样的数据(left(PAK_SkuMasterWeight_FIS.Model,4)=left(@model,4))时
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<PakSkuMasterWeightFisInfo> GetPakSkuMasterWeightFisListByLikeModel(string model);

        /// <summary>
        /// insert  into  Packinglist_RePrint values (@DN--刷入的code，ACTUAL_SHIPDATE, @model)
        /// </summary>
        /// <param name="item"></param>
        void InsertPackinglistRePrint(PackinglistRePrintInfo item);

        /// <summary>
        /// @weight = convert(decimal(10,3), sum(Weight)/count(*)) from PAK_SkuMasterWeight_FIS where Left(Model,4) = left(@model,4) and substring(Model,10,2)<>'25'
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        decimal GetAverageWeightFromPakSkuMasterWeightFisByModel(string model);

        /// <summary>
        /// exists (select DN from DN_PrintList where DN=@DN)
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        bool CheckExistDnPrintList(string dn);

        /// <summary>
        /// EXISTS (SELECT DN FROM HPEDI..DN_PrintList WHERE DN=@DN AND DOC_CAT = @Doc_Type)
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="docCat"></param>
        /// <returns></returns>
        bool CheckExistDnPrintList(string dn, string docCat);

        /// <summary>
        /// model在PAK_SkuMasterWeight_FIS里找到记录的
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool CheckExistModelInPakSkuMasterWeightFis(string model);

        /// <summary>
        /// 在Pizza_Part中得到与pizza绑定的part，只获取Pizza_Part.Value是14位且第一位为6的parts
        /// </summary>
        /// <param name="valueLength"></param>
        /// <param name="valuePrefix"></param>
        /// <returns></returns>
        IList<PizzaPart> GetPizzaPartsByValueLike(int valueLength, string valuePrefix);

        /// <summary>
        /// 在OlymBattery根据HPPN=Pizza_Part.Value的前5位 得到HSSN
        /// </summary>
        /// <param name="hppn"></param>
        /// <returns></returns>
        IList<OlymBatteryInfo> GetOlymBatteryListByHppn(string hppn);

        /// <summary>
        /// 在ACAdapMaintain根据ASSEMB=Pizza_Part.Value的前5位 得到Vendor和RMN等信息
        /// </summary>
        /// <param name="assemb"></param>
        /// <returns></returns>
        IList<ACAdapMaintainInfo> GetACAdapMaintainListByAssemb(string assemb);

        /// <summary>
        /// "按照以下条件进行查询：
        /// left(v_PAKComn.InternalID,10) = @Data or CONSOL_INVOICE = @Data or WAYBILL_NUMBER = @Data
        /// 显示以下字段：
        /// v_PAKComn. InternalID
        /// v_PAKComn.CONSOL_INVOICE 
        /// v_PAKComn.WAYBILL_NUMBER 
        /// v_PAKComn.INTL_CARRIER  "	
        ///
        /// "接口要求：
        /// 入参：string data；
        /// 实现功能：查询DN/Shipment/WaybillNo为输入数据的记录，返回UC所需的四个字段，并按第一个字段（v_PAKComn.InternalID即DeliveryNo）排序"
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<VPakComnInfo> GetVPakComnListByInternalIdOrConsolInvoiceOrWaybillNumber(string internalId, out int totalLength);

        /// <summary>
        /// delete [PAK_PAKComn] where InternalID = @DN
        /// </summary>
        /// <param name="internalId"></param>
        void DeletePakDashPakComn(string internalId);

        /// <summary>
        /// delete [PAK.PAKComn]  where InternalID = @DN
        /// </summary>
        /// <param name="internalId"></param>
        void DeletePakDotPakComn(string internalId);

        /// <summary>
        /// delete [PAK.PAKPaltno] where InternalID = @DN
        /// </summary>
        /// <param name="internalId"></param>
        void DeletePakDotPakPaltno(string internalId);

        /// <summary>
        /// DELETE HP_EDI.dbo.PAK_SkuMasterWeight_FIS WHERE Model = RTRIM(@model)
        /// </summary>
        /// <param name="model"></param>
        void DeletetPakSkuMasterWeightFisByModel(string model);

        /// <summary>
        /// 16.INSERT PAK_PQCLog VALUES (@snoid,@model,'PAK','81',GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void InsertPakPqcLog(PakPqclogInfo item);

        /// <summary>
        /// if exists (select BT from MP_BTOrder (nolock) where Pno = @pno and Qty-PrtQty >= @minDiff )
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="minDiff"></param>
        /// <returns></returns>
        bool CheckExistMpBtOrder(string pno, int minDiff);

        /// <summary>
        /// select @bt=BT,@btqty=Qty-PrtQty from MP_BTOrder (nolock) where Pno=@model and Qty-PrtQty>=@minDiff order by REF_DATE desc,ShipDate desc
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="minDiff"></param>
        /// <returns></returns>
        IList<MpBtOrderInfo> GetMpBtOrderInfoList(string pno, int minDiff);

        /// <summary>
        /// insert into FA_SnoBTDet Values (@btsnoid,@bt,@btrm,@user,getdate(),getdate() )
        /// </summary>
        /// <param name="item"></param>
        void InsertMpBtOrder(MpBtOrderInfo item);

        /// <summary>
        /// update MP_BTOrder set PrtQty=PrtQty+@count,Udt=getdate() where BT=@bt and Pno=@pno
        /// </summary>
        /// <param name="bt"></param>
        /// <param name="pno"></param>
        /// <param name="count"></param>
        void UpdateForIncreasePrtQty(string bt, string pno, int count);

        /// <summary>
        /// if exists (select BT from MP_BTOrder (nolock) where Pno=@pno and Qty-PrtQty < @maxdiff and Qty-PrtQty > @minDiff)
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="minDiff"></param>
        /// <param name="maxdiff"></param>
        /// <returns></returns>
        bool CheckExistMpBtOrder(string pno, int minDiff, int maxdiff);

        /// <summary>
        /// select count(BT) from MP_BTOrder (nolock) where Pno=@pno and Qty-PrtQty < @maxdiff  and Qty-PrtQty > @minDiff
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="minDiff"></param>
        /// <param name="maxdiff"></param>
        /// <returns></returns>
        int GetCountofMpBtOrder(string pno, int minDiff, int maxdiff);

        /// <summary>
        /// select @bt=BT,@btqty=Qty-PrtQty from MP_BTOrder (nolock) where Pno=@pno and Qty-PrtQty < @maxDiff and Qty-PrtQty > minDiff order by REF_DATE desc,ShipDate desc
        /// </summary>
        /// <param name="pno"></param>
        /// <param name="minDiff"></param>
        /// <param name="maxdiff"></param>
        /// <returns></returns>
        IList<MpBtOrderInfo> GetMpBtOrderInfoList(string pno, int minDiff, int maxdiff);

        /// <summary>
        /// 在PAK.PAKComn中增加记录
        /// </summary>
        /// <param name="item"></param>
        void AddPakDotPakComn(PakDotPakComnInfo item);

        /// <summary>
        /// 在PAK_PAKComn中增加记录
        /// </summary>
        /// <param name="item"></param>
        void AddPakDashPakComn(PakDashPakComnInfo item);

        /// <summary>
        /// 查询PAK.PAKComn中是否存在指定InternalID的记录
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        bool CheckExistPakDotPakComn(string internalId);

        /// <summary>
        /// 查询PAK_PAKComn中是否存在指定InternalID的记录
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        bool CheckExistPakDashPakComn(string internalId);

        /// <summary>
        /// (2).查询PAK.PAKPaltno中是否存在指定InternalID的记录[入参string id，返回值bool]
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        bool CheckExistPakDotPakPaltno(string internalId);

        /// <summary>
        /// (3).在PAK.PAKPaltno中增加记录[入参为新增的DataModel型，返回值void]
        /// </summary>
        /// <param name="item"></param>
        void AddPakDotPakPaltnoInfo(PakDotPakPaltnoInfo item);

#region not implement 210003
        /// <summary>
        /// 检索与pizza_id相对应的PizzaStatus。
        /// </summary>
        /// <param name="pizza_id"></param>
        /// <returns></returns>
        PizzaStatus GetPizzaStatus(string pizza_id);
#endregion

        /// <summary>
        /// SELECT DISTINCT LEFT(PartSn,5) 
        /// FROM IMES_PAK..Pizza_Part
        /// WHERE PizzaID = @pizzaId
        /// AND (PartSn LIKE '5%' OR PartSn LIKE 'W%') AND LEN(RTRIM(PartSn))=14
        /// </summary>
        /// <param name="partSnPrefixes"></param>
        /// <param name="lenOfPartSn"></param>
        /// <returns></returns>
        IList<string> GetPartSnPrefixesFromPizzaPart(string[] partSnPrefixes, int lenOfPartSn);

        /// <summary>
        /// SELECT DISTINCT b.*,a.AGENCY,a.SUPPLIER,a.VOLTAGE,a.CUR INTO #2 FROM dbo.ACAdapMaintain a,#ADP b WHERE a.ASSEMB=b.ADP
        /// </summary>
        /// <param name="assemb"></param>
        /// <returns></returns>
        IList<ACAdapMaintainInfo> GetACAdapMaintainList(string[] assemb);

        /// <summary>
        /// SELECT @indiaminid=min(ID),@indiamaxid=max(ID) FROM HP_EDI.dbo.PAKEDI_INSTR NOLOCK 
        /// WHERE PO_NUM=@PO_NUM and FIELDS='KEY_DETAIL_INDIA_SCREEN_DIM' and PO_ITEM=@item
        /// </summary>
        /// <param name="poNum"></param>
        /// <param name="fields"></param>
        /// <param name="poItem"></param>
        /// <returns>1st:max; 2nd:min</returns>
        int[] GetMinAndMaxIdOfPakEdiInstr(string poNum, string fields, string poItem);

        /// <summary>
        /// SELECT @indiascreendetail=isnull([VALUE],'') FROM HP_EDI.dbo.PAKEDI_INSTR NOLOCK WHERE PO_NUM=@PO_NUM and ID=@indiaid
        /// </summary>
        /// <param name="poNum"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<PakediInstrInfo> GetPakediInstrInfoListByPoNumAndId(string poNum, int id);

        /// <summary>
        /// EXISTS(SELECT * FROM HP_EDI.dbo.PAKEDI_INSTR NOLOCK WHERE PO_NUM=@PO_NUM and ID=@indiaid and charindex('1 N',[VALUE])>0)
        /// </summary>
        /// <param name="poNum"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckExistPakediInstrByPoNumAndIdAndValue(string poNum, int id);

        /// <summary>
        /// SELECT @config=isnull(C48,'') , @po=PO_NUM,@COUNTRY=isnull(UPPER(SHIP_TO_COUNTRY_NAME),'') 
        /// FROM HP_EDI..PAK_PAKComn (NOLOCK) WHERE InternalID=@dn and ORDER_TYPE='CTO' and REGION in ('SAF','SCN')
        /// </summary>
        /// <param name="internalId"></param>
        /// <param name="orderType"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
        IList<PakDashPakComnInfo> GetPakDashPakComnListByInternalIDAndOrderTypeAndRegions(string internalId, string orderType, string[] regions);

        /// <summary>
        /// SELECT DISTINCT @base=PROD_DESC_BASE FROM HP_EDI..[PAK.PAKEdi850raw] (NOLOCK) WHERE PO_NUM=@po
        /// </summary>
        /// <param name="poNum"></param>
        /// <returns></returns>
        IList<string> GetProdDescBaseFromPakDotPakEdi850raw(string poNum);

        /// <summary>
        /// 在Pizza_Part中得到与pizza绑定的part，只获取Pizza_Part.Value是14位且第一位为6的parts
        /// 
        /// 14. 基于Pizza ID 取结合Product 的Battery，取结合Product的Battery 数量保存到变量@BatQty
        /// IMES_PAK..Pizza_Part 表中PartSn 第一码为'6'，长度为14 的记录为Battery
        /// </summary>
        /// <param name="valueLength"></param>
        /// <param name="valuePrefix"></param>
        /// <param name="pizzaId"></param>
        /// <returns></returns>
        IList<PizzaPart> GetPizzaPartsByValueLike(int valueLength, string valuePrefix, string pizzaId);

        /// <summary>
        /// SELECT @setnum=DOC_SET_NUMBER FROM HP_EDI..[PAK.PAKComn] (NOLOCK) WHERE InternalID=@shipment 
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<string> GetDocSetNumListFromPakDashPakComnByInternalID(string internalId);

        /// <summary>
        /// EXISTS (SELECT * FROM HP_EDI..MEID_Log WHERE PALLET_ID=rtrim(@plt) AND StringIDValue=@imei)
        /// </summary>
        /// <param name="palletId"></param>
        /// <param name="stringIdValue"></param>
        /// <returns></returns>
        bool CheckExistMeidLogByPltAndStringIdValue(string palletId, string stringIdValue);

        /// <summary>
        /// EXISTS (SELECT * FROM HP_EDI..MEID_Log WHERE PALLET_ID=rtrim(@plt) AND IsPass='0')
        /// </summary>
        /// <param name="palletId"></param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        bool CheckExistMeidLogByPltAndIsPass(string palletId, short isPass);

        /// <summary>
        /// UPDATE HP_EDI..MEID_Log SET IsPass='1' WHERE PALLET_ID=rtrim(@plt) AND StringIDValue=@imei
        /// </summary>
        /// <param name="isPass"></param>
        /// <param name="palletId"></param>
        /// <param name="stringIdValue"></param>
        void UpdateMeidLogIsPassByPalletIdAndStringIDValue(short isPass, string palletId, string stringIdValue);

        /// <summary>
        /// EXISTS (SELECT SERIAL_NUM FROM HP_EDI.dbo.PAK_PackkingData WHERE SERIAL_NUM = @sno)
        /// </summary>
        /// <param name="serialNum"></param>
        /// <returns></returns>
        bool CheckExistPakPackkingDataBySerialNum(string serialNum);

        /// <summary>
        /// INSERT SAP_WEIGHT 
        ///  SELECT RTRIM(@consolidated),'S',sum(KG) FROM #WT
        /// INSERT SAP_WEIGHT 
        ///  SELECT LEFT(@dn,10),'D',sum(KG) FROM #WT 
        /// INSERT SAP_WEIGHT 
        ///  SELECT LEFT(@dn,10),'D',sum(KG) FROM #WT 
        /// </summary>
        /// <param name="item"></param>
        void InsertSapWeight(SapWeightInfo item);

        /// <summary>
        /// SELECT @dockingSW = convert(decimal(5,4), Descr) FROM [10.99.183.29].IES1.dbo.LocalMaintain 
        /// WHERE Tp='SW' and Code=@dockingModel
        /// 
        /// 这个表与宋杰确认过修改为ModelWeight了，在DB中已存在
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<string> GetDescrsFromLocalMaintainByTpAndCode(string tp, string code);
 
        /// <summary>
        /// UPDATE SAP_WEIGHT SET KG=KG+@dockingSW*@dockingQty
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="addingAmount"></param>
        /// <param name="condition"></param>
        void UpdateSapWeight(Decimal addingAmount, SapWeightInfo condition);

        /// <summary>
        /// DELETE HP_EDI.dbo.PAK_ShipmentWeight_FIS WHERE Shipment in (SELECT [DN/Shipment] FROM SAP_WEIGHT)
        /// </summary>
        void DeletePakShipmentWeightFisWithShipmentInSapWeight();

        /// <summary>
        /// Update MP_BTOrder
        /// setValue哪个字段赋值就有更新哪个字段
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.  
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateMpBtOrder(MpBtOrderInfo setValue, MpBtOrderInfo condition);

        /// <summary>
        /// IF EXISTS (SELECT 1  FROM [FA].[dbo].[MP_BTOrder] where [Pno]=model# and Qty>PrtQty)
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        bool CheckExistMpBtOrder(string pno);

        /// <summary>
        /// update MP_BTOrder set PrtQty=PrtQty+@count,Udt=getdate() where BT=@bt and ID=@id
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="count"></param>
        void UpdateForIncreasePrtQty(MpBtOrderInfo condition, int count);

        /// <summary>
        /// select @orderqty=qty,@prtqty= PrtQty ,@BT=BT from MP_BTOrder (nolock) where Pno=@model order by REF_DATE desc,ShipDate desc
        /// </summary>
        /// <param name="pno"></param>
        /// <returns></returns>
        IList<MpBtOrderInfo> GetMpBtOrderInfoList(string pno);

        /// <summary>
        /// 备份PizzaStatus，要备份到UnpacPizzaStatus 表
        /// </summary>
        /// <param name="pizzaId"></param>
        /// <param name="uEditor"></param>
        void BackUpPizzaStatus(string pizzaId, string uEditor);

        /// <summary>
        /// Call SP: op_TemplateCheck_LANEW
        /// @DN nvarchar(10),@doctpye nvarchar(100) 
        /// </summary>
        /// <returns></returns>
        DataTable CallTemplateCheckLaNew(string dn, string docType);

        /// <summary>
        /// SELECT a.PartSn 。。 
        ///    FROM Pizza_Part a (nolock), Part b (nolock), PartInfo c (nolock)
        ///    WHERE  a.PizzaID in (@firstPizzaID, @secondPizzaID)
        ///        AND a.PartNo = b.PartNo
        ///        AND a.PartNo = c.PartNo
        ///        AND b.BomNodeType = 'P1'
        ///        AND c.InfoType = 'DESC'
        ///        AND c.InfoValue = 'OOA'
        /// </summary>
        /// <param name="pizzaIds"></param>
        /// <param name="bomNodeType"></param>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        IList<string> GetPartNoListFromPizzaPart(string[] pizzaIds, string bomNodeType, string infoType, string infoValue);

        /// <summary>
        /// DELETE FROM PAK_PAKComn WHERE left(InternalID, 10) in (<prefixList中各元素>)
        /// </summary>
        /// <param name="prefixList"></param>
        void DeletePakDashPakComn(IList<string> prefixList);

        /// <summary>
        /// DELETE FROM [PAK.PAKComn] WHERE left(InternalID, 10) in (<prefixList中各元素>)
        /// </summary>
        /// <param name="prefixList"></param>
        void DeletePakDotPakComn(IList<string> prefixList);

        /// <summary>
        /// DELETE FROM [PAK.PAKPaltno] WHERE left(InternalID, 10) in (<prefixList中各元素>)
        /// </summary>
        /// <param name="prefixList"></param>
        void DeletePakDotPakPaltno(IList<string> prefixList);

        /// <summary>
        /// delete Pizza_Part where PartSn=@coano and PizzaID=@PizzaId
        /// </summary>
        /// <param name="condition"></param>
        void DeletePizzaPart(PizzaPart condition);

        #region . Defered .

        void InsertForBackupPakPackkingDataDefered(IUnitOfWork uow, IList<string> internalIds);

        void DeletePakPackkingDataDefered(IUnitOfWork uow, IList<string> internalIds);

        void InsertPakPackkingDataDefered(IUnitOfWork uow, IList<PakPackkingDataInfo> items);

        void InsertPakSkuMasterWeightFisDefered(IUnitOfWork uow, PakSkuMasterWeightFisInfo item);

        void InsertPakShipmentWeightFisDefered(IUnitOfWork uow, PakShipmentWeightFisInfo item);

        void InsertDnPrintListDefered(IUnitOfWork uow, DnPrintListInfo item);

        void UpdateMpBtOrderDefered(IUnitOfWork uow, MpBtOrderInfo item);

        void InsertFaSnobtdetDefered(IUnitOfWork uow, FaSnobtdetInfo item);

        void CopyFisToSapWeightToPakShipmentWeightFisDefered(IUnitOfWork uow, string dnDivShipment);

        void InsertPackinglistRePrintDefered(IUnitOfWork uow, PackinglistRePrintInfo item);

        void DeletePakDashPakComnDefered(IUnitOfWork uow, string internalId);

        void DeletePakDotPakComnDefered(IUnitOfWork uow, string internalId);

        void DeletePakDotPakPaltnoDefered(IUnitOfWork uow, string internalId);

        void DeletetPakSkuMasterWeightFisByModelDefered(IUnitOfWork uow, string model);

        void InsertPakPqcLogDefered(IUnitOfWork uow, PakPqclogInfo item);

        void InsertMpBtOrderDefered(IUnitOfWork uow, MpBtOrderInfo item);

        void UpdateForIncreasePrtQtyDefered(IUnitOfWork uow, string bt, string pno, int count);

        void AddPakDotPakComnDefered(IUnitOfWork uow, PakDotPakComnInfo item);

        void AddPakDashPakComnDefered(IUnitOfWork uow, PakDashPakComnInfo item);

        void AddPakDotPakPaltnoInfoDefered(IUnitOfWork uow, PakDotPakPaltnoInfo item);

        void UpdateMeidLogIsPassByPalletIdAndStringIDValueDefered(IUnitOfWork uow, short isPass, string palletId, string stringIdValue);

        void InsertSapWeightDefered(IUnitOfWork uow, SapWeightInfo item);

        void UpdateSapWeightDefered(IUnitOfWork uow, Decimal addingAmount, SapWeightInfo condition);

        void DeletePakShipmentWeightFisWithShipmentInSapWeightDefered(IUnitOfWork uow);

        void UpdateMpBtOrderDefered(IUnitOfWork uow, MpBtOrderInfo setValue, MpBtOrderInfo condition);

        void UpdateForIncreasePrtQtyDefered(IUnitOfWork uow, int id, int count);

        void BackUpPizzaStatusDefered(IUnitOfWork uow, string pizzaId, string uEditor);

        void DeletePakDashPakComnDefered(IUnitOfWork uow, IList<string> prefixList);

        void DeletePakDotPakComnDefered(IUnitOfWork uow, IList<string> prefixList);

        void DeletePakDotPakPaltnoDefered(IUnitOfWork uow, IList<string> prefixList);

        void DeletePizzaPartDefered(IUnitOfWork uow, PizzaPart condition);

        #endregion

        #region for PizzaLog
        /// <summary>
        /// 获取Pizaa对象Pizza Log
        /// </summary>
        /// <param name="currentPizza"></param>
        /// <returns></returns>
        Pizza FillPizzaLogs(Pizza currentPizza);
        
        /// <summary>
        /// select distinct PartNo from Pizza_Part where PizzaID=@PizzaID
        /// </summary>
        /// <param name="pizzaId"></param>
        /// <returns></returns>
        IList<string> GetCombinedPartNo(string pizzaId);

        /// <summary>
        /// select count(1)  from Pizza_Part where PizzaID=@PizzaID
        /// </summary>
        /// <param name="pizzaId"></param>
        /// <returns></returns>
        int GetCombinedPartQty(string pizzaId);

        /// <summary>
        /// select PartNo, count(1) as qty from Pizza_Part where PizzaID=@PizzaID group by PartNo
        /// </summary>
        /// <param name="pizzaId"></param>
        /// <returns></returns>
        IList<PizzaPartNoQtyInfo> GetCombinePartNoQty(string pizzaId);

        /// <summary>
        /// select * from Pizza where CartonSN=@CartonSN
        /// </summary>
        /// <param name="cartonSn"></param>
        /// <returns></returns>
        IList<Pizza> GetCombinePizzaByCartonSN(string cartonSn);

        /// <summary>
        ///  select count(CartonSN)  from Pizza where CartonSN=@CartonSN
        /// </summary>
        /// <param name="cartonSn"></param>
        /// <returns></returns>
        int GetCombinePizzaQtyByCartonSN(string cartonSn);
        #endregion

        #region backup Pizza_Part
        void BackUpPizzaPart(string pizzaId, string editor);
        void BackUpPizzaPartByPartType(string pizzaId, string partType,string editor);
        void BackUpPizzaPartByPartSn(string pizzaId, string partSn, string editor);
        void BackUpPizzaPartDefered(IUnitOfWork uow, string pizzaId, string editor);
        void BackUpPizzaPartByPartTypeDefered(IUnitOfWork uow, string pizzaId, string partType, string editor);
        void BackUpPizzaPartByPartSnDefered(IUnitOfWork uow, string pizzaId, string partSn, string editor);
        #endregion

        #region others function

        IList<PizzaPart> GetPizzaPart(PizzaPart condition);
        void UpdatePizzaPart(PizzaPart item);
        void BackUpPizzaPartById(int id, string editor);
        void UpdatePizzaPartDefered(IUnitOfWork uow, PizzaPart item);
        void BackUpPizzaPartByIdDefered(IUnitOfWork uow, int id, string editor);

        #endregion
    }
}