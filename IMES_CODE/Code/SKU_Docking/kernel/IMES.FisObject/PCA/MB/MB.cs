using System;
using System.Collections.Generic;
using System.Linq;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using System.Collections.ObjectModel;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Reflection;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MBModel;

namespace IMES.FisObject.PCA.MB
{
    /// <summary>
    /// 主板类。主板是PCA站的主要操作对象，大部分操作都基于主板完成。
    /// </summary>
    public class MB : FisObjectBase, IMB
    {
        /// <summary>
        /// 板卡类型
        /// </summary>
        public static class MBType
        {
            public const string MB = "MB";    
            public const string VB = "VB";        
            public const string SB = "SB";

            /// <summary>
            /// 获取所有板卡类型
            /// </summary>
            /// <returns></returns>
            public static string[] GetAllTypes()
            {
                List<string> ret = new List<string>();
                Type tp = typeof(MB.MBType);
                FieldInfo[] fis = tp.GetFields();
                if (fis != null && fis.Length > 0)
                {
                    foreach (FieldInfo fi in fis)
                    {
                        string typeName = (string)fi.GetValue(null);
                        ret.Add(typeName);
                    }
                }
                return ret.ToArray();
            }
        }

        //private static IMBRepository _mbRepository = null;
        private static readonly  IMBRepository MbRepository =RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
        //{
        //    get
        //    {
        //        if (_mbRepository == null)
        //            _mbRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
        //        return _mbRepository;
        //    }
        //}
        //private static IPartRepository _partRepository = null;
        private static readonly IPartRepository PartRepository =RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        //{
        //    get
        //    {
        //        if (_partRepository == null)
        //            _partRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
        //        return _partRepository;
        //    }
        //}

        /// <summary>
        /// 创建一个空的主板对象
        /// </summary>
//        public MB()
//        {
//            this._tracker.MarkAsAdded(this);
//        }

        /// <summary>
        /// 创建一个新的主板对象。
        /// </summary>
        /// <param name="sn">主板序号</param>
        /// <param name="mo">制造订单号</param>
        /// <param name="custSn">主板客户序列号</param>
        /// <param name="model">主板型号</param>
        /// <param name="datecode">主板日期码</param>
        /// <param name="mac">主板媒体存取控制地址</param>
        /// <param name="uuid">主板通用唯一标识</param>
        /// <param name="ecr">???</param>
        /// <param name="iecVer">IEC客户版本号</param>
        /// <param name="custVer">主板客户版本号</param>
        /// <param name="udt">主板记录更新日期</param>
        /// <param name="cdt">主板记录创建日期</param>
        public MB(string sn, string mo, string custSn, string model, string datecode, string mac, string uuid, string ecr, string iecVer, string custVer, string cvsn,DateTime udt, DateTime cdt)
        {
            _sn = sn;
            _mo = mo;
            _custSn = custSn;
            _model = model;
            _datecode = datecode;
            _mac = mac;
            _uuid = uuid;
            _ecr = ecr;
            _iecVer = iecVer;
            _custVer = custVer;
            _cvsn = cvsn;
            _udt = udt;
            _cdt = cdt;

            this._tracker.MarkAsAdded(this);
        }

        #region . Essential Fields .

        private string _sn = string.Empty;     //key
        private readonly string _mo = string.Empty;
        private string _custSn = string.Empty;
        private string _model = string.Empty;
        private string _datecode = string.Empty;
        private string _mac = string.Empty;
        private string _uuid = string.Empty;
        private string _ecr = string.Empty;
        private string _iecVer = string.Empty;
        private string _custVer = string.Empty;
        private string _cvsn = string.Empty;
        private string _state = string.Empty;
        private string _shipMode = string.Empty;
        private DateTime _udt = default(DateTime);
        private DateTime _cdt = default(DateTime);

        //Add field for RCT 147 
        private decimal _cartonWeight=default(decimal);
        private decimal _unitWeight = default(decimal);
        private string _cartonSN = string.Empty;
        private string _deliveryNo = string.Empty;
        private string _palletNo = string.Empty;
        private string _pizzaID = string.Empty;
        private string _qcStatus = string.Empty;
        private string _skuModel = string.Empty;


        /// <summary>
        /// 返回主板序号
        /// </summary>
        public string Sn
        {
            get { return _sn; }
        }

        /// <summary>
        /// Same table Column name
        /// </summary>
        public string PCBNo
        {
            get { return _sn; }
        }

        /// <summary>
        /// Custom Sn
        /// </summary>
        public string CustSn
        {
            get { return _custSn; }
            set
            {
                this._tracker.MarkAsModified(this);
                _custSn = value;
            }
        }

        /// <summary>
        /// Same PCB Table column Name
        /// </summary>
        public string CUSTSN
        {
            get { return _custSn; }
        }

        /// <summary>
        /// 返回制造订单号
        /// </summary>
        public string SMTMO
        {
            get { return _mo; }
        }

        public string OwnerId
        {
            get { return Sn; }
        }

        /// <summary>
        /// Family
        /// </summary>
        public string Family
        {
            get
            {
                //TODO: .Part.Descr, Contidion: GetData..Part.PartNo=@Model
                if (ModelObj != null)
                {
                    return ModelObj.Family;
                }
                return null;
            }
        }

        /// <summary>
        /// 返回主板型号。
        /// </summary>
        public string Model
        {
            get { return _model; }
            set 
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /// <summary>
        /// 返回主板型号。
        /// </summary>
        public string PCBModelID
        {
            get { return _model; }
            set
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }

        /// <summary>
        /// 返回主板日期码
        /// </summary>
        public string DateCode
        {
            get { return _datecode; }
            set
            {
                this._tracker.MarkAsModified(this);
                _datecode = value;
            }
        }

        /// <summary>
        /// 返回媒体存取控制地址
        /// </summary>
        public string MAC
        {
            get { return _mac; }
            set
            {
                this._tracker.MarkAsModified(this);
                _mac = value;
            }
        }

        /// <summary>
        /// 返回通用唯一标识
        /// </summary>
        public string UUID
        {
            get { return _uuid; }
            set
            {
                this._tracker.MarkAsModified(this);
                _uuid = value;
            }
        }

        /// <summary>
        /// ECR
        /// </summary>
        public string ECR
        {
            get { return _ecr; }
            set
            {
                if (value.CompareTo(_ecr) == 0)
                {
                    return;
                }

                //if (!IsTrialRun)
                //{
                //    var repository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository, IMBModel>();
                //    string ecr = value.Substring(value.Length - 2,  2);
                //    if (!repository.IsEcrExistInEcrVersion(ecr))
                //    {
                //        var erpara = new List<string> { value };
                //        var ex = new FisException("CHK090", erpara);
                //        throw ex;
                //    }
                //}

                this._tracker.MarkAsModified(this);
                _ecr = value;
            }
        }

        /// <summary>
        /// IEC版本号。字符串类型。
        /// IEC定义的主板版本号。
        /// </summary>
        public string IECVER
        {
            get { return _iecVer; }
            set
            {
                this._tracker.MarkAsModified(this);
                _iecVer = value;
            }
        }

        /// <summary>
        /// 客户版本号。字符串类型。
        /// 客户定义的主板版本号。
        /// </summary>
        public string CUSTVER
        {
            get { return _custVer; }
            set
            {
                this._tracker.MarkAsModified(this);
                _custVer = value;
            }
        }

        /// <summary>
        /// Cpu vendor sn
        /// </summary>
        public string CVSN
        {
            get { return _cvsn; }
            set
            {
                this._tracker.MarkAsModified(this);
                _cvsn = value;
            }
        }

        /// <summary>
        /// PCB State:Ship out
        /// </summary>
        public string State
        {
            get { return _state; }
            set
            {
                this._tracker.MarkAsModified(this);
                _state = value;
            }
        }

        /// <summary>
        /// Shiping Mode: RCTO/FRU/SKU
        /// </summary>
        public string ShipMode
        {
            get { return _shipMode; }
            set
            {
                this._tracker.MarkAsModified(this);
                _shipMode = value;
            }
        }

        /// <summary>
        /// 记录更新日期码。日期类型。
        /// 记录该条数据的更新日期和时间。
        /// </summary>
        public DateTime Udt
        {
            get { return _udt; }
        }

        /// <summary>
        /// 记录创建日期码。日期类型。
        /// 记录该条数据的创建日期和时间。
        /// </summary>
        public DateTime Cdt
        {
            get { return _cdt; }
        }

        //for RCTO 147
        public decimal CartonWeight {
            get { return _cartonWeight; }
            set
            {
                this._tracker.MarkAsModified(this);
                _cartonWeight = value;
            }
        }

        public decimal UnitWeight
        {
            get { return _unitWeight; }
            set
            {
                this._tracker.MarkAsModified(this);
                _unitWeight = value;
            }
        }

        public string CartonSN
        {
            get { return _cartonSN; }
            set
            {
                this._tracker.MarkAsModified(this);
                _cartonSN = value;
            }
        } 
    
        public string DeliveryNo
        {
            get { return _deliveryNo; }
            set
            {
                this._tracker.MarkAsModified(this);
                _deliveryNo = value;
            }
        } 

        public string PalletNo
        {
            get { return _palletNo; }
            set
            {
                this._tracker.MarkAsModified(this);
                _palletNo = value;
            }
        } 
       
        public string PizzaID
        {
            get { return _pizzaID; }
            set
            {
                this._tracker.MarkAsModified(this);
                _pizzaID = value;
            }
        } 
   
        public string QCStatus
        {
            get { return _qcStatus; }
            set
            {
                this._tracker.MarkAsModified(this);
                _qcStatus = value;
            }
        }

        public string SkuModel
        {
            get { return _skuModel; }
            set
            {
                this._tracker.MarkAsModified(this);
                _skuModel = value;
            }
        } 
        #endregion

        #region . Sub-Instances .

        private string _mb1397 = null;
        private Model _model1397Obj = null;
        private object _syncObj_model1397Obj = new object();
        private MBModel.MBModel _modelObj = null;
        private object _syncObj_modelObj = new object();
        private MBStatus _status = null;
        private object _syncObj_status = new object();
        private IList<IProductPart> _part = null;
        private object _syncObj_part = new object();
        private IList<MBRptRepair> _rptRepair = null;
        private object _syncObj_rptRepair = new object();
        private IList<MBInfo> _info = null;
        private object _syncObj_info = new object();
        private IList<Repair> _repair = null;
        private object _syncObj_repair = new object();
        private IList<MBLog> _logs = null;
        private object _syncObj_logs = new object();
        private IList<TestLog> _testLogs = null;
        private object _syncObj_testLogs = new object();
        private IList<ProductAttribute> _attributes = null;
        private object _syncObj_attributes = new object();
        private IList<ProductAttributeLog> _attributeLogs = null;
        private object _syncObj_attributeLogs = new object();

        /// <summary>
        /// Product所属机型对象
        /// </summary>
        public MBModel.MBModel ModelObj
        {
            get 
            {
                lock (_syncObj_modelObj)
                {
                    if (_modelObj == null)
                    {
                        MbRepository.FillModelObj(this);
                    }
                    return _modelObj;
                }
            }
        }

        public String MBCode { 
            get
            {
                string mbCode = null;
                IPart mbPart = PartRepository.Find(this._model);
                if (mbPart != null)
                {
                    mbCode = mbPart.GetAttribute("MB");
                }
                return mbCode;
            }
        }

        /// <summary>
        /// Product所属机型对象
        /// </summary>
        public Model Model1397Obj
        {
            get
            {
                lock (_syncObj_model1397Obj)
                {
                    if (_model1397Obj == null)
                    {
                        var repository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                        if (!string.IsNullOrEmpty(MB1397))
                        {
                            _model1397Obj = repository.Find(this.MB1397);
                        }
                    }
                    return _model1397Obj;
                }
            }
        }

        /// <summary>
        /// 主板1397阶model
        /// </summary>
        public string MB1397
        {
            get
            {
                return _mb1397;
            }
            set
            {
                _mb1397 = value;
            }
        }

        /// <summary>
        /// 1397阶对应的cusomer, 只有主板存在对应1397阶时才有值
        /// </summary>
        public string Customer
        {
            get
            {
                if (this.Model1397Obj != null && this.Model1397Obj.Family != null)
                {
                    return Model1397Obj.Family.Customer;
                }
                else if (this.ModelObj != null && this.ModelObj.FamilyObj != null)
                {
                    return this.ModelObj.FamilyObj.Customer;
                }
                return null;
            }
        }

        /// <summary>
        /// 是否是Trial Run主板
        /// </summary>
        public bool IsTrialRun
        {
            get
            {
                if (this.SMTMO.Substring(0, 1).CompareTo("P") == 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// if exists(select * from IMES2012_GetData..Part a(nolock),IMES2012_GetData..PartInfo b(nolock) 
        ///            where a.PartNo=b.PartNo
        ///            and a.PartNo=@Model
        ///            and a.BomNodeType = 'MB'
        ///            and b.InfoType='VGA'
        ///            and b.InfoValue = 'SV')
        ///         set @MBType = 'VB'

        /// </summary>
        public bool IsVB
        {
            get
            {
                if (this.ModelObj != null)
                {
                    return ModelObj.IsVB;
                }
                return false;
            }
        }

        public bool IsRCTO
        {
            get
            {
                //CheckCode，若为“R”，则是RCTO的
                //CheckCode:若MBSN的第5码为’M’，则取MBSN的第6码，否则为第7码
                if ((string.Compare(Sn, 4, "M", 0, 1) == 0 || string.Compare(Sn, 4, "B", 0, 1) == 0) && string.Compare(Sn, 5, "R", 0, 1) == 0)
                {
                    return true;
                }
                if ((string.Compare(Sn, 5, "M", 0, 1) == 0 || string.Compare(Sn, 5, "B", 0, 1) == 0)&& string.Compare(Sn, 6, "R", 0, 1) == 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 主板生产状态标识。枚举类型。
        /// 标识主板当前的生产状态。
        /// </summary>
        public MBStatus MBStatus
        {
            get
            {
                lock (_syncObj_status)
                {
                    if (_status == null)
                    {
                        MbRepository.FillStatus(this);
                    }
                    return _status;
                }
            }
            set
            {
                lock (_syncObj_status)
                {
                    this._tracker.MarkAsModified(this);
                    ((MBStatus)value).Tracker = this._tracker.Merge(((MBStatus)value).Tracker);
                    _status = value;
                }
            }
        }

        /// <summary>
        /// 已绑定的Part
        /// </summary>
        public IList<IProductPart> MBParts 
        {
            get 
            {
                lock (_syncObj_part)
                {
                    if (_part == null)
                    {
                        MbRepository.FillMBParts(this);
                    }
                    if (_part != null)
                    {
                        return new ReadOnlyCollection<IProductPart>(_part);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// mb rpt repair history
        /// </summary>
        public IList<MBRptRepair> MBRptRepairs
        {
            get
            {
                lock (_syncObj_rptRepair)
                {
                    if (_rptRepair == null)
                    {
                        MbRepository.FillMBRptRepairs(this);
                    }
                    if (_rptRepair != null)
                    {
                        return new ReadOnlyCollection<MBRptRepair>(_rptRepair);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// MB属性列表
        /// </summary>
        public IList<MBInfo> MBInfos 
        {
            get 
            {
                lock (_syncObj_info)
                {
                    if (_info == null)
                    {
                        MbRepository.FillMBInfos(this);
                    }
                    if (_info != null)
                    {
                        return new ReadOnlyCollection<MBInfo>(_info);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// 维修记录列表
        /// </summary>
        public IList<Repair> Repairs 
        {
            get 
            {
                lock (_syncObj_repair)
                {
                    if (_repair == null)
                    {
                        MbRepository.FillRepairs(this);
                    }
                    if (_repair != null)
                    {
                        return new ReadOnlyCollection<Repair>(_repair);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// 过站log列表
        /// </summary>
        public IList<MBLog> MBLogs 
        {
            get 
            {
                lock (_syncObj_logs)
                {
                    if (_logs == null)
                    {
                        MbRepository.FillMBLogs(this);
                    }
                    else
                    {
                        IList<MBLog> szAllAdd = new List<MBLog>();
                        foreach (MBLog ml in _logs)
                        {
                            if (ml.ID > 0)
                            {
                                szAllAdd.Clear();
                                break;
                            }
                            else
                            {
                                szAllAdd.Add(ml);
                            }
                        }
                        if (szAllAdd.Count > 0)
                        {
                            MbRepository.FillMBLogs(this);
                            foreach (MBLog ml in szAllAdd)
                            {
                                _logs.Add(ml);
                            }
                        }
                    }
                    if (_logs != null)
                    {
                        return new ReadOnlyCollection<MBLog>(_logs);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// 测试log列表
        /// </summary>
        public IList<TestLog> TestLogs 
        {
            get 
            {
                lock (_syncObj_testLogs)
                {
                    if (_testLogs == null)
                    {
                        MbRepository.FillTestLogs(this);
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
                            MbRepository.FillTestLogs(this);
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
        /// PCB 制程控制状态
        /// </summary>
        public IList<ProductAttribute> PCBAttributes
        {
            get
            {
                lock (_syncObj_attributes)
                {
                    if (_attributes == null)
                    {
                        MbRepository.FillPCBAttributes(this);
                    }
                    if (_attributes != null)
                    {
                        return new ReadOnlyCollection<ProductAttribute>(_attributes);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// PCB 制程控制状态Log
        /// </summary>
        public IList<ProductAttributeLog> PCBAttributeLogs
        {
            get
            {
                lock (_syncObj_attributeLogs)
                {
                    if (_attributeLogs == null)
                    {
                        MbRepository.FillPCBAttributeLogs(this);
                    }
                    if (_attributeLogs != null)
                    {
                        return new ReadOnlyCollection<ProductAttributeLog>(_attributeLogs);
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// 填充RepairDefects
        /// </summary>
        /// <param name="rep"></param>
        public static void FillingRepairDefects(Repair rep)
        {
            MbRepository.FillRepairDefectInfo(rep);
        }

        /// <summary>
        /// 填充TestLogDefects
        /// </summary>
        /// <param name="tstlg"></param>
        public static void FillingTestLogDefects(TestLog tstlg)
        {
            MbRepository.FillTestLogDefectInfo(tstlg);
        }

        #endregion

        #region . About Repair (IRepairTarget) .

        /// <summary>
        /// 取得主板所有维修记录。
        /// </summary>
        /// <returns>维修对象集合</returns>
        public IList<Repair> GetRepair()
        {
            lock (_syncObj_repair)
            {
                return this.Repairs;
            }
        }

        /// <summary>
        /// 向主板中添加一条添加维修记录。
        /// </summary>
        /// <param name="rep">维修记录对象</param>
        public void AddRepair(Repair rep)
        {
            if (rep == null)
                return;

            lock (_syncObj_repair)
            {
                //if (this._repair == null)
                //    this._repair = new List<Repair>();
                object naught = this.Repairs;
                if (this._repair.Contains(rep))
                    return;

                rep.Tracker = this._tracker.Merge(rep.Tracker);
                this._repair.Add(rep);
                rep.FillingRepairDefects += new FillRepair(FillingRepairDefects);
                this._tracker.MarkAsAdded(rep);
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 为指定Repair增加一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
        /// <param name="defect"></param>
        public void AddRepairDefect(int repairId, RepairDefect defect)
        {
            lock (_syncObj_repair)
            {
                object naught = this.Repairs;
                if (this._repair != null && defect != null)
                {
                    foreach (Repair rp in this._repair)
                    {
                        if (rp.Key.Equals(repairId))
                        {
                            rp.AddRepairDefect(defect);
                            rp.Tracker.MarkAsModified(rp);
                            this._tracker.MarkAsModified(this);
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
            lock (_syncObj_repair)
            {
                object naught = this.Repairs;
                if (this._repair != null)
                {
                    foreach (Repair rp in this._repair)
                    {
                        if (rp.Key.Equals(repairId))
                        {
                            rp.RemoveRepairDefect(repairDefectId);
                            rp.Tracker.MarkAsModified(rp);
                            this._tracker.MarkAsModified(this);
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
            lock (_syncObj_repair)
            {
                object naught = this.Repairs;
                if (this._repair != null && defect != null)
                {
                    foreach (Repair rp in this._repair)
                    {
                        if (rp.Key.Equals(repairId))
                        {
                            rp.UpdateRepairDefect(defect);
                            rp.Tracker.MarkAsModified(rp);
                            this._tracker.MarkAsModified(this);
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
            return MbRepository.GetRepairDefectBySiteComponent(this.Sn, site, component);
        }

        /// <summary>
        /// complte一次维修
        /// </summary>
        public void CompleteRepair(string line, string station, string editor)
        {
            var currentRepair = this.GetCurrentRepair(); 
            foreach(var repair in _repair)
            {
                if (repair.ID == currentRepair.ID)
                {
                    repair.SetStatus(Repair.RepairStatus.Finished);
                    //repair.SetLine(line);
                    //repair.SetStation(station);
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
        /// 将指定partID对应的Part替换
        /// </summary>
        /// <param name="partID">指定partID</param>
        /// <param name="newPart">新Part</param>
        public void ChangePart(int partID, IProductPart newPart)
        {
            foreach(var part in this.MBParts)
            {
                if (part.ID == partID)
                {
                    part.PartID = newPart.PartID;
                    part.Value = newPart.Value;
                    part.PartSn = newPart.PartSn;
                }
            }
        }

        public void RemoveRepair(int repairId)
        {
            lock (_syncObj_repair)
            {
                object naught = this.Repairs;
                if (this._repair != null)
                {
                    foreach (Repair rep in this._repair)
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
            lock (_syncObj_repair)
            {
                object naught = this.Repairs;
                if (this._repair != null)
                {
                    foreach (Repair rep in this._repair)
                    {
                        ((Repair)rep).Tracker = null;
                        this._tracker.MarkAsDeleted(rep);
                    }
                }
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 被维修对象(MB/Product)的机型
        /// </summary>
        public string RepairTargetModel
        {
            get { return Model; }
        }

        /// <summary>
        /// 是否初次维修
        /// 判断规则：1.不存在正在维修的记录；
        ///                   2.最新一笔TestLog的Status = Fail
        /// </summary>
        public bool IsFirstRepair
        {
            get
            {
                foreach (var repair in this.Repairs)
                {
                    if (repair.Status == Repair.RepairStatus.NotFinished)
                    {
                        return false;
                    }
                }
                return true;

//                if (this.TestLogs != null && this.TestLogs[0].Status == TestLog.TestLogStatus.Fail)
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
                for (int i = 0; i < TestLogs.Count; i++)
                {
                    if (TestLogs[i].Status == TestLog.TestLogStatus.Fail)
                    {
                        return TestLogs[i];
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
                for(int i = 0; i < TestLogs.Count; i++)
                {
                    if (TestLogs[i].Station.Equals(this.MBStatus.Station))
                    {
                        return TestLogs[i];
                    }
                }
                return null;
            }
        }

        #endregion

        #region . About Log .

        /// <summary>
        /// 添加日志。
        /// </summary>
        /// <param name="log">日志对象</param>
        public void AddLog(MBLog log)
        {
            if (log == null)
                return;

            lock (_syncObj_logs)
            {
                if (this._logs == null)
                {
                    this._logs = new List<MBLog>();
                }

                object naught = this.MBLogs;
                if (this._logs.Contains(log))
                    return;

                log.Tracker = this._tracker.Merge(log.Tracker);
                this._logs.Add(log);
                this._tracker.MarkAsAdded(log);
                this._tracker.MarkAsModified(this);
            }
        }

        public MBLog GetLatestFailLog()
        {
            return MbRepository.GetLatestFailLog(this.Sn);
        }

        /// <summary>
        /// 取得测试日志集合。通常用于查找或检查特定日志信息。
        /// </summary>
        /// <returns>返回测试日志对象列表。</returns>
        public IList<TestLog> GetTestLog()
        {
            lock (_syncObj_testLogs)
            {
                return this.TestLogs;
            }
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
                if (this._testLogs == null)
                {
                    this._testLogs = new List<TestLog>();
                }

                //object naught = this.TestLogs;
                if (this._testLogs.Contains(testLog))
                    return;

                testLog.Tracker = this._tracker.Merge(testLog.Tracker);
                this._testLogs.Add(testLog);
                testLog.FillingTestLogDefects += new FillTestLog(FillingTestLogDefects);
                this._tracker.MarkAsAdded(testLog);
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 为指定Repair增加一个RepairDefect
        /// </summary>
        /// <param name="repairId">指定Repair的Id</param>
        /// <param name="defect"></param>
        public void AddTestLogDefect(int testLogId, TestLogDefect defect)
        {
            lock (_syncObj_testLogs)
            {
                object naught = this.TestLogs;
                if (this._testLogs != null && defect != null)
                {
                    foreach (TestLog tl in this._testLogs)
                    {
                        if (tl.Key.Equals(testLogId))
                        {
                            tl.AddTestLogDefect(defect);
                            tl.Tracker.MarkAsModified(tl);
                            this._tracker.MarkAsModified(this);
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
        public void RemoveTestLogDefect(int testLogId, int testLogDefectId)
        {
            lock (_syncObj_testLogs)
            {
                object naught = this.TestLogs;
                if (this._testLogs != null)
                {
                    foreach (TestLog tl in this._testLogs)
                    {
                        if (tl.Key.Equals(testLogId))
                        {
                            tl.RemoveTestLogDefect(testLogDefectId);
                            tl.Tracker.MarkAsModified(tl);
                            this._tracker.MarkAsModified(this);
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
        public void UpdateTestLogDefect(int testLogId, TestLogDefect defect)
        {
            lock (_syncObj_testLogs)
            {
                object naught = this.TestLogs;
                if (this._testLogs != null && defect != null)
                {
                    foreach (TestLog tl in this._testLogs)
                    {
                        if (tl.Key.Equals(testLogId))
                        {
                            tl.UpdateTestLogDefect(defect);
                            tl.Tracker.MarkAsModified(tl);
                            this._tracker.MarkAsModified(this);
                            break;
                        }
                    }
                }
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
                object naught = this.PCBAttributes;

                foreach (var attrib in _attributes)
                {
                    if (attrib.AttributeName == name)
                    {
                        //update attribute
                        string oldValue = attrib.AttributeValue;
                        attrib.AttributeValue = value;
                        attrib.Editor = editor;

                        var pdal = new ProductAttributeLog()
                        {
                            Editor = editor,
                            AttributeName = name,
                            OldValue = oldValue,
                            NewValue = value,
                            ProductId = _sn,
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
                    ProductId = _sn
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
            if (PCBAttributes == null)
                return null;
            return _attributes.Where(x => x.AttributeName == name)
                                        .Select(x => x.AttributeValue).FirstOrDefault();
            //var values = (from p in this.PCBAttributes
            //              where p.AttributeName == name
            //              select p.AttributeValue).ToArray();
            //if (values != null && values.Length > 0)
            //{
            //    return values.First();
            //}
            //return null;
        }
        
        #endregion

        #region . Implementation of IMB  .

        /// <summary>
        /// 下一生产/测试站。对象类型。
        /// 返回下一个生产/测试站对象。
        /// (暂时无用)
        /// </summary>
        //public IMES.FisObject.Common.Station.Station NextStation
        //{
        //   get { throw new NotImplementedException(); }
        //}

        /// <summary>
        /// 确定是否在FA阶段。布尔类型。
        /// 返回主板当前是否在FA阶段。
        /// (暂时无用)
        /// </summary>
//        public bool IsInFAPhrase
//        {
//            get { throw new NotImplementedException(); }
//        }

        /// <summary>
        /// 主板切割状态。布尔类型。
        /// 返回主板是否已做过连扳切割。
        /// </summary>
        public bool HasBeenCut
        {
            get { return this.MBStatus.Status.Equals(MBStatusEnum.CL); }
        }

        /// <summary>
        /// 连板产生指定数量的子板
        /// </summary>
        /// <param name="qty">子板数量</param>
        public void GenerateChildMB(int qty)
        {
            
        }

        /// <summary>
        /// 设置MTA标记。
        /// </summary>
        public void SetMTAMark()
        {

        }

        /// <summary>
        /// 复位MTA标记。
        /// </summary>
        public void ResetMTAMark()
        {

        }

        /// <summary>
        /// 绑定CPU供应商信息。
        /// </summary>
        /// <param name="vendor">CPU供应商标识(字符串)</param>
        public void BindCPUVendor(string vendor)
        {

        }

        /// <summary>
        /// 获取指定station的log
        /// </summary>
        /// <param name="station">station</param>
        /// <param name="isPass">isPass</param>
        /// <returns></returns>
        public MBLog GetLogByStation(string station, bool isPass)
        {
            foreach (var log in MBLogs)
            {
                if (log.StationID.Equals(station) && ((log.Status == 1) == isPass))
                {
                    return log;                    
                }
            }
            return null;
        }

        /// <summary>
        /// 增加主板属性
        /// </summary>
        /// <param name="info">主板属性</param>
        public void AddMBInfo(MBInfo info)
        {
            if (info == null)
                return;

            lock (_syncObj_info)
            {
                object naught = this.MBInfos;
                if (this._info.Contains(info))
                    return;

                info.Tracker = this._tracker.Merge(info.Tracker);
                this._info.Add(info);
                this._tracker.MarkAsAdded(info);
                this._tracker.MarkAsModified(this);
            }
        }

        public void SetSn(string newSn)
        {
            this._sn = newSn;
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
            throw new NotImplementedException();
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
            PropertyInfo prop = this.GetType().GetProperty(name);
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
            if (MBInfos == null)
                return null;
            return _info.Where(x => x.InfoType == name)
                                    .Select(x => x.InfoValue).FirstOrDefault();
            //var values = (from p in this.MBInfos
            //              where p.InfoType == name
            //              select p.InfoValue).ToArray();
            //if (values != null && values.Length > 0)
            //{
            //    return values.First();
            //}
            //return null;
        }

        /// <summary>
        /// 设置检查目标对象的基本属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public void SetProperty(string name, object value)
        {
            this.GetType().GetProperty(name).SetValue(this, value, null);
        }

        /// <summary>
        /// 设置检查目标对象的扩展属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        /// <param name="editor"></param>
        public void SetExtendedProperty(string name, object value, string editor)
        {
            //(from p in this.MBInfos
            // where p.InfoType == name
            // select p).ToArray()[0].InfoValue = (string)value;

            if (string.IsNullOrEmpty(name))
                return;

            lock (_syncObj_info)
            {                
                MBInfo mbInfo =null;
                if (this.MBInfos != null && _info.Count > 0)
                {
                    mbInfo = (from p in _info where p.InfoType == name select p).FirstOrDefault();
                }

                if (mbInfo != null)
                {
                    mbInfo.Editor = editor;
                    mbInfo.InfoValue = value.ToString();
                }
                else
                {
                    MBInfo mbi = new MBInfo();
                    mbi.Editor = editor;
                    mbi.InfoType = name;
                    mbi.InfoValue = value.ToString();
                    mbi.PCBID = this._sn;

                    mbi.Tracker = this._tracker.Merge(mbi.Tracker);
                    this._info.Add(mbi);
                }
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 获取检查目标对象的pizza属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <returns>属性值</returns>
        public object GetPizzaProperty(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置检查目标对象的pizza属性
        /// </summary>
        /// <param name="name">属性名</param>
        /// <param name="value">属性值</param>
        public void SetPizzaProperty(string name, object value)
        {
            throw new NotImplementedException();
        }

//        public object GetBOMPartsByType(string item)
//        {
//            var repository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository, BOM>();
//            return repository.GetPnFromModelBOMByType(this.MB1397, item);
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

        //    return rep.GetMBExplicitCheckItem(this.Sn, customer, station);
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

        //    return rep.GetMBImplicitCheckItem(this.Sn, customer, station);
        //}

        public IList<object> FuzzyQueryExtendedProperty(string condition)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region . Overrides of FisObjectBase .

        /// <summary>
        /// 对象标示key, 在同类型FisObject范围内唯一
        /// </summary>
        public override object Key
        {
            get { return this._sn; }
        }

        //public override void Clean()
        //{
        //    //_statusChanged = false;
        //    //_logAdded.Clear();
        //    //base.Clean();
        //}

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
            //var parts = MbRepository.GetPCBPartsByPartNoAndValue(pn, sn);
            var parts = MbRepository.GetPCBPartsByPartSn(sn);
            if (parts != null)
            {
                foreach (var productPart in parts)
                {
                    if (productPart.ProductID != this.Sn)
                    {
                        List<string> erpara = new List<string>();
                        erpara.Add("Part");
                        erpara.Add(sn);
                        erpara.Add(productPart.ProductID);
                        var ex = new FisException("CHK008", erpara);
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
            var parts = MbRepository.GetPCBPartsByPartSn(sn);
            //var parts = MbRepository.GetPCBPartsByPartNoAndValue(pn, sn);
            if (parts != null && parts.Count > 0)
            {
                var productPart = parts.First();
                List<string> erpara = new List<string>();
                erpara.Add("Part");
                erpara.Add(sn);
                erpara.Add(parts.First().ProductID);
                var ex = new FisException("CHK008", erpara);
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
            IList<IProductPart> MBPartList = this.MBParts;
            bool hasBinded = MBPartList.Any(x => x.BomNodeType == BomNodeType &&
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
                IPartStrategy stgy = PartStrategyFactory.GetPartStrategy(part.PartType, this.Customer);
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

            lock (_syncObj_part)
            {
                foreach (var oldPart in this.MBParts)
                {

                    //PCA Shipping Label 中的VGA SN/CPU SN 不需要检查是否和MB 已绑定资料是否一致；
                    //如果发现MB 已经绑定VGA SN/CPU SN 等，在Save 的时候进行Update；如果尚未绑定，则Insert
                    //因为MB 重流缺失了解绑part的站，因此需要此处特殊处理，相关内容参考GetMB中注释
                    if (part.PartType.Equals(oldPart.PartType) &&
                       part.PartSn.Equals(oldPart.PartSn))
                    {
                        oldPart.PartID = part.PartID;
                        oldPart.Value = part.Value;
                        oldPart.Station = part.Station;
                        oldPart.Iecpn = part.Iecpn;
                        oldPart.CustomerPn = part.CustomerPn;
                        oldPart.BomNodeType = part.BomNodeType;
                        oldPart.CheckItemType = part.CheckItemType;
                        oldPart.Cdt = part.Cdt;
                        oldPart.Udt = DateTime.Now;
                        oldPart.Editor = part.Editor;
                        this._tracker.MarkAsModified(this);
                        return;
                    }
                }

                ((ProductPart)part).Tracker = this._tracker.Merge(((ProductPart)part).Tracker);
                this._part.Add(part);
                this._tracker.MarkAsAdded(part);
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 在MBRepair列表中添加Repair record
        /// </summary>
        /// <param name="rptRepair">Repair record</param>
        public void AddRptRepair(MBRptRepair rptRepair)
        {
            if (rptRepair == null)
                return;

            lock (_syncObj_rptRepair)
            {
                rptRepair.Tracker = this._tracker.Merge(rptRepair.Tracker);
                this._rptRepair.Add(rptRepair);
                this._tracker.MarkAsAdded(rptRepair);
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 在ProductPart列表中删除指定part
        /// </summary>
        /// <param name="partSn">partSn</param>
        /// <param name="partPn">partPn</param>
        public void RemovePart(string partSn, string partPn)
        {
            lock (_syncObj_part)
            {
                object naught = this.MBParts;
                if (this._part != null)
                {
                    foreach (IProductPart pt in this._part)
                    {
                        if (string.Compare(pt.Value, partSn) == 0 && string.Compare(pt.PartID, partPn) == 0)
                        {
                            ((ProductPart)pt).Tracker = null;
                            this._tracker.MarkAsDeleted(pt);
                            this._tracker.MarkAsModified(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除指定CheckItemType的Part
        /// </summary>
        public void RemovePartsByType(string type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除指定BomNodeType类型的Part
        /// </summary>
        public void RemovePartsByBomNodeType(string bomNodeType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除所有的Parts
        /// </summary>
        public void RemoveAllParts()
        {
            lock (_syncObj_part)
            {
                object naught = this.MBParts;
                if (this._part != null)
                {
                    foreach (IProductPart pt in this._part)
                    {
                        ((ProductPart)pt).Tracker = null;
                        this._tracker.MarkAsDeleted(pt);
                    }
                }
                this._tracker.MarkAsModified(this);
            }
        }

        /// <summary>
        /// 获取指定站绑定的Part
        /// </summary>
        /// <param name="station">station</param>
        /// <returns>Part列表</returns>
        public IList<IProductPart> GetProductPartByStation(string station)
        {
            IList<IProductPart> parts = new List<IProductPart>();
            IPartRepository rep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            foreach (var productPart in this.MBParts) 
            {
                if (productPart.Station.Equals(station))
                {
                    IPart pt = rep.Find(productPart.PartID);
                    ((ProductPart)productPart).SetPartTypeSilently(pt.Type);
                    IProductPart part = ProductPart.PartReverseSpecialDeal(productPart);
                    parts.Add(part);
                }
            }
            return parts;
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

        /// <summary>
        /// 131 顶腹 PCBModelID 
        /// </summary>
        public IPart Part
        {
            get
            {
                MBModel.MBModel mbModel = this.ModelObj;
                if (mbModel == null)
                {
                    return null;
                }
                else
                {
                    return mbModel.PartObj;
                }
            }
        }
    }
}