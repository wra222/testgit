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
    /// �����ࡣ������PCAվ����Ҫ�������󣬴󲿷ֲ���������������ɡ�
    /// </summary>
    public class MB : FisObjectBase, IMB
    {
        /// <summary>
        /// �忨����
        /// </summary>
        public static class MBType
        {
            public const string MB = "MB";    
            public const string VB = "VB";        
            public const string SB = "SB";

            /// <summary>
            /// ��ȡ���а忨����
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
        /// ����һ���յ��������
        /// </summary>
//        public MB()
//        {
//            this._tracker.MarkAsAdded(this);
//        }

        /// <summary>
        /// ����һ���µ��������
        /// </summary>
        /// <param name="sn">�������</param>
        /// <param name="mo">���충����</param>
        /// <param name="custSn">����ͻ����к�</param>
        /// <param name="model">�����ͺ�</param>
        /// <param name="datecode">����������</param>
        /// <param name="mac">����ý���ȡ���Ƶ�ַ</param>
        /// <param name="uuid">����ͨ��Ψһ��ʶ</param>
        /// <param name="ecr">???</param>
        /// <param name="iecVer">IEC�ͻ��汾��</param>
        /// <param name="custVer">����ͻ��汾��</param>
        /// <param name="udt">�����¼��������</param>
        /// <param name="cdt">�����¼��������</param>
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
        /// �����������
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
        /// �������충����
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
        /// ���������ͺš�
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
        /// ���������ͺš�
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
        /// ��������������
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
        /// ����ý���ȡ���Ƶ�ַ
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
        /// ����ͨ��Ψһ��ʶ
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
        /// IEC�汾�š��ַ������͡�
        /// IEC���������汾�š�
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
        /// �ͻ��汾�š��ַ������͡�
        /// �ͻ����������汾�š�
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
        /// ��¼���������롣�������͡�
        /// ��¼�������ݵĸ������ں�ʱ�䡣
        /// </summary>
        public DateTime Udt
        {
            get { return _udt; }
        }

        /// <summary>
        /// ��¼���������롣�������͡�
        /// ��¼�������ݵĴ������ں�ʱ�䡣
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
        /// Product�������Ͷ���
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
        /// Product�������Ͷ���
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
        /// ����1397��model
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
        /// 1397�׶�Ӧ��cusomer, ֻ��������ڶ�Ӧ1397��ʱ����ֵ
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
        /// �Ƿ���Trial Run����
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
                //CheckCode����Ϊ��R��������RCTO��
                //CheckCode:��MBSN�ĵ�5��Ϊ��M������ȡMBSN�ĵ�6�룬����Ϊ��7��
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
        /// ��������״̬��ʶ��ö�����͡�
        /// ��ʶ���嵱ǰ������״̬��
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
        /// �Ѱ󶨵�Part
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
        /// MB�����б�
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
        /// ά�޼�¼�б�
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
        /// ��վlog�б�
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
        /// ����log�б�
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
        /// PCB �Ƴ̿���״̬
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
        /// PCB �Ƴ̿���״̬Log
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
        /// ���RepairDefects
        /// </summary>
        /// <param name="rep"></param>
        public static void FillingRepairDefects(Repair rep)
        {
            MbRepository.FillRepairDefectInfo(rep);
        }

        /// <summary>
        /// ���TestLogDefects
        /// </summary>
        /// <param name="tstlg"></param>
        public static void FillingTestLogDefects(TestLog tstlg)
        {
            MbRepository.FillTestLogDefectInfo(tstlg);
        }

        #endregion

        #region . About Repair (IRepairTarget) .

        /// <summary>
        /// ȡ����������ά�޼�¼��
        /// </summary>
        /// <returns>ά�޶��󼯺�</returns>
        public IList<Repair> GetRepair()
        {
            lock (_syncObj_repair)
            {
                return this.Repairs;
            }
        }

        /// <summary>
        /// ������������һ������ά�޼�¼��
        /// </summary>
        /// <param name="rep">ά�޼�¼����</param>
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
        /// Ϊָ��Repair����һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
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
        /// Ϊָ��Repairɾ��һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="repairDefectId">ָ��RepairDefect��Id</param>
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
        /// Ϊָ��Repair����һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="defect">ָ��RepairDefect</param>
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
            return MbRepository.GetRepairDefectBySiteComponent(this.Sn, site, component);
        }

        /// <summary>
        /// complteһ��ά��
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
        /// ��ָ��partID��Ӧ��Part�滻
        /// </summary>
        /// <param name="partID">ָ��partID</param>
        /// <param name="newPart">��Part</param>
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
        /// ��ά�޶���(MB/Product)�Ļ���
        /// </summary>
        public string RepairTargetModel
        {
            get { return Model; }
        }

        /// <summary>
        /// �Ƿ����ά��
        /// �жϹ���1.����������ά�޵ļ�¼��
        ///                   2.����һ��TestLog��Status = Fail
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
        /// ���µĲ���log
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
        /// ������־��
        /// </summary>
        /// <param name="log">��־����</param>
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
        /// ȡ�ò�����־���ϡ�ͨ�����ڲ��һ����ض���־��Ϣ��
        /// </summary>
        /// <returns>���ز�����־�����б���</returns>
        public IList<TestLog> GetTestLog()
        {
            lock (_syncObj_testLogs)
            {
                return this.TestLogs;
            }
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
        /// Ϊָ��Repair����һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
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
        /// Ϊָ��Repairɾ��һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="repairDefectId">ָ��RepairDefect��Id</param>
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
        /// Ϊָ��Repair����һ��RepairDefect
        /// </summary>
        /// <param name="repairId">ָ��Repair��Id</param>
        /// <param name="defect">ָ��RepairDefect</param>
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
        /// ��ȡָ������ֵ
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>����ֵ</returns>
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
        /// ��һ����/����վ���������͡�
        /// ������һ������/����վ����
        /// (��ʱ����)
        /// </summary>
        //public IMES.FisObject.Common.Station.Station NextStation
        //{
        //   get { throw new NotImplementedException(); }
        //}

        /// <summary>
        /// ȷ���Ƿ���FA�׶Ρ��������͡�
        /// �������嵱ǰ�Ƿ���FA�׶Ρ�
        /// (��ʱ����)
        /// </summary>
//        public bool IsInFAPhrase
//        {
//            get { throw new NotImplementedException(); }
//        }

        /// <summary>
        /// �����и�״̬���������͡�
        /// ���������Ƿ������������и
        /// </summary>
        public bool HasBeenCut
        {
            get { return this.MBStatus.Status.Equals(MBStatusEnum.CL); }
        }

        /// <summary>
        /// �������ָ���������Ӱ�
        /// </summary>
        /// <param name="qty">�Ӱ�����</param>
        public void GenerateChildMB(int qty)
        {
            
        }

        /// <summary>
        /// ����MTA��ǡ�
        /// </summary>
        public void SetMTAMark()
        {

        }

        /// <summary>
        /// ��λMTA��ǡ�
        /// </summary>
        public void ResetMTAMark()
        {

        }

        /// <summary>
        /// ��CPU��Ӧ����Ϣ��
        /// </summary>
        /// <param name="vendor">CPU��Ӧ�̱�ʶ(�ַ���)</param>
        public void BindCPUVendor(string vendor)
        {

        }

        /// <summary>
        /// ��ȡָ��station��log
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
        /// ������������
        /// </summary>
        /// <param name="info">��������</param>
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
        /// ��ȡModelָ������
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public object GetModelProperty(string item)
        {
            throw new NotImplementedException();
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
            PropertyInfo prop = this.GetType().GetProperty(name);
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
        /// ���ü��Ŀ�����Ļ�������
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
        public void SetProperty(string name, object value)
        {
            this.GetType().GetProperty(name).SetValue(this, value, null);
        }

        /// <summary>
        /// ���ü��Ŀ��������չ����
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
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
        /// ��ȡ���Ŀ������pizza����
        /// </summary>
        /// <param name="name">������</param>
        /// <returns>����ֵ</returns>
        public object GetPizzaProperty(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ���ü��Ŀ������pizza����
        /// </summary>
        /// <param name="name">������</param>
        /// <param name="value">����ֵ</param>
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
        /// ȡ�ü����Ŀ�������Ŀͨ������ά�����洢�����ݿ��С�
        /// </summary>
        /// <param name="customer">�ͻ���ʶ(�ַ���)</param>
        /// <param name="station">վ��ʶ(�ַ���)</param>
        /// <returns>�����Ŀ���󼯺�</returns>
        //public IList<ICheckItem> GetExplicitCheckItem(string customer, string station)
        //{
        //    ICheckItemRepository rep =
        //        RepositoryFactory.GetInstance().GetRepository<ICheckItemRepository, ICheckItem>();

        //    return rep.GetMBExplicitCheckItem(this.Sn, customer, station);
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

        //    return rep.GetMBImplicitCheckItem(this.Sn, customer, station);
        //}

        public IList<object> FuzzyQueryExtendedProperty(string condition)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region . Overrides of FisObjectBase .

        /// <summary>
        /// �����ʾkey, ��ͬ����FisObject��Χ��Ψһ
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
        /// ���ָ��part�Ƿ��Ѿ�������PartOwner��, ֻ����ͨ�ù����飬
        /// ����Ϊ�Ѱ�Part�������Product_Part, PCB_Part
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
        /// ���ָ��part�Ƿ��Ѿ���any PartOwner��(including current owner), ֻ����ͨ�ù����飬
        /// ����Ϊ�Ѱ�Part�������Product_Part, PCB_Part, Pizza_Part
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
            IList<IProductPart> MBPartList = this.MBParts;
            bool hasBinded = MBPartList.Any(x => x.BomNodeType == BomNodeType &&
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
                IPartStrategy stgy = PartStrategyFactory.GetPartStrategy(part.PartType, this.Customer);
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

            lock (_syncObj_part)
            {
                foreach (var oldPart in this.MBParts)
                {

                    //PCA Shipping Label �е�VGA SN/CPU SN ����Ҫ����Ƿ��MB �Ѱ������Ƿ�һ�£�
                    //�������MB �Ѿ���VGA SN/CPU SN �ȣ���Save ��ʱ�����Update�������δ�󶨣���Insert
                    //��ΪMB ����ȱʧ�˽��part��վ�������Ҫ�˴����⴦����������ݲο�GetMB��ע��
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
        /// ��MBRepair�б�������Repair record
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
        /// ��ProductPart�б���ɾ��ָ��part
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
        /// ɾ��ָ��CheckItemType��Part
        /// </summary>
        public void RemovePartsByType(string type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ɾ��ָ��BomNodeType���͵�Part
        /// </summary>
        public void RemovePartsByBomNodeType(string bomNodeType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ɾ�����е�Parts
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
        /// ��ȡָ��վ�󶨵�Part
        /// </summary>
        /// <param name="station">station</param>
        /// <returns>Part�б�</returns>
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
        /// 131 ���Ƹ� PCBModelID 
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