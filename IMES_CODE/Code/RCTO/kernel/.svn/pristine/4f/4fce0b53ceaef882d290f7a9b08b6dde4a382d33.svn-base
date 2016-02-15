// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
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
//
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.TestLog;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Process;
using System.Data;
using IMES.FisObject.PAK.BoxerBookData;
using IMES.FisObject.PCA.TestBoxDataLog;
using IMES.Infrastructure.Util;
using IMES.FisObject.PAK.DN;
using System.Data.SqlClient;

namespace IMES.FisObject.FA.Product
{
    /// <summary>
    /// 用于访问Process对象的Repository接口
    /// </summary>
    public interface IProductRepository : IRepository<IProduct>
    {
        /// <summary>
        /// 用于Carton称重时，任意取一个SN来创建Product对象，进行卡站
        /// </summary>
        /// <param name="cartonNumber"></param>
        /// <returns></returns>
        string GetTop1SNByCarton(string cartonNumber);

        /// <summary>
        /// 根据前缀获取最大ProductId
        /// </summary>
        /// <param name="preSeqStr">前缀</param>
        /// <returns>最大ProductId</returns>
        string GetMaxProductId(string preSeqStr);

        /// <summary>
        /// 更新最大ProductId (Product)
        /// </summary>
        /// <param name="smtMO">MO</param>
        /// <param name="Product">最大Product</param>
        void SetMaxProductId(string smtMO, IProduct Product);

        /// <summary>
        /// 获取与指定mb绑定的Product
        /// </summary>
        /// <param name="mbSn"></param>
        /// <returns></returns>
        IProduct GetProductByMBSn(string mbSn);

        /// <summary>
        /// 更新一组Product的过站状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="ProductIDList"></param>
        void UpdateProductListStatus(ProductStatus status, IList<string> ProductIDList);

        /// <summary>
        /// 更新一组Product的DN
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="productIDList"></param>
        void BindDN(string dn, IList<string> productIDList);

        /// <summary>
        /// 更新一组Product的DN
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="productIDList"></param>
        /// <param name="dnQty"></param>
        /// <returns></returns>
        bool BindDN(string dn, IList<string> productIDList, int dnQty);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isThrow"></param>
        /// <param name="dn"></param>
        /// <param name="dnQty"></param>
        /// <returns>是否满了</returns>
        bool CheckResultOfBindDN_OnTrans(InvokeBody isThrow, string dn, int dnQty);

        /// <summary>
        /// 更新一组Product的Pallet
        /// </summary>
        /// <param name="Pallet"></param>
        /// <param name="ProductIDList"></param>
        void BindPallet(string Pallet, IList<string> ProductIDList);

        /// <summary>
        /// 更新一组Product的CartonSN
        /// </summary>
        /// <param name="CartonSN"></param>
        /// <param name="ProductIDList"></param>
        void BindCarton(string CartonSN, IList<string> ProductIDList);

        /// <summary>
        /// 记录一组Product的ProductLog
        /// </summary>
        /// <param name="NewLog"></param>
        /// <param name="ProductIDList"></param>
        void WriteProductListLog(IList<ProductLog> NewLogs);//, IList<string> ProductIDList);

        /// <summary>
        /// 晚加载Product Parts
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillProductParts(IProduct pd);

        /// <summary>
        /// 晚加载QCStatus
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillQCStatuses(IProduct pd);

        /// <summary>
        /// 晚加载TestLog
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillTestLogs(IProduct pd);

        /// <summary>
        /// 晚加载Repair
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillRepairs(IProduct pd);

        /// <summary>
        /// 晚加载Status
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillStatus(IProduct pd);

        /// <summary>
        /// 晚加载Model对象
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillModelObj(IProduct pd);

        /// <summary>
        /// 晚加载Log
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillLogs(IProduct pd);

        /// <summary>
        /// 晚加载ChangeLog
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillChangeLogs(IProduct pd);

        /// <summary>
        /// 晚加载ProductInfo
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillProductInfoes(IProduct pd);

        /// <summary>
        /// Lazy load of product attributes
        /// </summary>
        /// <param name="product"></param>
        void FillProductAttributes(IProduct product);

        /// <summary>
        /// Lazy load of product attribute logs
        /// </summary>
        /// <param name="product"></param>
        void FillProductAttributeLogs(IProduct product);

        /// <summary>
        /// 晚加载RepairDefectInfo
        /// </summary>
        /// <param name="rep"></param>
        /// <returns></returns>
        Repair FillRepairDefectInfo(Repair rep);

        /// <summary>
        /// 晚加载TestLogDefectInfo
        /// </summary>
        /// <param name="testLog"></param>
        /// <returns></returns>
        TestLog FillTestLogDefectInfo(TestLog testLog);

        /// <summary>
        /// 晚加载Pizza
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        IProduct FillPizza(IProduct pd);

        #region . For CommonIntf  .

        /// <summary>
        /// 根据MO获得Product Id Range列表
        /// </summary>
        /// <param name="MOId"></param>
        /// <returns></returns>
        IList<ProdIdRangeInfo> GetProdIdRangeList(string MOId);

        /// <summary>
        /// 根据Product Id获得Repair信息
        /// </summary>
        /// <param name="ProdId"></param>
        /// <returns></returns>
        IList<RepairInfo> GetProdRepairLogList(string ProdId);

        /// <summary>
        /// 获得Product信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IMES.DataModel.ProductInfo GetProductInfo(string productId);

        /// <summary>
        /// 根据customerSn获得Product信息
        /// </summary>
        /// <param name="customerSn"></param>
        /// <returns></returns>
        IMES.DataModel.ProductInfo GetProductInfoByCustomSn(string customerSn);

        /// <summary>
        /// 根据Product Id获得ProductStatus
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ProductStatusInfo GetProductStatusInfo(string productId);

        /// <summary>
        /// Product表中能否找到pcb的记录
        /// </summary>
        /// <param name="pcbid"></param>
        /// <returns></returns>
        bool IfBindPCB(string pcbid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        IList<ProductInfo> GetProductInfoListByKeyAndValue(string infoType, string infoValue);

        /// <summary>
        /// select * from ProductInfo nolock where ProductID = @ProductID and InfoType in ('SN','P/N', 'Key', 'hash')
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="itemTypes"></param>
        /// <returns></returns>
        IList<ProductInfo> GetProductInfoList(string proid, IList<string> itemTypes);

        #endregion

        ////暂不需要实现
        ////在PrintLog中查找
        ////Start ProdId和End ProdId在BegNo和EndNo中范围内,且MO满足1的条件
        ////1:MO.Prt_Qty>0 and MO.Status='H' and MO.SAPStatus=''
        //bool CheckTravelCardReprint(string startId, string endId);

        /// <summary>
        /// 在WipBuffer表中是否存在Code = @DM+’-’+@family 的纪录
        /// @family =Model.FamilyID，若有空格，只取空格前字串
        /// @DM= ModelInfo. Value where Model=(model) and Name= DM2(二楼)/DM(三楼)
        /// </summary>
        /// <param name="family"></param>
        /// <param name="model"></param>
        /// <param name="floor"></param>
        /// <returns></returns>
        bool CheckKitting(string family, string model, string floor);

        /// <summary>
        /// 检查输入的cpuVendorSn是否已经和product绑定
        /// 用于CombineproductCPU之前检查CPU是否已与其他product绑定;
        /// 检查product表的CVSN栏位如果等于输入的cpuVendorSn存在返回和它绑定的productSNO
        /// 不存在返回""
        /// select top 1 ProductID from Product where CVSN=@CVSN
        /// </summary>
        /// <param name="cpuVendorSn"></param>
        /// <returns></returns>
        string IsUsedCvsn(string cpuVendorSn);

        ///// <summary>
        ///// 在ProductStatus表里根据Station找到ProductId的列表
        ///// </summary>
        ///// <param name="station"></param>
        ///// <returns></returns>
        //IList<string> GetProductIdByCurrentStation(string station);

        /// <summary>
        /// 从Product_Part表根据PartNo和Value取多条记录
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IList<IProductPart> GetProductPartsByPartNoAndValue(string partNo, string val);

        /// <summary>
        /// 取得所输入PR SN（条件：Product_Part.BomNodeType=’PS’ and Product_Part.PartSn=@prsn）绑定的Product的ID（Product_Part.ProductID)
        /// </summary>
        /// <param name="bomNodeType"></param>
        /// <param name="partSn"></param>
        /// <returns></returns>
        IList<IProductPart> GetProductPartsByBomNodeTypeAndPartSn(string bomNodeType, string partSn);

        /// <summary>
        /// 调用SP: kittingBind
        /// </summary>
        /// <param name="mo"></param>
        /// <param name="proid"></param>
        /// <param name="boxid"></param>
        /// <param name="pdline"></param>
        void BindKitting(string mo, string proid, string boxid, string pdline);

        /// <summary>
        /// for 030 Query
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IList<ModelPassQty> GetModelPassQty(string line, string station, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 根据Carton号码获取所有属于改Carton的ProductID
        /// </summary>
        /// <param name="cartonSN"></param>
        /// <returns></returns>
        List<string> GetProductIDListByCarton(string cartonSN);

        /// <summary>
        /// 根据Carton号码更新所有属于该Carton的Product的CartonWeight
        /// </summary>
        /// <param name="cartonSN"></param>
        /// <param name="cartonWeight"></param>
        void UpdateCartonWeightByCarton(string cartonSN, decimal cartonWeight);

        /// <summary>
        /// 根据Carton号码更新所有属于该Carton的Product状态
        /// </summary>
        /// <param name="cartonSN"></param>
        /// <param name="newStatus"></param>
        void UpdateProductStatusByCarton(string cartonSN, ProductStatus newStatus);

        /// <summary>
        /// 根据Carton号码记录所有属于该Carton的Product的过站Log
        /// </summary>
        /// <param name="cartonSN"></param>
        /// <param name="newLog"></param>
        void WriteProductLogByCarton(string cartonSN, ProductLog newLog);

        /// <summary>
        /// 根据DeliveryNo号码记录所有属于该DeliveryNo的Product的过站Log
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="newLog"></param>
        void WriteProductLogByDeliveryNo(string dn, ProductLog newLog);

        /// <summary>
        /// 根据Customer SN获得Product对象
        /// </summary>
        /// <param name="customerSn"></param>
        /// <returns></returns>
        IProduct GetProductByCustomSn(string customerSn);

        /// <summary>
        /// 根据PalletNo获取所有属于该Pallet的CartonSN的重量之和
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        decimal GetAllCartonWeightByPallet(string palletNo);

        /// <summary>
        /// 根据deliveryNo获取所有属于该deliveryNo的Product的数量
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        int GetCombinedQtyByDN(string deliveryNo);

        /// <summary>
        /// 根据deliveryNo获取所有属于该deliveryNo結合棧板的Product的数量
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        int GetCombinedPalletQtyByDN(string deliveryNo);

        /// <summary>
        /// 根据palletNo获取所有属于该palletNo的Product
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<ProductModel> GetProductListByPalletNo(string palletNo);

        /// <summary>
        /// 根据起止号码获得ProductID的列表
        /// </summary>
        /// <param name="begNo"></param>
        /// <param name="endNo"></param>
        /// <returns></returns>
        IList<string> GetProductIdsByRange(string begNo, string endNo);

        /// <summary>
        /// select * from ProductStatus a join Product b on a.ProductId= b.ProductId where b.MO=MO
        /// </summary>
        /// <param name="mo"></param>
        /// <returns></returns>
        IList<IMES.DataModel.ProductStatus> GetProductFromProductStatus(string mo);

        /// <summary>
        /// 1.获得Product对象
        /// </summary>
        /// <param name="productIDOrCustSN">输入的是ProductID 或者CustSN</param>
        /// <returns></returns>
        IProduct FindOneProductWithProductIDOrCustSN(string productIDOrCustSN);

        /// <summary>
        /// 2.获得Product对象
        /// </summary>
        /// <param name="productIDOrCustSNOrCarton">输入的是ProductID 或者CustSN或者Carton</param>
        /// <returns></returns>
        IProduct FindOneProductWithProductIDOrCustSNOrCarton(string productIDOrCustSNOrCarton);

        /// <summary>
        /// 3.获得Product对象
        /// </summary>
        /// <param name="productIDOrCustSNOrPallet">输入的是ProductID 或者CustSN或者Pallet</param>
        /// <returns></returns>
        IProduct FindOneProductWithProductIDOrCustSNOrPallet(string productIDOrCustSNOrPallet);

        /// <summary>
        /// 根據pcbID獲取與之綁定的Product列表
        /// </summary>
        /// <param name="pcbID"></param>
        /// <returns></returns>
        IList<IProduct> GetProductListByPCBID(string pcbID);

        /// <summary>
        /// 获取与指定pizzaID绑定的Product
        /// </summary>
        /// <param name="pizzaID"></param>
        /// <returns></returns>
        IProduct GetProductByPizzaID(string pizzaID);

        /// <summary>
        /// 获取属于该DN,Pallet的所有Product
        /// 要求Product表的Delivery，Pallet分别等于输入的dn，pallet
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<ProductModel> GetProductByDnPallet(string dn, string pallet);

        /// <summary>
        /// 根据Type和Code获得RunInTimeControl
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<RunInTimeControl> GetRunInTimeControlByTypeAndCode(string type, string code);

        /// <summary>
        /// ProductLog中该ProductLog 最后一条Log 的时间
        /// </summary>
        /// <param name="prodid"></param>
        /// <returns></returns>
        DateTime GetTheNewestTime(string prodid);

        /// <summary>
        /// ReworkRejectStation中是否存在指定条件的数据
        /// </summary>
        /// <param name="customer">customer</param>
        /// <param name="station">station</param>
        /// <param name="status">status</param>
        /// <returns>是否存在指定条件的数据</returns>
        bool ReworkReject(string customer, string station, StationStatus status);

        /// <summary>
        /// 1.Copy ProductStatus and Product and Product_Part and ProductInfo资料至Rework_ProductStatus, Rework_Product, Rework_Product_Part, Rework_ProductInfo
        /// 条件:1).Rework_xxx 表中的ReworkCode = @code,
        ///      2).ProductID=@productID
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="reworkCode"></param>
        void CopyProductToRework(string productId, string reworkCode);

        /// <summary>
        /// delete ProductInfo where ProductID=@proid and InfoType in @itemtypes
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="itemTypes"></param>
        void RemoveProductInfosByType(string proid, IList<string> itemTypes);

        /// <summary>
        /// delete IMES_FA..Product_Part where PartNo in
        /// ( select PartNo from IMES_GetData..Part where 
        ///   PartNo in 
        ///           (select PartNo from IMES_FA..Product_Part where ProductID=@proid) 
        ///   and 
        ///   PartType in 
        ///           (select PartType from PartCheck where Customer=@customer and PartType in ReleaseType范围)
        /// )
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="customer"></param>
        /// <param name="partTypes"></param>
        void RemoveProductPartsByPnAndType(string proid, string customer, IList<string> partTypes);

        /// <summary>
        /// update Product set @itemtypes='' where ProductID=@proid
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="fieldNames"></param>
        void ClearFieldsOfProductById(string proid, IList<string> fieldNames);

        /// <summary>
        /// 跟其他的DropDownList一样，要求有id和descr
        /// SELECT '' as Code
        /// UNION
        /// SELECT DISTINCT RTRIM(Code) as Code
        ///     FROM KittingCode 
        ///     WHERE [Type] = 'Kitting' 
        ///     ORDER BY Code
        /// </summary>
        /// <returns></returns>
        IList<KittingCodeInfo> GetKittingCodeList();

        /// <summary>
        /// 返回结构List
        /// 参数@Code
        /// SELECT RTRIM(b.Code) as Code, RTRIM(b.PartNo) as [Part No],
        ///        RTRIM(b.Tp) as [Type], CONVERT(int, b.LightNo) as [Light No],
        ///        '' as Scan
        ///     FROM KittingCode a, WipBuffer b
        ///     WHERE a.[Type] = 'Kitting'
        ///        AND a.Code = b.Code
        ///        AND b.Code = @Code
        ///     ORDER BY [Light No], [Part No], [Type]
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<LightBomInfo> GetWipBufferInfoListByKittingCode(string code);

        /// <summary>
        /// 如何判断查询得到的Kitting Code 是否在Kitting 表中存在
        /// 参数@Code 
        /// SELECT count(*) FROM IMES_FA..KittingCode WHERE Code = @Code AND Type = 'Kitting'
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        int GetCountOfKittingCodeByCode(string code);

        /// <summary>
        /// Get 料位信息 By [Code] and [Model]，返回结构List，跟2步骤结构一样
        /// 参数@Model，@Code
        /// SELECT DISTINCT RTRIM(b.Code) as Code, RTRIM(b.PartNo) as [Part No],
        ///        RTRIM(b.Tp) as [Type], CONVERT(int, b.LightNo) as [Light No],
        ///        '' as Scan
        ///     FROM IMES_GetData..MoBOM a, IMES_FA..WipBuffer b
        ///     WHERE a.Deviation = 1
        ///        AND a.PartNo = b.PartNo
        ///        AND b.Code = @Code
        ///        AND b.Tp = 'Kitting'
        ///        AND MO IN (SELECT MO FROM IMES_GetData..MO WHERE Model = @Model)
        ///     ORDER BY [Light No], [Part No], [Type]
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<LightBomInfo> GetWipBufferInfoListByKittingCodeAndModel(string code, string model);

        /// <summary>
        /// SELECT Sno, Model, Borrower, Lender, Returner, Accepter, Status, Bdate, Rdate 
        ///     FROM IMES_FA..BorrowLog
        ///     WHERE Status = @status
        ///     ORDER BY Model, Sno, Rdate DESC
        ///     Status为空取所有status的数据
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<BorrowLog> GetBorrowLogByStatus(string status);

        /// <summary>
        /// SELECT * FROM IMES_FA..BorrowLog WHERE Sno = @ProductID
        /// </summary>
        /// <param name="sno"></param>
        /// <returns></returns>
        IList<BorrowLog> GetBorrowLogBySno(string sno);

        /// <summary>
        /// Insert IMES_FA..BorrowLog
        /// </summary>
        /// <param name="item"></param>
        void AddBorrowLog(BorrowLog item);

        /// <summary>
        /// UPDATE [IMES_FA].[dbo].[BorrowLog] SET Returner = @Lender, Accepter = @Editor, Status = 'R', 
        //       Rdate = GETDATE()
        //       WHERE Sno = @MBSno
        /// </summary>
        /// <param name="item"></param>
        void UpdateBorrowLog(BorrowLog item);

        /// <summary>
        /// SELECT @SFGCustomizingSiteCode = ISNULL(InfoValue, '')
        ///     FROM SFGSite
        ///     WHERE InfoType = 'PN' AND InfoValue = SUBSTRING(@CustPN, 6, 1)
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        string GetSFGCustomizingSiteCode(string infoType, string infoValue);

        /// <summary>
        /// SQL:SELECT TSBPN, IECPN, Cdt, Editor FROM IMES_FA..SmallPartsUpload ORDER BY Cdt DESC
        /// </summary>
        /// <returns></returns>
        IList<SMALLPartsUploadInfo> QuerySmallPartsUploadInfo();

        /// <summary>
        /// IF EXISTS(SELECT * FROM IMES_FA..SmallPartsUpload WHERE TSBPN = @tsbpn AND IECPN = @iecpn)
        ///          RETURN
        /// ELSE 
        ///       IF EXISTS(SELECT * FROM IMES_FA..SmallPartsUpload WHERE IECPN = @iecpn AND TSBPN <> @tsbpn)
        ///              UPDATE SmallPartsUpload SET TSBPN = @tsbpn WHERE IECPN = @iecpn
        ///       ELSE 
        ///              INSERT INTO SmallPartsUpload VALUES(@tsbpn,@iecpn,GETDATE(),@editor)
        /// </summary>
        /// <param name="list"></param>
        void SaveSmallPartsUploadInfo(IList<SMALLPartsUploadInfo> list);

        /// <summary>
        /// 检查刷入的IECPN在数据库中是否存在 
        /// IF EXISTS(SELECT TSBPN FROM SmallPartsUpload WHERE IECPN = @IECPN)
        ///     PRINT '存在'
        /// ELSE
        ///     PRINT '不存在'
        /// </summary>
        /// <param name="iecPn">iecPn</param>
        /// <returns>成功执行返回 影响行数 >1,否则返回 0</returns>
        int CheckIECPn(string iecPn);

        /// <summary>
        /// 判断FruNo是否存在
        /// </summary>
        /// <param name="fruNo"></param>
        /// <returns></returns>
        bool CheckFruNo(string fruNo);

        /// <summary>
        /// 根据ReworkCode到ReworkProcess获取Process
        /// 根据currentStatio和status判断是否是最后一站
        ///  当处理到Rework Process最后一站时(在rework process中按照当前站及Status找不到下一站)
        /// </summary>
        /// <param name="reworkcode"></param>
        /// <param name="currentStation"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool IsLastReworkStation(string reworkcode, string currentStation, int status);

        /// <summary>
        /// 根据reworkcode统计product数量，如果是0表示全部完成，返回true，否则返回false           
        /// select count(productid) from ProductStatus where ReworkCode=@ReworkCode and ProductId != @ProductId
        /// </summary>
        /// <param name="reworkcode"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool IsReworkFinish(string reworkcode, string productID);

        //update Rework set Status=@Status,editor=@editor udt=getdate() where Reworkcode=@ReworkCode and not exists(select ProductID FROM ProductStatus where ReworkCode=@ReworkCode and ProductID<>@ProductID)
        void UpdateReworkConsideredProductStatus(Rework rework, string productID);

        /// <summary>
        /// 更新一组Product对应的Rewrok和Status
        /// </summary>
        /// <param name="newStatus"></param>
        /// <param name="productIDList"></param>
        /// <param name="cartonSN"></param>
        void UpdateProductListRework(ProductStatus newStatus, IList<string> productIDList, string cartonSN);

        BoxerBookData GetNewBoxerBookDataByCustSN(string custSN);

        /// <summary>
        /// SQL:Update BoxerBookData Set PalletSerialNo = @PalletNo where SerialNumber=@CustomerSn
        /// </summary>
        /// <param name="pltNo"></param>
        /// <param name="custSN"></param>
        void UpdateBoxerBookData(string pltNo, string custSN);

        void DeleteProductPartByPartType(string productID, IList<string> partTypeList);

        /// <summary>
        /// 用Customer Sn = TestBoxDataLog.SerialNo 为条件，去检查Cdt 最新的那条记录
        /// </summary>
        /// <param name="custSN"></param>
        /// <returns></returns>
        TestBoxDataLog GetTestBoxDataLogByCustSN(string custSN);

        /// <summary>
        /// Update TestBoxDataLog Set PalletSerialNo = @PalletNo where SerialNumber=@CustomerSn
        /// </summary>
        /// <param name="pltno"></param>
        /// <param name="custSN"></param>
        void UpdateTestBoxDataLog(string pltno, string custSN);

        #region . Defered  .

        void SetMaxProductIdDefered(IUnitOfWork uow, string smtMO, IProduct product);

        void UpdateProductListStatusDefered(IUnitOfWork uow, ProductStatus status, IList<string> productIDList);

        void BindDNDefered(IUnitOfWork uow, string dn, IList<string> productIDList);

        InvokeBody BindDNDefered(IUnitOfWork uow, string dn, IList<string> productIDList, int dnQty);

        InvokeBody CheckResultOfBindDN_OnTransDefered(IUnitOfWork uow, InvokeBody isThrow, string dn, int dnQty);

        void BindPalletDefered(IUnitOfWork uow, string pallet, IList<string> productIDList);

        void BindCartonDefered(IUnitOfWork uow, string cartonSN, IList<string> productIDList);

        void WriteProductListLogDefered(IUnitOfWork uow, IList<ProductLog> newLogs);//, IList<string> ProductIDList);

        void BindKittingDefered(IUnitOfWork uow, string mo, string proid, string boxid, string pdline);

        /// <summary>
        /// 根据Carton号码更新所有属于该Carton的Product的CartonWeight
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="cartonSN"></param>
        /// <param name="cartonWeight"></param>
        void UpdateCartonWeightByCartonDefered(IUnitOfWork uow, string cartonSN, decimal cartonWeight);

        /// <summary>
        /// 根据Carton号码更新所有属于该Carton的Product状态
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="cartonSN"></param>
        /// <param name="newStatus"></param>
        void UpdateProductStatusByCartonDefered(IUnitOfWork uow, string cartonSN, ProductStatus newStatus);

        /// <summary>
        /// 根据Carton号码记录所有属于该Carton的Product的过站Log
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="cartonSN"></param>
        /// <param name="newLog"></param>
        void WriteProductLogByCartonDefered(IUnitOfWork uow, string cartonSN, ProductLog newLog);

        /// <summary>
        /// 根据DeliveryNo号码记录所有属于该DeliveryNo的Product的过站Log
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="dn"></param>
        /// <param name="newLog"></param>
        void WriteProductLogByDeliveryNoDefered(IUnitOfWork uow, string dn, ProductLog newLog);

        void CopyProductToReworkDefered(IUnitOfWork uow, string productId, string reworkCode);

        void RemoveProductInfosByTypeDefered(IUnitOfWork uow, string proid, IList<string> itemTypes);

        void RemoveProductPartsByPnAndTypeDefered(IUnitOfWork uow, string proid, string customer, IList<string> partTypes);

        void ClearFieldsOfProductByIdDefered(IUnitOfWork uow, string proid, IList<string> fieldNames);

        void AddBorrowLogDefered(IUnitOfWork uow, BorrowLog item);

        void UpdateBorrowLogDefered(IUnitOfWork uow, BorrowLog item);

        /// <summary>
        /// 根据Carton号码更新所有属于该Carton的Product的DeliverID，CartonSN为空
        /// </summary>
        /// <param name="cartonSN"></param>
        void CartonUnpack(string cartonSN);

        /// <summary>
        /// 根据Carton号码更新所有属于该Carton的Product的DeliverID，CartonSN为空
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="cartonSN"></param>
        void CartonUnpackDefered(IUnitOfWork uow, string cartonSN);

        void SaveSmallPartsUploadInfoDefered(IUnitOfWork uow, IList<SMALLPartsUploadInfo> list);

        void UpdateReworkConsideredProductStatusDefered(IUnitOfWork uow, Rework rework, string productID);

        /// <summary>
        /// 更新一组Product对应的Rewrok和Status
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="newStatus"></param>
        /// <param name="productIDList"></param>
        /// <param name="cartonSN"></param>
        void UpdateProductListReworkDefered(IUnitOfWork uow, ProductStatus newStatus, IList<string> productIDList, string cartonSN);

        /// <summary>
        /// 删除指定Product对应P
        /// </summary>
        /// <param name="uow">unit of work</param>
        /// <param name="productID">productID</param>
        /// <param name="partTypeList">partTypeList</param>
        void DeleteProductPartByPartTypeDefered(IUnitOfWork uow, string productID, IList<string> partTypeList);

        void UpdateBoxerBookDataDefered(IUnitOfWork uow, string pltNo, string custSN);

        void UpdateTestBoxDataLogDefered(IUnitOfWork uow, string pltno, string custSN);

        #endregion

        #region For Maintain

        /// <summary>
        /// select a.ProductID,a.CUSTSN,a.Model,a.MO,a.DeliveryNo,b.Station,b.Status 
        /// from Product a
        /// join ProductStatus b on a.ProductID=b.ProductID 
        /// where a.ProductID = 'productIdOrSN' or a.CUSTSN = 'productIdOrSN' 
        /// </summary>
        /// <param name="productIdOrSN"></param>
        /// <returns></returns>
        IProduct GetProductByIdOrSn(string productIdOrSN);

        /// <summary>
        /// 取得Product List,按ProdId排序
        /// 参考Sql
        /// select b.ProductID,b.CUSTSN,b.Model,b.MO,b.DeliveryNo,c.Station,c.Status 
        /// from IMES_GetData.dbo.TempProductID a 
        ///      join IMES_FA.dbo.Product b on a.ProductID = b.ProductID OR a.ProductID = b.CUSTSN
        ///      join IMES_FA.dbo.ProductStatus c on c.ProductID=b.ProductID 
        /// where a.UserKey=?
        /// order by b.ProductID 
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        IList<IProduct> GetProductListByByUserKey(string userKey);

        /// <summary>
        /// 取得不合法的ProductID List 
        /// 参考sql
        /// select a.ProductID  
        /// from IMES_GetData.dbo.TempProductID a 
        ///      left outer join IMES_FA.dbo.Product b on a.ProductID = b.ProductID  
        ///      left outer join IMES_FA.dbo.Product c on a.ProductID = c.CUSTSN  
        /// where a.UserKey=? and b.ProductID is null and c.ProductID is null 
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        IList<string> GetAllInvalidProductIdByUserKey(string userKey);

        /// <summary>
        /// 所有unit都没有在其他任何Rework流程中，即不存在以下情况：Unit存在Rework_Product表且ReworkCode对应的Rework.Status<>3
        /// 参考sql
        /// select count(1) 
        /// from IMES_GetData.dbo.TempProductID a 
        ///      join IMES_FA.dbo.ProductStatus b on a.ProductID = b.ProductID  
        ///      join IMES_FA.dbo.Rework c on b.ReworkCode=c.ReworkCode 
        /// where c.Status<>3 and a.UserKey=?
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        int GetUnitExistsCountByUserKey(string userKey);

        /// <summary>
        /// 所有unit所处的站可以做rework，即Unit所在的站满足以下条件：Product.Station and Status不存在于ReworkRejectStation表
        /// 参考sql
        /// select count(1) 
        /// from IMES_GetData.dbo.TempProductID a 
        ///      join IMES_FA.dbo.ProductStatus b on a.ProductID = b.ProductID  
        ///      join IMES_GetData.dbo.ReworkRejectStation c on b.Station =c.Station and b.Status=c.Status  
        /// where a.UserKey=?
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        int GetInvalidUnitCountByUserKey(string userKey);

        /// <summary>
        /// 取得Rework下的所有Product
        /// 参考sql
        /// select a.ProductID,a.CUSTSN,a.Model,a.MO,a.DeliveryNo,b.Station,b.Status 
        /// from IMES_FA.dbo.Product a
        ///      join IMES_FA.dbo.ProductStatus b on a.ProductID=b.ProductID 
        /// where b.ReworkCode=? 
        /// order by b.ProductID 
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        DataTable GetProductListByReworkCode(string reworkCode);

        /// <summary>
        /// 取得Rework下的Product，按Model分组
        /// 参考sql
        /// select a.Model,count(1) as Quantity  
        /// from IMES_FA.dbo.Product a
        ///      join IMES_FA.dbo.ProductStatus b on a.ProductID=b.ProductID 
        /// where b.ReworkCode=?
        /// group by a.Model 
        /// order by a.Model 
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        IList<ModelAndCount> GetProductModelStatisticByReworkCode(string reworkCode);

        /// <summary>
        /// 取得Rework下的Product，按Station分组
        /// 参考sql
        /// select b.Station+’(’+c.Descr+’)’,count(1) as Quantity  
        /// from IMES_FA.dbo.Product a
        ///      join IMES_FA.dbo.ProductStatus b on a.ProductID=b.ProductID 
        ///      join IMES_GetData.dbo.Station c on b.Station=c.Station 
        /// where b.ReworkCode=? 
        /// group by b.Station 
        /// order by b.Station 
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        IList<StationAndCount> GetProductStationStatisticByReworkCode(string reworkCode);

        /// <summary>
        /// 取得Rework下的unit的数量
        /// 参考sql
        /// select count(1) 
        /// from IMES_FA.dbo.ProductStatus 
        /// where b.ReworkCode=? 
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        int GetUnitCountByRework(string reworkCode);

        /// <summary>
        /// 保存reworkCode到ProductStatus 
        /// 参考sql
        /// update b 
        /// set b.ReworkCode=? 
        /// from IMES_GetData.dbo.TempProductID a 
        ///      join IMES_FA.dbo.ProductStatus b on a.ProductID = b.ProductID  
        /// where a.UserKey=?
        /// </summary>
        /// <param name="userKey"></param>
        /// <param name="reworkCode"></param>
        void UpdateProductStatusReworkCodeByUserKey(string userKey, ReworkObj reworkCode);

        /// <summary>
        /// 将ProductId的List存入临时表中，每个productId为一条记录。 
        /// 临时表名称TempProductID，包含两个字段ProductID char(10),UserKey char(32)
        /// 参考sql
        /// insert ProductIDListForRework(UserKey,ProductID) values(?,?)
        /// </summary>
        /// <param name="productIdList"></param>
        /// <param name="userKey"></param>
        void CreateTempProductIDList(IList<string> productIdList, string userKey);

        /// <summary>
        /// 删除掉临时表中ProductID
        /// 参考sql
        /// delete ProductIDListForRework where UserKey=?
        /// </summary>
        /// <param name="userKey"></param>
        void DeleteProductIDListByUserKey(string userKey);

        /// <summary>
        /// 创建Rework返回ReworkCode
        /// 参考sql
        /// insert Rework values('rewokcode',rework.Status,rework.Editor,rework.Cdt,rework.Udt)
        /// Rework Code产生规则：
        /// YYMMDDXX
        /// 第1-2码：两位年
        /// 第3-4码：两位月
        /// 第5-6码：两位日
        /// 第7-8码：Sequence No.，从01开始，36进制(0-9,A-Z)
        /// </summary>
        /// <param name="rework"></param>
        /// <returns></returns>
        string CreateRework(Rework rework);

        /// <summary>
        /// 取得Date From和Date To给定时期内修改了的所有非Finished的Rework信息，并按Update Date的倒序排列
        /// 参考sql
        /// select a.ReworkCode,
        ///        (select count(1) from Product_Status c where c.ReworkCode=a.ReworkCode) as Qty,
        ///        (case a.Status when ‘0’ then ‘Create’ when ‘1’ then ‘Submit’ when ‘2’ then ‘Confirm’ when ‘3’ then ‘Finish’ end),
        ///        a.Editor,
        ///        a.Cdt,
        ///        a.Udt,
        ///        b.Process 
        /// from IMES_FA.dbo.Rework a 
        ///      left outer join IMES_GetData.dbo.Rework_Process b on a.ReowrkCode=b.ReworkCode 
        /// where a.Udt >= ? and a.Udt <= ? and a.Status <> '3' 
        /// order by Udt desc 
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        DataTable GetReworkList(DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// 取得Rework的Status
        /// 参考sql
        /// select Status from Rework where ReworkCode=? 
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        string GetReworkStatus(string reworkCode);

        /// <summary>
        /// 删除Rework记录
        /// 参考sql
        /// a)update ProductStatus set ReworkCode='' where ReworkCode=? 
        /// b)delete Rework_Process where ReworkCode=? 
        /// c)delete Rework where ReworkCode=? 
        /// </summary>
        /// <param name="reworkCode"></param>
        void RemoveARework(string reworkCode);

        /// <summary>
        /// 更新Rework
        /// 参考sql
        /// Update Rework set Status=?,Editor=?,Udt=? where ReworkCode=? 
        /// </summary>
        /// <param name="rework"></param>
        void UpdateRework(Rework rework);

        /// <summary>
        /// 设置Rework的Process
        /// 参考sql
        /// a)delete Rework_Process where ReworkCode=? 
        /// b)insert Rework_Process(ReworkCode,Process,Editor,Cdt,Udt) values(?,?,?,?,?)
        /// </summary>
        /// <param name="reworkProcess"></param>
        void SetReworkProcess(ReworkProcess reworkProcess);

        /// <summary>
        /// 取得相关Process对应的所有Release Type 
        /// 参考sql
        /// select ReleaseType 
        /// from Rework_Process a
        ///      join Rework_ReleaseType b on a.Process=b.Process 
        /// where a.ReworkCode=? 
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        IList<string> GetProcessReleaseType(string reworkCode);

        /// <summary>
        /// select b.PreStation+’(’+c. Descr +’)’,b.Station+’(’+d. Descr +’)’,b.Status
        /// from Rework_Process a
        ///      join Process_Station b on a.Process = b.Process  
        ///      join Station c on b.PreStation=c.Station  
        ///      join Station d on b.Station=d.Station
        /// where a.ReworkCode=?
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        IList<ProcessStationInfo> GetProcessStationList(string reworkCode);

        /// <summary>
        /// select * from Rework_Process where ReworkCode = ?
        /// 存在返回true ，不存在返回false
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <returns></returns>
        bool IFReworkHasProcess(string reworkCode);

        /// <summary>
        /// 按ID列表批量获得Product对象列表
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        IList<IProduct> GetProductListByIdList(IList<string> idList);

        /// <summary>
        /// 数据复制与解绑，
        /// 参考sql：
        /// Exec ClearProductDataForRework ‘reworkCdoe’,’editor’
        /// </summary>
        /// <param name="reworkCode"></param>
        /// <param name="editor"></param>
        void ClearData(string reworkCode, string editor);

        /// <summary>
        /// SELECT RTRIM(Code), RTRIM(Descr) as [Description] FROM KittingCode
        ///          WHERE [Type] = 'Kitting'
        ///          ORDER BY Code, Descr
        /// </summary>
        /// <returns></returns>
        DataTable GetTabKittingCodeList();

        /// <summary>
        /// SELECT DISTINCT RTRIM(Code), RTRIM(PartNo) as [Part No], RTRIM([Tp]) as [Type], 
        ///           CONVERT(int, LightNo) as LightNo,
        ///           Qty, RTRIM(Sub) as Substitution, Safety_Stock as [Safety Stock], 
        ///           Max_Stock as [Max Stock],
        ///           Remark, Editor, Cdt as [Create Date], Udt as [Update Date], [ID]
        ///          FROM WipBuffer b
        ///          WHERE Code = @Code
        ///          ORDER BY CONVERT(int, LightNo), [Part No]
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetLightNoListByCode(string code);

        /// <summary>
        /// SELECT * FROM IMES_GetData..Part WHERE PartNo = @PartNo AND Flag=1
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        DataTable GetExistPartNo(string partNo);

        /// <summary>
        /// 说明：@Code等是item中对应的项的参数
        /// IF EXISTS(SELECT * FROM WipBuffer WHERE Code = @Code AND PartNo = @PartNo)
        ///          UPDATE [IMES_FA].[dbo].[WipBuffer]
        ///                    SET LightNo = @LightNo, Qty = @Qty, Sub = @Sub, Safety_Stock = @SafetyStock,
        ///                             Max_Stock = @MaxStock, Remark = @Remark, Editor = @Editor, Udt = GETDATE()
        ///                    WHERE Code = @Code AND PartNo = @PartNo
        /// ELSE
        ///          INSERT INTO [IMES_FA].[dbo].[WipBuffer]([Code],[PartNo],[Tp],[LightNo],[Picture],[Qty],[Sub],[Safety_Stock],[Max_Stock],[Remark],[Editor],[Cdt],[Udt])
        ///                    SELECT @Code, @PartNo, PartType, @LightNo, '', @Qty, @Substitution, @SafetyStock, @MaxStock,
        ///                             @Remark, @Editor, GETDATE(), GETDATE() 
        ///                             FROM IMES_GetData..Part
        ///                             WHERE PartNo = @PartNo AND Flag=1
        /// </summary>
        /// <param name="item"></param>
        void SaveWipBuffer(WipBuffer item);

        /// <summary>
        /// DELETE FROM [IMES_FA].[dbo].[WipBuffer]
        ///          WHERE Code = @Code AND LightNo = @LightNo
        /// </summary>
        /// <param name="code"></param>
        /// <param name="lightNo"></param>
        void DeleteLightNo(string code, string lightNo);

        /// <summary>
        /// SELECT RTRIM(Code) as Code, RTRIM([Type]) as Type, RTRIM(Descr) as Description, RTRIM(Remark) as Remark, RTRIM(Editor) as Editor, Cdt as [Create Date], Udt as [Update Date]
        ///          FROM KittingCode WHERE [Type]='Kitting' 
        ///          ORDER BY Code, Description
        /// </summary>
        /// <returns></returns>
        DataTable GetLightStationList();

        /// <summary>
        /// IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting')
        ///          UPDATE [IMES_FA].[dbo].[KittingCode]
        ///                    SET Descr = @Description,
        ///                             Remark = @Remark,
        ///                             Editor = @Remark,
        ///                             Udt = GETDATE()
        ///          WHERE Code = @Code AND [Type] = 'Kitting'
        /// ELSE
        ///          INSERT INTO [IMES_FA].[dbo].[KittingCode]([Code],[Type],[Descr],[Remark],[Editor],[Cdt],[Udt])
        ///                    VALUES(@Code, 'Kitting', @Description, @Remark, @Editor, GETDATE(), GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void SaveKittingCode(KittingCode item);

        /// <summary>
        /// DELETE FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting'
        /// </summary>
        /// <param name="code"></param>
        void DeleteKittingCode(string code);

        /// <summary>
        /// SELECT [ID] FROM WipBuffer WHERE Code = @Code AND PartNo = @PartNo
        /// </summary>
        /// <param name="code"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        DataTable GetWipBufferID(string code, string partNo);

        /// <summary>
        /// SELECT DISTINCT Area as Area
        ///          FROM TraceStd
        ///          ORDER BY Area
        /// </summary>
        /// <returns></returns>
        DataTable GetAreaItem();

        /// <summary>
        /// SELECT DISTINCT Area as Area
        ///          FROM TraceStd
        ///          ORDER BY Area
        /// </summary>
        /// <returns></returns>
        IList<AreaDef> GetAreaList();

        /// <summary>
        ///1,从表RunInTimeControl取得Family列表 
        ///参考sql：
        ///SELECT Code AS Family 
        ///FROM RunInTimeControl
        ///         WHERE Type = 'Family'
        ///         ORDER BY Family 
        /// </summary>
        /// <returns></returns>
        IList<string> GetFamilyListFromRunInTimeControl();

        /// <summary>
        ///2,根据Type取得RunInTimeControl列表
        ///参考sql：
        ///SELECT ID,Code, [Type], [Hour], Remark,Editor, Cdt, Udt 
        ///FROM RunInTimeControl
        ///         WHERE Type = ? 
        ///         ORDER BY Code 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<RunInTimeControl> GetRunInTimeControlListByType(string type);

        /// <summary>
        ///3,根据Type和Code取得RunInTimeControl 
        ///参考sql：
        ///SELECT ID,Code, [Type], [Hour], Remark,Editor, Cdt, Udt  
        ///FROM RunInTimeControl
        ///         WHERE Type = ?
        ///                   AND Code = ? 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        RunInTimeControl GetRunInTimeControl(string type, string code);

        /// <summary>
        ///4,更新RunInTimeControl
        ///参考sql：
        ///UPDATE RunInTimeControl 
        ///SET Hour = ?, Remark = ?, Editor = ?, Udt = GETDATE()
        ///WHERE [Type] = ? AND Code = ? 
        /// </summary>
        /// <param name="runInTimeControl"></param>
        void UpdateRunInTimeControlByTypeAndCode(RunInTimeControl runInTimeControl);

        /// <summary>
        /// UPDATE RunInTimeControl 
        /// SET Code = @Family, [Type] = @type, Hour = @Time, Editor = @Editor, Udt = GETDATE(), TestStation = @station, ControlType = @ctrlType 
        /// WHERE [ID] = @id
        /// </summary>
        /// <param name="runInTimeControl"></param>
        void UpdateRunInTimeControlById(RunInTimeControl runInTimeControl);

        /// <summary>
        ///5,纪录log信息
        ///参考sql：
        ///INSERT RunInTimeControlLog(Code, [Type], [Hour], Remark, Editor, TestStation, ControlType, Cdt)
        ///                   SELECT Code, [Type], Hour, Remark, Editor, TestStation, ControlType, Udt
        ///                            FROM RunInTimeControl
        ///                            WHERE WHERE [ID] = @id
        /// </summary>
        /// <param name="id"></param>
        void InsertRunInTimeControlLog(int id);

        /// <summary>
        ///6,新增RunInTimeControl纪录
        ///参考sql：
        ///INSERT RunInTimeControl(Code, [Type], [Hour], Remark, Editor, Cdt, Udt)
        ///                   VALUES(?, ?, ?, ?, ?, GETDATE(), GETDATE()) 
        /// </summary>
        /// <param name="runInTimeControl"></param>
        void InsertRunInTimeControl(RunInTimeControl runInTimeControl);

        /// <summary>
        ///7,删除RunInTimeControl纪录
        ///参考sql：
        ///DELETE FROM RunInTimeControl WHERE [Type] = ? AND Code = ?
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        void DeleteRunInTimeControl(string type, string code);

        /// <summary>
        /// DELETE FROM [iMES2012].[dbo].[RunInTimeControl] WHERE [ID] = @id
        /// </summary>
        /// <param name="id"></param>
        void DeleteRunInTimeControlById(int id);

        /// <summary>
        /// 2.返回值和参数说明:
        /// 泛型参数TraceStdInfo为IMES.DataModel中的可序列化的结构,其内部包括Family, Area, Type, Editor, CreateDate(即Cdt)的成员.
        /// 参数为字符串类型,对应数据库和SQL语句中的同名字段参数(如果等同于TraceStdInfo这个结构或类已经存在的话,请您直接使用.)
        /// SQL语句:
        /// IF @Family <> '' AND @Area <> ''
        ///          SELECT Family, Area as Area, Type as Type, Editor, Cdt as [Create Date], Udt, Id
        ///                    FROM TraceStd 
        ///                    WHERE Family = @Family
        ///                      AND Area = @Area
        ///                    ORDER BY Family, Area
        /// IF @Family <> '' AND @Area = ''
        ///          SELECT Family, Area as Area, Type as Type, Editor, Cdt as [Create Date], Udt, Id
        ///                    FROM TraceStd 
        ///                    WHERE Family = @Family
        ///                    ORDER BY Family, Area
        /// IF @Family = '' AND @Area <> ''
        ///          SELECT Family, Area as Area, Type as Type, Editor, Cdt as [Create Date], Udt, Id
        ///                    FROM TraceStd 
        ///                    WHERE Area = @Area
        ///                    ORDER BY Family, Area
        /// IF @Family = '' AND @Area = ''
        ///          SELECT Family, Area as Area, Type as Type, Editor, Cdt as [Create Date], Udt, Id
        ///                    FROM TraceStd 
        ///                    ORDER BY Family, Area 
        /// </summary>
        /// <param name="family"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        IList<TraceStdInfo> GetTraceStdList(string family, string area);

        /// <summary>
        /// 3.返回值和参数说明:
        /// TraceStdInfo为IMES.DataModel中可序列化结构,前文中曾作为返回值的泛型参数出现过,本方法中只提取此结构中的4个成员作为SQL语句中的参数
        /// SQL语句:
        /// IF NOT EXISTS(SELECT * FROM [IMES_FA].[dbo].[TraceStd]
        ///                    WHERE Family = @Family AND Area = @Area AND Type = @Type)
        ///          INSERT INTO [IMES_FA].[dbo].[TraceStd]([Family],[Area],[Type],[Editor],[Cdt])
        ///                    VALUES(@Family, @Area, @Type, @Editor, GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void SaveAllKindsOfTypeInfo(TraceStdInfo item);

        /// <summary>
        /// 4.返回值和参数说明:
        /// TraceStdInfo为IMES.DataModel中可序列化结构,前文中曾作为返回值的泛型参数出现过,本方法中只提取此结构中的3个成员作为SQL语句中的参数
        /// SQL语句:
        /// DELETE FROM [IMES_FA].[dbo].[TraceStd]
        ///          WHERE Family = @Family AND Area = @Area AND Type = @Type
        /// </summary>
        /// <param name="item"></param>
        void DeleteResult(TraceStdInfo item);

        /// <summary>
        /// 5.返回值和参数说明:
        /// TraceStdInfo为IMES.DataModel中可序列化结构,前文中曾作为返回值的泛型参数出现过,本方法中提取此结构中的4个成员作为SQL语句中的参数
        /// Id为TraceStd表中新增加的主键
        /// SQL语句:
        /// SELECT  Id 
        /// FROM [IMES_FA].[dbo].[TraceStd]
        /// WHERE Family = @Family AND Area = @Area AND Type = @Type)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int GetId(TraceStdInfo item);

        /// <summary>
        /// 取得全部的WLBTDescr 
        /// 参考sql：
        /// SELECT *
        /// FROM WLBTDescr
        /// ORDER BY Code, Site, Tp
        /// </summary>
        /// <returns></returns>
        IList<WLBTDescr> GetAllWLBTDescr();

        /// <summary>
        /// 取得某一Part的WLBTDescr 
        /// 参考sql：
        /// SELECT *
        /// FROM WLBTDescr
        /// WHERE Code = ?
        /// ORDER BY Code, Site, Tp
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        IList<WLBTDescr> GetWLBTDescrListByPartNo(string partNo);

        /// <summary>
        /// 判断WLBTDescr是否已经存在 
        /// 参考sql：
        /// SELECT ID 
        /// FROM [WLBTDescr] 
        /// WHERE Code = ? AND Tp = ? AND Site = ? AND TpDescr = ?
        /// </summary>
        /// <param name="descr"></param>
        /// <returns></returns>
        int IFWLBTDescrIsExists(WLBTDescr descr);

        /// <summary>
        /// 更新WLBTDescr
        /// 参考sql：
        /// UPDATE [WLBTDescr] 
        /// SET Descr = ?, Editor = ?, Udt = GETDATE()
        /// WHERE ID = ?
        /// </summary>
        /// <param name="descr"></param>
        void UpdateWLBTDescr(WLBTDescr descr);

        /// <summary>
        /// 新增WLBTDescr纪录, 返回ID  
        /// 参考sql：
        /// INSERT INTO [WLBTDescr]([Code],[Tp],[TpDescr],[Descr],[Site],[Editor],[Cdt],[Udt])
        ///         VALUES(?, ?, ?, ?, ?, ?, GETDATE(), GETDATE())
        /// </summary>
        /// <param name="descr"></param>
        void InsertWLBTDescr(WLBTDescr descr);

        /// <summary>
        /// 删除WLBTDescr
        /// 参考sql：
        /// Delete WLBTDescr where ID=?
        /// </summary>
        /// <param name="id"></param>
        void DeleteWLBTDescr(string id);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值；
        /// 参数为IMES.DataModel.TraceStdInfo
        /// SQL语句：
        /// IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[TraceStd]
        ///                    WHERE Family = @Family AND Area = @Area)
        /// UPDATE [IMES_FA].[dbo].[TraceStd]
        /// SET [Type]= @Type,[Editor]=@ Editor,[Udt]=GETDATE()
        /// WHERE Family = @Family AND Area = @Area 
        /// </summary>
        /// <param name="item"></param>
        void UpdateAllKindsOfTypeInfo(TraceStdInfo item);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为Int类型；
        /// 参数为两个字符串类型，对应数据库中TraceStd表中的Family和Area字段；
        /// SQL语句：
        /// SELECT COUNT(*) FROM [IMES_FA].[dbo].[TraceStd]
        /// WHERE Family = @Family AND Area = @Area AND Type=@Type
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int CheckExistsRecord(TraceStdInfo item);

        /// <summary>
        /// IF EXISTS(SELECT * FROM [IMES_FA].[dbo].[KittingCode] WHERE Code = @Code AND [Type] = 'Kitting')
        /// 返回true
        /// 否则false
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool IsKittingCodeExist(string code);

        /// <summary>
        /// UPDATE [IMES_FA].[dbo].[KittingCode]
        /// SET Descr = @Description,Remark = @Remark, Editor = @Editor,Udt = GETDATE()
        /// WHERE Code = @Code AND [Type] = 'Kitting'
        /// </summary>
        /// <param name="code"></param>
        /// <param name="descr"></param>
        /// <param name="remark"></param>
        /// <param name="editor"></param>
        void UpdateKittingCode(string code, string descr, string remark, string editor);

        /// <summary>
        /// INSERT INTO [IMES_FA].[dbo].[KittingCode]
        /// ([Code],[Type],[Descr],[Remark],[Editor],[Cdt],[Udt]) VALUES(@Code, 'Kitting', 
        /// @Description, @Remark, @Editor, GETDATE(), GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void AddKittingCode(KittingCode item);

        /// <summary>
        /// IF EXISTS(SELECT * FROM WipBuffer WHERE Code = @Code AND PartNo = @PartNo)
        /// 返回true
        /// 否则false
        /// </summary>
        /// <param name="code"></param>
        /// <param name="partNo"></param>
        /// <returns></returns>
        bool IsWipBufferExist(string code, string partNo);

        /// <summary>
        /// UPDATE [IMES_FA].[dbo].[WipBuffer]
        /// SET LightNo = @LightNo, Qty = @Qty, Sub = @Sub, Safety_Stock = @SafetyStock,
        ///     Max_Stock = @MaxStock, Remark = @Remark, Editor = @Editor, Udt = GETDATE()
        /// WHERE ID=id
        /// </summary>
        /// <param name="item"></param>
        void UpdateWipBuffer(WipBuffer item);

        /// <summary>
        /// INSERT INTO [IMES_FA].[dbo].[WipBuffer]([Code],[PartNo],[Tp],[LightNo],[Picture],[Qty],
        /// [Sub],[Safety_Stock],[Max_Stock],[Remark],[Editor],[Cdt],[Udt])
        /// 添加数据后，item中的ID存入该条记录新生成的ID
        /// </summary>
        /// <param name="item"></param>
        void AddWipBuffer(WipBuffer item);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为字符串为泛型参数的列表,字符串对应的是FA_Station表中的Line
        /// SQL语句：
        /// SELECT DISTINCT [Line]
        /// FROM [IMES_GetData].[dbo].[Line]
        /// </summary>
        /// <returns></returns>
        IList<string> GetLineList();

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为FaStationInfo为泛型参数的列表;
        /// SQL语句：
        /// SELECT [ID]
        ///       ,[Line]
        ///       ,[Station]
        ///       ,[OptCode]
        ///       ,[OptName]
        ///       ,[Remark]
        ///       ,[Editor]
        ///       ,[Cdt]
        ///       ,[Udt]
        ///   FROM [IMES_FA_Datamaintain].[dbo].[FA_Station]
        ///   ORDER BY [Line]
        /// </summary>
        /// <returns></returns>
        IList<FaStationInfo> GetFaStationInfoList();

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为FaStationInfo为泛型参数的列表;
        /// 参数为字符串类型,对应表中的[Line]字段
        /// SQL语句:
        /// SELECT [ID]
        ///       ,[Line]
        ///       ,[Station]
        ///       ,[OptCode]
        ///       ,[OptName]
        ///       ,[Remark]
        ///       ,[Editor]
        ///       ,[Cdt]
        ///       ,[Udt]
        ///   FROM [IMES_FA_Datamaintain].[dbo].[FA_Station]
        ///   WHERE [Line]=@Line
        ///   ORDER BY [Line]
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IList<FaStationInfo> GetFaStationInfoList(string line);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为int型,表示存在符合条件的记录的数目
        /// 参数为字符串类型,对应表中的[Line] [Station]字段
        /// SQL语句:
        /// SELECT COUNT(*)
        ///   FROM [IMES_FA_Datamaintain].[dbo].[FA_Station]
        ///   WHERE [Line]=@Line
        ///         AND
        ///         [Station]=@Station
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        int CheckExistsRecord(string line, string station);

        /// <summary>
        /// 返回值和参数说明：
        /// 返回值为int类型，对应数据库FA_Station表中的主键ID
        /// 参数为字符串类型,分别对应表中的[Line] [Station]字段
        /// SQL语句:
        /// SELECT [ID]
        ///   FROM [IMES_FA_Datamaintain].[dbo].[FA_Station]
        ///   WHERE [Line]=@Line 
        ///         AND
        ///         [Station]=@Station
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        int GetID(string line, string station);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值
        /// 参数为FaStationInfo结构类型
        /// SQL语句：
        /// UPDATE [IMES_FA_Datamaintain].[dbo].[FA_Station]
        ///    SET [OptCode] = @OptCode
        ///       ,[OptName] = @OptName
        ///       ,[Remark] =  @Remark
        ///       ,[Editor] = @Editor
        ///       ,[Udt] = GETDATE()
        ///  WHERE [Line] = @Line
        ///       AND [Station] = @Station
        /// </summary>
        /// <param name="item"></param>
        void UpdateFaStation(FaStationInfo item);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值
        /// 参数为FaStationInfo结构类型
        /// SQL语句：
        /// INSERT INTO [IMES_FA_Datamaintain].[dbo].[FA_Station]
        ///            ([Line]
        ///            ,[Station]
        ///            ,[OptCode]
        ///            ,[OptName]
        ///            ,[Remark]
        ///            ,[Editor]
        ///            ,[Cdt]
        ///            ,[Udt])
        ///      VALUES
        ///            ( @Line
        ///               ,@Station
        ///               ,@OtpCode
        ///               ,@OptName
        ///               ,@Remark
        ///               ,@Editor
        ///               ,GETDATE()
        ///               ,GETDATE())
        /// </summary>
        /// <param name="item"></param>
        void InsertFaStation(FaStationInfo item);

        /// <summary>
        /// 返回值和参数说明：
        /// 无返回值；
        /// 参数为两个字符串类型，分别对应数据库中FA_Station表中的[Line]和[Station]字段；
        /// SQL语句：
        /// DELETE FROM [IMES_FA_Datamaintain].[dbo].[FA_Station]
        ///       WHERE [Line]=@Line AND [Station]=@Station
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        void DeleteFaStationInfo(string line, string station);

        /// <summary>
        /// 1,查询所有的MasterLabel的信息:
        /// </summary>
        /// <returns></returns>
        IList<MasterLabelInfo> GetAllMasterLabels();

        /// <summary>
        /// SQL: select * from MasterLabel;
        /// 2,删除所选中的一条记录
        /// </summary>
        /// <param name="id"></param>
        void RemoveMasterLabelItem(int id);

        /// <summary>
        /// 3,根据vc,family查找DB中匹配的记录
        /// SQL:select * from masterLabel where family=[family] and vc=[vc]
        /// </summary>
        /// <param name="vc"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<MasterLabelInfo> GetMasterLabelByVCAndCode(string vc, string family);

        /// <summary>
        /// MasterLabel (Condition: MasterLabel.VC in @Items and MasterLabel.Family=@Family)
        /// </summary>
        /// <param name="vcs"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<MasterLabelInfo> GetMasterLabelByVCAndCode(string[] vcs, string family);

        /// <summary>
        /// SQL:select * from masterLabel where vc=[vc]
        /// </summary>
        /// <param name="vc"></param>
        /// <returns></returns>
        IList<MasterLabelInfo> GetMasterLabelByVC(string vc);

        /// <summary>
        /// SQL:select * from MasterLabel where family=[family]
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<MasterLabelInfo> GetMasterLabelByCode(string family);

        /// <summary>
        /// 4,添加
        /// </summary>
        /// <param name="ml"></param>
        void AddMasterLabelItem(MasterLabelInfo ml);

        /// <summary>
        /// 5,更新
        /// 根据vc,family 更新MasterLabelInfo中的各个字段.
        /// </summary>
        /// <param name="ml"></param>
        /// <param name="vc"></param>
        /// <param name="family"></param>
        void UpdateMasterLabelItem(MasterLabelInfo ml,string vc,string family);

        /// <summary>
        /// 1、取得IqcPnoBom表中的Vendor值，对应的SQL为：select distinct upper(Vendor) from IqcPnoBom where Descr like 'MEM%' and Vendor<>'' order by upper(Vendor)
        /// </summary>
        /// <param name="descr"></param>
        /// <returns></returns>
        IList<string> GetVendorsByLikeDescr(string descr);

        /// <summary>
        /// 2、 取得VendorCode数据的list(按Vendor、Priority栏位排序) 【对应SupplierCode表】
        /// </summary>
        /// <returns></returns>
        IList<SupplierCodeInfo> GetAllSupplierCodeList();

        /// <summary>
        /// 3、根据code取得VendorCode的记录数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<SupplierCodeInfo> GetSupplierCodeListByCode(string code);

        /// <summary>
        /// 4、根据Vendor和Idex取得VendorCode的记录数据
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="idex"></param>
        /// <returns></returns>
        IList<SupplierCodeInfo> GetSupplierCodeListByCode(string vendor, string idex);

        /// <summary>
        /// 5、添加VendorCode记录
        /// </summary>
        /// <param name="item"></param>
        void AddSupplierCodeInfo(SupplierCodeInfo item);

        /// <summary>
        /// 6、根据id删除VendorCode记录
        /// </summary>
        /// <param name="id"></param>
        void RemoveSupplierCodeInfo(int id);

        /// <summary>
        /// 7、根据id查找一条VendorCode记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SupplierCodeInfo FindSupplierCodeInfo(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runInTimeControl"></param>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="testStation"></param>
        void UpdateRunInTimeControlByTypeCodeAndTestStation(IMES.FisObject.FA.Product.RunInTimeControl runInTimeControl, string type, string code, string testStation);

        #region . LJK banned .
        ////a)insert ReworkProduct(ReworkCode,ProductID,Model,PCBID,PCBModel,MAC,UUID,MBECR,CVSN,CUSTSN,ECR,BIOS,IMGVER,WMAC,IMEI,MEID,ICCID,COAID,PizzaID,MO,UnitWeight,CartonSN,CartonWeight,DeliveryNo,PalletNo,HDVD,BLMAC,TVTuner)
        ////select b.ReworkCode,a.ProductID,a.Model,a.PCBID,a.PCBModel,a.MAC,a.UUID,a.MBECR,a.CVSN,a.CUSTSN,a.ECR,a.BIOS,a.IMGVER,a.WMAC,a.IMEI,a.MEID,a.ICCID,a.COAID,a.PizzaID,a.MO,a.UnitWeight,a.CartonSN,a.CartonWeight,a.DeliveryNo,a.PalletNo,a.HDVD,a.BLMAC,a.TVTuner
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=?
        ////
        ////b)insert Rework_ProductStatus
        ////(ReworkCode,ProductID,Station,Status,Line,Editor,Cdt,Udt)
        ////select ReworkCode,
        ////ProductID,Station,Status,Line,Editor,Cdt,Udt
        ////from ProductStatus 
        ////where ReworkCode=?
        ////
        ////c)insert Rework_Product_Part
        ////(ReworkCode,ProductID,PartNo,Value,ValueType,Station,Editor,Cdt,Udt)
        ////select b.ReworkCode,a.ProductID,a.PartNo,a.Value,a.ValueType,a.Station,a.Editor,a.Cdt,a.Udt
        ////from Product_Part a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=? 
        ////
        ////d)insert Rework_ProductInfo(
        ////ReworkCode,ProductID,InfoType,InfoValue,Editor,Cdt,Udt)
        ////select b.ReworkCode,a.ProductID,a.InfoType,a.InfoValue,a.Editor,a.Cdt,a.Udt
        ////from ProductInfo a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=?  
        ////
        ////e)update ProductStatus 
        ////  set Station='RW', Status=1 
        ////  where ReworkCode=? 
        ////
        ////f)insert ProductLog(ProductID,Model,Station,Status,Line,Editor,Cdt)
        ////select a.ProductID,a.Model,b.Station,b.Status,b.Line,?, getdate()  
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=? 
        ////
        ////g)update Process_Station 
        ////set Process_Station.PreStation='RW' 
        ////from Process_Station
        ////     join Rework_Process on Process_Station.Process=Rework_Process.Process
        ////where Process_Station.PreStation='' and Rework_Process.ReworkCode=? 
        //void CopyProductInfoToReworkTable(string reworkCode,string editor);

        ////解绑数据
        ////参考sql：
        ////update a 
        ////set a.UnitWeight = '' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=?  
        //void ClearData_BWEIGHT(string reworkCode);

        ////参考sql：
        ////update a 
        ////set a.CartonSN = ''  
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=?  
        //void ClearData_SN(string reworkCode);

        ////参考sql：
        ////update c 
        ////set c.Status='A2' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////     join IMES_PAK.dbo.COAStatus c on c.COASN=a.COAID  
        ////where b.ReworkCode=?  

        ////insert COALog
        ////(COASN,Station,Line,Editor,Cdt)
        ////select 
        //// a.COAID,
        //// 'RW',
        //// b.Line,
        //// ?,
        //// getdate()
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=?  
        ////
        ////update a 
        ////set a.COAID='' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=? 
        //void ClearData_COA(string reworkCode, string editor);

        ////参考sql：
        ////update a 
        ////set a.CUSTSN='' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=? 
        //void ClearData_CPQSNO(string reworkCode);

        ////参考sql：
        ////update c 
        ////set c.Status='00' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////     join IMES_PAK.dbo.Delivery c on c.DeliveryNo=a.DeliveryNo 
        ////where b.ReworkCode=?  
        ////   
        ////update a 
        ////set a.DeliveryNo='' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=? 
        //void ClearData_Delivery(string reworkCode);

        ////参考sql：
        ////delete c 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////     join IMES_PAK.dbo.Pizza_Part c on a.PizzaID=c.PizzaID 
        ////where b.ReworkCode=?  
        ////
        ////delete c 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////     join IMES_PAK.dbo.PizzaStatus c on a.PizzaID=c.PizzaID 
        ////where b.ReworkCode=?  
        ////
        ////update a 
        ////set a.PizzaID='' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=? 
        //void ClearData_KITID(string reworkCode);

        ////参考sql：
        ////update c  
        ////set c.Station=30  
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////     join IMES_PCA.dbo.PCBStatus c on a.PCBID=c.PCBNo  
        ////where b.ReworkCode=?  
        ////
        ////insert IMES_PCA.dbo.PCBLog(
        ////PCBNo,PCBModel,Staion,Status,Line,Editor,Cdt)
        ////select a.PCBID,a.PCBModel,'30',1,b.Line,?,getDdate()   
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=?  
        ////
        ////update a 
        ////set a.PCBID='', 
        ////    a.PCBModel='',
        ////    a.MBECR='',
        ////    a.MAC='',
        ////    a.UUID='',
        ////    a.CVSN='' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=?  
        //void ClearData_MB(string reworkCode, string editor);

        ////参考sql：
        ////update c 
        ////set MMIID='' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////     join IMES_PAK.dbo.Pizza c on a.PizzaID=c.PizzaID 
        ////where b.ReworkCode=?  
        //void ClearData_MMI(string reworkCode);

        ////参考sql：
        ////update a 
        ////set a.PalletNo='' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=? 
        //void ClearData_PLT(string reworkCode);

        ////参考sql：
        ////update a 
        ////set a.CartonWeight='' 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=? 
        //void ClearData_WEIGHT(string reworkCode);

        ////参考sql：
        ////delete c 
        ////from IMES_FA.dbo.Product a 
        ////     join IMES_FA.dbo.ProductStatus b on a.ProductID=b.ProductID
        ////     join IMES_FA.dbo.Product_Part c on c.ProductID=b.ProductID 
        ////     join IMES_GetData.dbo.Part d on d.PartNo=c.PartNo 
        ////     join 
        ////          (select ReleaseType from IMES_GetData.dbo.Rework_Process e  
        ////                        join IMES_GetData.dbo.Rework_ReleaseType f on e.Process = f.Process 
        ////                       join IMES_GetData.dbo.PartCheck g on g.PartType=f.ReleaseType 
        ////                          where e.ReworkCode=?) tempview 
        ////           on tempview.ReleaseType = d.PartType  
        ////where b.ReworkCode=?
        //void ClearData_PartCheck(string reworkCode);

        ////参考sql:
        ////select ReleaseType from IMES_GetData.dbo.Rework_Process e  
        ////                        join IMES_GetData.dbo.Rework_ReleaseType f on e.Process = f.Process 
        ////                       join IMES_GetData.dbo.PartCheck g on g.PartType=f.ReleaseType 
        ////                          where e.ReworkCode=? 
        //IList<string> GetPartCheckList(string reworkCode);

        ////参考sql:
        ////针对List中的每一个字符串作以下操作
        ////select ItemType from CheckItem where ItemName=? 
        ////如果未检索到ItemType，继续下一个item。检索到结果继续向下操作
        ////如果检索到的ItemType为0
        ////首先判断Item是否是Product表的一个栏位，如果是，将这个栏位置空，
        ////update a 
        ////set a.item=''
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////where b.ReworkCode=?  
        ////如果检索到的ItemType为1
        ////删除掉Product_Part中PartType等于item的数据
        ////delete d 
        ////from Product a
        ////     join ProductStatus b on a.ProductID=b.ProductID 
        ////     join IMES_FA.dbo.Product_Part c on c.ProductID=b.ProductID 
        ////     join IMES_GetData.dbo.Part d on d.PartNo=c.PartNo 
        ////where b.ReworkCode=? and d.PartType=?
        //void ClearData_CheckItem(IList<string> item, string reworkCode);
        #endregion

        #region . Defered  .

        void UpdateProductStatusReworkCodeByUserKeyDefered(IUnitOfWork uow, string userKey, ReworkObj reworkCode);

        void CreateTempProductIDListDefered(IUnitOfWork uow, IList<string> productIdList, string userkey);

        void DeleteProductIDListByUserKeyDefered(IUnitOfWork uow, string userKey);

        void CreateReworkDefered(IUnitOfWork uow, Rework rework);

        void RemoveAReworkDefered(IUnitOfWork uow, string reworkCode);

        void UpdateReworkDefered(IUnitOfWork uow, Rework rework);

        void SetReworkProcessDefered(IUnitOfWork uow, ReworkProcess reworkProcess);

        void ClearDataDefered(IUnitOfWork uow, string reworkCode, string editor);

        void SaveWipBufferDefered(IUnitOfWork uow, WipBuffer item);

        void DeleteLightNoDefered(IUnitOfWork uow, string code, string lightNo);

        void SaveKittingCodeDefered(IUnitOfWork uow, KittingCode item);

        void DeleteKittingCodeDefered(IUnitOfWork uow, string code);

        void UpdateRunInTimeControlByTypeAndCodeDefered(IUnitOfWork uow, RunInTimeControl runInTimeControl);

        void UpdateRunInTimeControlByIdDefered(IUnitOfWork uow, RunInTimeControl runInTimeControl);

        void InsertRunInTimeControlLogDefered(IUnitOfWork uow, int id);

        void InsertRunInTimeControlDefered(IUnitOfWork uow, RunInTimeControl runInTimeControl);

        void DeleteRunInTimeControlDefered(IUnitOfWork uow, string type, string code);

        void DeleteRunInTimeControlByIdDefered(IUnitOfWork uow, int id);

        void SaveAllKindsOfTypeInfoDefered(IUnitOfWork uow, TraceStdInfo item);

        void DeleteResultDefered(IUnitOfWork uow, TraceStdInfo item);

        void UpdateWLBTDescrDefered(IUnitOfWork uow, WLBTDescr descr);

        void InsertWLBTDescrDefered(IUnitOfWork uow, WLBTDescr descr);

        void DeleteWLBTDescrDefered(IUnitOfWork uow, string id);

        void UpdateAllKindsOfTypeInfoDefered(IUnitOfWork uow, TraceStdInfo item);

        void UpdateKittingCodeDefered(IUnitOfWork uow, string code, string descr, string remark, string editor);

        void AddKittingCodeDefered(IUnitOfWork uow, KittingCode item);

        void UpdateWipBufferDefered(IUnitOfWork uow, WipBuffer item);

        void AddWipBufferDefered(IUnitOfWork uow, WipBuffer item);

        void UpdateFaStationDefered(IUnitOfWork uow, FaStationInfo item);

        void InsertFaStationDefered(IUnitOfWork uow, FaStationInfo item);

        void DeleteFaStationInfoDefered(IUnitOfWork uow, string line, string station);

        void RemoveMasterLabelItemDefered(IUnitOfWork uow, int id);

        void AddMasterLabelItemDefered(IUnitOfWork uow, MasterLabelInfo ml);

        void UpdateMasterLabelItemDefered(IUnitOfWork uow, MasterLabelInfo ml, string vc, string family);

        void AddSupplierCodeInfoDefered(IUnitOfWork uow, SupplierCodeInfo item);

        void RemoveSupplierCodeInfoDefered(IUnitOfWork uow, int id);

        void UpdateRunInTimeControlByTypeCodeAndTestStationDefered(IUnitOfWork uow, IMES.FisObject.FA.Product.RunInTimeControl runInTimeControl, string type, string code, string testStation);

        #region . LJK banned .

        //void CopyProductInfoToReworkTableDefered(IUnitOfWork uow, string reworkCode, string editor);

        //void ClearData_BWEIGHTDefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_SNDefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_COADefered(IUnitOfWork uow, string reworkCode, string editor);

        //void ClearData_CPQSNODefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_DeliveryDefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_KITIDDefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_MBDefered(IUnitOfWork uow, string reworkCode, string editor);

        //void ClearData_MMIDefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_PLTDefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_WEIGHTDefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_PartCheckDefered(IUnitOfWork uow, string reworkCode);

        //void ClearData_CheckItemDefered(IUnitOfWork uow, IList<string> item, string reworkCode);

        #endregion

        #endregion

        #endregion

        #region Lucy Liu Added

        /// <summary>
        /// 返回值和参数说明：
        /// IList<string>
        /// 参数为两个字符串类型，分别对应数据库中ProductRepair表中的[ProductID]和[Status]字段；
        /// SQL语句：
        /// SELECT * FROM [IMES_FA].[dbo].[ProductRepair]
        ///       WHERE [ProductID]=@ProductID AND [Status]=@Status
        /// </summary>
        /// <param name="pordId"></param>
        /// <param name="status"></param>
        IList<string> GetProductRepairByProIdAndStatus(string pordId, int status);

        /// <summary>
        /// 返回值和参数说明：
        /// void
        /// 参数为1个字符串类型，分别对应数据库中Product_Part表中的[ProductID]字段；
        /// SQL语句：
        /// DELETE FROM [IMES_FA].[dbo].[Product_Part]
        ///       WHERE [ProductID]=@ProductID 
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="proID"></param>
        void DeleteProductPartByProIdDefered(IUnitOfWork uow, string proID);

        /// <summary>
        /// 返回值和参数说明：
        /// void
        /// 参数为1个字符串类型，分别对应数据库中ProductInfo表中的[ProductID]字段；
        /// SQL语句：
        /// DELETE FROM [IMES_FA].[dbo].[ProductInfo]
        ///       WHERE [ProductID]=@ProductID 
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="proID"></param>
        void DeleteProductInfoByProIdDefered(IUnitOfWork uow, string proID);

        /// <summary>
        /// 将符合条件的Product资料拷贝至UnpackProduct, UnpackProductStatus
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="cartonSN"></param>
        /// <param name="palletNo"></param>
        /// <param name="dn"></param>
        /// <param name="productId"></param>
        ///  /// <param name="editor"></param>
        void CopyProductToUnpackDefered(IUnitOfWork uow, string cartonSN, string palletNo, string dn, string productId, string editor);

        void WriteProductLogByPalletNo(string palletNo, IMES.FisObject.FA.Product.ProductLog newLog);

        /// <summary>
        /// 根据Pallet号码获取所有属于该Pallet的ProductID
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        List<string> GetProductIDListByPalletNo(string palletNo);

        /// <summary>
        /// 根据Pallet号码记录所有属于该Pallet的Product的过站Log
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="palletNo"></param>
        /// <param name="newLog"></param>
        void WriteProductLogByPalletNoDefered(IUnitOfWork uow, string palletNo, ProductLog newLog);

        /// <summary>
        /// 根据Pallet号码更新所有属于该Pallet的Product的PalletNo为空
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="palletNo"></param>        
        void PalletUnpackDefered(IUnitOfWork uow, string palletNo);

        /// <summary>
        /// 根据Carton号码更新所有属于该Carton的Product的DeliveryNo为空
        /// </summary>
        void DNUnpackDefered(IUnitOfWork uow, string cartonSN);

        /// <summary>
        /// 根据DeliveryNo号码更新所有属于该DeliveryNo的Product的DeliveryNo,PalletNo都置为空
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="dn"></param> 
        void DNUnpackForPODataDefered(IUnitOfWork uow, string dn);

        /// <summary>
        /// 根据SerialNumber更新所有属于该条件记录的CartonNo字段
        /// </summary>
        void UpdateBoxerBookDataListDefered(IUnitOfWork uow, BoxerBookData data, IList<string> productCustSNList);

        /// <summary>
        /// 根据SerialNumber更新所有属于该条件记录的CartonNo字段
        /// </summary>
        void UpdateTestBoxDataLogListDefered(IUnitOfWork uow, TestBoxDataLog data, IList<string> productCustSNList);
        void UpdateTestBoxDataLogList(TestBoxDataLog data, IList<string> productCustSNList);

        /// <summary>
        /// Update TestBoxDataLog Set PalletSerialNo = ‘’ where SerialNumber=@CustomerSn
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="custSN"></param> 
        void UpdateTestBoxDataLogForUnpackPalletDefered(IUnitOfWork uow, string custSN);

        /// <summary>
        /// Update TestBoxDataLog Set CartonSn = ‘’ where SerialNumber=@CustomerSn
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="custSN"></param> 
        void UpdateTestBoxDataLogForUnpackCartonDefered(IUnitOfWork uow, string custSN);


        /// <summary>
        /// 根据DeliveryNo号码获取所有属于该DeliveryNo的Product对象
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<IProduct> GetProductListByDeliveryNo(string dn);

        /// <summary>
        /// 根据Carton号码获取所有属于改Carton的CustSn
        /// </summary>
        /// <param name="cartonSN"></param>
        /// <returns></returns>
        List<string> GetCustSnListByCarton(string cartonSN);

        /// <summary>
        /// 根据Pallet号码获取所有属于改Pallet的ProductID
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        List<string> GetCustSnListByPalletNo(string palletNo);

        /// <summary>
        /// 根据SerialNumber更新所有属于该条件记录的CartonNo字段=''(Unpack carton)
        /// </summary>
        void UpdateTestBoxDataLogListForUnpackCartonDefered(IUnitOfWork uow, IList<string> productCustSNList);
        void UpdateTestBoxDataLogListForUnpackCarton(IList<string> productCustSNList);

        /// <summary>
        /// 根据SerialNumber更新所有属于该条件记录的PalletSerialNo字段=''(Unpack pallet)
        /// </summary>
        void UpdateTestBoxDataLogListForUnpackPalletDefered(IUnitOfWork uow, IList<string> productCustSNList);
        void UpdateTestBoxDataLogListForUnpackPallet(IList<string> productCustSNList);
        #endregion

        /// <summary>
        /// 2、在ProductLog不存在Station='81' and Status=1 and Line= 'ATSN Print'的记录
        /// </summary>
        /// <param name="station"></param>
        /// <param name="status"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        IList<ProductLog> GetProductLogs(string station, int status, string line);

        IList<ProductLog> GetProductLogs(string station, int status, string line, string prodId);

        /// <summary>
        /// 3、Product_Part.Value where ProductID=@prdid and PartNo in (bom中BomNodeType=’AT’对应的Pn) 
        /// </summary>
        /// <param name="partNos"></param>
        /// <param name="prodId"></param>
        /// <returns></returns>
        IList<IProductPart> GetProductPartsByPartNosAndProdId(string[] partNos, string prodId);

        /// <summary>
        /// 5、 保存product和Asset SN的绑定关系-- Insert Product_Part values(@prdid,@partpn,@astsn,’’,’AT’,@user,getdate(),getdate()) 
        /// </summary>
        void AddOneIProductPart(IProductPart item);

        /// <summary>
        /// 根据Delivery号码获取所有属于该Delivery的Product Object List
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<IProduct> GetProductObjListByDn(string dn);

        void DeleteProductInfo(string[] prodIds, string infoType);

        /// <summary>
        /// 取得所输入PR SN绑定的Product ID及其SN
        /// SELECT * FROM Product WHERE PRSN=@#PRSN
        /// </summary>
        /// <param name="prSn"></param>
        /// <returns></returns>
        IList<IProduct> GetProductByPrSn(string prSn);

        /// <summary>
        /// Update the pizzaId for Product Table Record.
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="pizzaId"></param>
        void UpdatePizzaIdForProduct(string proId, string pizzaId);

        /// <summary>
        /// SELECT * FROM FA.dbo.FA_SnoBTDet  WHERE SnoId=@ProductID
        /// </summary>
        /// <param name="productID"></param>
        FaSnobtdetInfo GetSnoBTDet(string productID);

        /// <summary>
        /// 1、 根据Type取得对应的Label Kitting Code数据的list(按Code栏位排序)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<LabelKittingCode> GetLabelKittingCodeListByType(string type);
        
        /// <summary>
        /// 2、根据code和Type取得Label KittingCode的记录数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>        
        /// <returns></returns>
        DataTable GetExistLabelKittingCode(string code, string type);

        /// <summary>
        /// 3、根据code和Type修改Label KittingCode记录
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="oldCode"></param>
        /// <param name="oldType"></param>
        void ChangeLabelKittingCode(LabelKittingCode obj, string oldCode, string oldType);

        /// <summary>
        /// 4、添加Label KittingCode
        /// </summary>
        /// <param name="obj"></param>
        void AddLabelKittingCode(LabelKittingCode obj);

        /// <summary>
        /// 5、根据code和Type删除Label KittingCode
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        void RemoveLabelKittingCode(string code, string type);

        /// <summary>
        /// 6、根据code和Type查找一条Label KittingCode记录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        LabelKittingCode FindLabelKittingCode(string code, string type);

        /// <summary>
        /// 当Asset Tag对应的Part.PartType=’ATSN7’ or ‘ATSN8’时，
        /// 需要看在Special_Det中是否存在对应的记录(Tp=’ATSN7’ or ‘ATSN8’ and SnoId=@prdid)，若存在，则Update Sno1=@astsn；
        /// </summary>
        /// <param name="tps"></param>
        /// <param name="snoId"></param>
        /// <returns></returns>
        IList<SpecialDetInfo> GetSpecialDetInfoByTpAndSnoId(string [] tps, string snoId);

        /// <summary>
        /// 则Update Sno1=@astsn；
        /// </summary>
        /// <param name="sno1"></param>
        /// <param name="ids"></param>
        void UpdateSno1ForSpecialDet(string sno1, int[] ids);

        /// <summary>
        /// 否则Insert(Tp分别是’ATSN7’ or ‘ATSN8’)
        /// </summary>
        /// <param name="item"></param>
        void InsertSpecialDet(SpecialDetInfo item);

        /// <summary>
        /// 1）取KittingCode表格
        /// 'FA', 'PAK'情况下
        /// select distinct(Left(rtrim(Line),1)) as Code, '' as Descr, 'Yes' as IsLine 
        /// from Line where Stage=@stage 
        /// union 
        /// select rtrim(Family) as Code, Descr, '' as IsLine 
        /// from Family order by Descr, Code
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        DataTable GetKittingCodeListFromLine(string stage);

        /// <summary>
        /// 2）取KittingCode表格
        /// 'FA Label','FA Label'情况下
        /// 
        ///         select rtrim(Code)
        ///         , Descr, 
        ///         '' as IsLine 
        ///         from LabelKitting (nolock) where Type=@type order by Descr
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetKittingCodeList(string type);

        /// <summary>
        /// 3)取PDLine列表
        /// 'FA', 'PAK'情况下
        /// SELECT [Line] AS pdLine where [Stage]='stage' ORDER BY [Line]
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        DataTable GetPdLineList(string stage);

        /// <summary>
        /// 3)取PDLine列表
        /// 'FA', 'PAK'情况下
        /// select distinct Left(rtrim(Line),1) as Code from Line (nolock) where Stage=@stage order by Code
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        DataTable GetPdLineListForLightNo(string stage);

        /// <summary>
        /// 4)取lightNo列表信息
        /// select distinct b.Code,b.PartNo,b.Tp,b.Station,convert(int,b.LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,b.Remark,b.Editor,b.Cdt,b.Udt,b.ID
        /// from (select PartType + "%" as PartType from IMES2012_GetData.dbo.KitLoc where PdLine = @code) a, IMES2012_FA.dbo.WipBuffer b where IsNull(b.Tp,'') like a.PartType and b.Code=@code and 
        /// b.KittingType = @kittingType order by convert(int,b.LightNo)
        /// </summary>
        /// <param name="kittingType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetLightNoList(string kittingType, string code);

        /// <summary>
        /// 5) 
        /// select distinct b.Code,b.PartNo,b.Tp,b.Station,convert(int,b.LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,b.Remark,b.Editor,b.Cdt,b.Udt,b.ID 
        /// from (select PartType + "%" as PartType from IMES2012_GetData.dbo.KitLoc where PdLine = @code) a, IMES2012_FA.dbo.WipBuffer b where IsNull(b.Tp,'') like a.PartType and b.Code=@code and 
        /// b.KittingType = @kittingType
        /// AND IsNull(Tp,'') not like 'DDD Kitting%' order by convert(int,b.LightNo)
        /// </summary>
        /// <param name="kittingType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetLightNoListPAK(string kittingType, string code);

        /// <summary>
        /// 6) 
        /// select distinct Code,PartNo,Tp,Station,convert(int,LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,Remark,Editor,Cdt,Udt,ID 
        /// from IMES2012_FA.dbo.WipBuffer where Code=@code and KittingType = @kittingType 
        /// order by convert(int,LightNo)
        /// </summary>
        /// <param name="kittingType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetLightNoListFami(string kittingType, string code);

        /// <summary>
        /// 7) 
        /// select distinct Code,PartNo,Tp,Station,convert(int,LightNo) as LightNo,Qty,Sub,Safety_Stock,Max_Stock,Remark,Editor,Cdt,Udt,ID 
        /// from IMES2012_FA.dbo.WipBuffer where Code=@code and KittingType = @kittingType 
        /// AND IsNull(Tp,'') not like 'DDD Kitting%' 
        /// order by convert(int,LightNo)
        /// </summary>
        /// <param name="kittingType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetLightNoListFamiPAK(string kittingType, string code);

        /// <summary>
        /// 8) 
        /// 新增的id返回到结构里
        /// </summary>
        /// <param name="item"></param>
        void AddLightNo(WipBuffer item);

        /// <summary>
        /// 9) 
        /// 更新的数据
        /// </summary>
        /// <param name="item"></param>
        void UpdateLightNo(WipBuffer item);

        /// <summary>
        /// 10) 
        /// DELETE FROM [WipBuffer]
        /// WHERE ID='id'
        /// </summary>
        /// <param name="id"></param>
        void DeleteLightNo(int id);

        /// <summary>
        /// 11) 
        /// SELECT Descr 
        /// FROM [Part]
        /// where [PartNo]='partNo'
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        DataTable GetPartInfoByPartNo(string partNo);

        /// <summary>
        /// 12) 
        /// SELECT [ID] 
        /// FROM [WipBuffer]
        /// where  Code='code' AND KittingType='kittingType' AND (PartNo='partNo')
        /// </summary>
        /// <param name="code"></param>
        /// <param name="partNo"></param>
        /// <param name="kittingType"></param>
        /// <returns></returns>
        bool ExistWipBuffer(string code, string partNo, string lightNo, string kittingType);

        /// <summary>
        /// SELECT [ID] 
        /// FROM [WipBuffer]
        /// where Code='code' AND KittingType='kittingType' AND (PartNo='partNo') AND isnull(Line,’’)='line'
        /// </summary>
        /// <param name="code"></param>
        /// <param name="partNo"></param>
        /// <param name="lightNo"></param>
        /// <param name="kittingType"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        bool ExistWipBuffer(string code, string partNo, string lightNo, string kittingType, string line);

        /// <summary>
        /// 14) SELECT [ID] FROM [WipBuffer] where Code='code' AND KittingType='kittingType' AND (PartNo='partNo') AND ID<>'id'
        /// </summary>
        /// <param name="code"></param>
        /// <param name="partNo"></param>
        /// <param name="id"></param>
        /// <param name="kittingType"></param>
        /// <returns></returns>
        bool ExistWipBufferExceptCode(string code, string partNo, string lightNo, int id, string kittingType);

        /// <summary>
        /// SELECT [ID] FROM [WipBuffer] where Code='code' AND KittingType='kittingType' AND (PartNo='partNo') AND isnull(Line,’’)='line' AND ID<>'id'
        /// </summary>
        /// <param name="code"></param>
        /// <param name="partNo"></param>
        /// <param name="lightNo"></param>
        /// <param name="id"></param>
        /// <param name="kittingType"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        bool ExistWipBufferExceptCode(string code, string partNo, string lightNo, int id, string kittingType, string line);

        /// <summary>
        /// 15) 
        /// SELECT distinct(PartType)
        /// FROM KitLoc
        /// ORDER BY PartType
        /// </summary>
        /// <returns></returns>
        DataTable GetLightNoPartType();

        /// <summary>
        /// 16)执行存储过程
        /// 在GataDate库中执行
        /// Exec op_KittingAutoCheck 'code','lightNo'
        /// 参数名称类型
        /// @family varchar(30),@loc char(4) 
        /// 返回存储过程中的select数据
        /// </summary>
        /// <param name="family"></param>
        /// <param name="loc"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        DataTable ExecOpKittingAutoCheck(string family, string loc, string line);

        /// <summary>
        /// 17) 1'
        /// DELETE FROM [TmpKit]
        /// WHERE PdLine='curLine' AND type='type'
        /// </summary>
        /// <param name="curLine"></param>
        /// <param name="type"></param>
        void DeleteTmpKit(string curLine, string type);

        /// <summary>
        /// 17) 2'
        /// INSERT INTO TmpKit
        /// (PdLine
        /// ,Model
        /// ,Type
        /// ,Qty)
        /// VALUES
        /// (@line
        /// ,@model
        /// ,@Type
        /// ,@Qty)
        /// </summary>
        /// <param name="items"></param>
        void ImportTmpKit(IList<TmpKitInfoDef> items);

        /// <summary>
        /// 19) 执行存储过程 For FA
        /// Exec op_KittingLocCheck @pdline,@type
        /// 返回存储过程中的select数据
        /// </summary>
        /// <param name="pdline"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable ExecOpKittingLocCheck(string pdline, string type);

        /// <summary>
        /// 20) 执行存储过程For PAK
        /// Exec op_PAKKitLoc_FV @pdline
        /// 返回存储过程中的select数据
        /// </summary>
        /// <param name="pdline"></param>
        /// <returns></returns>
        DataTable ExecOpPAKKitLocFV(string pdline);

        /// <summary>
        /// 1.返回所有KitLoc记录
        /// select * from KitLoc order by Family,PartType
        /// </summary>
        /// <returns></returns>
        IList<FAFloatLocationInfo> GetFAFloatLocationList();

        /// <summary>
        /// 2.
        /// select * from KitLoc where Family=family order by PartType
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        IList<FAFloatLocationInfo> GetFAFloatLocListByFamily(string family);

        /// <summary>
        /// 3.返回指定的KitLoc记录
        /// select * from KitLoc where ID=@id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FAFloatLocationInfo GetFAFloatLocationInfo(int id);

        /// <summary>
        /// 4.添加一条记录
        /// </summary>
        /// <param name="item"></param>
        void AddFAFloatLocationInfo(FAFloatLocationInfo item);

        /// <summary>
        /// 5.更新指定ID为[id]的记录 所有字段 where ID=id
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        void UpdateFAFloatLocationInfo(FAFloatLocationInfo item, int id);

        /// <summary>
        /// 6.删除指定ID为[id]的记录
        /// </summary>
        /// <param name="id"></param>
        void DeleteFAFloatLocationInfo(int id);

        /// <summary>
        /// 2,SQL:select * from PAKitLoc where pnline=[pdline] order by partNo
        /// </summary>
        /// <param name="pdline"></param>
        /// <returns></returns>
        IList<PAKitLoc> GetPAKitlocByPdLine(string pdline);

        /// <summary>
        /// 3,
        /// </summary>
        /// <param name="item"></param>
        void AddPAKitLoc(PAKitLoc item);

        /// <summary>
        /// 4,
        /// </summary>
        /// <param name="item"></param>
        void UpdatePAKitLoc(PAKitLoc item);

        /// <summary>
        /// 4',
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdatePAKitLoc(PAKitLoc setValue, PAKitLoc condition);

        /// <summary>
        /// 5,
        /// </summary>
        /// <param name="id"></param>
        void DeletePAKitLoc(int id);

        /// <summary>
        /// 符合条件的Product：
        /// 1） ProductStatus.Line的Stage=“FA”
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        IList<IProduct> GetProductByLineStage(string stage);

        /// <summary>
        /// 符合条件的Product：
        /// 2） ProductStatus.Line的Stage=“PAK”并且ProductStatus.Station=“69”
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<IProduct> GetProductByLineStage(string stage, string station);

        IList<ProductBTInfo> GetProductBT(string productId);

        void InsertProductBT(ProductBTInfo item);

        /// <summary>
        /// 根据条件删除，实体对象，给哪个赋值即使用哪个作条件，条件之间的关系是AND,
        /// </summary>
        /// <param name="condition"></param>
        void DeleteProductBTByObjAndCondition(ProductBTInfo condition);

        /// <summary>
        /// 1. [FA].[dbo].[rpt_ITCNDTS_SET_IMAGEDOWN_14]
        /// Description:
        /// Image Download实时调用的FIS 的Stored Procedure
        /// Parameter(s):
        /// @cpqsno varchar(24),--Customer SN
        /// @flag char(10),-- 'PASS'|'NEEDUP'|'INFO'|'TESTHDD'|'STARTDL'
        /// @version char(500)-- Image Download 提供的数据, 采用如下格式：Key1:Value1~Key2:Value2~....~Keyn:Value
        ///      -- 目前支持的Key 有如下几种：
        ///      -- BIOS|FLOW|HDVD|WMAC|TVTUNER|IMEI|VERSION|BLMAC
        /// </summary>
        /// <param name="cpqsno"></param>
        /// <param name="flag"></param>
        /// <param name="version"></param>
        void CallRpt_ITCNDTS_SET_IMAGEDOWN_14(string cpqsno, string flag, string version);
        
        /// <summary>
        /// 22), 通过ID 查找获得WipBuffer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WipBuffer FindWipBufferById(int id);

        /// <summary>
        /// SELECT Model FROM [TmpKit] WHERE PdLine='pdLine' AND [Type]='type'
        /// </summary>
        /// <param name="pdLine"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetRealModelData(string pdLine, string type);

        /// <summary>
        /// DELETE FROM [TmpKit]
        /// WHERE PdLine='pdLine' AND [Type]='type' AND
        /// Model Not IN(SELECT [Model] FROM [Model])
        /// </summary>
        /// <param name="pdline"></param>
        /// <param name="type"></param>
        void DeleteRealModelData(string pdline, string type);

        /// <summary>
        /// select top 1 * from QCStatus where ProductID='<id>' and Tp='PAQC' order by Udt DESC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProductQCStatus GetNewestProductQCStatus(string id);

        /// <summary>
        /// SELECT distinct ProductID  FROM QCStatus where Tp='PAQC' and Status in ('8', 'A') and ProductID IN (SELECT ProductID FROM Product WHERE DeliveryNo='<dn>')
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<string> GetProductIDListNeedToCheck(string dn);

        /// <summary>
        /// select * from IMES_FA..ProductLog where Station='' and ProductID='<prodID>'
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<ProductLog> GetProductLogs(string prodId, string station);

        /// <summary>
        /// 得到pass数量：
        /// 从ProductLog获取，条件：
        /// A. Status=1
        /// B. StationID=79(ITCND Check站) 
        /// C. Cdt范围：若当前时间大于7:50，则Cdt>当天日期的7:50；否则Cdt在前一天日期的7:50和当天日期的7:50之间
        /// D. Line=line#
        /// 显示的信息：
        /// Model
        /// Qty
        /// </summary>
        /// <param name="status"></param>
        /// <param name="station"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        IList<ModelStatistics> GetByModelStatisticsFromProductLog(int status, string station, DateTime begin, DateTime end, string line);

        /// <summary>
        /// 在WipBuffer表中不存在Code = @family 的纪录
        /// @family =Model.Family 
        /// If Not Exist(SELECT Top 1 [ID]
        ///   FROM [IMES2012_FA].[dbo].[WipBuffer] where [KittingType]='FA Part' and [Code]= @family)
        /// </summary>
        /// <param name="kittingType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<WipBuffer> GetWipBufferByKittingTypeAndCode(string kittingType, string code);

        /// <summary>
        /// SELECT RTRIM(a.Value) + '-' + CASE (CHARINDEX(' ', Family) - 1) WHEN -1 THEN Family
        /// ELSE SUBSTRING(Family, 1, (CHARINDEX(' ', Family) - 1)) END as [KittingCode]
        /// FROM IMES_GetData..ModelInfo a, IMES_GetData..Model b,Product c
        /// WHERE a.Model = b.Model
        /// AND a.Name = 'DM2'
        /// And b.Model=c.Model
        /// AND c.ID = @ ProductID
        /// </summary>
        /// <param name="modelInfoName"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        string GetKittingCodeFromProcuctAndModelAndModelInfo(string modelInfoName, string productId);

        /// <summary>
        /// IF EXISTS(SELECT * FROM IMES_FA..KittingCode WHERE Code = @Code AND Type = 'Kitting')
        /// </summary>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CheckExistKittingCodeByCodeAndType(string code, string type);

        /// <summary>
        /// Get 料位信息 By [Code] and [ProdID]
        /// 参考方法：
        /// SELECT DISTINCT RTRIM(b.Code) as Code, RTRIM(b.PartNo) as [Part No],
        ///   RTRIM(b.Tp) as [Type], CONVERT(int, b.LightNo) as [Light No],
        ///   '' as Scan
        ///  FROM IMES_GetData..MoBOM a, IMES_FA..WipBuffer b
        ///  WHERE a.Deviation = 1
        ///   AND a.PartNo = b.PartNo
        ///   AND b.Code = @Code
        ///   AND b.Tp = 'Kitting'
        ///   AND MO IN (SELECT MO FROM IMES_GetData..MO , Product WHERE Product.Model= MO .Model AND Product.ID= @ProdID)
        ///  ORDER BY [Light No], [Part No], [Type]
        /// </summary>
        /// <param name="code"></param>
        /// <param name="prodId"></param>
        /// <returns></returns>
        DataTable GetMaterialPositionInfoByCodeAndProdID(string code, string prodId);

        /// <summary>
        /// select @cnt=count(distinct ProductID) from QCStatus where Tp='PAQC' 
        /// and PdLine=@pdline 
        /// and Model=@model 
        /// AND Cdt ＞= @StartDate AND Cdt ＜ @EndDate --and convert(char(10),Cdt,111)=convert(char(10),getdate(),111)
        /// 
        /// Prequisite:每天的中午12点至转天的中午12点为一个抽检周期
        /// 
        /// DECLARE @StartDate datetime, @EndDate datetime
        /// IF DATEPART(HOUR, GETDATE()) > 12
        /// BEGIN
        ///     SET @StartDate = convert(char(10),GETDATE(),121) + ' 12:00:00.000'
        ///     SET @EndDate = convert(char(10),DATEADD(day, 1, GETDATE()),121) + ' 12:00:00.000'
        /// END
        /// ELSE
        /// BEGIN
        ///     SET @StartDate = convert(char(10),DATEADD(day, -1, GETDATE()),121) + ' 12:00:00.000'
        ///     SET @EndDate = convert(char(10),GETDATE(),121) + ' 12:00:00.000'
        /// END
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="pdLine"></param>
        /// <param name="pno"></param>
        /// <returns></returns>
        int GetCountOfQCStatusByTpAndPdLineAndPnoToday(string tp, string pdLine, string pno);

        /// <summary>
        /// SELECT @cnt=count(distinct ProductID) FROM QCStatus WHERE Tp='PAQC' 
        /// AND PdLine=@pdline 
        /// AND Model LIKE 'PC____[0-9]_____' AND RIGHT(RTRIM(Model), 3) NOT IN ('29Y', '39Y')
        /// AND Cdt ＞= @StartDate AND Cdt ＜ @EndDate 
        /// 
        /// Prequisite:每天的中午12点至转天的中午12点为一个抽检周期
        /// 
        /// DECLARE @StartDate datetime, @EndDate datetime
        /// IF DATEPART(HOUR, GETDATE()) > 12
        /// BEGIN
        ///     SET @StartDate = convert(char(10),GETDATE(),121) + ' 12:00:00.000'
        ///     SET @EndDate = convert(char(10),DATEADD(day, 1, GETDATE()),121) + ' 12:00:00.000'
        /// END
        /// ELSE
        /// BEGIN
        ///     SET @StartDate = convert(char(10),DATEADD(day, -1, GETDATE()),121) + ' 12:00:00.000'
        ///     SET @EndDate = convert(char(10),GETDATE(),121) + ' 12:00:00.000'
        /// END
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="pdLine"></param>
        /// <returns></returns>
        int GetCountOfQCStatusByTpAndPdLineAndModelToday(string tp, string pdLine);

        /// <summary>
        /// SELECT @cnt=count(distinct ProductID) FROM QCStatus WHERE Tp='PAQC' 
        /// AND Line=@pdline 
        /// AND Model LIKE 'PC____[0-9]_____' AND SUBSTRING(RTRIM(Model), 10, 2) = SUBSTRING(RTRIM(@Model), 10, 2)
        /// AND Cdt ＞= @StartDate AND Cdt ＜ @EndDate 
        /// 
        /// 其余同上 ///
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="pdLine"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        int GetCountOfQCStatusByTpAndPdLineAndModelToday(string tp, string pdLine, string model);
 
        /// <summary>
        /// 删除IMES2012_FA..MPInterface数据
        /// </summary>
        /// <param name="key"></param>
        void DeleteMPInterface(object key);

        /// <summary>
        /// SQL:select count(0) from ProductInfo where InfoType = [InfoType] and InfoValue = [InfoValue];
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="infoValue"></param>
        /// <returns></returns>
        int GetProductInfoCountByInfoValue(string infoType, string infoValue);

        /// <summary>
        /// 根据family,pType,pdLine查找表KitLoc的对应记录 where Family=family AND PartType=pType AND PdLine=pdLine
        /// </summary>
        /// <param name="family"></param>
        /// <param name="pType"></param>
        /// <param name="pdLine"></param>
        /// <returns></returns>
        FAFloatLocationInfo GetFAFloatLocation(string family,string pType,string pdLine);

        /// <summary>
        /// 根据palletNo获取所有属于该palletNo的Product,按照Customer S/N 升序排序
        /// 此接口与GetProductListByPalletNo的SQL类似，增加按照Customer S/N 升序排序
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<ProductModel> GetProductListByPalletNoOrderByCustSN(string palletNo);

        /// <summary>
        /// update ProductStatus set Status=@Status,Station=@Station,Editor=@Editor,Udt=GETDATE()
        /// from ProductStatus as S inner join Product as P ON S.ProductID =P.ProductID
        /// WHERE P.DeliveryNo =@DeliveryNo and (Model LIKE 'PC%' or Model LIKE 'QC%')
        /// </summary>
        /// <param name="newStatus"></param>
        /// <param name="dn"></param>
        void UpdateUnPackProductStatusByDeliveryNo(ProductStatus newStatus, string dn);

        /// <summary>
        /// update ProductStatus set Status=@Status,Station=@Station,Editor=@Editor,Udt=GETDATE()
        /// from ProductStatus as S inner join Product as P ON S.ProductID =P.ProductID
        /// WHERE P.DeliveryNo =@DeliveryNo 
        /// </summary>
        /// <param name="newStatus"></param>
        /// <param name="dn"></param>
        void UpdateUnPackProductStatusByDn(ProductStatus newStatus, string dn);

        /// <summary>
        /// insert into ProductLog(Line,Model,ProductID,Station,Status,Editor,Cdt)
        /// select @Line,Model,ProductID,@Station,@Status,@Editor,GETDATE()
        /// from Product 
        /// where DeliveryNo =@DeliveryNo and (Model LIKE 'PC%' or Model LIKE 'QC%') 
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="newLog"></param>
        /// <returns></returns>
        void WriteUnPackProductLogByDeliveryNo(string dn, ProductLog newLog);

        /// <summary>
        /// insert into ProductLog(Line,Model,ProductID,Station,Status,Editor,Cdt)
        /// select @Line,Model,ProductID,@Station,@Status,@Editor,GETDATE()
        /// from Product 
        /// where DeliveryNo =@DeliveryNo 
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="newLog"></param>
        /// <returns></returns>
        void WriteUnPackProductLogByDn(string dn, ProductLog newLog);

        /// <summary>
        /// delete ProductInfo from ProductInfo as I inner join Product as P ON I.ProductID = P.ProductID
        /// where I.InfoType=@InfoType and P.DeliveryNo=@DeliveryNo and (P.Model LIKE 'PC%' or P.Model LIKE 'QC%')
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="dn"></param>
        void UnPackProductInfoByDeliveryNo(string infoType, string dn);

        /// <summary>
        /// delete ProductInfo from ProductInfo as I inner join Product as P ON I.ProductID = P.ProductID
        /// where I.InfoType=@InfoType and P.DeliveryNo=@DeliveryNo 
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="dn"></param>
        void UnPackProductInfoByDeliveryNoAndInfoType(string infoType, string dn);

        /// <summary>
        /// update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0 
        /// where DeliveryNo=@DeliveryNo and (P.Model LIKE 'PC%' or P.Model LIKE 'QC%')
        /// </summary>
        /// <param name="dn"></param>
        void UnPackProductByDeliveryNo(string dn);

        /// <summary>
        /// update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0 ,UnitWeight=0.0
        /// where DeliveryNo=@DeliveryNo 
        /// </summary>
        /// <param name="dn"></param>
        void UnPackProductByDn(string dn);

        /// <summary>
        /// update Product set DeliveryNo='',PalletNo='',CartonWeight=0.0 ,UnitWeight=0.0
        /// where DeliveryNo=@DeliveryNo 
        /// </summary>
        /// <param name="dn"></param>
        void UnPackProductByDnWithoutCartonSN(string dn);

        /// <summary>
        /// 根据PalletNo号码获取所有属于该PalletNo的Product对象
        /// 根据Pallet在Product中按照PalletNo=@plt得到已绑定的unit：ProductID、CartonSN、CUSTSN
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        IList<IProduct> GetProductListByPalletNo2(string plt);

        /// <summary>
        /// 2.根据type,code和station获取runintimecontrol数据(添加)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        RunInTimeControlInfoMaintain GetRunInTimeControlByTypeCodeAndStation(string type, string code, string station);

        /// <summary>
        /// SELECT TOP 1 Cdt,Code,ControlType,Editor,Hour,ID,Remark,TestStation,Type,Udt 
        /// FROM RunInTimeControl 
        /// WHERE Code=@Code AND TestStation=@TestStation AND Type=@Type AND ID<>@id 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="testStation"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        RunInTimeControlInfoMaintain GetRunInTimeControlExceptId(string type, string code, string testStation, int id);

        /// <summary>
        /// 根据shipmentNo获取所有属于该deliveryNo的Product的数量
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        int GetCombinedQtyByShipmentNo(string shipmentNo);

        /// <summary>
        /// Insert to TSModel
        /// </summary>
        /// <param name="item"></param>
        void InsertTSModel(TsModelInfo item);

        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<TsModelInfo> GetTsModelList(TsModelInfo condition);

        /// <summary>
        /// Delete from TSModel
        /// </summary>
        /// <param name="mark"></param>
        /// <param name="model"></param>
        void DeleteTsModel(string mark, string model);

        /// <summary>
        /// 根据Type和TestStation获得RunInTimeControl
        /// </summary>
        /// <param name="type"></param>
        /// <param name="testStation"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<RunInTimeControl> GetRunInTimeControlByTypeAndTestStation(string type, string testStation, string code);

        /// <summary>
        /// 2. 
        /// 查询IMES_FA..Product 表，取得palletNoLst对应的ProductModel记录集,按照Customer S/N 升序排序
        /// </summary>
        /// <param name="palletNoLst"></param>
        /// <returns></returns>
        IList<ProductModel> GetProductListByPalletNoListOrderByCustSN(IList<string> palletNoLst);

        /// <summary>
        /// 3.
        /// 根据prodNoList获得ProductModel列表，按照Customer S/N 升序排序
        /// </summary>
        /// <param name="prodNoList"></param>
        /// <returns></returns>
        IList<ProductModel> GetproductlistOrderByCustSN(IList<string> prodNoList);

        /// <summary>
        /// 1.
        /// 统计在IMES_FA..ProductInfo表里，根据传入的InfoType ，查询到InfoValue值相同的记录数;
        /// 返回记录数（int），找不到返回 0;
        /// </summary>
        /// <param name="infoType"></param>
        /// <returns></returns>
        int GetProductInfoCountByInfoType(string infoType);

        /// <summary>
        /// 3.使用code, station, checkTp查询IMES_FA..AstRule 表存在记录,返回该AstRule类型的记录;
        /// </summary>
        /// <param name="code"></param>
        /// <param name="station"></param>
        /// <param name="checkTp"></param>
        /// <returns></returns>
        IList<AstRuleInfo> GetAstRuleByCodeAndStationAndCheckTp(string code, string station,string checkTp);

        /// <summary>
        /// 4.使用code, station, CUstName查询IMES_FA..AstRule 表存在记录，返回该AstRule类型数据;
        /// </summary>
        /// <param name="code"></param>
        /// <param name="station"></param>
        /// <param name="custName"></param>
        /// <returns></returns>
        IList<AstRuleInfo> GetAstRuleByCodeAndStationAndCust(string code, string station, string custName);

        DataTable ExistAstRule(AstRuleInfo condition);

        void AddAstRuleInfo(AstRuleInfo item);

        void DeleteAstRuleRule(AstRuleInfo condition);

        /// <summary>
        /// IMES_FA..ProductInfo表里，根据传入的prodId,InfoType，查询到的InfoValue值；
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        string GetProductInfoValue(string prodId, string infoType);

        /// <summary>
        /// 1.数据表【supplier code】：
        /// 根据vendor取得Max（idex）
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        string GetMaxIdexFromSupplierCodeByVendor(string vendor);

        /// <summary>
        /// Select b.Tp,Type,@message=Message,Sno1, @lwc=L_WC, @mwc=M_WC from Special_Det a,Special_Maintain b where a.SnoId=@Productid and b.Type=a.Tp and b.SWC=”68”
        /// select b.Tp,Type,Message,Sno1 from Special_Det a,Special_Maintain b where a.SnoId=@Productid and b.Type=a.Tp and b.SWC=”8D” order by Type,b.Tp
        /// </summary>
        /// <param name="snoId"></param>
        /// <param name="wc"></param>
        /// <returns></returns>
        IList<SpecialCombinationInfo> GetSpecialDetSpecialMaintainInfoList(string snoId, string swc);

        /// <summary>
        /// 在PrintLog中查找
        /// Start ProdId和End ProdId在BegNo和EndNo中范围内
        /// </summary>
        /// <returns></returns>
        bool CheckProductInPrintLogRange(string productId);

        /// <summary>
        /// 根据MAC取Product数据记录
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        IList<IProduct> GetProductlistByMac(string mac);

        /// <summary>
        /// Special_Det中是否存在对应的记录(Tp=’ATSN7’/ ‘ATSN8’ and SnoId=@prdid)，
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="snoid"></param>
        /// <returns></returns>
        bool CheckExistSpecialDet(string tp, string snoid);

        /// <summary>
        /// 若存在，则Update Sno1=@astsn,Udt=getdate()
        /// </summary>
        /// <param name="sno1"></param>
        /// <param name="id"></param>
        void UpdateSpecialDetSno1(string sno1, string tp, string snoid);

        /// <summary>
        /// 否则Insert(Tp是’ATSN7’ or ‘ATSN8’)
        /// </summary>
        /// <param name="item"></param>
        void AddSpecialDetInfo(SpecialDetInfo item);

        /// <summary>
        /// （新增加）Docking – IMES_FA..Product_Part 中有BomNodeType = 'PS'  的Part
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        bool CheckExistProductPart(string prodId, string bomNodeType);

        /// <summary>
        /// 1.Other Tips 需要使用弹出Message Box 方式进行显示
        /// 如果使用如下方法查询IMES_FA..Special_Det 和IMES_FA..Special_Maintain 表存在记录的话，那么就需要显示相关提示信息
        /// 方法如下：
        /// SQL： SELECT b.[Message] FROM Special_Det a,Special_Maintain b 
        ///          WHERE a.SnoId=@Productid AND b.Type=a.Tp AND b.SWC='69' ORDER BY b.Type,b.Tp
        /// </summary>
        /// <param name="snoId"></param>
        /// <param name="swc"></param>
        /// <returns></returns>
        IList<string> GetMessageListFromSpecialDetAndSpecialMaintain(string snoId, string swc);

        /// <summary>
        /// SELECT PartNo,Tp,Qty FROM WipBuffer (nolock) 
        /// WHERE PartNo in (SELECT SPno FROM #Bom) AND Code=RTRIM(@Code)
        /// </summary>
        /// <param name="pnoList"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<WipBuffer> GetWipBufferByPnoListAndCode(string[] pnoList, string code);

        /// <summary>
        /// SELECT LightNo FROM WipBuffer (nolock) 
        /// WHERE PartNo IN (SELECT partno FROM #wipbuffer) AND Code=RTRIM(@Code)
        /// </summary>
        /// <param name="pnoList"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<string> GetLightNoFromWipBufferByPnoListAndCode(string[] pnoList, string code);

        /// <summary>
        /// SELECT * FROM WipBuffer (nolock) 
        /// WHERE PartNo IN (SELECT partno FROM #wipbuffer) AND Code=RTRIM(@Code)
        /// ORDER BY PartNo, LightNo
        /// </summary>
        /// <param name="pnoList"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        IList<WipBuffer> GetWipBufferListByPnoListAndCode(string[] pnoList, string code);

        /// <summary>
        /// PAK Pallet Verify:
        /// 查询IMES_FA..Product来获取
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<IMES.FisObject.FA.Product.IProduct> GetProductListByDeliveryNoAndPalletNo(string deliveryNo, string palletNo);

        /// <summary>
        /// update Product set DeliveryNo='',PalletNo='',CartonSN='',CartonWeight=0.0,PizzaID=''  
        /// where DeliveryNo =@DeliveryNo
        /// </summary>
        /// <param name="dn"></param>
        void UpdateProductsForUnbound(string dn);

        /// <summary>
        /// delete ProductInfo from ProductInfo as I inner join Product as P ON I.ProductID = P.ProductID
        /// where I.InfoType=@InfoType and P.DeliveryNo=@DeliveryNo
        /// </summary>
        /// <param name="infoType"></param>
        /// <param name="dn"></param>
        void DeleteProductInfoForUnbound(string infoType, string dn);

        /// <summary>
        /// MSCOA – IMES_FA..Product_Part 中有PartType = 'P1' 属性存在Descr LIKE 'COA%' 的Part
        /// CN Card – IMES_FA..Product_Part 中有PartType = 'P1' 属性存在Descr LIKE 'Home Card' 的Part
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="partType"></param>
        /// <param name="descrLike">底层实现不加%</param>
        void DeleteProductPartByPartTypeAndDescrLike(string prodId, string partType, string descrLike);

        /// <summary>
        /// NYLON – IMES_FA..Product_Part 中有PartType LIKE ' NYLON%' 的Part
        /// Poser Card – IMES_FA..Product_Part 中有PartType LIKE ' Poster%' 的Part
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="partTypePrefix">底层实现参数后加%</param>
        void DeleteProductPartByLikePartType(string prodId, string partTypePrefix);

        /// <summary>
        /// IMES_FA..Product_Part 中有PartType = 'P1' 属性存在Descr LIKE 'COA%' 的Part
        /// IMES_FA..Product_Part 中有PartType = 'P1' 属性存在Descr LIKE 'Home Card' 的Part
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="partType"></param>
        /// <param name="descrLike">由上面参数赋予%或_</param>
        /// <returns></returns>
        IList<ProductPart> GetProductPartByPartTypeAndDescrLike(string prodId, string partType, string descrLike);

        IList<ProductPart> GetProductPartByBomNodeTypeAndDescrLike(string prodId, string bomNodeType, string descrLike);

        IList<ProductPart> GetProductPartByBomNodeTypeAndPartSnLike(string prodId, string bomNodeType, string partSnLike);

        IList<ProductPart> GetProductPartByPartTypeLike(string prodId, string partTypeLike);

        IList<ProductPart> GetProductPartByDescrLike(string prodId, string descrLike);

        IList<ProductPart> GetProductPartList(ProductPart condition);

        /// <summary>
        /// SELECT Code FROM IMES2012_FA..KittingCode WHERE Type = 'FA Label' Order By Code
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetCodeListFromKittingCode(KittingCode condition);

        /// <summary>
        /// condition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// select @astsn2=Sno from CDSIAST nolock where Tp ='ASSET_TAG2'and SnoId=@prdId
        /// select @astsn1=Sno from CDSIAST nolock where Tp ='ASSET_TAG' and SnoId=@prdId
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<CdsiastInfo> GetCdsiastInfoList(CdsiastInfo condition);

        /// <summary>
        /// use IMES2012_PAK
        /// select * from dbo.WipBuffer
        /// where KittingType=@kitType and 
        /// Station=@station and Code=@code
        /// </summary>
        /// <param name="station"></param>
        /// <param name="code"></param>
        /// <param name="kitType"></param>
        /// <returns></returns>
        IList<WipBuffer> GetWipBuffersByStaCodeType(string station, string code, string kitType);

        /// <summary>
        /// 根据type,code和teststation删除runintimecontrol数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="teststation"></param>
        void DeleteRunInTimeControlByTypeCodeAndTeststation(string type, string code,string teststation);

        /// <summary>
        /// select COUNT(1) from Product as P 
        /// inner join ProductStatus as S ON P.ProductID = S.ProductID
        /// INNER JOIN IMES2012_GetData.dbo.Line AS L ON L.Line = S.Line 
        /// WHERE P.Model=@Model AND ((L.Stage='FA') OR (L.Stage='PAK' AND S.Station ='69')) 
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        bool PreQueryForChangeToBTDeffered(string modelName);

        /// <summary>
        /// 1.
        /// insert into ProductBT(ProductID,BT,Editor,Cdt,Udt)
        /// select P.ProductID,@BT,@Editor,GETDATE(),GETDATE() from Product as P 
        /// inner join ProductStatus as S ON P.ProductID = S.ProductID
        /// INNER JOIN IMES2012_GetData.dbo.Line AS L ON L.Line = S.Line 
        /// WHERE P.Model=@Model AND ((L.Stage='FA') OR (L.Stage='PAK' AND S.Station ='69')) 
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="editor"></param>
        /// <param name="bt"></param>
        void ChangeToBTDeffered(string modelName,string editor,string bt);

        /// <summary>
        /// select COUNT(1) from ProductBT as B inner join Product as P on B.ProductID = P.ProductID
        /// inner join ProductStatus as S ON P.ProductID = S.ProductID
        /// INNER JOIN IMES2012_GetData.dbo.Line AS L ON L.Line = S.Line 
        /// WHERE P.Model=@Model AND ((L.Stage='FA') OR (L.Stage='PAK' AND S.Station ='69')) 
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        bool PreQueryForChangeToNonBT(string modelName);

        /// <summary>
        /// 2.
        /// delete ProductBT 
        /// from ProductBT as B inner join Product as P on B.ProductID = P.ProductID
        /// inner join ProductStatus as S ON P.ProductID = S.ProductID
        /// INNER JOIN IMES2012_GetData.dbo.Line AS L ON L.Line = S.Line 
        /// WHERE P.Model=@Model AND ((L.Stage='FA') OR (L.Stage='PAK' AND S.Station ='69')) 
        /// </summary>
        /// <param name="modelName"></param>
        void ChangeToNonBT(string modelName);

        /// <summary>
        /// 1,sql:select * from FA_PA_LightSt order by pno
        /// </summary>
        /// <returns></returns>
        IList<FaPaLightstInfo> GetAllFaPaLightStations();

        /// <summary>
        /// 2,SQL:select count(*) from FA_PA_LightSt where pno=iecpn and family=family and stn=lightstation.存在返回true,不存在返回false.
        /// </summary>
        /// <param name="iecpn"></param>
        /// <param name="family"></param>
        /// <param name="lightStation"></param>
        /// <returns></returns>
        IList<FaPaLightstInfo> CheckFaPaLightStationExist(string iecpn, string family, string lightStation);

        /// <summary>
        /// 3, Insert
        /// </summary>
        /// <param name="item"></param>
        void AddFaPaLightStationItem(FaPaLightstInfo item);

        /// <summary>
        /// 4, Delete
        /// </summary>
        /// <param name="id"></param>
        void DeleteFaPaLightStationItem(int id);

        /// <summary>
        /// 5,Sql:update FA_PA_LightSt set 各个字段 where id=[id]
        /// </summary>
        /// <param name="item"></param>
        void UpdateFaPaLightStationItem(FaPaLightstInfo item);
        
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="item"></param>
        void AddKittingBoxSNItem(KittingBoxSNInfo item);

        /// <summary>
        /// If exists (select a.ProductID from Product a (nolock), Product_Part b (nolock), ProductStatus c (nolock) where a.ProductID=b.ProductID and b.PartSn=@coano and a.ProductID=c.ProductID and (c.Station='85' or c.Station='99'))
        /// </summary>
        /// <param name="partSn"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
        bool CheckExistProductByPartSnAndStations(string partSn, string[] stations);

        /// <summary>
        /// If exists (select a.ProductID from Product a (nolock), Pizza_Part b (nolock), ProductStatus c (nolock) where a.PizzaID = b.PizzaID and b.PartSn = @coano and a.ProductID=c.ProductID and (c.Station='85' or c.Station='9A'))
        /// </summary>
        /// <param name="partSn"></param>
        /// <param name="stations"></param>
        /// <returns></returns>
        bool CheckExistProductByPartSnAndStationsWithPizzaPart(string partSn, string[] stations);

        /// <summary>
        /// Select a.ProductID, a.CUSTSN, c.CustPN from Product a (nolock), Product_Part b (nolock), Model c (nolock) where a.ProductID=b.ProductID and b.PartSn=@coano and a.Model=c.Model
        /// </summary>
        /// <param name="partSn"></param>
        /// <returns></returns>
        IList<ProductAndCustInfo> GetProductAndCustInfoListByPartSn(string partSn);

        /// <summary>
        /// delete Product_Part where PartSn=@coano and ProductID=@productID
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="partSn"></param>
        void DeleteProductPartByPartSn(string prodId, string partSn);

        /// <summary>
        /// ITCNDCheckSetting : 根据Line字段来查询.  SELECT [CheckItem],[CheckType],[CheckCondition] FROM [IMES2012_FA].[dbo].[ITCNDCheckSetting] where Line=''
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        IList<string[]> GetCheckItemListFromITCNDCheckSetting(string line);

        /// <summary>
        /// Get Products By Model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<IProduct> GetProductListByModel(string model);

        /// <summary>
        /// Select ReturnStn from ProductRepair a, ProductRepair_defectInfo b where a.ID=b.ProductRepairID and a.ProductID=ProductID and a.Status=0
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<string> GetReturnStnListFromProductRepair(string prodId, int status);

        /// <summary>
        /// select top 1 * from SupplierCode where Vendor=@Vendor order by Idex asc
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        SupplierCodeInfo GetSupplierCodeByVendor(string vendor);

        /// <summary>
        /// select top 1 * from SupplierCode where Vendor=@Vendor and Idex > @Idex order by Idex asc
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="idex"></param>
        /// <returns></returns>
        SupplierCodeInfo GetSupplierCodeByVendor(string vendor, string idex);

        /// <summary>
        /// select top 1 Code from SupplierCode where Vendor = @vendor and LEN(RTRIM(Code)) = LEN(@ast)
        /// </summary>
        /// <param name="vendor"></param>
        /// <param name="idex"></param>
        /// <returns></returns>
        SupplierCodeInfo GetSupplierCodeByVendorAndCodeLength(string vendor, string ast);

        /// <summary>
        /// select * from ProductInfo where InfoType=Item# and ProductInfo.ProductID<>ProductID# and ProductInfo.InfoValue in (select ProductInfo.InfoValue from ProductInfo where InfoType=Item# and ProductInfo.ProductID=ProductID#)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        bool CheckExistProductInfoWhoseInfoValueBeenOccupiedByAnotherProduct(string productId, string infoType);

        /// <summary>
        /// select ProductInfo.InfoValue from ProductInfo where InfoType=Item# and ProductInfo.ProductID=ProductID#
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        bool CheckExistProductInfo(string productId, string infoType);

        /// <summary>
        /// Condition：ProductID=PrdID and Status=0 Order By Cdt Desc
        /// </summary>
        /// <param name="productId">productId</param>
        /// <returns>ProductLog</returns>
        ProductLog GetLatestFailLog(string productId);

        /// <summary>
        /// delete Product_Part where PartNo=@partNo and ProductID=@productID
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="partNo"></param>
        void DeleteProductPartByPartNo(string prodId, string partNo);

        /// <summary>
        /// 根据partSn获得ProductPart列表.
        /// </summary>
        /// <param name="partSn"></param>
        /// <returns></returns>
        IList<ProductPart> GetProductPartsByPartSn(string partSn);

        /// <summary>
        /// SELECT Code FROM IMES_FA..LabelKitting WHERE Type='FA Label' Order By Code
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<string> GetLabelKittingCodeList(string type);

        /// <summary>
        /// EXISTS (SELECT * FROM FA.dbo.ProductBT (nolock) WHERE ProductId=@SnoId)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool CheckExistProductBT(string productId);

        /// <summary>
        /// select * from WipBuffer where Code=<输入指定code> and KittingType='FA Label'
        /// </summary>
        /// <param name="code"></param>
        /// <param name="kittingType"></param>
        /// <returns></returns>
        IList<WipBuffer> GetWipBufferListFromWipBuffer(string code, string kittingType);

        /// <summary>
        /// Update ProductStatus, Station=Dismantle Station, Status=1
        /// setValue哪个字段赋值就有更新哪个字段;
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="prodIds"></param>
        void UpdateProductStatuses(ProductStatus setValue, string[] prodIds);

        /// <summary>
        /// Insert ProductLog WC=38.
        /// </summary>
        /// <param name="items"></param>
        void InsertProductLogs(ProductLog[] items);

        /// <summary>
        /// Update Products
        /// setValue哪个字段赋值就有更新哪个字段;
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="prodIds"></param>
        void UpdateProducts(Product setValue, string[] prodIds);
        void UpdateProducts(Product setValue, string[] prodIds, decimal newCartonWeight, decimal newUnitWeight);

        /// <summary>
        /// Delete from Product_Part where ProductID=productID# and PartType=KP#
        /// Delete from Product_Part where ProductID=productID# and BomNodeType=’AT’
        /// otherCondition哪个字段赋值就有哪个条件,自由使用各个条件,条件间是AND关系.
        /// </summary>
        /// <param name="prodIds"></param>
        /// <param name="otherCondition"></param>
        void DeleteProductParts(string[] prodIds, ProductPart otherCondition);

        /// <summary>
        /// IF Exists (select ID FROM QCStatus WHERE ProductID= ProductID# AND Tp='PIA' AND Remark='1')
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="tp"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        bool IsRealEPIAReflow(string productID, string tp, string remark);

        bool AddQCStatusWithConditionOfIsRealEPIAReflow(ProductQCStatus item, string productID, string tp, string remark);

        DateTime GetQCStartTime();

        /// <summary>
        /// 得到计算已抽样数量的开始时间@QCStarTime：
        /// 如果当前时间在当天的7：50之前，则计算已抽样数量的开始时间@QCStarTime为前一天的7:50; 
        /// 如果当前时间在当天的7：50之后则则计算已抽样数量的开始时间@QCStarTime为当天的7:50	
        /// IF getdate() ＜ convert(char(10),getdate(),111)  + ' 07:50:00.000'
        /// select convert(char(10),getdate()-1,111)+' 07:50:00.000'
        /// Else
        /// select convert(char(10),getdate(),111)+' 07:50:00.000'
        /// </summary>
        /// <param name="t">赋Hour和Minute即可</param>
        /// <returns></returns>
        DateTime GetQCStartTime(DateTime t);

        ///// <summary>
        ///// select COUNT(DISTINCT ProductID) from QCStatus where Tp='PIA' and Cdt>=@QCStartTime and substring(Model,10,2)=substring(@ProdModel,10,2) and Model in (select part_number from Bom_Code (nolock) where os_code=@syscode)
        ///// </summary>
        ///// <param name="tp"></param>
        ///// <param name="qcStartTime"></param>
        ///// <param name="prodModel"></param>
        ///// <param name="syscode"></param>
        ///// <returns></returns>
        //int GetCTOSampleCount(string tp, DateTime qcStartTime, string prodModel, string syscode);

        /// <summary>
        /// select COUNT(DISTINCT ProductID) from QCStatus where Tp='PIA' and Cdt>=@QCStartTime and substring(Model,10,2)=substring(@ProdModel,10,2)
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="qcStartTime"></param>
        /// <param name="prodModel"></param>
        /// <returns></returns>
        int GetCTOSampleCount(string tp, DateTime qcStartTime, string prodModel);

        void AddQCStatusWithConditionOfGetCTOSampleCount(ProductQCStatus item, int eoqcRatio, string tp, DateTime qcStartTime, string prodModel);

        /// <summary>
        /// select COUNT(DISTINCT ProductID) from QCStatus where Tp='PIA' and Cdt>=@QCStartTime and substring(Model,10,2)=substring(@ProdModel,10,2) and Model in (select Model from Model where Family=@prodFamily)
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="qcStartTime"></param>
        /// <param name="prodModel"></param>
        /// <param name="prodFamily"></param>
        /// <returns></returns>
        int GetCTOSampleCount(string tp, DateTime qcStartTime, string prodModel, string prodFamily);

        void AddQCStatusWithConditionOfGetCTOSampleCount(ProductQCStatus item, int eoqcRatio, string tp, DateTime qcStartTime, string prodModel, string prodFamily);

        /// <summary>
        /// select COUNT(DISTINCT ProductID) from QCStatus where Tp='PIA' and Cdt>=@QCStartTime
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="qcStartTime"></param>
        /// <returns></returns>
        int GetNCTOSampleCount(string tp, DateTime qcStartTime);

        /// <summary>
        /// Select count(DISTINCT ProductID) from QCStatus where line=@pdline and Tp=@Tp and Cdt>@QCStartTime
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="qcStartTime"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        int GetSampleCount(string tp, DateTime qcStartTime, string line);

        /// <summary>
        /// SELECT @cnt=count(DISTINCT ProductID) FROM QCStatus WHERE Tp='PAQC' AND LEFT(Line, 1)=LEFT(@pdline ,1)
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        int GetSampleCount(string tp, string line);

        /// <summary>
        /// SELECT PartNo,LightNo,Sub 
        /// FROM [WipBuffer] where [Code] = @family
        /// UNION
        /// SELECT distinct PartNo,LightNo,Sub 
        /// FROM [KitLoc] a,[WipBuffer] b 
        /// where a.PdLine=b.Code 
        /// and b.Tp like a.PartType+'%'
        /// and a.Family=@family 
        /// and LEFT(a.PdLine,1)=LEFT(@line,1) 
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        DataTable GetPartNoAndLightNoFromWipBuffer(string family, string line);

        /// <summary>
        /// SELECT PartNo,LightNo,Sub 
        /// FROM [WipBuffer] where [Code] = @family AND Line=@Line 
        /// UNION
        /// SELECT distinct PartNo,LightNo,Sub 
        /// FROM [KitLoc] a,[WipBuffer] b 
        /// where a.PdLine=b.Code 
        /// and b.Tp like a.PartType+'%'
        /// and a.Family=@family 
        /// and LEFT(a.PdLine,1)=LEFT(@line,1) 
        /// </summary>
        /// <param name="family"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        DataTable GetPartNoAndLightNoFromWipBufferWithLine(string family, string line);

        /// <summary>
        /// SELECT PartNo,LightNo,Sub 
        /// FROM [WipBuffer] where [Code] = @family
        /// UNION
        /// SELECT  distinct PartNo,LightNo,Sub  
        /// FROM [KitLoc]  a,[WipBuffer] b 
        /// where a.PdLine=b.Code
        /// and b.Tp like a.PartType+'%' 
        /// and a.Family=@family 
        /// and b.Line=@Line 
        /// and LEFT(a.PdLine,1)=LEFT(@line,1) 
        /// </summary>
        /// <param name="family"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        DataTable GetPartNoAndLightNoFromWipBufferForDoubleLine(string family, string line);

        /// <summary>
        /// SELECT PartNo,LightNo,Sub 
        /// FROM [WipBuffer] where [Code] = @family AND Line=@Line 
        /// UNION
        /// SELECT distinct PartNo,LightNo,Sub  
        /// FROM [KitLoc] a,[WipBuffer] b 
        /// where a.PdLine=b.Code 
        /// and b.Tp like a.PartType+'%' 
        /// and a.Family=@family 
        /// and b.Line=@Line 
        /// and LEFT(a.PdLine,1)=LEFT(@line,1) 
        /// </summary>
        /// <param name="family"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        DataTable GetPartNoAndLightNoFromWipBufferWithLineForDoubleLine(string family, string line);

        /// <summary>
        /// if exists(
        /// SELECT distinct PartNo,LightNo 
        /// FROM [KitLoc] a,[WipBuffer] b 
        /// where a.PdLine=b.Code 
        /// and a.Family=@family 
        /// and b.Tp like a.PartType+'%' 
        /// and b.Line=@Line 
        /// and LEFT(a.PdLine,1)=LEFT(@line,1) 
        /// ) 
        /// </summary>
        /// <param name="familhy"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        bool CheckExistLightNoFromKitLocAndWipBuffer(string family, string line);

        /// <summary>
        /// select * from ProductRepair_DefectInfo 
        /// where ProductRepairID in (select ID from ProductRepair where ProductID='F21400001' and Status=0) 
        /// and DefectCode in (select DefectCode from DefectCode where Type='PRD')
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<RepairInfo> GetOQCProdRepairList(string prodId, int status, string type);

        void BackUpProduct(string productId, string uEditor);
            
        void BackUpProductStatus(string productId, string uEditor);
            
        void BackUpProductPart(string productId, string uEditor, string partType, string descrLike);

        void BackUpProductPart(string[] prodIds, ProductPart otherCondition, string uEditor);

        void RemoveProductPartByPartTypeAndDescrLike(string productId, string partType, string descrLike);

        void BackUpProductPart(string productId, string uEditor);

        void BackUpProductInfo(string productId, string uEditor, string infoTypeName);

        void BackUpProductInfo(string productId, string uEditor);

        void BackUpProductPartByBomNodeTypeAndDescrLike(string productId, string uEditor, string bomNodeType, string descrLike);

        void RemoveProductPartByBomNodeTypeAndDescrLike(string productId, string bomNodeType, string descrLike);

        void BackUpProductPartByBomNodeType(string productId, string uEditor, string bomNodeType);

        void RemoveProductPartByBomNodeType(string productId, string bomNodeType);

        void BackUpProductPartByPartType(string productId, string uEditor, string partType);

        void RemoveProductPartByPartType(string productId, string partType);

        void BackUpProductPartByDescrLike(string productId, string uEditor, string descrLike);

        void RemoveProductPartByDescrLike(string productId, string descrLike);

        /// <summary>
        /// ProductLog 表latest record WC=66, IsPass=1 如不存在则提示“请先进行Image DownLoad”
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="station"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        ProductLog GetLatestLogByWcAndStatus(string productId, string station, int status);

        /// <summary>
        /// insert into UnpackProduct (各字段) select 相应字段 from Product where DeliveryNo='输入DN' and (Model like 'PC%' or Model like 'QC%')
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="uEditor"></param>
        void BackUpProductByDn(string dn, string uEditor);

        /// <summary>
        /// insert into UnpackProductStatus (各字段) select 相应字段 from ProductStatus where ProductID in(select ProductID from Product where DeliveryNo='输入DN' and (Model like 'PC%' or Model like 'QC%'))
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="uEditor"></param>
        void BackUpProductStatusByDn(string dn, string uEditor);

        /// <summary>
        /// insert into UnpackProduct_Part (各字段) select 相应字段 from Product_Part where ProductID in(select ProductID from Product where DeliveryNo='输入DN' and (Model like 'PC%' or Model like 'QC%'))
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="uEditor"></param>
        void BackUpProductPartByDn(string dn, string uEditor);

        /// <summary>
        /// Insert UnpackProduct_Part select *, @editor, @Udt from Product_Part where ProductID=@ProductID and CheckItemType<>'LCM'
        /// </summary>
        /// <param name="uEditor"></param> 
        /// <param name="eqCondition"></param>
        /// <param name="neqCondition"></param>
        void BackUpProductPartByDn(string uEditor, ProductPart eqCondition, ProductPart neqCondition);

        /// <summary>
        /// insert into UnpackProductInfo (各字段) select 相应字段 from ProductInfo where ProductID in(select ProductID from Product where DeliveryNo='输入DN' and (Model like 'PC%' or Model like 'QC%')) and InfoType in (传入的infoTypeNames列表))
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="uEditor"></param>
        /// <param name="infoTypeNames"></param>       
        void BackUpProductInfoByDn(string dn, string uEditor, IList<string> infoTypeNames);

        void UpdateProductStatusByPallet(string palletSN, ProductStatus newStatus, int newTestFailCount, StationStatus newStationStatus);

        void DeleteKittingBoxSNItem(KittingBoxSNInfo condition);

        /// <summary>
        /// Update Product_Part PartSN= PCA..PCB. CVSN Where CheckItemType=’CPU’ and ProductID=ProductID#
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateProductPart(ProductPart setValue, ProductPart condition);

        bool CheckExistWipBuffer(WipBuffer condition);

        /// <summary>
        /// SQL: select * from PAKitLoc where Station=[station]
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<PAKitLoc> GetPAKitlocByStation(string station);

        /// <summary>
        /// SQL: select * from PAKitLoc where Station=[station] and PdLine=[pdline]
        /// </summary>
        /// <param name="station"></param>
        /// <param name="pdline"></param>
        /// <returns></returns>
        IList<PAKitLoc> GetPAKitlocByStationAndPdLine(string station, string pdline);

        void UpdateProductRepairDefectInfo(RepairInfo setValue, RepairInfo condition);

        /// <summary>
        /// select * from ProductLog where ProductID='<input-id>' and Station='<input-station>' order by Cdt desc
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<ProductLog> GetProductLogsOrderByCdtDesc(string prodId, string station);

        /// <summary>
        /// select * from ProductLog where ProductID='<input-id>' and Station='<input-station>' and Status='<input-status>' order by Cdt desc
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="station"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<ProductLog> GetProductLogsOrderByCdtDesc(string prodId, string station, int status);

        /// <summary>
        /// 实现sql：select COUNT(DISTINCT ProductID) from QCStatus where Tp='<input-tp>' and Model='<input-prodModel>' and Cdt>=<input-qcStartTime>
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="qcStartTime"></param>
        /// <param name="prodModel"></param>
        /// <returns></returns>
        int GetBTOSampleCount(string tp, DateTime qcStartTime, string prodModel);

        void AddQCStatusWithConditionOfGetBTOSampleCount(ProductQCStatus item, int eoqcRatio, string tp, DateTime qcStartTime, string prodModel);

        void InsertKittingLogInfo(KittingLogInfo item);

        void InsertKittingLogInfosFromKittingLocationFaX(KittingLogInfo item, KittingLocationFaXInfo condition, int[] proritySet);

        IList<IProduct> GetProductByPalletNo(string palletNo);

        /// <summary>
        /// 若ITCNDCheckQCHold存在IsHold=1 and (Code=Model或者Code=CUSTSN#)
        /// </summary>
        /// <param name="isHold"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
        bool CheckExistItcndCheckQcHold(string isHold, string[] codes);

        /// <summary>
        /// # 获得所有ITCNDCheckQCHold记录(按Code栏位排序)
        /// </summary>
        /// <returns></returns>
        IList<ITCNDCheckQCHoldDef> GetITCNDCheckQCHoldList();

        /// <summary>
        /// # 获得传入code(即ITCNDCheckQCHoldDef.Code)在数据库中已有的记录（返回值为null或者DataTable.Rows.Count<=0视为数据库中无code对应的记录）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<ITCNDCheckQCHoldDef> GetExistITCNDCheckQCHold(ITCNDCheckQCHoldDef condition);

        /// <summary>
        /// # 保存code对应数据到数据库（需要底层处理时加上cdt和udt）
        /// </summary>
        /// <param name="item"></param>
        void AddITCNDCheckQCHold(ITCNDCheckQCHoldDef item);

        /// <summary>
        /// # 删除code对应的数据：
        /// </summary>
        /// <param name="condition"></param>
        void RemoveITCNDCheckQCHold(ITCNDCheckQCHoldDef condition);

        /// <summary>
        /// # 更新对应的数据，连Code也可更新
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void ChangeITCNDCheckQCHold(ITCNDCheckQCHoldDef setValue, ITCNDCheckQCHoldDef condition);

        /// <summary>
        /// 操作[ITCNDCheckSetting]表，Get：获取该表记录.
        /// 若要所有记录,传一个new ITCNDCheckSettingDef()即可.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<ITCNDCheckSettingDef> GetExistITCNDCheckSetting(ITCNDCheckSettingDef condition);

        /// <summary>
        /// 操作[ITCNDCheckSetting]表，Insert：添加一条记录
        /// </summary>
        /// <param name="item"></param>
        void AddITCNDCheckSetting(ITCNDCheckSettingDef item);

        /// <summary>
        /// 操作[ITCNDCheckSetting]表，Delete：删除一条记录
        /// </summary>
        /// <param name="condition"></param>
        void RemoveITCNDCheckSetting(ITCNDCheckSettingDef condition);

        /// <summary>
        /// 操作[ITCNDCheckSetting]表，Update：更新某条记录
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void ChangeITCNDCheckSetting(ITCNDCheckSettingDef setValue, ITCNDCheckSettingDef condition);

        /// <summary>
        /// 由于FA Kitting Input产线测试结果不太良好，现将其部分业务功能提取到SP，直接调用SP去实现：
        /// 名称：op_FAKitting_MiddleWare.sql
        /// 参数：
        ///     @pid char(9) ,--ProductID
        ///     @boxid char(4), --4码BOXID
        ///     @editor char(30),
        ///     @kitpdline char(10)—PdLine
        ///     
        /// 结果：(返回的结果集还要看当时最新的存储过程部署代码)
        /// OK,Descr--执行成功
        /// NG,'1',Descr--执行失败
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="boxId"></param>
        /// <param name="editor"></param>
        /// <param name="kitPdLine"></param>
        /// <returns></returns>
        DataTable Callop_FAKitting_MiddleWare(string pid, string boxId, string editor, string kitPdLine);

        /// <summary>
        /// 1、	获取列表
        /// 功能：获取SnoDet_PoMo表中的记录列表
        /// 入参：无
        /// 出参：无
        /// 返回值：IList<DS>
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<SnoDetPoMoInfo> GetSnoDetPoMoInfoList(SnoDetPoMoInfo condition);

        /// <summary>
        /// 2、	添加记录
        /// 功能：向SnoDet_PoMo表中添加一条记录
        /// 入参：DS item
        /// 出参：无
        /// 返回值：void
        /// </summary>
        /// <param name="item"></param>
        void AddSnoDetPoMoInfo(SnoDetPoMoInfo item);

        /// <summary>
        /// 3、	删除记录
        /// 功能：从SnoDet_PoMo表中删除SnoId值为输入值的记录
        /// 入参：string id
        /// 出参：无
        /// 返回值：void 
        /// </summary>
        /// <param name="condition"></param>
        void DeleteSnoDetPoMoInfo(SnoDetPoMoInfo condition);

        /// <summary>
        /// 4、	更新记录
        /// 功能：在SnoDet_PoMo表中更新SnoId值为输入值的记录各字段为输入结构对应字段值，Cdt字段不更新，Udt更新为GetDate()得到的值
        /// 入参：string id, DS item
        /// 出参：无
        /// 返回值：void
        /// </summary>
        /// <param name="setValue"></param>
        /// <param name="condition"></param>
        void UpdateSnoDetPoMoInfo(SnoDetPoMoInfo setValue, SnoDetPoMoInfo condition);

        /// <summary>
        /// 5、	根据关键字查找记录是否已存在
        /// 功能：查找在SnoDet_PoMo表中是否存在SnoId值为输入值的记录
        /// 入参：string id
        /// 出参：无
        /// 返回值：bool
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        bool CheckExistSnoDetPoMoInfo(SnoDetPoMoInfo condition);

        /// <summary>
        /// Select Model,count(distinct ProductID) as Cnt 
        /// from ProductLog where Station='40' and Line=[PdLine] 
        /// and Cdt > convert(char(10),getdate(),111) 
        /// group by Model order by Model 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        IList<ModelPassQty> GetModelPassQty(string line, string station);

        /// <summary>
        /// select COUNT(*) from CTOBom nolock
        /// where rtrim(ISNULL(Tp,''))<>''
        /// and MPno = @ProdModel
        /// </summary>
        /// <param name="prodModel"></param>
        /// <returns></returns>
        bool IsSpecialCTO(string prodModel);

        void UpdateProductInfo(ProductInfo setValue, ProductInfo condition);

        void InsertProductInfo(ProductInfo item);

        void InsertQcStatus(ProductQCStatus item);

        /// <summary>
        /// 2、--取Pallet 实际包装完毕的Carton 数量
        /// SELECT @factCartonQty = COUNT(DISTINCT CartonSN) FROM Product NOLOCK WHERE PalletNo=@PalletNo AND ISNULL(CartonSN,'')<>'' 
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        int GetFactCartonQtyByPalletNo(string palletNo);
       
        /// <summary>
        /// 参考语句：SELECT COUNT(ProductID) FROM Product NOLOCK 
        /// WHERE DeliveryNo = @dn AND ISNULL(CartonSN,'') <> ''
        /// </summary>
        /// <param name="dn"></param>
        /// <returns></returns>
        int GetCombinedQtyByDN_WithCartonSNNotNull(string dn);

        /// <summary>
        /// SELECT @Pass = COUNT(DISTINCT ProductID) FROM ProductLog NOLOCK
        /// WHERE Station = @COOStation
        /// AND ProductID IN (SELECT ProductID FROM Product NOLOCK WHERE MO = @Mo)
        /// </summary>
        /// <param name="station"></param>
        /// <param name="mo"></param>
        /// <returns></returns>
        int GetCountOfProductLogByMo(string station, string mo);

        /// <summary>
        /// 1、获取QCStatus（Condition：QCStatus.ProductID and Tp in (‘PIA’,’PIA1’)），按Cdt倒序排列
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="tps"></param>
        /// <returns></returns>
        IList<ProductQCStatus> GetQCStatusOrderByCdtDesc(string productID, string[] tps);

        /// <summary>
        /// 2、SELECT @status = [Status] FROM [IMES_FA].[dbo].[QCStatus] where (Tp='PIA' or ‘PIA1’) and [ProductID]= @id order by [Udt]
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="tps"></param>
        /// <returns></returns>
        IList<ProductQCStatus> GetQCStatusOrderByUdt(string productID, string[] tps);

        /// <summary>
        /// SELECT @status = [Status] FROM [IMES_FA].[dbo].[QCStatus] where  (Tp='PIA' or ‘PIA1’) and [ProductID]= @id order by [Udt] DESC
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="tps"></param>
        /// <returns></returns>
        IList<ProductQCStatus> GetQCStatusOrderByUdtDesc(string productID, string[] tps);

        /// <summary>
        /// select top 1 @QCStatusID=ID, @PrdQCStatus = Status from QCStatus where ProductID = @ProductID order by Udt desc
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<ProductQCStatus> GetQCStatusOrderByUdtDesc(string productID);

        void UpdateForBindDNAndPallet(Product item);

        /// <summary>
        /// 3、	获取disntinct (Product_Part.PartSn的前5码)（Condition: Product_Part.PartType=’WIRELESS’ and ProductID=[ProductID]）
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<string> GetPartSnPrefixListFromProductPart(ProductPart condition);

        /// <summary>
        /// select distinct left(InfoValue,CHARINDEX('M',InfoValue)-3) from ProductInfo nolock
        ///     where InfoType = 'VGA'
        /// and RTRIM(ProductID) In ([Selected Product])
        /// and CHARINDEX('M',InfoValue)>3
        /// </summary>
        /// <param name="prodIds"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetInfoValuePrefixFromProductInfo(string[] prodIds, string infoType);

        /// <summary>
        /// select distinct left(PartSn,CHARINDEX ('M',PartSn)-3) from Product_Part nolock
        ///     where BomNodeType = 'MB'
        /// and RTRIM(ProductID) In ([Selected Product])
        /// and CHARINDEX('M',PartSn)>3
        /// </summary>
        /// <param name="prodIds"></param>
        /// <param name="bomNodeType"></param>
        /// <returns></returns>
        IList<string> GetPartSnPrefixFromProductPart(string[] prodIds, string bomNodeType);

        /// <summary>
        /// select COUNT(*) as Qty
        /// from ProductStatus a, Product b
        /// where a.ProductID = b.ProductID
        /// and b.Model = @Model
        /// and a.Station = @Station
        /// and b.CUSTSN <>''
        /// and a.Status = 1
        /// </summary>
        /// <param name="model"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        int GetCountOfProductByModelAndStation(string model, string station);

        /// <summary>
        /// 6．获取指定Model在指定Station的指定数量的ProductID 列表（有CustSN,且状态为1）
        /// Select top @ChangeQty  ProductID
        /// from ProductStatus a, Product b
        /// where a.ProductID = b.ProductID
        /// and b.Model = @Model
        /// and a.Station=@Station
        /// and b.CUSTSN <>''
        /// and a.Status = 1
        /// order by Udt desc
        /// </summary>
        /// <param name="model"></param>
        /// <param name="station"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IList<string> GetProductIdsByModelAndStation(string model, string station, int count);

        /// <summary>
        /// select @Win8 = MAX(Cdt) from ProductInfo 
        /// where ProductID = @Prod 
        /// and Upper(InfoType) in ('P/N','KEY','HASH')
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="infoTypes"></param>
        /// <returns></returns>
        DateTime GetNewestCdtFromProductInfo(string productId, string[] infoTypes);

        /// <summary>
        /// select @Log = Cdt from ProductLog  
        ///     where ProductID = @Prod 
        ///     and Station = '66'
        ///     and Status =‘1’
        /// order by Cdt desc
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="station"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        DateTime GetNewestCdtFromProductLog(string productId, string station, int status);

        IList<IProduct> GetProductListByCartonNo(string cartonNo);

        DataTable ExecSpForQuery(string dbConnString, string spName, params SqlParameter[] paramsArray);

        void ExecSpForNonQuery(string dbConnString, string spName, params SqlParameter[] paramsArray);

        /// <summary>
        /// select c.Defect + '    ' +c.Descr as Descr 
        /// FROM ProductRepair a, ProductRepair_DefectInfo b , DefectCode c
        /// where a.ID = b. ProductRepairID 
        /// and b.DefectCode = c.Defect
        /// and a.ID = @RepairID
        /// </summary>
        /// <param name="repairId"></param>
        /// <returns></returns>
        IList<string> GetDefectForProductRepair(int repairId);

        /// <summary>
        /// SELECT b.Remark
        /// FROM ProductRepair AS a INNER JOIN ProductRepair_DefectInfo AS b ON a.ID = b.ProductRepairID
        /// where a.ID = @RepairID
        /// </summary>
        /// <param name="repairId"></param>
        /// <returns></returns>
        IList<string> GetRemarkListOfProductRepairDefectInfo(int repairId);

        /// <summary>
        /// select TOP 1 ID from ProductRepair nolock
        /// where ProductID = @Prd
        /// and Status = '1'
        /// order by Udt desc
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int GetNewestProductRepairId(string productId);

        /// <summary>
        /// select TOP 1 ID from ProductRepair nolock
        /// where ProductID = @Prd
        /// order by Udt desc
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int GetNewestProductRepairIdRegardlessStatus(string productId);

        /// <summary>
        /// 功能：获取当天成功通过某站的指定Model的product数
        /// Sql：
        /// SELECT COUNT(DISTINCT ProductID) FROM ProductLog NOLOCK 
        ///     WHERE Model = @Model
        ///        AND Station = @Station
        ///        AND CONVERT(char(10), Cdt, 121) = CONVERT(char(10), GETDATE(), 121)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        int GetCountOfCurrentDayByModelAndStation(string model, string station);

        /// <summary>
        ///    功能：获取当天成功通过UnitWeight站的指定Model的所有product的平均重量
        /// SELECT AVG(UnitWeight)
        ///        FROM Product NOLOCK
        ///        WHERE ProductID IN (SELECT DISTINCT ProductID FROM ProductLog NOLOCK 
        ///                             WHERE Model = @Model
        ///                                 AND Station = @Station
        ///                                 AND CONVERT(char(10), Cdt, 121) = CONVERT(char(10), GETDATE(), 121))
        /// </summary>
        /// <param name="model"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        decimal GetAverageModelWeightOfCurrentDay(string model, string station);

        void BackUpProduct(string uEditor, Product eqCondition, Product neqCondition);

        void BackUpProductInfo(string uEditor, ProductInfo eqCondition, ProductInfo neqCondition);

        void BackUpProductPart(string uEditor, ProductPart eqCondition, ProductPart neqCondition);

        void DeleteProductInfo(ProductInfo eqCondition, ProductInfo neqCondition);

        void DeleteProductPart(ProductPart eqCondition, ProductPart neqCondition);

        void InsertUnitWeightLog(UnitWeightLog item);

        void InsertFaItCnDefectCheckInfo(FaItCnDefectCheckInfo item);

        void DeleteFaItCnDefectCheckInfo(FaItCnDefectCheckInfo condition);

        void UpdateFaItCnDefectCheckInfo(FaItCnDefectCheckInfo setValue, FaItCnDefectCheckInfo condition);

        IList<FaItCnDefectCheckInfo> GetFaItCnDefectCheckInfoList(FaItCnDefectCheckInfo condition);

        IList<string> GetProdIDListByMO(string MOId);

        /// <summary>
        /// select distinct Cause from ProductRepair_DefectInfo where ProductRepairID = @RepairID
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        IList<string> GetCauseListByProductRepairId(int repId);

        /// <summary>
        /// select top 1 Station from ProductLog where ProductID = @ProductID and Station <>'76' and Station <>'7P' and Station<>’45’ order by Cdt desc
        /// </summary>
        /// <param name="eqCondition"></param>
        /// <param name="neqCondition"></param>
        /// <returns></returns>
        IList<ProductLog> GetProductLogList(ProductLog eqCondition, string[] neqStations);
     
        /// <summary>
        /// 9. 针对指定的Defect,以相同Defect数作条件创建SA的Alarm（无返回值，参数为AlarmSetting对象，SQL语句变量与AlarmSetting对象成员变量相对应）。
        ///   名称：
        ///   方法：当@type=1 and @defectType=0  @curTime=GETDATE()
        /// INSERT [Alarm] 
        /// SELECT	'FA', DATEADD(HOUR, -@period, @curTime), @curTime, @id, a.Line, 
        ///        a.Station, d.Family, b.DefectCodeID  AS Defect, 'ALM2' AS ReasonCode, 
        ///        'Defect: '+ b.DefectCodeID+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.ID))+' >= '+ @defectQty AS Reason, 'Created', @curTime
        /// FROM ProductTestLogBack a 
        /// LEFT OUTER JOIN ProductTestLogBack_DefectInfo b 
        /// ON a.ID=b.ProductTestLogBackID 
        /// INNER JOIN Product c 
        /// ON a.ProductID=c.ProductID 
        /// INNER JOIN Model d 
        /// ON c.Model=d.Model 
        /// WHERE a.Status=0 
        /// AND DATEDIFF(HOUR, a.Cdt, @curTime)<=@period 
        /// AND a.Station=@station 
        /// AND b.TriggerAlarm <>1 
        /// AND b.DefectCodeID+',' LIKE @defects 
        /// AND d.Family=@family 
        /// GROUP BY a.Line, a.Station, d.Family, b.DefectCodeID 
        /// HAVING COUNT(a.ID)>= @defectQty
        ///         注：AlarmSetting.defects是","分割的字符串。b.DefectCodeID+',' like @defects用in更好。
        /// </summary>
        /// <param name="alarmSetting"></param>
        int CreateAlarmWithSpecifiedDefectForFA(AlarmSettingInfo alarmSetting);

        /// <summary>
        /// 最后，将相关LogDefect记录设置标记以阻止其参与下一轮报警统计
        /// UPDATE ProductTestLogBack_DefectInfo a 
        /// SET a.TriggerAlarm=1 
        /// WHERE a.ID IN 
        /// (
        ///     SELECT a.ID 
        ///     FROM Alarm b 
        ///     LEFT OUTER JOIN ProductTestLogBack c 
        ///     ON b.Line=c.Line 
        ///     AND b.Station=c.Station 
        ///     LEFT OUTER JOIN a 
        ///     ON a.ProductTestLogBackID=c.ID 
        ///     INNER JOIN Product d 
        ///     ON d.ProductID=c.ProductID 
        ///     INNER JOIN Model e 
        ///     ON d.Model=e.Model 
        ///     WHERE b.Stage='FA' 
        ///     AND b.Cdt>@curTime 
        ///     AND c.Status=0 
        ///     AND c.Cdt>= DATEADD(HOUR, -@period, @curTime) 
        ///     AND c.Cdt<=@curTime 
        ///     AND a.DefectCodeID=b.Defect 
        ///     AND e.Family=b.Family
        /// )
        /// </summary>
        /// <param name="alarmSetting"></param>
        void UpdateForCreateAlarmWithDefectForFA(AlarmSettingInfo alarmSetting, int alarm_id);

		/// <summary>
        /// 10. 针对Exclude指定的Defect,以相同Defect数作条件创建SA的Alarm（无返回值，参数为AlarmSetting对象，SQL语句变量与AlarmSetting对象成员变量相对应）。
        ///   名称：
        ///   方法：当@type=1 and @defectType=1 and @defects is not null and @defects<>''
        /// INSERT [Alarm] 
        /// SELECT 	'FA', DATEADD(HOUR, -@period, @curTime), @curTime, @id, a.Line, 
        ///        a.Station, d.Family, b.DefectCodeID AS Defect, 'ALM2' AS ReasonCode, 
        ///        'Defect: '+ b.DefectCodeID+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.ID))+' >= '+ @defectQty AS Reason, 'Created', @curTime 
        /// FROM ProductTestLogBack a 
        /// LEFT OUTER JOIN ProductTestLogBack_DefectInfo b 
        /// ON a.ID=b.ProductTestLogBackID 
        /// INNER JOIN Product c 
        /// ON a.ProductID=c.ProductID 
        /// INNER JOIN Model d 
        /// ON c.Model=d.Model 
        /// WHERE a.Status=0 
        /// AND DATEDIFF(HOUR, a.Cdt, @curTime)<=@period 
        /// AND a.Station=@station 
        /// AND b.TriggerAlarm<>1 
        /// AND b.DefectCodeID+',' NOT LIKE @defects 
        /// AND d.Family=@family 
        /// GROUP BY a.Line, a.Station, d.Family, b.DefectCodeID 
        /// HAVING COUNT(a.ID)>=@defectQty
		/// </summary>
		/// <param name="alarmSetting"></param>
        int CreateAlarmWithExcludedDefectForFA(AlarmSettingInfo alarmSetting);

        /// <summary>
        /// 11. 针对All Defect,以相同Defect数作条件（无返回值，参数为AlarmSetting对象，SQL语句变量与AlarmSetting对象成员变量相对应）。
        ///    名称：
        ///    方法：当@type=1 and @defectType=1 and (@defects is null or @defects=’’)
        /// INSERT [Alarm] 
        /// SELECT 'FA', DATEADD(HOUR, -@period, @curTime), @curTime, @id, a.Line, 
        ///        a.Station, d.Family AS Family, b.DefectCodeID AS Defect, 'ALM2' AS ReasonCode, 
        ///        'Defect: '+ b.DefectCodeID+' / Qty:'+ CONVERT(VARCHAR,COUNT(a.ID))+' >= '+ @defectQty AS Reason, 'Created', @curTime
        /// FROM ProductTestLogBack a 
        /// LEFT OUTER JOIN ProductTestLogBack_DefectInfo b 
        /// ON a.ID=b.ProductTestLogBackID 
        /// INNER JOIN Product c 
        /// ON a.ProductID=c.ProductID 
        /// INNER JOIN Model d 
        /// ON c.Model=d.Model 
        /// WHERE a.Status=0 
        /// AND DATEDIFF(HOUR, a.Cdt, @curTime)<=@period 
        /// AND a.Station=@station 
        /// AND b.TriggerAlarm<>1 
        /// AND d.Family=@family 
        /// GROUP BY a.Line, a.Station, d.Family, b.DefectCodeID 
        /// HAVING COUNT(a.ID)>=@defectQty
        /// </summary>
        /// <param name="alarmSetting"></param>
        int CreateAlarmWithAllDefectForFA(AlarmSettingInfo alarmSetting);

        /// <summary>
        /// 12. 以良率作条件创建SA的Alarm（无返回值，参数为AlarmSetting对象，SQL语句变量与AlarmSetting对象成员变量相对应）。 
        ///     名称：
        ///    方法：当@type=0 @curTime=GETDATE()
        /// SELECT a.Line, COUNT(a.ID) AS Total INTO #temp 
        /// FROM ProductTestLogBack a, Product b, Model c 
        /// WHERE DATEDIFF(HOUR, a.Cdt, @curTime)<=@period 
        /// AND a.Station=@station 
        /// AND a.ProductID=b.ProductID 
        /// AND b.Model=c.Model 
        /// AND c.Family=@family 
        /// GROUP BY a.Line 
        /// HAVING COUNT(a.ID)>=@minQty
        /// INSERT [Alarm] 
        /// SELECT 'FA', DATEADD(HOUR, -@period, @curTime), @curTime, @id, a.Line, 
        ///         b.Station, d.Family, '' AS Defect, 'ALM1' AS ReasonCode, 
        ///         CONVERT(VARCHAR,COUNT(b.ID))+' / '+ CONVERT(VARCHAR,a.Total)+' < '+ CONVERT(VARCHAR,@yieldRate) +'%' AS Reason, 'Created', GETDATE()
        /// FROM #temp a, ProductTestLogBack b, Product c, Model d 
        /// WHERE a.Line=b.Line 
        /// AND DATEDIFF(HOUR, b.Cdt, @curTime)<=@period 
        /// AND b.Station=@station 
        /// AND b.ProductID=c.ProductID 
        /// AND c.Model=d.Model 
        /// AND d.Family=@family 
        /// GROUP BY a.Line, b.Station, d.Family, a.Total  
        /// HAVING COUNT(b.ID)*100<@yieldRate*a.Total
        /// DROP TABLE #temp 
        /// </summary>
        /// <param name="alarmSetting"></param>
        void CreateAlarmWithYieldForFA(AlarmSettingInfo alarmSetting);

        /// <summary>
        /// 1.
        /// 获取Win8机器ImageDownLoad上传的最新的P/N：
        /// @ImgPN 
        /// ProductInfo.InfoValue 
        /// Condtion: Uppder(ProductInfo.InfoType) = ‘P/N’ and ProductID=[ProductID] order by Udt desc
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="itemTypes"></param>
        /// <returns></returns>
        IList<ProductInfo> GetProductInfoListUpperCaseItemType(string proid, IList<string> itemTypes);

        /// <summary>
        /// select distinct c.Station + ' ' +c.Descr as Text, c.Station as Value from ProductRepair a 
        /// inner join ProductRepair_DefectInfo b 
        /// on a.ID = b.ProductRepairID
        /// left join Station c 
        /// on b.ReturnStn = c.Station
        /// where a.ProductID = '@Product'
        /// and a.Status = '0'
        /// order by c.Station
        /// </summary>
        /// <param name="product"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<StationInfo> GetStationInfoListFromProductRepair(string product, int status);

        /// <summary>
        /// SELECT * FROM Product NOLOCK WHERE CartonSN = @CartonNo AND PalletNo = ''
        /// </summary>
        /// <param name="eqCondition"></param>
        /// <param name="isNullCondition"></param>
        /// <returns></returns>
        IList<IProduct> GetProductInfoListByConditions(IProduct eqCondition, IProduct isNullCondition);

        /// <summary>
        /// SELECT * FROM Product NOLOCK WHERE CartonSN = @CartonNo AND ISNULL(PalletNo, '') <> ''
        /// </summary>
        /// <param name="eqCondition"></param>
        /// <param name="notNullCondition"></param>
        /// <returns></returns>
        IList<IProduct> GetProductInfoListByConditionsNotNull(IProduct eqCondition, IProduct notNullCondition);

        /// <summary>
        /// SELECT * FROM [IPC_IMES2012_0330].[dbo].[Product] Where PizzaID In (select PizzaID from [IPC_IMES2012_0330].[dbo].[Pizza_Part] where PartSn=@PartSn)
        /// </summary>
        /// <param name="partSn"></param>
        /// <returns></returns>
        IList<IProduct> GetProductInfoListByPizzaPartSn(string partSn);

        /// <summary>
        /// 其中idList为ProductID列表，rsList为ReturnStation列表，如果rsList为空或元素个数为0，则执行以下的前一段sql，否则执行后一段sql，两段sql的唯一差别在于Station的条件，参数和两段sql的差别均已用红色粗体标出：
        /// 不含ReturnStation条件执行的sql：
        /// select 
        /// CONVERT (varchar,a.Cdt,111) as IssueDate,
        /// a.Line as Line,
        /// b.ID,  
        /// b.Action  as [Action],
        /// b.Mark as Mark,
        /// c.Family as Family, 
        /// c.Model as Model,
        /// d.Defect as DefectCode, 
        /// d.Descr as DefectDescription,
        /// e.Code + ' ' + e.Description as RootCause,
        /// f.Description as Owner,
        /// g.CUSTSN as SN,  
        /// g.ProductID as ProdID 
        ///     from ProductRepair a with (nolock), 
        ///     join ProductRepair_DefectInfo b with (nolock) on a.ID = b.ProductRepairID
        ///     join Model c with (nolock) on a.Model = c.Model 
        ///     left join DefectCode d on b.DefectCode = d.Defect
        ///     left join DefectInfo e on b.Cause = e.Code and e.Type = 'FACause'
        ///     left join DefectInfo f on b.Obligation = f.Code and f.Type = 'Obligation'
        ///     join Product g with (nolock) on a.ProductID = g.ProductID 
        ///     where a.ProductID  in (<idList>)
        ///     and a.Status = '1'
        /// order by CONVERT (varchar,a.Cdt,111), a.Line, c.Family, c.Model, g.CUSTSN
        /// 包含ReturnStation条件执行的sql：
        /// select 
        /// CONVERT (varchar,a.Cdt,111) as IssueDate,
        /// a.Line as Line,
        /// b.ID,  
        /// b.Action  as [Action],
        /// b.Mark as Mark,
        /// c.Family as Family, 
        /// c.Model as Model,
        /// d.Defect as DefectCode, 
        /// d.Descr as DefectDescription,
        /// e.Code + ' ' + e.Description as RootCause,
        /// f.Description as Owner,
        /// g.CUSTSN as SN,  
        /// g.ProductID as ProdID 
        ///     from ProductRepair a with (nolock), 
        ///     join ProductRepair_DefectInfo b with (nolock) on a.ID = b.ProductRepairID
        ///     join Model c with (nolock) on a.Model = c.Model 
        ///     left join DefectCode d on b.DefectCode = d.Defect
        ///     left join DefectInfo e on b.Cause = e.Code and e.Type = 'FACause'
        ///     left join DefectInfo f on b.Obligation = f.Code and f.Type = 'Obligation'
        ///     join Product g with (nolock) on a.ProductID = g.ProductID 
        ///     where a.ProductID  in (<idList>)
        /// and a.Station in (<rsList>)
        ///     and a.Status = '1'
        /// order by CONVERT (varchar,a.Cdt,111), a.Line, c.Family, c.Model, g.CUSTSN
        /// </summary>
        /// <param name="idList"></param>
        /// <param name="rsList"></param>
        /// <returns></returns>
        DataTable GetProductRepairInfoListDataTable(IList<string> idList, IList<string>rsList);

        /// <summary>
        /// 功能：将ProductRepair_DefectInfo表中指定id的记录备份到ProductRepair_DefectInfo_BackUp中，其中新增三个字段值为：
        /// BackUpProductID = <prodID>
        /// BackUpEditor = <editor>
        /// BackUpCdt = GETDATE()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prodID"></param>
        /// <param name="editor"></param>
        void BackupProductRepairDefectInfo(int id, string prodID, string editor);
        
        /// <summary>
        /// select * from ProductRepair_DefectInfo where ID=<id>;
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<RepairInfo> GetProductRepairDefectInfo(RepairInfo condition);

        void UpdateProducts(Product setValue, Product condition);
        
        /// <summary>
        /// Select top 1 @Verdor = Vendor from SupplierCode  where Vendor in ('@2TG') and @ast like  REPLACE(Code,'X','_')
        /// </summary>
        /// <param name="vendors"></param>
        /// <param name="ast"></param>
        /// <returns></returns>
        SupplierCodeInfo GetSupplierCodeByVendorsAndAstLike(string[] vendors, string ast);

        /// <summary>
        /// 检查BoxId是否已经被另一个Product占用了
        /// </summary>
        /// <param name="item"></param>
        bool CheckTheBoxId(IProduct item);

        /// <summary>
        /// select prd.* from Alarm a,ProductTestLogBack p,ProductTestLogBack_DefectInfo pdi,Product prd 
        /// where a.id=alarm_info.id and a.Line=p.Line and a.Station = p.Station and p.Status=1 
        /// and (p.Cdt > alarm_info. StartTime and p.Cdt <= alarm_info. EndTime) and p.ID=pdi.ProductTestLogBackID 
        /// and pdi.TriggerAlarm=1 and p.ProductID=prd.ProductID
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        IList<IProduct> GetProductWithAlarm(AlarmInfo condition);

        /// <summary>
        /// select * from Product_Part where ProductID = @ProductID and PartType like '%CPU%'
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="partTypeLike"></param>
        /// <returns></returns>
        IList<ProductPart> GetProductPartByPartLikeType(string productId, string partTypeLike);

        void BackUpProductStatusByCarton(string carton, string uEditor);

        void BackUpProductByDnPure(string dn, string uEditor);

        void BackUpProductInfoByDnPure(string dn, string uEditor, IList<string> infoTypeNames);

        void BackUpProductStatusByDnPure(string dn, string uEditor);

        /// <summary>
        /// select * from ProductInfo where InfoType ='BoxId' and
        /// ProductID in (select ProductID from Product where PalletNo = @PalletNo)
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        IList<ProductInfo> GetProductInfoListByPalletNo(ProductInfo condition, string palletNo);

        /// <summary>
        /// EXISTS(SELECT * FROM CartonInfo a (NOLOCK), Product b (NOLOCK)
        /// WHERE a.CartonNo = b.CartonSN
        /// AND b.PalletNo = @PalletNo 
        /// AND a.InfoType = 'BoxId')
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        bool CheckExistCartonInfoWithProduct(string palletNo, string infoType);

        /// <summary>
        /// EXISTS(SELECT * FROM Product b (NOLOCK) LEFT JOIN CartonInfo a (NOLOCK)
        /// ON a.CartonNo = b.CartonSN 
        /// AND b.PalletNo = @PalletNo 
        /// AND a.InfoType = 'BoxId'
        /// WHERE ISNULL(a.InfoValue, '') = '')
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        bool CheckExistProductWithCartonInfo(string palletNo, string infoType);

        /// <summary>
        /// SELECT @BoxId = a.InfoValue FROM CartonInfo a (NOLOCK), Product b (NOLOCK)
        /// WHERE a.CartonNo = b.CartonSN
        /// AND b.PalletNo = @PalletNo
        /// AND a.InfoType = 'BoxId'
        /// GROUP BY InfoValue
        /// HAVING COUNT(*) > 1
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetInfoValueFromCartonInfoWithProduct(string palletNo, string infoType);

        /// <summary>
        /// SELECT * FROM (SELECT * FROM Product WHERE PalletNo = @PalletNo) b
        /// LEFT JOIN (SELECT * FROM CartonInfo WHERE InfoType = 'BoxId') a
        /// ON b.CartonSN = a.CartonNo WHERE ISNULL(a.InfoValue, '') = ''
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        bool CheckExistProductWithCartonInfo2(string palletNo, string infoType);

        /// <summary>
        /// SELECT @BoxId = a.InfoValue FROM (SELECT DISTINCT a.CartonNo, a.InfoValue FROM CartonInfo a (NOLOCK), Product b (NOLOCK)
        /// WHERE a.CartonNo = b.CartonSN
        /// AND a.InfoType = 'BoxId'
        /// AND b.PalletNo = @PalletNo) a
        /// GROUP BY InfoValue
        /// HAVING COUNT(*) > 1
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoType"></param>
        /// <returns></returns>
        IList<string> GetInfoValueList(string palletNo, string infoType);

        /// <summary>
        /// select a.* from SnoDet_PoMo a 
        /// left join Delivery b
        /// on a.Delivery = b.DeliveryNo
        /// where isnull(b.Status,'') <> '98'
        /// order by a.SnoId
        /// </summary>
        /// <returns></returns>
        IList<SnoDetPoMoInfo> GetSnoDetPoMoInfoList_NOT98DN();

        void PersistUpdateProductWithoutUdt(IProduct prdct);

        /// <summary>
        /// select COUNT(distinct InfoValue) from ProductInfo where InfoType in ('BoxId', 'UCC') and  InfoValue <> ''
        /// ProductID in (select ProductID from Product where PalletNo = @PalletNo)
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="infoTypes"></param>
        /// <returns></returns>
        int GetCountOfInfoValueInProductInfo(string palletNo, string[] infoTypes);

        void CallOp_FAUnpackProduct(string prodId, string restation, string editor, string isprint);
        #region . Locks For All Repository .

        Guid GrabLockByTransThread(string type, string lockValue, ConcurrentLocksInfo clck);

        void ReleaseLockByTransThread(string type, Guid id);

        Guid GrabLockBySeperateSegment(string type, string lockValue, ConcurrentLocksInfo clck);

        void ReleaseLockBySeperateSegment(string type, Guid id);

        #endregion

        #region . Defered .

        void CallOp_FAUnpackProductDefered(IUnitOfWork uow, string prodId, string restation, string editor, string isprint);

        void AddOneIProductPartDefered(IUnitOfWork uow, IProductPart item);

        void DeleteProductInfoDefered(IUnitOfWork uow, string[] prodIds, string infoType);

        void UpdatePizzaIdForProductDefered(IUnitOfWork uow, string proId, string pizzaId);

        void ChangeLabelKittingCodeDefered(IUnitOfWork uow, LabelKittingCode obj, string oldCode, string oldType);

        void AddLabelKittingCodeDefered(IUnitOfWork uow, LabelKittingCode obj);

        void RemoveLabelKittingCodeDefered(IUnitOfWork uow, string code, string type);

        void UpdateSno1ForSpecialDetDefered(IUnitOfWork uow, string sno1, int[] ids);

        void InsertSpecialDetDefered(IUnitOfWork uow, SpecialDetInfo item);

        void AddLightNoDefered(IUnitOfWork uow, WipBuffer item);

        void UpdateLightNoDefered(IUnitOfWork uow, WipBuffer item);

        void DeleteLightNoDefered(IUnitOfWork uow, int id);

        void DeleteTmpKitDefered(IUnitOfWork uow, string curLine, string type);

        void ImportTmpKitDefered(IUnitOfWork uow, IList<TmpKitInfoDef> items);

        void ExecOpKittingAutoCheckDefered(IUnitOfWork uow, string family, string loc, string line);

        void ExecOpKittingLocCheckDefered(IUnitOfWork uow, string pdline, string partType);

        void ExecOpPAKKitLocFVDefered(IUnitOfWork uow, string pdline);

        void AddFAFloatLocationInfoDefered(IUnitOfWork uow, FAFloatLocationInfo item);

        void UpdateFAFloatLocationInfoDefered(IUnitOfWork uow, FAFloatLocationInfo item, int id);

        void DeleteFAFloatLocationInfoDefered(IUnitOfWork uow, int id);

        void AddPAKitLocDefered(IUnitOfWork uow, PAKitLoc item);

        void UpdatePAKitLocDefered(IUnitOfWork uow, PAKitLoc item);

        void UpdatePAKitLocDefered(IUnitOfWork uow, PAKitLoc setValue, PAKitLoc condition);

        void DeletePAKitLocDefered(IUnitOfWork uow, int id);

        void InsertProductBTDefered(IUnitOfWork uow, ProductBTInfo item);

        void DeleteProductBTByObjAndConditionDefered(IUnitOfWork uow, ProductBTInfo cond);

        void CallRpt_ITCNDTS_SET_IMAGEDOWN_14Defered(IUnitOfWork uow, string cpqsno, string flag, string version);

        void DeleteRealModelDataDefered(IUnitOfWork uow, string pdline, string type);

        void DeleteMPInterfaceDefered(IUnitOfWork uow, object key);

        void UpdateUnPackProductStatusByDeliveryNoDefered(IUnitOfWork uow, ProductStatus newStatus, string dn);

        void UpdateUnPackProductStatusByDnDefered(IUnitOfWork uow, ProductStatus newStatus, string dn);

        void WriteUnPackProductLogByDeliveryNoDefered(IUnitOfWork uow, string dn, ProductLog newLog);

        void WriteUnPackProductLogByDnDefered(IUnitOfWork uow, string dn, ProductLog newLog);

        void UnPackProductInfoByDeliveryNoDefered(IUnitOfWork uow, string infoType, string dn);

        void UnPackProductInfoByDeliveryNoAndInfoTypeDefered(IUnitOfWork uow, string infoType, string dn);

        void UnPackProductByDeliveryNoDefered(IUnitOfWork uow, string dn);

        void UnPackProductByDnDefered(IUnitOfWork uow, string dn);

        void InsertTSModelDefered(IUnitOfWork uow, TsModelInfo item);

        void DeleteTsModelDefered(IUnitOfWork uow, string mark, string model);

        void UpdateSpecialDetSno1Defered(IUnitOfWork uow, string sno1, string tp, string snoid);

        void AddSpecialDetInfoDefered(IUnitOfWork uow, SpecialDetInfo item);

        void UpdateProductsForUnboundDefered(IUnitOfWork uow, string dn);

        void DeleteProductInfoForUnboundDefered(IUnitOfWork uow, string infoType, string dn);

        void DeleteProductPartByPartTypeAndDescrLikeDefered(IUnitOfWork uow, string prodId, string partType, string descrLike);

        void DeleteProductPartByLikePartTypeDefered(IUnitOfWork uow, string prodId, string partTypePrefix);

        void DeleteRunInTimeControlByTypeCodeAndTeststationDefered(IUnitOfWork uow, string type, string code, string teststation);

        void ChangeToBTDefferedDefered(IUnitOfWork uow, string modelName, string editor, string bt);

        void ChangeToNonBTDefered(IUnitOfWork uow, string modelName);

        void AddFaPaLightStationItemDefered(IUnitOfWork uow, FaPaLightstInfo item);

        void DeleteFaPaLightStationItemDefered(IUnitOfWork uow, int id);

        void UpdateFaPaLightStationItemDefered(IUnitOfWork uow, FaPaLightstInfo item);

        void AddKittingBoxSNItemDefered(IUnitOfWork uow, KittingBoxSNInfo item);

        void DeleteProductPartByPartSnDefered(IUnitOfWork uow, string prodId, string partSn);

        void DeleteProductPartByPartNoDefered(IUnitOfWork uow, string prodId, string partNo);

        void UpdateProductStatusesDefered(IUnitOfWork uow, ProductStatus setValue, string[] prodIds);

        void InsertProductLogsDefered(IUnitOfWork uow, ProductLog[] items);

        void UpdateProductsDefered(IUnitOfWork uow, Product setValue, string[] prodIds);

        void DeleteProductPartsDefered(IUnitOfWork uow, string[] prodIds, ProductPart otherCondition);

        void BackUpProductDefered(IUnitOfWork uow, string productId, string uEditor);

        void BackUpProductStatusDefered(IUnitOfWork uow, string productId, string uEditor);

        void BackUpProductPartDefered(IUnitOfWork uow, string productId, string uEditor, string partType, string descrLike);

        void BackUpProductPartDefered(IUnitOfWork uow, string[] prodIds, ProductPart otherCondition, string uEditor);

        void RemoveProductPartByPartTypeAndDescrLikeDefered(IUnitOfWork uow, string productId, string partType, string descrLike);

        void BackUpProductPartDefered(IUnitOfWork uow, string productId, string uEditor);

        void BackUpProductInfoDefered(IUnitOfWork uow, string productId, string uEditor, string infoTypeName);

        void BackUpProductInfoDefered(IUnitOfWork uow, string productId, string uEditor);

        void BackUpProductByDnDefered(IUnitOfWork uow, string dn, string uEditor);

        void BackUpProductStatusByDnDefered(IUnitOfWork uow, string dn, string uEditor);

        void BackUpProductPartByDnDefered(IUnitOfWork uow, string dn, string uEditor);

        void BackUpProductInfoByDnDefered(IUnitOfWork uow, string dn, string uEditor, IList<string> infoTypeNames);

        void UpdateProductStatusByPalletDefered(IUnitOfWork uow, string palletSN, ProductStatus newStatus, int newTestFailCount, StationStatus newStationStatus);

        void DeleteKittingBoxSNItemDefered(IUnitOfWork uow, KittingBoxSNInfo condition);

        void UpdateProductPartDefered(IUnitOfWork uow, ProductPart setValue, ProductPart condition);

        void UpdateProductRepairDefectInfoDefered(IUnitOfWork uow, RepairInfo setValue, RepairInfo condition);

        void BackUpProductPartByBomNodeTypeAndDescrLikeDefered(IUnitOfWork uow, string productId, string uEditor, string bomNodeType, string descrLike);

        void RemoveProductPartByBomNodeTypeAndDescrLikeDefered(IUnitOfWork uow, string productId, string bomNodeType, string descrLike);

        void BackUpProductPartByBomNodeTypeDefered(IUnitOfWork uow, string productId, string uEditor, string bomNodeType);

        void RemoveProductPartByBomNodeTypeDefered(IUnitOfWork uow, string productId, string bomNodeType);

        void BackUpProductPartByPartTypeDefered(IUnitOfWork uow, string productId, string uEditor, string partType);

        void RemoveProductPartByPartTypeDefered(IUnitOfWork uow, string productId, string partType);

        void BackUpProductPartByDescrLikeDefered(IUnitOfWork uow, string productId, string uEditor, string descrLike);

        void RemoveProductPartByDescrLikeDefered(IUnitOfWork uow, string productId, string descrLike);

        void InsertKittingLogInfoDefered(IUnitOfWork uow, KittingLogInfo item);

        void InsertKittingLogInfosFromKittingLocationFaXDefered(IUnitOfWork uow, KittingLogInfo item, KittingLocationFaXInfo condition, int[] proritySet);

        void AddITCNDCheckQCHoldDefered(IUnitOfWork uow, ITCNDCheckQCHoldDef item);

        void RemoveITCNDCheckQCHoldDefered(IUnitOfWork uow, ITCNDCheckQCHoldDef condition);

        void ChangeITCNDCheckQCHoldDefered(IUnitOfWork uow, ITCNDCheckQCHoldDef setValue, ITCNDCheckQCHoldDef condition);

        void AddITCNDCheckSettingDefered(IUnitOfWork uow, ITCNDCheckSettingDef item);

        void RemoveITCNDCheckSettingDefered(IUnitOfWork uow, ITCNDCheckSettingDef condition);

        void ChangeITCNDCheckSettingDefered(IUnitOfWork uow, ITCNDCheckSettingDef setValue, ITCNDCheckSettingDef condition);

        void AddSnoDetPoMoInfoDefered(IUnitOfWork uow, SnoDetPoMoInfo item);

        void DeleteSnoDetPoMoInfoDefered(IUnitOfWork uow, SnoDetPoMoInfo condition);

        void UpdateSnoDetPoMoInfoDefered(IUnitOfWork uow, SnoDetPoMoInfo setValue, SnoDetPoMoInfo condition);

        void UpdateProductInfoDefered(IUnitOfWork uow, ProductInfo setValue, ProductInfo condition);

        void InsertProductInfoDefered(IUnitOfWork uow, ProductInfo item);

        void InsertQcStatusDefered(IUnitOfWork uow, ProductQCStatus item);

        void UpdateForBindDNAndPalletDefered(IUnitOfWork uow, Product item);

        InvokeBody ExecSpForQueryDefered(IUnitOfWork uow, string dbConnString, string spName, params SqlParameter[] paramsArray);

        void ExecSpForNonQueryDefered(IUnitOfWork uow, string dbConnString, string spName, params SqlParameter[] paramsArray);

        void BackUpProductDefered(IUnitOfWork uow, string uEditor, Product eqCondition, Product neqCondition);

        void BackUpProductInfoDefered(IUnitOfWork uow, string uEditor, ProductInfo eqCondition, ProductInfo neqCondition);

        void BackUpProductPartDefered(IUnitOfWork uow, string uEditor, ProductPart eqCondition, ProductPart neqCondition);

        void DeleteProductInfoDefered(IUnitOfWork uow, ProductInfo eqCondition, ProductInfo neqCondition);

        void DeleteProductPartDefered(IUnitOfWork uow, ProductPart eqCondition, ProductPart neqCondition);

        void InsertUnitWeightLogDefered(IUnitOfWork uow, UnitWeightLog item);

        void InsertFaItCnDefectCheckInfoDefered(IUnitOfWork uow, FaItCnDefectCheckInfo item);

        void DeleteFaItCnDefectCheckInfoDefered(IUnitOfWork uow, FaItCnDefectCheckInfo condition);

        void UpdateFaItCnDefectCheckInfoDefered(IUnitOfWork uow, FaItCnDefectCheckInfo setValue, FaItCnDefectCheckInfo condition);

        void CreateAlarmWithSpecifiedDefectForFADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void UpdateForCreateAlarmWithDefectForFADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void CreateAlarmWithExcludedDefectForFADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void CreateAlarmWithAllDefectForFADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void CreateAlarmWithYieldForFADefered(IUnitOfWork uow, AlarmSettingInfo alarmSetting);

        void AddAstRuleInfoDefered(IUnitOfWork uow, AstRuleInfo item);

        void DeleteAstRuleRuleDefered(IUnitOfWork uow, AstRuleInfo condition);

        void BackUpProductPartByDnDefered(IUnitOfWork uow, string uEditor, ProductPart eqCondition, ProductPart neqCondition);

        void BackupProductRepairDefectInfoDefered(IUnitOfWork uow, int id, string prodID, string editor);

        void UpdateProductsDefered(IUnitOfWork uow, Product setValue, Product condition);

        InvokeBody CheckTheBoxIdDefered(IUnitOfWork uow, IProduct item);
        void BackUpProductStatusByCartonDefered(IUnitOfWork uow, string carton, string uEditor);

        void BackUpProductByDnPureDefered(IUnitOfWork uow, string dn, string uEditor);

        void BackUpProductInfoByDnPureDefered(IUnitOfWork uow, string dn, string uEditor, IList<string> infoTypeNames);

        void BackUpProductStatusByDnPureDefered(IUnitOfWork uow, string dn, string uEditor);

        #endregion

        #region . OnTrans .

        IList<IProduct> GetProductListByDeliveryNo_OnTrans(string dn);

        IList<IProduct> GetProductByCustomSn_OnTrans(string customerSn);

        int GetCombinedQtyByDN_OnTrans(string deliveryNo);

        //void GetCombinedQtyByDN_OnTrans_WithCheck(string deliveryNo, );

        #endregion

        #region Vincent add for others
        IList<string> GetDeliveryNoListByPalletNo(string dn);
        int CheckExistsProductIDOrModelOrFamily(string inputStr);

        void UpdateProductPreStation(IList<string> productIDList);
        void UpdateProductPreStationDefered(IUnitOfWork uow, IList<string> productIDList);

        void UpdateProductPreStation(IList<TbProductStatus> productStatusList);
        void UpdateProductPreStationDefered(IUnitOfWork uow, IList<TbProductStatus> productStatusList);

        void UpdateProductStatus(IList<string> productIDList, string line, string station, int status, int testFailCount, string editor);
        void UpdateProductStatusDefered(IUnitOfWork uow, IList<string> productIDList, string line, string station, int status, int testFailCount, string editor);

        void WriteProductLog(IList<string> productIDList, string line, string station, int status, string editor);
        void WriteProductLogDefered(IUnitOfWork uow, IList<string> productIDList, string line, string station, int status, string editor);

        void UpdateStationToPreStation(IList<string> productIDList, string editor);
        void UpdateStationToPreStationDefered(IUnitOfWork uow, IList<string> productIDList, string editor);

        void WriteProdLogByPreStation(IList<string> productIDList, string editor);
        void WriteProdLogByPreStationDefered(IUnitOfWork uow, IList<string> productIDList, string editor);

        void WriteHoldCode(IList<string> productIDList, TestLog log, IList<string> defectList);
        void WriteHoldCodeDefered(IUnitOfWork uow, IList<string> productIDList, TestLog log, IList<string> defectList);

        void ReleaseHoldProductID(IList<HoldInfo> holdInfo, string releaseReason, string editor);
        void ReleaseHoldProductIDDefered(IUnitOfWork uow, IList<HoldInfo> holdInfo, string releaseReason, string editor);

        IList<string> GetProductIDByStation(IList<string> stationList);
        IList<string> GetProductIDByModelStation(string model,IList<string> stationList);
        IList<string> GetModelByStation(IList<string> stationList);
        IList<HoldInfo> GetHoldInfo(IList<string> productIDList);        
        IList<HoldInfo> GetHoldInfo(string productIDorCustSN, string station);
        IList<HoldInfo> GetHoldInfo(string Model, IList<string> stationList);
        //IList<HoldInfo> GetHoldInfoByModel(string Model, IList<string> stationList);
        IList<HoldInfo> GetHoldInfoByProdID(IList<string> productIDList, IList<string> stationList);
        IList<HoldInfo> GetHoldInfoByCustSN(IList<string> custSNList, IList<string> stationList);

        IList<ProductStatusExInfo> GetProductPreStation(IList<string> productIDList);
        IList<TbProductStatus> GetProductStatus(IList<string> productIDList);

       

        bool CheckPassStation(string productID, string station);


        //for Repair defect_infom mark update
        void UpdateProductRepair_DefectInfo_Mark(IList<int> mark_0, IList<int> mark_1, string editor);
        void UpdateProductRepair_DefectInfo_MarkDefered(IUnitOfWork uow, IList<int> mark_0, IList<int> mark_1, string editor);

        bool ExistQCStatusByLineModelTp(string line, string model, string tp);
        bool ExistQCStatusByLineRegionTp(string line, string region, string tp);
        int GetSampleCountByModel(string tp, string line, string model);
        int GetSampleCountByFamily(string tp, string line,string family);


      
        int GetSampleCountByPdLine(string tp, string pdLine, DateTime startTime, DateTime endTime);
        int GetSampleCountByPdLineModel(string tp, string pdLine, string model, DateTime startTime, DateTime endTime);
        int GetSampleCountByPdLineFamily(string tp, string pdLine, string family, DateTime startTime, DateTime endTime);

        bool ExistQCStatusByPdLineModelTp(string pdLine, string model, string tp);
        bool ExistQCStatusByPdLineRegionTp(string pdLine, string region, string tp);
        //for spec check QCStatus Rule
        bool ExistQCStatusByModelTpStatus(string model, string tp, IList<string> status, int intervalDays);
        int GetSampleCountByModelDays(string tp, string model, int intervalDays);




        //for check CTO Bom
        bool ExistCTOBom(string model);

        /// <summary>
        /// EXISTS (SELECT * FROM FA.dbo.Product (nolock) WHERE CUSTSN=@CUSTSN)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool CheckExistCustomSn(string customSn);
        #endregion

        #region for  Product_Part by CT
        /// <summary>
        /// 从Product_Part表根据Value取多条记录
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IList<IProductPart> GetProductPartsByValue(string partSn);
        #endregion

        #region others function

        IList<ProductPart> GetProductPart(ProductPart condition);
        void UpdateProductPart(ProductPart item);
        void BackUpProductPartById(int id, string editor);
        void UpdateProductPartDefered(IUnitOfWork uow, ProductPart item);
        void BackUpProductPartByIdDefered(IUnitOfWork uow, int id, string editor);

        ProductLog GetLatestLogByWc(string productId, string station);

        ProductLog GetLatestLog(string productId);

        void DeleteProductPartByProductIDAndStation(IList<string> productIDList, IList<string> stationList,string editor);
        void DeleteProductPartByProductIDAndStationDefered(IUnitOfWork uow, IList<string> productIDList, IList<string> stationList,string editor);

        void UpdateForceNWCByProductID(IList<string> productIDList, string forceNWC, string editor);
        void UpdateForceNWCByProductIDDefered(IUnitOfWork uow, IList<string> productIDList, string forceNWC, string editor);

        IList<string> GetProductIDListNeedToCheck(string dn, IList<string> qcStatusList);

        IList<string> GetProductPartCheckItemTypeByStation(IList<string> productIdList, IList<string> stationList);
        IList<ProductMBInfo> GetPCBListByProductId(IList<string> productIdList);

        void UnPackMB(IList<string> productIdList);
        void UnPackMBDefered(IUnitOfWork uow, IList<string> productIdList);

        void UnPackCPU(IList<string> productIdList);
        void UnPackCPUDefered(IUnitOfWork uow, IList<string> productIdList);
        void BackUpProductPartByCheckItemType(string productId, string uEditor, string checkItemType);
        void BackUpProductPartByCheckItemTypeDefered(IUnitOfWork uow,string productId, string uEditor, string checkItemType);
        #endregion

        #region for Pilot Run MO Check
        bool ExistsProductInfoAndLogStation(string infoType, string infoValue, IList<string> stationList, int status);
        #endregion

        #region for ITCNDCheckSetting
        IList<ITCNDCheckSettingDef> GetITCNDCheckSettingByStationAndLine(string station, IList<string> lineList);
        #endregion

        #region CombinedAstNumber table
        ReleasedAstNumberInfo GetAvailableReleaseAstNumberWithReadPast(string code, string astType, string state);
        IList<CombinedAstNumberInfo> GetCombinedAstNumber(CombinedAstNumberInfo condition);
        void InsertCombinedAstNumber(CombinedAstNumberInfo info);
        void InsertCombinedAstNumberDefered(IUnitOfWork uow, CombinedAstNumberInfo info);
        void UpdateCombinedAstNumber(CombinedAstNumberInfo info);
        void UpdateCombinedAstNumberDefered(IUnitOfWork uow, CombinedAstNumberInfo info);
        #endregion

        IList<ProductModel> GetProductListByDeliveryNoList(IList<string> deliveryNoLst);
        void ExistsCustomSnThrowError(string productId, string customSn);
        void ExistsCustomSnThrowErrorDefered(IUnitOfWork uow, string productId, string customSn);

        #region UPS
        void RemoveCDSIAST(string productId);
        void RemoveCDSIASTDefered(IUnitOfWork uow, string productId);
        void RemoveSnoDetPoMo(string productId);
        void RemoveSnoDetPoMoDefered(IUnitOfWork uow, string productId);
        void RemoveSpecialDet(string productId);
        void RemoveSpecialDetDefered(IUnitOfWork uow, string productId);

        #endregion
    }
}
