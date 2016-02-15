using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.ObjectModel;
using IMES.FisObject.Common.Process;
using IMES.DataModel;
using IMES.Infrastructure.Repository._Metas;
using mtns = IMES.Infrastructure.Repository._Metas;


namespace IMES.FisObject.Common.Material
{
    [ORMapping(typeof(mtns.Material))]
    public class Material : FisObjectBase, IAggregateRoot
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Material()
        {
            this._tracker.MarkAsAdded(this);
        }

        private const string _subTrackerName = "MaterialRelation";
        public string SubTrackerName
        {
            get
            {
                return _subTrackerName;
            }
        }
        private static IMaterialRepository _materialRepository;
        private static IMaterialRepository MaterialRepository
        {
            get
            {
                if (_materialRepository == null)
                    _materialRepository = RepositoryFactory.GetInstance().GetRepository<IMaterialRepository, Material>();
                return _materialRepository;
            }
        }

        private static IProcessRepository _procRepository;
        private static IProcessRepository ProcessRepository
        {
            get
            {
                if (_procRepository == null)
                    _procRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository>();
                return _procRepository;
            }
        }

        #region . Essential Fields .

        [ORMapping(mtns.Material.fn_materialCT)]
        private string _MaterialCT = null;

        [ORMapping(mtns.Material.fn_materialType)]
        private string _MaterialType = null;

        [ORMapping(mtns.Material.fn_lotNo)]
        private string _LotNo = null;

        [ORMapping(mtns.Material.fn_stage)]
        private string _Stage = null;
        [ORMapping(mtns.Material.fn_line)]
        private string _Line = null;
        [ORMapping(mtns.Material.fn_preStatus)]
        private string _PreStatus = null;
        [ORMapping(mtns.Material.fn_status)]
        private string _Status = null;
        [ORMapping(mtns.Material.fn_editor)]
        private string _Editor = null;
        [ORMapping(mtns.Material.fn_cdt)]
        private DateTime _Cdt = DateTime.MinValue;
        [ORMapping(mtns.Material.fn_udt)]
        private DateTime _Udt = DateTime.MinValue;

       //Add field for RCT 147 
        [ORMapping(mtns.Material.fn_model)]
        private string _model = null;
        [ORMapping(mtns.Material.fn_cartonSN)]
        private string _cartonSN = null;
        [ORMapping(mtns.Material.fn_deliveryNo)]
        private string _deliveryNo = null;
        [ORMapping(mtns.Material.fn_palletNo)]
        private string _palletNo = null;
        [ORMapping(mtns.Material.fn_pizzaID)]
        private string _pizzaID = null;
        [ORMapping(mtns.Material.fn_qcStatus)]
        private string _qcStatus = null;
        [ORMapping(mtns.Material.fn_cartonWeight)]
        private decimal _cartonWeight = default(decimal);
        [ORMapping(mtns.Material.fn_unitWeight)]
        private decimal _unitWeight = default(decimal);
        [ORMapping(mtns.Material.fn_shipMode)]
        private string _shipMode = null;

        public string MaterialCT
        {
            get
            {
                return this._MaterialCT;
            }
            set
            {
               this._tracker.MarkAsModified(this);
               this._MaterialCT = value;
            }
        }

        public string MaterialType
        {
            get
            {
                return this._MaterialType;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._MaterialType = value;
            }
        }

        public string LotNo
        {
            get
            {
                return this._LotNo;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._LotNo = value;
            }
        }

        public string Stage
        {
            get
            {
                return this._Stage;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Stage = value;
            }
        }

        public string Line
        {
            get
            {
                return this._Line;
            }
           set
            {
                this._tracker.MarkAsModified(this);
                this._Line = value;
            }
        }

        public string PreStatus
        {
            get
            {
                return this._PreStatus;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._PreStatus = value;
            }
        }

        public string Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Status = value;
            }
        }



        /// <summary>
        /// Editor
        /// </summary>
        public string Editor
        {
            get
            {
                return this._Editor;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Editor = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Cdt
        {
            get
            {
                return this._Cdt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Cdt = value;
            }
        }

        public DateTime Udt
        {
            get
            {
                return this._Udt;
            }
            set
            {
                this._tracker.MarkAsModified(this);
                this._Udt = value;
            }
        }

        //for RCTO 147
        public decimal CartonWeight
        {
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

        public string Model
        {
            get { return _model; }
            set
            {
                this._tracker.MarkAsModified(this);
                _model = value;
            }
        }
        public string ShipMode
        {
            get { return _shipMode; }
            set
            {
                this._tracker.MarkAsModified(this);
                _shipMode = value;
            }
        }
        #endregion

        #region IFisObject Members
        public override object Key
        {
            get { return _MaterialCT; }
        }

        #endregion

        #region external object

        private object _syncMaterialLotObj = new object();
        private MaterialLot _materialLot;
        public MaterialLot  MaterialLot
        {
            get
            {
                lock (_syncMaterialLotObj)
                {
                    if (_materialLot == null)
                    {
                        MaterialRepository.FillMaterialLot(this);
                    }

                    if (_materialLot != null)
                        return _materialLot;
                    else
                        return null;
                }
            }
        }        

        private object _syncMaterialLogObj = new object();
        private IList<MaterialLog> _materialLogList;
        public IList<MaterialLog> MaterialLogs
        {
            get
            {
                lock (_syncMaterialLogObj)
                {
                    if (_materialLogList == null)
                    {
                        MaterialRepository.FillMaterialLog(this);
                    }
                    if (_materialLogList != null)
                        return new ReadOnlyCollection<MaterialLog>(_materialLogList);
                    else
                        return null;
                }
            }
        }

        public void AddMaterialLog(MaterialLog log)
        {

            lock (_syncMaterialLogObj)
            {
                object naught = this.MaterialLogs;
                _materialLogList.Add(log);
                _tracker.MarkAsModified(_subTrackerName);
            }
        }


        private object _syncMaterialAttrObj = new object();
        private IList<MaterialAttr> _materialAttrList;
        public IList<MaterialAttr> MaterialAttributes
        {
            get
            {
                lock (_syncMaterialAttrObj)
                {
                    if (_materialAttrList == null)
                    {
                        MaterialRepository.FillMaterialAttr(this);
                    }
                    if (_materialAttrList != null)
                        return new ReadOnlyCollection<MaterialAttr>(_materialAttrList);
                    else
                        return null;
                }
            }
        }

        private object _syncMaterialAttrLogObj = new object();
        private IList<MaterialAttrLog> _materialAttrLogList;
        public IList<MaterialAttrLog> MaterialAttributeLogs
        {
            get
            {
                lock (_syncMaterialAttrLogObj)
                {
                    if (_materialAttrLogList == null)
                    {
                        MaterialRepository.FillMaterialAttrLog(this);
                    }
                    if (_materialLogList != null)
                        return new ReadOnlyCollection<MaterialAttrLog>(_materialAttrLogList);
                    else
                        return null;
                }
            }
        }


        private object _syncmaterialProcessObj = new object();
        private MaterialProcess _materialProcess = null;
        public MaterialProcess GetMaterialProcess
        {
            get
            {
                lock (_syncmaterialProcessObj)
                {
                    if (_materialProcess == null)
                    {
                        _materialProcess = ProcessRepository.GetMaterialProcessByType(_MaterialType);
                    }
                    return _materialProcess;
                }
            }
        }

        public bool CheckMaterailStatus(string nextStatus)
        {
            return ProcessRepository.CheckMaterialProcessStatus(_MaterialType, _Status, nextStatus);       
        }


        #endregion

        #region set attribute
        public void SetAttributeValue(string name, string value, string editor, string descr)
        {
            if (string.IsNullOrEmpty(name))
                return;

            lock (_syncMaterialAttrObj)
            {
                object naught = MaterialAttributes;              

                foreach (var attrib in _materialAttrList)
                {
                    if (attrib.AttrName == name)
                    {
                        //update attribute
                        string oldValue = attrib.AttrValue;
                        attrib.AttrValue = value;
                        attrib.Editor = editor;
                        attrib.Udt = DateTime.Now;
                        attrib.Cdt = DateTime.Now;

                        var log = new MaterialAttrLog
                        {
                            Editor = editor,
                            AttrName = name,
                            AttrOldValue = oldValue,
                            AttrNewValue = value,
                            MaterialCT = _MaterialCT,                            
                            Status = _Status,
                            Descr = descr,
                            Cdt=DateTime.Now
                        };

                        log.Tracker = _tracker.Merge(log.Tracker);
                        if (_materialAttrLogList == null)
                        {
                            _materialAttrLogList = new List<MaterialAttrLog>();
                        }
                        _materialAttrLogList.Add(log);
                        //_tracker.MarkAsModified(this);
                        _tracker.MarkAsModified(_subTrackerName);
                        return;
                    }
                }

                //add new record
                var attr = new MaterialAttr
                {
                    Editor = editor,
                    AttrName = name,
                    AttrValue = value,
                    MaterialCT = _MaterialCT,
                     Cdt =DateTime.Now,
                     Udt = DateTime.Now
                };

                 attr.Tracker = _tracker.Merge(attr.Tracker);
                _materialAttrList.Add(attr);
               // _tracker.MarkAsModified(this);
                _tracker.MarkAsModified(_subTrackerName);
            }
        }

        /// <summary>
        /// 鳳硌隅扽俶硉
        /// </summary>
        /// <param name="name">扽俶靡</param>
        /// <returns>扽俶硉</returns>
        public string GetAttributeValue(string name)
        {
            var values = (from p in _materialAttrList
                          where p.AttrName == name
                          select p.AttrValue).ToArray();
            if (values.Length > 0)
            {
                return values.First();
            }
            return null;
        }
        #endregion
    }
}
