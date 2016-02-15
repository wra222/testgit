// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yang WeiHua                 create
// Known issues:

using System.Collections.Generic;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure.FisObjectBase;
using IMES.DataModel;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.Common.Part;
using System;
using IMES.FisObject.Common.UPS;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PAK.DN;

namespace IMES.FisObject.FA.Product
{
    /// <summary>
    /// Product接口
    /// </summary>
    public interface IProduct : IAggregateRoot, ICheckObject, IRepairTarget, IPartOwner
    {
        /// <summary>
        /// Product厂内标识,作为Product对象的主键
        /// </summary>
        string ProId { get;}

        /// <summary>
        /// same column
        /// </summary>
        string ProductID { get; }

        /// <summary>
        /// Product所属机型名称
        /// </summary>
        new string Model { get; set; }

        /// <summary>
        /// 与Product绑定的板卡ID
        /// </summary>
        string PCBID { get; set; }

        /// <summary>
        /// 与Product绑定的Mac地址
        /// </summary>
        string MAC { get; set; }

        /// <summary>
        /// 与Product绑定的UUID
        /// </summary>
        string UUID { get; set; }

        /// <summary>
        /// Engineer Change Version
        /// </summary>
        string ECR { get; set; }

        /// <summary>
        ///Family Name 
        /// </summary>
        string Family { get; }

        /// <summary>
        /// MBEcr版本
        /// </summary>
        string MBECR { get; set; }

        ///// <summary>
        ///// 客户版本
        ///// </summary>
        //string CUSTVER { get; set; }

        /// <summary>
        /// Product的客户Sn
        /// </summary>
        string CUSTSN { get; set; }

        ///// <summary>
        ///// 与Product绑定的BIOS
        ///// </summary>
        //string BIOS { get; set; }

        ///// <summary>
        ///// 与Product绑定的Image版本
        ///// </summary>
        //string IMGVER { get; set; }

        ///// <summary>
        ///// 与Product绑定的IMEI
        ///// </summary>
        //string IMEI { get; set; }

        ///// <summary>
        ///// 与Product绑定的MEID
        ///// </summary>
        //string MEID { get; set; }

        ///// <summary>
        ///// 与Product绑定的ICCID
        ///// </summary>
        //string ICCID { get; set; }

        ///// <summary>
        ///// 与Product绑定的COA
        ///// </summary>
        //string COAID { get; set; }

        /// <summary>
        /// 与Product结合的PizzaID
        /// </summary>
        string PizzaID { get; set; }

        /// <summary>
        /// 与Product结合的Pizza Object
        /// </summary>
        Pizza PizzaObj { get; set; }

        /// <summary>
        /// 与Product结合的2ndPizzaID
        /// </summary>
        string SecondPizzaID { get; }

        /// <summary>
        /// 与Product结合的2nd Pizza Object
        /// </summary>
        Pizza SecondPizzaObj { get; }

        /// <summary>
        /// Product所属的MO
        /// </summary>
        string MO { get; set; }

        /// <summary>
        /// Product的重量
        /// </summary>
        decimal UnitWeight { get; set; }

        /// <summary>
        /// Product所属的Carton
        /// </summary>
        string CartonSN { get; set; }

        /// <summary>
        /// Product的Carton重量
        /// </summary>
        decimal CartonWeight { get; set; }

        /// <summary>
        /// Product所属的Delivery
        /// </summary>
        string DeliveryNo { get; set; }

        /// <summary>
        /// Product所属的Pallet
        /// </summary>
        string PalletNo { get; set; }

        /// <summary>
        /// model of PCB
        /// </summary>
        string PCBModel { get; set; }

        ///// <summary>
        ///// Wireless MAC
        ///// </summary>
        //string WMAC { get; set; }

        ///// <summary>
        ///// HDVD
        ///// </summary>
        //string HDVD { get; set; }

        ///// <summary>
        ///// BLMAC
        ///// </summary>
        //string BLMAC { get; set; }

        ///// <summary>
        ///// TVTuner
        ///// </summary>
        //string TVTuner { get; set; }

        /// <summary>
        /// CVSN
        /// </summary>
        string CVSN { get; set; }
        string PRSN { get; set; }
        string State { get; set; }
        string OOAID { get; set; }

        DateTime Cdt { get; set; }

        DateTime Udt { get; set; }

        /// <summary>
        /// Product所属机型对象
        /// </summary>
        Model ModelObj { get; }

        /// <summary>
        /// Family object 
        /// </summary>
        Family FamilyObj { get; }

        /// <summary>
        /// Pruduct过站状态
        /// </summary>
        ProductStatus Status { get; }

        /// <summary>
        /// 获取与Product绑定的所有part
        /// </summary>
        IList<IProductPart> ProductParts { get; }

        /// <summary>
        /// 获取所有QCStatus
        /// </summary>
        IList<ProductQCStatus> QCStatus { get; }

        /// <summary>
        /// Product过站log
        /// </summary>
        IList<ProductLog> ProductLogs { get; }

        /// <summary>
        /// Prodcut 测试log
        /// </summary>
        IList<TestLog> TestLog { get; }

        /// <summary>
        /// Product维修记录
        /// </summary>
        IList<Repair> Repairs { get; }

        /// <summary>
        /// 变更Log
        /// </summary>
        IList<ProductChangeLog> ChangeLogs { get; }

        /// <summary>
        /// Product 扩展属性
        /// </summary>
        IList<ProductInfo> ProductInfoes { get; }

        /// <summary>
        /// Product 制程控制状态
        /// </summary>
        IList<ProductAttribute> ProductAttributes { get; }

        /// <summary>
        /// Product 制程控制状态Log
        /// </summary>
        IList<ProductAttributeLog> ProductAttributeLogs { get; }

        /// <summary>
        /// Product对应的cusomer
        /// </summary>
        string Customer { get; }

        /// <summary>
        /// 是否允许做Rework/Dismantle
        /// </summary>
        bool IsAvailableForRework { get; }

        /// <summary>
        /// Whether a BT product?
        /// </summary>
        bool IsBT { get; }

        /// <summary>
        /// Whether a CDSI product?
        /// </summary>
        bool IsCDSI { get; }

        /// <summary>
        /// whether a RCTO product
        /// </summary>
        bool IsRCTO { get; }

        /// <summary>
        /// whether bind Po
        /// </summary>
        bool IsBindedPo { get; }        

        /// <summary>
        /// Bindes PoNo
        /// </summary>
        string BindPoNo { get; }

        /// <summary>
        /// Mo bind poNo device
        /// </summary>
        bool IsMOPoDevice { get; }

        /// <summary>
        /// MO object
        /// </summary>
        IMES.FisObject.Common.MO.MO MOObject { get; }

        /// <summary>
        /// whether bind UPSPo
        /// </summary>
        bool IsBindedUPS { get; }

        /// <summary>
        /// whether is UPS Device
        /// </summary>
        bool IsUPSDevice { get; }

        /// <summary>
        /// whether is UPS Ship PO
        /// </summary>
        bool IsUPSShipPO { get; }
        
        /// <summary>
        /// combine UPS PO
        /// </summary>
        UPSCombinePO UPSCombinePO { get; }
        
        /// <summary>
        /// CNRS Device
        /// </summary>
        bool IsCNRS { get; }

        /// <summary>
        /// 记录Product过站记录
        /// </summary>
        /// <param name="log">Product过站记录</param>
        void AddLog(ProductLog log);

        /// <summary>
        /// Get Latest Fail Log
        /// </summary>
        /// <returns></returns>
        ProductLog GetLatestFailLog();

        /// <summary>
        /// 记录Product修改记录
        /// </summary>
        /// <param name="log">Product修改记录</param>
        void AddChangeLog(ProductChangeLog log);

        /// <summary>
        /// 设置制程控制状态值
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        /// <param name="editor">Editor</param>
        /// <param name="station">Station</param>
        /// <param name="descr">Description</param>
        /// <remarks>
        /// 若指定属性已存在则覆盖原属性值同时记录ProductAttributeLog，否则新增属性
        /// </remarks>
        void SetAttributeValue(string name, string value, string editor, string station, string descr);

        /// <summary>
        /// 获取指定属性值
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>返回属性值,指定属性若不存在则返回null</returns>
        string GetAttributeValue(string name);

        /// <summary>
        /// 更新Product过站状态
        /// </summary>
        /// <param name="status"></param>
        void UpdateStatus(ProductStatus status);

        ///<summary>
        /// 添加QCStatus
        ///</summary>
        ///<param name="status">QCStatus</param>
        void AddQCStatus(ProductQCStatus status);

        /// <summary>
        /// 更新QCStatus
        /// </summary>
        /// <param name="status">status</param>
        void UpdateQCStatus(ProductQCStatus status);

        /// <summary>
        /// Product 对应BOM中是否包含HDVD
        /// </summary>
        /// <returns></returns>
        bool ContainHDVD();

        /// <summary>
        /// 转换为IMES.DataModel.ProductInfo
        /// </summary>
        /// <returns>IMES.DataModel.ProductInfo</returns>
        IMES.DataModel.ProductInfo ToProductInfo();

        ProductInfo GetExtendedPropertyBody(string key);
      //ProductInfo GetExtendedPropertyBody2(string key);
        string GetAttribute(string name);

        /// <summary>
        /// Combine MB object
        /// </summary>
        IMB MB
        {
            get;
        }
        /// <summary>
        /// Combined Delivery object
        /// </summary>
        Delivery Delivery
        {
            get;
        }
    }

}