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
    /// Product�ӿ�
    /// </summary>
    public interface IProduct : IAggregateRoot, ICheckObject, IRepairTarget, IPartOwner
    {
        /// <summary>
        /// Product���ڱ�ʶ,��ΪProduct���������
        /// </summary>
        string ProId { get;}

        /// <summary>
        /// same column
        /// </summary>
        string ProductID { get; }

        /// <summary>
        /// Product������������
        /// </summary>
        new string Model { get; set; }

        /// <summary>
        /// ��Product�󶨵İ忨ID
        /// </summary>
        string PCBID { get; set; }

        /// <summary>
        /// ��Product�󶨵�Mac��ַ
        /// </summary>
        string MAC { get; set; }

        /// <summary>
        /// ��Product�󶨵�UUID
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
        /// MBEcr�汾
        /// </summary>
        string MBECR { get; set; }

        ///// <summary>
        ///// �ͻ��汾
        ///// </summary>
        //string CUSTVER { get; set; }

        /// <summary>
        /// Product�Ŀͻ�Sn
        /// </summary>
        string CUSTSN { get; set; }

        ///// <summary>
        ///// ��Product�󶨵�BIOS
        ///// </summary>
        //string BIOS { get; set; }

        ///// <summary>
        ///// ��Product�󶨵�Image�汾
        ///// </summary>
        //string IMGVER { get; set; }

        ///// <summary>
        ///// ��Product�󶨵�IMEI
        ///// </summary>
        //string IMEI { get; set; }

        ///// <summary>
        ///// ��Product�󶨵�MEID
        ///// </summary>
        //string MEID { get; set; }

        ///// <summary>
        ///// ��Product�󶨵�ICCID
        ///// </summary>
        //string ICCID { get; set; }

        ///// <summary>
        ///// ��Product�󶨵�COA
        ///// </summary>
        //string COAID { get; set; }

        /// <summary>
        /// ��Product��ϵ�PizzaID
        /// </summary>
        string PizzaID { get; set; }

        /// <summary>
        /// ��Product��ϵ�Pizza Object
        /// </summary>
        Pizza PizzaObj { get; set; }

        /// <summary>
        /// ��Product��ϵ�2ndPizzaID
        /// </summary>
        string SecondPizzaID { get; }

        /// <summary>
        /// ��Product��ϵ�2nd Pizza Object
        /// </summary>
        Pizza SecondPizzaObj { get; }

        /// <summary>
        /// Product������MO
        /// </summary>
        string MO { get; set; }

        /// <summary>
        /// Product������
        /// </summary>
        decimal UnitWeight { get; set; }

        /// <summary>
        /// Product������Carton
        /// </summary>
        string CartonSN { get; set; }

        /// <summary>
        /// Product��Carton����
        /// </summary>
        decimal CartonWeight { get; set; }

        /// <summary>
        /// Product������Delivery
        /// </summary>
        string DeliveryNo { get; set; }

        /// <summary>
        /// Product������Pallet
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
        /// Product�������Ͷ���
        /// </summary>
        Model ModelObj { get; }

        /// <summary>
        /// Family object 
        /// </summary>
        Family FamilyObj { get; }

        /// <summary>
        /// Pruduct��վ״̬
        /// </summary>
        ProductStatus Status { get; }

        /// <summary>
        /// ��ȡ��Product�󶨵�����part
        /// </summary>
        IList<IProductPart> ProductParts { get; }

        /// <summary>
        /// ��ȡ����QCStatus
        /// </summary>
        IList<ProductQCStatus> QCStatus { get; }

        /// <summary>
        /// Product��վlog
        /// </summary>
        IList<ProductLog> ProductLogs { get; }

        /// <summary>
        /// Prodcut ����log
        /// </summary>
        IList<TestLog> TestLog { get; }

        /// <summary>
        /// Productά�޼�¼
        /// </summary>
        IList<Repair> Repairs { get; }

        /// <summary>
        /// ���Log
        /// </summary>
        IList<ProductChangeLog> ChangeLogs { get; }

        /// <summary>
        /// Product ��չ����
        /// </summary>
        IList<ProductInfo> ProductInfoes { get; }

        /// <summary>
        /// Product �Ƴ̿���״̬
        /// </summary>
        IList<ProductAttribute> ProductAttributes { get; }

        /// <summary>
        /// Product �Ƴ̿���״̬Log
        /// </summary>
        IList<ProductAttributeLog> ProductAttributeLogs { get; }

        /// <summary>
        /// Product��Ӧ��cusomer
        /// </summary>
        string Customer { get; }

        /// <summary>
        /// �Ƿ�������Rework/Dismantle
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
        /// ��¼Product��վ��¼
        /// </summary>
        /// <param name="log">Product��վ��¼</param>
        void AddLog(ProductLog log);

        /// <summary>
        /// Get Latest Fail Log
        /// </summary>
        /// <returns></returns>
        ProductLog GetLatestFailLog();

        /// <summary>
        /// ��¼Product�޸ļ�¼
        /// </summary>
        /// <param name="log">Product�޸ļ�¼</param>
        void AddChangeLog(ProductChangeLog log);

        /// <summary>
        /// �����Ƴ̿���״ֵ̬
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        /// <param name="editor">Editor</param>
        /// <param name="station">Station</param>
        /// <param name="descr">Description</param>
        /// <remarks>
        /// ��ָ�������Ѵ����򸲸�ԭ����ֵͬʱ��¼ProductAttributeLog��������������
        /// </remarks>
        void SetAttributeValue(string name, string value, string editor, string station, string descr);

        /// <summary>
        /// ��ȡָ������ֵ
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>��������ֵ,ָ���������������򷵻�null</returns>
        string GetAttributeValue(string name);

        /// <summary>
        /// ����Product��վ״̬
        /// </summary>
        /// <param name="status"></param>
        void UpdateStatus(ProductStatus status);

        ///<summary>
        /// ����QCStatus
        ///</summary>
        ///<param name="status">QCStatus</param>
        void AddQCStatus(ProductQCStatus status);

        /// <summary>
        /// ����QCStatus
        /// </summary>
        /// <param name="status">status</param>
        void UpdateQCStatus(ProductQCStatus status);

        /// <summary>
        /// Product ��ӦBOM���Ƿ����HDVD
        /// </summary>
        /// <returns></returns>
        bool ContainHDVD();

        /// <summary>
        /// ת��ΪIMES.DataModel.ProductInfo
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