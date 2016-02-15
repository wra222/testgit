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
    /// Product类，Prodcut是FA，PAK的主要操作对象
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
        /// Product厂内标识,作为Product对象的主键
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
        /// Product所属机型名称
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
        /// 与Product绑定的板卡ID
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
        /// 与Product绑定的Mac地址
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
        /// 与Product绑定的UUID
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
        /// IEC版本
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
        ///// 客户版本
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
        /// Product的客户Sn
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
        ///// 与Product绑定的BIOS
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
        ///// 与Product绑定的Image版本
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
        ///// 与Product绑定的IMEI
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
        ///// 与Product绑定的MEID
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
        ///// 与Product绑定的ICCID
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
        ///// 与Product绑定的COA
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
        /// 与Product结合的Pizza
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
        /// Product所属的MO
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
        /// Product的重量
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
        /// Product所属的Carton
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
        /// Product的Carton重量
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
        /// Product所属的Delivery
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
        /// Product所属的Pallet
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
        /// Product所属机型对象
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
        /// Product所属机型对象
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
        /// Pruduct过站状态
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
        /// 获取与Product绑定的所有part
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
        /// 获取所有QCStatus
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
        /// 获取与Product绑定的所有log
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
        /// 取得测试日志集合。通常用于查找或检查特定日志信息。
        /// </summary>
        /// <returns>返回测试日志对象列表。</returns>
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
        /// 取得主板所有维修记录。
        /// </summary>
        /// <returns>维修对象集合</returns>
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
        /// 变更Log
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
        /// Product 扩展属性
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
        /// Product 制程控制状态
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
        /// Product 制程控制状态Log
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
        /// Product对应的cusomer
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
        /// 是否允许做Rework/Dismantle
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
                //  FROM [IMES2012_GetData].[dbo].[ModelInfo] where [Model]= @model and Name=’ATSNAV’
                //IF @ATSNAV = ‘’ then not Shell(CDSI)；
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
        /// 填充Product的指定Repair对象的RepairDefect
        /// </summary>
        /// <param name="rep">需填充的Repair</param>
        public static void FillingRepairDefects(Repair rep)
        {
            PrdctRepository.FillRepairDefectInfo(rep);
        }

        /// <summary>
        /// 填充指定TestLog的TestLogDefect
        /// </summary>
        /// <param name="tstlg">指定TestLog</param>
        public static void FillingTestLogDefects(TestLog tstlg)
        {
            PrdctRepository.FillTestLogDefectInfo(tstlg);
        }

        #endregion

        #region . About Repair (IRepairTarget) .

        /// <summary>
        /// 取得主板所有维修记录。
        /// </summary>
        /// <returns>维修对象集合</returns>
        public IList<Repair> GetRepair()
        {
            return Repairs;
        }

        /// <summary>
        /// 向主板中添加一条添加维修记录。
        /// </summary>
        /// <param name="rep">维修记录对象</param>
        /// <returns>Defect号</returns>
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
        /// 为指定Repair增加一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
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
        /// 为指定Repair删除一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
        /// <param name="repairDefectId">指定RepairDefect的Id</param>
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
        /// 为指定Repair更新一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
        /// <param name="defect">指定RepairDefect</param>
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
        /// 取得主板当前维修记录。
        /// </summary>
        /// <returns>当前维修对象</returns>
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
        /// 获取指定Site+Component的维修历史
        /// </summary>
        /// <param name="site">site</param>
        /// <param name="component">component</param>
        /// <returns>维修历史</returns>
        public IList<RepairDefect> GetRepairDefectBySiteComponent(string site, string component)
        {
            throw new NotImplementedException("Product does not need to implement this method");
            //return PrdctRepository.GetRepairDefectBySiteComponent(ProId, site, component);
        }

        /// <summary>
        /// complte一次维修
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
        /// 改Part
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
        /// complte一次维修
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
        /// RepairTarget机型
        /// </summary>
        public string RepairTargetModel
        {
            get { return Model; }
        }

        /// <summary>
        /// 是否初次维修
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
        /// 最新的测试log
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
        /// 记录Product过站记录
        /// </summary>
        /// <param name="log">Product过站记录</param>
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
        /// 记录Product修改记录
        /// </summary>
        /// <param name="log">Product修改记录</param>
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
        /// 设置制程控制状态值
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
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
        /// 获取指定属性值
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
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
        /// 添加测试记录
        /// </summary>
        /// <param name="testLog">测试记录</param>
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
        /// 取得测试日志集合。通常用于查找或检查特定日志信息。
        /// </summary>
        /// <returns>返回测试日志对象列表。</returns>
        public IList<TestLog> GetTestLog()
        {
            return TestLog;
        }

        # endregion

        #region . Implementation of IProduct .

        /// <summary>
        /// 更新Product过站状态
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
        /// 添加QCStatus
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
        /// 更新QCStatus
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
        /// Product 对应BOM中是否包含HDVD
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
        /// 转换为IMES.DataModel.ProductInfo
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
        /// 获取Model指定属性
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
        /// 保存Model指定属性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetModelProperty(string name, object value)
        {
            throw new NotImplementedException("A property of Model can not be updated.");
        }

        /// <summary>
        /// 获取检查目标对象的基本属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
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
        /// 获取检查目标对象的扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
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
        /// 字符串是否符合SQL通配符表达式(LIKE)
        /// </summary>
        /// <param name="sqlWildcardStr">SQL通配符表达式</param>
        /// <param name="strToCmp">要检查的字符串</param>
        /// <returns>LIKE与否</returns>
        private static bool IsLike(string sqlWildcardStr, string strToCmp)
        {
            Regex regex = new Regex(string.Format("^{0}$", sqlWildcardStr.Replace("%", ".*").Replace("_", ".")));
            return regex.IsMatch(strToCmp);
        }

        /// <summary>
        /// 设置检查目标对象的基本属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public void SetProperty(string name, object value)
        {
            GetType().GetProperty(name).SetValue(this, value, null);
        }

        /// <summary>
        /// 设置检查目标对象的扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
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
        /// 获取检查目标对象的pizza属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
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
        /// 设置检查目标对象的pizza属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public void SetPizzaProperty(string name, object value)
        {
            PizzaObj.GetType().GetProperty(name).SetValue(PizzaObj, value, null);
        }

//        /// <summary>
//        /// 获取BOM中指定类型的Pn列表
//        /// </summary>
//        /// <param name="item"></param>
//        /// <returns></returns>
//        public object GetBOMPartsByType(string item)
//        {
//            var repository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
//            return repository.GetPnFromMoBOMByType(MO, item);
//        }

        /// <summary>
        /// 取得检查条目。检查条目通过界面维护并存储在数据库中。
        /// </summary>
        /// <param name="customer">客户标识(字符串)</param>
        /// <param name="station">站标识(字符串)</param>
        /// <returns>检查条目对象集合</returns>
        //public IList<ICheckItem> GetExplicitCheckItem(string customer, string station)
        //{
        //    ICheckItemRepository rep =
        //        RepositoryFactory.GetInstance().GetRepository<ICheckItemRepository, ICheckItem>();

        //    return rep.GetProductExplicitCheckItem(ProId, customer, station);
        //}

        /// <summary>
        /// 取得检查条目。检查条目通过界面维护并存储在数据库中。
        /// </summary>
        /// <param name="customer">客户标识(字符串)</param>
        /// <param name="station">站标识(字符串)</param>
        /// <returns>检查条目对象集合</returns>
        //public IList<ICheckItem> GetImplicitCheckItem(string customer, string station)
        //{
        //    ICheckItemRepository rep =
        //        RepositoryFactory.GetInstance().GetRepository<ICheckItemRepository, ICheckItem>();

        //    return rep.GetProductImplicitCheckItem(ProId, customer, station);
        //}

        #endregion

        # region . Additional fields .

        /// <summary>
        /// Box编号
        /// </summary>
        public string BoxId { get; private set; }

        /// <summary>
        /// 主板序号
        /// </summary>
        public string MbSn { get; private set; }

        # endregion

        #region . Overrides of FisObjectBase .

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return _proId; }
        }

        #endregion

        #region Implementation of IPartOwner

        /// <summary>
        /// 检查指定part是否已经和其它PartOwner绑定, 只按照通用规则检查，
        /// 即认为已绑定Part都存放在Product_Part, PCB_Part
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
        /// 检查指定part是否已经和any PartOwner绑定(including current owner), 只按照通用规则检查，
        /// 即认为已绑定Part都存放在Product_Part, PCB_Part, Pizza_Part
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
        /// 浪琩PartNo/PartType/CT/CheckItemType 琌Μ栋筁,璝ゼΜ栋筁厨岿
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
        /// 将Part绑定到PartOwner对象
        /// 根据Part类型获取相应的IPartStrategy实现，调用IPartStrategy.BindTo()
        /// 以实现不同类型Part的绑定逻辑
        /// </summary>
        /// <param name="parts">要绑定的part列表</param>
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
        /// 在ProductPart列表中添加指定part
        /// 这是PartOwner的默认行为，只将
        /// Part单纯绑定到Owner不会针对不
        /// 同的Part类型进行特殊的处理
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
        /// 在ProductPart列表中删除指定part
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
        /// 删除所有的Parts
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
        /// 删除指定CheckItemType的Part
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
        /// 删除指定BomNodeType类型的Part
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
        /// 获取指定Station收集的Part
        /// </summary>
        /// <param name="station">Station</param>
        /// <returns>Part列表</returns>
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
