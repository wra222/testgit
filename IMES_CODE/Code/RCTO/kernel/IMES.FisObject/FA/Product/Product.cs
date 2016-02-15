// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-10-23   Yuan XiaoWei                 create
// 2009-11-24   He Jiang                     Implementation
// Known issues:

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using IMES.DataModel;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.PAK.Pizza;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using System.Collections.ObjectModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.Repository._Metas;
using IMES.FisObject.Common.MO;
using fons=IMES.FisObject.Common.UPS;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.PAK.DN;

namespace IMES.FisObject.FA.Product
{
    /// <summary>
    /// Product�࣬Prodcut��FA��PAK����Ҫ��������
    /// </summary>

    [ORMapping(typeof(IMES.Infrastructure.Repository._Metas.Product))]
    public class Product : FisObjectBase, IProduct
    {
        //private static IProductRepository _prdctRepository;
        private readonly static IProductRepository PrdctRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        //{
        //    get
        //    {
        //        if (_prdctRepository == null)
        //            _prdctRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
        //        return _prdctRepository;
        //    }
        //}

        // private static IMORepository _moRepository;
        private readonly static IMORepository moRep = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
        //{
        //    get
        //    {
        //        if (_moRepository == null)
        //            _moRepository = RepositoryFactory.GetInstance().GetRepository<IMORepository>();
        //        return _moRepository;
        //    }
        //}

        // private static IMBRepository _mbRep;
        private readonly static IMBRepository mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        //{
        //    get
        //    {
        //        if (_mbRep == null)
        //            _mbRep = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        //        return _mbRep;
        //    }
        //}

        //private static IDeliveryRepository _dnRep;
        private readonly static IDeliveryRepository dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
        //{
        //    get
        //    {
        //        if (_dnRep == null)
        //            _dnRep = RepositoryFactory.GetInstance().GetRepository<IDeliveryRepository>();
        //        return _dnRep;
        //    }
        //}

        //private static fons.IUPSRepository _upsRep;
        private readonly static fons.IUPSRepository UPSRep=RepositoryFactory.GetInstance().GetRepository<fons.IUPSRepository>();
        //{
        //    get
        //    {
        //        if (_upsRep == null)
        //            _upsRep = RepositoryFactory.GetInstance().GetRepository<fons.IUPSRepository>();
        //        return _upsRep;
        //    }
        //}

        /// <summary>
        /// Constructor
        /// </summary>
        public Product()
        {
            _tracker.MarkAsAdded(this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="proId">Product Id</param>
        public Product(string proId)
        {
            _proId = proId;
            _tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_productID)]
        private string _proId = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_model)]
        private string _model = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_pcbid)]
        private string _pcbId = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_mac)]
        private string _mac = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_uuid)]
        private string _uuid = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_ecr)]
        private string _ecr = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_mbecr)]
        private string _mbecr = null;
        //private string _custVer;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_custsn)]
        private string _custSn = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_bios)]
        //private string _bios = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_imgver)]
        //private string _imgVer = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_imei)]
        //private string _imei = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_meid)]
        //private string _meid = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_iccid)]
        //private string _iccid = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_coaid)]
        //private string _coa = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_pizzaID)]
        private string _pizza = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_mo)]
        private string _mo = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_unitWeight)]
        private decimal _unitWeight = default(decimal);//decimal.MinValue;//
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_cartonSN)]
        private string _cartonSN = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_cartonWeight)]
        private decimal _cartonWeight = default(decimal);//decimal.MinValue;//
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_deliveryNo)]
        private string _deliveryNo = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_palletNo)]
        private string _palletNo = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_wmac)]
        //private string _wmac;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_pcbmodel)]
        private string _pcbmodel = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_cvsn)]
        private string _cvsn = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_prsn)]
        private string _prsn = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_state)]
        private string _state = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_ooaid)]
        private string _ooaid = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_hdvd)]
        //private string _hdvd = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_blmac)]
        //private string _blmac = null;
        //[ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_tvtuner)]
        //private string _tvtuner = null;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_cdt)]
        private DateTime _cdt = DateTime.MinValue;
        [ORMapping(IMES.Infrastructure.Repository._Metas.Product.fn_udt)]
        private DateTime _udt = DateTime.MinValue;

        ///// <summary>
        ///// HDVD
        ///// </summary>
        //public string HDVD
        //{
        //    get { return _hdvd; }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _hdvd = value;
        //    }
        //}

        ///// <summary>
        ///// BLMAC
        ///// </summary>
        //public string BLMAC
        //{
        //    get { return _blmac; }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _blmac = value;
        //    }
        //}

        ///// <summary>
        ///// TVTuner
        ///// </summary>
        //public string TVTuner
        //{
        //    get { return _tvtuner; }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _tvtuner = value;
        //    }
        //}

        /// <summary>
        /// CVSN
        /// </summary>
        public string CVSN
        {
            get { return _cvsn; }
            set {
                _tracker.MarkAsModified(this);
                _cvsn = value;
            }
        }

        public string PRSN
        {
            get { return _prsn; }
            set { _prsn = value; }
        }

        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        public string OOAID
        {
            get { return _ooaid; }
            set { _ooaid = value; }
        }

        /// <summary>
        /// Product���ڱ�ʶ,��ΪProduct���������
        /// </summary>
        public string ProId
        {
            get { return _proId; }
            private set
            {
                _tracker.MarkAsModified(this);
                _proId = value;
            }
        }

        /// <summary>
        /// same column
        /// </summary>
        public string ProductID
        {
            get { return _proId; }
        }


        public string OwnerId
        {
            get { return ProId; }
        }

        /// <summary>
        /// Family
        /// </summary>
        public string Family
        {
            get
            {
                if (ModelObj == null)
                {
                    return null;
                }
                return ModelObj.FamilyName;
            }
        }

        /// <summary>
        /// Product������������
        /// </summary>
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /// <summary>
        /// ��Product�󶨵İ忨ID
        /// </summary>
        public string PCBID
        {
            get
            {
                return _pcbId;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _pcbId = value;
            }
        }

        /// <summary>
        /// ��Product�󶨵�Mac��ַ
        /// </summary>
        public string MAC
        {
            get
            {
                return _mac;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _mac = value;
            }
        }

        /// <summary>
        /// ��Product�󶨵�UUID
        /// </summary>
        public string UUID
        {
            get
            {
                return _uuid;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _uuid = value;
            }
        }

        /// <summary>
        /// Engineer Change Version
        /// </summary>
        public string ECR
        {
            get
            {
                return _ecr;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _ecr = value;
            }
        }

        /// <summary>
        /// IEC�汾
        /// </summary>
        public string MBECR
        {
            get
            {
                return _mbecr;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _mbecr = value;
            }
        }

        ///// <summary>
        ///// �ͻ��汾
        ///// </summary>
        //public string CUSTVER
        //{
        //    get
        //    {
        //        return _custVer;
        //    }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _custVer = value;
        //    }
        //}

        /// <summary>
        /// Product�Ŀͻ�Sn
        /// </summary>
        public string CUSTSN
        {
            get
            {
                return _custSn;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _custSn = value;
            }
        }

        ///// <summary>
        ///// ��Product�󶨵�BIOS
        ///// </summary>
        //public string BIOS
        //{
        //    get
        //    {
        //        return _bios;
        //    }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _bios = value;
        //    }
        //}

        ///// <summary>
        ///// ��Product�󶨵�Image�汾
        ///// </summary>
        //public string IMGVER
        //{
        //    get
        //    {
        //        return _imgVer;
        //    }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _imgVer = value;
        //    }
        //}

        ///// <summary>
        ///// ��Product�󶨵�IMEI
        ///// </summary>
        //public string IMEI
        //{
        //    get
        //    {
        //        return _imei;
        //    }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _imei = value;
        //    }
        //}

        ///// <summary>
        ///// ��Product�󶨵�MEID
        ///// </summary>
        //public string MEID
        //{
        //    get
        //    {
        //        return _meid;
        //    }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _meid = value;
        //    }
        //}

        ///// <summary>
        ///// ��Product�󶨵�ICCID
        ///// </summary>
        //public string ICCID
        //{
        //    get
        //    {
        //        return _iccid;
        //    }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _iccid = value;
        //    }
        //}

        ///// <summary>
        ///// ��Product�󶨵�COA
        ///// </summary>
        //public string COAID
        //{
        //    get
        //    {
        //        return _coa;
        //    }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _coa = value;
        //    }
        //}

        /// <summary>
        /// ��Product��ϵ�Pizza
        /// </summary>
        public string PizzaID
        {
            get
            {
                return _pizza;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _pizza = value;
            }
        }

        public string SecondPizzaID
        {
            get { return (string)GetExtendedProperty("KIT2"); }
        }

        /// <summary>
        /// Product������MO
        /// </summary>
        public string MO
        {
            get
            {
                return _mo;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _mo = value;
            }
        }

        /// <summary>
        /// Product������
        /// </summary>
        public decimal UnitWeight
        {
            get
            {
                return _unitWeight;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _unitWeight = value;
            }
        }

        /// <summary>
        /// Product������Carton
        /// </summary>
        public string CartonSN
        {
            get
            {
                return _cartonSN;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _cartonSN = value;
            }
        }

        /// <summary>
        /// Product��Carton����
        /// </summary>
        public decimal CartonWeight
        {
            get
            {
                return _cartonWeight;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _cartonWeight = value;
            }
        }

        /// <summary>
        /// Product������Delivery
        /// </summary>
        public string DeliveryNo
        {
            get
            {
                return _deliveryNo;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _deliveryNo = value;
            }
        }

        /// <summary>
        /// Product������Pallet
        /// </summary>
        public string PalletNo
        {
            get
            {
                return _palletNo;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _palletNo = value;
            }
        }

        /// <summary>
        /// model of PCB
        /// </summary>
        public string PCBModel
        {
            get
            {
                return _pcbmodel;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _pcbmodel = value;
            }
        }

        ///// <summary>
        ///// Wireless MAC
        ///// </summary>
        //public string WMAC
        //{
        //    get
        //    {
        //        return _wmac;
        //    }
        //    set
        //    {
        //        _tracker.MarkAsModified(this);
        //        _wmac = value;
        //    }
        //}

        public DateTime Cdt
        {
            get
            {
                return _cdt;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _cdt = value;
            }
        }

        public DateTime Udt
        {
            get
            {
                return _udt;
            }
            set
            {
                _tracker.MarkAsModified(this);
                _udt = value;
            }
        }

        #endregion

        #region . Sub-Instances .

        private bool _isBT = false;
        private IMES.FisObject.Common.Model.Model _modelObj;
        private object _syncObj_modelObj = new object();
        private Pizza _pizzaObj;
        private object _syncObj_pizzaObj = new object();
        private ProductStatus _status;
        private object _syncObj_status = new object();
        private IList<IProductPart> _parts;
        private object _syncObj_parts = new object();
        private IList<ProductQCStatus> _qcStatus;
        private object _syncObj_qcStatus = new object();
        private IList<ProductLog> _productLogs;
        private object _syncObj_productLogs = new object();
        private IList<TestLog> _testLogs;
        private object _syncObj_testLogs = new object();
        private IList<Repair> _repairs;
        private object _syncObj_repairs = new object();
        private IList<ProductChangeLog> _chngLogs;
        private object _syncObj_chngLogs = new object();
        private IList<ProductInfo> _infoes;
        private object _syncObj_infoes = new object();
        private IList<ProductAttribute> _attributes;
        private object _syncObj_attributes = new object();
        private IList<ProductAttributeLog> _attributeLogs;
        private object _syncObj_attributeLogs = new object();

        private IMES.FisObject.Common.MO.MO _MOObject;
        private object _syncObj_MOObj = new object();

        private fons.UPSCombinePO _upsCombinePO;
        private object _syncObj_UPSObj = new object();

        private Nullable<bool> _isUPSModel = null;

        /// <summary>
        /// Product�������Ͷ���
        /// </summary>
        public Pizza PizzaObj
        {
            get
            {
                lock (_syncObj_pizzaObj)
                {
                    if (_pizzaObj == null)
                    {
                        PrdctRepository.FillPizza(this);
                    }
                    return _pizzaObj;
                }
            }
            set
            {
                lock (_syncObj_pizzaObj)
                {
                    _pizzaObj = value;
                }
            }
        }

        public Pizza SecondPizzaObj
        {
            get 
            { 
                Pizza pizza = null;
                if (!string.IsNullOrEmpty(SecondPizzaID))
                {
                    pizza = RepositoryFactory.GetInstance().GetRepository<IPizzaRepository, Pizza>().Find(SecondPizzaID);
                }
                return pizza;
            }
        }


        /// <summary>
        /// Product�������Ͷ���
        /// </summary>
        public IMES.FisObject.Common.Model.Model ModelObj
        {
            get
            {
                lock (_syncObj_modelObj)
                {
                    if (_modelObj == null)
                    {
                        PrdctRepository.FillModelObj(this);
                    }
                    return _modelObj;
                }
            }
        }

        public IMES.FisObject.Common.Model.Family FamilyObj
        {
            get
            {
                IMES.FisObject.Common.Model.Model model = this.ModelObj;
                if (model == null)
                {
                    return null;
                }
                return model.Family;
            }
        }

        /// <summary>
        /// Pruduct��վ״̬
        /// </summary>
        public ProductStatus Status
        {
            get
            {
                lock (_syncObj_status)
                {
                    if (_status == null)
                    {
                        PrdctRepository.FillStatus(this);
                    }
                    return _status;
                }
            }
            set
            {
                lock (_syncObj_status)
                {
                    _tracker.MarkAsModified(this);
                    _status = value;

                    LoggingInfoFormat("Status Setter: {0}", _tracker.ToString());
                }
            }
        }

        /// <summary>
        /// ��ȡ��Product�󶨵�����part
        /// </summary>
        public IList<IProductPart> ProductParts
        {
            get
            {
                lock (_syncObj_parts)
                {
                    if (_parts == null)
                    {
                        PrdctRepository.FillProductParts(this);
                    }
                    if (_parts != null)
                    {
                        return new ReadOnlyCollection<IProductPart>(_parts);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// ��ȡ����QCStatus
        /// </summary>
        public IList<ProductQCStatus> QCStatus
        {
            get
            {
                lock (_syncObj_qcStatus)
                {
                    if (_qcStatus == null)
                    {
                        PrdctRepository.FillQCStatuses(this);
                    }
                    if (_qcStatus != null)
                    {
                        return new ReadOnlyCollection<ProductQCStatus>(_qcStatus);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// ��ȡ��Product�󶨵�����log
        /// </summary>
        public IList<ProductLog> ProductLogs
        {
            get
            {
                lock (_syncObj_productLogs)
                {
                    if (_productLogs == null)
                    {
                        PrdctRepository.FillLogs(this);
                    }
                    else
                    {
                        IList<ProductLog> szAllAdd = new List<ProductLog>();
                        foreach (ProductLog pl in _productLogs)
                        {
                            if (pl.Id > 0)
                            {
                                szAllAdd.Clear();
                                break;
                            }
                            else
                            {
                                szAllAdd.Add(pl);
                            }
                        }
                        if (szAllAdd.Count > 0)
                        {
                            PrdctRepository.FillLogs(this);
                            foreach (ProductLog pl in szAllAdd)
                            {
                                _productLogs.Add(pl);
                            }
                        }
                    }
                    if (_productLogs != null)
                    {
                        return new ReadOnlyCollection<ProductLog>(_productLogs);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// ȡ�ò�����־���ϡ�ͨ�����ڲ��һ����ض���־��Ϣ��
        /// </summary>
        /// <returns>���ز�����־�����б���</returns>
        public IList<TestLog> TestLog
        {
            get
            {
                lock (_syncObj_testLogs)
                {
                    if (_testLogs == null)
                    {
                        PrdctRepository.FillTestLogs(this);
                    }
                    else
                    {
                        IList<TestLog> szAllAdd = new List<TestLog>();
                        foreach (TestLog tl in _testLogs)
                        {
                            if (tl.ID > 0)
                            {
                                szAllAdd.Clear();
                                break;
                            }
                            else
                            {
                                szAllAdd.Add(tl);
                            }
                        }
                        if (szAllAdd.Count > 0)
                        {
                            PrdctRepository.FillTestLogs(this);
                            foreach (TestLog tl in szAllAdd)
                            {
                                _testLogs.Add(tl);
                            }
                        }
                    }
                    if (_testLogs != null)
                    {
                        return new ReadOnlyCollection<TestLog>(_testLogs);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// ȡ����������ά�޼�¼��
        /// </summary>
        /// <returns>ά�޶��󼯺�</returns>
        public IList<Repair> Repairs
        {
            get
            {
                lock (_syncObj_repairs)
                {
                    if (_repairs == null)
                    {
                        PrdctRepository.FillRepairs(this);
                    }
                    if (_repairs != null)
                    {
                        return new ReadOnlyCollection<Repair>(_repairs);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// ���Log
        /// </summary>
        public IList<ProductChangeLog> ChangeLogs
        {
            get
            {
                lock (_syncObj_chngLogs)
                {
                    if (_chngLogs == null)
                    {
                        PrdctRepository.FillChangeLogs(this);
                    }
                    else
                    {
                        IList<ProductChangeLog> szAllAdd = new List<ProductChangeLog>();
                        foreach (ProductChangeLog pcl in _chngLogs)
                        {
                            if (pcl.ID > 0)
                            {
                                szAllAdd.Clear();
                                break;
                            }
                            else
                            {
                                szAllAdd.Add(pcl);
                            }
                        }
                        if (szAllAdd.Count > 0)
                        {
                            PrdctRepository.FillChangeLogs(this);
                            foreach (ProductChangeLog pcl in szAllAdd)
                            {
                                _chngLogs.Add(pcl);
                            }
                        }
                    }
                    if (_chngLogs != null)
                    {
                        return new ReadOnlyCollection<ProductChangeLog>(_chngLogs);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Product ��չ����
        /// </summary>
        public IList<ProductInfo> ProductInfoes
        {
            get
            {
                lock (_syncObj_infoes)
                {
                    if (_infoes == null)
                    {
                        PrdctRepository.FillProductInfoes(this);
                    }
                    if (_infoes != null)
                    {
                        return new ReadOnlyCollection<ProductInfo>(_infoes);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Product �Ƴ̿���״̬
        /// </summary>
        public IList<ProductAttribute> ProductAttributes
        {
            get
            {
                lock (_syncObj_attributes)
                {
                    if (_attributes == null)
                    {
                        PrdctRepository.FillProductAttributes(this); 
                    }
                    if (_attributes != null)
                    {
                        return new ReadOnlyCollection<ProductAttribute>(_attributes);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Product �Ƴ̿���״̬Log
        /// </summary>
        public IList<ProductAttributeLog> ProductAttributeLogs
        {
            get
            {
                lock (_syncObj_attributeLogs)
                {
                    if (_attributeLogs == null)
                    {
                        PrdctRepository.FillProductAttributeLogs(this);
                    }
                    if (_attributeLogs != null)
                    {
                        return new ReadOnlyCollection<ProductAttributeLog>(_attributeLogs);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Product��Ӧ��cusomer
        /// </summary>
        public string Customer
        {
            get
            {
                if (ModelObj == null)
                {
                    return null;
                }
                if (ModelObj.Family == null)
                {
                    return null;
                }
                return ModelObj.Family.Customer;
            }
        }

        /// <summary>
        /// �Ƿ�������Rework/Dismantle
        /// </summary>
        public bool IsAvailableForRework 
        { 
            get
            {
                return !PrdctRepository.ReworkReject(Customer, Status.StationId, Status.Status);
            } 
        }

        /// <summary>
        /// Whether a BT product?
        /// </summary>
        public bool IsBT
        {
            get 
            {
                IList<ProductBTInfo> bt = PrdctRepository.GetProductBT(_proId);
                if (bt != null && bt.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsCDSI
        {
            get
            {
                //SELECT     @ATSNAV=[Value]
                //  FROM [IMES2012_GetData].[dbo].[ModelInfo] where [Model]= @model and Name=��ATSNAV��
                //IF @ATSNAV = ���� then not Shell(CDSI)��
                //else   IS shell (CDSI)
                string CNRSModel = (string)GetModelProperty("CNRS");
                string CDSIModel = (string)GetModelProperty("ATSNAV");
                if (!string.IsNullOrEmpty(CDSIModel))
                {
                    return true;
                }

                 if (!string.IsNullOrEmpty(CNRSModel) && CNRSModel=="Y")
                {
                    return true;
                }                
               
                return false;
            }
        }
        /// <summary>
        /// CNRS Device
        /// </summary>
        public bool IsCNRS
        {
            get 
            { 
                string CNRSModel = (string)GetModelProperty("CNRS");
                if (!string.IsNullOrEmpty(CNRSModel) && CNRSModel == "Y")
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsRCTO
        {
            get
            {
                if (string.Compare(this.Model, 0, "173", 0, 3) == 0
                 || string.Compare(this.Model, 0, "146", 0, 3) == 0)
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// is bined PoNo
        /// </summary>
        public bool IsBindedPo
        {
            get
            {
                #region disable code
                //lock (_syncObj_MOObj)
                //{
                //    if (_MOObject == null)
                //    {
                //        if (!string.IsNullOrEmpty(this.MO))
                //        {
                //            _MOObject = moRep.Find(this.MO);
                //        }
                //    }                   
                //}
                //return _MOObject!=null && !string.IsNullOrEmpty(_MOObject.PoNo);
                #endregion

                IMES.FisObject.Common.MO.MO mo = this.MOObject;
                if (mo != null && !string.IsNullOrEmpty(mo.PoNo))
                {
                    return true;
                }
                else
                {
                    fons.UPSCombinePO combinePo = this.UPSCombinePO;
                    if (combinePo != null &&
                        combinePo.IsShipPO == "Y" &&
                        !string.IsNullOrEmpty(combinePo.IECPO))
                    {
                        return true;
                    }
                }
                return false;                
            }
        }

        /// <summary>
        /// Bindes PoNo
        /// </summary>
        public string BindPoNo {
            get
            {
                #region disable code
                //lock (_syncObj_MOObj)
                //{
                //    if (_MOObject == null)
                //    {
                //        if (!string.IsNullOrEmpty(this.MO))
                //        {
                //            _MOObject = moRep.Find(this.MO);
                //        }
                //    }
                //}
                //return _MOObject==null? null:_MOObject.PoNo;
                #endregion
                IMES.FisObject.Common.MO.MO mo = this.MOObject;
                if (mo != null && !string.IsNullOrEmpty(mo.PoNo))
                {
                    return mo.PoNo;
                }
                else
                {
                    fons.UPSCombinePO combinePo = this.UPSCombinePO;
                    if (combinePo != null &&
                        combinePo.IsShipPO == "Y" &&
                        !string.IsNullOrEmpty(combinePo.IECPO))
                    {
                        return combinePo.IECPO;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Mo bind poNo device
        /// </summary>
        public bool IsMOPoDevice 
        {
            get
            {
                 IMES.FisObject.Common.MO.MO mo = this.MOObject;
                 if (mo != null && !string.IsNullOrEmpty(mo.PoNo))
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
            } 
        }


        /// <summary>
        /// MO object
        /// </summary>
        public IMES.FisObject.Common.MO.MO MOObject
        {
            get
            {
                lock (_syncObj_MOObj)
                {
                    if (_MOObject == null)
                    {
                        if (!string.IsNullOrEmpty(this.MO))
                        {
                            _MOObject=moRep.Find(this.MO);
                        }
                    }
                    return _MOObject;
                }
            }
        }
        /// <summary>
        /// ���Product��ָ��Repair�����RepairDefect
        /// </summary>
        /// <param name="rep">������Repair</param>
        public static void FillingRepairDefects(Repair rep)
        {
            PrdctRepository.FillRepairDefectInfo(rep);
        }

        /// <summary>
        /// ���ָ��TestLog��TestLogDefect
        /// </summary>
        /// <param name="tstlg">ָ��TestLog</param>
        public static void FillingTestLogDefects(TestLog tstlg)
        {
            PrdctRepository.FillTestLogDefectInfo(tstlg);
        }

        #endregion

        #region . About Repair (IRepairTarget) .

        /// <summary>
        /// ȡ����������ά�޼�¼��
        /// </summary>
        /// <returns>ά�޶��󼯺�</returns>
        public IList<Repair> GetRepair()
        {
            return Repairs;
        }

        /// <summary>
        /// ������������һ������ά�޼�¼��
        /// </summary>
        /// <param name="rep">ά�޼�¼����</param>
        /// <returns>Defect��</returns>
        public void AddRepair(Repair rep)
        {
            if (rep == null)
                return;

            lock (_syncObj_repairs)
            {
                object naught = Repairs;
                if (_repairs.Contains(rep))
                    return;

                rep.Tracker = _tracker.Merge(rep.Tracker);
                _repairs.Add(rep);
                rep.FillingRepairDefects += new FillRepair(FillingRepairDefects);
                //GetRepair().Add(rep);
                _tracker.MarkAsAdded(rep);
                _tracker.MarkAsModified(this);
                //int defectNo = 0;
                // TODO: How to return the defect No?
                //return defectNo;
            }
        }

        /// <summary>
        /// Ϊָ��Repair����һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="defect"></param>
        public void AddRepairDefect(int repairId, RepairDefect defect)
        {
            lock (_syncObj_repairs)
            {
                object naught = Repairs;
                if (_repairs != null && defect != null)
                {
                    foreach (Repair rp in _repairs)
                    {
                        if (rp.Key.Equals(repairId))
                        {
                            rp.AddRepairDefect(defect);
                            rp.Tracker.MarkAsModified(rp);
                            _tracker.MarkAsModified(this);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ϊָ��Repairɾ��һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="repairDefectId">ָ��RepairDefect��Id</param>
        public void RemoveRepairDefect(int repairId, int repairDefectId)
        {
            lock (_syncObj_repairs)
            {
                object naught = Repairs;
                if (_repairs != null)
                {
                    foreach (Repair rp in _repairs)
                    {
                        if (rp.Key.Equals(repairId))
                        {
                            rp.RemoveRepairDefect(repairDefectId);
                            rp.Tracker.MarkAsModified(rp);
                            _tracker.MarkAsModified(this);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ϊָ��Repair����һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="defect">ָ��RepairDefect</param>
        public void UpdateRepairDefect(int repairId, RepairDefect defect)
        {
            lock (_syncObj_repairs)
            {
                object naught = Repairs;
                if (_repairs != null && defect != null)
                {
                    foreach (Repair rp in _repairs)
                    {
                        if (rp.Key.Equals(repairId))
                        {
                            rp.UpdateRepairDefect(defect);
                            rp.Tracker.MarkAsModified(rp);
                            _tracker.MarkAsModified(this);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ȡ�����嵱ǰά�޼�¼��
        /// </summary>
        /// <returns>��ǰά�޶���</returns>
        public Repair GetCurrentRepair()
        {
            foreach (var repair in Repairs)
            {
                if (repair.Status == Repair.RepairStatus.NotFinished)
                {
                    return repair;
                }
            }
            return null;
        }

        /// <summary>
        /// ��ȡָ��Site+Component��ά����ʷ
        /// </summary>
        /// <param name="site">site</param>
        /// <param name="component">component</param>
        /// <returns>ά����ʷ</returns>
        public IList<RepairDefect> GetRepairDefectBySiteComponent(string site, string component)
        {
            throw new NotImplementedException("Product does not need to implement this method");
            //return PrdctRepository.GetRepairDefectBySiteComponent(ProId, site, component);
        }

        /// <summary>
        /// complteһ��ά��
        /// </summary>
        public void CompleteRepair(string line, string station, string editor)
        {
            var currentRepair = GetCurrentRepair();
            foreach (var repair in Repairs)
            {
                if (repair.ID == currentRepair.ID)
                {
                    repair.SetStatus(Repair.RepairStatus.Finished);
                    //repair.SetLine(line);
                    //repair.SetStation(station);
                    repair.SetLine(line);
                    repair.SetStation(station);
                    repair.SetEditor(editor);
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //public void GenerateRepairByTestLog()
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// ��Part
        /// </summary>
        /// <param name="partID">Part Id</param>
        /// <param name="newPart">new part</param>
        public void ChangePart(int partID, IProductPart newPart)
        {
            lock (_syncObj_parts)
            {
                object naught = ProductParts;
                foreach (var part in _parts)
                {
                    if (part.ID == partID)
                    {
                        part.PartID = newPart.PartID;
                        part.PartSn = newPart.PartSn;
                        part.Value = newPart.Value;
                        part.Editor = newPart.Editor;
                    }
                }
            }
        }

        public void RemoveRepair(int repairId)
        {
            lock (_syncObj_repairs)
            {
                object naught = this.Repairs;
                if (this._repairs != null)
                {
                    foreach (Repair rep in this._repairs)
                    {
                        if (repairId.CompareTo(rep.ID) == 0)
                        {
                            ((Repair)rep).Tracker = null;
                            this._tracker.MarkAsDeleted(rep);
                        }
                    }
                }
                this._tracker.MarkAsModified(this);
            }
        }

        public void RemoveAllRepair()
        {
            lock (_syncObj_repairs)
            {
                object naught = this.Repairs;
                if (this._repairs != null)
                {
                    foreach (Repair rep in this._repairs)
                    {
                        ((Repair)rep).Tracker = null;
                        this._tracker.MarkAsDeleted(rep);
                    }
                }
                this._tracker.MarkAsModified(this);
            }
        }

/*
        /// <summary>
        /// complteһ��ά��
        /// </summary>
        /// <param name="repairID"></param>
*/
        //public void CompleteRepair(int repairID, string line)
        //{
        //    var ra = (from r in Repairs where r.ID = repairID select r).ToArray();

        //    if (ra.Length > 0)
        //    {
        //        ra[0].SetStatus(Repair.RepairStatus.Finished);
        //        ra[0].SetLine(line);
        //    }
        //}

        /// <summary>
        /// RepairTarget����
        /// </summary>
        public string RepairTargetModel
        {
            get { return Model; }
        }

        /// <summary>
        /// �Ƿ����ά��
        /// </summary>
        public bool IsFirstRepair
        {
            get
            {
                foreach (var repair in Repairs)
                {
                    if (repair.Status == Repair.RepairStatus.NotFinished)
                    {
                        return false;
                    }
                }
                return true;

                //                if (TestLogs != null && TestLogs[0].Status == TestLog.TestLogStatus.Fail)
                //                {
                //                    return true;
                //                }
                //
                //                return false;
            }
        }

        public TestLog LatestFailTestLog
        {
            get
            {
                for (int i = 0; i < TestLog.Count; i++)
                {
                    if (TestLog[i].Status == Common.TestLog.TestLog.TestLogStatus.Fail)
                    {
                        return TestLog[i];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// ���µĲ���log
        /// </summary>
        public TestLog LatestTestLog
        {
            get
            {
                for (int i = 0; i < TestLog.Count; i++)
                {
                    if (TestLog[i].Station.Equals(Status.StationId))
                    {
                        return TestLog[i];
                    }
                }
                return null;
            }
        }

        #endregion

        #region . About Log .

        /// <summary>
        /// ��¼Product��վ��¼
        /// </summary>
        /// <param name="log">Product��վ��¼</param>
        public void AddLog(ProductLog log)
        {
            if (log == null)
                return;

            lock (_syncObj_productLogs)
            {
                if (_productLogs == null)
                {
                    _productLogs = new List<ProductLog>();
                }

                //object naught = ProductLogs;
                if (_productLogs.Contains(log))
                    return;

                log.Tracker = _tracker.Merge(log.Tracker);
                _productLogs.Add(log);
                //ProductLogs.Add(log);
                _tracker.MarkAsAdded(log);
                _tracker.MarkAsModified(this);

                LoggingInfoFormat("Tracker Content: {0}", _tracker.ToString());
            }
        }

        /// <summary>
        /// Get Latest Fail Log
        /// </summary>
        /// <returns></returns>
        public ProductLog GetLatestFailLog() 
        {
            return PrdctRepository.GetLatestFailLog(this.ProId);                                    
        }

        /// <summary>
        /// ��¼Product�޸ļ�¼
        /// </summary>
        /// <param name="log">Product�޸ļ�¼</param>
        public void AddChangeLog(ProductChangeLog log)
        {
            if (log == null)
                return;

            lock (_syncObj_chngLogs)
            {
                if (_chngLogs == null)
                {
                    _chngLogs = new List<ProductChangeLog>();
                }

                //object naught = ChangeLogs;
                if (_chngLogs.Contains(log))
                    return;

                log.Tracker = _tracker.Merge(log.Tracker);
                _chngLogs.Add(log);
                //ProductLogs.Add(log);
                _tracker.MarkAsAdded(log);
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// �����Ƴ̿���״ֵ̬
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        /// <param name="editor">Editor</param>
        /// <param name="station">Station</param>
        /// <param name="descr">Description</param>
        public void SetAttributeValue(string name, string value, string editor, string station, string descr)
        {
            if (string.IsNullOrEmpty(name))
                return;

            lock (_syncObj_attributes)
            {
                object naught = ProductAttributes;

                foreach (var attrib in _attributes)
                {
                    if (attrib.AttributeName == name)
                    {
                        //update attribute
                        string oldValue = attrib.AttributeValue;
                        attrib.AttributeValue = value;
                        attrib.Editor = editor;

                        var pdal = new ProductAttributeLog
                                       {
                                           Editor = editor,
                                           AttributeName = name,
                                           OldValue = oldValue,
                                           NewValue = value,
                                           ProductId = _proId,
                                           Model = _model,
                                           Station = station,
                                           Descr = descr
                                       };
                        pdal.Tracker = _tracker.Merge(pdal.Tracker);
                        if (_attributeLogs == null)
                        {
                            _attributeLogs = new List<ProductAttributeLog>();
                        }
                        _attributeLogs.Add(pdal);
                        _tracker.MarkAsModified(this);
                        return;
                    }
                }

                //add new record
                var pda = new ProductAttribute
                              {
                                  Editor = editor,
                                  AttributeName = name,
                                  AttributeValue = value,
                                  ProductId = _proId
                              };

                pda.Tracker = _tracker.Merge(pda.Tracker);
                _attributes.Add(pda);
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// ��ȡָ������ֵ
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>����ֵ</returns>
        public string GetAttributeValue(string name)
        {
            var values = (from p in ProductAttributes
                          where p.AttributeName == name
                          select p.AttributeValue).ToArray();
            if (values.Length > 0)
            {
                return values.First();
            }
            return null;
        }

        /// <summary>
        /// ���Ӳ��Լ�¼
        /// </summary>
        /// <param name="testLog">���Լ�¼</param>
        public void AddTestLog(TestLog testLog)
        {
            if (testLog == null)
                return;

            lock (_syncObj_testLogs)
            {
                if (_testLogs == null)
                {
                    _testLogs = new List<TestLog>();
                }
                //object naught = TestLog;
                if (_testLogs.Contains(testLog))
                    return;

                testLog.Tracker = _tracker.Merge(testLog.Tracker);
                _testLogs.Add(testLog);
                testLog.FillingTestLogDefects += new FillTestLog(FillingTestLogDefects);
                _tracker.MarkAsAdded(testLog);
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// ȡ�ò�����־���ϡ�ͨ�����ڲ��һ����ض���־��Ϣ��
        /// </summary>
        /// <returns>���ز�����־�����б���</returns>
        public IList<TestLog> GetTestLog()
        {
            return TestLog;
        }

        # endregion

        #region . Implementation of IProduct .

        /// <summary>
        /// ����Product��վ״̬
        /// </summary>
        /// <param name="status"></param>
        public void UpdateStatus(ProductStatus status)
        {
            if (status == null)
                return;

            lock (_syncObj_status)
            {
                Status = status;
                Tracker.MarkAsModified(Status);
            }
        }

        #endregion

        #region . PAK  .

        ///<summary>
        /// ����QCStatus
        ///</summary>
        ///<param name="status">QCStatus</param>
        public void AddQCStatus(ProductQCStatus status)
        {
            if (status == null)
                return;

            lock (_syncObj_qcStatus)
            {
                object naught = QCStatus;
                if (_qcStatus.Contains(status))
                    return;

                status.Tracker = _tracker.Merge(status.Tracker);
                _qcStatus.Add(status);
                //QCStatus.Add(status);
                _tracker.MarkAsAdded(status);
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// ����QCStatus
        /// </summary>
        /// <param name="status">status</param>
        public void UpdateQCStatus(ProductQCStatus status)
        {
            if (status == null)
                return;

            lock (_syncObj_qcStatus)
            {
                object naught = QCStatus;
                // classic finding process
                for (int i = 0; i < _qcStatus.Count; ++i)
                {
                    if (_qcStatus[i].ID == status.ID)
                    {
                        _qcStatus[i] = status;
                        _tracker.MarkAsModified(status);
                        _tracker.MarkAsModified(this);

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Product ��ӦBOM���Ƿ����HDVD
        /// </summary>
        /// <returns></returns>
        public bool ContainHDVD()
        {
            throw new NotImplementedException();
            //var repository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
            //IList<string> hdvdPns = repository.GetPnFromMoBOMByTypeAndDescrCondition(MO, "DVD", "HDVD");
            //if (hdvdPns != null && hdvdPns.Count > 0 )
            //{
            //    return true;
            //}
            //return false;
        }

        /// <summary>
        /// ת��ΪIMES.DataModel.ProductInfo
        /// </summary>
        /// <returns>IMES.DataModel.ProductInfo</returns>
        public DataModel.ProductInfo ToProductInfo()
        {
            var info = new DataModel.ProductInfo();
            info.id = ProId;
            info.modelId = Model;
            info.pizzaId = PizzaID;
            info.familyId = Family;
            info.modelId = Model;
            info.MOId = MO;
            info.cvSN = CVSN;
            info.customSN = CUSTSN;
            return info;
        }

        #endregion

        #region . Implementation of ICheckObject .

        /// <summary>
        /// ��ȡModelָ������
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public object GetModelProperty(string item)
        {
            if (ModelObj == null)
            {
                return null;
            }
            PropertyInfo prop = ModelObj.GetType().GetProperty(item);
            if (prop != null)
            {
                return prop.GetValue(ModelObj, null);
            }
            return ModelObj.GetAttribute(item);
        }

        /// <summary>
        /// ����Modelָ������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetModelProperty(string name, object value)
        {
            throw new NotImplementedException("A property of Model can not be updated.");
        }

        /// <summary>
        /// ��ȡ���Ŀ�����Ļ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>����ֵ</returns>
        public object GetProperty(string name)
        {
            PropertyInfo prop = GetType().GetProperty(name);
            if (prop != null)
            {
                return prop.GetValue(this, null);
            }
            return null;
        }

        /// <summary>
        /// ��ȡ���Ŀ��������չ����
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>����ֵ</returns>
        public string GetExtendedProperty(string name)
        {
            if (ProductInfoes == null)
                return null;
            return _infoes.Where(x => x.InfoType == name)
                                                .Select(x => x.InfoValue).FirstOrDefault(); 
            //var values = (from p in ProductInfoes
            //                                where p.InfoType == name
            //                                select p.InfoValue).ToArray();
            //if (values.Length > 0)
            //{
            //    return values.First();
            //}
            //return null;
        }

        public ProductInfo GetExtendedPropertyBody(string key)
        {
            var values = (from p in ProductInfoes
                          where p.InfoType == key
                          select p).ToArray();

            if (values.Length > 0)
            {
                return values.First();
            }
            return null;
        }

        //public ProductInfo GetExtendedPropertyBody2(string key)
        //{
        //    foreach(var p in ProductInfoes)
        //    {
        //        if (p.InfoType == key)
        //            return p;
        //    }
        //    return null;
        //}

        /// <summary>
        /// Fuzzy query from ProductInfo with 'LIKE'
        /// </summary>
        /// <param name="condition">query condition such as AST%</param>
        /// <returns></returns>
        public IList<object> FuzzyQueryExtendedProperty(string condition)
        {
            var values = (from p in ProductInfoes
                          where IsLike(condition,p.InfoValue)
                          select p.InfoValue).ToArray();
            return values;
        }
        /// <summary>
        /// �ַ����Ƿ����SQLͨ�������ʽ(LIKE)
        /// </summary>
        /// <param name="sqlWildcardStr">SQLͨ�������ʽ</param>
        /// <param name="strToCmp">Ҫ�����ַ���</param>
        /// <returns>LIKE���</returns>
        private static bool IsLike(string sqlWildcardStr, string strToCmp)
        {
            Regex regex = new Regex(string.Format("^{0}$", sqlWildcardStr.Replace("%", ".*").Replace("_", ".")));
            return regex.IsMatch(strToCmp);
        }

        /// <summary>
        /// ���ü��Ŀ�����Ļ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        public void SetProperty(string name, object value)
        {
            GetType().GetProperty(name).SetValue(this, value, null);
        }

        /// <summary>
        /// ���ü��Ŀ��������չ����
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        /// <param name="editor"></param>
        public void SetExtendedProperty(string name, object value, string editor)
        {
            //(from p in ProductInfoes
            // where p.InfoType == name
            // select p).ToArray()[0].InfoValue = (string)value;

            if (string.IsNullOrEmpty(name))
                return;

            lock (_syncObj_infoes)
            {
                object naught = ProductInfoes;

                ProductInfo[] arr = (from p in ProductInfoes where p.InfoType == name select p).ToArray();

                if (arr.Length > 0)
                {
                    ProductInfo pdi = arr[0];
                    pdi.Editor = editor;
                    pdi.InfoValue = value.ToString();

                    pdi.IsInsertingOrUpdating = true;
                }
                else
                {
                    ProductInfo pdi = new ProductInfo();
                    pdi.Editor = editor;
                    pdi.InfoType = name;
                    pdi.InfoValue = value.ToString();
                    pdi.ProductID = _pcbId;

                    pdi.Tracker = _tracker.Merge(pdi.Tracker);
                    _infoes.Add(pdi);

                    pdi.IsInsertingOrUpdating = true;
                }
                
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// ��ȡ���Ŀ������pizza����
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>����ֵ</returns>
        public object GetPizzaProperty(string name)
        {
            object value = null;
            PropertyInfo prop = PizzaObj.GetType().GetProperty(name);
            if (prop != null)
            {
                value = prop.GetValue(PizzaObj, null);
            }
            return value;
        }

        /// <summary>
        /// ���ü��Ŀ������pizza����
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        public void SetPizzaProperty(string name, object value)
        {
            PizzaObj.GetType().GetProperty(name).SetValue(PizzaObj, value, null);
        }

//        /// <summary>
//        /// ��ȡBOM��ָ�����͵�Pn�б�
//        /// </summary>
//        /// <param name="item"></param>
//        /// <returns></returns>
//        public object GetBOMPartsByType(string item)
//        {
//            var repository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
//            return repository.GetPnFromMoBOMByType(MO, item);
//        }

        /// <summary>
        /// ȡ�ü����Ŀ�������Ŀͨ������ά�����洢�����ݿ��С�
        /// </summary>
        /// <param name="customer">�ͻ���ʶ(�ַ���)</param>
        /// <param name="station">վ��ʶ(�ַ���)</param>
        /// <returns>�����Ŀ���󼯺�</returns>
        //public IList<ICheckItem> GetExplicitCheckItem(string customer, string station)
        //{
        //    ICheckItemRepository rep =
        //        RepositoryFactory.GetInstance().GetRepository<ICheckItemRepository, ICheckItem>();

        //    return rep.GetProductExplicitCheckItem(ProId, customer, station);
        //}

        /// <summary>
        /// ȡ�ü����Ŀ�������Ŀͨ������ά�����洢�����ݿ��С�
        /// </summary>
        /// <param name="customer">�ͻ���ʶ(�ַ���)</param>
        /// <param name="station">վ��ʶ(�ַ���)</param>
        /// <returns>�����Ŀ���󼯺�</returns>
        //public IList<ICheckItem> GetImplicitCheckItem(string customer, string station)
        //{
        //    ICheckItemRepository rep =
        //        RepositoryFactory.GetInstance().GetRepository<ICheckItemRepository, ICheckItem>();

        //    return rep.GetProductImplicitCheckItem(ProId, customer, station);
        //}

        #endregion

        # region . Additional fields .

        /// <summary>
        /// Box���
        /// </summary>
        public string BoxId { get; private set; }

        /// <summary>
        /// �������
        /// </summary>
        public string MbSn { get; private set; }

        # endregion

        #region . Overrides of FisObjectBase .

        /// <summary>
        /// �����ʾkey, ��ͬ����FisObject��Χ��Ψһ
        /// </summary>
        public override object Key
        {
            get { return _proId; }
        }

        #endregion

        #region Implementation of IPartOwner

        /// <summary>
        /// ���ָ��part�Ƿ��Ѿ�������PartOwner��, ֻ����ͨ�ù����飬
        /// ����Ϊ�Ѱ�Part�������Product_Part, PCB_Part
        /// </summary>
        /// <param name="pn">pn</param>
        /// <param name="sn">sn</param>
        public void PartUniqueCheck(string pn, string sn)
        {
            var parts = PrdctRepository.GetProductPartsByPartSn(sn);
            //var parts = PrdctRepository.GetProductPartsByPartNoAndValue(pn, sn);
            if (parts != null)
            {
                foreach (var productPart in parts)
                {
                    if (productPart.ProductID != ProId)
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add("Part");
                        erpara.Add(sn);
                        erpara.Add(productPart.ProductID);
                        var ex = new FisException("CHK009", erpara);
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// ���ָ��part�Ƿ��Ѿ���any PartOwner��(including current owner), ֻ����ͨ�ù����飬
        /// ����Ϊ�Ѱ�Part�������Product_Part, PCB_Part, Pizza_Part
        /// </summary>
        /// <param name="pn">pn</param>
        /// <param name="sn">sn</param>
        public void PartUniqueCheckWithoutOwner(string pn, string sn)
        {
            var parts = PrdctRepository.GetProductPartsByPartSn(sn);
            //var parts = PrdctRepository.GetProductPartsByPartNoAndValue(pn, sn);
            if (parts != null && parts.Count > 0)
            {
                var productPart = parts.First();
                List<string> erpara = new List<string>();
                erpara.Add("Part");
                erpara.Add(sn);
                erpara.Add(parts.First().ProductID);
                var ex = new FisException("CHK009", erpara);
                throw ex;
            }
        }

        /// <summary>
        /// �ˬd��JPartNo/PartType/CT/CheckItemType �O�_�w�����L,�Y�������L����
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="partNoList"></param>
        /// <param name="BomNodeType"></param>
        /// <param name="checkItemType"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        public bool CheckPartBinded(string partType, IList<string> partNoList, string BomNodeType, string checkItemType, string sn)
        {
            IList<IProductPart> productPartList = this.ProductParts;
            bool hasBinded = productPartList.Any(x => x.BomNodeType == BomNodeType &&
                                                                            partNoList.Contains(x.PartID) &&
                                                                            x.CheckItemType == checkItemType &&
                                                                            x.PartSn == sn);
            return hasBinded;
        }

        /// <summary>
        /// ��Part�󶨵�PartOwner����
        /// ����Part���ͻ�ȡ��Ӧ��IPartStrategyʵ�֣�����IPartStrategy.BindTo()
        /// ��ʵ�ֲ�ͬ����Part�İ��߼�
        /// </summary>
        /// <param name="parts">Ҫ�󶨵�part�б�</param>
        public void BindPart(List<IProductPart> parts)
        {
            if (parts == null)
                return;

            foreach (IProductPart part in parts)
            {
                IPartStrategy stgy = PartStrategyFactory.GetPartStrategy(part.PartType, Customer);
                stgy.BindTo(part, this);
            }
        }

        /// <summary>
        /// ��ProductPart�б�������ָ��part
        /// ����PartOwner��Ĭ����Ϊ��ֻ��
        /// Part�����󶨵�Owner������Բ�
        /// ͬ��Part���ͽ�������Ĵ���
        /// </summary>
        /// <param name="part">part</param>
        public void AddPart(IProductPart part)
        {
            if (part == null)
                return;

            lock (_syncObj_parts)
            {
                object naught = ProductParts;
                ((ProductPart)part).Tracker = _tracker.Merge(((ProductPart)part).Tracker);
                _parts.Add(part);
                _tracker.MarkAsAdded(part);
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// ��ProductPart�б���ɾ��ָ��part
        /// </summary>
        /// <param name="partSn">partSn</param>
        /// <param name="partPn">partPn</param>
        public void RemovePart(string partSn, string partPn)
        {
            lock (_syncObj_parts)
            {
                object naught = ProductParts;
                if (_parts != null)
                {
                    foreach (IProductPart pt in _parts)
                    {
                        if (string.Compare(pt.Value, partSn) == 0 && string.Compare(pt.PartID, partPn) == 0)
                        {
                            ((ProductPart)pt).Tracker = null;
                            _tracker.MarkAsDeleted(pt);
                            _tracker.MarkAsModified(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ɾ�����е�Parts
        /// </summary>
        public void RemoveAllParts()
        {
            lock (_syncObj_parts)
            {
                object naught = ProductParts;
                if (_parts != null)
                {
                    foreach (IProductPart pt in _parts)
                    {
                        ((ProductPart)pt).Tracker = null;
                        _tracker.MarkAsDeleted(pt);
                    }
                }
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// ɾ��ָ��CheckItemType��Part
        /// </summary>
        public void RemovePartsByType(string type)
        {
            lock (_syncObj_parts)
            {
                object naught = ProductParts;
                if (_parts != null)
                {
                    foreach (IProductPart pt in _parts)
                    {
                        if (pt.CheckItemType.CompareTo(type) == 0)
                        {
                            ((ProductPart)pt).Tracker = null;
                            _tracker.MarkAsDeleted(pt);
                        }
                    }
                }
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// ɾ��ָ��BomNodeType���͵�Part
        /// </summary>
        public void RemovePartsByBomNodeType(string bomNodeType)
        {
            lock (_syncObj_parts)
            {
                object naught = ProductParts;
                if (_parts != null)
                {
                    foreach (IProductPart pt in _parts)
                    {
                        if (pt.BomNodeType.CompareTo(bomNodeType) == 0)
                        {
                            ((ProductPart)pt).Tracker = null;
                            _tracker.MarkAsDeleted(pt);
                        }
                    }
                }
                _tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// ��ȡָ��Station�ռ���Part
        /// </summary>
        /// <param name="station">Station</param>
        /// <returns>Part�б�</returns>
        public IList<IProductPart> GetProductPartByStation(string station)
        {
            IList<IProductPart> parts = new List<IProductPart>();
            lock (_syncObj_parts)
            {
                object naught = ProductParts;
                IPartRepository rep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                foreach (var productPart in _parts)
                {
                    if (productPart.Station.Equals(station))
                    {
                        IPart pt = rep.Find(productPart.PartID);
                        ((ProductPart)productPart).SetPartTypeSilently(pt.Type);
                        IProductPart part = ProductPart.PartReverseSpecialDeal(productPart);
                        parts.Add(part);
                    }
                }
            }
            return parts;
        }

        #endregion

        #region UPS
        /// <summary>
        /// whether bind UPSPo
        /// </summary>
        public bool IsBindedUPS 
        { 
            get 
            {
                if (this.UPSCombinePO == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }                
            }
         }

        /// <summary>
        /// whether is UPS Device
        /// </summary>
        public bool IsUPSDevice
        {
            get
            {
                if (_isUPSModel.HasValue)
                {
                    return _isUPSModel.Value;
                }
                else
                {
                    if (string.IsNullOrEmpty(this._model))
                    {
                        _isUPSModel = false;
                    }
                    else
                    {
                        //DateTime afterReceiveDate = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-15);
                        //_isUPSModel = UPSRep.IsUPSModel(this._model, afterReceiveDate);
                        _isUPSModel = UPSRep.IsUPSModel(this._model);
                    }
                    return _isUPSModel.Value;
                }
            }
        }

        /// <summary>
        /// whether is UPS Ship PO
        /// </summary>
        public bool IsUPSShipPO
        {
            get
            {
                fons.UPSCombinePO combinePo = this.UPSCombinePO;
                if (combinePo != null && 
                    combinePo.IsShipPO == "Y" &&
                    !string.IsNullOrEmpty(combinePo.IECPO))
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
        }
        /// <summary>
        /// combine UPS PO
        /// </summary>
        public fons.UPSCombinePO UPSCombinePO
        {
            get
            {
                lock (_syncObj_UPSObj)
                {
                    if (_upsCombinePO == null)
                    {
                        if (!string.IsNullOrEmpty(this._proId))
                        {
                            _upsCombinePO = UPSRep.Find(this._proId);
                        }
                    }
                    return _upsCombinePO;
                }
            }
        }
       
        #endregion
        public string GetAttribute(string name)
        {
            string value = GetExtendedProperty(name);
            if (value == null)
            {
                value = GetAttributeValue(name);
            }
            return value;
        }

        private IMB _mbObj;
        private object _syncObj_mbObj = new object();

        /// <summary>
        /// Combine MB object
        /// </summary>
        public IMB MB
        {
            get
            {
                lock (_syncObj_mbObj)
                {
                    if (!string.IsNullOrEmpty(_pcbId) && _mbObj == null)
                    {
                        _mbObj = mbRep.Find(_pcbId);
                    }
                    return _mbObj;
                }
            }
        }

        private IMES.FisObject.PAK.DN.Delivery _dnObj;
        private object _syncObj_dnObj = new object();

        /// <summary>
        /// Combined Delivery object
        /// </summary>
        public IMES.FisObject.PAK.DN.Delivery Delivery
        {
            get
            {
                lock (_syncObj_dnObj)
                {
                    if (!string.IsNullOrEmpty(_deliveryNo) && _dnObj == null)
                    {
                        _dnObj = dnRep.Find(_deliveryNo);
                    }
                    return _dnObj;
                }
            }
        }
    }
}