// INVENTEC corporation (c)2009 all rights reserved. 
// Description: Delivery对象Repository接口
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-04   Yuan XiaoWei                 create
// Known issues:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectRepositoryFramework;
//
using IMES.DataModel;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure.Util;

namespace IMES.FisObject.PAK.DN
{
    /// <summary>
    /// Delivery对象Repository接口
    /// </summary>
    public interface IDeliveryRepository : IRepository<Delivery>
    {
        #region . For CommonIntf  .

        string GetPAKConnectionString();

        string GetDataConnectionString();

        /// <summary>
        /// 获取符合出货条件的BolNo列表
        /// Status为00
        /// Delivery表中Shipdate大于等于当前日期减五天
        /// </summary>
        /// <returns>符合出货条件的BOL码集合</returns>
        IList<BOLNoInfo> GetBolNo();

        /// <summary>
        /// 根据BolNo获取DN
        /// 且DN对应的Model是出整機的（看Model表ShipType栏位是PC）
        /// Status为00
        /// Delivery表中Shipdate大于等于当前日期减五天
        /// </summary>
        /// <param name="bolNo">BOL码</param>
        /// <returns>DN集合</returns>
        IList<string> GetDNByBolNo(string bolNo);

        /// <summary>
        /// 获取可以做Pallet Data Collection TRO的DeliveryNo列表
        /// Delivery表中Shipdate大于等于当前日期减五天,
        /// 且Model的Region是TRO,
        /// ShipType是整机
        /// Status为00或82
        /// </summary>
        /// <returns></returns>
        IList<string> GetDeliveryNoListFor054();

        /// <summary>
        /// 获取可以做CombingPOInCarton的DeliveryNo列表
        /// 获取Model与输入Model相同，
        /// ShipDate大于等于当前日期减五天，
        /// 状态为可以结合的(00)DN列表。
        /// </summary>
        /// <returns></returns>
        IList<string> GetDeliveryNoListFor053(string Model);
        #endregion

        /// <summary>
        /// 根据DN获取其绑定的Pallet列表
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        Delivery FillDnPalletList(Delivery delivery);

        //void FillDeliveryAttributes(Delivery item);

        void FillDeliveryAttributes(Delivery delivery);

        Delivery FillDeliveryLogs(Delivery delivery);

        /// <summary>
        /// select distinct b.InfoValue from IMES_PAK..Delivery a,IMES_PAK..DeliveryInfo b where a.DeliveryNo = b.DeliveryNo and b.InfoType='RedShipment' and left(a.Model,2)='60' and a.ShipDate=’’order by b.InfoValue
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="modelPrefix"></param>
        /// <param name="shipDate"></param>
        /// <returns></returns>
        IList<ShipmentInfo> GetDeliveryInfoValueByTypeAndModelPrefix(string infoType, string modelPrefix, DateTime shipDate);

        #region For PODataManage,没有工作流，修改都要求及时生效

        /// <summary>
        /// 用户可以选择其中的任何一个或多个条件进行查询
        /// DN,PONo,Model都需要用Like查询
        /// order by DeliveryNo
        /// 
        /// 对于输入条件，dn需要判断长度，如果是16位则需要精确匹配，如果是10位，则模糊匹配（前10位）其他条件需要精确匹配
        /// 
        /// 但新的UC中增加了一个DN Info的查询条件，需要改造该接口或新增查询接口；
        /// 查询时还需增加一个条件Shipment；
        /// TOP 1000
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByCondition(DNQueryCondition MyCondition, out int totalLength);

        /// <summary>
        /// 能够显示出所查询DN的所有Qty之和
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <param name="totalLength"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByCondition(DNQueryCondition MyCondition, out int totalLength, out long sum);

        IList<DNForUI> GetDNListByCondition(DNQueryCondition MyCondition);

        /// <summary>
        /// Based on last one. 按照 DN的ShipDate, Qty, DeliveryNo顺序排序
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByConditionWithSorting(DNQueryCondition MyCondition);

        /// <summary>
        ///           IList<S_RowData_COAandDN> ret = new List<S_RowData_COAandDN>();
        ///           DNQueryCondition condition = new DNQueryCondition();
        ///           DateTime temp = DateTime.Now;
        ///           temp = temp.AddDays(-3);
        ///           condition.ShipDateFrom = new DateTime(temp.Year, temp.Month, temp.Day, 0,0,0,0);
        ///           IList<DNForUI> dnList = currentRepository.GetDNListByCondition(condition);
        ///           foreach (DNForUI tmp in dnList)
        ///           {
        ///               S_RowData_COAandDN ele = new S_RowData_COAandDN();
        ///               ele.DeliveryNO = tmp.DeliveryNo;
        ///               if (tmp.Status != "00")
        ///               {
        ///                   continue;
        ///               }
        ///               if (!(tmp.ModelName.Length == 12 && tmp.ModelName.Substring(0, 2) == "PC"))
        ///               {
        ///                   continue;
        ///               }
        ///               ele.Model = tmp.ModelName;
        ///               ele.CustomerPN = currentRepository.GetDeliveryInfoValue(tmp.DeliveryNo, "PartNo");
        ///               ele.PoNo = tmp.PoNo;
        ///               ele.Date = tmp.ShipDate.ToString("yyyy/MM/dd");
        ///               ele.Qty = tmp.Qty.ToString();
        ///               int qty = 0;
        ///               int packedQty = 0;
        ///               qty = tmp.Qty;
        ///               IList<IProduct> productList = new List<IProduct>();
        ///               productList = productRepository.GetProductListByDeliveryNo(tmp.DeliveryNo);
        ///               if (null != productList)
        ///               {
        ///                   ele.PackedQty = productList.Count.ToString();
        ///                   packedQty = productList.Count;
        ///               }
        ///               if (packedQty > qty )
        ///               {
        ///                   continue;
        ///               }
        ///               ret.Add(ele);
        ///           }
        ///           return ret;
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <returns></returns>
        IList<Srd4CoaAndDn> GetDNListByConditionForPerformance(DNQueryCondition MyCondition);

        /// <summary>
        /// Based on last one. 按照 DN的ShipDate, Qty, DeliveryNo顺序排序
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <returns></returns>
        IList<Srd4CoaAndDn> GetDNListByConditionForPerformanceWithSorting(DNQueryCondition MyCondition);

        /// <summary>
        /// 对于输入条件，dn需要判断长度，如果是16位则需要精确匹配，如果是10位，则模糊匹配（前10位）其他条件需要精确匹配
        /// 
        /// DNInfo精确匹配的方式是表中的Descr属性值like '%=输入值~%'
        /// 
        /// 参数类型：DNQueryCondition（含DNInfo的查询条件）
        /// TOP 1000
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByConditionFromPoDataEdi(DNQueryCondition MyCondition, out int totalLength);

        /// <summary>
        /// 上传DN信息
        /// </summary>
        /// <param name="uploadID"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        IList<DNForUI> UploadPOData(string uploadID, string editor);

        /// <summary>
        /// 根据DN获取DNInfoForUI列表
        /// Order by InfoType
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<DNInfoForUI> GetDNInfoList(string dn);

        /// <summary>
        /// 从IMES_PAK..PoPlt_EDI里获取数据 按照栈板号排序
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        PoDataEdiInfo GetPoDataEdiInfo(string deliveryNo);

        ///// <summary>
        ///// a.	取得Customer S/N 绑定的Delivery No (IMES_FA..Product.DeliveryNo)
        ///// b.	取得该Delivery No 的Consolidated 属性（IMES_PAK.DeliveryInfo）
        ///// </summary>
        ///// <param name="CustomerSN"></param>
        ///// <returns></returns>
        //string GetDeliveryInfoConsolidated(string CustomerSN);

        /// <summary>
        /// 使用ProductID = @ProductId and Tp = 'PAQC' 查询IMES_FA..QCStatus 表取Udt 最新的记录，
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        ProductQCStatus GetQCStatus(string productID, string tp);

        /// <summary>
        /// 根据DN获取Pallet列表
        /// Order By PalletNo
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<DNPalletQty> GetPalletList(string dn);

        /// <summary>
        /// 访问ConstValue 表，取得ConstValue.Type = 'ProcReg' 的记录，这些记录的Name 字段为SKU / Docking 等名称，Value 为正则表达式；使用Model 依次配置这些记录的正则表达式，匹配上的记录的Name 字段即可确定SKU / Docking...
        /// 
        /// --Get Non Bulk Pallet
        /// SELECT PLT AS PalletNo, COUNT(DISTINCT PLT) AS Qty 
        ///    INTO #Pallets
        ///    FROM #PickDeliveries a (nolock), DeliveryInfo b (nolock), Dummy_ShipDet c (nolock)
        ///    WHERE a.DeliveryNo = b.DeliveryNo
        ///        AND b.InfoType = 'BOL'
        ///        AND b.InfoValue = c.BOL
        ///    GROUP BY PLT
        ///
        /// --Get Bulk Pallet   
        /// INSERT #Pallets
        ///     SELECT b.PalletNo AS PalletNo, SUM(b.DeliveryQty) AS Qty
        ///         FROM #PickDeliveries a (nolock), Delivery_Pallet b (nolock)
        ///         WHERE a.DeliveryNo = b.DeliveryNo
        ///             AND LEFT(b.PalletNo, 2) IN ('00', '01')
        ///         GROUP BY b.PalletNo
        ///         
        /// --Get Docking Pallet   
        /// INSERT #Pallets
        ///     SELECT b.PalletNo AS PalletNo, SUM(b.DeliveryQty) AS Qty
        ///         FROM #PickDeliveries a (nolock), Delivery_Pallet b (nolock)
        ///         WHERE a.DeliveryNo = b.DeliveryNo
        ///             AND LEFT(b.PalletNo, 2) IN (‘00’,’01’,'NA')
        ///         GROUP BY b.PalletNo
        ///    
        ///  TODO 2: GetPalletList方法 对于返回的 DeliveryQty, 需要按如下描述实现
        ///  * 散装为使用PLT = @PalletNo 得到的记录数量
        ///  * @PalletQty – 非散装为使用PalletNo= @Pallet 查询Delivery_Pallet 表得到的SUM(Delivery_Pallet.DeliveryQty)
        /// 
        ///  TODO 1: GetPalletList方法 需要通过查询判断并分辨出究竟是哪种Delivery
        ///  * d. (散装的 – Delivery 结合的Pallets 都是NA开头的栈板号)根据Delivery的BOL (DeliveryInfo.InfoValue, Condition: DeliveryNo = @Delivery and InfoType = ‘BOL’)查询Dummy_ShipDet 表得到Pallets
        ///  * c. (非散装的 – Delivery 结合的Pallets 都是00 or 01 开头的栈板号)根据Delivery根据Delivery 查询Delivery_Pallet 表得到Pallets
        /// </summary>
        /// <param name="dnList"></param>
        /// <returns></returns>
        IList<DNPalletQty> GetPalletList2(string[] dnList);

        IList<DNPalletQty> GetPalletList2FromView(string[] dnList);
        /// <summary>
        /// 从IMES_PAK..PoPlt_EDI里获取数据 按照栈板号排序
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<DNPalletQty> GetPalletListFromPoPltEdi(string dn);

        /// <summary>
        /// 根据DNUpdateCondition中的DeliveryNo修改一个Delivery的几个属性
        /// 其中 ShipmentNo， PoNo， Model，ShipDate如果不是Null才需要更新
        /// 否则不更新
        /// 没有工作流，修改都要求及时生效
        /// </summary>
        /// <param name="myCondition"></param>
        /// <param name="editor"></param>
        void UpdateDNByCondition(DNUpdateCondition myCondition, string editor);

        /// <summary>
        /// 根据ID修改一个DeliverInfo的InfoValue
        /// 没有工作流，修改都要求及时生效
        /// </summary>
        /// <param name="deliverInfoID"></param>
        /// <param name="infoValue"></param>
        /// <param name="editor"></param>
        void UpdateDeliverInfoByID(int deliverInfoID, string infoValue, string editor);

        /// <summary>
        /// 根据ID修改一个DeliverPallet的DeliveryQty
        /// 没有工作流，修改都要求及时生效
        /// </summary>
        /// <param name="deliveryPalletID"></param>
        /// <param name="deliveryQty"></param>
        /// <param name="editor"></param>
        void UpdateDeliverQty(int deliveryPalletID, short deliveryQty, string editor);

        /// <summary>
        /// 调用SP: IMES_Update_POData
        /// </summary>
        /// <param name="uploadID"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        IList<DNForUI> UpdatePOData(string uploadID, string editor);

        #endregion

        /// <summary>
        /// Update Delivey set Status=@Status,Editor=@Editor,Udt=@Udt
        /// Where DeliceryNo=@DeliveryNo and not exists(select 1 from Delicery_Pallet where Status='0'and DeliceryNo=@DeliveryNo)
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="status"></param>
        /// <param name="editor"></param>
        void UpdateDNStatusWhenDNPalletFull(string deliveryNo, string status, string editor);

        /// <summary>
        /// 调用存储过程IMES_PAK.dbo.IMES_UpdateDN_PalletFull
        /// 当Pallet满时更新其上所有满的DN的状态
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="status"></param>
        /// <param name="editor"></param>
        void UpdateAllDNStatusWhenPalletFull(string deliveryNo, string palletno, string status, string editor);

        /// <summary>
        /// 获取PrintShiptoCartonLabel的DN列表
        /// 调用 IDeliveryRepository.getDeliveryNoListFor071(string Model)
        /// DN 按照Model（按照Carton No 查询IMES_FA..Product 得到）
        /// Status='00'
        /// Ship Date大于等于当天为条件进行查询
        /// 结果按照ShipDate, Delivery No 进行排序
        /// 
        /// select b.DeliveryNo from [IMES_FA].[dbo].[Product] a inner join [IMES_PAK].[dbo].[Delivery] b on a.Model=b.Model where b.Status='00' and b.ShipDate >= DATEADD (dd, DATEDIFF(dd,0,getdate()), 0)
        /// </summary>
        /// <returns></returns>
        IList<string> GetDeliveryNoListFor071(string Model);

        /// <summary>
        /// select B.PalletNo  
        /// from Delivery_Pallet A 
        /// inner join Delivery_Pallet B
        /// on A.PalletNo=B.PalletNo 
        /// and A.DeliveryNo=@dn 
        /// and A.Status = '0'
        /// group by B.PalletNo having count(B.DeliveryNo)>1
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<string> GetPalletNoListByDn(string dn);

        /// <summary>
        /// select B.PalletNo 
        /// from Delivery_Pallet A 
        /// inner join Delivery_Pallet B
        /// on A.PalletNo=B.PalletNo 
        /// and A.DeliveryNo=@dn 
        /// and A.Status = '0'
        /// and B.Status = '0'
        /// group by B.PalletNo having count(distinct(B.DeliveryNo))=1
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<string> GetPalletNoListByDnAndWithSoloDn(string dn);//ITC-1360-1446

        /// <summary>
        /// select B.PalletNo 
        /// from Delivery_Pallet A 
        /// inner join Delivery_Pallet B
        /// on A.PalletNo=B.PalletNo 
        /// and A.Shipment=@shipment 
        /// and A.Status = '0'
        /// and B.Status = '0'
        /// group by B.PalletNo having count(distinct(B.Shipment))=1
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        IList<string> GetPalletNoListByShipmentAndWithSoloShipment(string shipment);//ITC-1360-1446

        /// <summary>
        /// 1、Product.DeliveryNo 在DeliveryInfo分别查找InfoType= ‘CustPo’ / ‘IECSo’对应的InfoValue
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        string GetDeliveryInfoValue(string dn, string infoType);

        /// <summary>
        /// CI-MES12-SPEC-PAK-UC Unpack：(P9)请帮忙提供下述数据库功能:
        /// 更新IMES_PAK..ShipBoxDet.SnoId = ''
        /// SQL: UPDATE IMES_PAK..ShipBoxDet SET SnoId=@SnoId WHERE DeliveryNo=@DeliveryNo
        /// </summary>
        /// <param name="snoid"></param>
        /// <param name="dn"></param>
        void UpdateSnoidForShipBoxDet(string snoId, string dn);

        /// <summary>
        /// DELETE HP_EDI.dbo.PAK_PackkingData WHERE SERIAL_NUM IN @prodIds
        /// </summary>
        /// <param name="prodIds"></param>
        void RemovePAK_PackkingData_EDIDataByProdIds(string[] prodIds);

        /// <summary>
        /// DELETE HP_EDI.dbo.PAKODMSESSION WHERE SERIAL_NUM IN @prodIds
        /// </summary>
        /// <param name="prodIds"></param>
        void RemovePAKOdmSession_EDIDataByProdIds(string[] prodIds);

        /// <summary>
        /// SELECT @RegId = a.InfoValue FROM Delivery_Info a (nolock), Delivery_Pallet b (nolock)
        ///  WHERE b.PalletNo = @PalletNo
        ///   and a.DeliveryNo = b.DeliveryNo
        ///   and a.InfoType = 'RegId'
        /// SELECT @PalletQty = a.InfoValue FROM Delivery_Info a (nolock), Delivery_Pallet b (nolock)
        ///  WHERE b.PalletNo = @PalletNo
        ///   and a.DeliveryNo = b.DeliveryNo
        ///   and a.InfoType = 'PalletQty'
        /// SELECT @ShipWay = a.InfoValue FROM Delivery_Info a (nolock), Delivery_Pallet b (nolock)
        ///  WHERE b.PalletNo = @PalletNo
        ///   and a.DeliveryNo = b.DeliveryNo
        ///   and a.InfoType = 'ShipWay'
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        string GetCertainInfoValueForDeliveryByPalletNoOnDeliveryNo(string palletNo, string infoType);

        /// <summary>
        /// SELECT @RegId = a.InfoValue FROM Delivery_Info a (nolock), Delivery_Pallet b (nolock), Delivery c (nolock)
        /// WHERE b.PalletNo = @PalletNo and c.DeliveryNo = a.DeliveryNo
        ///   and b.ShipmentNo = c.ShipmentNo
        ///   and a.InfoType = 'RegId'
        /// SELECT @PalletQty = a.InfoValue FROM Delivery_Info a (nolock), Delivery_Pallet b (nolock), Delivery c (nolock)
        /// WHERE b.PalletNo = @PalletNo and c.DeliveryNo = a.DeliveryNo
        ///   and b.ShipmentNo = c.ShipmentNo
        ///   and a.InfoType = 'PalletQty'
        /// SELECT @ShipWay = a.InfoValue FROM Delivery_Info a (nolock), Delivery_Pallet b (nolock), Delivery c (nolock)
        /// WHERE b.PalletNo = @PalletNo and c.DeliveryNo = a.DeliveryNo
        ///   and b.ShipmentNo = c.ShipmentNo
        ///   and a.InfoType = 'ShipWay'
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        string GetCertainInfoValueForDeliveryByPalletNoOnShipmentNo(string palletNo, string infoType);

        /// <summary>
        /// SELECT @pqty = SUM(DeliveryQty) FROM Delivery_Pallet WHERE PalletNo = @PalletNo
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        int GetSumofDeliveryQtyFromDeliveryPallet(string palletNo);

        IList<Delivery> GetDeliveriesByShipDateRange(DateTime begin, DateTime end);

        /// <summary>
        /// not exists (select * from PAK_PackkingData nolock where InternalID in @dn)
        /// </summary>
        /// <param name="internalIds"></param>
        /// <returns></returns>
        bool CheckExistPakPackkingData(IList<string> internalIds);

        /// <summary>
        ///  Delivery.ShipDate 大于3天前 (例如：当天为2011-9-13，那么获取的DN 的ShipDate 要大于2011-9-10)
        ///  Delivery.Status = '00'
        ///  Delivery.Model 长度为12 位
        ///  Delivery.Model 前两码为'PC'
        ///  除满足上述条件的DN 外，还需要增加一条Virtual DN
        ///  按照DeliveryNo 升序排列，Virtual DN 插在记录集的首部
        /// </summary>
        /// <returns></returns>
        IList<Delivery> GetDNListForCombineCOAandDN();

        /// <summary>
        /// SELECT COUNT(DISTINCT LEFT(DeliveryNo,10)) AS Expr1
        /// FROM IMES_PAK.dbo.DeliveryInfo
        /// where InfoValue like @ConsolidateNo and InfoType = 'Consolidated'
        /// </summary>
        /// <param name="consolidateNo"></param>
        /// <returns></returns>
        int GetDistinctDeliveryNo(string consolidateNo);

        /// <summary>
        /// select sum(DeliveryQty) from Delivery_Pallet where DeliveryNo=@DeliveryNo
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        int GetSumDeliveryQtyOfACertainDN(string deliveryNo);

        void InsertDeliveryInfo(DeliveryInfo item);

        /// <summary>
        /// update DN的Udt
        /// </summary>
        /// <param name="dn"></param>
        void UpdateDNUdt(string dn);

        /// <summary>
        /// 从SnoCtrl_BoxId里获取满足条件的第一条记录：Cust= BoxId前缀 and valid=’1’，得到BoxId
        /// select TOP 1 * from SnoCtrl_BoxId where Cust like BoxId+ '%' and valid='1
        /// </summary>
        /// <param name="custLike">由调用方加%或_</param>
        /// <param name="valid"></param>
        /// <returns></returns>
        SnoCtrlBoxIdInfo GetSnoCtrlBoxIdInfoByLikeCustAndValid(string custLike, string valid);

        /// <summary>
        /// 从SnoCtrl_BoxId删除这条记录
        /// </summary>
        /// <param name="id"></param>
        void DeleteSnoCtrlBoxIdInfo(int id);

        /// <summary>
        /// 或Consolidated<>””且Consolidated前10位相同
        /// </summary>
        /// <param name="consolidated"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByLikeConsolidated(string consolidated);

        /// <summary>
        /// 查找含有指定InfoType和InfoValue的DN的列表.
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByInfoTypeAndValue(string infoType, string infoValue);

        /// <summary>
        /// insert到ShipBoxDet(每个BoxId新增一条记录，其中：Shipment=Delivery.ShipmentNo；Invoice= DN的Invoice属性值；SnoId=’’)，
        /// </summary>
        /// <param name="item"></param>
        void InsertShipBoxDet(ShipBoxDetInfo item);

        /// <summary>
        /// 同时从SnoCtrl_BoxId_SQ删除相关的记录
        /// </summary>
        /// <param name="item"></param>
        void DeleteSnoCtrlBoxIdSQInfo(SnoCtrlBoxIdSQInfo item);

        /// <summary>
        /// select distinct Col from PAK_WH_LocMas (nolock)
        /// </summary>
        /// <returns></returns>
        IList<string> GetColListFromPakWhLocMas();

        /// <summary>
        /// 查找含有指定InfoType的DN的列表.
        /// </summary>
        /// <param name="infoType"></param>
         /// <returns></returns>
        IList<Delivery> GetDeliveryListByInfoType(string infoType);

        /// <summary>
        /// Insert Delivery_Pallet
        /// </summary>
        /// <param name="item"></param>
        void InsertDeliveryPallet(DeliveryPalletInfo item);

        /// <summary>
        /// a) 选择的DN需要满足如下要求：
        ///  Delivery.ShipDate 大于3天前 (例如：当天为2011-9-13，那么获取的DN 的ShipDate 要大于2011-9-10)
        ///  Delivery.Status = ‘00’
        ///  Delivery.Model 长度为12 位
        ///  Delivery.Model 前两码为’PC’
        /// Delivery.ShipDate>= convert(char(10), getdate()-2, 111)  and Status in(‘00’,’87’) and len(Model)=12 and left(Model,2)=’PC’；按照DN正序排序
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="statuses"></param>
        /// <param name="modelPrefix"></param>
        /// <param name="modelLength"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByStatusesAndModel(DateTime begin, string[] statuses, string modelPrefix, int modelLength);

        /// <summary>
        /// 使用DeliveryNo = @DeliveryNo 查询IMES_PAK..Delivery_Pallet 表，取PalletNo 栏位
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        IList<DeliveryPalletInfo> GetDeliveryPalletListByDN(string deliveryNo);

        /// <summary>
        /// 使用Delivery = @DeliveryNo and PalletNo = @PalletNo查询IMES_PAK..Delivery_Pallet 表得到记录的DeliveryQty值
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<DeliveryPalletInfo> GetDeliveryPalletListByDN(string deliveryNo, string palletNo);

        /// <summary>
        /// C.获取该DN对应的Pallet List填充到Pallet 下拉框中：
        /// 从Delivery_Pallet中得到满足DeliveryNo=@DN的所有Pallet；下拉框格式为PalletNo+’  ’+ DeliveryQty；排序规则为绑定unit最多的pallet显示在最前面
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        IList<DeliveryPalletInfo> GetDeliveryPalletListOrderByUnitQty(string deliveryNo);

        /// <summary>
        /// sum(DeliveryQty) from Delivery_Pallet where PalletNo=@plt
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        int GetSumDeliveryQtyOfACertainPallet(string plt);

        void BackupToDelivery(string dn);

        void BackupToDeliveryPallet(string dn);

        /// <summary>
        /// 4A.刷入的是PalletNo. Delivery_Pallet.PalletNo=刷入的code
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<DeliveryPalletInfo> GetDeliveryPalletListByPlt(string palletNo);

        /// <summary>
        /// 需新增接口，入参：IList<string> dnList；返回值：IList<DNForUI>；实现功能：从IMES2012_PAK..Delivery中获取相关信息
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByDNList(IList<string> dnList);

        /// <summary>
        /// 能够显示出所查询DN的所有Qty之和
        /// </summary>
        /// <param name="dnList"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByDNList(IList<string> dnList, out long sum);

        /// <summary>
        /// 需新增接口，入参：IList<string> dnList；返回值：Ilist<DNForUI>；实现功能：从IMES2012_PAK..PoData_EDI中获取相关信息
        /// </summary>
        /// <param name="dnList"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByDNListFromPoDataEDI(IList<string> dnList);

        /// <summary>
        /// delete PoData_EDI where DeliveryNo = @DN
        /// </summary>
        /// <param name="dn"></param>
        void DeletePoDataEdi(string dn);

        /// <summary>
        /// delete PoPlt_EDI where DeliveryNo = @DN
        /// </summary>
        /// <param name="dn"></param>
        void DeletePoPltEdi(string dn);

        /// <summary>
        /// delete ShipBoxDet where Shipment=@dn
        /// </summary>
        /// <param name="shipment"></param>
        void DeleteShipBoxDet(string shipment);

        /// <summary>
        /// C. 将所有ShipmentNo等于删除DN的Shipment的所有DN，其DeliveryInfo中Consolidated属性值 ”/”后面的字串减1后保存	
        /// 需要接口，需求如下：
        /// 入参：DeliveryInfo item
        /// 功能：Update这个DeliveryInfo
        /// 
        /// 如果Consolidated中不含‘/’，就不要做‘/’后ConQTY的减1计算
        /// </summary>
        /// <param name="shipmentNo"></param>
        void UpdateDeliveryInfoForDecreaseConsolidated(string shipmentNo);

        /// <summary>
        /// delete ShipBoxDet where DeliveryNo=@dn
        /// </summary>
        /// <param name="dn"></param>
        void DeleteShipBoxDetByDn(string dn);

        void DeleteDeliveryPalletByShipmentNo(string shipmentNo);

        void DeleteDeliveryPalletByPalletNo(string palletNo);

        void BackupToDeliveryByShipmentNo(string shipmentNo);

        void BackupToDeliveryPalletByShipmentNo(string shipmentNo);

        /// <summary>
        /// delete [Delivery] by shipmentNo
        /// </summary>
        /// <param name="dn"></param>
        void DeleteDeliveryByShipmentNo(string shipmentNo);

        /// <summary>
        /// delete [DeliveryInfo] by shipmentNo
        /// </summary>
        /// <param name="dn"></param>
        void DeleteDeliveryInfoByShipmentNo(string shipmentNo);

        /// <summary>
        /// SELECT @carrier = a.InfoValue FROM DeliveryInfo a (nolock), Delivery_Pallet b (nolock)
        /// WHERE a.DeliveryNo = b.DeliveryNo
        /// AND b.Pallet = @PalletNo
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<string> GetInfoValuesByPalletNoOnDeliveryNo(string palletNo);

        /// <summary>
        /// SELECT @carrier = a.InfoValue FROM DeliveryInfo a (nolock), Delivery_Pallet b (nolock)
        /// WHERE a.DeliveryNo = b.DeliveryNo
        ///   AND b.Pallet = @PalletNo
        ///   AND a.InfoType = 'Carrier'
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetInfoValuesByPalletNoOnDeliveryNo(string palletNo, string infoType);

        /// <summary>
        /// 1.[PAK] 
        /// 与Product 绑定的Delivery (IMES_FA..Product.DeliveryNo)的ShipDate (IMES_PAK..Delivery.ShipDate)属性值和 Carrier (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Carrier')属性值相同的所有Delivery
        /// 提供SQL参考：
        /// SELECT DISTINCT a.DeliveryNo
        /// FROM Delivery AS a INNER JOIN
        /// DeliveryInfo AS b ON a.DeliveryNo = b.DeliveryNo
        /// WHERE (a.ShipDate = @ShipDate) AND (b.InfoValue = @infoValue) AND (b.InfoType = @infoType)
        /// </summary>
        /// <param name="shipdate"></param>
        /// <param name="infoValue"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetDeliveryNoByShipDateAndValueAndType(DateTime shipdate, string infoValue, string infoType);

        /// <summary>
        /// 2.[PAK] 
        /// 与Product 绑定的Delivery (IMES_FA..Product.DeliveryNo)的ShipDate (IMES_PAK..Delivery.ShipDate)属性值和 Carrier (IMES_PAK..DeliveryInfo.InfoValue，Condition: InfoType = 'Carrier')属性值相同的所有Delivery的Shipm
        /// 提供SQL参考：
        /// SELECT DISTINCT a.ShipmentNo
        /// FROM Delivery AS a INNER JOIN
        /// DeliveryInfo AS b ON a.DeliveryNo = b.DeliveryNo
        /// WHERE (a.ShipDate = @ShipDate) AND (b.InfoValue = @infoValue) AND (b.InfoType = @infoType)
        /// </summary>
        /// <param name="shipdate"></param>
        /// <param name="infoValue"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetShipmentNoByShipDateAndValueAndType(DateTime shipdate, string infoValue, string infoType);

        /// <summary>
        /// 3. [PAK] 
        /// 提供SQL参考：
        /// SELECT * FROM Delivery AS a INNER JOIN
        /// DeliveryInfo AS b ON a.DeliveryNo = b.DeliveryNo
        /// WHERE (b.InfoValue = @infoValue) AND (b.InfoType = @infoType)
        /// </summary>
        /// <param name="infoValue"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryByValueAndType(string infoValue, string infoType);

        /// <summary>
        /// SELECT a.DeliveryNo, a.ShipDate, a.Qty
        /// FROM Delivery a (nolock), ModelInfo b (nolock)
        /// WHERE a.Status < '82' 
        ///       AND a.Model IN (SELECT Model FROM Product NOLOCK WHERE ProductID in (SELECT ProductID FROM Product_Part NOLOCK WHERE PartSn = @VendorCT))
        ///       AND a.Model = b.Model
        ///       AND b.Name = 'BomPn'
        ///       AND CHARINDEX(b.Value, @146Models) > 0
        /// ORDER BY a.ShipDate
        /// </summary>
        /// <param name="infoValue"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryByModelInfoName(string status, string vendorCT, string modelInfoName, string _146Models);

        /// <summary>
        /// SELECT DeliveryNo,ShipDate,Qty 
        /// FROM Delivery 
        /// WHERE Status < '82' 
        ///       AND Model IN (SELECT Model FROM Product NOLOCK WHERE ProductID in (SELECT ProductID FROM Product_Part NOLOCK WHERE PartSn = @VendorCT))
        ///       AND CHARINDEX(Model, @146Models) > 0
        /// ORDER BY ShipDate
        /// </summary>
        /// <param name="status"></param>
        /// <param name="vendorCT"></param>
        /// <param name="_146Models"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryByVendorCT(string status, string vendorCT, string _146Models);

        /// <summary>
        /// 在ShipBoxDet中已经存在相同的DN/Pallet对应的记录时
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <returns></returns>
        bool CheckExistShipBoxDet(string dn, string plt);

        /// <summary>
        /// (1).在PoData_EDI中增加记录
        /// </summary>
        void AddPoDataEdiInfo(PoDataEdiInfo item);

        /// <summary>
        /// (2).在PoPlt_EDI中增加记录
        /// </summary>
        void AddPoPltEdiInfo(PoPltEdiInfo item);

        /// <summary>
        /// (3).更新PoData_EDI中指定DeliveryNo记录的Udt
        /// </summary>
        /// <param name="dn"></param>
        void UpdateUdtForPoDataEdi(string dn);

        /// <summary>
        /// 判断在PoData_EDI中是否已存在指定DN的记录
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        bool CheckExistPoDataEdi(string dn);

        /// <summary>
        /// EXISTS (SELECT * FROM ShipBoxDet NOLOCK WHERE DeliveryNo=@dn and PLT<>@plt and SnoId=@id)
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="snoId"></param>
        /// <returns></returns>
        bool CheckExistShipBoxDetExceptPlt(string dn, string pltExcept, string snoId);

        /// <summary>
        /// UPDATE ShipBoxDet SET SnoId='',Udt=GETDATE() WHERE DeliveryNo=@dn and SnoId=@id
        /// </summary>
        /// <param name="snoId"></param>
        /// <param name="dn"></param>
        void UpdateShipBoxDetForClearSnoid(string snoId, string dn);

        /// <summary>
        /// 使用DeliveryNo=@dn and PLT=@plt and SnoId=@id 查询ShipBoxDet表
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="pltExcept"></param>
        /// <param name="snoId"></param>
        /// <returns></returns>
        bool CheckExistShipBoxDet(string dn, string plt, string snoId);
        
        /// <summary>
        /// 使用DeliveryNo=@dn and PLT=@plt and SnoId=@id 查询ShipBoxDet表
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="snoId"></param>
        /// <returns></returns>
        IList<ShipBoxDetInfo> GetShipBoxDetList(string dn, string plt, string snoId);

        /// <summary>
        /// Update ShipBoxDet 记录结合Product 信息
        /// Condition: DeliveryNo=@dn and PLT=@plt and BoxId=@BoxId
        /// SET：SnoId=@id,Udt=GETDATE()
        /// </summary>
        /// <param name="snoid"></param>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="boxId"></param>
        void UpdateShipBoxDetForSetSnoId(string snoid, string dn, string plt, string boxId);

        /// <summary>
        /// i. 使用DeliveryNo=@dn and SnoId='' 查询ShipBoxDet 表，按照PLT, BoxId 升序排序，
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="snoId"></param>
        /// <returns></returns>
        IList<ShipBoxDetInfo> GetShipBoxDetList(string dn, string snoId);


        /// <summary>
        /// 抓第一筆BoxId 分配給ProductID
        /// update top (1) ShipBoxDet with (rowlock, readpast)
        ///set SnoId=@snoId, Editor=@snoId+Editor,
        ///    Udt =GETDATE() 
        ///output DELETED.BoxId
        ///where DeliveryNo= @dn and
        ///      PLT=@Plt  and
        ///      SnoId=''
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="snoId"></param>
        /// <returns>BoxId </returns>
        IList<string> GetAndUpdateShipBoxDet(string dn, string plt, string snoId);

        /// <summary>
        ///   Rollback before assign boxid, update 
        ///   update ShipBoxDet set SnoId='', Editor=replace(Editor,@snoid,''),Udt=getdate()
        ///   where Editor like @snoid+%
        /// </summary>
        /// <param name="snoId"></param>
        void RollBackAssignBoxId(string snoId);
        
        /// <summary>
        ///  succeed assign Boxid ,then change editor to original 
        ///  Update ShipBoxDet set Editor=replace(Editor,@snoid,''),Udt=getdate()
        ///  where DeliveryNo=@dn and SnoId=@snoid
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="plt"></param>
        /// <param name="snoId"></param>
        void UpdateAssignBoxIdEditor(string dn, string plt, string snoId);

        void UpdateAssignBoxIdEditorDefered(IUnitOfWork uow, string dn, string plt, string snoId);


        /// <summary>
        /// a. 查找Delivery 创建时间（IMES_PAK..Delivery.ShipDate）大于两天前的，状态为（IMES_PAK..Delivery.Status）'00' 的，Model（IMES_PAK..Delivery.Model） 与当前Product Model 相同的Deliveries
        /// </summary>
        /// <param name="beginCdt"></param>
        /// <param name="statuses"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByCdtAboveAndStatusesAndModel(DateTime beginCdt, string[] statuses, string model);

        /// <summary>
        /// b.查找与上述Deliveries 已经结合了Product 的Deliveries数量（可以查询IMES_FA..Product表）
        /// </summary>
        /// <param name="beginCdt"></param>
        /// <param name="statuses"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        int GetCountOfBoundDeliveryByCdtAboveAndStatusesAndModel(DateTime beginCdt, string[] statuses, string model);

        /// <summary>
        /// 2.使用IMES_PAK..Delivery.DeliveryNo = @Shipment or IMES_PAK..Delivery.ShipmentNo = 以@Shipment 为条件查询IMES_PAK..Delivery 表，
        /// 取得ShipDate > convert(char(10),GETDATE()-@day,111) 的记录的@Delivery (IMES_PAK..Delivery.DeliveryNo)
        /// </summary>
        /// <param name="shipmentNo"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        IList<string> GetDeliveryListByShipmentAndShipDateInNearlyDays(string shipmentNo, int day);

        /// <summary>
        /// 4.是否是并栈板
        /// （使用Delivery = @Delivery 查询IMES_PAK..Delivery_Pallet 表，得到所有和该Delivery 结合的Pallet ，
        /// 这些Pallet在Delivery_Pallet 表中都只有一条记录）时
        /// exists(select * from Delivery_Pallet t1 JOIN Delivery_Pallet t2 ON t1.PalletNo = t2.PalletNo 
        /// where t1.DeliveryNo = @Delivery and t2.DeliveryNo <> @Delivery)
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        bool IsCombinedPallet(string dn);

        /// <summary>
        /// SELECT @icnt = COUNT(Distinct Pallet) FROM Delivery_Pallet nolock
        /// WHERE LEFT(ShipmentNo, 10) IN (SELECT DISTINCT LEFT(ShipmentNo, 10) FROM Delivery_Pallet WHERE PalletNo = @Pallet)
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        int GetCountOfPalletWithTheSameShipmentNoPrefix(string palletNo);

        /// <summary>
        /// 如果Pallet No 不以'BA' 为前缀时，
        /// 取Delivery数量（注意这里的Delivery 指的是Delivery 前10 位，不包括后6 为的Item No）
        /// 参考方法：
        /// SELECT @icnt = COUNT(DISTINCT LEFT(DeliveryNo, 10)) FROM Delivery nolock
        /// WHERE DeliveryNo LIKE LEFT(@DeliveryNo, 10) + '%'
        /// 
        /// LD: 从SQL上看,这个的结果就只有0和1这两种可能.
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        int GetCountOfDeliveryNoWithTheSameDeliveryNoPrefix(string deliveryNo);

        /// <summary>
        /// 如果Pallet No （From UI）以'BA' 为前缀时，取Pallet数量
        /// 参考方法：
        /// SELECT @icnt = COUNT(DISTINCT a.PalletNo) FROM Delivery_Pallet a (nolock), DeliveryInfo b (nolock)
        /// WHERE a.DeliveryNo = b.DeliveryNo
        ///  AND b.InfoType = 'Consolidated'
        ///  AND b.InfoValue = @Consolidated
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        int GetCountOfPalletOfDeliveryWithInfoTypeAndInfoValue(string infoType, string infoValue);

        /// <summary>
        /// SELECT ShipmentNo FROM Delivery_Pallet 
        ///  WHERE ShipmentNo LIKE LEFT(@Shipment, 10) + '%' or ShipmentNo = @Shipment
        /// </summary>
        /// <param name="shipmentNo"></param>
        /// <returns></returns>
        IList<string> GetShipmentNoListWithSimilarShipmentNo(string shipmentNo);
        
        IList<string> GetPalletNoListWithSimilarShipmentNo(string shipmentNo);

        /// <summary>
        /// SELECT DISTINCT PalletNo, DeliveryQty FROM Delivery_Pallet 
        ///  WHERE ShipmentNo LIKE LEFT(@Shipment, 10) + '%' or ShipmentNo = @Shipment
        /// </summary>
        /// <param name="shipmentNo"></param>
        /// <returns></returns>
        IList<PalletNoAndDeliveryQtyEntity> GetDistinctPalletNoAndDeliveryQtyWithSimilarShipmentNo(string shipmentNo);

        /// <summary>
        /// SELECT DISTINCT a.ShipmentNo FROM Delivery a(nolock), DeliveryInfo b(nolock)
        /// WHERE a.DeliveryNo = b.DeliveryNo
        ///  b.InfoType = 'Consolidated'
        ///  b.InfoValue = @Consolidated
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        IList<string> GetShipmentNoListOfDeliveryWithInfoTypeAndInfoValue(string infoType, string infoValue);

        /// <summary>
        /// SELECT DISTINCT ShipmentNo FROM Delivery_Pallet nolock
        /// WHERE PalletNo = @Pallet and ShipmentNo not in (SELECT DeliveryNo FROM #DN)
        /// </summary>
        /// <param name="pltNo"></param>
        /// <param name="shipmentNos"></param>
        /// <returns></returns>
        IList<string> GetShipmentNoListByPalletNoAndShipmentNo(string pltNo, string[] shipmentNos);

        /// <summary>
        /// SELECT DISTINCT a.PalletNo, a.DeliveryQty FROM Delivery_Pallet a (nolock), #DN b (nolock)
        ///  WHERE LEFT(a.DeliveryNo, 10) = LEFT(b.DeliveryNo, 10)
        /// </summary>
        /// <param name="deliveryNos"></param>
        /// <returns></returns>
        IList<PalletNoAndDeliveryQtyEntity> GetDistinctPalletNoAndDeliveryQtyByDeliveryNos(string[] deliveryNos);

        /// <summary>
        /// SELECT DISTINCT a.PalletNo, a.DeliveryQty FROM Delivery_Pallet a (nolock), #DN b (nolock)
        ///  WHERE a.ShipmentNo = b.DeliveryNo
        /// </summary>
        /// <param name="shipmentNos"></param>
        /// <returns></returns>
        IList<PalletNoAndDeliveryQtyEntity> GetDistinctPalletNoAndDeliveryQtyByShipmentNos(string[] shipmentNos);
        /// <summary>
        /// 
        /// 
        /// 取得Pallet 数量
        /// 参考方法：
        /// SELECT @rcnt=COUNT(PLT) FROM #plt 
        /// WHERE PLT not in (SELECT DISTINCT a.PLT FROM #plt a, Pallet b (nolock)
        ///  WHERE a.PLT = b.PalletNo
        ///   and LEFT(a.PLT, 2) not in ('BA', 'NA')
        /// </summary>
        /// <param name="pltNos"></param>
        /// <returns></returns>
        IList<string> GetExistedPalletNoList(string[] pltNos);

        /// <summary>
        /// SELECT @rcnt2 = COUNT(a.ProductID) FROM IMES_FA..Product a nolock, b.DeliveryInfo b nolock
        /// WHERE a.DeliveryNo = b.DeliveryNo
        ///  b.InfoType = 'Consolidated'
        ///  b.InfoValue = @Consolidated 
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        int GetCountOfProductOfDeliveryWithInfoTypeAndInfoValue(string infoType, string infoValue);

        /// <summary>
        /// SELECT @rcnt2 = COUNT(a.ProductID) FROM IMES_FA..Product a(nolock), Delivery_Pallet b(nolock)
        /// WHERE a.PalletNo = b.PalletNo
        ///  AND b.ShipmentNo IN (SELECT ShipmentNo FROM Delivery_Pallet WHERE PalletNo = @Pallet)
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        int GetCountOfProductWithTheSameShipmentNo(string palletNo);

        /// <summary>
        /// EXISTS(SELECT * FROM Delivery WHERE DeliveryNo LIKE LEFT(@Shipment, 10) + '%' and Model LIKE 'PO%')
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="modelPrefix"></param>
        /// <returns></returns>
        bool CheckExistDeliveryByShipmentPrefixAndModelPrefix(string shipment, string modelPrefix);

        /// <summary>
        /// SELECT  @dockingModel = Model FROM Delivery WHERE DeliveryNo LIKE LEFT(@Shipment, 10) + '%' and Model LIKE 'PO%'
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="modelPrefix"></param>
        /// <returns></returns>
        IList<string> GetModelsByShipmentPrefixAndModelPrefix(string shipment, string modelPrefix);

        /// <summary>
        /// SELECT @dockingQty = SUM(Qty) FROM Delivery WHERE DeliveryNo LIKE LEFT(@Shipment, 10) + '%' and Model LIKE 'PO%'
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="modelPrefix"></param>
        /// <returns></returns>
        int GetSumQtyOfDeliveryByShipmentPrefixAndModelPrefix(string shipment, string modelPrefix);

        #region NotImplementedException

        /// <summary>
        /// select * from delivery where deliveryno = deliverty_no
        /// </summary>
        /// <param name="deliverty_no"></param>
        /// <returns></returns>
        Delivery GetDelivery(string deliverty_no);

        #endregion

        /// <summary>
        /// SELECT SUM(a.Qty) as [BOLQTY] FROM IMES_PAK..Delivery a (nolock), IMES_PAK..DeliveryInfo b (nolock)
        /// WHERE a.DeliveryNo = b.DeliveryNo
        /// and b.InfoValue LIKE '%' + RTRIM(@BOL) + '%'
        /// </summary>
        /// <param name="bol"></param>
        /// <returns></returns>
        int GetBOLQty(string bol);

        /// <summary>
        /// SELECT b.DeliveryNo as DN, COUNT(a.SnoId) as DNQty
        /// FROM Dummy_ShipDet a (nolock), IMES_FA..Product b(nolock)
        /// WHERE a.SnoId = b.ProductID 
        /// AND a.PLT = @DummyPalletNo
        /// GROUP BY b.DeliveryNo
        /// </summary>
        /// <param name="dummyPalletNo"></param>
        /// <returns></returns>
        IList<DeliveryNoAndQtyEntity> GetDNAndQtyByDummyPalletNo(string dummyPalletNo);

        /// <summary>
        /// SELECT b.CUSTSN as [Customer S/N]
        /// FROM Dummy_ShipDet a (nolock), IMES_FA..Product b(nolock)
        /// WHERE a.SnoId = b.ProductID 
        /// AND a.PLT = @DummyPalletNo
        /// ORDER BY b.CUSTSN
        /// </summary>
        /// <param name="dummyPalletNo"></param>
        /// <returns></returns>
        IList<string> GetCustSnListByDummyPalletNo(string dummyPalletNo);

        /// <summary>
        /// 保存IMES_PAK..Dummy_ShipDet表
        /// </summary>
        /// <param name="item"></param>
        void InsertDummyShipDetInfo(DummyShipDetInfo item);

        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IList<DummyShipDetInfo> GetDummyShipDetInfoList(DummyShipDetInfo condition);

        /// <summary>
        /// setValue哪个字段赋值就有更新哪个字段
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateDummyShipDetInfo(DummyShipDetInfo setValue, DummyShipDetInfo condition);

        /// <summary>
        /// 1、通过DN获取关联的Pallets
        /// </summary>
        /// <param name="shipmentNo"></param>
        /// <returns></returns>
        IList<string> GetPalletNoListByShipmentNo(string shipmentNo);

        /// <summary>
        /// 2、通过shipmentNo获取关联的Pallets
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        IList<string> GetPalletNoListByDeliveryNo(string deliveryNo);

        /// <summary>
        /// 查找含有指定InfoValue的DN的列表.
        /// </summary>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByInfoValue(string infoValue);

        /// <summary>
        /// delete from ShipBoxDet where DeliveryNo in (select DeliveryNo from Delivery where ShipmentNo='')
        /// </summary>
        /// <param name="shipment"></param>
        void DeleteShipBoxDetByShipmentNo(string shipment);

        /// <summary>
        /// setValue哪个字段赋值就有更新哪个字段
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateShipBoxDetInfo(ShipBoxDetInfo setValue, ShipBoxDetInfo condition);

        /// <summary>
        /// update Delivery set [Status]='9'+substring([Status],2,1) where DeliveryNo in (输入列表)
        /// </summary>
        /// <param name="dns"></param>
        /// <param name="newTitleChar"></param>
        void UpdateDeliveryForStatusChange(string[] dns, string newTitleChar);

        /// <summary>
        /// update Delivery set [Status]=@status where DeliveryNo in (输入列表)
        /// </summary>
        /// <param name="dns"></param>
        /// <param name="status"></param>
        void UpdateMultiDeliveryForStatusChange(string[] dns, string status);


        /// <summary>
        /// select 字段名 as InfoType,字段值 as InfoValue from [PAK.PAKComn] where InternalID='P19DA30A00000003' 按属性名排序
        /// TOP 1 
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<DNInfoForUI> GetPakDotPakComnInKeyValue(string internalId);

        /// <summary>
        /// select PALLET_ID as PalletNo,PALLET_UNIT_QTY as DeliveryQty,'' as UCC from [PAK.PAKPaltno] where InternalID='输入dn'
        /// </summary>
        /// <param name="internalId"></param>
        /// <returns></returns>
        IList<DNPalletQty> GetPakDotPakPaltnoInfoList(string internalId);

        /// <summary>
        /// consol_invoice对应SHipmentID
        /// po_num对应PoNo
        /// model对应ModelName
        /// actual_shipdate对应ShipDate_Str
        /// Qty = select sum(PALLET_UNIT_QTY) from [PAK.PAKPaltno] where InternalID='对应dn'
        /// </summary>
        /// <param name="dnList"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByDNListFromPakDotPakComn(IList<string> dnList);

        /// <summary>
        /// 对于输入条件，dn需要判断长度，如果是16位则需要精确匹配，如果是10位，则模糊匹配（前10位）其他条件需要精确匹配
        /// 
        /// DNInfo精确匹配的方式是 等于 表中的 任何字段的值 
        /// 
        /// 参数类型：DNQueryCondition（含DNInfo的查询条件）
        /// TOP 1000
        /// </summary>
        /// <param name="MyCondition"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        IList<DNForUI> GetDNListByConditionFromPakDotPakComn(DNQueryCondition MyCondition, out int totalLength);

        /// <summary>
        /// UPDATE SnoCtrl_BoxId SET valid=@editor WHERE Cust=@alarm AND valid='1'
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateSnoCtrlBoxIdInfo(SnoCtrlBoxIdInfo setValue, SnoCtrlBoxIdInfo condition);

        /// <summary>
        /// SELECT @BoxId=RTRIM(BoxId) FROM SnoCtrl_BoxId WHERE Cust=@alarm AND valid=@editor
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetBoxIdListFromSnoCtrlBoxId(SnoCtrlBoxIdInfo condition);

        /// <summary>
        /// DELETE FROM SnoCtrl_BoxId WHERE BoxId=@BoxId
        /// </summary>
        /// <param name="condition"></param>
        void DeleteSnoCtrlBoxIdInfo(SnoCtrlBoxIdInfo condition);

        /// <summary>
        /// SELECT * 
        /// FROM Delivery (NOLOCK) 
        /// WHERE Status ='00' 
        ///     And Model = @Model
        ///     And ShipDate >= CONVERT(char(10),GETDATE() - 5,111) 
        /// ORDER BY ShipDate
        /// </summary>
        /// <param name="model"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByModel(string model, string status);
        
        /// <summary>
        /// SELECT * 
        /// FROM Delivery (NOLOCK)         
        /// WHERE Status < '82' 
        ///     and Model = @Model
		///     and ShipDate>=convert(char(10),getdate()-5,111) 
        /// ORDER BY ShipDate，
        /// </summary>
        /// <param name="model"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByModelAndStatus(string model, string status);

        /// <summary>
        /// 10 位的DN 对应的是Delivery.DeliveryNo 的前10位，需要将其对应的16 位DN 找到
        /// SELECT [DeliveryNo] FROM [IMES2012HP_FORRD].[dbo].[Delivery] where SUBSTRING(DeliveryNo,1,10)=@input 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IList<string> GetDNListBy10BitPrefix(string input);

        /// <summary>
        /// 从整机数据库中使用Consolidate No 检索DeliveryInfo 表，取得相关记录，统计这些记录共有多少个不同的LEFT(DeliveryNo，10)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int GetDnCountBySP(string value);

        /// <summary>
        /// SELECT TOP 1 @Delivery = DeliveryNo
        ///     FROM Delivery
        ///     WHERE PoNo = @FactoryPo
        ///        AND CONVERT(char(10), ShipDate, 111)>=CONVERT(char(10),GETDATE()-3,111) 
        ///        AND LEFT(Model, 2) = 'PC' 
        ///        AND LEN(Model)=12 
        ///        AND Status = '00'
        /// ORDER BY ShipDate, Qty, DeliveryNo
        /// </summary>
        /// <param name="factoryPo"></param>
        /// <param name="modelPrefix"></param>
        /// <param name="modelLength"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByModelPrefix(string factoryPo, string modelPrefix, int modelLength, string status);

        /// <summary>
        /// SELECT * FROM Delivery NOLOCK 
        /// WHERE Model = @Model		AND LEFT(Model, 2) = 'PC'
        ///             AND CONVERT(char(10), ShipDate, 111)>=CONVERT(char(10),GETDATE()-3,111)
        ///             AND LEN(Model) = 12
        ///             AND Status = '00' ordery by shipdate,DeliveryNo,
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelPrefix"></param>
        /// <param name="modelLength"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByModel(string model, string modelPrefix, int modelLength, string status);

        /// <summary>
        /// 1、获取DN信息 list
        /// SELECT *
        /// FROM Delivery 
        /// WHERE Status < '82' 
        /// AND ShipDate>=convert(char(10),getdate()-5,111) 
        /// AND Model = @Model
        /// ORDER BY ShipDate
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<Delivery> GetDeliveryListByShipDateAndModelAndStatus(string model);

        /// <summary>
        /// INSERT INTO [DeliveryAttrLog]([DeliveryNo],[ShipmentNo],[AttrName],[AttrOldValue],[AttrNewValue],[Descr],[Editor],[Cdt])
        /// </summary>
        /// <param name="item"></param>
        void InsertDeliveryAttrLogInfo(DeliveryAttrLogInfo item);

        /// <summary>
        /// UPDATE DeliveryInfo SET InfoValue=@NewConsolidated, Editor=@editor
        /// WHERE InfoType = 'Consolidated' 
        /// AND LEFT(InfoValue, 10) = @Consolidate
        /// </summary>
        /// <param name="newConsolidated"></param>
        /// <param name="infoType"></param>
        /// <param name="consolidate"></param>
        void UpdateDeliveryInfoValueByInfoTypeAndInfoValuePrefix(string newConsolidated, string infoType, string consolidate, string editor);

        /// <summary>
        /// INSERT INTO [DeliveryAttrLog]([DeliveryNo],[ShipmentNo],[AttrName],[AttrOldValue],[AttrNewValue],[Descr],[Editor],[Cdt])
        /// SELECT a.DeliveryNo, a.ShipmentNo, 'Consolidated', b.InfoValue, @NewConsolidated, 'CQ', @Editor, GETDATE()
        /// FROM Delivery a (NOLOCK), DeliveryInfo b (NOLOCK)
        /// WHERE a.DeliveryNo = b.DeliveryNo
        /// AND b.InfoType = 'Consolidated'
        /// AND LEFT(InfoValue, 10) = @Consolidate
        /// </summary>
        /// <param name="newConsolidated"></param>
        /// <param name="editor"></param>
        /// <param name="consolidate"></param>
        void InsertDeliveryAttrLog(string newConsolidated, string editor, string consolidate);

        /// <summary>
        /// SELECT @COUNT=COUNT(DISTINCT LEFT(DeliveryNo,10)) 
        /// FROM DeliveryInfo (NOLOCK) 
        /// WHERE (InfoType = 'Consolidated' AND InfoValue = RTRIM(@consolidate))
        /// OR (InfoType = 'RedShipment' AND InfoValue = RTRIM(@consolidate))
        /// </summary>
        /// <param name="infoType1"></param>
        /// <param name="infoValue1"></param>
        /// <param name="infoType2"></param>
        /// <param name="infoValue2"></param>
        /// <returns></returns>
        int GetCountOfDeliveryNoPrefixForDoubleDeliveryInfoPairs(string infoType1, string infoValue1, string infoType2, string infoValue2);

        /// <summary>
        /// UPDATE Delivery SET Status = '00', Editor = @Editor, Udt = GETDATE()
        /// WHERE DeliveryNo IN (SELECT DISTINCT DeliveryNo FROM Product NOLOCK WHERE CartonSN = @CartonNo) 
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="cartonNo"></param>
        void UpdateDeliveryForStatusChange(string editor, string cartonNo);

        /// <summary>
        /// Insert IMES_PAK..[DeliveryLog] – 记录Delivery
        /// </summary>
        /// <param name="item"></param>
        void InsertDeliveryLog(DeliveryLog item);
        
        /// <summary>
        /// select count（DISTINCT pono）from Delivery where DeliveryNo = @DeliveryNo
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCountOfPonoFromDelivery(Delivery condition);

        /// <summary>
        /// select count（DISTINCT model）from Delivery where DeliveryNo = @DeliveryNo
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCountOfModelFromDelivery(Delivery condition);
        
        /// <summary>
        /// select Infovalue from DeliveryInfo where DeliveryNo IN (SELECT DeliveryNo FROM Product NOLOCK WHERE PalletNo = @PalletNo) and InfoType='ShiptoId'
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<DeliveryInfo> GetDeliveryInfoFromDeliveryByPalletNo(string palletNo, DeliveryInfo condition);

        /// <summary>
        /// select count（DISTINCT pono）from Delivery where DeliveryNo IN (SELECT DeliveryNo FROM Product NOLOCK WHERE PalletNo = @PalletNo) 
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCountOfPonoFromDeliveryByPalletNo(string palletNo, Delivery condition);

        /// <summary>
        /// select count（DISTINCT model）from Delivery where DeliveryNo IN (SELECT DeliveryNo FROM Product NOLOCK WHERE PalletNo = @PalletNo)
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCountOfModelFromDeliveryByPalletNo(string palletNo, Delivery condition);

        IList<ShipBoxDetInfo> GetShipBoxDetInfoListByCondition(ShipBoxDetInfo condition);
        #region Upload SN to SAP
        IList<Delivery> GetDeliveryListWithTrans(IList<string> deliveryNoList);

        IList<Delivery> GetDeliveryWithSamePrefixDeliveryNo(string prefixDeliveyNo);
        IList<DeliveryPalletInfo> GetDeliveryPalletWithSamePrefixDeliveryNo(string prefixDeliveyNo);
        #endregion
        void UpdateDeliveryPalletInfo(DeliveryPalletInfo setValue, DeliveryPalletInfo condition);

        #region Defered

        void UpdateDNByConditionDefered(IUnitOfWork uow, DNUpdateCondition myCondition, string editor);

        void UpdateDeliverInfoByIDDefered(IUnitOfWork uow, int deliverInfoID, string infoValue, string editor);

        void UpdateDeliverQtyDefered(IUnitOfWork uow, int deliveryPalletID, short deliveryQty, string editor);

        void UpdateDNStatusWhenDNPalletFullDefered(IUnitOfWork uow, string deliveryNo, string status, string editor);

        void UpdateAllDNStatusWhenPalletFullDefered(IUnitOfWork uow, string deliveryNo, string palletno, string status, string editor);

        void UpdateSnoidForShipBoxDetDefered(IUnitOfWork uow, string snoId, string dn);

        void RemovePAK_PackkingData_EDIDataByProdIdsDefered(IUnitOfWork uow, string[] prodIds);

        void RemovePAKOdmSession_EDIDataByProdIdsDefered(IUnitOfWork uow, string[] prodIds);

        void InsertDeliveryInfoDefered(IUnitOfWork uow, DeliveryInfo item);

        void UpdateDNUdtDefered(IUnitOfWork uow, string dn);

        void DeleteSnoCtrlBoxIdInfoDefered(IUnitOfWork uow,int id);

        void InsertShipBoxDetDefered(IUnitOfWork uow, ShipBoxDetInfo item);

        void DeleteSnoCtrlBoxIdSQInfoDefered(IUnitOfWork uow, SnoCtrlBoxIdSQInfo item);

        void InsertDeliveryPalletDefered(IUnitOfWork uow, DeliveryPalletInfo item);

        void DeletePoDataEdiDefered(IUnitOfWork uow, string dn);

        void DeletePoPltEdiDefered(IUnitOfWork uow, string dn);

        void DeleteShipBoxDetDefered(IUnitOfWork uow, string shipment);

        void DeleteShipBoxDetByDnDefered(IUnitOfWork uow, string dn);

        void AddPoDataEdiInfoDefered(IUnitOfWork uow, PoDataEdiInfo item);

        void AddPoPltEdiInfoDefered(IUnitOfWork uow, PoPltEdiInfo item);

        void UpdateUdtForPoDataEdiDefered(IUnitOfWork uow, string dn);

        void UpdateShipBoxDetForClearSnoidDefered(IUnitOfWork uow, string snoId, string dn);

        void UpdateShipBoxDetForSetSnoIdDefered(IUnitOfWork uow, string snoid, string dn, string plt, string boxId);

        void InsertDummyShipDetInfoDefered(IUnitOfWork uow, DummyShipDetInfo item);

        void UpdateDummyShipDetInfoDefered(IUnitOfWork uow, DummyShipDetInfo setValue, DummyShipDetInfo condition);

        void DeleteShipBoxDetByShipmentNoDefered(IUnitOfWork uow, string shipment);

        void UpdateShipBoxDetInfoDefered(IUnitOfWork uow, ShipBoxDetInfo setValue, ShipBoxDetInfo condition);

        void UpdateDeliveryForStatusChangeDefered(IUnitOfWork uow, string[] dns, string newTitleChar);

        void UpdateMultiDeliveryForStatusChangeDefered(IUnitOfWork uow, string[] dns, string status);

        void PersistUpdatedItemDefered(IUnitOfWork uow, InvokeBody preCond, Delivery item);

        void UpdateSnoCtrlBoxIdInfoDefered(IUnitOfWork uow, SnoCtrlBoxIdInfo setValue, SnoCtrlBoxIdInfo condition);

        void DeleteSnoCtrlBoxIdInfoDefered(IUnitOfWork uow, SnoCtrlBoxIdInfo condition);

        void InsertDeliveryAttrLogInfoDefered(IUnitOfWork uow, DeliveryAttrLogInfo item);

        void UpdateDeliveryInfoValueByInfoTypeAndInfoValuePrefixDefered(IUnitOfWork uow, string newConsolidated, string infoType, string consolidate, string editor);

        void InsertDeliveryAttrLogDefered(IUnitOfWork uow, string newConsolidated, string editor, string consolidate);

        void UpdateDeliveryForStatusChangeDefered(IUnitOfWork uow, string editor, string cartonNo);

        void InsertDeliveryLogDefered(IUnitOfWork uow, DeliveryLog item);

        void UpdateDeliveryPalletInfoDefered(IUnitOfWork uow, DeliveryPalletInfo setValue, DeliveryPalletInfo condition);

        #endregion

        #region For Maintain

        /// <summary>
        /// 参考sql如下：
        /// select DeliveryNo from Delivery
        /// 按DeliveryNo 列的字符序排序
        /// </summary>
        /// <returns></returns>
        IList<string> GetDeliveryList();

        /// <summary>
        /// 参考sql如下：
        /// select Distinct InfoType from DeliveryInfo
        /// 按InfoType列的字符序排序
        /// </summary>
        /// <returns></returns>
        IList<string> GetDeliveryInfoList();

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为KittingLocationInfo为泛型参数的列表;
        /// SQL语句：
        /// SELECT [TagID]      
        ///       ,[GateWayIP]    
        ///       ,[RackID]
        ///       ,[TagDescr]
        ///       ,[Editor]
        ///       ,[Cdt]
        ///       ,[Udt]
        ///   FROM [IMES_PAK].[dbo].[Kitting_Location]
        ///   ORDER BY [TagID], [TagDescr]
        /// </summary>
        /// <returns></returns>
        IList<KittingLocationInfo> GetKittingLocationList();

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为KittingLocationInfo为泛型参数的列表;
        /// 参数为字符串类型,对应表中的TagID字段.
        /// SQL语句：
        /// SELECT [TagID]      
        ///       ,[GateWayIP]    
        ///       ,[RackID]
        ///       ,[TagDescr]
        ///       ,[Editor]
        ///       ,[Cdt]
        ///       ,[Udt]
        ///   FROM [IMES_PAK].[dbo].[Kitting_Location]
        ///   WHERE [TagID]=@TagID
        ///   ORDER BY [TagID], [TagDescr]
        /// </summary>
        /// <param name="tagID"></param>
        /// <returns></returns>
        IList<KittingLocationInfo> GetKittingLocationList(string tagID);

        /// <summary>
        /// 返回值和参数说明：
        /// 参数为两个字符串,分别对应数据库表中的[TagDescr]和[Editor]字段
        /// SQL语句：
        /// UPDATE [IMES_PAK].[dbo].[Kitting_Location]
        ///    SET [TagDescr] = @TagDescr
        ///       ,[Editor] = @Editor
        ///       ,[Udt] = GETDATE()
        ///  WHERE [GateWayIP]=@GateWayIP
        ///      AND [RackID]=@RackID
        /// </summary>
        /// <param name="tagDescr"></param>
        /// <param name="editor"></param>
        void UpdateKittingLocation(KittingLocationInfo item);

        #region Defered

        void UpdateKittingLocationDefered(IUnitOfWork uow, KittingLocationInfo item);

        #endregion

        #endregion

        #region Lucy Liu Added

        /// <summary>
        /// 调用DeleteDeliveryByDn,DeleteDeliveryInfoByDn,DeleteDeliveryPalletByDn三个函数
        /// </summary>
        /// <param name="dn"></param>
        void DeleteDn(string dn);

        /// <summary>
        /// 返回值和参数说明：
        /// /// SQL语句：
        /// delete from [IMES_PAK].[dbo].[Delivery] Where DeliveryNo=@dn
        /// </summary>
        /// <param name="dn"></param>
        void DeleteDeliveryByDn(string dn);

        /// <summary>
        /// 返回值和参数说明：
        /// /// SQL语句：
        /// delete from [IMES_PAK].[dbo].[DeliveryInfo] Where DeliveryNo=@dn
        /// </summary>
        /// <param name="dn"></param>
        void DeleteDeliveryInfoByDn(string dn);

        /// <summary>
        /// SQL: delete from DeliveryAttr where DeliveryNo='<dn>'
        /// </summary>
        /// <param name="dn"></param>
        void DeleteDeliveryAttrsByDn(string dn);

        /// <summary>
        /// SQL: delete from DeliveryAttrLog where DeliveryNo='<dn>'
        /// </summary>
        /// <param name="dn"></param>
        void DeleteDeliveryAttrLogByDn(string dn);

        /// <summary>
        /// SQL: delete from PalletAttrLog where PalletNo in ('<pltNos>')
        /// </summary>
        /// <param name="pltNos"></param>
        void DeletePalletAttrLog(IList<string> pltNos);

        /// <summary>
        /// SQL: delete from DeliveryAttr where DeliveryNo in (select DeliveryNo from Delivery where ShipmentNo='<shipment>')
        /// </summary>
        /// <param name="shipment"></param>
        void DeleteDeliveryAttrsByShipmentNo(string shipment);

        /// <summary>
        /// SQL: delete from DeliveryAttrLog where DeliveryNo in (select DeliveryNo from Delivery where ShipmentNo='<shipment>')
        /// </summary>
        /// <param name="shipment"></param>
        void DeleteDeliveryAttrLogByShipmentNo(string shipment);

        /// <summary>
        /// 返回值和参数说明：
        /// /// SQL语句：
        /// delete from [IMES_PAK].[dbo].[Delivery_Pallet] Where DeliveryNo=@dn
        /// </summary>
        /// <param name="dn"></param>
        void DeleteDeliveryPalletByDn(string dn);

        /// <summary>
        /// "删除Pallet表中一条记录数据
        /// </summary>
        /// <param name="palletNo"></param>
        void DeletePalletByPalletNo(string palletNo);

        /// <summary>
        /// 获取DN List: select distinct LEFT(DeliveryNo, 10) from DeliveryInfo nolock where InfoType = 'BOL' AND InfoValue = @DN
        /// </summary>
        /// <param name="infoValue"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetDeliveryNoPrefixByValueAndType(string infoValue, string infoType);

        /// <summary>
        /// select * from DeliveryInfo nolock where InfoType = 'BOL' AND InfoValue = @DN
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<DeliveryInfo> GetDeliveryInfoList(DeliveryInfo condition);

        /// <summary>
        /// DELETE FROM PoData_EDI WHERE left(DeliveryNo, 10) in (<prefixList中各元素>)
        /// </summary>
        /// <param name="prefixList"></param>
        void DeletePoDataEdi(IList<string> prefixList);

        /// <summary>
        /// DELETE FROM PoPlt_EDI WHERE left(DeliveryNo, 10) in (<prefixList中各元素>)
        /// </summary>
        /// <param name="prefixList"></param>
        void DeletePoPltEdi(IList<string> prefixList);

        #region . Defered .

        void DeleteDeliveryPalletByDnDefered(IUnitOfWork uow, string dn);

        void UpdateDeliveryInfoForDecreaseConsolidatedDefered(IUnitOfWork uow, string deliveryNo);

        void BackupToDeliveryDefered(IUnitOfWork uow, string dn);

        void BackupToDeliveryPalletDefered(IUnitOfWork uow, string dn);

        void DeleteDeliveryPalletByShipmentNoDefered(IUnitOfWork uow, string shipmentNo);

        void DeleteDeliveryPalletByPalletNoDefered(IUnitOfWork uow, string palletNo);

        void BackupToDeliveryByShipmentNoDefered(IUnitOfWork uow, string shipmentNo);

        void BackupToDeliveryPalletByShipmentNoDefered(IUnitOfWork uow, string shipmentNo);

        void DeleteDeliveryByShipmentNoDefered(IUnitOfWork uow, string shipmentNo);

        void DeleteDeliveryInfoByShipmentNoDefered(IUnitOfWork uow, string shipmentNo);

        void DeletePoDataEdiDefered(IUnitOfWork uow, IList<string> prefixList);

        void DeletePoPltEdiDefered(IUnitOfWork uow, IList<string> prefixList);

        void DeleteDeliveryByDnDefered(IUnitOfWork uow, string dn);

        void DeleteDeliveryInfoByDnDefered(IUnitOfWork uow, string dn);

        void DeleteDeliveryAttrsByDnDefered(IUnitOfWork uow, string dn);

        void DeleteDeliveryAttrLogByDnDefered(IUnitOfWork uow, string dn);

        void DeletePalletAttrLogDefered(IUnitOfWork uow, IList<string> pltNos);

        void DeleteDeliveryAttrsByShipmentNoDefered(IUnitOfWork uow, string shipment);

        void DeleteDeliveryAttrLogByShipmentNoDefered(IUnitOfWork uow, string shipment);

        #endregion

        #endregion

        void DeleteDnDefered(IUnitOfWork uow, string dn);

        #region . OnTrans .

        Delivery QueryAsLockDnForCombineCOAandDN_OnTrans(string dn);

        /// <summary>
        /// 从SnoCtrl_BoxId_SQ 按照Cust=[BoxId前缀]获取Delivery_Pallet.DeliveryQty条记录，
        /// 就是表中应该有很多条记录的Cust字段是相同的，接口要实现的就是按顺序取出Cust='入参1'的'入参2'条记录
        /// </summary>
        /// <param name="cust"></param>
        /// <param name="rowCount"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        IList<SnoCtrlBoxIdSQInfo> GetSnoCtrlBoxIdSQListByCust(string cust, int rowCount, int offset);

         
        #endregion

        #region for DeliveryEx table
        void FillDeliveryEx(Delivery delivery);
        /// <summary>
        /// sql merge statement
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="deliveryEx"></param>
        //void SetDeliveryEx(string deliveryNo, DeliveryEx deliveryEx);
        //void SetDeliveryExDefered(IUnitOfWork uow, string deliveryNo, DeliveryEx deliveryEx);

        void RemoveDeliveryEx(string deliveryNo);
        void RemoveDeliveryExDefered(IUnitOfWork uow, string deliveryNo);

        int GetUploadDNQtyByShipment(string shipmentNo);
        void UpdateConsolidateQtyInDeliveryEx(string consolidateId,int qty,string editor);
        void UpdateConsolidateQtyInDeliveryExDefered(IUnitOfWork uow, string consolidateId, int qty,string editor);

        IList<ShipmentInfoDef> GetShipmentByDnList(string DBconnectionStr, IList<string> dnList);

        int GetSumCartonQtyByPalletNo(string palletNo);
        int GetFullPalletCartonQtyByDeliveryPallet(string deliveryNo ,string palletNo); 
        #endregion

        #region Get EDI Data

        MRPLabelDef GetMRPLabel(string deliveryNo);
        int GetPackingDataSNCount(IList<string> dnList);
        IList<string> GetEdiDnListByWayBill(string wayBill);
        IList<string> GetEdiDnListByShipment(string shipmentNo);
        int GetPackComnSNCount(IList<string> dnList);

        bool CheckEDI850ByHPPoNum(string hpPoNum);
        void updateEDIPAKComnShipDate(string internalID, string shipDate);
        void updateEDIPAKComnShipDateDefered(IUnitOfWork uow, string internalID, string shipDate);
 
        #endregion

        #region SpecialOrder table for maintain UI
        
        void InsertSpecialOrder(SpecialOrderInfo sepcialOrder);
        void UpdateSpecialOrder(SpecialOrderInfo sepcialOrder);
        void DeleteSpecialOrder(string factoryPO);
        IList<SpecialOrderInfo> GetSpecialOrder(string category, SpecialOrderStatus status, DateTime startTime, DateTime endTime);
        bool ExistSpecialOrder(string factoryPO);
        SpecialOrderInfo GetSpecialOrderByPO(string factoryPO);

        #endregion

        #region  DeliveryAttr insert/update/Get
        void UpdateAndInsertDeliveryAttr(string deliveryNo, string attrName, string attrValue, string descr, string editor);
        void UpdateAndInsertDeliveryAttrDefered(IUnitOfWork uow, string deliveryNo, string attrName, string attrValue, string descr, string editor);

        IList<DeliveryAttrInfo> GetDeliveryAttr(DeliveryAttrInfo condition);
        IList<DeliveryAttrLogInfo> GetDeliveryAttrLog(DeliveryAttrLogInfo condition);
        IList<DeliveryForRCTO146> GetDeliveryForRCTO146(string model, string status, DateTime beginShipDate, DateTime endShipDate);

        IList<DeliveryForRCTO146> GetDeliveryForRCTO146(string modelPrefix, string status, DateTime beginShipDate, DateTime endShipDate,string modelInfoName, string modelInfoValue);

        IList<string> GetModelByPalletNo(string palletNo);

        int GetDeliveryQtyOnTrans(string deliveryNo,string status);
        int GetDeliveryQtyOnTrans(string deliveryNo);
        IList<DeliveryPalletInfo> GetDeliveryPalletListByDNOnTrans(string deliveryNo);

        void UpdateDeliveryStatus(string deliveryNo, string status);
        void UpdateDeliveryStatusDefered(IUnitOfWork uow, string deliveryNo, string status);

        void InsertEDIUploadPOLog(EDIUploadPOLogInfo log);        

        #endregion

        #region UPS
        bool ExistsDeliveryByPoNo(string model, string poNo, string status, DateTime afterDate);
        #endregion
    }
}
